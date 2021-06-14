using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how the parent row labels of a <see cref="T:System.Windows.Forms.DataGrid" /> control are displayed.</summary>
	// Token: 0x02000175 RID: 373
	public enum DataGridParentRowsLabelStyle
	{
		/// <summary>Display no parent row labels.</summary>
		// Token: 0x040009B6 RID: 2486
		None,
		/// <summary>Displays the parent table name.</summary>
		// Token: 0x040009B7 RID: 2487
		TableName,
		/// <summary>Displays the parent column name.</summary>
		// Token: 0x040009B8 RID: 2488
		ColumnName,
		/// <summary>Displays both the parent table and column names.</summary>
		// Token: 0x040009B9 RID: 2489
		Both
	}
}
