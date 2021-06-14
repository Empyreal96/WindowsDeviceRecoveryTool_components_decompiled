using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics;
using Microsoft.Data.Edm.Csdl.Internal.Parsing;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl
{
	// Token: 0x020000AF RID: 175
	public class EdmxReader
	{
		// Token: 0x0600030C RID: 780 RVA: 0x00007CE0 File Offset: 0x00005EE0
		private EdmxReader(XmlReader reader)
		{
			this.reader = reader;
			this.errors = new List<EdmError>();
			this.csdlParser = new CsdlParser();
			this.edmxParserLookup = new Dictionary<string, Action>
			{
				{
					"DataServices",
					new Action(this.ParseDataServicesElement)
				},
				{
					"Runtime",
					new Action(this.ParseRuntimeElement)
				}
			};
			this.dataServicesParserLookup = new Dictionary<string, Action>
			{
				{
					"Schema",
					new Action(this.ParseCsdlSchemaElement)
				}
			};
			this.runtimeParserLookup = new Dictionary<string, Action>
			{
				{
					"ConceptualModels",
					new Action(this.ParseConceptualModelsElement)
				}
			};
			this.conceptualModelsParserLookup = new Dictionary<string, Action>
			{
				{
					"Schema",
					new Action(this.ParseCsdlSchemaElement)
				}
			};
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00007DB8 File Offset: 0x00005FB8
		public static bool TryParse(XmlReader reader, out IEdmModel model, out IEnumerable<EdmError> errors)
		{
			EdmxReader edmxReader = new EdmxReader(reader);
			return edmxReader.TryParse(Enumerable.Empty<IEdmModel>(), out model, out errors);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00007DDC File Offset: 0x00005FDC
		public static IEdmModel Parse(XmlReader reader)
		{
			IEdmModel result;
			IEnumerable<EdmError> parseErrors;
			if (!EdmxReader.TryParse(reader, out result, out parseErrors))
			{
				throw new EdmParseException(parseErrors);
			}
			return result;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00007E00 File Offset: 0x00006000
		public static bool TryParse(XmlReader reader, IEdmModel reference, out IEdmModel model, out IEnumerable<EdmError> errors)
		{
			EdmxReader edmxReader = new EdmxReader(reader);
			return edmxReader.TryParse(new IEdmModel[]
			{
				reference
			}, out model, out errors);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00007E28 File Offset: 0x00006028
		public static IEdmModel Parse(XmlReader reader, IEdmModel referencedModel)
		{
			IEdmModel result;
			IEnumerable<EdmError> parseErrors;
			if (!EdmxReader.TryParse(reader, referencedModel, out result, out parseErrors))
			{
				throw new EdmParseException(parseErrors);
			}
			return result;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00007E4C File Offset: 0x0000604C
		public static bool TryParse(XmlReader reader, IEnumerable<IEdmModel> references, out IEdmModel model, out IEnumerable<EdmError> errors)
		{
			EdmxReader edmxReader = new EdmxReader(reader);
			return edmxReader.TryParse(references, out model, out errors);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00007E6C File Offset: 0x0000606C
		public static IEdmModel Parse(XmlReader reader, IEnumerable<IEdmModel> referencedModels)
		{
			IEdmModel result;
			IEnumerable<EdmError> parseErrors;
			if (!EdmxReader.TryParse(reader, referencedModels, out result, out parseErrors))
			{
				throw new EdmParseException(parseErrors);
			}
			return result;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00007E90 File Offset: 0x00006090
		private static bool TryParseVersion(string input, out Version version)
		{
			version = null;
			if (string.IsNullOrEmpty(input))
			{
				return false;
			}
			input = input.Trim();
			string[] array = input.Split(new char[]
			{
				'.'
			});
			if (array.Length != 2)
			{
				return false;
			}
			int major;
			int minor;
			if (!int.TryParse(array[0], out major) || !int.TryParse(array[1], out minor))
			{
				return false;
			}
			version = new Version(major, minor);
			return true;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00007EF4 File Offset: 0x000060F4
		private bool TryParse(IEnumerable<IEdmModel> references, out IEdmModel model, out IEnumerable<EdmError> parsingErrors)
		{
			Version version;
			try
			{
				this.ParseEdmxFile(out version);
			}
			catch (XmlException ex)
			{
				model = null;
				parsingErrors = new EdmError[]
				{
					new EdmError(new CsdlLocation(ex.LineNumber, ex.LinePosition), EdmErrorCode.XmlError, ex.Message)
				};
				return false;
			}
			if (this.errors.Count == 0)
			{
				CsdlModel astModel;
				IEnumerable<EdmError> collection;
				if (this.csdlParser.GetResult(out astModel, out collection))
				{
					model = new CsdlSemanticsModel(astModel, new CsdlSemanticsDirectValueAnnotationsManager(), references);
					model.SetEdmxVersion(version);
					if (this.dataServiceVersion != null)
					{
						model.SetDataServiceVersion(this.dataServiceVersion);
					}
					if (this.maxDataServiceVersion != null)
					{
						model.SetMaxDataServiceVersion(this.maxDataServiceVersion);
					}
				}
				else
				{
					this.errors.AddRange(collection);
					model = null;
				}
			}
			else
			{
				model = null;
			}
			parsingErrors = this.errors;
			return this.errors.Count == 0;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00007FF0 File Offset: 0x000061F0
		private void ParseEdmxFile(out Version edmxVersion)
		{
			edmxVersion = null;
			if (this.reader.NodeType != XmlNodeType.Element)
			{
				while (this.reader.Read() && this.reader.NodeType != XmlNodeType.Element)
				{
				}
			}
			if (this.reader.EOF)
			{
				this.RaiseEmptyFile();
				return;
			}
			if (this.reader.LocalName != "Edmx" || !CsdlConstants.SupportedEdmxNamespaces.TryGetValue(this.reader.NamespaceURI, out edmxVersion))
			{
				this.RaiseError(EdmErrorCode.UnexpectedXmlElement, Strings.XmlParser_UnexpectedRootElement(this.reader.Name, "Edmx"));
				return;
			}
			this.ParseEdmxElement(edmxVersion);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00008094 File Offset: 0x00006294
		private void ParseElement(string elementName, Dictionary<string, Action> elementParsers)
		{
			if (this.reader.IsEmptyElement)
			{
				this.reader.Read();
				return;
			}
			this.reader.Read();
			while (this.reader.NodeType != XmlNodeType.EndElement)
			{
				if (this.reader.NodeType == XmlNodeType.Element)
				{
					if (elementParsers.ContainsKey(this.reader.LocalName))
					{
						elementParsers[this.reader.LocalName]();
					}
					else
					{
						this.ParseElement(this.reader.LocalName, EdmxReader.EmptyParserLookup);
					}
				}
				else if (!this.reader.Read())
				{
					break;
				}
			}
			this.reader.Read();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00008144 File Offset: 0x00006344
		private void ParseEdmxElement(Version edmxVersion)
		{
			string attributeValue = this.GetAttributeValue(null, "Version");
			Version v;
			if (attributeValue != null && (!EdmxReader.TryParseVersion(attributeValue, out v) || v != edmxVersion))
			{
				this.RaiseError(EdmErrorCode.InvalidVersionNumber, Strings.EdmxParser_EdmxVersionMismatch);
			}
			this.ParseElement("Edmx", this.edmxParserLookup);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00008194 File Offset: 0x00006394
		private string GetAttributeValue(string namespaceUri, string localName)
		{
			string namespaceURI = this.reader.NamespaceURI;
			string result = null;
			bool flag = this.reader.MoveToFirstAttribute();
			while (flag)
			{
				if (((namespaceUri != null && this.reader.NamespaceURI == namespaceUri) || string.IsNullOrEmpty(this.reader.NamespaceURI) || this.reader.NamespaceURI == namespaceURI) && this.reader.LocalName == localName)
				{
					result = this.reader.Value;
					break;
				}
				flag = this.reader.MoveToNextAttribute();
			}
			this.reader.MoveToElement();
			return result;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00008235 File Offset: 0x00006435
		private void ParseRuntimeElement()
		{
			this.ParseTargetElement("Runtime", this.runtimeParserLookup);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00008248 File Offset: 0x00006448
		private void ParseDataServicesElement()
		{
			string attributeValue = this.GetAttributeValue("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", "DataServiceVersion");
			if (attributeValue != null && !EdmxReader.TryParseVersion(attributeValue, out this.dataServiceVersion))
			{
				this.RaiseError(EdmErrorCode.InvalidVersionNumber, Strings.EdmxParser_EdmxDataServiceVersionInvalid);
			}
			string attributeValue2 = this.GetAttributeValue("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", "MaxDataServiceVersion");
			if (attributeValue2 != null && !EdmxReader.TryParseVersion(attributeValue2, out this.maxDataServiceVersion))
			{
				this.RaiseError(EdmErrorCode.InvalidVersionNumber, Strings.EdmxParser_EdmxMaxDataServiceVersionInvalid);
			}
			this.ParseTargetElement("DataServices", this.dataServicesParserLookup);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000082C4 File Offset: 0x000064C4
		private void ParseTargetElement(string elementName, Dictionary<string, Action> elementParsers)
		{
			if (!this.targetParsed)
			{
				this.targetParsed = true;
			}
			else
			{
				this.RaiseError(EdmErrorCode.UnexpectedXmlElement, Strings.EdmxParser_BodyElement("DataServices"));
				elementParsers = EdmxReader.EmptyParserLookup;
			}
			this.ParseElement(elementName, elementParsers);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000082F8 File Offset: 0x000064F8
		private void ParseConceptualModelsElement()
		{
			this.ParseElement("ConceptualModels", this.conceptualModelsParserLookup);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000830C File Offset: 0x0000650C
		private void ParseCsdlSchemaElement()
		{
			using (StringReader stringReader = new StringReader(this.reader.ReadOuterXml()))
			{
				using (XmlReader xmlReader = XmlReader.Create(stringReader))
				{
					this.csdlParser.AddReader(xmlReader);
				}
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00008374 File Offset: 0x00006574
		private void RaiseEmptyFile()
		{
			this.RaiseError(EdmErrorCode.EmptyFile, Strings.XmlParser_EmptySchemaTextReader);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00008384 File Offset: 0x00006584
		private CsdlLocation Location()
		{
			IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
			if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
			{
				return new CsdlLocation(xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
			}
			return new CsdlLocation(0, 0);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000083C1 File Offset: 0x000065C1
		private void RaiseError(EdmErrorCode errorCode, string errorMessage)
		{
			this.errors.Add(new EdmError(this.Location(), errorCode, errorMessage));
		}

		// Token: 0x04000158 RID: 344
		private static readonly Dictionary<string, Action> EmptyParserLookup = new Dictionary<string, Action>();

		// Token: 0x04000159 RID: 345
		private readonly Dictionary<string, Action> edmxParserLookup;

		// Token: 0x0400015A RID: 346
		private readonly Dictionary<string, Action> runtimeParserLookup;

		// Token: 0x0400015B RID: 347
		private readonly Dictionary<string, Action> conceptualModelsParserLookup;

		// Token: 0x0400015C RID: 348
		private readonly Dictionary<string, Action> dataServicesParserLookup;

		// Token: 0x0400015D RID: 349
		private readonly XmlReader reader;

		// Token: 0x0400015E RID: 350
		private readonly List<EdmError> errors;

		// Token: 0x0400015F RID: 351
		private readonly CsdlParser csdlParser;

		// Token: 0x04000160 RID: 352
		private Version dataServiceVersion;

		// Token: 0x04000161 RID: 353
		private Version maxDataServiceVersion;

		// Token: 0x04000162 RID: 354
		private bool targetParsed;
	}
}
