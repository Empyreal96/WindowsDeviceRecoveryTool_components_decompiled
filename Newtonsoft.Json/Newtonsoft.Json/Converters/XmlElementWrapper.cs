using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002C RID: 44
	internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x060001CB RID: 459 RVA: 0x000081DD File Offset: 0x000063DD
		public XmlElementWrapper(XmlElement element) : base(element)
		{
			this._element = element;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000081F0 File Offset: 0x000063F0
		public void SetAttributeNode(IXmlNode attribute)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)attribute;
			this._element.SetAttributeNode((XmlAttribute)xmlNodeWrapper.WrappedNode);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000821B File Offset: 0x0000641B
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this._element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00008229 File Offset: 0x00006429
		public bool IsEmpty
		{
			get
			{
				return this._element.IsEmpty;
			}
		}

		// Token: 0x040000A1 RID: 161
		private readonly XmlElement _element;
	}
}
