using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the possible effects of a drag-and-drop operation.</summary>
	// Token: 0x02000226 RID: 550
	[Flags]
	public enum DragDropEffects
	{
		/// <summary>The drop target does not accept the data.</summary>
		// Token: 0x04000E60 RID: 3680
		None = 0,
		/// <summary>The data from the drag source is copied to the drop target.</summary>
		// Token: 0x04000E61 RID: 3681
		Copy = 1,
		/// <summary>The data from the drag source is moved to the drop target.</summary>
		// Token: 0x04000E62 RID: 3682
		Move = 2,
		/// <summary>The data from the drag source is linked to the drop target.</summary>
		// Token: 0x04000E63 RID: 3683
		Link = 4,
		/// <summary>The target can be scrolled while dragging to locate a drop position that is not currently visible in the target.</summary>
		// Token: 0x04000E64 RID: 3684
		Scroll = -2147483648,
		/// <summary>The combination of the <see cref="F:System.Windows.DragDropEffects.Copy" />, <see cref="F:System.Windows.Forms.DragDropEffects.Move" />, and <see cref="F:System.Windows.Forms.DragDropEffects.Scroll" /> effects.</summary>
		// Token: 0x04000E65 RID: 3685
		All = -2147483645
	}
}
