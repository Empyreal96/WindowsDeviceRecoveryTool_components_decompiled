using System;
using System.ComponentModel;

namespace System.Drawing.Design
{
	// Token: 0x020000FD RID: 253
	internal class Com2ExtendedUITypeEditor : UITypeEditor
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x0000CB2C File Offset: 0x0000AD2C
		public Com2ExtendedUITypeEditor(UITypeEditor baseTypeEditor)
		{
			this.innerEditor = baseTypeEditor;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000CB3B File Offset: 0x0000AD3B
		public Com2ExtendedUITypeEditor(Type baseType)
		{
			this.innerEditor = (UITypeEditor)TypeDescriptor.GetEditor(baseType, typeof(UITypeEditor));
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000CB5E File Offset: 0x0000AD5E
		public UITypeEditor InnerEditor
		{
			get
			{
				return this.innerEditor;
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000CB66 File Offset: 0x0000AD66
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (this.innerEditor != null)
			{
				return this.innerEditor.EditValue(context, provider, value);
			}
			return base.EditValue(context, provider, value);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000CB88 File Offset: 0x0000AD88
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			if (this.innerEditor != null)
			{
				return this.innerEditor.GetPaintValueSupported(context);
			}
			return base.GetPaintValueSupported(context);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000CBA6 File Offset: 0x0000ADA6
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (this.innerEditor != null)
			{
				return this.innerEditor.GetEditStyle(context);
			}
			return base.GetEditStyle(context);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000CBC4 File Offset: 0x0000ADC4
		public override void PaintValue(PaintValueEventArgs e)
		{
			if (this.innerEditor != null)
			{
				this.innerEditor.PaintValue(e);
			}
			base.PaintValue(e);
		}

		// Token: 0x0400042F RID: 1071
		private UITypeEditor innerEditor;
	}
}
