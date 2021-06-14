using System;

namespace System.Windows.Markup
{
	// Token: 0x0200024C RID: 588
	internal class XamlElementEndNode : XamlNode
	{
		// Token: 0x060022E4 RID: 8932 RVA: 0x000AC55C File Offset: 0x000AA75C
		internal XamlElementEndNode(int lineNumber, int linePosition, int depth) : this(XamlNodeType.ElementEnd, lineNumber, linePosition, depth)
		{
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x000AC1E0 File Offset: 0x000AA3E0
		internal XamlElementEndNode(XamlNodeType tokenType, int lineNumber, int linePosition, int depth) : base(tokenType, lineNumber, linePosition, depth)
		{
		}
	}
}
