using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for mouse events raised by a <see cref="T:System.Windows.Forms.DataGridView" /> whenever the mouse is moved within a <see cref="T:System.Windows.Forms.DataGridViewCell" />. </summary>
	// Token: 0x0200019E RID: 414
	public class DataGridViewCellMouseEventArgs : MouseEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> class.</summary>
		/// <param name="columnIndex">The cell's zero-based column index.</param>
		/// <param name="rowIndex">The cell's zero-based row index.</param>
		/// <param name="localX">The x-coordinate of the mouse, in pixels.</param>
		/// <param name="localY">The y-coordinate of the mouse, in pixels.</param>
		/// <param name="e">The originating <see cref="T:System.Windows.Forms.MouseEventArgs" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="columnIndex" /> is less than -1.-or-
		///         <paramref name="rowIndex" /> is less than -1.</exception>
		// Token: 0x06001B02 RID: 6914 RVA: 0x000870E0 File Offset: 0x000852E0
		public DataGridViewCellMouseEventArgs(int columnIndex, int rowIndex, int localX, int localY, MouseEventArgs e) : base(e.Button, e.Clicks, localX, localY, e.Delta)
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
		}

		/// <summary>Gets the zero-based column index of the cell.</summary>
		/// <returns>An integer specifying the column index.</returns>
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x00087137 File Offset: 0x00085337
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets the zero-based row index of the cell.</summary>
		/// <returns>An integer specifying the row index.</returns>
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001B04 RID: 6916 RVA: 0x0008713F File Offset: 0x0008533F
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04000C18 RID: 3096
		private int rowIndex;

		// Token: 0x04000C19 RID: 3097
		private int columnIndex;
	}
}
