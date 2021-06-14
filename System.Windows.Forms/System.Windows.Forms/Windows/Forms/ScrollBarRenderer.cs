using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render a scroll bar control with visual styles. This class cannot be inherited.</summary>
	// Token: 0x02000346 RID: 838
	public sealed class ScrollBarRenderer
	{
		// Token: 0x060034D7 RID: 13527 RVA: 0x000027DB File Offset: 0x000009DB
		private ScrollBarRenderer()
		{
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ScrollBarRenderer" /> class can be used to draw a scroll bar with visual styles.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has enabled visual styles in the operating system and visual styles are applied to the client areas of application windows; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x060034D8 RID: 13528 RVA: 0x00023BA7 File Offset: 0x00021DA7
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		/// <summary>Draws a scroll arrow with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the scroll arrow.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the scroll arrow.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarArrowButtonState" /> values that specifies the visual state of the scroll arrow.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034D9 RID: 13529 RVA: 0x000F19F2 File Offset: 0x000EFBF2
		public static void DrawArrowButton(Graphics g, Rectangle bounds, ScrollBarArrowButtonState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.ArrowButton.LeftNormal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a horizontal scroll box (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the scroll box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the scroll box.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the scroll box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034DA RID: 13530 RVA: 0x000F1A0B File Offset: 0x000EFC0B
		public static void DrawHorizontalThumb(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.ThumbButtonHorizontal.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a vertical scroll box (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the scroll box.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the scroll box.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the scroll box.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034DB RID: 13531 RVA: 0x000F1A24 File Offset: 0x000EFC24
		public static void DrawVerticalThumb(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.ThumbButtonVertical.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a grip on a horizontal scroll box (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the scroll box grip.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the scroll box grip.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the scroll box grip.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034DC RID: 13532 RVA: 0x000F1A3D File Offset: 0x000EFC3D
		public static void DrawHorizontalThumbGrip(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.GripperHorizontal.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a grip on a vertical scroll box (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the scroll box grip.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the scroll box grip.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the scroll box grip.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034DD RID: 13533 RVA: 0x000F1A56 File Offset: 0x000EFC56
		public static void DrawVerticalThumbGrip(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.GripperVertical.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a horizontal scroll bar track with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the scroll bar track.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the scroll bar track.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the scroll bar track.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034DE RID: 13534 RVA: 0x000F1A6F File Offset: 0x000EFC6F
		public static void DrawRightHorizontalTrack(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.RightTrackHorizontal.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a horizontal scroll bar track with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the scroll bar track.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the scroll bar track.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the scroll bar track.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034DF RID: 13535 RVA: 0x000F1A88 File Offset: 0x000EFC88
		public static void DrawLeftHorizontalTrack(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.LeftTrackHorizontal.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a vertical scroll bar track with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the scroll bar track.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the scroll bar track.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the scroll bar track.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034E0 RID: 13536 RVA: 0x000F1AA1 File Offset: 0x000EFCA1
		public static void DrawUpperVerticalTrack(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.UpperTrackVertical.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a vertical scroll bar track with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the scroll bar track.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the scroll bar track.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the scroll bar track.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034E1 RID: 13537 RVA: 0x000F1ABA File Offset: 0x000EFCBA
		public static void DrawLowerVerticalTrack(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.LowerTrackVertical.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a scroll bar sizing handle with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the sizing handle.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the sizing handle.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarSizeBoxState" /> values that specifies the visual state of the sizing handle.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034E2 RID: 13538 RVA: 0x000F1AD3 File Offset: 0x000EFCD3
		public static void DrawSizeBox(Graphics g, Rectangle bounds, ScrollBarSizeBoxState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.SizeBox.LeftAlign, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Returns the size of the scroll box grip.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> this operation will use.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the scroll box grip.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the size of the scroll box grip.</returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034E3 RID: 13539 RVA: 0x000F1AEC File Offset: 0x000EFCEC
		public static Size GetThumbGripSize(Graphics g, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.GripperHorizontal.Normal, (int)state);
			return ScrollBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		/// <summary>Returns the size of the sizing handle.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> this operation will use.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.ScrollBarState" /> values that specifies the visual state of the sizing handle.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the size of the sizing handle.</returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060034E4 RID: 13540 RVA: 0x000F1B05 File Offset: 0x000EFD05
		public static Size GetSizeBoxSize(Graphics g, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.SizeBox.LeftAlign, (int)state);
			return ScrollBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x000F1B1E File Offset: 0x000EFD1E
		private static void InitializeRenderer(VisualStyleElement element, int state)
		{
			if (ScrollBarRenderer.visualStyleRenderer == null)
			{
				ScrollBarRenderer.visualStyleRenderer = new VisualStyleRenderer(element.ClassName, element.Part, state);
				return;
			}
			ScrollBarRenderer.visualStyleRenderer.SetParameters(element.ClassName, element.Part, state);
		}

		// Token: 0x04002069 RID: 8297
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;
	}
}
