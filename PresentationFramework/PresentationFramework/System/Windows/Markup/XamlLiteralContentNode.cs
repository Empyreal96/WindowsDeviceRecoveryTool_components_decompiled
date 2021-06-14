using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x0200024D RID: 589
	[DebuggerDisplay("Cont:{_content}")]
	internal class XamlLiteralContentNode : XamlNode
	{
		// Token: 0x060022E6 RID: 8934 RVA: 0x000AC568 File Offset: 0x000AA768
		internal XamlLiteralContentNode(int lineNumber, int linePosition, int depth, string content) : base(XamlNodeType.LiteralContent, lineNumber, linePosition, depth)
		{
			this._content = content;
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x000AC57D File Offset: 0x000AA77D
		internal string Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x04001A50 RID: 6736
		private string _content;
	}
}
