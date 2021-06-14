using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004CF RID: 1231
	internal class CheckBoxPopupAdapter : CheckBoxBaseAdapter
	{
		// Token: 0x060051D3 RID: 20947 RVA: 0x00155333 File Offset: 0x00153533
		internal CheckBoxPopupAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x001555A0 File Offset: 0x001537A0
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintUp(e, base.Control.CheckState);
				return;
			}
			Graphics graphics = e.Graphics;
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintPopupLayout(e, false).Layout();
			Region clip = e.Graphics.Clip;
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackground(e, layoutData.checkBounds, colorData.windowText, colorData.options.highContrast ? colorData.buttonFace : colorData.highlight, true, colorData);
			ButtonBaseAdapter.DrawFlatBorder(e.Graphics, layoutData.checkBounds, (colorData.options.highContrast && !base.Control.Enabled && AccessibilityImprovements.Level1) ? colorData.windowFrame : colorData.buttonShadow);
			base.DrawCheckOnly(e, layoutData, colorData, colorData.windowText, colorData.highlight);
			base.AdjustFocusRectangle(layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x001556C8 File Offset: 0x001538C8
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			Graphics graphics = e.Graphics;
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintOver(e, base.Control.CheckState);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintPopupLayout(e, true).Layout();
			Region clip = e.Graphics.Clip;
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackground(e, layoutData.checkBounds, colorData.windowText, colorData.options.highContrast ? colorData.buttonFace : colorData.highlight, true, colorData);
			CheckBoxBaseAdapter.DrawPopupBorder(graphics, layoutData.checkBounds, colorData);
			base.DrawCheckOnly(e, layoutData, colorData, colorData.windowText, colorData.highlight);
			if (!AccessibilityImprovements.Level2 || !string.IsNullOrEmpty(base.Control.Text))
			{
				e.Graphics.Clip = clip;
				e.Graphics.ExcludeClip(layoutData.checkArea);
			}
			base.AdjustFocusRectangle(layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x001557F4 File Offset: 0x001539F4
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintDown(e, base.Control.CheckState);
				return;
			}
			Graphics graphics = e.Graphics;
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintPopupLayout(e, true).Layout();
			Region clip = e.Graphics.Clip;
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackground(e, layoutData.checkBounds, colorData.windowText, colorData.buttonFace, true, colorData);
			CheckBoxBaseAdapter.DrawPopupBorder(graphics, layoutData.checkBounds, colorData);
			base.DrawCheckOnly(e, layoutData, colorData, colorData.windowText, colorData.buttonFace);
			base.AdjustFocusRectangle(layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x001558D2 File Offset: 0x00153AD2
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonPopupAdapter(base.Control);
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x001558E0 File Offset: 0x00153AE0
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			return this.PaintPopupLayout(e, true);
		}

		// Token: 0x060051D9 RID: 20953 RVA: 0x001558F8 File Offset: 0x00153AF8
		internal static ButtonBaseAdapter.LayoutOptions PaintPopupLayout(Graphics g, bool show3D, int checkSize, Rectangle clientRectangle, Padding padding, bool isDefault, Font font, string text, bool enabled, ContentAlignment textAlign, RightToLeft rtl, Control control = null)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = ButtonBaseAdapter.CommonLayout(clientRectangle, padding, isDefault, font, text, enabled, textAlign, rtl);
			layoutOptions.shadowedText = false;
			if (show3D)
			{
				layoutOptions.checkSize = (int)((double)checkSize * CheckableControlBaseAdapter.GetDpiScaleRatio(g, control) + 1.0);
			}
			else
			{
				layoutOptions.checkSize = (int)((double)checkSize * CheckableControlBaseAdapter.GetDpiScaleRatio(g, control));
				layoutOptions.checkPaddingSize = 1;
			}
			return layoutOptions;
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x0015595C File Offset: 0x00153B5C
		private ButtonBaseAdapter.LayoutOptions PaintPopupLayout(PaintEventArgs e, bool show3D)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.shadowedText = false;
			if (show3D)
			{
				layoutOptions.checkSize = (int)(11.0 * base.GetDpiScaleRatio(e.Graphics) + 1.0);
			}
			else
			{
				layoutOptions.checkSize = (int)(11.0 * base.GetDpiScaleRatio(e.Graphics));
				layoutOptions.checkPaddingSize = 1;
			}
			return layoutOptions;
		}
	}
}
