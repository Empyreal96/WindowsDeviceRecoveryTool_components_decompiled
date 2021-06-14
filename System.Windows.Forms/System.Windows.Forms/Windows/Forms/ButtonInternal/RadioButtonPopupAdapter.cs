using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004D3 RID: 1235
	internal class RadioButtonPopupAdapter : RadioButtonFlatAdapter
	{
		// Token: 0x060051F6 RID: 20982 RVA: 0x00156769 File Offset: 0x00154969
		internal RadioButtonPopupAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x00156774 File Offset: 0x00154974
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			Graphics graphics = e.Graphics;
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintUp(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackgroundFlat(e, layoutData.checkBounds, colorData.buttonShadow, colorData.options.highContrast ? colorData.buttonFace : colorData.highlight);
			base.DrawCheckOnly(e, layoutData, colorData.windowText, colorData.highlight, true);
			base.AdjustFocusRectangle(layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x00156850 File Offset: 0x00154A50
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			Graphics graphics = e.Graphics;
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintOver(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			Color checkBackground = (colorData.options.highContrast && AccessibilityImprovements.Level1) ? colorData.buttonFace : colorData.highlight;
			base.DrawCheckBackground3DLite(e, layoutData.checkBounds, colorData.windowText, checkBackground, colorData, true);
			base.DrawCheckOnly(e, layoutData, colorData.windowText, colorData.highlight, true);
			base.AdjustFocusRectangle(layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x00156938 File Offset: 0x00154B38
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			Graphics graphics = e.Graphics;
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintDown(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackground3DLite(e, layoutData.checkBounds, colorData.windowText, colorData.highlight, colorData, true);
			base.DrawCheckOnly(e, layoutData, colorData.buttonShadow, colorData.highlight, true);
			base.AdjustFocusRectangle(layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x00156A00 File Offset: 0x00154C00
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonPopupAdapter(base.Control);
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x00156A10 File Offset: 0x00154C10
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = base.Layout(e);
			if (!base.Control.MouseIsDown && !base.Control.MouseIsOver)
			{
				layoutOptions.shadowedText = true;
			}
			return layoutOptions;
		}
	}
}
