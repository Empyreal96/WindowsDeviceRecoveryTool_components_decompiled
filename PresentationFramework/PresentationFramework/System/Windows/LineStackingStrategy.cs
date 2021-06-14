using System;

namespace System.Windows
{
	/// <summary> Describes a mechanism by which a line box is determined for each line.  </summary>
	// Token: 0x02000126 RID: 294
	public enum LineStackingStrategy
	{
		/// <summary> The stack height is determined by the block element line-height property value. </summary>
		// Token: 0x04000ADB RID: 2779
		BlockLineHeight,
		/// <summary> The stack height is the smallest value that containing all the inline elements on that line when those elements are properly aligned. </summary>
		// Token: 0x04000ADC RID: 2780
		MaxHeight
	}
}
