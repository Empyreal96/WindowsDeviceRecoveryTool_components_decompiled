using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the text orientation to use with a particular <see cref="P:System.Windows.Forms.ToolStrip.LayoutStyle" />.</summary>
	// Token: 0x020003F9 RID: 1017
	public enum ToolStripTextDirection
	{
		/// <summary>Specifies that the text direction is inherited from the parent control.</summary>
		// Token: 0x040025E7 RID: 9703
		Inherit,
		/// <summary>Specifies horizontal text orientation.</summary>
		// Token: 0x040025E8 RID: 9704
		Horizontal,
		/// <summary>Specifies that text is to be rotated 90 degrees.</summary>
		// Token: 0x040025E9 RID: 9705
		Vertical90,
		/// <summary>Specifies that text is to be rotated 270 degrees.</summary>
		// Token: 0x040025EA RID: 9706
		Vertical270
	}
}
