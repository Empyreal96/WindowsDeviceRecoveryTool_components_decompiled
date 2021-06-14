using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies where a <see cref="T:System.Windows.Forms.ToolStripItem" /> is to be layed out.</summary>
	// Token: 0x020003C9 RID: 969
	public enum ToolStripItemPlacement
	{
		/// <summary>Specifies that a <see cref="T:System.Windows.Forms.ToolStripItem" /> is to be layed out on the main <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		// Token: 0x040024B6 RID: 9398
		Main,
		/// <summary>Specifies that a <see cref="T:System.Windows.Forms.ToolStripItem" /> is to be layed out on the overflow <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		// Token: 0x040024B7 RID: 9399
		Overflow,
		/// <summary>Specifies that a <see cref="T:System.Windows.Forms.ToolStripItem" /> is not to be layed out on the screen.</summary>
		// Token: 0x040024B8 RID: 9400
		None
	}
}
