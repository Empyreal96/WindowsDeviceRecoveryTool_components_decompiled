using System;

namespace System.Windows.Markup
{
	// Token: 0x0200023F RID: 575
	internal class XamlDocumentStartNode : XamlNode
	{
		// Token: 0x060022A1 RID: 8865 RVA: 0x000AC078 File Offset: 0x000AA278
		internal XamlDocumentStartNode(int lineNumber, int linePosition, int depth) : base(XamlNodeType.DocumentStart, lineNumber, linePosition, depth)
		{
		}
	}
}
