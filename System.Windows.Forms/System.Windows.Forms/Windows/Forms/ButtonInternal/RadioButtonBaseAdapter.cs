using System;
using System.Drawing;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x020004D1 RID: 1233
	internal abstract class RadioButtonBaseAdapter : CheckableControlBaseAdapter
	{
		// Token: 0x060051E3 RID: 20963 RVA: 0x00154A36 File Offset: 0x00152C36
		internal RadioButtonBaseAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x060051E4 RID: 20964 RVA: 0x00155D08 File Offset: 0x00153F08
		protected new RadioButton Control
		{
			get
			{
				return (RadioButton)base.Control;
			}
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x00155D15 File Offset: 0x00153F15
		protected void DrawCheckFlat(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout, Color checkColor, Color checkBackground, Color checkBorder)
		{
			this.DrawCheckBackgroundFlat(e, layout.checkBounds, checkBorder, checkBackground);
			this.DrawCheckOnly(e, layout, checkColor, checkBackground, true);
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x00155D34 File Offset: 0x00153F34
		protected void DrawCheckBackground3DLite(PaintEventArgs e, Rectangle bounds, Color checkColor, Color checkBackground, ButtonBaseAdapter.ColorData colors, bool disabledColors)
		{
			Graphics graphics = e.Graphics;
			Color color = checkBackground;
			if (!this.Control.Enabled && disabledColors)
			{
				color = SystemColors.Control;
			}
			using (Brush brush = new SolidBrush(color))
			{
				using (Pen pen = new Pen(colors.buttonShadow))
				{
					using (Pen pen2 = new Pen(colors.buttonFace))
					{
						using (Pen pen3 = new Pen(colors.highlight))
						{
							int num = bounds.Width;
							bounds.Width = num - 1;
							num = bounds.Height;
							bounds.Height = num - 1;
							graphics.DrawPie(pen, bounds, 136f, 88f);
							graphics.DrawPie(pen, bounds, 226f, 88f);
							graphics.DrawPie(pen3, bounds, 316f, 88f);
							graphics.DrawPie(pen3, bounds, 46f, 88f);
							bounds.Inflate(-1, -1);
							graphics.FillEllipse(brush, bounds);
							graphics.DrawEllipse(pen2, bounds);
						}
					}
				}
			}
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x00155E80 File Offset: 0x00154080
		protected void DrawCheckBackgroundFlat(PaintEventArgs e, Rectangle bounds, Color borderColor, Color checkBackground)
		{
			Color color = checkBackground;
			Color color2 = borderColor;
			if (!this.Control.Enabled)
			{
				if (!SystemInformation.HighContrast || !AccessibilityImprovements.Level1)
				{
					color2 = ControlPaint.ContrastControlDark;
				}
				color = SystemColors.Control;
			}
			double dpiScaleRatio = base.GetDpiScaleRatio(e.Graphics);
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(e.Graphics))
			{
				using (WindowsPen windowsPen = new WindowsPen(windowsGraphics.DeviceContext, color2))
				{
					using (WindowsBrush windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, color))
					{
						if (dpiScaleRatio > 1.1)
						{
							int num = bounds.Width;
							bounds.Width = num - 1;
							num = bounds.Height;
							bounds.Height = num - 1;
							windowsGraphics.DrawAndFillEllipse(windowsPen, windowsBrush, bounds);
							bounds.Inflate(-1, -1);
						}
						else
						{
							RadioButtonBaseAdapter.DrawAndFillEllipse(windowsGraphics, windowsPen, windowsBrush, bounds);
						}
					}
				}
			}
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x00155F90 File Offset: 0x00154190
		private static void DrawAndFillEllipse(WindowsGraphics wg, WindowsPen borderPen, WindowsBrush fieldBrush, Rectangle bounds)
		{
			if (wg == null)
			{
				return;
			}
			wg.FillRectangle(fieldBrush, new Rectangle(bounds.X + 2, bounds.Y + 2, 8, 8));
			wg.FillRectangle(fieldBrush, new Rectangle(bounds.X + 4, bounds.Y + 1, 4, 10));
			wg.FillRectangle(fieldBrush, new Rectangle(bounds.X + 1, bounds.Y + 4, 10, 4));
			wg.DrawLine(borderPen, new Point(bounds.X + 4, bounds.Y), new Point(bounds.X + 8, bounds.Y));
			wg.DrawLine(borderPen, new Point(bounds.X + 4, bounds.Y + 11), new Point(bounds.X + 8, bounds.Y + 11));
			wg.DrawLine(borderPen, new Point(bounds.X + 2, bounds.Y + 1), new Point(bounds.X + 4, bounds.Y + 1));
			wg.DrawLine(borderPen, new Point(bounds.X + 8, bounds.Y + 1), new Point(bounds.X + 10, bounds.Y + 1));
			wg.DrawLine(borderPen, new Point(bounds.X + 2, bounds.Y + 10), new Point(bounds.X + 4, bounds.Y + 10));
			wg.DrawLine(borderPen, new Point(bounds.X + 8, bounds.Y + 10), new Point(bounds.X + 10, bounds.Y + 10));
			wg.DrawLine(borderPen, new Point(bounds.X, bounds.Y + 4), new Point(bounds.X, bounds.Y + 8));
			wg.DrawLine(borderPen, new Point(bounds.X + 11, bounds.Y + 4), new Point(bounds.X + 11, bounds.Y + 8));
			wg.DrawLine(borderPen, new Point(bounds.X + 1, bounds.Y + 2), new Point(bounds.X + 1, bounds.Y + 4));
			wg.DrawLine(borderPen, new Point(bounds.X + 1, bounds.Y + 8), new Point(bounds.X + 1, bounds.Y + 10));
			wg.DrawLine(borderPen, new Point(bounds.X + 10, bounds.Y + 2), new Point(bounds.X + 10, bounds.Y + 4));
			wg.DrawLine(borderPen, new Point(bounds.X + 10, bounds.Y + 8), new Point(bounds.X + 10, bounds.Y + 10));
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x00156287 File Offset: 0x00154487
		private static int GetScaledNumber(int n, double scale)
		{
			return (int)((double)n * scale);
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x00156290 File Offset: 0x00154490
		protected void DrawCheckOnly(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout, Color checkColor, Color checkBackground, bool disabledColors)
		{
			if (this.Control.Checked)
			{
				if (!this.Control.Enabled && disabledColors)
				{
					checkColor = SystemColors.ControlDark;
				}
				double dpiScaleRatio = base.GetDpiScaleRatio(e.Graphics);
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(e.Graphics))
				{
					using (WindowsBrush windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, checkColor))
					{
						int num = 5;
						Rectangle rect = new Rectangle(layout.checkBounds.X + RadioButtonBaseAdapter.GetScaledNumber(num, dpiScaleRatio), layout.checkBounds.Y + RadioButtonBaseAdapter.GetScaledNumber(num - 1, dpiScaleRatio), RadioButtonBaseAdapter.GetScaledNumber(2, dpiScaleRatio), RadioButtonBaseAdapter.GetScaledNumber(4, dpiScaleRatio));
						windowsGraphics.FillRectangle(windowsBrush, rect);
						Rectangle rect2 = new Rectangle(layout.checkBounds.X + RadioButtonBaseAdapter.GetScaledNumber(num - 1, dpiScaleRatio), layout.checkBounds.Y + RadioButtonBaseAdapter.GetScaledNumber(num, dpiScaleRatio), RadioButtonBaseAdapter.GetScaledNumber(4, dpiScaleRatio), RadioButtonBaseAdapter.GetScaledNumber(2, dpiScaleRatio));
						windowsGraphics.FillRectangle(windowsBrush, rect2);
					}
				}
			}
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x001563B0 File Offset: 0x001545B0
		protected ButtonState GetState()
		{
			ButtonState buttonState = ButtonState.Normal;
			if (this.Control.Checked)
			{
				buttonState |= ButtonState.Checked;
			}
			else
			{
				buttonState |= ButtonState.Normal;
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

		// Token: 0x060051EC RID: 20972 RVA: 0x00156408 File Offset: 0x00154608
		protected void DrawCheckBox(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout)
		{
			Graphics graphics = e.Graphics;
			Rectangle checkBounds = layout.checkBounds;
			if (!Application.RenderWithVisualStyles)
			{
				int x = checkBounds.X;
				checkBounds.X = x - 1;
			}
			ButtonState state = this.GetState();
			if (Application.RenderWithVisualStyles)
			{
				RadioButtonRenderer.DrawRadioButton(graphics, new Point(checkBounds.Left, checkBounds.Top), RadioButtonRenderer.ConvertFromButtonState(state, this.Control.MouseIsOver), this.Control.HandleInternal);
				return;
			}
			ControlPaint.DrawRadioButton(graphics, checkBounds, state);
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x00156487 File Offset: 0x00154687
		protected void AdjustFocusRectangle(ButtonBaseAdapter.LayoutData layout)
		{
			if (AccessibilityImprovements.Level2 && string.IsNullOrEmpty(this.Control.Text))
			{
				layout.focus = (this.Control.AutoSize ? layout.checkBounds : layout.field);
			}
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x001564C4 File Offset: 0x001546C4
		internal override ButtonBaseAdapter.LayoutOptions CommonLayout()
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = base.CommonLayout();
			layoutOptions.checkAlign = this.Control.CheckAlign;
			return layoutOptions;
		}
	}
}
