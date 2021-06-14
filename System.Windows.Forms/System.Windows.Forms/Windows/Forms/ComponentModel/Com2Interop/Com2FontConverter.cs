using System;
using System.Drawing;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004A8 RID: 1192
	internal class Com2FontConverter : Com2DataTypeToManagedDataTypeConverter
	{
		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x06005077 RID: 20599 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool AllowExpand
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x06005078 RID: 20600 RVA: 0x0014D201 File Offset: 0x0014B401
		public override Type ManagedType
		{
			get
			{
				return typeof(Font);
			}
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x0014D210 File Offset: 0x0014B410
		public override object ConvertNativeToManaged(object nativeValue, Com2PropertyDescriptor pd)
		{
			UnsafeNativeMethods.IFont font = nativeValue as UnsafeNativeMethods.IFont;
			if (font == null)
			{
				this.lastHandle = IntPtr.Zero;
				this.lastFont = Control.DefaultFont;
				return this.lastFont;
			}
			IntPtr hfont = font.GetHFont();
			if (hfont == this.lastHandle && this.lastFont != null)
			{
				return this.lastFont;
			}
			this.lastHandle = hfont;
			try
			{
				Font font2 = Font.FromHfont(this.lastHandle);
				try
				{
					this.lastFont = ControlPaint.FontInPoints(font2);
				}
				finally
				{
					font2.Dispose();
				}
			}
			catch (ArgumentException)
			{
				this.lastFont = Control.DefaultFont;
			}
			return this.lastFont;
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x0014D2C4 File Offset: 0x0014B4C4
		public override object ConvertManagedToNative(object managedValue, Com2PropertyDescriptor pd, ref bool cancelSet)
		{
			if (managedValue == null)
			{
				managedValue = Control.DefaultFont;
			}
			cancelSet = true;
			if (this.lastFont != null && this.lastFont.Equals(managedValue))
			{
				return null;
			}
			this.lastFont = (Font)managedValue;
			UnsafeNativeMethods.IFont font = (UnsafeNativeMethods.IFont)pd.GetNativeValue(pd.TargetObject);
			if (font != null)
			{
				bool flag = ControlPaint.FontToIFont(this.lastFont, font);
				if (flag)
				{
					this.lastFont = null;
					this.ConvertNativeToManaged(font, pd);
				}
			}
			return null;
		}

		// Token: 0x04003413 RID: 13331
		private IntPtr lastHandle = IntPtr.Zero;

		// Token: 0x04003414 RID: 13332
		private Font lastFont;
	}
}
