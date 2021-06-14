using System;

namespace System.Windows.Forms.Design
{
	/// <summary>Specifies controls that are visible in the designer.</summary>
	// Token: 0x0200049C RID: 1180
	[Flags]
	public enum ToolStripItemDesignerAvailability
	{
		/// <summary>Specifies that no controls are visible.</summary>
		// Token: 0x040033FE RID: 13310
		None = 0,
		/// <summary>Specifies that <see cref="T:System.Windows.Forms.ToolStrip" /> is visible.</summary>
		// Token: 0x040033FF RID: 13311
		ToolStrip = 1,
		/// <summary>Specifies that <see cref="T:System.Windows.Forms.MenuStrip" /> is visible.</summary>
		// Token: 0x04003400 RID: 13312
		MenuStrip = 2,
		/// <summary>Specifies that <see cref="T:System.Windows.Forms.ContextMenuStrip" /> is visible.</summary>
		// Token: 0x04003401 RID: 13313
		ContextMenuStrip = 4,
		/// <summary>Specifies that <see cref="T:System.Windows.Forms.StatusStrip" /> is visible.</summary>
		// Token: 0x04003402 RID: 13314
		StatusStrip = 8,
		/// <summary>Specifies that all controls are visible.</summary>
		// Token: 0x04003403 RID: 13315
		All = 15
	}
}
