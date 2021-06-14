using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.RowDividerDoubleClick" /> event of a <see cref="T:System.Windows.Forms.DataGridView" />. </summary>
	// Token: 0x020001FC RID: 508
	public class DataGridViewRowDividerDoubleClickEventArgs : HandledMouseEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowDividerDoubleClickEventArgs" /> class. </summary>
		/// <param name="rowIndex">The index of the row above the row divider that was double-clicked.</param>
		/// <param name="e">A new <see cref="T:System.Windows.Forms.HandledMouseEventArgs" /> containing the inherited event data.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than -1.</exception>
		// Token: 0x06001F23 RID: 7971 RVA: 0x0009CC60 File Offset: 0x0009AE60
		public DataGridViewRowDividerDoubleClickEventArgs(int rowIndex, HandledMouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta, e.Handled)
		{
			if (rowIndex < -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			this.rowIndex = rowIndex;
		}

		/// <summary>The index of the row above the row divider that was double-clicked.</summary>
		/// <returns>The index of the row above the divider.</returns>
		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001F24 RID: 7972 RVA: 0x0009CCAD File Offset: 0x0009AEAD
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04000D7A RID: 3450
		private int rowIndex;
	}
}
