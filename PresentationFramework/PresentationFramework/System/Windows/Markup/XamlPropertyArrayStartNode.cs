using System;

namespace System.Windows.Markup
{
	// Token: 0x02000255 RID: 597
	internal class XamlPropertyArrayStartNode : XamlPropertyComplexStartNode
	{
		// Token: 0x060022FA RID: 8954 RVA: 0x000AC6A8 File Offset: 0x000AA8A8
		internal XamlPropertyArrayStartNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.PropertyArrayStart, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}
	}
}
