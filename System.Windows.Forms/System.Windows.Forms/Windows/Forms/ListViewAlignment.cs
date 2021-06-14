using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how items align in the <see cref="T:System.Windows.Forms.ListView" />.</summary>
	// Token: 0x020002C4 RID: 708
	public enum ListViewAlignment
	{
		/// <summary>When the user moves an item, it remains where it is dropped.</summary>
		// Token: 0x0400124F RID: 4687
		Default,
		/// <summary>Items are aligned to the top of the <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		// Token: 0x04001250 RID: 4688
		Top = 2,
		/// <summary>Items are aligned to the left of the <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		// Token: 0x04001251 RID: 4689
		Left = 1,
		/// <summary>Items are aligned to an invisible grid in the control. When the user moves an item, it moves to the closest juncture in the grid.</summary>
		// Token: 0x04001252 RID: 4690
		SnapToGrid = 5
	}
}
