using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a text box control that can be hosted in a <see cref="T:System.Windows.Forms.DataGridViewTextBoxCell" />. </summary>
	// Token: 0x0200020E RID: 526
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class DataGridViewTextBoxEditingControl : TextBox, IDataGridViewEditingControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewTextBoxEditingControl" /> class.</summary>
		// Token: 0x06001FF7 RID: 8183 RVA: 0x000A008A File Offset: 0x0009E28A
		public DataGridViewTextBoxEditingControl()
		{
			base.TabStop = false;
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Forms.AccessibleObject" /> for this <see cref="T:System.Windows.Forms.DataGridViewTextBoxEditingControl" /> instance. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> instance that supports the ControlType UIA property. </returns>
		// Token: 0x06001FF8 RID: 8184 RVA: 0x000A0099 File Offset: 0x0009E299
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new DataGridViewTextBoxEditingControlAccessibleObject(this);
			}
			if (AccessibilityImprovements.Level2)
			{
				return new DataGridViewEditingControlAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.DataGridView" /> that contains the text box control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridView" /> that contains the <see cref="T:System.Windows.Forms.DataGridViewTextBoxCell" /> that contains this control; otherwise, <see langword="null" /> if there is no associated <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x000A00BD File Offset: 0x0009E2BD
		// (set) Token: 0x06001FFA RID: 8186 RVA: 0x000A00C5 File Offset: 0x0009E2C5
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

		/// <summary>Gets or sets the formatted representation of the current value of the text box control.</summary>
		/// <returns>An object representing the current value of this control.</returns>
		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x000A00CE File Offset: 0x0009E2CE
		// (set) Token: 0x06001FFC RID: 8188 RVA: 0x000A00D7 File Offset: 0x0009E2D7
		public virtual object EditingControlFormattedValue
		{
			get
			{
				return this.GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			set
			{
				this.Text = (string)value;
			}
		}

		/// <summary>Gets or sets the index of the owning cell's parent row.</summary>
		/// <returns>The index of the row that contains the owning cell; -1 if there is no owning row.</returns>
		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001FFD RID: 8189 RVA: 0x000A00E5 File Offset: 0x0009E2E5
		// (set) Token: 0x06001FFE RID: 8190 RVA: 0x000A00ED File Offset: 0x0009E2ED
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

		/// <summary>Gets or sets a value indicating whether the current value of the text box control has changed.</summary>
		/// <returns>
		///     <see langword="true" /> if the value of the control has changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x000A00F6 File Offset: 0x0009E2F6
		// (set) Token: 0x06002000 RID: 8192 RVA: 0x000A00FE File Offset: 0x0009E2FE
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

		/// <summary>Gets the cursor used when the mouse pointer is over the <see cref="P:System.Windows.Forms.DataGridView.EditingPanel" /> but not over the editing control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Cursor" /> that represents the mouse pointer used for the editing panel. </returns>
		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x000284A2 File Offset: 0x000266A2
		public virtual Cursor EditingPanelCursor
		{
			get
			{
				return Cursors.Default;
			}
		}

		/// <summary>Gets a value indicating whether the cell contents need to be repositioned whenever the value changes.</summary>
		/// <returns>
		///     <see langword="true" /> if the cell's <see cref="P:System.Windows.Forms.DataGridViewCellStyle.WrapMode" /> is set to <see langword="true" /> and the alignment property is not set to one of the <see cref="T:System.Windows.Forms.DataGridViewContentAlignment" /> values that aligns the content to the top; otherwise, <see langword="false" />.</returns>
		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x000A0107 File Offset: 0x0009E307
		public virtual bool RepositionEditingControlOnValueChange
		{
			get
			{
				return this.repositionOnValueChange;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x000A010F File Offset: 0x0009E30F
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3;
			}
		}

		/// <summary>Changes the control's user interface (UI) to be consistent with the specified cell style.</summary>
		/// <param name="dataGridViewCellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to use as the model for the UI.</param>
		// Token: 0x06002004 RID: 8196 RVA: 0x000A0118 File Offset: 0x0009E318
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
			if (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True)
			{
				base.WordWrap = true;
			}
			base.TextAlign = DataGridViewTextBoxEditingControl.TranslateAlignment(dataGridViewCellStyle.Alignment);
			this.repositionOnValueChange = (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True && (dataGridViewCellStyle.Alignment & DataGridViewTextBoxEditingControl.anyTop) == DataGridViewContentAlignment.NotSet);
		}

		/// <summary>Determines whether the specified key is a regular input key that the editing control should process or a special key that the <see cref="T:System.Windows.Forms.DataGridView" /> should process.</summary>
		/// <param name="keyData">A <see cref="T:System.Windows.Forms.Keys" /> that represents the key that was pressed.</param>
		/// <param name="dataGridViewWantsInputKey">
		///       <see langword="true" /> when the <see cref="T:System.Windows.Forms.DataGridView" /> wants to process the <paramref name="keyData" />; otherwise, <see langword="false" />.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key that should be handled by the editing control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002005 RID: 8197 RVA: 0x000A01CC File Offset: 0x0009E3CC
		public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
		{
			Keys keys = keyData & Keys.KeyCode;
			if (keys != Keys.Return)
			{
				switch (keys)
				{
				case Keys.Prior:
				case Keys.Next:
					if (this.valueChanged)
					{
						return true;
					}
					break;
				case Keys.End:
				case Keys.Home:
					if (this.SelectionLength != this.Text.Length)
					{
						return true;
					}
					break;
				case Keys.Left:
					if ((this.RightToLeft == RightToLeft.No && (this.SelectionLength != 0 || base.SelectionStart != 0)) || (this.RightToLeft == RightToLeft.Yes && (this.SelectionLength != 0 || base.SelectionStart != this.Text.Length)))
					{
						return true;
					}
					break;
				case Keys.Up:
					if (this.Text.IndexOf("\r\n") >= 0 && base.SelectionStart + this.SelectionLength >= this.Text.IndexOf("\r\n"))
					{
						return true;
					}
					break;
				case Keys.Right:
					if ((this.RightToLeft == RightToLeft.No && (this.SelectionLength != 0 || base.SelectionStart != this.Text.Length)) || (this.RightToLeft == RightToLeft.Yes && (this.SelectionLength != 0 || base.SelectionStart != 0)))
					{
						return true;
					}
					break;
				case Keys.Down:
				{
					int startIndex = base.SelectionStart + this.SelectionLength;
					if (this.Text.IndexOf("\r\n", startIndex) != -1)
					{
						return true;
					}
					break;
				}
				case Keys.Delete:
					if (this.SelectionLength > 0 || base.SelectionStart < this.Text.Length)
					{
						return true;
					}
					break;
				}
			}
			else if ((keyData & (Keys.Shift | Keys.Control | Keys.Alt)) == Keys.Shift && this.Multiline && base.AcceptsReturn)
			{
				return true;
			}
			return !dataGridViewWantsInputKey;
		}

		/// <summary>Retrieves the formatted value of the cell.</summary>
		/// <param name="context">One of the <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values that specifies the data error context.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the formatted version of the cell contents.</returns>
		// Token: 0x06002006 RID: 8198 RVA: 0x00093405 File Offset: 0x00091605
		public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return this.Text;
		}

		/// <summary>Prepares the currently selected cell for editing.</summary>
		/// <param name="selectAll">
		///       <see langword="true" /> to select the cell contents; otherwise, <see langword="false" />.</param>
		// Token: 0x06002007 RID: 8199 RVA: 0x000A0373 File Offset: 0x0009E573
		public virtual void PrepareEditingControlForEdit(bool selectAll)
		{
			if (selectAll)
			{
				base.SelectAll();
				return;
			}
			base.SelectionStart = this.Text.Length;
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x000A0390 File Offset: 0x0009E590
		private void NotifyDataGridViewOfValueChange()
		{
			this.valueChanged = true;
			this.dataGridView.NotifyCurrentCellDirty(true);
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x000A03A5 File Offset: 0x0009E5A5
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (AccessibilityImprovements.Level3)
			{
				base.AccessibilityObject.RaiseAutomationEvent(20005);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x0600200A RID: 8202 RVA: 0x000A03C6 File Offset: 0x0009E5C6
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.dataGridView.OnMouseWheelInternal(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event and notifies the <see cref="T:System.Windows.Forms.DataGridView" /> of the text change.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x0600200B RID: 8203 RVA: 0x000A03D4 File Offset: 0x0009E5D4
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.NotifyDataGridViewOfValueChange();
		}

		/// <summary>Processes key events.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> indicating the key that was pressed.</param>
		/// <returns>
		///     <see langword="true" /> if the key event was handled by the editing control; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600200C RID: 8204 RVA: 0x000A03E4 File Offset: 0x0009E5E4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyEventArgs(ref Message m)
		{
			Keys keys = (Keys)((int)m.WParam);
			if (keys != Keys.LineFeed)
			{
				if (keys != Keys.Return)
				{
					if (keys == Keys.A)
					{
						if (m.Msg == 256 && Control.ModifierKeys == Keys.Control)
						{
							base.SelectAll();
							return true;
						}
					}
				}
				else if (m.Msg == 258 && (Control.ModifierKeys != Keys.Shift || !this.Multiline || !base.AcceptsReturn))
				{
					return true;
				}
			}
			else if (m.Msg == 258 && Control.ModifierKeys == Keys.Control && this.Multiline && base.AcceptsReturn)
			{
				return true;
			}
			return base.ProcessKeyEventArgs(ref m);
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x000A048C File Offset: 0x0009E68C
		private static HorizontalAlignment TranslateAlignment(DataGridViewContentAlignment align)
		{
			if ((align & DataGridViewTextBoxEditingControl.anyRight) != DataGridViewContentAlignment.NotSet)
			{
				return HorizontalAlignment.Right;
			}
			if ((align & DataGridViewTextBoxEditingControl.anyCenter) != DataGridViewContentAlignment.NotSet)
			{
				return HorizontalAlignment.Center;
			}
			return HorizontalAlignment.Left;
		}

		// Token: 0x04000DD0 RID: 3536
		private static readonly DataGridViewContentAlignment anyTop = (DataGridViewContentAlignment)7;

		// Token: 0x04000DD1 RID: 3537
		private static readonly DataGridViewContentAlignment anyRight = (DataGridViewContentAlignment)1092;

		// Token: 0x04000DD2 RID: 3538
		private static readonly DataGridViewContentAlignment anyCenter = (DataGridViewContentAlignment)546;

		// Token: 0x04000DD3 RID: 3539
		private DataGridView dataGridView;

		// Token: 0x04000DD4 RID: 3540
		private bool valueChanged;

		// Token: 0x04000DD5 RID: 3541
		private bool repositionOnValueChange;

		// Token: 0x04000DD6 RID: 3542
		private int rowIndex;
	}
}
