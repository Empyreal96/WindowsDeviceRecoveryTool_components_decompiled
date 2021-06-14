using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Identifies the integer properties of a visual style element.</summary>
	// Token: 0x0200046A RID: 1130
	public enum IntegerProperty
	{
		/// <summary>The number of state images in multiple-image file.</summary>
		// Token: 0x0400324D RID: 12877
		ImageCount = 2401,
		/// <summary>The alpha value for an icon, between 0 and 255.</summary>
		// Token: 0x0400324E RID: 12878
		AlphaLevel,
		/// <summary>The size of the border line for elements with a filled-border background.</summary>
		// Token: 0x0400324F RID: 12879
		BorderSize,
		/// <summary>A percentage value that represents the width of a rounded corner, from 0 to 100.</summary>
		// Token: 0x04003250 RID: 12880
		RoundCornerWidth,
		/// <summary>A percentage value that represents the height of a rounded corner, from 0 to 100.</summary>
		// Token: 0x04003251 RID: 12881
		RoundCornerHeight,
		/// <summary>The amount of <see cref="F:System.Windows.Forms.VisualStyles.ColorProperty.GradientColor1" />  to use in a color gradient. The sum of the five <see langword="GradientRatio" /> properties must equal 255.</summary>
		// Token: 0x04003252 RID: 12882
		GradientRatio1,
		/// <summary>The amount of <see cref="F:System.Windows.Forms.VisualStyles.ColorProperty.GradientColor2" />  to use in a color gradient. The sum of the five <see langword="GradientRatio" /> properties must equal 255.</summary>
		// Token: 0x04003253 RID: 12883
		GradientRatio2,
		/// <summary>The amount of <see cref="F:System.Windows.Forms.VisualStyles.ColorProperty.GradientColor3" />  to use in a color gradient. The sum of the five <see langword="GradientRatio" /> properties must equal 255.</summary>
		// Token: 0x04003254 RID: 12884
		GradientRatio3,
		/// <summary>The amount of <see cref="F:System.Windows.Forms.VisualStyles.ColorProperty.GradientColor4" />  to use in a color gradient. The sum of the five <see langword="GradientRatio" /> properties must equal 255.</summary>
		// Token: 0x04003255 RID: 12885
		GradientRatio4,
		/// <summary>The amount of <see cref="F:System.Windows.Forms.VisualStyles.ColorProperty.GradientColor5" />  to use in a color gradient. The sum of the five <see langword="GradientRatio" /> properties must equal 255.</summary>
		// Token: 0x04003256 RID: 12886
		GradientRatio5,
		/// <summary>The size of progress bar elements.</summary>
		// Token: 0x04003257 RID: 12887
		ProgressChunkSize,
		/// <summary>The size of spaces between progress bar elements.</summary>
		// Token: 0x04003258 RID: 12888
		ProgressSpaceSize,
		/// <summary>The amount of saturation for an image, between 0 and 255.</summary>
		// Token: 0x04003259 RID: 12889
		Saturation,
		/// <summary>The size of the border around text characters.</summary>
		// Token: 0x0400325A RID: 12890
		TextBorderSize,
		/// <summary>The minimum alpha value of a solid pixel, between 0 and 255.</summary>
		// Token: 0x0400325B RID: 12891
		AlphaThreshold,
		/// <summary>The width of an element.</summary>
		// Token: 0x0400325C RID: 12892
		Width,
		/// <summary>The height of an element. </summary>
		// Token: 0x0400325D RID: 12893
		Height,
		/// <summary>The index into the font for font-based glyphs.</summary>
		// Token: 0x0400325E RID: 12894
		GlyphIndex,
		/// <summary>A percentage value indicating how far a fixed-size element will stretch when the target exceeds the source. </summary>
		// Token: 0x0400325F RID: 12895
		TrueSizeStretchMark,
		/// <summary>The minimum dots per inch (DPI) that <see cref="F:System.Windows.Forms.VisualStyles.FilenameProperty.ImageFile1" /> was designed for.</summary>
		// Token: 0x04003260 RID: 12896
		MinDpi1,
		/// <summary>The minimum DPI that <see cref="F:System.Windows.Forms.VisualStyles.FilenameProperty.ImageFile2" /> was designed for.</summary>
		// Token: 0x04003261 RID: 12897
		MinDpi2,
		/// <summary>The minimum DPI that <see cref="F:System.Windows.Forms.VisualStyles.FilenameProperty.ImageFile3" /> was designed for.</summary>
		// Token: 0x04003262 RID: 12898
		MinDpi3,
		/// <summary>The minimum DPI that <see cref="F:System.Windows.Forms.VisualStyles.FilenameProperty.ImageFile4" /> was designed for.</summary>
		// Token: 0x04003263 RID: 12899
		MinDpi4,
		/// <summary>The minimum DPI that <see cref="F:System.Windows.Forms.VisualStyles.FilenameProperty.ImageFile5" /> was designed for.</summary>
		// Token: 0x04003264 RID: 12900
		MinDpi5
	}
}
