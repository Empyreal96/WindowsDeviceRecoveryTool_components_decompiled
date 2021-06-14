using System;

namespace System.Windows.Markup
{
	// Token: 0x02000257 RID: 599
	internal class XamlPropertyIDictionaryStartNode : XamlPropertyComplexStartNode
	{
		// Token: 0x060022FC RID: 8956 RVA: 0x000AC6E8 File Offset: 0x000AA8E8
		internal XamlPropertyIDictionaryStartNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.PropertyIDictionaryStart, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}
	}
}
