using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies when the visual style selects a different glyph font size.</summary>
	// Token: 0x02000465 RID: 1125
	public enum GlyphFontSizingType
	{
		/// <summary>Glyph font sizes do not change.</summary>
		// Token: 0x04003216 RID: 12822
		None,
		/// <summary>Glyph font size changes are based on font size settings.</summary>
		// Token: 0x04003217 RID: 12823
		Size,
		/// <summary>Glyph font size changes are based on dots per inch (DPI) settings.</summary>
		// Token: 0x04003218 RID: 12824
		Dpi
	}
}
