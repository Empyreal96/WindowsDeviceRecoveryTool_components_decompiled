using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellToolTipTextNeeded" /> event. </summary>
	// Token: 0x020001A8 RID: 424
	public class DataGridViewCellToolTipTextNeededEventArgs : DataGridViewCellEventArgs
	{
		// Token: 0x06001B62 RID: 7010 RVA: 0x00088738 File Offset: 0x00086938
		internal DataGridViewCellToolTipTextNeededEventArgs(int columnIndex, int rowIndex, string toolTipText) : base(columnIndex, rowIndex)
		{
			this.toolTipText = toolTipText;
		}

		/// <summary>Gets or sets the ToolTip text.</summary>
		/// <returns>The current ToolTip text.</returns>
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001B63 RID: 7011 RVA: 0x00088749 File Offset: 0x00086949
		// (set) Token: 0x06001B64 RID: 7012 RVA: 0x00088751 File Offset: 0x00086951
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				this.toolTipText = value;
			}
		}

		// Token: 0x04000C4F RID: 3151
		private string toolTipText;
	}
}
