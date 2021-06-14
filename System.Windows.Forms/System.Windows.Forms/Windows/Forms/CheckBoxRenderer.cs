using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render a check box control with or without visual styles. This class cannot be inherited.</summary>
	// Token: 0x0200013C RID: 316
	public sealed class CheckBoxRenderer
	{
		// Token: 0x060009BD RID: 2493 RVA: 0x000027DB File Offset: 0x000009DB
		private CheckBoxRenderer()
		{
		}

		/// <summary>Gets or sets a value indicating whether the renderer uses the application state to determine rendering style.</summary>
		/// <returns>
		///     <see langword="true" /> if the application state is used to determine rendering style; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0001D468 File Offset: 0x0001B668
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0001D46F File Offset: 0x0001B66F
		public static bool RenderMatchingApplicationState
		{
			get
			{
				return CheckBoxRenderer.renderMatchingApplicationState;
			}
			set
			{
				CheckBoxRenderer.renderMatchingApplicationState = value;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0001D477 File Offset: 0x0001B677
		private static bool RenderWithVisualStyles
		{
			get
			{
				return !CheckBoxRenderer.renderMatchingApplicationState || Application.RenderWithVisualStyles;
			}
		}

		/// <summary>Indicates whether the background of the check box has semitransparent or alpha-blended pieces.</summary>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.CheckBoxState" /> values that specifies the visual state of the check box.</param>
		/// <returns>
		///     <see langword="true" /> if the background of the check box has semitransparent or alpha-blended pieces; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009C1 RID: 2497 RVA: 0x0001D487 File Offset: 0x0001B687
		public static bool IsBackgroundPartiallyTransparent(CheckBoxState state)
		{
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				return CheckBoxRenderer.visualStyleRenderer.IsBackgroundPartiallyTransparent();
			}
			return false;
		}

		/// <summary>Draws the background of a control's parent in the specified area.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the background of the parent of <paramref name="childControl" />. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> in which to draw the parent control's background. This rectangle should be inside the child control’s bounds.</param>
		/// <param name="childControl">The control whose parent's background will be drawn.</param>
		// Token: 0x060009C2 RID: 2498 RVA: 0x0001D4A2 File Offset: 0x0001B6A2
		public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl)
		{
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer(0);
				CheckBoxRenderer.visualStyleRenderer.DrawParentBackground(g, bounds, childControl);
			}
		}

		/// <summary>Draws a check box control in the specified state and location.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the check box.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the check box glyph at.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.CheckBoxState" /> values that specifies the visual state of the check box.</param>
		// Token: 0x060009C3 RID: 2499 RVA: 0x0001D4BE File Offset: 0x0001B6BE
		public static void DrawCheckBox(Graphics g, Point glyphLocation, CheckBoxState state)
		{
			CheckBoxRenderer.DrawCheckBox(g, glyphLocation, state, IntPtr.Zero);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0001D4D0 File Offset: 0x0001B6D0
		internal static void DrawCheckBox(Graphics g, Point glyphLocation, CheckBoxState state, IntPtr hWnd)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, CheckBoxRenderer.GetGlyphSize(g, state, hWnd));
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				CheckBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle, hWnd);
				return;
			}
			if (CheckBoxRenderer.IsMixed(state))
			{
				ControlPaint.DrawMixedCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				return;
			}
			ControlPaint.DrawCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
		}

		/// <summary>Draws a check box control in the specified state and location, with the specified text, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the check box.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the check box glyph at.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="checkBoxText" /> in.</param>
		/// <param name="checkBoxText">The <see cref="T:System.String" /> to draw with the check box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="checkBoxText" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.CheckBoxState" /> values that specifies the visual state of the check box.</param>
		// Token: 0x060009C5 RID: 2501 RVA: 0x0001D52B File Offset: 0x0001B72B
		public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, bool focused, CheckBoxState state)
		{
			CheckBoxRenderer.DrawCheckBox(g, glyphLocation, textBounds, checkBoxText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, focused, state);
		}

		/// <summary>Draws a check box control in the specified state and location, with the specified text and text formatting, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the check box.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the check box glyph at.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="checkBoxText" /> in.</param>
		/// <param name="checkBoxText">The <see cref="T:System.String" /> to draw with the check box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="checkBoxText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.CheckBoxState" /> values that specifies the visual state of the check box.</param>
		// Token: 0x060009C6 RID: 2502 RVA: 0x0001D540 File Offset: 0x0001B740
		public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, TextFormatFlags flags, bool focused, CheckBoxState state)
		{
			CheckBoxRenderer.DrawCheckBox(g, glyphLocation, textBounds, checkBoxText, font, flags, focused, state, IntPtr.Zero);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0001D564 File Offset: 0x0001B764
		internal static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, TextFormatFlags flags, bool focused, CheckBoxState state, IntPtr hWnd)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, CheckBoxRenderer.GetGlyphSize(g, state, hWnd));
			Color foreColor;
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				CheckBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				foreColor = CheckBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				if (CheckBoxRenderer.IsMixed(state))
				{
					ControlPaint.DrawMixedCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				}
				else
				{
					ControlPaint.DrawCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				}
				foreColor = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, checkBoxText, font, textBounds, foreColor, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, textBounds);
			}
		}

		/// <summary>Draws a check box control in the specified state and location, with the specified text and image, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the check box.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the check box glyph at.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="checkBoxText" /> in.</param>
		/// <param name="checkBoxText">The <see cref="T:System.String" /> to draw with the check box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="checkBoxText" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw with the check box.</param>
		/// <param name="imageBounds">The <see cref="T:System.Drawing.Rectangle" /> that represents the dimensions of <paramref name="image" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.CheckBoxState" /> values that specifies the visual state of the check box.</param>
		// Token: 0x060009C8 RID: 2504 RVA: 0x0001D5F4 File Offset: 0x0001B7F4
		public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, Image image, Rectangle imageBounds, bool focused, CheckBoxState state)
		{
			CheckBoxRenderer.DrawCheckBox(g, glyphLocation, textBounds, checkBoxText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, image, imageBounds, focused, state);
		}

		/// <summary>Draws a check box control in the specified state and location; with the specified text, text formatting, and image; and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the check box.</param>
		/// <param name="glyphLocation">The <see cref="T:System.Drawing.Point" /> to draw the check box glyph at.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> to draw <paramref name="checkBoxText" /> in.</param>
		/// <param name="checkBoxText">The <see cref="T:System.String" /> to draw with the check box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="checkBoxText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw with the check box.</param>
		/// <param name="imageBounds">The <see cref="T:System.Drawing.Rectangle" /> that represents the dimensions of <paramref name="image" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.CheckBoxState" /> values that specifies the visual state of the check box.</param>
		// Token: 0x060009C9 RID: 2505 RVA: 0x0001D618 File Offset: 0x0001B818
		public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, TextFormatFlags flags, Image image, Rectangle imageBounds, bool focused, CheckBoxState state)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, CheckBoxRenderer.GetGlyphSize(g, state));
			Color foreColor;
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				CheckBoxRenderer.visualStyleRenderer.DrawImage(g, imageBounds, image);
				CheckBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				foreColor = CheckBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				g.DrawImage(image, imageBounds);
				if (CheckBoxRenderer.IsMixed(state))
				{
					ControlPaint.DrawMixedCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				}
				else
				{
					ControlPaint.DrawCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				}
				foreColor = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, checkBoxText, font, textBounds, foreColor, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, textBounds);
			}
		}

		/// <summary>Returns the size of the check box glyph.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> this operation will use.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.CheckBoxState" /> values that specifies the visual state of the check box.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the check box glyph.</returns>
		// Token: 0x060009CA RID: 2506 RVA: 0x0001D6BF File Offset: 0x0001B8BF
		public static Size GetGlyphSize(Graphics g, CheckBoxState state)
		{
			return CheckBoxRenderer.GetGlyphSize(g, state, IntPtr.Zero);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0001D6CD File Offset: 0x0001B8CD
		internal static Size GetGlyphSize(Graphics g, CheckBoxState state, IntPtr hWnd)
		{
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				return CheckBoxRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.Draw, hWnd);
			}
			return new Size(13, 13);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0001D6F4 File Offset: 0x0001B8F4
		internal static ButtonState ConvertToButtonState(CheckBoxState state)
		{
			switch (state)
			{
			case CheckBoxState.UncheckedPressed:
				return ButtonState.Pushed;
			case CheckBoxState.UncheckedDisabled:
				return ButtonState.Inactive;
			case CheckBoxState.CheckedNormal:
			case CheckBoxState.CheckedHot:
				return ButtonState.Checked;
			case CheckBoxState.CheckedPressed:
				return ButtonState.Checked | ButtonState.Pushed;
			case CheckBoxState.CheckedDisabled:
				return ButtonState.Checked | ButtonState.Inactive;
			case CheckBoxState.MixedNormal:
			case CheckBoxState.MixedHot:
				return ButtonState.Checked;
			case CheckBoxState.MixedPressed:
				return ButtonState.Checked | ButtonState.Pushed;
			case CheckBoxState.MixedDisabled:
				return ButtonState.Checked | ButtonState.Inactive;
			default:
				return ButtonState.Normal;
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0001D764 File Offset: 0x0001B964
		internal static CheckBoxState ConvertFromButtonState(ButtonState state, bool isMixed, bool isHot)
		{
			if (isMixed)
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return CheckBoxState.MixedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return CheckBoxState.MixedDisabled;
				}
				if (isHot)
				{
					return CheckBoxState.MixedHot;
				}
				return CheckBoxState.MixedNormal;
			}
			else if ((state & ButtonState.Checked) == ButtonState.Checked)
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return CheckBoxState.CheckedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return CheckBoxState.CheckedDisabled;
				}
				if (isHot)
				{
					return CheckBoxState.CheckedHot;
				}
				return CheckBoxState.CheckedNormal;
			}
			else
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return CheckBoxState.UncheckedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return CheckBoxState.UncheckedDisabled;
				}
				if (isHot)
				{
					return CheckBoxState.UncheckedHot;
				}
				return CheckBoxState.UncheckedNormal;
			}
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0001D7FA File Offset: 0x0001B9FA
		private static bool IsMixed(CheckBoxState state)
		{
			return state - CheckBoxState.MixedNormal <= 3;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0001D806 File Offset: 0x0001BA06
		private static bool IsDisabled(CheckBoxState state)
		{
			return state == CheckBoxState.UncheckedDisabled || state == CheckBoxState.CheckedDisabled || state == CheckBoxState.MixedDisabled;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0001D818 File Offset: 0x0001BA18
		private static void InitializeRenderer(int state)
		{
			int part = CheckBoxRenderer.CheckBoxElement.Part;
			if (AccessibilityImprovements.Level2 && SystemInformation.HighContrast && CheckBoxRenderer.IsDisabled((CheckBoxState)state) && VisualStyleRenderer.IsCombinationDefined(CheckBoxRenderer.CheckBoxElement.ClassName, VisualStyleElement.Button.CheckBox.HighContrastDisabledPart))
			{
				part = VisualStyleElement.Button.CheckBox.HighContrastDisabledPart;
			}
			if (CheckBoxRenderer.visualStyleRenderer == null)
			{
				CheckBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(CheckBoxRenderer.CheckBoxElement.ClassName, part, state);
				return;
			}
			CheckBoxRenderer.visualStyleRenderer.SetParameters(CheckBoxRenderer.CheckBoxElement.ClassName, part, state);
		}

		// Token: 0x040006B8 RID: 1720
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x040006B9 RID: 1721
		private static readonly VisualStyleElement CheckBoxElement = VisualStyleElement.Button.CheckBox.UncheckedNormal;

		// Token: 0x040006BA RID: 1722
		private static bool renderMatchingApplicationState = true;
	}
}
