using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200003A RID: 58
	internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000087F9 File Offset: 0x000069F9
		private XElement Element
		{
			get
			{
				return (XElement)base.WrappedNode;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00008806 File Offset: 0x00006A06
		public XElementWrapper(XElement element) : base(element)
		{
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00008810 File Offset: 0x00006A10
		public void SetAttributeNode(IXmlNode attribute)
		{
			XObjectWrapper xobjectWrapper = (XObjectWrapper)attribute;
			this.Element.Add(xobjectWrapper.WrappedNode);
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000883D File Offset: 0x00006A3D
		public override IList<IXmlNode> Attributes
		{
			get
			{
				return (from a in this.Element.Attributes()
				select new XAttributeWrapper(a)).Cast<IXmlNode>().ToList<IXmlNode>();
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00008876 File Offset: 0x00006A76
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00008883 File Offset: 0x00006A83
		public override string Value
		{
			get
			{
				return this.Element.Value;
			}
			set
			{
				this.Element.Value = value;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00008891 File Offset: 0x00006A91
		public override string LocalName
		{
			get
			{
				return this.Element.Name.LocalName;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000232 RID: 562 RVA: 0x000088A3 File Offset: 0x00006AA3
		public override string NamespaceUri
		{
			get
			{
				return this.Element.Name.NamespaceName;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000088B5 File Offset: 0x00006AB5
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this.Element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000234 RID: 564 RVA: 0x000088C8 File Offset: 0x00006AC8
		public bool IsEmpty
		{
			get
			{
				return this.Element.IsEmpty;
			}
		}
	}
}
