using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the <see cref="T:System.Windows.Forms.DataGridView" /> entity that owns the cell style that was changed.</summary>
	// Token: 0x020001A7 RID: 423
	[Flags]
	public enum DataGridViewCellStyleScopes
	{
		/// <summary>The owning entity is unspecified.</summary>
		// Token: 0x04000C46 RID: 3142
		None = 0,
		/// <summary>One or more values of the object returned by the <see cref="P:System.Windows.Forms.DataGridViewCell.Style" /> property changed.</summary>
		// Token: 0x04000C47 RID: 3143
		Cell = 1,
		/// <summary>One or more values of the object returned by the <see cref="P:System.Windows.Forms.DataGridViewColumn.DefaultCellStyle" /> property changed.</summary>
		// Token: 0x04000C48 RID: 3144
		Column = 2,
		/// <summary>One or more values of the object returned by the <see cref="P:System.Windows.Forms.DataGridViewRow.DefaultCellStyle" /> property changed.</summary>
		// Token: 0x04000C49 RID: 3145
		Row = 4,
		/// <summary>One or more values of the object returned by the <see cref="P:System.Windows.Forms.DataGridView.DefaultCellStyle" /> property changed.</summary>
		// Token: 0x04000C4A RID: 3146
		DataGridView = 8,
		/// <summary>One or more values of the object returned by the <see cref="P:System.Windows.Forms.DataGridView.ColumnHeadersDefaultCellStyle" /> property changed.</summary>
		// Token: 0x04000C4B RID: 3147
		ColumnHeaders = 16,
		/// <summary>One or more values of the object returned by the <see cref="P:System.Windows.Forms.DataGridView.RowHeadersDefaultCellStyle" /> property changed.</summary>
		// Token: 0x04000C4C RID: 3148
		RowHeaders = 32,
		/// <summary>One or more values of the object returned by the <see cref="P:System.Windows.Forms.DataGridView.RowsDefaultCellStyle" /> property changed.</summary>
		// Token: 0x04000C4D RID: 3149
		Rows = 64,
		/// <summary>One or more values of the object returned by the <see cref="P:System.Windows.Forms.DataGridView.AlternatingRowsDefaultCellStyle" /> property changed.</summary>
		// Token: 0x04000C4E RID: 3150
		AlternatingRows = 128
	}
}
