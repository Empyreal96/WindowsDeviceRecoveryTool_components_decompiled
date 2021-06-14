using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.AutoSizeColumnsModeChanged" /> event. </summary>
	// Token: 0x02000183 RID: 387
	public class DataGridViewAutoSizeColumnsModeEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnsModeEventArgs" /> class. </summary>
		/// <param name="previousModes">An array of <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnMode" /> values representing the previous <see cref="P:System.Windows.Forms.DataGridViewColumn.AutoSizeMode" /> property values of each column. </param>
		// Token: 0x06001969 RID: 6505 RVA: 0x0007E7A2 File Offset: 0x0007C9A2
		public DataGridViewAutoSizeColumnsModeEventArgs(DataGridViewAutoSizeColumnMode[] previousModes)
		{
			this.previousModes = previousModes;
		}

		/// <summary>Gets an array of the previous values of the column <see cref="P:System.Windows.Forms.DataGridViewColumn.AutoSizeMode" /> properties.</summary>
		/// <returns>An array of <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnMode" /> values representing the previous values of the column <see cref="P:System.Windows.Forms.DataGridViewColumn.AutoSizeMode" /> properties.</returns>
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x0007E7B1 File Offset: 0x0007C9B1
		public DataGridViewAutoSizeColumnMode[] PreviousModes
		{
			get
			{
				return this.previousModes;
			}
		}

		// Token: 0x04000B80 RID: 2944
		private DataGridViewAutoSizeColumnMode[] previousModes;
	}
}
