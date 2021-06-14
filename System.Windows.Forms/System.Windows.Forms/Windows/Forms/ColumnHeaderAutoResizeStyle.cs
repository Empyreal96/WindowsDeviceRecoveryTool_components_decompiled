using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how a column contained in a <see cref="T:System.Windows.Forms.ListView" /> should be resized.</summary>
	// Token: 0x02000146 RID: 326
	public enum ColumnHeaderAutoResizeStyle
	{
		/// <summary>Specifies no resizing should occur.</summary>
		// Token: 0x040006EA RID: 1770
		None,
		/// <summary>Specifies the column should be resized based on the length of the column header content.</summary>
		// Token: 0x040006EB RID: 1771
		HeaderSize,
		/// <summary>Specifies the column should be resized based on the length of the column content.</summary>
		// Token: 0x040006EC RID: 1772
		ColumnContent
	}
}
