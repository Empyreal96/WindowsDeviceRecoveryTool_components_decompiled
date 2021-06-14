using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004CD RID: 1229
	internal abstract class CheckBoxBaseAdapter : CheckableControlBaseAdapter
	{
		// Token: 0x060051BD RID: 20925 RVA: 0x00154A36 File Offset: 0x00152C36
		internal CheckBoxBaseAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x1700140D RID: 5133
		// (get) Token: 0x060051BE RID: 20926 RVA: 0x00154A3F File Offset: 0x00152C3F
		protected new CheckBox Control
		{
			get
			{
				return (CheckBox)base.Control;
			}
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x00154A4C File Offset: 0x00152C4C
		protected void DrawCheckFlat(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout, Color checkColor, Color checkBackground, Color checkBorder, ButtonBaseAdapter.ColorData colors)
		{
			Rectangle checkBounds = layout.checkBounds;
			if (!layout.options.everettButtonCompat)
			{
				int num = checkBounds.Width;
				checkBounds.Width = num - 1;
				num = checkBounds.Height;
				checkBounds.Height = num - 1;
			}
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(e.Graphics))
			{
				using (WindowsPen windowsPen = new WindowsPen(windowsGraphics.DeviceContext, checkBorder))
				{
					windowsGraphics.DrawRectangle(windowsPen, checkBounds);
				}
				if (layout.options.everettButtonCompat)
				{
					int num = checkBounds.Width;
					checkBounds.Width = num - 1;
					num = checkBounds.Height;
					checkBounds.Height = num - 1;
				}
				checkBounds.Inflate(-1, -1);
			}
			if (this.Control.CheckState == CheckState.Indeterminate)
			{
				int num = checkBounds.Width;
				checkBounds.Width = num + 1;
				num = checkBounds.Height;
				checkBounds.Height = num + 1;
				ButtonBaseAdapter.DrawDitheredFill(e.Graphics, colors.buttonFace, checkBackground, checkBounds);
			}
			else
			{
				using (WindowsGraphics windowsGraphics2 = WindowsGraphics.FromGraphics(e.Graphics))
				{
					using (WindowsBrush windowsBrush = new WindowsSolidBrush(windowsGraphics2.DeviceContext, checkBackground))
					{
						int num = checkBounds.Width;
						checkBounds.Width = num + 1;
						num = checkBounds.Height;
						checkBounds.Height = num + 1;
						windowsGraphics2.FillRectangle(windowsBrush, checkBounds);
					}
				}
			}
			this.DrawCheckOnly(e, layout, colors, checkColor, checkBackground);
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x00154BEC File Offset: 0x00152DEC
		internal static void DrawCheckBackground(bool controlEnabled, CheckState controlCheckState, Graphics g, Rectangle bounds, Color checkColor, Color checkBackground, bool disabledColors, ButtonBaseAdapter.ColorData colors)
		{
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
			{
				WindowsBrush windowsBrush;
				if (!controlEnabled && disabledColors)
				{
					windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, SystemColors.Control);
				}
				else if (controlCheckState == CheckState.Indeterminate && checkBackground == SystemColors.Window && disabledColors)
				{
					Color color = SystemInformation.HighContrast ? SystemColors.ControlDark : SystemColors.Control;
					byte red = (color.R + SystemColors.Window.R) / 2;
					byte green = (color.G + SystemColors.Window.G) / 2;
					byte blue = (color.B + SystemColors.Window.B) / 2;
					windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, Color.FromArgb((int)red, (int)green, (int)blue));
				}
				else
				{
					windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, checkBackground);
				}
				try
				{
					windowsGraphics.FillRectangle(windowsBrush, bounds);
				}
				finally
				{
					if (windowsBrush != null)
					{
						windowsBrush.Dispose();
					}
				}
			}
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x00154CFC File Offset: 0x00152EFC
		protected void DrawCheckBackground(PaintEventArgs e, Rectangle bounds, Color checkColor, Color checkBackground, bool disabledColors, ButtonBaseAdapter.ColorData colors)
		{
			if (this.Control.CheckState == CheckState.Indeterminate)
			{
				ButtonBaseAdapter.DrawDitheredFill(e.Graphics, colors.buttonFace, checkBackground, bounds);
				return;
			}
			CheckBoxBaseAdapter.DrawCheckBackground(this.Control.Enabled, this.Control.CheckState, e.Graphics, bounds, checkColor, checkBackground, disabledColors, colors);
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x00154D58 File Offset: 0x00152F58
		protected void DrawCheckOnly(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout, ButtonBaseAdapter.ColorData colors, Color checkColor, Color checkBackground)
		{
			CheckBoxBaseAdapter.DrawCheckOnly(11, this.Control.Checked, this.Control.Enabled, this.Control.CheckState, e.Graphics, layout, colors, checkColor, checkBackground);
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x00154D9C File Offset: 0x00152F9C
		internal static void DrawCheckOnly(int checkSize, bool controlChecked, bool controlEnabled, CheckState controlCheckState, Graphics g, ButtonBaseAdapter.LayoutData layout, ButtonBaseAdapter.ColorData colors, Color checkColor, Color checkBackground)
		{
			if (controlChecked)
			{
				if (!controlEnabled)
				{
					checkColor = colors.buttonShadow;
				}
				else if (controlCheckState == CheckState.Indeterminate)
				{
					checkColor = (SystemInformation.HighContrast ? colors.highlight : colors.buttonShadow);
				}
				Rectangle checkBounds = layout.checkBounds;
				int num;
				if (checkBounds.Width == checkSize)
				{
					num = checkBounds.Width;
					checkBounds.Width = num + 1;
					num = checkBounds.Height;
					checkBounds.Height = num + 1;
				}
				num = checkBounds.Width;
				checkBounds.Width = num + 1;
				num = checkBounds.Height;
				checkBounds.Height = num + 1;
				Bitmap checkBoxImage;
				if (controlCheckState == CheckState.Checked)
				{
					checkBoxImage = CheckBoxBaseAdapter.GetCheckBoxImage(checkColor, checkBounds, ref CheckBoxBaseAdapter.checkImageCheckedBackColor, ref CheckBoxBaseAdapter.checkImageChecked);
				}
				else
				{
					checkBoxImage = CheckBoxBaseAdapter.GetCheckBoxImage(checkColor, checkBounds, ref CheckBoxBaseAdapter.checkImageIndeterminateBackColor, ref CheckBoxBaseAdapter.checkImageIndeterminate);
				}
				if (layout.options.everettButtonCompat)
				{
					checkBounds.Y--;
				}
				else
				{
					checkBounds.Y -= 2;
				}
				ControlPaint.DrawImageColorized(g, checkBoxImage, checkBounds, checkColor);
			}
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x00154E98 File Offset: 0x00153098
		internal static Rectangle DrawPopupBorder(Graphics g, Rectangle r, ButtonBaseAdapter.ColorData colors)
		{
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
			{
				using (WindowsPen windowsPen = new WindowsPen(windowsGraphics.DeviceContext, colors.highlight))
				{
					using (WindowsPen windowsPen2 = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonShadow))
					{
						using (WindowsPen windowsPen3 = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonFace))
						{
							windowsGraphics.DrawLine(windowsPen, r.Right - 1, r.Top, r.Right - 1, r.Bottom);
							windowsGraphics.DrawLine(windowsPen, r.Left, r.Bottom - 1, r.Right, r.Bottom - 1);
							windowsGraphics.DrawLine(windowsPen2, r.Left, r.Top, r.Left, r.Bottom);
							windowsGraphics.DrawLine(windowsPen2, r.Left, r.Top, r.Right - 1, r.Top);
							windowsGraphics.DrawLine(windowsPen3, r.Right - 2, r.Top + 1, r.Right - 2, r.Bottom - 1);
							windowsGraphics.DrawLine(windowsPen3, r.Left + 1, r.Bottom - 2, r.Right - 1, r.Bottom - 2);
						}
					}
				}
			}
			r.Inflate(-1, -1);
			return r;
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x00155068 File Offset: 0x00153268
		protected ButtonState GetState()
		{
			ButtonState buttonState = ButtonState.Normal;
			if (this.Control.CheckState == CheckState.Unchecked)
			{
				buttonState |= ButtonState.Normal;
			}
			else
			{
				buttonState |= ButtonState.Checked;
			}
			if (!this.Control.Enabled)
			{
				buttonState |= ButtonState.Inactive;
			}
			if (this.Control.MouseIsDown)
			{
				buttonState |= ButtonState.Pushed;
			}
			return buttonState;
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x001550C0 File Offset: 0x001532C0
		protected void DrawCheckBox(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout)
		{
			Graphics graphics = e.Graphics;
			ButtonState state = this.GetState();
			if (this.Control.CheckState == CheckState.Indeterminate)
			{
				if (Application.RenderWithVisualStyles)
				{
					CheckBoxRenderer.DrawCheckBox(graphics, new Point(layout.checkBounds.Left, layout.checkBounds.Top), CheckBoxRenderer.ConvertFromButtonState(state, true, this.Control.MouseIsOver), this.Control.HandleInternal);
					return;
				}
				ControlPaint.DrawMixedCheckBox(graphics, layout.checkBounds, state);
				return;
			}
			else
			{
				if (Application.RenderWithVisualStyles)
				{
					CheckBoxRenderer.DrawCheckBox(graphics, new Point(layout.checkBounds.Left, layout.checkBounds.Top), CheckBoxRenderer.ConvertFromButtonState(state, false, this.Control.MouseIsOver), this.Control.HandleInternal);
					return;
				}
				ControlPaint.DrawCheckBox(graphics, layout.checkBounds, state);
				return;
			}
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x00155190 File Offset: 0x00153390
		private static Bitmap GetCheckBoxImage(Color checkColor, Rectangle fullSize, ref Color cacheCheckColor, ref Bitmap cacheCheckImage)
		{
			if (cacheCheckImage != null && cacheCheckColor.Equals(checkColor) && cacheCheckImage.Width == fullSize.Width && cacheCheckImage.Height == fullSize.Height)
			{
				return cacheCheckImage;
			}
			if (cacheCheckImage != null)
			{
				cacheCheckImage.Dispose();
				cacheCheckImage = null;
			}
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(0, 0, fullSize.Width, fullSize.Height);
			Bitmap bitmap = new Bitmap(fullSize.Width, fullSize.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.Transparent);
			IntPtr hdc = graphics.GetHdc();
			try
			{
				SafeNativeMethods.DrawFrameControl(new HandleRef(graphics, hdc), ref rect, 2, 1);
			}
			finally
			{
				graphics.ReleaseHdcInternal(hdc);
				graphics.Dispose();
			}
			bitmap.MakeTransparent();
			cacheCheckImage = bitmap;
			cacheCheckColor = checkColor;
			return cacheCheckImage;
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x0015526C File Offset: 0x0015346C
		protected void AdjustFocusRectangle(ButtonBaseAdapter.LayoutData layout)
		{
			if (AccessibilityImprovements.Level2 && string.IsNullOrEmpty(this.Control.Text))
			{
				layout.focus = (this.Control.AutoSize ? Rectangle.Inflate(layout.checkBounds, -2, -2) : layout.field);
			}
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x001552BC File Offset: 0x001534BC
		internal override ButtonBaseAdapter.LayoutOptions CommonLayout()
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = base.CommonLayout();
			layoutOptions.checkAlign = this.Control.CheckAlign;
			layoutOptions.textOffset = false;
			layoutOptions.shadowedText = !this.Control.Enabled;
			layoutOptions.layoutRTL = (RightToLeft.Yes == this.Control.RightToLeft);
			return layoutOptions;
		}

		// Token: 0x04003477 RID: 13431
		protected const int flatCheckSize = 11;

		// Token: 0x04003478 RID: 13432
		[ThreadStatic]
		private static Bitmap checkImageChecked = null;

		// Token: 0x04003479 RID: 13433
		[ThreadStatic]
		private static Color checkImageCheckedBackColor = Color.Empty;

		// Token: 0x0400347A RID: 13434
		[ThreadStatic]
		private static Bitmap checkImageIndeterminate = null;

		// Token: 0x0400347B RID: 13435
		[ThreadStatic]
		private static Color checkImageIndeterminateBackColor = Color.Empty;
	}
}
