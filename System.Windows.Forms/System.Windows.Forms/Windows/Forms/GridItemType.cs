using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the valid grid item types for a <see cref="T:System.Windows.Forms.PropertyGrid" />.</summary>
	// Token: 0x0200025B RID: 603
	public enum GridItemType
	{
		/// <summary>A grid entry that corresponds to a property.</summary>
		// Token: 0x04000FA0 RID: 4000
		Property,
		/// <summary>A grid entry that is a category name. A category is a descriptive grouping for groups of <see cref="T:System.Windows.Forms.GridItem" /> rows. Typical categories include the following Behavior, Layout, Data, and Appearance.</summary>
		// Token: 0x04000FA1 RID: 4001
		Category,
		/// <summary>The <see cref="T:System.Windows.Forms.GridItem" /> is an element of an array.</summary>
		// Token: 0x04000FA2 RID: 4002
		ArrayValue,
		/// <summary>A root item in the grid hierarchy.</summary>
		// Token: 0x04000FA3 RID: 4003
		Root
	}
}
