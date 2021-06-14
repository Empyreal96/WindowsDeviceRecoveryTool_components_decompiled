using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render a text box control with visual styles. This class cannot be inherited.</summary>
	// Token: 0x02000392 RID: 914
	public sealed class TextBoxRenderer
	{
		// Token: 0x06003A04 RID: 14852 RVA: 0x000027DB File Offset: 0x000009DB
		private TextBoxRenderer()
		{
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.TextBoxRenderer" /> class can be used to draw a text box with visual styles.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has enabled visual styles in the operating system and visual styles are applied to the client area of application windows; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06003A05 RID: 14853 RVA: 0x00023BA7 File Offset: 0x00021DA7
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x001018E8 File Offset: 0x000FFAE8
		private static void DrawBackground(Graphics g, Rectangle bounds, TextBoxState state)
		{
			TextBoxRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			if (state != TextBoxState.Disabled)
			{
				Color color = TextBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.FillColor);
				if (color != SystemColors.Window)
				{
					Rectangle backgroundContentRectangle = TextBoxRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
					using (SolidBrush solidBrush = new SolidBrush(SystemColors.Window))
					{
						g.FillRectangle(solidBrush, backgroundContentRectangle);
					}
				}
			}
		}

		/// <summary>Draws a text box control in the specified state and bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TextBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003A07 RID: 14855 RVA: 0x00101960 File Offset: 0x000FFB60
		public static void DrawTextBox(Graphics g, Rectangle bounds, TextBoxState state)
		{
			TextBoxRenderer.InitializeRenderer((int)state);
			TextBoxRenderer.DrawBackground(g, bounds, state);
		}

		/// <summary>Draws a text box control in the specified state and bounds, and with the specified text.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="textBoxText">The <see cref="T:System.String" /> to draw in the text box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="textBoxText" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TextBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003A08 RID: 14856 RVA: 0x00101970 File Offset: 0x000FFB70
		public static void DrawTextBox(Graphics g, Rectangle bounds, string textBoxText, Font font, TextBoxState state)
		{
			TextBoxRenderer.DrawTextBox(g, bounds, textBoxText, font, TextFormatFlags.TextBoxControl, state);
		}

		/// <summary>Draws a text box control in the specified state and bounds, and with the specified text and text bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="textBoxText">The <see cref="T:System.String" /> to draw in the text box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="textBoxText" />.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of <paramref name="textBoxText" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TextBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003A09 RID: 14857 RVA: 0x00101982 File Offset: 0x000FFB82
		public static void DrawTextBox(Graphics g, Rectangle bounds, string textBoxText, Font font, Rectangle textBounds, TextBoxState state)
		{
			TextBoxRenderer.DrawTextBox(g, bounds, textBoxText, font, textBounds, TextFormatFlags.TextBoxControl, state);
		}

		/// <summary>Draws a text box control in the specified state and bounds, and with the specified text and text formatting.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="textBoxText">The <see cref="T:System.String" /> to draw in the text box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="textBoxText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TextBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003A0A RID: 14858 RVA: 0x00101998 File Offset: 0x000FFB98
		public static void DrawTextBox(Graphics g, Rectangle bounds, string textBoxText, Font font, TextFormatFlags flags, TextBoxState state)
		{
			TextBoxRenderer.InitializeRenderer((int)state);
			Rectangle backgroundContentRectangle = TextBoxRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
			backgroundContentRectangle.Inflate(-2, -2);
			TextBoxRenderer.DrawTextBox(g, bounds, textBoxText, font, backgroundContentRectangle, flags, state);
		}

		/// <summary>Draws a text box control in the specified state and bounds, and with the specified text, text bounds, and text formatting.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the text box.</param>
		/// <param name="textBoxText">The <see cref="T:System.String" /> to draw in the text box.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="textBoxText" />.</param>
		/// <param name="textBounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of <paramref name="textBoxText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TextBoxState" /> values that specifies the visual state of the text box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003A0B RID: 14859 RVA: 0x001019D4 File Offset: 0x000FFBD4
		public static void DrawTextBox(Graphics g, Rectangle bounds, string textBoxText, Font font, Rectangle textBounds, TextFormatFlags flags, TextBoxState state)
		{
			TextBoxRenderer.InitializeRenderer((int)state);
			TextBoxRenderer.DrawBackground(g, bounds, state);
			Color color = TextBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			TextRenderer.DrawText(g, textBoxText, font, textBounds, color, flags);
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x00101A10 File Offset: 0x000FFC10
		private static void InitializeRenderer(int state)
		{
			if (TextBoxRenderer.visualStyleRenderer == null)
			{
				TextBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(TextBoxRenderer.TextBoxElement.ClassName, TextBoxRenderer.TextBoxElement.Part, state);
				return;
			}
			TextBoxRenderer.visualStyleRenderer.SetParameters(TextBoxRenderer.TextBoxElement.ClassName, TextBoxRenderer.TextBoxElement.Part, state);
		}

		// Token: 0x040022AE RID: 8878
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x040022AF RID: 8879
		private static readonly VisualStyleElement TextBoxElement = VisualStyleElement.TextBox.TextEdit.Normal;
	}
}
