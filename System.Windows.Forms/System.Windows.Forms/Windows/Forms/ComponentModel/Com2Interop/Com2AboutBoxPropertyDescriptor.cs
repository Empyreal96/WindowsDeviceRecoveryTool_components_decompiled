using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004A0 RID: 1184
	internal class Com2AboutBoxPropertyDescriptor : Com2PropertyDescriptor
	{
		// Token: 0x0600503B RID: 20539 RVA: 0x0014C904 File Offset: 0x0014AB04
		public Com2AboutBoxPropertyDescriptor() : base(-552, "About", new Attribute[]
		{
			new DispIdAttribute(-552),
			DesignerSerializationVisibilityAttribute.Hidden,
			new DescriptionAttribute(SR.GetString("AboutBoxDesc")),
			new ParenthesizePropertyNameAttribute(true)
		}, true, typeof(string), null, false)
		{
		}

		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x0600503C RID: 20540 RVA: 0x0014C964 File Offset: 0x0014AB64
		public override Type ComponentType
		{
			get
			{
				return typeof(UnsafeNativeMethods.IDispatch);
			}
		}

		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x0600503D RID: 20541 RVA: 0x0014C970 File Offset: 0x0014AB70
		public override TypeConverter Converter
		{
			get
			{
				if (this.converter == null)
				{
					this.converter = new TypeConverter();
				}
				return this.converter;
			}
		}

		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x0600503E RID: 20542 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x0600503F RID: 20543 RVA: 0x0014C98B File Offset: 0x0014AB8B
		public override Type PropertyType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x0014C997 File Offset: 0x0014AB97
		public override object GetEditor(Type editorBaseType)
		{
			if (editorBaseType == typeof(UITypeEditor) && this.editor == null)
			{
				this.editor = new Com2AboutBoxPropertyDescriptor.AboutBoxUITypeEditor();
			}
			return this.editor;
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x000E9114 File Offset: 0x000E7314
		public override object GetValue(object component)
		{
			return "";
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x0000701A File Offset: 0x0000521A
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x0014C9C4 File Offset: 0x0014ABC4
		public override void SetValue(object component, object value)
		{
			throw new ArgumentException();
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x0400340A RID: 13322
		private TypeConverter converter;

		// Token: 0x0400340B RID: 13323
		private UITypeEditor editor;

		// Token: 0x0200083F RID: 2111
		public class AboutBoxUITypeEditor : UITypeEditor
		{
			// Token: 0x06006F53 RID: 28499 RVA: 0x001984A0 File Offset: 0x001966A0
			public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				object instance = context.Instance;
				if (Marshal.IsComObject(instance) && instance is UnsafeNativeMethods.IDispatch)
				{
					UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)instance;
					NativeMethods.tagEXCEPINFO pExcepInfo = new NativeMethods.tagEXCEPINFO();
					Guid empty = Guid.Empty;
					int num = dispatch.Invoke(-552, ref empty, SafeNativeMethods.GetThreadLCID(), 1, new NativeMethods.tagDISPPARAMS(), null, pExcepInfo, null);
				}
				return value;
			}

			// Token: 0x06006F54 RID: 28500 RVA: 0x0000E211 File Offset: 0x0000C411
			public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}
		}
	}
}
