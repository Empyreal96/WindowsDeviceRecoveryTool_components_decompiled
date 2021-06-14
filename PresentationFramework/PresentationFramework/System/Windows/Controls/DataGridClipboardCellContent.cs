using System;

namespace System.Windows.Controls
{
	/// <summary>Encapsulates the value and location of a <see cref="T:System.Windows.Controls.DataGrid" /> cell for use when copying content to the Clipboard.</summary>
	// Token: 0x0200049C RID: 1180
	public struct DataGridClipboardCellContent
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> structure. </summary>
		/// <param name="item">The data item for the row that contains the cell being copied. </param>
		/// <param name="column">The column that contains the cell being copied. </param>
		/// <param name="content">The text value of the cell being copied. </param>
		// Token: 0x06004792 RID: 18322 RVA: 0x00145137 File Offset: 0x00143337
		public DataGridClipboardCellContent(object item, DataGridColumn column, object content)
		{
			this._item = item;
			this._column = column;
			this._content = content;
		}

		/// <summary>Gets the data item for the row that contains the cell being copied. </summary>
		/// <returns>The data item for the row that contains the cell being copied. </returns>
		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06004793 RID: 18323 RVA: 0x0014514E File Offset: 0x0014334E
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		/// <summary>Gets the column that contains the cell being copied. </summary>
		/// <returns>The column that contains the cell being copied. </returns>
		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x06004794 RID: 18324 RVA: 0x00145156 File Offset: 0x00143356
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		/// <summary>Gets the text value of the cell being copied.</summary>
		/// <returns>The text value of the cell being copied.</returns>
		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x06004795 RID: 18325 RVA: 0x0014515E File Offset: 0x0014335E
		public object Content
		{
			get
			{
				return this._content;
			}
		}

		/// <summary>Indicates whether the current and specified <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instances are equivalent.</summary>
		/// <param name="data">The <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instance to compare with the current <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instance.</param>
		/// <returns>
		///     <see langword="true" /> if the current and specified <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instances have the same <see cref="P:System.Windows.Controls.DataGridClipboardCellContent.Item" />, <see cref="P:System.Windows.Controls.DataGridClipboardCellContent.Column" />, and <see cref="P:System.Windows.Controls.DataGridClipboardCellContent.Content" /> property values; otherwise, <see langword="false" />. </returns>
		// Token: 0x06004796 RID: 18326 RVA: 0x00145168 File Offset: 0x00143368
		public override bool Equals(object data)
		{
			if (data is DataGridClipboardCellContent)
			{
				DataGridClipboardCellContent dataGridClipboardCellContent = (DataGridClipboardCellContent)data;
				return this._column == dataGridClipboardCellContent._column && this._content == dataGridClipboardCellContent._content && this._item == dataGridClipboardCellContent._item;
			}
			return false;
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instance.</summary>
		/// <returns>The hash code for this <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instance.</returns>
		// Token: 0x06004797 RID: 18327 RVA: 0x001451B4 File Offset: 0x001433B4
		public override int GetHashCode()
		{
			return ((this._column == null) ? 0 : this._column.GetHashCode()) ^ ((this._content == null) ? 0 : this._content.GetHashCode()) ^ ((this._item == null) ? 0 : this._item.GetHashCode());
		}

		/// <summary>Indicates whether the specified <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instances are equivalent.</summary>
		/// <param name="clipboardCellContent1">The first <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instance to be compared.</param>
		/// <param name="clipboardCellContent2">The second <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instance to be compared.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instances have the same <see cref="P:System.Windows.Controls.DataGridClipboardCellContent.Item" />, <see cref="P:System.Windows.Controls.DataGridClipboardCellContent.Column" />, and <see cref="P:System.Windows.Controls.DataGridClipboardCellContent.Content" /> property values; otherwise, <see langword="false" />. </returns>
		// Token: 0x06004798 RID: 18328 RVA: 0x00145205 File Offset: 0x00143405
		public static bool operator ==(DataGridClipboardCellContent clipboardCellContent1, DataGridClipboardCellContent clipboardCellContent2)
		{
			return clipboardCellContent1._column == clipboardCellContent2._column && clipboardCellContent1._content == clipboardCellContent2._content && clipboardCellContent1._item == clipboardCellContent2._item;
		}

		/// <summary>Indicates whether the specified <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instances are not equivalent.</summary>
		/// <param name="clipboardCellContent1">The first <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instance to be compared.</param>
		/// <param name="clipboardCellContent2">The second <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instance to be compared.</param>
		/// <returns>
		///     <see langword="true" /> if the current and specified <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> instances do not have the same <see cref="P:System.Windows.Controls.DataGridClipboardCellContent.Item" />, <see cref="P:System.Windows.Controls.DataGridClipboardCellContent.Column" />, and <see cref="P:System.Windows.Controls.DataGridClipboardCellContent.Content" /> property values; otherwise, <see langword="false" />. </returns>
		// Token: 0x06004799 RID: 18329 RVA: 0x00145233 File Offset: 0x00143433
		public static bool operator !=(DataGridClipboardCellContent clipboardCellContent1, DataGridClipboardCellContent clipboardCellContent2)
		{
			return clipboardCellContent1._column != clipboardCellContent2._column || clipboardCellContent1._content != clipboardCellContent2._content || clipboardCellContent1._item != clipboardCellContent2._item;
		}

		// Token: 0x0400295F RID: 10591
		private object _item;

		// Token: 0x04002960 RID: 10592
		private DataGridColumn _column;

		// Token: 0x04002961 RID: 10593
		private object _content;
	}
}
