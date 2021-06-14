using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.ColumnDisplayIndexChanged" /> and <see cref="E:System.Windows.Controls.DataGrid.ColumnReordered" /> events.</summary>
	// Token: 0x020004A2 RID: 1186
	public class DataGridColumnEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridColumnEventArgs" /> class.</summary>
		/// <param name="column">The column related to the event.</param>
		// Token: 0x06004878 RID: 18552 RVA: 0x00149A62 File Offset: 0x00147C62
		public DataGridColumnEventArgs(DataGridColumn column)
		{
			this._column = column;
		}

		/// <summary>Gets the column related to the event.</summary>
		/// <returns>An object that represents the column related to the event.</returns>
		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x06004879 RID: 18553 RVA: 0x00149A71 File Offset: 0x00147C71
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x0400299A RID: 10650
		private DataGridColumn _column;
	}
}
