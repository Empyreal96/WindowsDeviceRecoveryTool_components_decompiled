using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.BeginningEdit" /> event. </summary>
	// Token: 0x02000494 RID: 1172
	public class DataGridBeginningEditEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridBeginningEditEventArgs" /> class. </summary>
		/// <param name="column">The column that contains the cell to be edited. </param>
		/// <param name="row">The row that contains the cell to be edited. </param>
		/// <param name="editingEventArgs">Information about the user gesture that caused the cell to enter edit mode.</param>
		// Token: 0x060046CB RID: 18123 RVA: 0x0014157D File Offset: 0x0013F77D
		public DataGridBeginningEditEventArgs(DataGridColumn column, DataGridRow row, RoutedEventArgs editingEventArgs)
		{
			this._dataGridColumn = column;
			this._dataGridRow = row;
			this._editingEventArgs = editingEventArgs;
		}

		/// <summary>Gets or sets a value that indicates whether the event should be canceled. </summary>
		/// <returns>
		///     <see langword="true" /> if the event should be canceled; otherwise, <see langword="false" />. The default is <see langword="false" />. </returns>
		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x060046CC RID: 18124 RVA: 0x0014159A File Offset: 0x0013F79A
		// (set) Token: 0x060046CD RID: 18125 RVA: 0x001415A2 File Offset: 0x0013F7A2
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		/// <summary>Gets the column that contains the cell to be edited. </summary>
		/// <returns>The column that contains the cell to be edited. </returns>
		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x060046CE RID: 18126 RVA: 0x001415AB File Offset: 0x0013F7AB
		public DataGridColumn Column
		{
			get
			{
				return this._dataGridColumn;
			}
		}

		/// <summary>Gets the row that contains the cell to be edited. </summary>
		/// <returns>The row that contains the cell to be edited. </returns>
		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x060046CF RID: 18127 RVA: 0x001415B3 File Offset: 0x0013F7B3
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		/// <summary>Gets information about the user gesture that caused the cell to enter edit mode.</summary>
		/// <returns>Information about the user gesture that caused the cell to enter edit mode.</returns>
		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x060046D0 RID: 18128 RVA: 0x001415BB File Offset: 0x0013F7BB
		public RoutedEventArgs EditingEventArgs
		{
			get
			{
				return this._editingEventArgs;
			}
		}

		// Token: 0x04002939 RID: 10553
		private bool _cancel;

		// Token: 0x0400293A RID: 10554
		private DataGridColumn _dataGridColumn;

		// Token: 0x0400293B RID: 10555
		private DataGridRow _dataGridRow;

		// Token: 0x0400293C RID: 10556
		private RoutedEventArgs _editingEventArgs;
	}
}
