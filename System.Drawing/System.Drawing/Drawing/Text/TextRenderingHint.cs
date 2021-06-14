using System;

namespace System.Drawing.Text
{
	/// <summary>Specifies the quality of text rendering.</summary>
	// Token: 0x0200008A RID: 138
	public enum TextRenderingHint
	{
		/// <summary>Each character is drawn using its glyph bitmap, with the system default rendering hint. The text will be drawn using whatever font-smoothing settings the user has selected for the system.</summary>
		// Token: 0x04000729 RID: 1833
		SystemDefault,
		/// <summary>Each character is drawn using its glyph bitmap. Hinting is used to improve character appearance on stems and curvature.</summary>
		// Token: 0x0400072A RID: 1834
		SingleBitPerPixelGridFit,
		/// <summary>Each character is drawn using its glyph bitmap. Hinting is not used.</summary>
		// Token: 0x0400072B RID: 1835
		SingleBitPerPixel,
		/// <summary>Each character is drawn using its antialiased glyph bitmap with hinting. Much better quality due to antialiasing, but at a higher performance cost.</summary>
		// Token: 0x0400072C RID: 1836
		AntiAliasGridFit,
		/// <summary>Each character is drawn using its antialiased glyph bitmap without hinting. Better quality due to antialiasing. Stem width differences may be noticeable because hinting is turned off.</summary>
		// Token: 0x0400072D RID: 1837
		AntiAlias,
		/// <summary>Each character is drawn using its glyph ClearType bitmap with hinting. The highest quality setting. Used to take advantage of ClearType font features.</summary>
		// Token: 0x0400072E RID: 1838
		ClearTypeGridFit
	}
}
