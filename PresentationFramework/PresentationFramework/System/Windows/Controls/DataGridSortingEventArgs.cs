using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.Sorting" /> event. </summary>
	// Token: 0x020004BC RID: 1212
	public class DataGridSortingEventArgs : DataGridColumnEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridSortingEventArgs" /> class.</summary>
		/// <param name="column">The column that is being sorted.</param>
		// Token: 0x0600499D RID: 18845 RVA: 0x0014A062 File Offset: 0x00148262
		public DataGridSortingEventArgs(DataGridColumn column) : base(column)
		{
		}

		/// <summary>Gets or sets a value that specifies whether the routed event is handled.</summary>
		/// <returns>
		///     <see langword="true" /> if the event has been handled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x0600499E RID: 18846 RVA: 0x0014D329 File Offset: 0x0014B529
		// (set) Token: 0x0600499F RID: 18847 RVA: 0x0014D331 File Offset: 0x0014B531
		public bool Handled
		{
			get
			{
				return this._handled;
			}
			set
			{
				this._handled = value;
			}
		}

		// Token: 0x04002A24 RID: 10788
		private bool _handled;
	}
}
