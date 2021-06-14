using System;

namespace System.Windows.Markup
{
	// Token: 0x0200025B RID: 603
	internal class XamlEndAttributesNode : XamlNode
	{
		// Token: 0x06002300 RID: 8960 RVA: 0x000AC72F File Offset: 0x000AA92F
		internal XamlEndAttributesNode(int lineNumber, int linePosition, int depth, bool compact) : base(XamlNodeType.EndAttributes, lineNumber, linePosition, depth)
		{
			this._compact = compact;
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002301 RID: 8961 RVA: 0x000AC744 File Offset: 0x000AA944
		internal bool IsCompact
		{
			get
			{
				return this._compact;
			}
		}

		// Token: 0x04001A5E RID: 6750
		private bool _compact;
	}
}
