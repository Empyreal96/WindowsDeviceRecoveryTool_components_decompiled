using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004C9 RID: 1225
	internal class ButtonFlatAdapter : ButtonBaseAdapter
	{
		// Token: 0x0600519B RID: 20891 RVA: 0x0015344C File Offset: 0x0015164C
		internal ButtonFlatAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x00153458 File Offset: 0x00151658
		private void PaintBackground(PaintEventArgs e, Rectangle r, Color backColor)
		{
			Rectangle rectangle = r;
			rectangle.Inflate(-base.Control.FlatAppearance.BorderSize, -base.Control.FlatAppearance.BorderSize);
			base.Control.PaintBackground(e, rectangle, backColor, rectangle.Location);
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x001534A8 File Offset: 0x001516A8
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			bool flag = base.Control.FlatAppearance.BorderSize != 1 || !base.Control.FlatAppearance.BorderColor.IsEmpty;
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintFlatLayout(e, !base.Control.FlatAppearance.CheckedBackColor.IsEmpty || (SystemInformation.HighContrast ? (state != CheckState.Indeterminate) : (state == CheckState.Unchecked)), !flag && SystemInformation.HighContrast && state == CheckState.Checked, base.Control.FlatAppearance.BorderSize).Layout();
			if (!base.Control.FlatAppearance.BorderColor.IsEmpty)
			{
				colorData.windowFrame = base.Control.FlatAppearance.BorderColor;
			}
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			Color color = base.Control.BackColor;
			if (!base.Control.FlatAppearance.CheckedBackColor.IsEmpty)
			{
				if (state != CheckState.Checked)
				{
					if (state == CheckState.Indeterminate)
					{
						color = ButtonBaseAdapter.MixedColor(base.Control.FlatAppearance.CheckedBackColor, colorData.buttonFace);
					}
				}
				else
				{
					color = base.Control.FlatAppearance.CheckedBackColor;
				}
			}
			else if (state != CheckState.Checked)
			{
				if (state == CheckState.Indeterminate)
				{
					color = ButtonBaseAdapter.MixedColor(colorData.highlight, colorData.buttonFace);
				}
			}
			else
			{
				color = colorData.highlight;
			}
			this.PaintBackground(e, clientRectangle, base.IsHighContrastHighlighted2() ? SystemColors.Highlight : color);
			if (base.Control.IsDefault)
			{
				clientRectangle.Inflate(-1, -1);
			}
			base.PaintImage(e, layoutData);
			base.PaintField(e, layoutData, colorData, base.IsHighContrastHighlighted2() ? SystemColors.HighlightText : colorData.windowText, false);
			if (base.Control.Focused && base.Control.ShowFocusCues)
			{
				ButtonBaseAdapter.DrawFlatFocus(graphics, layoutData.focus, colorData.options.highContrast ? colorData.windowText : colorData.constrastButtonShadow);
			}
			if (!base.Control.IsDefault || !base.Control.Focused || base.Control.FlatAppearance.BorderSize != 0)
			{
				ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle, colorData.windowFrame, base.Control.IsDefault);
			}
			if (flag)
			{
				if (base.Control.FlatAppearance.BorderSize != 1)
				{
					ButtonBaseAdapter.DrawFlatBorderWithSize(graphics, clientRectangle, colorData.windowFrame, base.Control.FlatAppearance.BorderSize);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
			else
			{
				if (state == CheckState.Checked && SystemInformation.HighContrast)
				{
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.buttonShadow);
					return;
				}
				if (state == CheckState.Indeterminate)
				{
					ButtonBaseAdapter.Draw3DLiteBorder(graphics, clientRectangle, colorData, false);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x00153794 File Offset: 0x00151994
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			bool flag = base.Control.FlatAppearance.BorderSize != 1 || !base.Control.FlatAppearance.BorderColor.IsEmpty;
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintFlatLayout(e, !base.Control.FlatAppearance.CheckedBackColor.IsEmpty || (SystemInformation.HighContrast ? (state != CheckState.Indeterminate) : (state == CheckState.Unchecked)), !flag && SystemInformation.HighContrast && state == CheckState.Checked, base.Control.FlatAppearance.BorderSize).Layout();
			if (!base.Control.FlatAppearance.BorderColor.IsEmpty)
			{
				colorData.windowFrame = base.Control.FlatAppearance.BorderColor;
			}
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			Color backColor = base.Control.BackColor;
			if (!base.Control.FlatAppearance.MouseDownBackColor.IsEmpty)
			{
				backColor = base.Control.FlatAppearance.MouseDownBackColor;
			}
			else if (state > CheckState.Checked)
			{
				if (state == CheckState.Indeterminate)
				{
					backColor = ButtonBaseAdapter.MixedColor(colorData.options.highContrast ? colorData.buttonShadow : colorData.lowHighlight, colorData.buttonFace);
				}
			}
			else
			{
				backColor = (colorData.options.highContrast ? colorData.buttonShadow : colorData.lowHighlight);
			}
			this.PaintBackground(e, clientRectangle, backColor);
			if (base.Control.IsDefault)
			{
				clientRectangle.Inflate(-1, -1);
			}
			base.PaintImage(e, layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, false);
			if (base.Control.Focused && base.Control.ShowFocusCues)
			{
				ButtonBaseAdapter.DrawFlatFocus(graphics, layoutData.focus, colorData.options.highContrast ? colorData.windowText : colorData.constrastButtonShadow);
			}
			if (!base.Control.IsDefault || !base.Control.Focused || base.Control.FlatAppearance.BorderSize != 0)
			{
				ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle, colorData.windowFrame, base.Control.IsDefault);
			}
			if (flag)
			{
				if (base.Control.FlatAppearance.BorderSize != 1)
				{
					ButtonBaseAdapter.DrawFlatBorderWithSize(graphics, clientRectangle, colorData.windowFrame, base.Control.FlatAppearance.BorderSize);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
			else
			{
				if (state == CheckState.Checked && SystemInformation.HighContrast)
				{
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.buttonShadow);
					return;
				}
				if (state == CheckState.Indeterminate)
				{
					ButtonBaseAdapter.Draw3DLiteBorder(graphics, clientRectangle, colorData, false);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x00153A60 File Offset: 0x00151C60
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (SystemInformation.HighContrast)
			{
				this.PaintUp(e, state);
				return;
			}
			bool flag = base.Control.FlatAppearance.BorderSize != 1 || !base.Control.FlatAppearance.BorderColor.IsEmpty;
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintFlatLayout(e, !base.Control.FlatAppearance.CheckedBackColor.IsEmpty || state == CheckState.Unchecked, false, base.Control.FlatAppearance.BorderSize).Layout();
			if (!base.Control.FlatAppearance.BorderColor.IsEmpty)
			{
				colorData.windowFrame = base.Control.FlatAppearance.BorderColor;
			}
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			Color color = base.Control.BackColor;
			if (!base.Control.FlatAppearance.MouseOverBackColor.IsEmpty)
			{
				color = base.Control.FlatAppearance.MouseOverBackColor;
			}
			else if (!base.Control.FlatAppearance.CheckedBackColor.IsEmpty)
			{
				if (state == CheckState.Checked || state == CheckState.Indeterminate)
				{
					color = ButtonBaseAdapter.MixedColor(base.Control.FlatAppearance.CheckedBackColor, colorData.lowButtonFace);
				}
				else
				{
					color = colorData.lowButtonFace;
				}
			}
			else if (state == CheckState.Indeterminate)
			{
				color = ButtonBaseAdapter.MixedColor(colorData.buttonFace, colorData.lowButtonFace);
			}
			else
			{
				color = colorData.lowButtonFace;
			}
			this.PaintBackground(e, clientRectangle, base.IsHighContrastHighlighted2() ? SystemColors.Highlight : color);
			if (base.Control.IsDefault)
			{
				clientRectangle.Inflate(-1, -1);
			}
			base.PaintImage(e, layoutData);
			base.PaintField(e, layoutData, colorData, base.IsHighContrastHighlighted2() ? SystemColors.HighlightText : colorData.windowText, false);
			if (base.Control.Focused && base.Control.ShowFocusCues)
			{
				ButtonBaseAdapter.DrawFlatFocus(graphics, layoutData.focus, colorData.constrastButtonShadow);
			}
			if (!base.Control.IsDefault || !base.Control.Focused || base.Control.FlatAppearance.BorderSize != 0)
			{
				ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle, colorData.windowFrame, base.Control.IsDefault);
			}
			if (flag)
			{
				if (base.Control.FlatAppearance.BorderSize != 1)
				{
					ButtonBaseAdapter.DrawFlatBorderWithSize(graphics, clientRectangle, colorData.windowFrame, base.Control.FlatAppearance.BorderSize);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
			else
			{
				if (state == CheckState.Unchecked)
				{
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
					return;
				}
				ButtonBaseAdapter.Draw3DLiteBorder(graphics, clientRectangle, colorData, false);
				return;
			}
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x00153D18 File Offset: 0x00151F18
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			return this.PaintFlatLayout(e, false, true, base.Control.FlatAppearance.BorderSize);
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x00153D40 File Offset: 0x00151F40
		internal static ButtonBaseAdapter.LayoutOptions PaintFlatLayout(Graphics g, bool up, bool check, int borderSize, Rectangle clientRectangle, Padding padding, bool isDefault, Font font, string text, bool enabled, ContentAlignment textAlign, RightToLeft rtl)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = ButtonBaseAdapter.CommonLayout(clientRectangle, padding, isDefault, font, text, enabled, textAlign, rtl);
			layoutOptions.borderSize = borderSize + (check ? 1 : 0);
			layoutOptions.paddingSize = (check ? 1 : 2);
			layoutOptions.focusOddEvenFixup = false;
			layoutOptions.textOffset = !up;
			layoutOptions.shadowedText = SystemInformation.HighContrast;
			return layoutOptions;
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x00153D9C File Offset: 0x00151F9C
		private ButtonBaseAdapter.LayoutOptions PaintFlatLayout(PaintEventArgs e, bool up, bool check, int borderSize)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.borderSize = borderSize + (check ? 1 : 0);
			layoutOptions.paddingSize = (check ? 1 : 2);
			layoutOptions.focusOddEvenFixup = false;
			layoutOptions.textOffset = !up;
			layoutOptions.shadowedText = SystemInformation.HighContrast;
			return layoutOptions;
		}

		// Token: 0x04003472 RID: 13426
		private const int BORDERSIZE = 1;
	}
}
