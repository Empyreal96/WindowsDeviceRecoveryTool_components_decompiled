using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.EditingControlShowing" /> event.</summary>
	// Token: 0x020001C2 RID: 450
	public class DataGridViewEditingControlShowingEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewEditingControlShowingEventArgs" /> class.</summary>
		/// <param name="control">A <see cref="T:System.Windows.Forms.Control" /> in which the user will edit the selected cell's contents.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> representing the style of the cell being edited.</param>
		// Token: 0x06001D24 RID: 7460 RVA: 0x00093652 File Offset: 0x00091852
		public DataGridViewEditingControlShowingEventArgs(Control control, DataGridViewCellStyle cellStyle)
		{
			this.control = control;
			this.cellStyle = cellStyle;
		}

		/// <summary>Gets or sets the cell style of the edited cell.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> representing the style of the cell being edited.</returns>
		/// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is <see langword="null" />.</exception>
		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x00093668 File Offset: 0x00091868
		// (set) Token: 0x06001D26 RID: 7462 RVA: 0x00093670 File Offset: 0x00091870
		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.cellStyle;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.cellStyle = value;
			}
		}

		/// <summary>The control shown to the user for editing the selected cell's value.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Control" /> that displays an area for the user to enter or change the selected cell's value.</returns>
		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x00093687 File Offset: 0x00091887
		public Control Control
		{
			get
			{
				return this.control;
			}
		}

		// Token: 0x04000CF9 RID: 3321
		private Control control;

		// Token: 0x04000CFA RID: 3322
		private DataGridViewCellStyle cellStyle;
	}
}
