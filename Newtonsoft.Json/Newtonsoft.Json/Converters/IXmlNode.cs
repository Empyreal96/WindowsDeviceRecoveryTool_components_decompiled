using System;
using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000027 RID: 39
	internal interface IXmlNode
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000197 RID: 407
		XmlNodeType NodeType { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000198 RID: 408
		string LocalName { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000199 RID: 409
		IList<IXmlNode> ChildNodes { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600019A RID: 410
		IList<IXmlNode> Attributes { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600019B RID: 411
		IXmlNode ParentNode { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600019C RID: 412
		// (set) Token: 0x0600019D RID: 413
		string Value { get; set; }

		// Token: 0x0600019E RID: 414
		IXmlNode AppendChild(IXmlNode newChild);

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600019F RID: 415
		string NamespaceUri { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001A0 RID: 416
		object WrappedNode { get; }
	}
}
