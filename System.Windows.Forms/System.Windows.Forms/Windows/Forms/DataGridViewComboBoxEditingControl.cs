using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Represents the hosted combo box control in a <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</summary>
	// Token: 0x020001BA RID: 442
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class DataGridViewComboBoxEditingControl : ComboBox, IDataGridViewEditingControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewComboBoxEditingControl" /> class.</summary>
		// Token: 0x06001CEB RID: 7403 RVA: 0x000932AF File Offset: 0x000914AF
		public DataGridViewComboBoxEditingControl()
		{
			base.TabStop = false;
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Forms.AccessibleObject" /> for this <see cref="T:System.Windows.Forms.DataGridViewComboBoxEditingControl" /> instance. </summary>
		/// <returns>A new accessibility object. </returns>
		// Token: 0x06001CEC RID: 7404 RVA: 0x000932BE File Offset: 0x000914BE
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new DataGridViewComboBoxEditingControlAccessibleObject(this);
			}
			if (AccessibilityImprovements.Level2)
			{
				return new DataGridViewEditingControlAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.DataGridView" /> that contains the combo box control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridView" /> that contains the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> that contains this control; otherwise, <see langword="null" /> if there is no associated <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x000932E2 File Offset: 0x000914E2
		// (set) Token: 0x06001CEE RID: 7406 RVA: 0x000932EA File Offset: 0x000914EA
		public virtual DataGridView EditingControlDataGridView
		{
			get
			{
				return this.dataGridView;
			}
			set
			{
				this.dataGridView = value;
			}
		}

		/// <summary>Gets or sets the formatted representation of the current value of the control.</summary>
		/// <returns>An object representing the current value of this control.</returns>
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x000932F3 File Offset: 0x000914F3
		// (set) Token: 0x06001CF0 RID: 7408 RVA: 0x000932FC File Offset: 0x000914FC
		public virtual object EditingControlFormattedValue
		{
			get
			{
				return this.GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			set
			{
				string text = value as string;
				if (text != null)
				{
					this.Text = text;
					if (string.Compare(text, this.Text, true, CultureInfo.CurrentCulture) != 0)
					{
						this.SelectedIndex = -1;
					}
				}
			}
		}

		/// <summary>Gets or sets the index of the owning cell's parent row.</summary>
		/// <returns>The index of the row that contains the owning cell; -1 if there is no owning row.</returns>
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x00093335 File Offset: 0x00091535
		// (set) Token: 0x06001CF2 RID: 7410 RVA: 0x0009333D File Offset: 0x0009153D
		public virtual int EditingControlRowIndex
		{
			get
			{
				return this.rowIndex;
			}
			set
			{
				this.rowIndex = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the current value of the control has changed.</summary>
		/// <returns>
		///     <see langword="true" /> if the value of the control has changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x00093346 File Offset: 0x00091546
		// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x0009334E File Offset: 0x0009154E
		public virtual bool EditingControlValueChanged
		{
			get
			{
				return this.valueChanged;
			}
			set
			{
				this.valueChanged = value;
			}
		}

		/// <summary>Gets the cursor used during editing.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor image used by the mouse pointer during editing.</returns>
		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x000284A2 File Offset: 0x000266A2
		public virtual Cursor EditingPanelCursor
		{
			get
			{
				return Cursors.Default;
			}
		}

		/// <summary>Gets a value indicating whether the cell contents need to be repositioned whenever the value changes.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public virtual bool RepositionEditingControlOnValueChange
		{
			get
			{
				return false;
			}
		}

		/// <summary>Changes the control's user interface (UI) to be consistent with the specified cell style.</summary>
		/// <param name="dataGridViewCellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to use as a pattern for the UI.</param>
		// Token: 0x06001CF7 RID: 7415 RVA: 0x00093358 File Offset: 0x00091558
		public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			this.Font = dataGridViewCellStyle.Font;
			if (dataGridViewCellStyle.BackColor.A < 255)
			{
				Color backColor = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
				this.BackColor = backColor;
				this.dataGridView.EditingPanel.BackColor = backColor;
			}
			else
			{
				this.BackColor = dataGridViewCellStyle.BackColor;
			}
			this.ForeColor = dataGridViewCellStyle.ForeColor;
		}

		/// <summary>Determines whether the specified key is a regular input key that the editing control should process or a special key that the <see cref="T:System.Windows.Forms.DataGridView" /> should process.</summary>
		/// <param name="keyData">A bitwise combination of <see cref="T:System.Windows.Forms.Keys" /> values that represents the key that was pressed.</param>
		/// <param name="dataGridViewWantsInputKey">
		///       <see langword="true" /> to indicate that the <see cref="T:System.Windows.Forms.DataGridView" /> control can process the key; otherwise, <see langword="false" />.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key that should be handled by the editing control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001CF8 RID: 7416 RVA: 0x000933C9 File Offset: 0x000915C9
		public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
		{
			return (keyData & Keys.KeyCode) == Keys.Down || (keyData & Keys.KeyCode) == Keys.Up || (base.DroppedDown && (keyData & Keys.KeyCode) == Keys.Escape) || (keyData & Keys.KeyCode) == Keys.Return || !dataGridViewWantsInputKey;
		}

		/// <summary>Retrieves the formatted value of the cell.</summary>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values that specifies the data error context.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the formatted version of the cell contents.</returns>
		// Token: 0x06001CF9 RID: 7417 RVA: 0x00093405 File Offset: 0x00091605
		public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return this.Text;
		}

		/// <summary>Prepares the currently selected cell for editing.</summary>
		/// <param name="selectAll">
		///       <see langword="true" /> to select all of the cell's content; otherwise, <see langword="false" />.</param>
		// Token: 0x06001CFA RID: 7418 RVA: 0x0009340D File Offset: 0x0009160D
		public virtual void PrepareEditingControlForEdit(bool selectAll)
		{
			if (selectAll)
			{
				base.SelectAll();
			}
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x00093418 File Offset: 0x00091618
		private void NotifyDataGridViewOfValueChange()
		{
			this.valueChanged = true;
			this.dataGridView.NotifyCurrentCellDirty(true);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectedIndexChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06001CFC RID: 7420 RVA: 0x0009342D File Offset: 0x0009162D
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			if (this.SelectedIndex != -1)
			{
				this.NotifyDataGridViewOfValueChange();
			}
		}

		// Token: 0x04000CDA RID: 3290
		private DataGridView dataGridView;

		// Token: 0x04000CDB RID: 3291
		private bool valueChanged;

		// Token: 0x04000CDC RID: 3292
		private int rowIndex;
	}
}
