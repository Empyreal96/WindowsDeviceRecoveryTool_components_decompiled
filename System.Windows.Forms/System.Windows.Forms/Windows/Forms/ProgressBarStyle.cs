using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the style that a <see cref="T:System.Windows.Forms.ProgressBar" /> uses to indicate the progress of an operation.</summary>
	// Token: 0x02000319 RID: 793
	public enum ProgressBarStyle
	{
		/// <summary>Indicates progress by increasing the number of segmented blocks in a <see cref="T:System.Windows.Forms.ProgressBar" />.</summary>
		// Token: 0x04001DB5 RID: 7605
		Blocks,
		/// <summary>Indicates progress by increasing the size of a smooth, continuous bar in a <see cref="T:System.Windows.Forms.ProgressBar" />.</summary>
		// Token: 0x04001DB6 RID: 7606
		Continuous,
		/// <summary>Indicates progress by continuously scrolling a block across a <see cref="T:System.Windows.Forms.ProgressBar" /> in a marquee fashion.</summary>
		// Token: 0x04001DB7 RID: 7607
		Marquee
	}
}
