using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004A3 RID: 1187
	internal abstract class Com2DataTypeToManagedDataTypeConverter
	{
		// Token: 0x170013D4 RID: 5076
		// (get) Token: 0x0600504D RID: 20557 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public virtual bool AllowExpand
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x0600504E RID: 20558
		public abstract Type ManagedType { get; }

		// Token: 0x0600504F RID: 20559
		public abstract object ConvertNativeToManaged(object nativeValue, Com2PropertyDescriptor pd);

		// Token: 0x06005050 RID: 20560
		public abstract object ConvertManagedToNative(object managedValue, Com2PropertyDescriptor pd, ref bool cancelSet);
	}
}
