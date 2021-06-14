using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.RowErrorTextNeeded" /> event of a <see cref="T:System.Windows.Forms.DataGridView" /> control. </summary>
	// Token: 0x020001FD RID: 509
	public class DataGridViewRowErrorTextNeededEventArgs : EventArgs
	{
		// Token: 0x06001F25 RID: 7973 RVA: 0x0009CCB5 File Offset: 0x0009AEB5
		internal DataGridViewRowErrorTextNeededEventArgs(int rowIndex, string errorText)
		{
			this.rowIndex = rowIndex;
			this.errorText = errorText;
		}

		/// <summary>Gets or sets the error text for the row.</summary>
		/// <returns>A string that represents the error text for the row.</returns>
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001F26 RID: 7974 RVA: 0x0009CCCB File Offset: 0x0009AECB
		// (set) Token: 0x06001F27 RID: 7975 RVA: 0x0009CCD3 File Offset: 0x0009AED3
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

		/// <summary>Gets the row that raised the <see cref="E:System.Windows.Forms.DataGridView.RowErrorTextNeeded" /> event.</summary>
		/// <returns>The zero based row index for the row.</returns>
		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001F28 RID: 7976 RVA: 0x0009CCDC File Offset: 0x0009AEDC
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04000D7B RID: 3451
		private int rowIndex;

		// Token: 0x04000D7C RID: 3452
		private string errorText;
	}
}
