using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to render a track bar control with visual styles. This class cannot be inherited.</summary>
	// Token: 0x020003FF RID: 1023
	public sealed class TrackBarRenderer
	{
		// Token: 0x060045C4 RID: 17860 RVA: 0x000027DB File Offset: 0x000009DB
		private TrackBarRenderer()
		{
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.TrackBarRenderer" /> class can be used to draw a track bar with visual styles.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has enabled visual styles in the operating system and visual styles are applied to the client area of application windows; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x060045C5 RID: 17861 RVA: 0x00023BA7 File Offset: 0x00021DA7
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		/// <summary>Draws the track for a horizontal track bar with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the track.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the track.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045C6 RID: 17862 RVA: 0x00129DB8 File Offset: 0x00127FB8
		public static void DrawHorizontalTrack(Graphics g, Rectangle bounds)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.Track.Normal, 1);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws the track for a vertical track bar with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the track.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the track.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045C7 RID: 17863 RVA: 0x00129DD1 File Offset: 0x00127FD1
		public static void DrawVerticalTrack(Graphics g, Rectangle bounds)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.TrackVertical.Normal, 1);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a horizontal track bar slider (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the track bar slider.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the track bar slider.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the track bar slider.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045C8 RID: 17864 RVA: 0x00129DEA File Offset: 0x00127FEA
		public static void DrawHorizontalThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.Thumb.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a vertical track bar slider (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the track bar slider.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the track bar slider.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the track bar slider.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045C9 RID: 17865 RVA: 0x00129E03 File Offset: 0x00128003
		public static void DrawVerticalThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbVertical.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a left-pointing track bar slider (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the track bar slider.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the track bar slider.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the track bar slider.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045CA RID: 17866 RVA: 0x00129E1C File Offset: 0x0012801C
		public static void DrawLeftPointingThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbLeft.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a right-pointing track bar slider (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the track bar slider.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the track bar slider.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the track bar slider.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045CB RID: 17867 RVA: 0x00129E35 File Offset: 0x00128035
		public static void DrawRightPointingThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbRight.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws an upward-pointing track bar slider (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the track bar slider.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the track bar slider.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the track bar slider.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045CC RID: 17868 RVA: 0x00129E4E File Offset: 0x0012804E
		public static void DrawTopPointingThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbTop.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws a downward-pointing track bar slider (also known as the thumb) with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the track bar slider.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the track bar slider.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the track bar slider.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045CD RID: 17869 RVA: 0x00129E67 File Offset: 0x00128067
		public static void DrawBottomPointingThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbBottom.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		/// <summary>Draws the specified number of horizontal track bar ticks with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the ticks.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the ticks.</param>
		/// <param name="numTicks">The number of ticks to draw.</param>
		/// <param name="edgeStyle">One of the <see cref="T:System.Windows.Forms.VisualStyles.EdgeStyle" /> values.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045CE RID: 17870 RVA: 0x00129E80 File Offset: 0x00128080
		public static void DrawHorizontalTicks(Graphics g, Rectangle bounds, int numTicks, EdgeStyle edgeStyle)
		{
			if (numTicks <= 0 || bounds.Height <= 0 || bounds.Width <= 0 || g == null)
			{
				return;
			}
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.Ticks.Normal, 1);
			if (numTicks == 1)
			{
				TrackBarRenderer.visualStyleRenderer.DrawEdge(g, new Rectangle(bounds.X, bounds.Y, 2, bounds.Height), Edges.Left, edgeStyle, EdgeEffects.None);
				return;
			}
			float num = ((float)bounds.Width - 2f) / ((float)numTicks - 1f);
			while (numTicks > 0)
			{
				float num2 = (float)bounds.X + (float)(numTicks - 1) * num;
				TrackBarRenderer.visualStyleRenderer.DrawEdge(g, new Rectangle((int)Math.Round((double)num2), bounds.Y, 2, bounds.Height), Edges.Left, edgeStyle, EdgeEffects.None);
				numTicks--;
			}
		}

		/// <summary>Draws the specified number of vertical track bar ticks with visual styles.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the ticks.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that specifies the bounds of the ticks.</param>
		/// <param name="numTicks">The number of ticks to draw.</param>
		/// <param name="edgeStyle">One of the <see cref="T:System.Windows.Forms.VisualStyles.EdgeStyle" /> values.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045CF RID: 17871 RVA: 0x00129F44 File Offset: 0x00128144
		public static void DrawVerticalTicks(Graphics g, Rectangle bounds, int numTicks, EdgeStyle edgeStyle)
		{
			if (numTicks <= 0 || bounds.Height <= 0 || bounds.Width <= 0 || g == null)
			{
				return;
			}
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.TicksVertical.Normal, 1);
			if (numTicks == 1)
			{
				TrackBarRenderer.visualStyleRenderer.DrawEdge(g, new Rectangle(bounds.X, bounds.Y, bounds.Width, 2), Edges.Top, edgeStyle, EdgeEffects.None);
				return;
			}
			float num = ((float)bounds.Height - 2f) / ((float)numTicks - 1f);
			while (numTicks > 0)
			{
				float num2 = (float)bounds.Y + (float)(numTicks - 1) * num;
				TrackBarRenderer.visualStyleRenderer.DrawEdge(g, new Rectangle(bounds.X, (int)Math.Round((double)num2), bounds.Width, 2), Edges.Top, edgeStyle, EdgeEffects.None);
				numTicks--;
			}
		}

		/// <summary>Returns the size, in pixels, of the track bar slider (also known as the thumb) that points to the left.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> this operation will use.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the slider.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the size, in pixels, of the slider.</returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045D0 RID: 17872 RVA: 0x0012A005 File Offset: 0x00128205
		public static Size GetLeftPointingThumbSize(Graphics g, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbLeft.Normal, (int)state);
			return TrackBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		/// <summary>Returns the size, in pixels, of the track bar slider (also known as the thumb) that points to the right.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> this operation will use.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the slider.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the size, in pixels, of the slider.</returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045D1 RID: 17873 RVA: 0x0012A01E File Offset: 0x0012821E
		public static Size GetRightPointingThumbSize(Graphics g, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbRight.Normal, (int)state);
			return TrackBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		/// <summary>Returns the size, in pixels, of the track bar slider (also known as the thumb) that points up.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> this operation will use.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the slider.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the size, in pixels, of the slider.</returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045D2 RID: 17874 RVA: 0x0012A037 File Offset: 0x00128237
		public static Size GetTopPointingThumbSize(Graphics g, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbTop.Normal, (int)state);
			return TrackBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		/// <summary>Returns the size, in pixels, of the track bar slider (also known as the thumb) that points down.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> this operation will use.</param>
		/// <param name="state">One of the <see cref="T:System.Windows.Forms.VisualStyles.TrackBarThumbState" /> values that specifies the visual state of the track bar slider.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the size, in pixels, of the slider.</returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x060045D3 RID: 17875 RVA: 0x0012A050 File Offset: 0x00128250
		public static Size GetBottomPointingThumbSize(Graphics g, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbBottom.Normal, (int)state);
			return TrackBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x0012A069 File Offset: 0x00128269
		private static void InitializeRenderer(VisualStyleElement element, int state)
		{
			if (TrackBarRenderer.visualStyleRenderer == null)
			{
				TrackBarRenderer.visualStyleRenderer = new VisualStyleRenderer(element.ClassName, element.Part, state);
				return;
			}
			TrackBarRenderer.visualStyleRenderer.SetParameters(element.ClassName, element.Part, state);
		}

		// Token: 0x04002625 RID: 9765
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;

		// Token: 0x04002626 RID: 9766
		private const int lineWidth = 2;
	}
}
