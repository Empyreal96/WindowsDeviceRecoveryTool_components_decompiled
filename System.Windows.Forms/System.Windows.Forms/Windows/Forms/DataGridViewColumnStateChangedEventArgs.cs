using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.ColumnStateChanged" /> event. </summary>
	// Token: 0x020001B6 RID: 438
	public class DataGridViewColumnStateChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewColumnStateChangedEventArgs" /> class. </summary>
		/// <param name="dataGridViewColumn">The <see cref="T:System.Windows.Forms.DataGridViewColumn" /> whose state has changed.</param>
		/// <param name="stateChanged">One of the <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		// Token: 0x06001C70 RID: 7280 RVA: 0x0008F355 File Offset: 0x0008D555
		public DataGridViewColumnStateChangedEventArgs(DataGridViewColumn dataGridViewColumn, DataGridViewElementStates stateChanged)
		{
			this.dataGridViewColumn = dataGridViewColumn;
			this.stateChanged = stateChanged;
		}

		/// <summary>Gets the column whose state changed.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewColumn" /> whose state changed.</returns>
		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001C71 RID: 7281 RVA: 0x0008F36B File Offset: 0x0008D56B
		public DataGridViewColumn Column
		{
			get
			{
				return this.dataGridViewColumn;
			}
		}

		/// <summary>Gets the new column state.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</returns>
		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x0008F373 File Offset: 0x0008D573
		public DataGridViewElementStates StateChanged
		{
			get
			{
				return this.stateChanged;
			}
		}

		// Token: 0x04000CAB RID: 3243
		private DataGridViewColumn dataGridViewColumn;

		// Token: 0x04000CAC RID: 3244
		private DataGridViewElementStates stateChanged;
	}
}
