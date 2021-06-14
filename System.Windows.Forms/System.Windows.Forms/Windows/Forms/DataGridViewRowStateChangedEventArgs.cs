using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.RowStateChanged" /> event of a <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
	// Token: 0x02000206 RID: 518
	public class DataGridViewRowStateChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowStateChangedEventArgs" /> class. </summary>
		/// <param name="dataGridViewRow">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> that has a changed state.</param>
		/// <param name="stateChanged">One of the <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values indicating the state that has changed on the row.</param>
		// Token: 0x06001F80 RID: 8064 RVA: 0x0009ED33 File Offset: 0x0009CF33
		public DataGridViewRowStateChangedEventArgs(DataGridViewRow dataGridViewRow, DataGridViewElementStates stateChanged)
		{
			this.dataGridViewRow = dataGridViewRow;
			this.stateChanged = stateChanged;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridViewRow" /> that has a changed state.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewRow" /> that has a changed state.</returns>
		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x0009ED49 File Offset: 0x0009CF49
		public DataGridViewRow Row
		{
			get
			{
				return this.dataGridViewRow;
			}
		}

		/// <summary>Gets the state that has changed on the row.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values indicating the state that has changed on the row.</returns>
		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001F82 RID: 8066 RVA: 0x0009ED51 File Offset: 0x0009CF51
		public DataGridViewElementStates StateChanged
		{
			get
			{
				return this.stateChanged;
			}
		}

		// Token: 0x04000DAD RID: 3501
		private DataGridViewRow dataGridViewRow;

		// Token: 0x04000DAE RID: 3502
		private DataGridViewElementStates stateChanged;
	}
}
