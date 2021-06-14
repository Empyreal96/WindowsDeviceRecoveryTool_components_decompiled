using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="T:System.Windows.Forms.DataGridView" /><see cref="E:System.Windows.Forms.DataGridView.AutoSizeRowsModeChanged" /> and <see cref="E:System.Windows.Forms.DataGridView.RowHeadersWidthSizeModeChanged" /> events.</summary>
	// Token: 0x0200018D RID: 397
	public class DataGridViewAutoSizeModeEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewAutoSizeModeEventArgs" /> class.</summary>
		/// <param name="previousModeAutoSized">
		///       <see langword="true" /> if the <see cref="P:System.Windows.Forms.DataGridView.AutoSizeRowsMode" /> property was previously set to any <see cref="T:System.Windows.Forms.DataGridViewAutoSizeRowsMode" /> value other than <see cref="F:System.Windows.Forms.DataGridViewAutoSizeRowsMode.None" /> or the <see cref="P:System.Windows.Forms.DataGridView.RowHeadersWidthSizeMode" /> property was previously set to any <see cref="T:System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode" /> value other than <see cref="F:System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing" /> or <see cref="F:System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing" />; otherwise, <see langword="false" />.</param>
		// Token: 0x0600196B RID: 6507 RVA: 0x0007E7B9 File Offset: 0x0007C9B9
		public DataGridViewAutoSizeModeEventArgs(bool previousModeAutoSized)
		{
			this.previousModeAutoSized = previousModeAutoSized;
		}

		/// <summary>Gets a value specifying whether the <see cref="T:System.Windows.Forms.DataGridView" /> was previously set to automatically resize.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.DataGridView.AutoSizeRowsMode" /> property was previously set to any <see cref="T:System.Windows.Forms.DataGridViewAutoSizeRowsMode" /> value other than <see cref="F:System.Windows.Forms.DataGridViewAutoSizeRowsMode.None" /> or the <see cref="P:System.Windows.Forms.DataGridView.RowHeadersWidthSizeMode" /> property was previously set to any <see cref="T:System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode" /> value other than <see cref="F:System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing" /> or <see cref="F:System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600196C RID: 6508 RVA: 0x0007E7C8 File Offset: 0x0007C9C8
		public bool PreviousModeAutoSized
		{
			get
			{
				return this.previousModeAutoSized;
			}
		}

		// Token: 0x04000BB8 RID: 3000
		private bool previousModeAutoSized;
	}
}
