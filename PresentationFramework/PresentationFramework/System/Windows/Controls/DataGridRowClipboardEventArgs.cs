using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.CopyingRowClipboardContent" /> event. </summary>
	// Token: 0x020004B5 RID: 1205
	public class DataGridRowClipboardEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridRowClipboardEventArgs" /> class. </summary>
		/// <param name="item">The data item for the row for which the event occurred.</param>
		/// <param name="startColumnDisplayIndex">The <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the first selected cell in the row.</param>
		/// <param name="endColumnDisplayIndex">The <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the last selected cell in the row. </param>
		/// <param name="isColumnHeadersRow">A value that indicates whether the row for which the event occurred represents the column headers. </param>
		// Token: 0x06004987 RID: 18823 RVA: 0x0014D1BE File Offset: 0x0014B3BE
		public DataGridRowClipboardEventArgs(object item, int startColumnDisplayIndex, int endColumnDisplayIndex, bool isColumnHeadersRow)
		{
			this._item = item;
			this._startColumnDisplayIndex = startColumnDisplayIndex;
			this._endColumnDisplayIndex = endColumnDisplayIndex;
			this._isColumnHeadersRow = isColumnHeadersRow;
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x0014D1EA File Offset: 0x0014B3EA
		internal DataGridRowClipboardEventArgs(object item, int startColumnDisplayIndex, int endColumnDisplayIndex, bool isColumnHeadersRow, int rowIndexHint) : this(item, startColumnDisplayIndex, endColumnDisplayIndex, isColumnHeadersRow)
		{
			this._rowIndexHint = rowIndexHint;
		}

		/// <summary>Gets the data item for the row for which the event occurred.</summary>
		/// <returns>The data item for the row for which the event occurred.</returns>
		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x06004989 RID: 18825 RVA: 0x0014D1FF File Offset: 0x0014B3FF
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		/// <summary>Gets a list of <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> values that represent the text values of the cells being copied. </summary>
		/// <returns>A list of <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> values that represent the text values of the cells being copied. </returns>
		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x0600498A RID: 18826 RVA: 0x0014D207 File Offset: 0x0014B407
		public List<DataGridClipboardCellContent> ClipboardRowContent
		{
			get
			{
				if (this._clipboardRowContent == null)
				{
					this._clipboardRowContent = new List<DataGridClipboardCellContent>();
				}
				return this._clipboardRowContent;
			}
		}

		/// <summary>Returns the <see cref="P:System.Windows.Controls.DataGridRowClipboardEventArgs.ClipboardRowContent" /> values as a string in the specified format. </summary>
		/// <param name="format">The data format in which to serialize the cell values. </param>
		/// <returns>The formatted string.</returns>
		// Token: 0x0600498B RID: 18827 RVA: 0x0014D224 File Offset: 0x0014B424
		public string FormatClipboardCellValues(string format)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.ClipboardRowContent.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridClipboardHelper.FormatCell(this.ClipboardRowContent[i].Content, i == 0, i == count - 1, stringBuilder, format);
			}
			return stringBuilder.ToString();
		}

		/// <summary>Gets the <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the first selected cell in the row.</summary>
		/// <returns>The <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the first selected cell in the row.</returns>
		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x0600498C RID: 18828 RVA: 0x0014D27A File Offset: 0x0014B47A
		public int StartColumnDisplayIndex
		{
			get
			{
				return this._startColumnDisplayIndex;
			}
		}

		/// <summary>Gets the <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the last selected cell in the row. </summary>
		/// <returns>The <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the last selected cell in the row. </returns>
		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x0600498D RID: 18829 RVA: 0x0014D282 File Offset: 0x0014B482
		public int EndColumnDisplayIndex
		{
			get
			{
				return this._endColumnDisplayIndex;
			}
		}

		/// <summary>Gets a value that indicates whether the row for which the event occurred represents the column headers. </summary>
		/// <returns>
		///     <see langword="true" /> if the row represents the column headers; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x0600498E RID: 18830 RVA: 0x0014D28A File Offset: 0x0014B48A
		public bool IsColumnHeadersRow
		{
			get
			{
				return this._isColumnHeadersRow;
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x0600498F RID: 18831 RVA: 0x0014D292 File Offset: 0x0014B492
		internal int RowIndexHint
		{
			get
			{
				return this._rowIndexHint;
			}
		}

		// Token: 0x04002A0D RID: 10765
		private int _startColumnDisplayIndex;

		// Token: 0x04002A0E RID: 10766
		private int _endColumnDisplayIndex;

		// Token: 0x04002A0F RID: 10767
		private object _item;

		// Token: 0x04002A10 RID: 10768
		private bool _isColumnHeadersRow;

		// Token: 0x04002A11 RID: 10769
		private List<DataGridClipboardCellContent> _clipboardRowContent;

		// Token: 0x04002A12 RID: 10770
		private int _rowIndexHint = -1;
	}
}
