using System;

namespace System.Windows.Forms
{
	/// <summary>Determines the alignment of a <see cref="T:System.Windows.Forms.ToolStripItem" /> in a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
	// Token: 0x020003BD RID: 957
	public enum ToolStripItemAlignment
	{
		/// <summary>Specifies that the <see cref="T:System.Windows.Forms.ToolStripItem" /> is to be anchored toward the left or top end of the <see cref="T:System.Windows.Forms.ToolStrip" />, depending on the <see cref="T:System.Windows.Forms.ToolStrip" /> orientation. If the value of <see cref="T:System.Windows.Forms.RightToLeft" /> is <see langword="Yes" />, items marked as <see cref="F:System.Windows.Forms.ToolStripItemAlignment.Left" /> are aligned to the right side of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		// Token: 0x04002492 RID: 9362
		Left,
		/// <summary>Specifies that the <see cref="T:System.Windows.Forms.ToolStripItem" /> is to be anchored toward the right or bottom end of the <see cref="T:System.Windows.Forms.ToolStrip" />, depending on the <see cref="T:System.Windows.Forms.ToolStrip" /> orientation. If the value of <see cref="T:System.Windows.Forms.RightToLeft" /> is <see langword="Yes" />, items marked as <see cref="F:System.Windows.Forms.ToolStripItemAlignment.Right" /> are aligned to the left side of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		// Token: 0x04002493 RID: 9363
		Right
	}
}
