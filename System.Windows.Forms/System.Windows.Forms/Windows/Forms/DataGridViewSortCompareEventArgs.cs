using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.SortCompare" /> event.</summary>
	// Token: 0x0200020B RID: 523
	public class DataGridViewSortCompareEventArgs : HandledEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewSortCompareEventArgs" /> class.</summary>
		/// <param name="dataGridViewColumn">The column to sort.</param>
		/// <param name="cellValue1">The value of the first cell to compare.</param>
		/// <param name="cellValue2">The value of the second cell to compare.</param>
		/// <param name="rowIndex1">The index of the row containing the first cell.</param>
		/// <param name="rowIndex2">The index of the row containing the second cell.</param>
		// Token: 0x06001FCC RID: 8140 RVA: 0x0009EFB2 File Offset: 0x0009D1B2
		public DataGridViewSortCompareEventArgs(DataGridViewColumn dataGridViewColumn, object cellValue1, object cellValue2, int rowIndex1, int rowIndex2)
		{
			this.dataGridViewColumn = dataGridViewColumn;
			this.cellValue1 = cellValue1;
			this.cellValue2 = cellValue2;
			this.rowIndex1 = rowIndex1;
			this.rowIndex2 = rowIndex2;
		}

		/// <summary>Gets the value of the first cell to compare.</summary>
		/// <returns>The value of the first cell.</returns>
		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x0009EFDF File Offset: 0x0009D1DF
		public object CellValue1
		{
			get
			{
				return this.cellValue1;
			}
		}

		/// <summary>Gets the value of the second cell to compare.</summary>
		/// <returns>The value of the second cell.</returns>
		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06001FCE RID: 8142 RVA: 0x0009EFE7 File Offset: 0x0009D1E7
		public object CellValue2
		{
			get
			{
				return this.cellValue2;
			}
		}

		/// <summary>Gets the column being sorted. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewColumn" /> to sort.</returns>
		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x0009EFEF File Offset: 0x0009D1EF
		public DataGridViewColumn Column
		{
			get
			{
				return this.dataGridViewColumn;
			}
		}

		/// <summary>Gets the index of the row containing the first cell to compare.</summary>
		/// <returns>The index of the row containing the second cell.</returns>
		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x0009EFF7 File Offset: 0x0009D1F7
		public int RowIndex1
		{
			get
			{
				return this.rowIndex1;
			}
		}

		/// <summary>Gets the index of the row containing the second cell to compare.</summary>
		/// <returns>The index of the row containing the second cell.</returns>
		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x0009EFFF File Offset: 0x0009D1FF
		public int RowIndex2
		{
			get
			{
				return this.rowIndex2;
			}
		}

		/// <summary>Gets or sets a value indicating the order in which the compared cells will be sorted.</summary>
		/// <returns>Less than zero if the first cell will be sorted before the second cell; zero if the first cell and second cell have equivalent values; greater than zero if the second cell will be sorted before the first cell.</returns>
		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x0009F007 File Offset: 0x0009D207
		// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x0009F00F File Offset: 0x0009D20F
		public int SortResult
		{
			get
			{
				return this.sortResult;
			}
			set
			{
				this.sortResult = value;
			}
		}

		// Token: 0x04000DB8 RID: 3512
		private DataGridViewColumn dataGridViewColumn;

		// Token: 0x04000DB9 RID: 3513
		private object cellValue1;

		// Token: 0x04000DBA RID: 3514
		private object cellValue2;

		// Token: 0x04000DBB RID: 3515
		private int sortResult;

		// Token: 0x04000DBC RID: 3516
		private int rowIndex1;

		// Token: 0x04000DBD RID: 3517
		private int rowIndex2;
	}
}
