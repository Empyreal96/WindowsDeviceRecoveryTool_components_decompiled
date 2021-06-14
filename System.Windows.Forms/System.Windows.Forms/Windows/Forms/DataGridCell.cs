using System;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Identifies a cell in the grid.</summary>
	// Token: 0x0200016E RID: 366
	public struct DataGridCell
	{
		/// <summary>Gets or sets the number of a column in the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
		/// <returns>The number of the column.</returns>
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x000484A4 File Offset: 0x000466A4
		// (set) Token: 0x060012FD RID: 4861 RVA: 0x000484AC File Offset: 0x000466AC
		public int ColumnNumber
		{
			get
			{
				return this.columnNumber;
			}
			set
			{
				this.columnNumber = value;
			}
		}

		/// <summary>Gets or sets the number of a row in the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
		/// <returns>The number of the row.</returns>
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x000484B5 File Offset: 0x000466B5
		// (set) Token: 0x060012FF RID: 4863 RVA: 0x000484BD File Offset: 0x000466BD
		public int RowNumber
		{
			get
			{
				return this.rowNumber;
			}
			set
			{
				this.rowNumber = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridCell" /> class.</summary>
		/// <param name="r">The number of a row in the <see cref="T:System.Windows.Forms.DataGrid" />. </param>
		/// <param name="c">The number of a column in the <see cref="T:System.Windows.Forms.DataGrid" />. </param>
		// Token: 0x06001300 RID: 4864 RVA: 0x000484C6 File Offset: 0x000466C6
		public DataGridCell(int r, int c)
		{
			this.rowNumber = r;
			this.columnNumber = c;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.DataGridCell" /> is identical to a second <see cref="T:System.Windows.Forms.DataGridCell" />.</summary>
		/// <param name="o">An object you are to comparing. </param>
		/// <returns>
		///     <see langword="true" /> if the second object is identical to the first; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001301 RID: 4865 RVA: 0x000484D8 File Offset: 0x000466D8
		public override bool Equals(object o)
		{
			if (o is DataGridCell)
			{
				DataGridCell dataGridCell = (DataGridCell)o;
				return dataGridCell.RowNumber == this.RowNumber && dataGridCell.ColumnNumber == this.ColumnNumber;
			}
			return false;
		}

		/// <summary>Gets a hash value that can be added to a <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>A number that uniquely identifies the <see cref="T:System.Windows.Forms.DataGridCell" /> in a <see cref="T:System.Collections.Hashtable" />.</returns>
		// Token: 0x06001302 RID: 4866 RVA: 0x00048516 File Offset: 0x00046716
		public override int GetHashCode()
		{
			return (~this.rowNumber * (this.columnNumber + 1) & 16776960) >> 8;
		}

		/// <summary>Gets the row number and column number of the cell.</summary>
		/// <returns>A string containing the row number and column number.</returns>
		// Token: 0x06001303 RID: 4867 RVA: 0x00048530 File Offset: 0x00046730
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridCell {RowNumber = ",
				this.RowNumber.ToString(CultureInfo.CurrentCulture),
				", ColumnNumber = ",
				this.ColumnNumber.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		// Token: 0x04000984 RID: 2436
		private int rowNumber;

		// Token: 0x04000985 RID: 2437
		private int columnNumber;
	}
}
