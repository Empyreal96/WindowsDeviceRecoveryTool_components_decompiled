using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellStateChanged" /> event. </summary>
	// Token: 0x020001A1 RID: 417
	public class DataGridViewCellStateChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellStateChangedEventArgs" /> class. </summary>
		/// <param name="dataGridViewCell">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> that has a changed state.</param>
		/// <param name="stateChanged">One of the <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values indicating the state that has changed on the cell.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewCell" /> is <see langword="null" />.</exception>
		// Token: 0x06001B1E RID: 6942 RVA: 0x00087594 File Offset: 0x00085794
		public DataGridViewCellStateChangedEventArgs(DataGridViewCell dataGridViewCell, DataGridViewElementStates stateChanged)
		{
			if (dataGridViewCell == null)
			{
				throw new ArgumentNullException("dataGridViewCell");
			}
			this.dataGridViewCell = dataGridViewCell;
			this.stateChanged = stateChanged;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that has a changed state.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCell" /> whose state has changed.</returns>
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x000875B8 File Offset: 0x000857B8
		public DataGridViewCell Cell
		{
			get
			{
				return this.dataGridViewCell;
			}
		}

		/// <summary>Gets the state that has changed on the cell.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values indicating the state that has changed on the cell.</returns>
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x000875C0 File Offset: 0x000857C0
		public DataGridViewElementStates StateChanged
		{
			get
			{
				return this.stateChanged;
			}
		}

		// Token: 0x04000C2B RID: 3115
		private DataGridViewCell dataGridViewCell;

		// Token: 0x04000C2C RID: 3116
		private DataGridViewElementStates stateChanged;
	}
}
