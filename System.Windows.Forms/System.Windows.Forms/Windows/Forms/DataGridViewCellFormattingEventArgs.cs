using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellFormatting" /> event of a <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
	// Token: 0x0200019A RID: 410
	public class DataGridViewCellFormattingEventArgs : ConvertEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellFormattingEventArgs" /> class.</summary>
		/// <param name="columnIndex">The column index of the cell that caused the event.</param>
		/// <param name="rowIndex">The row index of the cell that caused the event.</param>
		/// <param name="value">The cell's contents.</param>
		/// <param name="desiredType">The type to convert <paramref name="value" /> to. </param>
		/// <param name="cellStyle">The style of the cell that caused the event.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="columnIndex" /> is less than -1-or-
		///         <paramref name="rowIndex" /> is less than -1.</exception>
		// Token: 0x06001AE9 RID: 6889 RVA: 0x00086D81 File Offset: 0x00084F81
		public DataGridViewCellFormattingEventArgs(int columnIndex, int rowIndex, object value, Type desiredType, DataGridViewCellStyle cellStyle) : base(value, desiredType)
		{
			if (columnIndex < -1)
			{
				throw new ArgumentOutOfRangeException("columnIndex");
			}
			if (rowIndex < -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			this.columnIndex = columnIndex;
			this.rowIndex = rowIndex;
			this.cellStyle = cellStyle;
		}

		/// <summary>Gets or sets the style of the cell that is being formatted.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the display style of the cell being formatted. The default is the value of the cell's <see cref="P:System.Windows.Forms.DataGridViewCell.InheritedStyle" /> property. </returns>
		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x00086DC0 File Offset: 0x00084FC0
		// (set) Token: 0x06001AEB RID: 6891 RVA: 0x00086DC8 File Offset: 0x00084FC8
		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.cellStyle;
			}
			set
			{
				this.cellStyle = value;
			}
		}

		/// <summary>Gets the column index of the cell that is being formatted.</summary>
		/// <returns>The column index of the cell that is being formatted.</returns>
		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001AEC RID: 6892 RVA: 0x00086DD1 File Offset: 0x00084FD1
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets or sets a value indicating whether the cell value has been successfully formatted.</summary>
		/// <returns>
		///     <see langword="true" /> if the formatting for the cell value has been handled; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x00086DD9 File Offset: 0x00084FD9
		// (set) Token: 0x06001AEE RID: 6894 RVA: 0x00086DE1 File Offset: 0x00084FE1
		public bool FormattingApplied
		{
			get
			{
				return this.formattingApplied;
			}
			set
			{
				this.formattingApplied = value;
			}
		}

		/// <summary>Gets the row index of the cell that is being formatted.</summary>
		/// <returns>The row index of the cell that is being formatted.</returns>
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x00086DEA File Offset: 0x00084FEA
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04000C0B RID: 3083
		private int columnIndex;

		// Token: 0x04000C0C RID: 3084
		private int rowIndex;

		// Token: 0x04000C0D RID: 3085
		private DataGridViewCellStyle cellStyle;

		// Token: 0x04000C0E RID: 3086
		private bool formattingApplied;
	}
}
