using System;

namespace System.Windows.Forms
{
	/// <summary>Determines whether a <see cref="T:System.Windows.Forms.ToolStripItem" /> is placed in the overflow <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
	// Token: 0x020003C8 RID: 968
	public enum ToolStripItemOverflow
	{
		/// <summary>Specifies that a <see cref="T:System.Windows.Forms.ToolStripItem" /> is never a candidate for the overflow <see cref="T:System.Windows.Forms.ToolStrip" />. If the <see cref="T:System.Windows.Forms.ToolStripItem" /> cannot fit on the main <see cref="T:System.Windows.Forms.ToolStrip" />, it will not be shown.</summary>
		// Token: 0x040024B2 RID: 9394
		Never,
		/// <summary>Specifies that a <see cref="T:System.Windows.Forms.ToolStripItem" /> is permanently shown in the overflow <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		// Token: 0x040024B3 RID: 9395
		Always,
		/// <summary>Specifies that a <see cref="T:System.Windows.Forms.ToolStripItem" /> drifts between the main <see cref="T:System.Windows.Forms.ToolStrip" /> and the overflow <see cref="T:System.Windows.Forms.ToolStrip" /> as required if space is not available on the main <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		// Token: 0x040024B4 RID: 9396
		AsNeeded
	}
}
