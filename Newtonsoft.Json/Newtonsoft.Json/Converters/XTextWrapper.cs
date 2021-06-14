using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000036 RID: 54
	internal class XTextWrapper : XObjectWrapper
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000215 RID: 533 RVA: 0x000086A1 File Offset: 0x000068A1
		private XText Text
		{
			get
			{
				return (XText)base.WrappedNode;
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000086AE File Offset: 0x000068AE
		public XTextWrapper(XText text) : base(text)
		{
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000217 RID: 535 RVA: 0x000086B7 File Offset: 0x000068B7
		// (set) Token: 0x06000218 RID: 536 RVA: 0x000086C4 File Offset: 0x000068C4
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

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000219 RID: 537 RVA: 0x000086D2 File Offset: 0x000068D2
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
