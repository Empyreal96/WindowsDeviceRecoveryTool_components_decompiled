using System;

namespace System.Windows.Markup
{
	// Token: 0x02000248 RID: 584
	internal class XamlUnknownAttributeNode : XamlAttributeNode
	{
		// Token: 0x060022D4 RID: 8916 RVA: 0x000AC42C File Offset: 0x000AA62C
		internal XamlUnknownAttributeNode(int lineNumber, int linePosition, int depth, string xmlNamespace, string name, string value, BamlAttributeUsage attributeUsage) : base(XamlNodeType.UnknownAttribute, lineNumber, linePosition, depth, value)
		{
			this._xmlNamespace = xmlNamespace;
			this._name = name;
			this._attributeUsage = attributeUsage;
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x060022D5 RID: 8917 RVA: 0x000AC453 File Offset: 0x000AA653
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x000AC45B File Offset: 0x000AA65B
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x04001A45 RID: 6725
		private string _xmlNamespace;

		// Token: 0x04001A46 RID: 6726
		private string _name;

		// Token: 0x04001A47 RID: 6727
		private BamlAttributeUsage _attributeUsage;
	}
}
