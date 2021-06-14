using System;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.RowsRemoved" /> event.</summary>
	// Token: 0x02000205 RID: 517
	public class DataGridViewRowsRemovedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowsRemovedEventArgs" /> class.</summary>
		/// <param name="rowIndex">The zero-based index of the row that was deleted, or the first deleted row if multiple rows were deleted. </param>
		/// <param name="rowCount">The number of rows that were deleted.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than 0.-or-
		///         <paramref name="rowCount" /> is less than 1.</exception>
		// Token: 0x06001F7D RID: 8061 RVA: 0x0009EC74 File Offset: 0x0009CE74
		public DataGridViewRowsRemovedEventArgs(int rowIndex, int rowCount)
		{
			if (rowIndex < 0)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"rowIndex",
					rowIndex.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (rowCount < 1)
			{
				throw new ArgumentOutOfRangeException("rowCount", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"rowCount",
					rowCount.ToString(CultureInfo.CurrentCulture),
					1.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.rowIndex = rowIndex;
			this.rowCount = rowCount;
		}

		/// <summary>Gets the zero-based index of the row deleted, or the first deleted row if multiple rows were deleted.</summary>
		/// <returns>The zero-based index of the row that was deleted, or the first deleted row if multiple rows were deleted.</returns>
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001F7E RID: 8062 RVA: 0x0009ED23 File Offset: 0x0009CF23
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		/// <summary>Gets the number of rows that were deleted.</summary>
		/// <returns>The number of deleted rows.</returns>
		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001F7F RID: 8063 RVA: 0x0009ED2B File Offset: 0x0009CF2B
		public int RowCount
		{
			get
			{
				return this.rowCount;
			}
		}

		// Token: 0x04000DAB RID: 3499
		private int rowIndex;

		// Token: 0x04000DAC RID: 3500
		private int rowCount;
	}
}
