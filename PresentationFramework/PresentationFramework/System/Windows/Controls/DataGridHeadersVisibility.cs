using System;

namespace System.Windows.Controls
{
	/// <summary>Defines constants that specify the visibility of row and column headers in a <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
	// Token: 0x020004AA RID: 1194
	[Flags]
	public enum DataGridHeadersVisibility
	{
		/// <summary>Both row and column headers are visible.</summary>
		// Token: 0x040029B8 RID: 10680
		All = 3,
		/// <summary>Only column headers are visible.</summary>
		// Token: 0x040029B9 RID: 10681
		Column = 1,
		/// <summary>Only row headers are visible.</summary>
		// Token: 0x040029BA RID: 10682
		Row = 2,
		/// <summary>No headers are visible.</summary>
		// Token: 0x040029BB RID: 10683
		None = 0
	}
}
