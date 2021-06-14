using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004D2 RID: 1234
	internal class RadioButtonFlatAdapter : RadioButtonBaseAdapter
	{
		// Token: 0x060051EF RID: 20975 RVA: 0x001564EA File Offset: 0x001546EA
		internal RadioButtonFlatAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x001564F4 File Offset: 0x001546F4
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonFlatAdapter buttonFlatAdapter = new ButtonFlatAdapter(base.Control);
				buttonFlatAdapter.PaintDown(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			if (base.Control.Enabled)
			{
				this.PaintFlatWorker(e, colorData.windowText, colorData.highlight, colorData.windowFrame, colorData);
				return;
			}
			this.PaintFlatWorker(e, colorData.buttonShadow, colorData.buttonFace, colorData.buttonShadow, colorData);
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x00156588 File Offset: 0x00154788
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonFlatAdapter buttonFlatAdapter = new ButtonFlatAdapter(base.Control);
				buttonFlatAdapter.PaintOver(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			if (base.Control.Enabled)
			{
				this.PaintFlatWorker(e, colorData.windowText, colorData.lowHighlight, colorData.windowFrame, colorData);
				return;
			}
			this.PaintFlatWorker(e, colorData.buttonShadow, colorData.buttonFace, colorData.buttonShadow, colorData);
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x0015661C File Offset: 0x0015481C
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonFlatAdapter buttonFlatAdapter = new ButtonFlatAdapter(base.Control);
				buttonFlatAdapter.PaintUp(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			if (base.Control.Enabled)
			{
				this.PaintFlatWorker(e, colorData.windowText, colorData.highlight, colorData.windowFrame, colorData);
				return;
			}
			this.PaintFlatWorker(e, colorData.buttonShadow, colorData.buttonFace, colorData.buttonShadow, colorData);
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x001566B0 File Offset: 0x001548B0
		private void PaintFlatWorker(PaintEventArgs e, Color checkColor, Color checkBackground, Color checkBorder, ButtonBaseAdapter.ColorData colors)
		{
			Graphics graphics = e.Graphics;
			ButtonBaseAdapter.LayoutData layout = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layout);
			base.DrawCheckFlat(e, layout, checkColor, colors.options.highContrast ? colors.buttonFace : checkBackground, checkBorder);
			base.AdjustFocusRectangle(layout);
			base.PaintField(e, layout, colors, checkColor, true);
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x00156722 File Offset: 0x00154922
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonFlatAdapter(base.Control);
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x00156730 File Offset: 0x00154930
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.checkSize = (int)(12.0 * base.GetDpiScaleRatio(e.Graphics));
			layoutOptions.shadowedText = false;
			return layoutOptions;
		}

		// Token: 0x0400347C RID: 13436
		protected const int flatCheckSize = 12;
	}
}
