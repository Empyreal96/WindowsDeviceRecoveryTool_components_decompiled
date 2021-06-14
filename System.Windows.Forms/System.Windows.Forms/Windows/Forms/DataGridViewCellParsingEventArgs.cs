using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellParsing" /> event of a <see cref="T:System.Windows.Forms.DataGridView" /> control. </summary>
	// Token: 0x020001A0 RID: 416
	public class DataGridViewCellParsingEventArgs : ConvertEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellParsingEventArgs" /> class. </summary>
		/// <param name="rowIndex">The row index of the cell that was changed.</param>
		/// <param name="columnIndex">The column index of the cell that was changed.</param>
		/// <param name="value">The new value.</param>
		/// <param name="desiredType">The type of the new value.</param>
		/// <param name="inheritedCellStyle">The style applied to the cell that was changed.</param>
		// Token: 0x06001B17 RID: 6935 RVA: 0x00087541 File Offset: 0x00085741
		public DataGridViewCellParsingEventArgs(int rowIndex, int columnIndex, object value, Type desiredType, DataGridViewCellStyle inheritedCellStyle) : base(value, desiredType)
		{
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.inheritedCellStyle = inheritedCellStyle;
		}

		/// <summary>Gets the row index of the cell that requires parsing.</summary>
		/// <returns>The row index of the cell that was changed.</returns>
		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x00087562 File Offset: 0x00085762
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		/// <summary>Gets the column index of the cell data that requires parsing.</summary>
		/// <returns>The column index of the cell that was changed.</returns>
		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0008756A File Offset: 0x0008576A
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets or sets the style applied to the edited cell.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the current style of the cell being edited. The default value is the value of the cell <see cref="P:System.Windows.Forms.DataGridViewCell.InheritedStyle" /> property.</returns>
		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001B1A RID: 6938 RVA: 0x00087572 File Offset: 0x00085772
		// (set) Token: 0x06001B1B RID: 6939 RVA: 0x0008757A File Offset: 0x0008577A
		public DataGridViewCellStyle InheritedCellStyle
		{
			get
			{
				return this.inheritedCellStyle;
			}
			set
			{
				this.inheritedCellStyle = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether a cell's value has been successfully parsed.</summary>
		/// <returns>
		///     <see langword="true" /> if the cell's value has been successfully parsed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001B1C RID: 6940 RVA: 0x00087583 File Offset: 0x00085783
		// (set) Token: 0x06001B1D RID: 6941 RVA: 0x0008758B File Offset: 0x0008578B
		public bool ParsingApplied
		{
			get
			{
				return this.parsingApplied;
			}
			set
			{
				this.parsingApplied = value;
			}
		}

		// Token: 0x04000C27 RID: 3111
		private int rowIndex;

		// Token: 0x04000C28 RID: 3112
		private int columnIndex;

		// Token: 0x04000C29 RID: 3113
		private DataGridViewCellStyle inheritedCellStyle;

		// Token: 0x04000C2A RID: 3114
		private bool parsingApplied;
	}
}
