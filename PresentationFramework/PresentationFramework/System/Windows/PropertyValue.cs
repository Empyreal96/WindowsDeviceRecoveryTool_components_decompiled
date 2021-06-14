using System;

namespace System.Windows
{
	// Token: 0x020000FD RID: 253
	internal struct PropertyValue
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00020C5C File Offset: 0x0001EE5C
		internal object Value
		{
			get
			{
				DeferredReference deferredReference = this.ValueInternal as DeferredReference;
				if (deferredReference != null)
				{
					this.ValueInternal = deferredReference.GetValue(BaseValueSourceInternal.Unknown);
				}
				return this.ValueInternal;
			}
		}

		// Token: 0x040007EE RID: 2030
		internal PropertyValueType ValueType;

		// Token: 0x040007EF RID: 2031
		internal TriggerCondition[] Conditions;

		// Token: 0x040007F0 RID: 2032
		internal string ChildName;

		// Token: 0x040007F1 RID: 2033
		internal DependencyProperty Property;

		// Token: 0x040007F2 RID: 2034
		internal object ValueInternal;
	}
}
