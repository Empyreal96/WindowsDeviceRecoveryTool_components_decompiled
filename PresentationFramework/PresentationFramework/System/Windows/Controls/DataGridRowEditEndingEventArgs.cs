using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.RowEditEnding" /> event. </summary>
	// Token: 0x020004B8 RID: 1208
	public class DataGridRowEditEndingEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridRowEditEndingEventArgs" /> class. </summary>
		/// <param name="row">The row for which the event occurred. </param>
		/// <param name="editAction">A value that indicates whether the edit was canceled or committed. </param>
		// Token: 0x06004995 RID: 18837 RVA: 0x0014D2D2 File Offset: 0x0014B4D2
		public DataGridRowEditEndingEventArgs(DataGridRow row, DataGridEditAction editAction)
		{
			this._dataGridRow = row;
			this._editAction = editAction;
		}

		/// <summary>Gets or sets a value that indicates whether the event should be canceled. </summary>
		/// <returns>
		///     <see langword="true" /> if the event should be canceled; otherwise, <see langword="false" />. The default is <see langword="false" />. </returns>
		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06004996 RID: 18838 RVA: 0x0014D2E8 File Offset: 0x0014B4E8
		// (set) Token: 0x06004997 RID: 18839 RVA: 0x0014D2F0 File Offset: 0x0014B4F0
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

		/// <summary>Gets the row for which the event occurred. </summary>
		/// <returns>The row for which the event occurred. </returns>
		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06004998 RID: 18840 RVA: 0x0014D2F9 File Offset: 0x0014B4F9
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		/// <summary>Gets a value that indicates whether the edit was canceled or committed. </summary>
		/// <returns>A value that indicates whether the edit was canceled or committed. </returns>
		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06004999 RID: 18841 RVA: 0x0014D301 File Offset: 0x0014B501
		public DataGridEditAction EditAction
		{
			get
			{
				return this._editAction;
			}
		}

		// Token: 0x04002A19 RID: 10777
		private bool _cancel;

		// Token: 0x04002A1A RID: 10778
		private DataGridRow _dataGridRow;

		// Token: 0x04002A1B RID: 10779
		private DataGridEditAction _editAction;
	}
}
