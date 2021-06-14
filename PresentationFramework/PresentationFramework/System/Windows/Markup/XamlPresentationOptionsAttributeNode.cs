using System;

namespace System.Windows.Markup
{
	// Token: 0x0200025F RID: 607
	internal class XamlPresentationOptionsAttributeNode : XamlAttributeNode
	{
		// Token: 0x0600230A RID: 8970 RVA: 0x000AC7D7 File Offset: 0x000AA9D7
		internal XamlPresentationOptionsAttributeNode(int lineNumber, int linePosition, int depth, string name, string value) : base(XamlNodeType.PresentationOptionsAttribute, lineNumber, linePosition, depth, value)
		{
			this._name = name;
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x0600230B RID: 8971 RVA: 0x000AC7EE File Offset: 0x000AA9EE
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x04001A63 RID: 6755
		private string _name;
	}
}
