using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml;
using System.Xml.Schema;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200034A RID: 842
	internal sealed class XpsDocStructSchema : XpsSchema
	{
		// Token: 0x06002D0E RID: 11534 RVA: 0x000CB61C File Offset: 0x000C981C
		public XpsDocStructSchema()
		{
			XpsSchema.RegisterSchema(this, new ContentType[]
			{
				XpsDocStructSchema._documentStructureContentType,
				XpsDocStructSchema._storyFragmentsContentType
			});
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000CB640 File Offset: 0x000C9840
		public override XmlReaderSettings GetXmlReaderSettings()
		{
			if (XpsDocStructSchema._xmlReaderSettings == null)
			{
				XpsDocStructSchema._xmlReaderSettings = new XmlReaderSettings();
				XpsDocStructSchema._xmlReaderSettings.ValidationFlags = (XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints);
				MemoryStream input = new MemoryStream(XpsDocStructSchema.SchemaBytes);
				XmlResolver xmlResolver = new XmlUrlResolver();
				XpsDocStructSchema._xmlReaderSettings.ValidationType = ValidationType.Schema;
				XpsDocStructSchema._xmlReaderSettings.Schemas.XmlResolver = xmlResolver;
				XpsDocStructSchema._xmlReaderSettings.Schemas.Add("http://schemas.microsoft.com/xps/2005/06/documentstructure", new XmlTextReader(input));
			}
			return XpsDocStructSchema._xmlReaderSettings;
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000CB6B6 File Offset: 0x000C98B6
		public override bool IsValidRootNamespaceUri(string namespaceUri)
		{
			return namespaceUri.Equals("http://schemas.microsoft.com/xps/2005/06/documentstructure", StringComparison.Ordinal);
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06002D11 RID: 11537 RVA: 0x000CB6C4 File Offset: 0x000C98C4
		public override string RootNamespaceUri
		{
			get
			{
				return "http://schemas.microsoft.com/xps/2005/06/documentstructure";
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06002D12 RID: 11538 RVA: 0x000CB6CC File Offset: 0x000C98CC
		private static byte[] SchemaBytes
		{
			get
			{
				ResourceManager resourceManager = new ResourceManager("Schemas_DocStructure", Assembly.GetAssembly(typeof(XpsDocStructSchema)));
				return (byte[])resourceManager.GetObject("DocStructure.xsd");
			}
		}

		// Token: 0x04001D64 RID: 7524
		private static ContentType _documentStructureContentType = new ContentType("application/vnd.ms-package.xps-documentstructure+xml");

		// Token: 0x04001D65 RID: 7525
		private static ContentType _storyFragmentsContentType = new ContentType("application/vnd.ms-package.xps-storyfragments+xml");

		// Token: 0x04001D66 RID: 7526
		private const string _xpsDocStructureSchemaNamespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure";

		// Token: 0x04001D67 RID: 7527
		private static XmlReaderSettings _xmlReaderSettings;
	}
}
