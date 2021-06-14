using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000029 RID: 41
	internal interface IXmlDocument : IXmlNode
	{
		// Token: 0x060001AD RID: 429
		IXmlNode CreateComment(string text);

		// Token: 0x060001AE RID: 430
		IXmlNode CreateTextNode(string text);

		// Token: 0x060001AF RID: 431
		IXmlNode CreateCDataSection(string data);

		// Token: 0x060001B0 RID: 432
		IXmlNode CreateWhitespace(string text);

		// Token: 0x060001B1 RID: 433
		IXmlNode CreateSignificantWhitespace(string text);

		// Token: 0x060001B2 RID: 434
		IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);

		// Token: 0x060001B3 RID: 435
		IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset);

		// Token: 0x060001B4 RID: 436
		IXmlNode CreateProcessingInstruction(string target, string data);

		// Token: 0x060001B5 RID: 437
		IXmlElement CreateElement(string elementName);

		// Token: 0x060001B6 RID: 438
		IXmlElement CreateElement(string qualifiedName, string namespaceUri);

		// Token: 0x060001B7 RID: 439
		IXmlNode CreateAttribute(string name, string value);

		// Token: 0x060001B8 RID: 440
		IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value);

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001B9 RID: 441
		IXmlElement DocumentElement { get; }
	}
}
