using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.ColumnDividerDoubleClick" /> event of a <see cref="T:System.Windows.Forms.DataGridView" />. </summary>
	// Token: 0x020001B2 RID: 434
	public class DataGridViewColumnDividerDoubleClickEventArgs : HandledMouseEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewColumnDividerDoubleClickEventArgs" /> class. </summary>
		/// <param name="columnIndex">The index of the column next to the column divider that was double-clicked. </param>
		/// <param name="e">A new <see cref="T:System.Windows.Forms.HandledMouseEventArgs" /> containing the inherited event data. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="columnIndex" /> is less than -1.</exception>
		// Token: 0x06001C59 RID: 7257 RVA: 0x0008D7A8 File Offset: 0x0008B9A8
		public DataGridViewColumnDividerDoubleClickEventArgs(int columnIndex, HandledMouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta, e.Handled)
		{
			if (columnIndex < -1)
			{
				throw new ArgumentOutOfRangeException("columnIndex");
			}
			this.columnIndex = columnIndex;
		}

		/// <summary>The index of the column next to the column divider that was double-clicked.</summary>
		/// <returns>The index of the column next to the divider. </returns>
		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001C5A RID: 7258 RVA: 0x0008D7F5 File Offset: 0x0008B9F5
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x04000C96 RID: 3222
		private int columnIndex;
	}
}
