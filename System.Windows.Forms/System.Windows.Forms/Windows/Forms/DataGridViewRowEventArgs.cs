using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for row-related <see cref="T:System.Windows.Forms.DataGridView" /> events. </summary>
	// Token: 0x020001FE RID: 510
	public class DataGridViewRowEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowEventArgs" /> class. </summary>
		/// <param name="dataGridViewRow">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> that the event occurred for.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewRow" /> is <see langword="null" />.</exception>
		// Token: 0x06001F29 RID: 7977 RVA: 0x0009CCE4 File Offset: 0x0009AEE4
		public DataGridViewRowEventArgs(DataGridViewRow dataGridViewRow)
		{
			if (dataGridViewRow == null)
			{
				throw new ArgumentNullException("dataGridViewRow");
			}
			this.dataGridViewRow = dataGridViewRow;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridViewRow" /> associated with the event.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewRow" /> associated with the event.</returns>
		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001F2A RID: 7978 RVA: 0x0009CD01 File Offset: 0x0009AF01
		public DataGridViewRow Row
		{
			get
			{
				return this.dataGridViewRow;
			}
		}

		// Token: 0x04000D7D RID: 3453
		private DataGridViewRow dataGridViewRow;
	}
}
