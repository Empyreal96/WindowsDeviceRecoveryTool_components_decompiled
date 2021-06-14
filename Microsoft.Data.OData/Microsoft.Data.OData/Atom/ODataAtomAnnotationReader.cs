using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000E9 RID: 233
	internal sealed class ODataAtomAnnotationReader
	{
		// Token: 0x060005CC RID: 1484 RVA: 0x000142CC File Offset: 0x000124CC
		internal ODataAtomAnnotationReader(ODataAtomInputContext inputContext, ODataAtomPropertyAndValueDeserializer propertyAndValueDeserializer)
		{
			this.inputContext = inputContext;
			this.propertyAndValueDeserializer = propertyAndValueDeserializer;
			BufferingXmlReader xmlReader = this.inputContext.XmlReader;
			xmlReader.NameTable.Add("target");
			xmlReader.NameTable.Add("term");
			xmlReader.NameTable.Add("type");
			xmlReader.NameTable.Add("null");
			xmlReader.NameTable.Add("string");
			xmlReader.NameTable.Add("bool");
			xmlReader.NameTable.Add("decimal");
			xmlReader.NameTable.Add("int");
			xmlReader.NameTable.Add("float");
			this.odataMetadataNamespace = xmlReader.NameTable.Add("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			this.attributeElementName = xmlReader.NameTable.Add("annotation");
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000143C0 File Offset: 0x000125C0
		internal bool TryReadAnnotation(out AtomInstanceAnnotation annotation)
		{
			BufferingXmlReader xmlReader = this.inputContext.XmlReader;
			annotation = null;
			if (this.propertyAndValueDeserializer.MessageReaderSettings.ShouldIncludeAnnotation != null && xmlReader.NamespaceEquals(this.odataMetadataNamespace) && xmlReader.LocalNameEquals(this.attributeElementName))
			{
				annotation = AtomInstanceAnnotation.CreateFrom(this.inputContext, this.propertyAndValueDeserializer);
			}
			return annotation != null;
		}

		// Token: 0x04000264 RID: 612
		private readonly ODataAtomInputContext inputContext;

		// Token: 0x04000265 RID: 613
		private readonly string odataMetadataNamespace;

		// Token: 0x04000266 RID: 614
		private readonly string attributeElementName;

		// Token: 0x04000267 RID: 615
		private readonly ODataAtomPropertyAndValueDeserializer propertyAndValueDeserializer;
	}
}
