using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render a combo box control with visual styles. This class cannot be inherited.</summary>
	// Token: 0x02000150 RID: 336
	public sealed class ComboBoxRenderer
	{
		// Token: 0x06000B56 RID: 2902 RVA: 0x000027DB File Offset: 0x000009DB
		private ComboBoxRenderer()
		{
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ComboBoxRenderer" /> class can be used to draw a combo box with visual styles.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has enabled visual styles in the operating system and visual styles are applied to the client area of application windows; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x00023BA7 File Offset: 0x00021DA7
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00023BB0 File Offset: 0x00021DB0
		private static void DrawBackground(Graphics g, Rectangle bounds, ComboBoxState state)
		{
			ComboBoxRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			if (state != ComboBoxState.Disabled)
			{
				Color color = ComboBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.FillColor);
				if (color != SystemColors.Window)
				{
					Rectangle backgroundContentRectangle = ComboBoxRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
					backgroundContentRectangle.Inflate(-2, -2);
					g.FillRectangle(SystemBrushes.Window, backgroundContentRectangle);
				}
			}
		}

		/// <summary>Draws a text box in the specified state and bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ComboBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06000B59 RID: 2905 RVA: 0x00023C10 File Offset: 0x00021E10
		public static void DrawTextBox(Graphics g, Rectangle bounds, ComboBoxState state)
		{
			if (ComboBoxRenderer.visualStyleRenderer == null)
			{
				ComboBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			else
			{
				ComboBoxRenderer.visualStyleRenderer.SetParameters(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			ComboBoxRenderer.DrawBackground(g, bounds, state);
		}

		/// <summary>Draws a text box in the specified state and bounds, with the specified text.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="comboBoxText">The <see cref="T:System.String" /> to draw in the text box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="comboBoxText" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ComboBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06000B5A RID: 2906 RVA: 0x00023C6C File Offset: 0x00021E6C
		public static void DrawTextBox(Graphics g, Rectangle bounds, string comboBoxText, Font font, ComboBoxState state)
		{
			ComboBoxRenderer.DrawTextBox(g, bounds, comboBoxText, font, TextFormatFlags.TextBoxControl, state);
		}

		/// <summary>Draws a text box in the specified state and bounds, with the specified text and text bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="comboBoxText">The <see cref="T:System.String" /> to draw in the text box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="comboBoxText" />.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds in which to draw <paramref name="comboBoxText" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ComboBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06000B5B RID: 2907 RVA: 0x00023C7E File Offset: 0x00021E7E
		public static void DrawTextBox(Graphics g, Rectangle bounds, string comboBoxText, Font font, Rectangle textBounds, ComboBoxState state)
		{
			ComboBoxRenderer.DrawTextBox(g, bounds, comboBoxText, font, textBounds, TextFormatFlags.TextBoxControl, state);
		}

		/// <summary>Draws a text box in the specified state and bounds, with the specified text and text formatting.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="comboBoxText">The <see cref="T:System.String" /> to draw in the text box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="comboBoxText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ComboBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06000B5C RID: 2908 RVA: 0x00023C94 File Offset: 0x00021E94
		public static void DrawTextBox(Graphics g, Rectangle bounds, string comboBoxText, Font font, TextFormatFlags flags, ComboBoxState state)
		{
			if (ComboBoxRenderer.visualStyleRenderer == null)
			{
				ComboBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			else
			{
				ComboBoxRenderer.visualStyleRenderer.SetParameters(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			Rectangle backgroundContentRectangle = ComboBoxRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
			backgroundContentRectangle.Inflate(-2, -2);
			ComboBoxRenderer.DrawTextBox(g, bounds, comboBoxText, font, backgroundContentRectangle, flags, state);
		}

		/// <summary>Draws a text box in the specified state and bounds, with the specified text, text formatting, and text bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="comboBoxText">The <see cref="T:System.String" /> to draw in the text box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="comboBoxText" />.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds in which to draw <paramref name="comboBoxText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ComboBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06000B5D RID: 2909 RVA: 0x00023D10 File Offset: 0x00021F10
		public static void DrawTextBox(Graphics g, Rectangle bounds, string comboBoxText, Font font, Rectangle textBounds, TextFormatFlags flags, ComboBoxState state)
		{
			if (ComboBoxRenderer.visualStyleRenderer == null)
			{
				ComboBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			else
			{
				ComboBoxRenderer.visualStyleRenderer.SetParameters(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			ComboBoxRenderer.DrawBackground(g, bounds, state);
			Color color = ComboBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			TextRenderer.DrawText(g, comboBoxText, font, textBounds, color, flags);
		}

		/// <summary>Draws a drop-down arrow with the current visual style of the operating system.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the drop-down arrow.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the drop-down arrow.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ComboBoxState" /> values that specifies the visual state of the drop-down arrow.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06000B5E RID: 2910 RVA: 0x00023D8C File Offset: 0x00021F8C
		public static void DrawDropDownButton(Graphics g, Rectangle bounds, ComboBoxState state)
		{
			ComboBoxRenderer.DrawDropDownButtonForHandle(g, bounds, state, IntPtr.Zero);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00023D9C File Offset: 0x00021F9C
		internal static void DrawDropDownButtonForHandle(Graphics g, Rectangle bounds, ComboBoxState state, IntPtr handle)
		{
			if (ComboBoxRenderer.visualStyleRenderer == null)
			{
				ComboBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(ComboBoxRenderer.ComboBoxElement.ClassName, ComboBoxRenderer.ComboBoxElement.Part, (int)state);
			}
			else
			{
				ComboBoxRenderer.visualStyleRenderer.SetParameters(ComboBoxRenderer.ComboBoxElement.ClassName, ComboBoxRenderer.ComboBoxElement.Part, (int)state);
			}
			ComboBoxRenderer.visualStyleRenderer.DrawBackground(g, bounds, handle);
		}

		// Token: 0x04000731 RID: 1841
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x04000732 RID: 1842
		private static readonly VisualStyleElement ComboBoxElement = VisualStyleElement.ComboBox.DropDownButton.Normal;

		// Token: 0x04000733 RID: 1843
		private static readonly VisualStyleElement TextBoxElement = VisualStyleElement.TextBox.TextEdit.Normal;
	}
}
