using System;

namespace System.Windows.Forms
{
	/// <summary>Provides an editing notification interface.</summary>
	// Token: 0x0200016F RID: 367
	public interface IDataGridColumnStyleEditingNotificationService
	{
		/// <summary>Informs the <see cref="T:System.Windows.Forms.DataGrid" /> that the user has begun editing the column.</summary>
		/// <param name="editingControl">The <see cref="T:System.Windows.Forms.Control" /> that is editing the column. </param>
		// Token: 0x06001304 RID: 4868
		void ColumnStartedEditing(Control editingControl);
	}
}
