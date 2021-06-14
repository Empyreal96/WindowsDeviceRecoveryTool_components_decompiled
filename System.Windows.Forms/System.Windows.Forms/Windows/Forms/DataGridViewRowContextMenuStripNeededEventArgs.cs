using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.RowContextMenuStripNeeded" /> event. </summary>
	// Token: 0x020001FA RID: 506
	public class DataGridViewRowContextMenuStripNeededEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventArgs" /> class. </summary>
		/// <param name="rowIndex">The index of the row.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than -1.</exception>
		// Token: 0x06001F1B RID: 7963 RVA: 0x0009CBA5 File Offset: 0x0009ADA5
		public DataGridViewRowContextMenuStripNeededEventArgs(int rowIndex)
		{
			if (rowIndex < -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			this.rowIndex = rowIndex;
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0009CBC3 File Offset: 0x0009ADC3
		internal DataGridViewRowContextMenuStripNeededEventArgs(int rowIndex, ContextMenuStrip contextMenuStrip) : this(rowIndex)
		{
			this.contextMenuStrip = contextMenuStrip;
		}

		/// <summary>Gets the index of the row that is requesting a shortcut menu.</summary>
		/// <returns>The zero-based index of the row that is requesting a shortcut menu.</returns>
		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001F1D RID: 7965 RVA: 0x0009CBD3 File Offset: 0x0009ADD3
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		/// <summary>Gets or sets the shortcut menu for the row that raised the <see cref="E:System.Windows.Forms.DataGridView.RowContextMenuStripNeeded" /> event.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> in use.</returns>
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001F1E RID: 7966 RVA: 0x0009CBDB File Offset: 0x0009ADDB
		// (set) Token: 0x06001F1F RID: 7967 RVA: 0x0009CBE3 File Offset: 0x0009ADE3
		public ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return this.contextMenuStrip;
			}
			set
			{
				this.contextMenuStrip = value;
			}
		}

		// Token: 0x04000D78 RID: 3448
		private int rowIndex;

		// Token: 0x04000D79 RID: 3449
		private ContextMenuStrip contextMenuStrip;
	}
}
