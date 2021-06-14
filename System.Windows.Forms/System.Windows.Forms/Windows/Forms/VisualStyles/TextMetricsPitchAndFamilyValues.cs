using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies information about the pitch, technology, and family of the font specified by a visual style for a particular element.</summary>
	// Token: 0x02000473 RID: 1139
	[Flags]
	public enum TextMetricsPitchAndFamilyValues
	{
		/// <summary>If this value is set, the font is a variable pitch font. Otherwise, the font is a fixed-pitch font. </summary>
		// Token: 0x040032A8 RID: 12968
		FixedPitch = 1,
		/// <summary>The font is a vector font.</summary>
		// Token: 0x040032A9 RID: 12969
		Vector = 2,
		/// <summary>The font is a TrueType font.</summary>
		// Token: 0x040032AA RID: 12970
		TrueType = 4,
		/// <summary>The font is a device font.</summary>
		// Token: 0x040032AB RID: 12971
		Device = 8
	}
}
