using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render a group box control with or without visual styles. This class cannot be inherited.</summary>
	// Token: 0x0200025D RID: 605
	public sealed class GroupBoxRenderer
	{
		// Token: 0x06002496 RID: 9366 RVA: 0x000027DB File Offset: 0x000009DB
		private GroupBoxRenderer()
		{
		}

		/// <summary>Gets or sets a value indicating whether the renderer uses the application state to determine rendering style.</summary>
		/// <returns>
		///     <see langword="true" /> if the application state is used to determine rendering style; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06002497 RID: 9367 RVA: 0x000B1718 File Offset: 0x000AF918
		// (set) Token: 0x06002498 RID: 9368 RVA: 0x000B171F File Offset: 0x000AF91F
		public static bool RenderMatchingApplicationState
		{
			get
			{
				return GroupBoxRenderer.renderMatchingApplicationState;
			}
			set
			{
				GroupBoxRenderer.renderMatchingApplicationState = value;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x000B1727 File Offset: 0x000AF927
		private static bool RenderWithVisualStyles
		{
			get
			{
				return !GroupBoxRenderer.renderMatchingApplicationState || Application.RenderWithVisualStyles;
			}
		}

		/// <summary>Indicates whether the background of the group box has any semitransparent or alpha-blended pieces.</summary>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.GroupBoxState" /> values that specifies the visual state of the group box.</param>
		/// <returns>
		///     <see langword="true" /> if the background of the group box has semitransparent or alpha-blended pieces; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600249A RID: 9370 RVA: 0x000B1737 File Offset: 0x000AF937
		public static bool IsBackgroundPartiallyTransparent(GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.InitializeRenderer((int)state);
				return GroupBoxRenderer.visualStyleRenderer.IsBackgroundPartiallyTransparent();
			}
			return false;
		}

		/// <summary>Draws the background of a control's parent in the specified area.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the background of the parent of <paramref name="childControl" />.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> in which to draw the parent control's background. This rectangle should be inside the child control’s bounds.</param>
		/// <param name="childControl">The control whose parent's background will be drawn.</param>
		// Token: 0x0600249B RID: 9371 RVA: 0x000B1752 File Offset: 0x000AF952
		public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.InitializeRenderer(0);
				GroupBoxRenderer.visualStyleRenderer.DrawParentBackground(g, bounds, childControl);
			}
		}

		/// <summary>Draws a group box control in the specified state and bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the group box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the group box.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.GroupBoxState" /> values that specifies the visual state of the group box.</param>
		// Token: 0x0600249C RID: 9372 RVA: 0x000B176E File Offset: 0x000AF96E
		public static void DrawGroupBox(Graphics g, Rectangle bounds, GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.DrawThemedGroupBoxNoText(g, bounds, state);
				return;
			}
			GroupBoxRenderer.DrawUnthemedGroupBoxNoText(g, bounds, state);
		}

		/// <summary>Draws a group box control in the specified state and bounds, with the specified text and font.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the group box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the group box.</param>
		/// <param name="groupBoxText">The <see cref="T:System.String" /> to draw with the group box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="groupBoxText" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.GroupBoxState" /> values that specifies the visual state of the group box.</param>
		// Token: 0x0600249D RID: 9373 RVA: 0x000B1788 File Offset: 0x000AF988
		public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, GroupBoxState state)
		{
			GroupBoxRenderer.DrawGroupBox(g, bounds, groupBoxText, font, TextFormatFlags.Default, state);
		}

		/// <summary>Draws a group box control in the specified state and bounds, with the specified text, font, and color.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the group box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the group box.</param>
		/// <param name="groupBoxText">The <see cref="T:System.String" /> to draw with the group box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="groupBoxText" />.</param>
		/// <param name="textColor">The <see cref="T:System.Drawing.Color" /> to apply to <paramref name="groupBoxText" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.GroupBoxState" /> values that specifies the visual state of the group box.</param>
		// Token: 0x0600249E RID: 9374 RVA: 0x000B1796 File Offset: 0x000AF996
		public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, Color textColor, GroupBoxState state)
		{
			GroupBoxRenderer.DrawGroupBox(g, bounds, groupBoxText, font, textColor, TextFormatFlags.Default, state);
		}

		/// <summary>Draws a group box control in the specified state and bounds, with the specified text, font, and text formatting.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the group box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the group box.</param>
		/// <param name="groupBoxText">The <see cref="T:System.String" /> to draw with the group box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="groupBoxText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.GroupBoxState" /> values that specifies the visual state of the group box.</param>
		// Token: 0x0600249F RID: 9375 RVA: 0x000B17A6 File Offset: 0x000AF9A6
		public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, TextFormatFlags flags, GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.DrawThemedGroupBoxWithText(g, bounds, groupBoxText, font, GroupBoxRenderer.DefaultTextColor(state), flags, state);
				return;
			}
			GroupBoxRenderer.DrawUnthemedGroupBoxWithText(g, bounds, groupBoxText, font, GroupBoxRenderer.DefaultTextColor(state), flags, state);
		}

		/// <summary>Draws a group box control in the specified state and bounds, with the specified text, font, color, and text formatting.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the group box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the group box.</param>
		/// <param name="groupBoxText">The <see cref="T:System.String" /> to draw with the group box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="groupBoxText" />.</param>
		/// <param name="textColor">The <see cref="T:System.Drawing.Color" /> to apply to <paramref name="groupBoxText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.GroupBoxState" /> values that specifies the visual state of the group box.</param>
		// Token: 0x060024A0 RID: 9376 RVA: 0x000B17D8 File Offset: 0x000AF9D8
		public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, Color textColor, TextFormatFlags flags, GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.DrawThemedGroupBoxWithText(g, bounds, groupBoxText, font, textColor, flags, state);
				return;
			}
			GroupBoxRenderer.DrawUnthemedGroupBoxWithText(g, bounds, groupBoxText, font, textColor, flags, state);
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000B1800 File Offset: 0x000AFA00
		private static void DrawThemedGroupBoxNoText(Graphics g, Rectangle bounds, GroupBoxState state)
		{
			GroupBoxRenderer.InitializeRenderer((int)state);
			GroupBoxRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000B1814 File Offset: 0x000AFA14
		private static void DrawThemedGroupBoxWithText(Graphics g, Rectangle bounds, string groupBoxText, Font font, Color textColor, TextFormatFlags flags, GroupBoxState state)
		{
			GroupBoxRenderer.InitializeRenderer((int)state);
			Rectangle bounds2 = bounds;
			bounds2.Width -= 14;
			Size size = TextRenderer.MeasureText(g, groupBoxText, font, new Size(bounds2.Width, bounds2.Height), flags);
			bounds2.Width = size.Width;
			bounds2.Height = size.Height;
			if ((flags & TextFormatFlags.Right) == TextFormatFlags.Right)
			{
				bounds2.X = bounds.Right - bounds2.Width - 7 + 1;
			}
			else
			{
				bounds2.X += 6;
			}
			TextRenderer.DrawText(g, groupBoxText, font, bounds2, textColor, flags);
			Rectangle rectangle = bounds;
			rectangle.Y += font.Height / 2;
			rectangle.Height -= font.Height / 2;
			Rectangle clipRectangle = rectangle;
			Rectangle clipRectangle2 = rectangle;
			Rectangle clipRectangle3 = rectangle;
			clipRectangle.Width = 7;
			clipRectangle2.Width = Math.Max(0, bounds2.Width - 3);
			if ((flags & TextFormatFlags.Right) == TextFormatFlags.Right)
			{
				clipRectangle.X = rectangle.Right - 7;
				clipRectangle2.X = clipRectangle.Left - clipRectangle2.Width;
				clipRectangle3.Width = clipRectangle2.X - rectangle.X;
			}
			else
			{
				clipRectangle2.X = clipRectangle.Right;
				clipRectangle3.X = clipRectangle2.Right;
				clipRectangle3.Width = rectangle.Right - clipRectangle3.X;
			}
			clipRectangle2.Y = bounds2.Bottom;
			clipRectangle2.Height -= bounds2.Bottom - rectangle.Top;
			GroupBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle, clipRectangle);
			GroupBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle, clipRectangle2);
			GroupBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle, clipRectangle3);
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000B19D4 File Offset: 0x000AFBD4
		private static void DrawUnthemedGroupBoxNoText(Graphics g, Rectangle bounds, GroupBoxState state)
		{
			Color control = SystemColors.Control;
			Pen pen = new Pen(ControlPaint.Light(control, 1f));
			Pen pen2 = new Pen(ControlPaint.Dark(control, 0f));
			try
			{
				g.DrawLine(pen, bounds.Left + 1, bounds.Top + 1, bounds.Left + 1, bounds.Height - 1);
				g.DrawLine(pen2, bounds.Left, bounds.Top + 1, bounds.Left, bounds.Height - 2);
				g.DrawLine(pen, bounds.Left, bounds.Height - 1, bounds.Width - 1, bounds.Height - 1);
				g.DrawLine(pen2, bounds.Left, bounds.Height - 2, bounds.Width - 1, bounds.Height - 2);
				g.DrawLine(pen, bounds.Left + 1, bounds.Top + 1, bounds.Width - 1, bounds.Top + 1);
				g.DrawLine(pen2, bounds.Left, bounds.Top, bounds.Width - 2, bounds.Top);
				g.DrawLine(pen, bounds.Width - 1, bounds.Top, bounds.Width - 1, bounds.Height - 1);
				g.DrawLine(pen2, bounds.Width - 2, bounds.Top, bounds.Width - 2, bounds.Height - 2);
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (pen2 != null)
				{
					pen2.Dispose();
				}
			}
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000B1B80 File Offset: 0x000AFD80
		private static void DrawUnthemedGroupBoxWithText(Graphics g, Rectangle bounds, string groupBoxText, Font font, Color textColor, TextFormatFlags flags, GroupBoxState state)
		{
			Rectangle bounds2 = bounds;
			bounds2.Width -= 8;
			Size size = TextRenderer.MeasureText(g, groupBoxText, font, new Size(bounds2.Width, bounds2.Height), flags);
			bounds2.Width = size.Width;
			bounds2.Height = size.Height;
			if ((flags & TextFormatFlags.Right) == TextFormatFlags.Right)
			{
				bounds2.X = bounds.Right - bounds2.Width - 8;
			}
			else
			{
				bounds2.X += 8;
			}
			TextRenderer.DrawText(g, groupBoxText, font, bounds2, textColor, flags);
			if (bounds2.Width > 0)
			{
				bounds2.Inflate(2, 0);
			}
			Pen pen = new Pen(SystemColors.ControlLight);
			Pen pen2 = new Pen(SystemColors.ControlDark);
			int num = bounds.Top + font.Height / 2;
			g.DrawLine(pen, bounds.Left + 1, num, bounds.Left + 1, bounds.Height - 1);
			g.DrawLine(pen2, bounds.Left, num - 1, bounds.Left, bounds.Height - 2);
			g.DrawLine(pen, bounds.Left, bounds.Height - 1, bounds.Width, bounds.Height - 1);
			g.DrawLine(pen2, bounds.Left, bounds.Height - 2, bounds.Width - 1, bounds.Height - 2);
			g.DrawLine(pen, bounds.Left + 1, num, bounds2.X - 2, num);
			g.DrawLine(pen2, bounds.Left, num - 1, bounds2.X - 3, num - 1);
			g.DrawLine(pen, bounds2.X + bounds2.Width + 1, num, bounds.Width - 1, num);
			g.DrawLine(pen2, bounds2.X + bounds2.Width + 2, num - 1, bounds.Width - 2, num - 1);
			g.DrawLine(pen, bounds.Width - 1, num, bounds.Width - 1, bounds.Height - 1);
			g.DrawLine(pen2, bounds.Width - 2, num - 1, bounds.Width - 2, bounds.Height - 2);
			pen.Dispose();
			pen2.Dispose();
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000B1DC6 File Offset: 0x000AFFC6
		private static Color DefaultTextColor(GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.InitializeRenderer((int)state);
				return GroupBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			return SystemColors.ControlText;
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000B1DEC File Offset: 0x000AFFEC
		private static void InitializeRenderer(int state)
		{
			int part = GroupBoxRenderer.GroupBoxElement.Part;
			if (AccessibilityImprovements.Level2 && SystemInformation.HighContrast && state == 2 && VisualStyleRenderer.IsCombinationDefined(GroupBoxRenderer.GroupBoxElement.ClassName, VisualStyleElement.Button.GroupBox.HighContrastDisabledPart))
			{
				part = VisualStyleElement.Button.GroupBox.HighContrastDisabledPart;
			}
			if (GroupBoxRenderer.visualStyleRenderer == null)
			{
				GroupBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(GroupBoxRenderer.GroupBoxElement.ClassName, part, state);
				return;
			}
			GroupBoxRenderer.visualStyleRenderer.SetParameters(GroupBoxRenderer.GroupBoxElement.ClassName, part, state);
		}

		// Token: 0x04000FA7 RID: 4007
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x04000FA8 RID: 4008
		private static readonly VisualStyleElement GroupBoxElement = VisualStyleElement.Button.GroupBox.Normal;

		// Token: 0x04000FA9 RID: 4009
		private const int textOffset = 8;

		// Token: 0x04000FAA RID: 4010
		private const int boxHeaderWidth = 7;

		// Token: 0x04000FAB RID: 4011
		private static bool renderMatchingApplicationState = true;
	}
}
