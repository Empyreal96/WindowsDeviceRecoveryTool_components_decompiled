using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000037 RID: 55
	internal class XCommentWrapper : XObjectWrapper
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600021A RID: 538 RVA: 0x000086F3 File Offset: 0x000068F3
		private XComment Text
		{
			get
			{
				return (XComment)base.WrappedNode;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00008700 File Offset: 0x00006900
		public XCommentWrapper(XComment text) : base(text)
		{
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00008709 File Offset: 0x00006909
		// (set) Token: 0x0600021D RID: 541 RVA: 0x00008716 File Offset: 0x00006916
		public override string Value
		{
			get
			{
				return this.Text.Value;
			}
			set
			{
				this.Text.Value = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00008724 File Offset: 0x00006924
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Text.Parent);
			}
		}
	}
}
