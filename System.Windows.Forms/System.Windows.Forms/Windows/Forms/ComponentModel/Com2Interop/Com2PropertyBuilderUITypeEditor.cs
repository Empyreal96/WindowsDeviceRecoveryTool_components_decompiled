using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004B1 RID: 1201
	internal class Com2PropertyBuilderUITypeEditor : Com2ExtendedUITypeEditor
	{
		// Token: 0x060050C4 RID: 20676 RVA: 0x0014EA60 File Offset: 0x0014CC60
		public Com2PropertyBuilderUITypeEditor(Com2PropertyDescriptor pd, string guidString, int type, UITypeEditor baseEditor) : base(baseEditor)
		{
			this.propDesc = pd;
			this.guidString = guidString;
			this.bldrType = type;
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x0014EA80 File Offset: 0x0014CC80
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IntPtr handle = UnsafeNativeMethods.GetFocus();
			IUIService iuiservice = (IUIService)provider.GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				IWin32Window dialogOwnerWindow = iuiservice.GetDialogOwnerWindow();
				if (dialogOwnerWindow != null)
				{
					handle = dialogOwnerWindow.Handle;
				}
			}
			bool flag = false;
			object result = value;
			try
			{
				object obj = this.propDesc.TargetObject;
				if (obj is ICustomTypeDescriptor)
				{
					obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this.propDesc);
				}
				NativeMethods.IProvidePropertyBuilder providePropertyBuilder = (NativeMethods.IProvidePropertyBuilder)obj;
				if (NativeMethods.Failed(providePropertyBuilder.ExecuteBuilder(this.propDesc.DISPID, this.guidString, null, new HandleRef(null, handle), ref result, ref flag)))
				{
					flag = false;
				}
			}
			catch (ExternalException ex)
			{
			}
			if (flag && (this.bldrType & 4) == 0)
			{
				return result;
			}
			return value;
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x0000E211 File Offset: 0x0000C411
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x0400342A RID: 13354
		private Com2PropertyDescriptor propDesc;

		// Token: 0x0400342B RID: 13355
		private string guidString;

		// Token: 0x0400342C RID: 13356
		private int bldrType;
	}
}
