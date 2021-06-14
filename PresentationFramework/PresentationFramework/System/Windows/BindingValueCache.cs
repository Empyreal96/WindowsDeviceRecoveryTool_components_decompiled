using System;

namespace System.Windows
{
	// Token: 0x02000101 RID: 257
	internal class BindingValueCache
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x00020F7B File Offset: 0x0001F17B
		internal BindingValueCache(Type bindingValueType, object valueAsBindingValueType)
		{
			this.BindingValueType = bindingValueType;
			this.ValueAsBindingValueType = valueAsBindingValueType;
		}

		// Token: 0x04000806 RID: 2054
		internal readonly Type BindingValueType;

		// Token: 0x04000807 RID: 2055
		internal readonly object ValueAsBindingValueType;
	}
}
