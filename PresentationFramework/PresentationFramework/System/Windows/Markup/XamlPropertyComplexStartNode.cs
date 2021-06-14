using System;

namespace System.Windows.Markup
{
	// Token: 0x02000243 RID: 579
	internal class XamlPropertyComplexStartNode : XamlPropertyBaseNode
	{
		// Token: 0x060022AE RID: 8878 RVA: 0x000AC194 File Offset: 0x000AA394
		internal XamlPropertyComplexStartNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.PropertyComplexStart, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x000AC1B4 File Offset: 0x000AA3B4
		internal XamlPropertyComplexStartNode(XamlNodeType token, int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(token, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}
	}
}
