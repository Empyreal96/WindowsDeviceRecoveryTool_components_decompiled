using System;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000E7 RID: 231
	internal sealed class AtomInstanceAnnotation
	{
		// Token: 0x0600059F RID: 1439 RVA: 0x00013CE4 File Offset: 0x00011EE4
		private AtomInstanceAnnotation(string target, string term, ODataValue value)
		{
			this.target = target;
			this.term = term;
			this.value = value;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00013D01 File Offset: 0x00011F01
		internal string Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00013D09 File Offset: 0x00011F09
		internal string TermName
		{
			get
			{
				return this.term;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00013D11 File Offset: 0x00011F11
		internal ODataValue Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00013D19 File Offset: 0x00011F19
		internal bool IsTargetingCurrentElement
		{
			get
			{
				return string.IsNullOrEmpty(this.Target) || string.Equals(this.Target, ".", StringComparison.Ordinal);
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00013D3B File Offset: 0x00011F3B
		internal static AtomInstanceAnnotation CreateFrom(ODataInstanceAnnotation odataInstanceAnnotation, string target)
		{
			return new AtomInstanceAnnotation(target, odataInstanceAnnotation.Name, odataInstanceAnnotation.Value);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00013D50 File Offset: 0x00011F50
		internal static AtomInstanceAnnotation CreateFrom(ODataAtomInputContext inputContext, ODataAtomPropertyAndValueDeserializer propertyAndValueDeserializer)
		{
			BufferingXmlReader xmlReader = inputContext.XmlReader;
			string text = null;
			string text2 = null;
			string typeAttributeValue = null;
			bool flag = false;
			bool flag2 = false;
			string attributeValueNotationAttributeName = null;
			string attributeValueNotationAttributeValue = null;
			IEdmPrimitiveTypeReference edmPrimitiveTypeReference = null;
			XmlNameTable nameTable = xmlReader.NameTable;
			string namespaceUri = nameTable.Get("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			string localName = nameTable.Get("null");
			string localName2 = nameTable.Get("type");
			string namespaceUri2 = nameTable.Get(string.Empty);
			string localName3 = nameTable.Get("term");
			string localName4 = nameTable.Get("target");
			while (xmlReader.MoveToNextAttribute())
			{
				if (xmlReader.NamespaceEquals(namespaceUri))
				{
					if (xmlReader.LocalNameEquals(localName2))
					{
						typeAttributeValue = xmlReader.Value;
					}
					else if (xmlReader.LocalNameEquals(localName))
					{
						flag = ODataAtomReaderUtils.ReadMetadataNullAttributeValue(xmlReader.Value);
					}
				}
				else if (xmlReader.NamespaceEquals(namespaceUri2))
				{
					if (xmlReader.LocalNameEquals(localName3))
					{
						text = xmlReader.Value;
						if (propertyAndValueDeserializer.MessageReaderSettings.ShouldSkipAnnotation(text))
						{
							xmlReader.MoveToElement();
							return null;
						}
					}
					else if (xmlReader.LocalNameEquals(localName4))
					{
						text2 = xmlReader.Value;
					}
					else
					{
						IEdmPrimitiveTypeReference edmPrimitiveTypeReference2 = AtomInstanceAnnotation.LookupEdmTypeByAttributeValueNotationName(xmlReader.LocalName);
						if (edmPrimitiveTypeReference2 != null)
						{
							if (edmPrimitiveTypeReference != null)
							{
								flag2 = true;
							}
							edmPrimitiveTypeReference = edmPrimitiveTypeReference2;
							attributeValueNotationAttributeName = xmlReader.LocalName;
							attributeValueNotationAttributeValue = xmlReader.Value;
						}
					}
				}
			}
			xmlReader.MoveToElement();
			if (text == null)
			{
				throw new ODataException(Strings.AtomInstanceAnnotation_MissingTermAttributeOnAnnotationElement);
			}
			if (flag2)
			{
				throw new ODataException(Strings.AtomInstanceAnnotation_MultipleAttributeValueNotationAttributes);
			}
			IEdmTypeReference edmTypeReference = MetadataUtils.LookupTypeOfValueTerm(text, propertyAndValueDeserializer.Model);
			ODataValue odataValue;
			if (flag)
			{
				ReaderValidationUtils.ValidateNullValue(propertyAndValueDeserializer.Model, edmTypeReference, propertyAndValueDeserializer.MessageReaderSettings, true, propertyAndValueDeserializer.Version, text);
				odataValue = new ODataNullValue();
			}
			else if (edmPrimitiveTypeReference != null)
			{
				odataValue = AtomInstanceAnnotation.GetValueFromAttributeValueNotation(edmTypeReference, edmPrimitiveTypeReference, attributeValueNotationAttributeName, attributeValueNotationAttributeValue, typeAttributeValue, xmlReader.IsEmptyElement, propertyAndValueDeserializer.Model, propertyAndValueDeserializer.MessageReaderSettings, propertyAndValueDeserializer.Version);
			}
			else
			{
				odataValue = AtomInstanceAnnotation.ReadValueFromElementContent(propertyAndValueDeserializer, edmTypeReference);
			}
			xmlReader.Read();
			return new AtomInstanceAnnotation(text2, text, odataValue);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00013F34 File Offset: 0x00012134
		internal static string LookupAttributeValueNotationNameByEdmTypeKind(EdmPrimitiveTypeKind typeKind)
		{
			if (typeKind != EdmPrimitiveTypeKind.Boolean)
			{
				switch (typeKind)
				{
				case EdmPrimitiveTypeKind.Decimal:
					return "decimal";
				case EdmPrimitiveTypeKind.Double:
					return "float";
				case EdmPrimitiveTypeKind.Guid:
				case EdmPrimitiveTypeKind.Int16:
					break;
				case EdmPrimitiveTypeKind.Int32:
					return "int";
				default:
					if (typeKind == EdmPrimitiveTypeKind.String)
					{
						return "string";
					}
					break;
				}
				return null;
			}
			return "bool";
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00013F8C File Offset: 0x0001218C
		internal static IEdmPrimitiveTypeReference LookupEdmTypeByAttributeValueNotationName(string attributeName)
		{
			if (attributeName != null)
			{
				if (attributeName == "int")
				{
					return EdmCoreModel.Instance.GetInt32(false);
				}
				if (attributeName == "string")
				{
					return EdmCoreModel.Instance.GetString(false);
				}
				if (attributeName == "float")
				{
					return EdmCoreModel.Instance.GetDouble(false);
				}
				if (attributeName == "bool")
				{
					return EdmCoreModel.Instance.GetBoolean(false);
				}
				if (attributeName == "decimal")
				{
					return EdmCoreModel.Instance.GetDecimal(false);
				}
			}
			return null;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00014020 File Offset: 0x00012220
		private static ODataValue ReadValueFromElementContent(ODataAtomPropertyAndValueDeserializer propertyAndValueDeserializer, IEdmTypeReference expectedType)
		{
			object objectToConvert = propertyAndValueDeserializer.ReadNonEntityValue(expectedType, null, null, true, false);
			return objectToConvert.ToODataValue();
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00014040 File Offset: 0x00012240
		private static ODataPrimitiveValue GetValueFromAttributeValueNotation(IEdmTypeReference expectedTypeReference, IEdmPrimitiveTypeReference attributeValueNotationTypeReference, string attributeValueNotationAttributeName, string attributeValueNotationAttributeValue, string typeAttributeValue, bool positionedOnEmptyElement, IEdmModel model, ODataMessageReaderSettings messageReaderSettings, ODataVersion version)
		{
			if (!positionedOnEmptyElement)
			{
				throw new ODataException(Strings.AtomInstanceAnnotation_AttributeValueNotationUsedOnNonEmptyElement(attributeValueNotationAttributeName));
			}
			if (typeAttributeValue != null && !string.Equals(attributeValueNotationTypeReference.Definition.ODataFullName(), typeAttributeValue, StringComparison.Ordinal))
			{
				throw new ODataException(Strings.AtomInstanceAnnotation_AttributeValueNotationUsedWithIncompatibleType(typeAttributeValue, attributeValueNotationAttributeName));
			}
			IEdmTypeReference type = ReaderValidationUtils.ResolveAndValidatePrimitiveTargetType(expectedTypeReference, EdmTypeKind.Primitive, attributeValueNotationTypeReference.Definition, attributeValueNotationTypeReference.ODataFullName(), attributeValueNotationTypeReference.Definition, model, messageReaderSettings, version);
			return new ODataPrimitiveValue(AtomValueUtils.ConvertStringToPrimitive(attributeValueNotationAttributeValue, type.AsPrimitive()));
		}

		// Token: 0x0400025F RID: 607
		private readonly string target;

		// Token: 0x04000260 RID: 608
		private readonly string term;

		// Token: 0x04000261 RID: 609
		private readonly ODataValue value;
	}
}
