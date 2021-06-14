using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.CellEditEnding" /> event. </summary>
	// Token: 0x02000498 RID: 1176
	public class DataGridCellEditEndingEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridCellEditEndingEventArgs" /> class. </summary>
		/// <param name="column">The column that contains the cell for which the event occurred. </param>
		/// <param name="row">The row that contains the cell for which the event occurred. </param>
		/// <param name="editingElement">The element that the cell displays in editing mode.</param>
		/// <param name="editAction">A value that indicates whether the edit was canceled or committed. </param>
		// Token: 0x06004723 RID: 18211 RVA: 0x001427B0 File Offset: 0x001409B0
		public DataGridCellEditEndingEventArgs(DataGridColumn column, DataGridRow row, FrameworkElement editingElement, DataGridEditAction editAction)
		{
			this._dataGridColumn = column;
			this._dataGridRow = row;
			this._editingElement = editingElement;
			this._editAction = editAction;
		}

		/// <summary>Gets or sets a value that indicates whether the event should be canceled. </summary>
		/// <returns>
		///     <see langword="true" /> if the event should be canceled; otherwise, <see langword="false" />. The default is <see langword="false" />. </returns>
		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x06004724 RID: 18212 RVA: 0x001427D5 File Offset: 0x001409D5
		// (set) Token: 0x06004725 RID: 18213 RVA: 0x001427DD File Offset: 0x001409DD
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

		/// <summary>Gets the column that contains the cell for which the event occurred. </summary>
		/// <returns>The column that contains the cell for which the event occurred. </returns>
		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x06004726 RID: 18214 RVA: 0x001427E6 File Offset: 0x001409E6
		public DataGridColumn Column
		{
			get
			{
				return this._dataGridColumn;
			}
		}

		/// <summary>Gets the row that contains the cell for which the event occurred. </summary>
		/// <returns>The row that contains the cell for which the event occurred. </returns>
		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x06004727 RID: 18215 RVA: 0x001427EE File Offset: 0x001409EE
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		/// <summary>Gets the element that the cell displays in editing mode.</summary>
		/// <returns>The element that the cell displays in editing mode.</returns>
		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x06004728 RID: 18216 RVA: 0x001427F6 File Offset: 0x001409F6
		public FrameworkElement EditingElement
		{
			get
			{
				return this._editingElement;
			}
		}

		/// <summary>Gets a value that indicates whether the edit was canceled or committed. </summary>
		/// <returns>A value that indicates whether the edit was canceled or committed. </returns>
		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x06004729 RID: 18217 RVA: 0x001427FE File Offset: 0x001409FE
		public DataGridEditAction EditAction
		{
			get
			{
				return this._editAction;
			}
		}

		// Token: 0x0400294E RID: 10574
		private bool _cancel;

		// Token: 0x0400294F RID: 10575
		private DataGridColumn _dataGridColumn;

		// Token: 0x04002950 RID: 10576
		private DataGridRow _dataGridRow;

		// Token: 0x04002951 RID: 10577
		private FrameworkElement _editingElement;

		// Token: 0x04002952 RID: 10578
		private DataGridEditAction _editAction;
	}
}
