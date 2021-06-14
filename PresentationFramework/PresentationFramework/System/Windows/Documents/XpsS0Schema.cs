using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml;
using System.Xml.Schema;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000347 RID: 839
	internal class XpsS0Schema : XpsSchema
	{
		// Token: 0x06002CFF RID: 11519 RVA: 0x000CAE3C File Offset: 0x000C903C
		protected XpsS0Schema()
		{
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000CAE44 File Offset: 0x000C9044
		public override XmlReaderSettings GetXmlReaderSettings()
		{
			if (XpsS0Schema._xmlReaderSettings == null)
			{
				XpsS0Schema._xmlReaderSettings = new XmlReaderSettings();
				XpsS0Schema._xmlReaderSettings.ValidationFlags = (XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints);
				MemoryStream input = new MemoryStream(XpsS0Schema.S0SchemaBytes);
				MemoryStream input2 = new MemoryStream(XpsS0Schema.DictionarySchemaBytes);
				XmlResolver xmlResolver = new XmlUrlResolver();
				XpsS0Schema._xmlReaderSettings.ValidationType = ValidationType.Schema;
				XpsS0Schema._xmlReaderSettings.Schemas.XmlResolver = xmlResolver;
				XpsS0Schema._xmlReaderSettings.Schemas.Add("http://schemas.microsoft.com/xps/2005/06", new XmlTextReader(input));
				XpsS0Schema._xmlReaderSettings.Schemas.Add(null, new XmlTextReader(input2));
			}
			return XpsS0Schema._xmlReaderSettings;
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x000CAEDC File Offset: 0x000C90DC
		public override bool HasRequiredResources(ContentType mimeType)
		{
			return XpsS0Schema._fixedPageContentType.AreTypeAndSubTypeEqual(mimeType);
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x00016748 File Offset: 0x00014948
		public override bool HasUriAttributes(ContentType mimeType)
		{
			return true;
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x000CAEEE File Offset: 0x000C90EE
		public override bool AllowsMultipleReferencesToSameUri(ContentType mimeType)
		{
			return !XpsS0Schema._fixedDocumentSequenceContentType.AreTypeAndSubTypeEqual(mimeType) && !XpsS0Schema._fixedDocumentContentType.AreTypeAndSubTypeEqual(mimeType);
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x000CAF0D File Offset: 0x000C910D
		public override bool IsValidRootNamespaceUri(string namespaceUri)
		{
			return namespaceUri.Equals("http://schemas.microsoft.com/xps/2005/06", StringComparison.Ordinal);
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06002D05 RID: 11525 RVA: 0x000CAF1B File Offset: 0x000C911B
		public override string RootNamespaceUri
		{
			get
			{
				return "http://schemas.microsoft.com/xps/2005/06";
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000CAF24 File Offset: 0x000C9124
		public override string[] ExtractUriFromAttr(string attrName, string attrValue)
		{
			if (attrName.Equals("Source", StringComparison.Ordinal) || attrName.Equals("FontUri", StringComparison.Ordinal))
			{
				return new string[]
				{
					attrValue
				};
			}
			if (!attrName.Equals("ImageSource", StringComparison.Ordinal))
			{
				if (attrName.Equals("Color", StringComparison.Ordinal) || attrName.Equals("Fill", StringComparison.Ordinal) || attrName.Equals("Stroke", StringComparison.Ordinal))
				{
					attrValue = attrValue.Trim();
					if (attrValue.StartsWith("ContextColor ", StringComparison.Ordinal))
					{
						attrValue = attrValue.Substring("ContextColor ".Length);
						attrValue = attrValue.Trim();
						string[] array = attrValue.Split(new char[]
						{
							' '
						});
						if (array.GetLength(0) >= 1)
						{
							return new string[]
							{
								array[0]
							};
						}
					}
				}
				return null;
			}
			if (attrValue.StartsWith("{ColorConvertedBitmap ", StringComparison.Ordinal))
			{
				attrValue = attrValue.Substring("{ColorConvertedBitmap ".Length);
				return attrValue.Split(new char[]
				{
					' ',
					'}'
				});
			}
			return new string[]
			{
				attrValue
			};
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06002D07 RID: 11527 RVA: 0x000CB030 File Offset: 0x000C9230
		private static byte[] S0SchemaBytes
		{
			get
			{
				ResourceManager resourceManager = new ResourceManager("Schemas_S0", Assembly.GetAssembly(typeof(XpsS0Schema)));
				return (byte[])resourceManager.GetObject("s0schema.xsd");
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06002D08 RID: 11528 RVA: 0x000CB068 File Offset: 0x000C9268
		private static byte[] DictionarySchemaBytes
		{
			get
			{
				ResourceManager resourceManager = new ResourceManager("Schemas_S0", Assembly.GetAssembly(typeof(XpsS0Schema)));
				return (byte[])resourceManager.GetObject("rdkey.xsd");
			}
		}

		// Token: 0x04001D4F RID: 7503
		protected static ContentType _fontContentType = new ContentType("application/vnd.ms-opentype");

		// Token: 0x04001D50 RID: 7504
		protected static ContentType _colorContextContentType = new ContentType("application/vnd.ms-color.iccprofile");

		// Token: 0x04001D51 RID: 7505
		protected static ContentType _obfuscatedContentType = new ContentType("application/vnd.ms-package.obfuscated-opentype");

		// Token: 0x04001D52 RID: 7506
		protected static ContentType _jpgContentType = new ContentType("image/jpeg");

		// Token: 0x04001D53 RID: 7507
		protected static ContentType _pngContentType = new ContentType("image/png");

		// Token: 0x04001D54 RID: 7508
		protected static ContentType _tifContentType = new ContentType("image/tiff");

		// Token: 0x04001D55 RID: 7509
		protected static ContentType _wmpContentType = new ContentType("image/vnd.ms-photo");

		// Token: 0x04001D56 RID: 7510
		protected static ContentType _fixedDocumentSequenceContentType = new ContentType("application/vnd.ms-package.xps-fixeddocumentsequence+xml");

		// Token: 0x04001D57 RID: 7511
		protected static ContentType _fixedDocumentContentType = new ContentType("application/vnd.ms-package.xps-fixeddocument+xml");

		// Token: 0x04001D58 RID: 7512
		protected static ContentType _fixedPageContentType = new ContentType("application/vnd.ms-package.xps-fixedpage+xml");

		// Token: 0x04001D59 RID: 7513
		protected static ContentType _resourceDictionaryContentType = new ContentType("application/vnd.ms-package.xps-resourcedictionary+xml");

		// Token: 0x04001D5A RID: 7514
		protected static ContentType _printTicketContentType = new ContentType("application/vnd.ms-printing.printticket+xml");

		// Token: 0x04001D5B RID: 7515
		protected static ContentType _discardControlContentType = new ContentType("application/vnd.ms-package.xps-discard-control+xml");

		// Token: 0x04001D5C RID: 7516
		private const string _xpsS0SchemaNamespace = "http://schemas.microsoft.com/xps/2005/06";

		// Token: 0x04001D5D RID: 7517
		private const string _contextColor = "ContextColor ";

		// Token: 0x04001D5E RID: 7518
		private const string _colorConvertedBitmap = "{ColorConvertedBitmap ";

		// Token: 0x04001D5F RID: 7519
		private static XmlReaderSettings _xmlReaderSettings;
	}
}
