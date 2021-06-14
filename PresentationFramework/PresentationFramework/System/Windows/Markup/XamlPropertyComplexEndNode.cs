using System;

namespace System.Windows.Markup
{
	// Token: 0x02000244 RID: 580
	internal class XamlPropertyComplexEndNode : XamlNode
	{
		// Token: 0x060022B0 RID: 8880 RVA: 0x000AC1D4 File Offset: 0x000AA3D4
		internal XamlPropertyComplexEndNode(int lineNumber, int linePosition, int depth) : base(XamlNodeType.PropertyComplexEnd, lineNumber, linePosition, depth)
		{
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x000AC1E0 File Offset: 0x000AA3E0
		internal XamlPropertyComplexEndNode(XamlNodeType token, int lineNumber, int linePosition, int depth) : base(token, lineNumber, linePosition, depth)
		{
		}
	}
}
