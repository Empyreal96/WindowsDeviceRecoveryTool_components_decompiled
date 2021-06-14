using System;

namespace System.Windows.Markup
{
	// Token: 0x02000240 RID: 576
	internal class XamlDocumentEndNode : XamlNode
	{
		// Token: 0x060022A2 RID: 8866 RVA: 0x000AC084 File Offset: 0x000AA284
		internal XamlDocumentEndNode(int lineNumber, int linePosition, int depth) : base(XamlNodeType.DocumentEnd, lineNumber, linePosition, depth)
		{
		}
	}
}
