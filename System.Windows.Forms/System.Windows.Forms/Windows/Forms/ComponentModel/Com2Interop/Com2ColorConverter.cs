using System;
using System.Drawing;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004A1 RID: 1185
	internal class Com2ColorConverter : Com2DataTypeToManagedDataTypeConverter
	{
		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x06005046 RID: 20550 RVA: 0x0014C9CB File Offset: 0x0014ABCB
		public override Type ManagedType
		{
			get
			{
				return typeof(Color);
			}
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x0014C9D8 File Offset: 0x0014ABD8
		public override object ConvertNativeToManaged(object nativeValue, Com2PropertyDescriptor pd)
		{
			int oleColor = 0;
			if (nativeValue is uint)
			{
				oleColor = (int)((uint)nativeValue);
			}
			else if (nativeValue is int)
			{
				oleColor = (int)nativeValue;
			}
			return ColorTranslator.FromOle(oleColor);
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x0014CA14 File Offset: 0x0014AC14
		public override object ConvertManagedToNative(object managedValue, Com2PropertyDescriptor pd, ref bool cancelSet)
		{
			cancelSet = false;
			if (managedValue == null)
			{
				managedValue = Color.Black;
			}
			if (managedValue is Color)
			{
				return ColorTranslator.ToOle((Color)managedValue);
			}
			return 0;
		}
	}
}
