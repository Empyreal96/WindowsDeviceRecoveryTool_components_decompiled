using System;

namespace System.Windows.Markup
{
	// Token: 0x02000260 RID: 608
	internal class XamlKeyElementStartNode : XamlElementStartNode
	{
		// Token: 0x0600230C RID: 8972 RVA: 0x000AC7F8 File Offset: 0x000AA9F8
		internal XamlKeyElementStartNode(int lineNumber, int linePosition, int depth, string assemblyName, string typeFullName, Type elementType, Type serializerType) : base(XamlNodeType.KeyElementStart, lineNumber, linePosition, depth, assemblyName, typeFullName, elementType, serializerType, false, false, false)
		{
		}
	}
}
