using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how list items are displayed in a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
	// Token: 0x02000420 RID: 1056
	public enum View
	{
		/// <summary>Each item appears as a full-sized icon with a label below it.</summary>
		// Token: 0x040026E7 RID: 9959
		LargeIcon,
		/// <summary>Each item appears on a separate line with further information about each item arranged in columns. The left-most column contains a small icon and label, and subsequent columns contain sub items as specified by the application. A column displays a header which can display a caption for the column. The user can resize each column at run time.</summary>
		// Token: 0x040026E8 RID: 9960
		Details,
		/// <summary>Each item appears as a small icon with a label to its right.</summary>
		// Token: 0x040026E9 RID: 9961
		SmallIcon,
		/// <summary>Each item appears as a small icon with a label to its right. Items are arranged in columns with no column headers.</summary>
		// Token: 0x040026EA RID: 9962
		List,
		/// <summary>Each item appears as a full-sized icon with the item label and subitem information to the right of it. The subitem information that appears is specified by the application. This view is available only on Windows XP and the Windows Server 2003 family. On earlier operating systems, this value is ignored and the <see cref="T:System.Windows.Forms.ListView" /> control displays in the <see cref="F:System.Windows.Forms.View.LargeIcon" /> view.</summary>
		// Token: 0x040026EB RID: 9963
		Tile
	}
}
