using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the layout of multiple document interface (MDI) child windows in an MDI parent window.</summary>
	// Token: 0x020002DD RID: 733
	public enum MdiLayout
	{
		/// <summary>All MDI child windows are cascaded within the client region of the MDI parent form.</summary>
		// Token: 0x040012E1 RID: 4833
		Cascade,
		/// <summary>All MDI child windows are tiled horizontally within the client region of the MDI parent form.</summary>
		// Token: 0x040012E2 RID: 4834
		TileHorizontal,
		/// <summary>All MDI child windows are tiled vertically within the client region of the MDI parent form.</summary>
		// Token: 0x040012E3 RID: 4835
		TileVertical,
		/// <summary>All MDI child icons are arranged within the client region of the MDI parent form.</summary>
		// Token: 0x040012E4 RID: 4836
		ArrangeIcons
	}
}
