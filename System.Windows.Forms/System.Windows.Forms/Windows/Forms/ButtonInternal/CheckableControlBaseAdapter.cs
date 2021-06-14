using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004CC RID: 1228
	internal abstract class CheckableControlBaseAdapter : ButtonBaseAdapter
	{
		// Token: 0x060051B5 RID: 20917 RVA: 0x0015344C File Offset: 0x0015164C
		internal CheckableControlBaseAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x060051B6 RID: 20918 RVA: 0x001548CD File Offset: 0x00152ACD
		protected ButtonBaseAdapter ButtonAdapter
		{
			get
			{
				if (this.buttonAdapter == null)
				{
					this.buttonAdapter = this.CreateButtonAdapter();
				}
				return this.buttonAdapter;
			}
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x001548EC File Offset: 0x00152AEC
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (this.Appearance == Appearance.Button)
			{
				return this.ButtonAdapter.GetPreferredSizeCore(proposedSize);
			}
			Size preferredSizeCore;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				using (PaintEventArgs paintEventArgs = new PaintEventArgs(graphics, default(Rectangle)))
				{
					ButtonBaseAdapter.LayoutOptions layoutOptions = this.Layout(paintEventArgs);
					preferredSizeCore = layoutOptions.GetPreferredSizeCore(proposedSize);
				}
			}
			return preferredSizeCore;
		}

		// Token: 0x060051B8 RID: 20920
		protected abstract ButtonBaseAdapter CreateButtonAdapter();

		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x060051B9 RID: 20921 RVA: 0x0015496C File Offset: 0x00152B6C
		private Appearance Appearance
		{
			get
			{
				CheckBox checkBox = base.Control as CheckBox;
				if (checkBox != null)
				{
					return checkBox.Appearance;
				}
				RadioButton radioButton = base.Control as RadioButton;
				if (radioButton != null)
				{
					return radioButton.Appearance;
				}
				return Appearance.Normal;
			}
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x001549A8 File Offset: 0x00152BA8
		internal override ButtonBaseAdapter.LayoutOptions CommonLayout()
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = base.CommonLayout();
			layoutOptions.growBorderBy1PxWhenDefault = false;
			layoutOptions.borderSize = 0;
			layoutOptions.paddingSize = 0;
			layoutOptions.maxFocus = false;
			layoutOptions.focusOddEvenFixup = true;
			layoutOptions.checkSize = 13;
			return layoutOptions;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x001549E8 File Offset: 0x00152BE8
		internal double GetDpiScaleRatio(Graphics g)
		{
			return CheckableControlBaseAdapter.GetDpiScaleRatio(g, base.Control);
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x001549F6 File Offset: 0x00152BF6
		internal static double GetDpiScaleRatio(Graphics g, Control control)
		{
			if (DpiHelper.EnableDpiChangedMessageHandling && control != null && control.IsHandleCreated)
			{
				return (double)control.deviceDpi / 96.0;
			}
			if (g == null)
			{
				return 1.0;
			}
			return (double)(g.DpiX / 96f);
		}

		// Token: 0x04003475 RID: 13429
		private const int standardCheckSize = 13;

		// Token: 0x04003476 RID: 13430
		private ButtonBaseAdapter buttonAdapter;
	}
}
