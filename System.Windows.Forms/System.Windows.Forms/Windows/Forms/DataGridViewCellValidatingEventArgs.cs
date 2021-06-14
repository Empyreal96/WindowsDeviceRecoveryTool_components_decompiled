using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellValidating" /> event of a <see cref="T:System.Windows.Forms.DataGridView" /> control. </summary>
	// Token: 0x020001A9 RID: 425
	public class DataGridViewCellValidatingEventArgs : CancelEventArgs
	{
		// Token: 0x06001B65 RID: 7013 RVA: 0x0008875A File Offset: 0x0008695A
		internal DataGridViewCellValidatingEventArgs(int columnIndex, int rowIndex, object formattedValue)
		{
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.formattedValue = formattedValue;
		}

		/// <summary>Gets the column index of the cell that needs to be validated.</summary>
		/// <returns>A zero-based integer that specifies the column index of the cell that needs to be validated.</returns>
		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001B66 RID: 7014 RVA: 0x00088777 File Offset: 0x00086977
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets the formatted contents of the cell that needs to be validated.</summary>
		/// <returns>A reference to the formatted value.</returns>
		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001B67 RID: 7015 RVA: 0x0008877F File Offset: 0x0008697F
		public object FormattedValue
		{
			get
			{
				return this.formattedValue;
			}
		}

		/// <summary>Gets the row index of the cell that needs to be validated.</summary>
		/// <returns>A zero-based integer that specifies the row index of the cell that needs to be validated.</returns>
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001B68 RID: 7016 RVA: 0x00088787 File Offset: 0x00086987
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04000C50 RID: 3152
		private int rowIndex;

		// Token: 0x04000C51 RID: 3153
		private int columnIndex;

		// Token: 0x04000C52 RID: 3154
		private object formattedValue;
	}
}
