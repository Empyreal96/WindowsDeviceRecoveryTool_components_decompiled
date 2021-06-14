using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.PreparingCellForEdit" /> event.</summary>
	// Token: 0x020004B3 RID: 1203
	public class DataGridPreparingCellForEditEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridPreparingCellForEditEventArgs" /> class. </summary>
		/// <param name="column">The column that contains the cell to be edited. </param>
		/// <param name="row">The row that contains the cell to be edited. </param>
		/// <param name="editingEventArgs">Information about the user gesture that caused the cell to enter edit mode.</param>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		// Token: 0x06004929 RID: 18729 RVA: 0x0014BCDD File Offset: 0x00149EDD
		public DataGridPreparingCellForEditEventArgs(DataGridColumn column, DataGridRow row, RoutedEventArgs editingEventArgs, FrameworkElement editingElement)
		{
			this._dataGridColumn = column;
			this._dataGridRow = row;
			this._editingEventArgs = editingEventArgs;
			this._editingElement = editingElement;
		}

		/// <summary>Gets the column that contains the cell to be edited. </summary>
		/// <returns>The column that contains the cell to be edited. </returns>
		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x0600492A RID: 18730 RVA: 0x0014BD02 File Offset: 0x00149F02
		public DataGridColumn Column
		{
			get
			{
				return this._dataGridColumn;
			}
		}

		/// <summary>Gets the row that contains the cell to be edited. </summary>
		/// <returns>The row that contains the cell to be edited. </returns>
		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x0600492B RID: 18731 RVA: 0x0014BD0A File Offset: 0x00149F0A
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		/// <summary>Gets information about the user gesture that caused the cell to enter edit mode.</summary>
		/// <returns>Information about the user gesture that caused the cell to enter edit mode.</returns>
		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x0600492C RID: 18732 RVA: 0x0014BD12 File Offset: 0x00149F12
		public RoutedEventArgs EditingEventArgs
		{
			get
			{
				return this._editingEventArgs;
			}
		}

		/// <summary>Gets the element that the column displays for a cell in editing mode.</summary>
		/// <returns>The element that the column displays for a cell in editing mode.</returns>
		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x0600492D RID: 18733 RVA: 0x0014BD1A File Offset: 0x00149F1A
		public FrameworkElement EditingElement
		{
			get
			{
				return this._editingElement;
			}
		}

		// Token: 0x040029E2 RID: 10722
		private DataGridColumn _dataGridColumn;

		// Token: 0x040029E3 RID: 10723
		private DataGridRow _dataGridRow;

		// Token: 0x040029E4 RID: 10724
		private RoutedEventArgs _editingEventArgs;

		// Token: 0x040029E5 RID: 10725
		private FrameworkElement _editingElement;
	}
}
