using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Identifies the Boolean properties of a visual style element.</summary>
	// Token: 0x0200046E RID: 1134
	public enum BooleanProperty
	{
		/// <summary>The image has transparent areas.</summary>
		// Token: 0x04003275 RID: 12917
		Transparent = 2201,
		/// <summary>The width of nonclient captions varies with the extent of the text.</summary>
		// Token: 0x04003276 RID: 12918
		AutoSize,
		/// <summary>Only the border of an image is drawn.</summary>
		// Token: 0x04003277 RID: 12919
		BorderOnly,
		/// <summary>The control will handle composite drawing.</summary>
		// Token: 0x04003278 RID: 12920
		Composited,
		/// <summary>The background of a fixed-size element is a filled rectangle.</summary>
		// Token: 0x04003279 RID: 12921
		BackgroundFill,
		/// <summary>The glyph has transparent areas.</summary>
		// Token: 0x0400327A RID: 12922
		GlyphTransparent,
		/// <summary>Only the glyph should be drawn, not the background.</summary>
		// Token: 0x0400327B RID: 12923
		GlyphOnly,
		/// <summary>The sizing handle will always be displayed.</summary>
		// Token: 0x0400327C RID: 12924
		AlwaysShowSizingBar,
		/// <summary>The image is mirrored in right-to-left display modes.</summary>
		// Token: 0x0400327D RID: 12925
		MirrorImage,
		/// <summary>The height and width must be sized equally.</summary>
		// Token: 0x0400327E RID: 12926
		UniformSizing,
		/// <summary>The scaling factor must be an integer for fixed-size elements.</summary>
		// Token: 0x0400327F RID: 12927
		IntegralSizing,
		/// <summary>The source image will scale larger when needed.</summary>
		// Token: 0x04003280 RID: 12928
		SourceGrow,
		/// <summary>The source image will scale smaller when needed.</summary>
		// Token: 0x04003281 RID: 12929
		SourceShrink
	}
}
