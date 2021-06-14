using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the behavior of a <see cref="T:System.Windows.Forms.MenuItem" /> when it is merged with items in another menu.</summary>
	// Token: 0x020002E5 RID: 741
	public enum MenuMerge
	{
		/// <summary>The <see cref="T:System.Windows.Forms.MenuItem" /> is added to the collection of existing <see cref="T:System.Windows.Forms.MenuItem" /> objects in a merged menu.</summary>
		// Token: 0x0400131A RID: 4890
		Add,
		/// <summary>The <see cref="T:System.Windows.Forms.MenuItem" /> replaces an existing <see cref="T:System.Windows.Forms.MenuItem" /> at the same position in a merged menu.</summary>
		// Token: 0x0400131B RID: 4891
		Replace,
		/// <summary>All submenu items of this <see cref="T:System.Windows.Forms.MenuItem" /> are merged with those of existing <see cref="T:System.Windows.Forms.MenuItem" /> objects at the same position in a merged menu.</summary>
		// Token: 0x0400131C RID: 4892
		MergeItems,
		/// <summary>The <see cref="T:System.Windows.Forms.MenuItem" /> is not included in a merged menu.</summary>
		// Token: 0x0400131D RID: 4893
		Remove
	}
}
