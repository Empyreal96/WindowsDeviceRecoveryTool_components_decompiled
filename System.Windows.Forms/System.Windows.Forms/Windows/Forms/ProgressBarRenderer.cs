using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render a progress bar control with visual styles. This class cannot be inherited.</summary>
	// Token: 0x02000318 RID: 792
	public sealed class ProgressBarRenderer
	{
		// Token: 0x06003094 RID: 12436 RVA: 0x000027DB File Offset: 0x000009DB
		private ProgressBarRenderer()
		{
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ProgressBarRenderer" /> class can be used to draw a progress bar control with visual styles.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has enabled visual styles in the operating system and visual styles are applied to the client area of application windows; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06003095 RID: 12437 RVA: 0x00023BA7 File Offset: 0x00021DA7
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		/// <summary>Draws an empty progress bar control that fills in horizontally.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the progress bar.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the progress bar.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003096 RID: 12438 RVA: 0x000E32D1 File Offset: 0x000E14D1
		public static void DrawHorizontalBar(Graphics g, Rectangle bounds)
		{
			ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.Bar.Normal);
			ProgressBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws an empty progress bar control that fills in vertically.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the progress bar.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the progress bar.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003097 RID: 12439 RVA: 0x000E32E9 File Offset: 0x000E14E9
		public static void DrawVerticalBar(Graphics g, Rectangle bounds)
		{
			ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.BarVertical.Normal);
			ProgressBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a set of progress bar pieces that fill a horizontal progress bar.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the progress bar.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds to be filled by progress bar pieces.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003098 RID: 12440 RVA: 0x000E3301 File Offset: 0x000E1501
		public static void DrawHorizontalChunks(Graphics g, Rectangle bounds)
		{
			ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.Chunk.Normal);
			ProgressBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a set of progress bar pieces that fill a vertical progress bar.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the progress bar.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds to be filled by progress bar pieces.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06003099 RID: 12441 RVA: 0x000E3319 File Offset: 0x000E1519
		public static void DrawVerticalChunks(Graphics g, Rectangle bounds)
		{
			ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.ChunkVertical.Normal);
			ProgressBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Gets the width, in pixels, of a single inner piece of the progress bar.</summary>
		/// <returns>The width, in pixels, of a single inner piece of the progress bar.</returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x0600309A RID: 12442 RVA: 0x000E3331 File Offset: 0x000E1531
		public static int ChunkThickness
		{
			get
			{
				ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.Chunk.Normal);
				return ProgressBarRenderer.visualStyleRenderer.GetInteger(IntegerProperty.ProgressChunkSize);
			}
		}

		/// <summary>Gets the width, in pixels, of the space between each inner piece of the progress bar.</summary>
		/// <returns>The width, in pixels, of the space between each inner piece of the progress bar. </returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x0600309B RID: 12443 RVA: 0x000E334C File Offset: 0x000E154C
		public static int ChunkSpaceThickness
		{
			get
			{
				ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.Chunk.Normal);
				return ProgressBarRenderer.visualStyleRenderer.GetInteger(IntegerProperty.ProgressSpaceSize);
			}
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x000E3367 File Offset: 0x000E1567
		private static void InitializeRenderer(VisualStyleElement element)
		{
			if (ProgressBarRenderer.visualStyleRenderer == null)
			{
				ProgressBarRenderer.visualStyleRenderer = new VisualStyleRenderer(element);
				return;
			}
			ProgressBarRenderer.visualStyleRenderer.SetParameters(element);
		}

		// Token: 0x04001DB3 RID: 7603
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;
	}
}
