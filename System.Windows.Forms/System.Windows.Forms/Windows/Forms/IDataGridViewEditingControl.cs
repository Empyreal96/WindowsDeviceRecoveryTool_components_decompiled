using System;

namespace System.Windows.Forms
{
	/// <summary>Defines common functionality for controls that are hosted within cells of a <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
	// Token: 0x020001C0 RID: 448
	public interface IDataGridViewEditingControl
	{
		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.DataGridView" /> that contains the cell.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridView" /> that contains the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being edited; <see langword="null" /> if there is no associated <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001D10 RID: 7440
		// (set) Token: 0x06001D11 RID: 7441
		DataGridView EditingControlDataGridView { get; set; }

		/// <summary>Gets or sets the formatted value of the cell being modified by the editor.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the formatted value of the cell.</returns>
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001D12 RID: 7442
		// (set) Token: 0x06001D13 RID: 7443
		object EditingControlFormattedValue { get; set; }

		/// <summary>Gets or sets the index of the hosting cell's parent row.</summary>
		/// <returns>The index of the row that contains the cell, or –1 if there is no parent row.</returns>
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001D14 RID: 7444
		// (set) Token: 0x06001D15 RID: 7445
		int EditingControlRowIndex { get; set; }

		/// <summary>Gets or sets a value indicating whether the value of the editing control differs from the value of the hosting cell.</summary>
		/// <returns>
		///     <see langword="true" /> if the value of the control differs from the cell value; otherwise, <see langword="false" />.</returns>
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001D16 RID: 7446
		// (set) Token: 0x06001D17 RID: 7447
		bool EditingControlValueChanged { get; set; }

		/// <summary>Gets the cursor used when the mouse pointer is over the <see cref="P:System.Windows.Forms.DataGridView.EditingPanel" /> but not over the editing control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Cursor" /> that represents the mouse pointer used for the editing panel. </returns>
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06001D18 RID: 7448
		Cursor EditingPanelCursor { get; }

		/// <summary>Gets or sets a value indicating whether the cell contents need to be repositioned whenever the value changes.</summary>
		/// <returns>
		///     <see langword="true" /> if the contents need to be repositioned; otherwise, <see langword="false" />.</returns>
		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001D19 RID: 7449
		bool RepositionEditingControlOnValueChange { get; }

		/// <summary>Changes the control's user interface (UI) to be consistent with the specified cell style.</summary>
		/// <param name="dataGridViewCellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to use as the model for the UI.</param>
		// Token: 0x06001D1A RID: 7450
		void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle);

		/// <summary>Determines whether the specified key is a regular input key that the editing control should process or a special key that the <see cref="T:System.Windows.Forms.DataGridView" /> should process.</summary>
		/// <param name="keyData">A <see cref="T:System.Windows.Forms.Keys" /> that represents the key that was pressed.</param>
		/// <param name="dataGridViewWantsInputKey">
		///       <see langword="true" /> when the <see cref="T:System.Windows.Forms.DataGridView" /> wants to process the <see cref="T:System.Windows.Forms.Keys" /> in <paramref name="keyData" />; otherwise, <see langword="false" />.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key that should be handled by the editing control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001D1B RID: 7451
		bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey);

		/// <summary>Retrieves the formatted value of the cell.</summary>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values that specifies the context in which the data is needed.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the formatted version of the cell contents.</returns>
		// Token: 0x06001D1C RID: 7452
		object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context);

		/// <summary>Prepares the currently selected cell for editing.</summary>
		/// <param name="selectAll">
		///       <see langword="true" /> to select all of the cell's content; otherwise, <see langword="false" />.</param>
		// Token: 0x06001D1D RID: 7453
		void PrepareEditingControlForEdit(bool selectAll);
	}
}
