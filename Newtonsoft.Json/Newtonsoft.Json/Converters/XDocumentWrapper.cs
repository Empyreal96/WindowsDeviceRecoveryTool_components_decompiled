using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000035 RID: 53
	internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000850E File Offset: 0x0000670E
		private XDocument Document
		{
			get
			{
				return (XDocument)base.WrappedNode;
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000851B File Offset: 0x0000671B
		public XDocumentWrapper(XDocument document) : base(document)
		{
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00008524 File Offset: 0x00006724
		public override IList<IXmlNode> ChildNodes
		{
			get
			{
				IList<IXmlNode> childNodes = base.ChildNodes;
				if (this.Document.Declaration != null && childNodes[0].NodeType != XmlNodeType.XmlDeclaration)
				{
					childNodes.Insert(0, new XDeclarationWrapper(this.Document.Declaration));
				}
				return childNodes;
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000856D File Offset: 0x0000676D
		public IXmlNode CreateComment(string text)
		{
			return new XObjectWrapper(new XComment(text));
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000857A File Offset: 0x0000677A
		public IXmlNode CreateTextNode(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00008587 File Offset: 0x00006787
		public IXmlNode CreateCDataSection(string data)
		{
			return new XObjectWrapper(new XCData(data));
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00008594 File Offset: 0x00006794
		public IXmlNode CreateWhitespace(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000085A1 File Offset: 0x000067A1
		public IXmlNode CreateSignificantWhitespace(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000085AE File Offset: 0x000067AE
		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000085BD File Offset: 0x000067BD
		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XDocumentTypeWrapper(new XDocumentType(name, publicId, systemId, internalSubset));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000085CE File Offset: 0x000067CE
		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000085DC File Offset: 0x000067DC
		public IXmlElement CreateElement(string elementName)
		{
			return new XElementWrapper(new XElement(elementName));
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000085F0 File Offset: 0x000067F0
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			string localName = MiscellaneousUtils.GetLocalName(qualifiedName);
			return new XElementWrapper(new XElement(XName.Get(localName, namespaceUri)));
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00008615 File Offset: 0x00006815
		public IXmlNode CreateAttribute(string name, string value)
		{
			return new XAttributeWrapper(new XAttribute(name, value));
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00008628 File Offset: 0x00006828
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			string localName = MiscellaneousUtils.GetLocalName(qualifiedName);
			return new XAttributeWrapper(new XAttribute(XName.Get(localName, namespaceUri), value));
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000864E File Offset: 0x0000684E
		public IXmlElement DocumentElement
		{
			get
			{
				if (this.Document.Root == null)
				{
					return null;
				}
				return new XElementWrapper(this.Document.Root);
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008670 File Offset: 0x00006870
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			XDeclarationWrapper xdeclarationWrapper = newChild as XDeclarationWrapper;
			if (xdeclarationWrapper != null)
			{
				this.Document.Declaration = xdeclarationWrapper.Declaration;
				return xdeclarationWrapper;
			}
			return base.AppendChild(newChild);
		}
	}
}
