using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004CB RID: 1227
	internal class ButtonStandardAdapter : ButtonBaseAdapter
	{
		// Token: 0x060051AA RID: 20906 RVA: 0x0015344C File Offset: 0x0015164C
		internal ButtonStandardAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x060051AB RID: 20907 RVA: 0x001543F5 File Offset: 0x001525F5
		// (set) Token: 0x060051AC RID: 20908 RVA: 0x001543FD File Offset: 0x001525FD
		private protected bool IsFilledWithHighlightColor { protected get; private set; }

		// Token: 0x060051AD RID: 20909 RVA: 0x00154408 File Offset: 0x00152608
		private PushButtonState DetermineState(bool up)
		{
			PushButtonState result = PushButtonState.Normal;
			if (!up)
			{
				result = PushButtonState.Pressed;
			}
			else if (base.Control.MouseIsOver)
			{
				result = PushButtonState.Hot;
			}
			else if (!base.Control.Enabled)
			{
				result = PushButtonState.Disabled;
			}
			else if (base.Control.Focused || base.Control.IsDefault)
			{
				result = PushButtonState.Default;
			}
			return result;
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x0015445D File Offset: 0x0015265D
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			this.PaintWorker(e, true, state);
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x00154468 File Offset: 0x00152668
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			this.PaintWorker(e, false, state);
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x00154473 File Offset: 0x00152673
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			this.PaintUp(e, state);
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x00154480 File Offset: 0x00152680
		private void PaintThemedButtonBackground(PaintEventArgs e, Rectangle bounds, bool up)
		{
			PushButtonState state = this.DetermineState(up);
			if (ButtonRenderer.IsBackgroundPartiallyTransparent(state))
			{
				ButtonRenderer.DrawParentBackground(e.Graphics, bounds, base.Control);
			}
			if (!DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				ButtonRenderer.DrawButton(e.Graphics, base.Control.ClientRectangle, false, state);
			}
			else
			{
				ButtonRenderer.DrawButtonForHandle(e.Graphics, base.Control.ClientRectangle, false, state, base.Control.HandleInternal);
			}
			bounds.Inflate(-ButtonBaseAdapter.buttonBorderSize, -ButtonBaseAdapter.buttonBorderSize);
			if (!base.Control.UseVisualStyleBackColor)
			{
				bool flag = false;
				bool flag2 = up && base.IsHighContrastHighlighted();
				Color color = flag2 ? SystemColors.Highlight : base.Control.BackColor;
				if (color.A == 255 && e.HDC != IntPtr.Zero && DisplayInformation.BitsPerPixel > 8)
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
					SafeNativeMethods.FillRect(new HandleRef(e, e.HDC), ref rect, new HandleRef(this, flag2 ? SafeNativeMethods.GetSysColorBrush(ColorTranslator.ToOle(color) & 255) : base.Control.BackColorBrush));
					flag = true;
				}
				if (!flag && color.A > 0)
				{
					if (color.A == 255)
					{
						color = e.Graphics.GetNearestColor(color);
					}
					using (Brush brush = new SolidBrush(color))
					{
						e.Graphics.FillRectangle(brush, bounds);
						this.IsFilledWithHighlightColor = (color.ToArgb() == SystemColors.Highlight.ToArgb());
					}
				}
			}
			if (base.Control.BackgroundImage != null && !DisplayInformation.HighContrast)
			{
				ControlPaint.DrawBackgroundImage(e.Graphics, base.Control.BackgroundImage, Color.Transparent, base.Control.BackgroundImageLayout, base.Control.ClientRectangle, bounds, base.Control.DisplayRectangle.Location, base.Control.RightToLeft);
			}
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x001546A4 File Offset: 0x001528A4
		private void PaintWorker(PaintEventArgs e, bool up, CheckState state)
		{
			up = (up && state == CheckState.Unchecked);
			this.IsFilledWithHighlightColor = false;
			ButtonBaseAdapter.ColorData colorData = base.PaintRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData;
			if (Application.RenderWithVisualStyles)
			{
				layoutData = this.PaintLayout(e, true).Layout();
			}
			else
			{
				layoutData = this.PaintLayout(e, up).Layout();
			}
			Graphics graphics = e.Graphics;
			Button button = base.Control as Button;
			if (Application.RenderWithVisualStyles)
			{
				this.PaintThemedButtonBackground(e, base.Control.ClientRectangle, up);
			}
			else
			{
				Brush brush = null;
				if (state == CheckState.Indeterminate)
				{
					brush = ButtonBaseAdapter.CreateDitherBrush(colorData.highlight, colorData.buttonFace);
				}
				try
				{
					Rectangle clientRectangle = base.Control.ClientRectangle;
					if (up)
					{
						clientRectangle.Inflate(-2, -2);
					}
					else
					{
						clientRectangle.Inflate(-1, -1);
					}
					base.PaintButtonBackground(e, clientRectangle, brush);
				}
				finally
				{
					if (brush != null)
					{
						brush.Dispose();
						brush = null;
					}
				}
			}
			base.PaintImage(e, layoutData);
			if (Application.RenderWithVisualStyles)
			{
				layoutData.focus.Inflate(1, 1);
			}
			if (up & base.IsHighContrastHighlighted2())
			{
				Color highlightText = SystemColors.HighlightText;
				base.PaintField(e, layoutData, colorData, highlightText, false);
				if (base.Control.Focused && base.Control.ShowFocusCues)
				{
					ControlPaint.DrawHighContrastFocusRectangle(graphics, layoutData.focus, highlightText);
				}
			}
			else if (up & base.IsHighContrastHighlighted())
			{
				base.PaintField(e, layoutData, colorData, SystemColors.HighlightText, true);
			}
			else
			{
				base.PaintField(e, layoutData, colorData, colorData.windowText, true);
			}
			if (!Application.RenderWithVisualStyles)
			{
				Rectangle clientRectangle2 = base.Control.ClientRectangle;
				if (base.Control.IsDefault)
				{
					clientRectangle2.Inflate(-1, -1);
				}
				ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle2, colorData.windowFrame, base.Control.IsDefault);
				if (up)
				{
					base.Draw3DBorder(graphics, clientRectangle2, colorData, up);
					return;
				}
				ControlPaint.DrawBorder(graphics, clientRectangle2, colorData.buttonShadow, ButtonBorderStyle.Solid);
			}
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x00154888 File Offset: 0x00152A88
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			return this.PaintLayout(e, false);
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x001548A0 File Offset: 0x00152AA0
		private ButtonBaseAdapter.LayoutOptions PaintLayout(PaintEventArgs e, bool up)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.textOffset = !up;
			layoutOptions.everettButtonCompat = !Application.RenderWithVisualStyles;
			return layoutOptions;
		}

		// Token: 0x04003473 RID: 13427
		private const int borderWidth = 2;
	}
}
