using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.LoadingRowDetails" />, <see cref="E:System.Windows.Controls.DataGrid.UnloadingRowDetails" />, and <see cref="E:System.Windows.Controls.DataGrid.RowDetailsVisibilityChanged" /> events.</summary>
	// Token: 0x020004B6 RID: 1206
	public class DataGridRowDetailsEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridRowDetailsEventArgs" /> class. </summary>
		/// <param name="row">The row for which the event occurred.</param>
		/// <param name="detailsElement">The row details section as a framework element. </param>
		// Token: 0x06004990 RID: 18832 RVA: 0x0014D29A File Offset: 0x0014B49A
		public DataGridRowDetailsEventArgs(DataGridRow row, FrameworkElement detailsElement)
		{
			this.Row = row;
			this.DetailsElement = detailsElement;
		}

		/// <summary>Gets the row details section as a framework element. </summary>
		/// <returns>The row details section as a framework element. </returns>
		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x06004991 RID: 18833 RVA: 0x0014D2B0 File Offset: 0x0014B4B0
		// (set) Token: 0x06004992 RID: 18834 RVA: 0x0014D2B8 File Offset: 0x0014B4B8
		public FrameworkElement DetailsElement { get; private set; }

		/// <summary>Gets the row for which the event occurred.</summary>
		/// <returns>The row for which the event occurred.</returns>
		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x06004993 RID: 18835 RVA: 0x0014D2C1 File Offset: 0x0014B4C1
		// (set) Token: 0x06004994 RID: 18836 RVA: 0x0014D2C9 File Offset: 0x0014B4C9
		public DataGridRow Row { get; private set; }
	}
}
