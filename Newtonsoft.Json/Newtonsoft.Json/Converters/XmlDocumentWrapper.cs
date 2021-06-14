using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002A RID: 42
	internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x060001BA RID: 442 RVA: 0x00008097 File Offset: 0x00006297
		public XmlDocumentWrapper(XmlDocument document) : base(document)
		{
			this._document = document;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000080A7 File Offset: 0x000062A7
		public IXmlNode CreateComment(string data)
		{
			return new XmlNodeWrapper(this._document.CreateComment(data));
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000080BA File Offset: 0x000062BA
		public IXmlNode CreateTextNode(string text)
		{
			return new XmlNodeWrapper(this._document.CreateTextNode(text));
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000080CD File Offset: 0x000062CD
		public IXmlNode CreateCDataSection(string data)
		{
			return new XmlNodeWrapper(this._document.CreateCDataSection(data));
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000080E0 File Offset: 0x000062E0
		public IXmlNode CreateWhitespace(string text)
		{
			return new XmlNodeWrapper(this._document.CreateWhitespace(text));
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000080F3 File Offset: 0x000062F3
		public IXmlNode CreateSignificantWhitespace(string text)
		{
			return new XmlNodeWrapper(this._document.CreateSignificantWhitespace(text));
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008106 File Offset: 0x00006306
		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XmlDeclarationWrapper(this._document.CreateXmlDeclaration(version, encoding, standalone));
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000811B File Offset: 0x0000631B
		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XmlDocumentTypeWrapper(this._document.CreateDocumentType(name, publicId, systemId, null));
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008131 File Offset: 0x00006331
		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XmlNodeWrapper(this._document.CreateProcessingInstruction(target, data));
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008145 File Offset: 0x00006345
		public IXmlElement CreateElement(string elementName)
		{
			return new XmlElementWrapper(this._document.CreateElement(elementName));
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008158 File Offset: 0x00006358
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XmlElementWrapper(this._document.CreateElement(qualifiedName, namespaceUri));
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000816C File Offset: 0x0000636C
		public IXmlNode CreateAttribute(string name, string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(name))
			{
				Value = value
			};
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008194 File Offset: 0x00006394
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(qualifiedName, namespaceUri))
			{
				Value = value
			};
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x000081BC File Offset: 0x000063BC
		public IXmlElement DocumentElement
		{
			get
			{
				if (this._document.DocumentElement == null)
				{
					return null;
				}
				return new XmlElementWrapper(this._document.DocumentElement);
			}
		}

		// Token: 0x040000A0 RID: 160
		private readonly XmlDocument _document;
	}
}
