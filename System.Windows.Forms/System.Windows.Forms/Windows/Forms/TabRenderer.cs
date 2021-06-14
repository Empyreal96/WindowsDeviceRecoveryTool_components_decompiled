using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render a tab control with visual styles. This class cannot be inherited.</summary>
	// Token: 0x0200038D RID: 909
	public sealed class TabRenderer
	{
		// Token: 0x0600393B RID: 14651 RVA: 0x000027DB File Offset: 0x000009DB
		private TabRenderer()
		{
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.TabRenderer" /> class can be used to draw a tab control with visual styles.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has enabled visual styles in the operating system and visual styles are applied to the client area of application windows; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x0600393C RID: 14652 RVA: 0x00023BA7 File Offset: 0x00021DA7
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		/// <summary>Draws a tab in the specified state and bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the tab.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the tab.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TabItemState" /> values that specifies the visual state of the tab.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x0600393D RID: 14653 RVA: 0x000FEE6F File Offset: 0x000FD06F
		public static void DrawTabItem(Graphics g, Rectangle bounds, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a tab in the specified state and bounds, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the tab.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the tab.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TabItemState" /> values that specifies the visual state of the tab.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x0600393E RID: 14654 RVA: 0x000FEE88 File Offset: 0x000FD088
		public static void DrawTabItem(Graphics g, Rectangle bounds, bool focused, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			Rectangle rectangle = Rectangle.Inflate(bounds, -3, -3);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		/// <summary>Draws a tab in the specified state and bounds, and with the specified text.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the tab.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the tab.</param>
		/// <param name="tabItemText">The <see cref="T:System.String" /> to draw in the tab.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="tabItemText" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TabItemState" /> values that specifies the visual state of the tab.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x0600393F RID: 14655 RVA: 0x000FEEC1 File Offset: 0x000FD0C1
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, TabItemState state)
		{
			TabRenderer.DrawTabItem(g, bounds, tabItemText, font, false, state);
		}

		/// <summary>Draws a tab in the specified state and bounds, with the specified text, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the tab.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the tab.</param>
		/// <param name="tabItemText">The <see cref="T:System.String" /> to draw in the tab.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="tabItemText" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TabItemState" /> values that specifies the visual state of the tab.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003940 RID: 14656 RVA: 0x000FEECF File Offset: 0x000FD0CF
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, bool focused, TabItemState state)
		{
			TabRenderer.DrawTabItem(g, bounds, tabItemText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, focused, state);
		}

		/// <summary>Draws a tab in the specified state and bounds, with the specified text and text formatting, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the tab.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the tab.</param>
		/// <param name="tabItemText">The <see cref="T:System.String" /> to draw in the tab.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="tabItemText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TabItemState" /> values that specifies the visual state of the tab.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003941 RID: 14657 RVA: 0x000FEEE0 File Offset: 0x000FD0E0
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, TextFormatFlags flags, bool focused, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			Rectangle rectangle = Rectangle.Inflate(bounds, -3, -3);
			Color color = TabRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			TextRenderer.DrawText(g, tabItemText, font, rectangle, color, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		/// <summary>Draws a tab in the specified state and bounds, with the specified image, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the tab.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the tab.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw in the tab.</param>
		/// <param name="imageRectangle">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of <paramref name="image" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TabItemState" /> values that specifies the visual state of the tab.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003942 RID: 14658 RVA: 0x000FEF38 File Offset: 0x000FD138
		public static void DrawTabItem(Graphics g, Rectangle bounds, Image image, Rectangle imageRectangle, bool focused, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			Rectangle rectangle = Rectangle.Inflate(bounds, -3, -3);
			TabRenderer.visualStyleRenderer.DrawImage(g, imageRectangle, image);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		/// <summary>Draws a tab in the specified state and bounds, with the specified text and image, and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the tab.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the tab.</param>
		/// <param name="tabItemText">The <see cref="T:System.String" /> to draw in the tab.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="tabItemText" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw in the tab.</param>
		/// <param name="imageRectangle">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of <paramref name="image" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TabItemState" /> values that specifies the visual state of the tab.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003943 RID: 14659 RVA: 0x000FEF80 File Offset: 0x000FD180
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, Image image, Rectangle imageRectangle, bool focused, TabItemState state)
		{
			TabRenderer.DrawTabItem(g, bounds, tabItemText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, image, imageRectangle, focused, state);
		}

		/// <summary>Draws a tab in the specified state and bounds; with the specified text, text formatting, and image; and with an optional focus rectangle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the tab.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the tab.</param>
		/// <param name="tabItemText">The <see cref="T:System.String" /> to draw in the tab.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to <paramref name="tabItemText" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw in the tab.</param>
		/// <param name="imageRectangle">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of <paramref name="image" />.</param>
		/// <param name="focused">
		///       <see langword="true" /> to draw a focus rectangle; otherwise, <see langword="false" />.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TabItemState" /> values that specifies the visual state of the tab.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003944 RID: 14660 RVA: 0x000FEFA0 File Offset: 0x000FD1A0
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, TextFormatFlags flags, Image image, Rectangle imageRectangle, bool focused, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			Rectangle rectangle = Rectangle.Inflate(bounds, -3, -3);
			TabRenderer.visualStyleRenderer.DrawImage(g, imageRectangle, image);
			Color color = TabRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			TextRenderer.DrawText(g, tabItemText, font, rectangle, color, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		/// <summary>Draws a tab page in the specified bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the tab page.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the tab page.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003945 RID: 14661 RVA: 0x000FF006 File Offset: 0x000FD206
		public static void DrawTabPage(Graphics g, Rectangle bounds)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.Pane.Normal, 0);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x000FF01F File Offset: 0x000FD21F
		private static void InitializeRenderer(VisualStyleElement element, int state)
		{
			if (TabRenderer.visualStyleRenderer == null)
			{
				TabRenderer.visualStyleRenderer = new VisualStyleRenderer(element.ClassName, element.Part, state);
				return;
			}
			TabRenderer.visualStyleRenderer.SetParameters(element.ClassName, element.Part, state);
		}

		// Token: 0x04002281 RID: 8833
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;
	}
}
