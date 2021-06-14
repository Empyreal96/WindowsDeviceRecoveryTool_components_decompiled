using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render an option button control (also known as a radio button) with or without visual styles. This class cannot be inherited.</summary>
	// Token: 0x0200032D RID: 813
	public sealed class RadioButtonRenderer
	{
		// Token: 0x0600324C RID: 12876 RVA: 0x000027DB File Offset: 0x000009DB
		private RadioButtonRenderer()
		{
		}

		/// <summary>Gets or sets a value indicating whether the renderer uses the application state to determine rendering style.</summary>
		/// <returns>
		///     <see langword="true" /> if the application state is used to determine rendering style; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x0600324D RID: 12877 RVA: 0x000EA6F9 File Offset: 0x000E88F9
		// (set) Token: 0x0600324E RID: 12878 RVA: 0x000EA700 File Offset: 0x000E8900
		public static bool RenderMatchingApplicationState
		{
			get
			{
				return RadioButtonRenderer.renderMatchingApplicationState;
			}
			set
			{
				RadioButtonRenderer.renderMatchingApplicationState = value;
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x0600324F RID: 12879 RVA: 0x000EA708 File Offset: 0x000E8908
		private static bool RenderWithVisualStyles
		{
			get
			{
				return !RadioButtonRenderer.renderMatchingApplicationState || Application.RenderWithVisualStyles;
			}
		}

		/// <summary>Indicates whether the background of the option button (also known as a radio button) has semitransparent or alpha-blended pieces.</summary>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.RadioButtonState" /> values that specifies the visual state of the option button.</param>
		/// <returns>
		///     <see langword="true" /> if the background of the option button has semitransparent or alpha-blended pieces; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003250 RID: 12880 RVA: 0x000EA718 File Offset: 0x000E8918
		public static bool IsBackgroundPartiallyTransparent(RadioButtonState state)
		{
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				return RadioButtonRenderer.visualStyleRenderer.IsBackgroundPartiallyTransparent();
			}
			return false;
		}

		/// <summary>Draws the background of a control's parent in the specified area.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the background of the parent of <paramref name="childControl" />. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> in which to draw the parent control's background. This rectangle should be inside the child control’s bounds.</param>
		/// <param name="childControl">The control whose parent's background will be drawn.</param>
		// Token: 0x06003251 RID: 12881 RVA: 0x000EA733 File Offset: 0x000E8933
		public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl)
		{
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer(0);
				RadioButtonRenderer.visualStyleRenderer.DrawParentBackground(g, bounds, childControl);
			}
		}

		/// <summary>Draws an option button control (also known as a radio button) in the specified state and location.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the option button.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the option button glyph at.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.RadioButtonState" /> values that specifies the visual state of the option button.</param>
		// Token: 0x06003252 RID: 12882 RVA: 0x000EA74F File Offset: 0x000E894F
		public static void DrawRadioButton(Graphics g, Point glyphLocation, RadioButtonState state)
		{
			RadioButtonRenderer.DrawRadioButton(g, glyphLocation, state, IntPtr.Zero);
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x000EA760 File Offset: 0x000E8960
		internal static void DrawRadioButton(Graphics g, Point glyphLocation, RadioButtonState state, IntPtr hWnd)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, RadioButtonRenderer.GetGlyphSize(g, state, hWnd));
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				RadioButtonRenderer.visualStyleRenderer.DrawBackground(g, rectangle, hWnd);
				return;
			}
			ControlPaint.DrawRadioButton(g, rectangle, RadioButtonRenderer.ConvertToButtonState(state));
		}

		/// <summary>Draws an option button control (also known as a radio button) in the specified state and location, with the specified text, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the option button.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the option button glyph at.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="radioButtonText" /> in.</param>
		/// <param name="radioButtonText">The <see cref="T:System.String" /> to draw with the option button.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="radioButtonText" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.RadioButtonState" /> values that specifies the visual state of the option button.</param>
		// Token: 0x06003254 RID: 12884 RVA: 0x000EA7A5 File Offset: 0x000E89A5
		public static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, bool focused, RadioButtonState state)
		{
			RadioButtonRenderer.DrawRadioButton(g, glyphLocation, textBounds, radioButtonText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, focused, state);
		}

		/// <summary>Draws an option button control (also known as a radio button) in the specified state and location, with the specified text and text formatting, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the option button.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the option button glyph at.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="radioButtonText" /> in.</param>
		/// <param name="radioButtonText">The <see cref="T:System.String" /> to draw with the option button.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="radioButtonText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.RadioButtonState" /> values that specifies the visual state of the option button.</param>
		// Token: 0x06003255 RID: 12885 RVA: 0x000EA7B8 File Offset: 0x000E89B8
		public static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, TextFormatFlags flags, bool focused, RadioButtonState state)
		{
			RadioButtonRenderer.DrawRadioButton(g, glyphLocation, textBounds, radioButtonText, font, flags, focused, state, IntPtr.Zero);
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x000EA7DC File Offset: 0x000E89DC
		internal static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, TextFormatFlags flags, bool focused, RadioButtonState state, IntPtr hWnd)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, RadioButtonRenderer.GetGlyphSize(g, state, hWnd));
			Color foreColor;
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				RadioButtonRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				foreColor = RadioButtonRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				ControlPaint.DrawRadioButton(g, rectangle, RadioButtonRenderer.ConvertToButtonState(state));
				foreColor = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, radioButtonText, font, textBounds, foreColor, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, textBounds);
			}
		}

		/// <summary>Draws an option button control (also known as a radio button) in the specified state and location, with the specified text and image, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the option button.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the option button glyph at.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="radioButtonText" /> in.</param>
		/// <param name="radioButtonText">The <see cref="T:System.String" /> to draw with the option button.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="radioButtonText" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw with the option button.</param>
		/// <param name="imageBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="image" /> in.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.RadioButtonState" /> values that specifies the visual state of the option button.</param>
		// Token: 0x06003257 RID: 12887 RVA: 0x000EA854 File Offset: 0x000E8A54
		public static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, Image image, Rectangle imageBounds, bool focused, RadioButtonState state)
		{
			RadioButtonRenderer.DrawRadioButton(g, glyphLocation, textBounds, radioButtonText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, image, imageBounds, focused, state);
		}

		/// <summary>Draws an option button control (also known as a radio button) in the specified state and location; with the specified text, text formatting, and image; and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the option button.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the option button glyph at.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="radioButtonText" /> in.</param>
		/// <param name="radioButtonText">The <see cref="T:System.String" /> to draw with the option button.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="radioButtonText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw with the option button.</param>
		/// <param name="imageBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="image" /> in.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.RadioButtonState" /> values that specifies the visual state of the option button.</param>
		// Token: 0x06003258 RID: 12888 RVA: 0x000EA878 File Offset: 0x000E8A78
		public static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, TextFormatFlags flags, Image image, Rectangle imageBounds, bool focused, RadioButtonState state)
		{
			RadioButtonRenderer.DrawRadioButton(g, glyphLocation, textBounds, radioButtonText, font, flags, image, imageBounds, focused, state, IntPtr.Zero);
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x000EA8A0 File Offset: 0x000E8AA0
		internal static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, TextFormatFlags flags, Image image, Rectangle imageBounds, bool focused, RadioButtonState state, IntPtr hWnd)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, RadioButtonRenderer.GetGlyphSize(g, state, hWnd));
			Color foreColor;
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				RadioButtonRenderer.visualStyleRenderer.DrawImage(g, imageBounds, image);
				RadioButtonRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				foreColor = RadioButtonRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				g.DrawImage(image, imageBounds);
				ControlPaint.DrawRadioButton(g, rectangle, RadioButtonRenderer.ConvertToButtonState(state));
				foreColor = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, radioButtonText, font, textBounds, foreColor, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, textBounds);
			}
		}

		/// <summary>Returns the size, in pixels, of the option button (also known as a radio button) glyph.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the option button.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.RadioButtonState" /> values that specifies the visual state of the option button.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size, in pixels, of the option button glyph.</returns>
		// Token: 0x0600325A RID: 12890 RVA: 0x000EA930 File Offset: 0x000E8B30
		public static Size GetGlyphSize(Graphics g, RadioButtonState state)
		{
			return RadioButtonRenderer.GetGlyphSize(g, state, IntPtr.Zero);
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x000EA93E File Offset: 0x000E8B3E
		internal static Size GetGlyphSize(Graphics g, RadioButtonState state, IntPtr hWnd)
		{
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				return RadioButtonRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.Draw, hWnd);
			}
			return new Size(13, 13);
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x000EA964 File Offset: 0x000E8B64
		internal static ButtonState ConvertToButtonState(RadioButtonState state)
		{
			switch (state)
			{
			case RadioButtonState.UncheckedPressed:
				return ButtonState.Pushed;
			case RadioButtonState.UncheckedDisabled:
				return ButtonState.Inactive;
			case RadioButtonState.CheckedNormal:
			case RadioButtonState.CheckedHot:
				return ButtonState.Checked;
			case RadioButtonState.CheckedPressed:
				return ButtonState.Checked | ButtonState.Pushed;
			case RadioButtonState.CheckedDisabled:
				return ButtonState.Checked | ButtonState.Inactive;
			default:
				return ButtonState.Normal;
			}
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x000EA9B4 File Offset: 0x000E8BB4
		internal static RadioButtonState ConvertFromButtonState(ButtonState state, bool isHot)
		{
			if ((state & ButtonState.Checked) == ButtonState.Checked)
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return RadioButtonState.CheckedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return RadioButtonState.CheckedDisabled;
				}
				if (isHot)
				{
					return RadioButtonState.CheckedHot;
				}
				return RadioButtonState.CheckedNormal;
			}
			else
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return RadioButtonState.UncheckedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return RadioButtonState.UncheckedDisabled;
				}
				if (isHot)
				{
					return RadioButtonState.UncheckedHot;
				}
				return RadioButtonState.UncheckedNormal;
			}
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x000EAA1C File Offset: 0x000E8C1C
		private static void InitializeRenderer(int state)
		{
			int part = RadioButtonRenderer.RadioElement.Part;
			if (AccessibilityImprovements.Level2 && SystemInformation.HighContrast && (state == 8 || state == 4) && VisualStyleRenderer.IsCombinationDefined(RadioButtonRenderer.RadioElement.ClassName, VisualStyleElement.Button.RadioButton.HighContrastDisabledPart))
			{
				part = VisualStyleElement.Button.RadioButton.HighContrastDisabledPart;
			}
			if (RadioButtonRenderer.visualStyleRenderer == null)
			{
				RadioButtonRenderer.visualStyleRenderer = new VisualStyleRenderer(RadioButtonRenderer.RadioElement.ClassName, part, state);
				return;
			}
			RadioButtonRenderer.visualStyleRenderer.SetParameters(RadioButtonRenderer.RadioElement.ClassName, part, state);
		}

		// Token: 0x04001E42 RID: 7746
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x04001E43 RID: 7747
		private static readonly VisualStyleElement RadioElement = VisualStyleElement.Button.RadioButton.UncheckedNormal;

		// Token: 0x04001E44 RID: 7748
		private static bool renderMatchingApplicationState = true;
	}
}
