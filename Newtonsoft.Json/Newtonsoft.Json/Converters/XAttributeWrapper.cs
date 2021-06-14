using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000039 RID: 57
	internal class XAttributeWrapper : XObjectWrapper
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00008783 File Offset: 0x00006983
		private XAttribute Attribute
		{
			get
			{
				return (XAttribute)base.WrappedNode;
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00008790 File Offset: 0x00006990
		public XAttributeWrapper(XAttribute attribute) : base(attribute)
		{
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00008799 File Offset: 0x00006999
		// (set) Token: 0x06000227 RID: 551 RVA: 0x000087A6 File Offset: 0x000069A6
		public override string Value
		{
			get
			{
				return this.Attribute.Value;
			}
			set
			{
				this.Attribute.Value = value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000087B4 File Offset: 0x000069B4
		public override string LocalName
		{
			get
			{
				return this.Attribute.Name.LocalName;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000087C6 File Offset: 0x000069C6
		public override string NamespaceUri
		{
			get
			{
				return this.Attribute.Name.NamespaceName;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000087D8 File Offset: 0x000069D8
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Attribute.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Attribute.Parent);
			}
		}
	}
}
