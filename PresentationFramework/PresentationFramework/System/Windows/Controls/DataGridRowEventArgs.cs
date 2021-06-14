using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.LoadingRow" /> and <see cref="E:System.Windows.Controls.DataGrid.UnloadingRow" /> events. </summary>
	// Token: 0x020004B9 RID: 1209
	public class DataGridRowEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridRowEventArgs" /> class. </summary>
		/// <param name="row">The row for which the event occurred. </param>
		// Token: 0x0600499A RID: 18842 RVA: 0x0014D309 File Offset: 0x0014B509
		public DataGridRowEventArgs(DataGridRow row)
		{
			this.Row = row;
		}

		/// <summary>Gets the row for which the event occurred. </summary>
		/// <returns>The row for which the event occurred. </returns>
		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x0600499B RID: 18843 RVA: 0x0014D318 File Offset: 0x0014B518
		// (set) Token: 0x0600499C RID: 18844 RVA: 0x0014D320 File Offset: 0x0014B520
		public DataGridRow Row { get; private set; }
	}
}
