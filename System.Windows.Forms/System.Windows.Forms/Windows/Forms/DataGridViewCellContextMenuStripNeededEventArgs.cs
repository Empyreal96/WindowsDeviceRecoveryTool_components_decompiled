using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellContextMenuStripNeeded" /> event. </summary>
	// Token: 0x02000196 RID: 406
	public class DataGridViewCellContextMenuStripNeededEventArgs : DataGridViewCellEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventArgs" /> class. </summary>
		/// <param name="columnIndex">The column index of cell that the event occurred for.</param>
		/// <param name="rowIndex">The row index of the cell that the event occurred for.</param>
		// Token: 0x06001ADB RID: 6875 RVA: 0x00086C68 File Offset: 0x00084E68
		public DataGridViewCellContextMenuStripNeededEventArgs(int columnIndex, int rowIndex) : base(columnIndex, rowIndex)
		{
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x00086C72 File Offset: 0x00084E72
		internal DataGridViewCellContextMenuStripNeededEventArgs(int columnIndex, int rowIndex, ContextMenuStrip contextMenuStrip) : base(columnIndex, rowIndex)
		{
			this.contextMenuStrip = contextMenuStrip;
		}

		/// <summary>Gets or sets the shortcut menu for the cell that raised the <see cref="E:System.Windows.Forms.DataGridView.CellContextMenuStripNeeded" /> event.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> for the cell. </returns>
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001ADD RID: 6877 RVA: 0x00086C83 File Offset: 0x00084E83
		// (set) Token: 0x06001ADE RID: 6878 RVA: 0x00086C8B File Offset: 0x00084E8B
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

		// Token: 0x04000C07 RID: 3079
		private ContextMenuStrip contextMenuStrip;
	}
}
