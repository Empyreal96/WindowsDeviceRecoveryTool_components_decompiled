using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellErrorTextNeeded" /> event of a <see cref="T:System.Windows.Forms.DataGridView" /> control. </summary>
	// Token: 0x02000198 RID: 408
	public class DataGridViewCellErrorTextNeededEventArgs : DataGridViewCellEventArgs
	{
		// Token: 0x06001AE2 RID: 6882 RVA: 0x00086D07 File Offset: 0x00084F07
		internal DataGridViewCellErrorTextNeededEventArgs(int columnIndex, int rowIndex, string errorText) : base(columnIndex, rowIndex)
		{
			this.errorText = errorText;
		}

		/// <summary>Gets or sets the message that is displayed when the cell is selected.</summary>
		/// <returns>The error message.</returns>
		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x00086D18 File Offset: 0x00084F18
		// (set) Token: 0x06001AE4 RID: 6884 RVA: 0x00086D20 File Offset: 0x00084F20
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
			set
			{
				this.errorText = value;
			}
		}

		// Token: 0x04000C08 RID: 3080
		private string errorText;
	}
}
