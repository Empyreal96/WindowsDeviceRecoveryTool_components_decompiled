using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.RowsAdded" /> event. </summary>
	// Token: 0x02000204 RID: 516
	public class DataGridViewRowsAddedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowsAddedEventArgs" /> class. </summary>
		/// <param name="rowIndex">The index of the first added row.</param>
		/// <param name="rowCount">The number of rows that have been added.</param>
		// Token: 0x06001F7A RID: 8058 RVA: 0x0009EC4D File Offset: 0x0009CE4D
		public DataGridViewRowsAddedEventArgs(int rowIndex, int rowCount)
		{
			this.rowIndex = rowIndex;
			this.rowCount = rowCount;
		}

		/// <summary>Gets the index of the first added row.</summary>
		/// <returns>The index of the first added row.</returns>
		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001F7B RID: 8059 RVA: 0x0009EC63 File Offset: 0x0009CE63
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		/// <summary>Gets the number of rows that have been added.</summary>
		/// <returns>The number of rows that have been added.</returns>
		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x0009EC6B File Offset: 0x0009CE6B
		public int RowCount
		{
			get
			{
				return this.rowCount;
			}
		}

		// Token: 0x04000DA9 RID: 3497
		private int rowIndex;

		// Token: 0x04000DAA RID: 3498
		private int rowCount;
	}
}
