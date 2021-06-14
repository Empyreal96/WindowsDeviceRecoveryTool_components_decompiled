using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000028 RID: 40
	internal class XmlNodeWrapper : IXmlNode
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00007F06 File Offset: 0x00006106
		public XmlNodeWrapper(XmlNode node)
		{
			this._node = node;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00007F15 File Offset: 0x00006115
		public object WrappedNode
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00007F1D File Offset: 0x0000611D
		public XmlNodeType NodeType
		{
			get
			{
				return this._node.NodeType;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00007F2A File Offset: 0x0000612A
		public virtual string LocalName
		{
			get
			{
				return this._node.LocalName;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007F37 File Offset: 0x00006137
		public IList<IXmlNode> ChildNodes
		{
			get
			{
				if (this._childNodes == null)
				{
					this._childNodes = this._node.ChildNodes.Cast<XmlNode>().Select(new Func<XmlNode, IXmlNode>(XmlNodeWrapper.WrapNode)).ToList<IXmlNode>();
				}
				return this._childNodes;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007F74 File Offset: 0x00006174
		internal static IXmlNode WrapNode(XmlNode node)
		{
			XmlNodeType nodeType = node.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				return new XmlElementWrapper((XmlElement)node);
			}
			if (nodeType == XmlNodeType.DocumentType)
			{
				return new XmlDocumentTypeWrapper((XmlDocumentType)node);
			}
			if (nodeType != XmlNodeType.XmlDeclaration)
			{
				return new XmlNodeWrapper(node);
			}
			return new XmlDeclarationWrapper((XmlDeclaration)node);
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00007FC2 File Offset: 0x000061C2
		public IList<IXmlNode> Attributes
		{
			get
			{
				if (this._node.Attributes == null)
				{
					return null;
				}
				return this._node.Attributes.Cast<XmlAttribute>().Select(new Func<XmlAttribute, IXmlNode>(XmlNodeWrapper.WrapNode)).ToList<IXmlNode>();
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007FFC File Offset: 0x000061FC
		public IXmlNode ParentNode
		{
			get
			{
				XmlNode xmlNode = (this._node is XmlAttribute) ? ((XmlAttribute)this._node).OwnerElement : this._node.ParentNode;
				if (xmlNode == null)
				{
					return null;
				}
				return XmlNodeWrapper.WrapNode(xmlNode);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000803F File Offset: 0x0000623F
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0000804C File Offset: 0x0000624C
		public string Value
		{
			get
			{
				return this._node.Value;
			}
			set
			{
				this._node.Value = value;
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000805C File Offset: 0x0000625C
		public IXmlNode AppendChild(IXmlNode newChild)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
			this._node.AppendChild(xmlNodeWrapper._node);
			this._childNodes = null;
			return newChild;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000808A File Offset: 0x0000628A
		public string NamespaceUri
		{
			get
			{
				return this._node.NamespaceURI;
			}
		}

		// Token: 0x0400009E RID: 158
		private readonly XmlNode _node;

		// Token: 0x0400009F RID: 159
		private IList<IXmlNode> _childNodes;
	}
}
