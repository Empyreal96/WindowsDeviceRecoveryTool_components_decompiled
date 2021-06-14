using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.UserDeletingRow" /> event of a <see cref="T:System.Windows.Forms.DataGridView" />. </summary>
	// Token: 0x020001F8 RID: 504
	public class DataGridViewRowCancelEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowCancelEventArgs" /> class. </summary>
		/// <param name="dataGridViewRow">The row the user is deleting.</param>
		// Token: 0x06001EBE RID: 7870 RVA: 0x00099AD6 File Offset: 0x00097CD6
		public DataGridViewRowCancelEventArgs(DataGridViewRow dataGridViewRow)
		{
			this.dataGridViewRow = dataGridViewRow;
		}

		/// <summary>Gets the row that the user is deleting.</summary>
		/// <returns>The row that the user deleted.</returns>
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x00099AE5 File Offset: 0x00097CE5
		public DataGridViewRow Row
		{
			get
			{
				return this.dataGridViewRow;
			}
		}

		// Token: 0x04000D6E RID: 3438
		private DataGridViewRow dataGridViewRow;
	}
}
