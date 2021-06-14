using System;

namespace System.Windows.Forms
{
	/// <summary>Defines common functionality for a cell that allows the manipulation of its value.</summary>
	// Token: 0x020001BF RID: 447
	public interface IDataGridViewEditingCell
	{
		/// <summary>Gets or sets the formatted value of the cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains the cell's value.</returns>
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001D0A RID: 7434
		// (set) Token: 0x06001D0B RID: 7435
		object EditingCellFormattedValue { get; set; }

		/// <summary>Gets or sets a value indicating whether the value of the cell has changed.</summary>
		/// <returns>
		///     <see langword="true" /> if the value of the cell has changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001D0C RID: 7436
		// (set) Token: 0x06001D0D RID: 7437
		bool EditingCellValueChanged { get; set; }

		/// <summary>Retrieves the formatted value of the cell.</summary>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values that specifies the context in which the data is needed.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the formatted version of the cell contents.</returns>
		// Token: 0x06001D0E RID: 7438
		object GetEditingCellFormattedValue(DataGridViewDataErrorContexts context);

		/// <summary>Prepares the currently selected cell for editing</summary>
		/// <param name="selectAll">
		///       <see langword="true" /> to select the cell contents; otherwise, <see langword="false" />.</param>
		// Token: 0x06001D0F RID: 7439
		void PrepareEditingCellForEdit(bool selectAll);
	}
}
