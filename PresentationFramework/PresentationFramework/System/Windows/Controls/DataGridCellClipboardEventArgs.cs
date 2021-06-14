using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGridColumn.CopyingCellClipboardContent" /> and <see cref="E:System.Windows.Controls.DataGridColumn.PastingCellClipboardContent" /> events.</summary>
	// Token: 0x02000497 RID: 1175
	public class DataGridCellClipboardEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridCellClipboardEventArgs" /> class. </summary>
		/// <param name="item">The data item for the row that contains the cell for which the event occurred.</param>
		/// <param name="column">The column that contains the cell for which the event occurred. </param>
		/// <param name="content">The text value of the cell for which the event occurred. </param>
		// Token: 0x0600471E RID: 18206 RVA: 0x00142772 File Offset: 0x00140972
		public DataGridCellClipboardEventArgs(object item, DataGridColumn column, object content)
		{
			this._item = item;
			this._column = column;
			this._content = content;
		}

		/// <summary>Gets or sets the text value of the cell for which the event occurred.</summary>
		/// <returns>The text value of the cell for which the event occurred.</returns>
		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x0600471F RID: 18207 RVA: 0x0014278F File Offset: 0x0014098F
		// (set) Token: 0x06004720 RID: 18208 RVA: 0x00142797 File Offset: 0x00140997
		public object Content
		{
			get
			{
				return this._content;
			}
			set
			{
				this._content = value;
			}
		}

		/// <summary>Gets the data item for the row that contains the cell for which the event occurred.</summary>
		/// <returns>The data item for the row that contains the cell for which the event occurred.</returns>
		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x06004721 RID: 18209 RVA: 0x001427A0 File Offset: 0x001409A0
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		/// <summary>Gets the column that contains the cell for which the event occurred. </summary>
		/// <returns>The column that contains the cell for which the event occurred. </returns>
		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x06004722 RID: 18210 RVA: 0x001427A8 File Offset: 0x001409A8
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x0400294B RID: 10571
		private object _content;

		// Token: 0x0400294C RID: 10572
		private object _item;

		// Token: 0x0400294D RID: 10573
		private DataGridColumn _column;
	}
}
