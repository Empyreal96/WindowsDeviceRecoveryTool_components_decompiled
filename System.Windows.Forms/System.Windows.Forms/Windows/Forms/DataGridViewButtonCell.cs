using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Internal;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Displays a button-like user interface (UI) for use in a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x02000190 RID: 400
	public class DataGridViewButtonCell : DataGridViewCell
	{
		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x060019AB RID: 6571 RVA: 0x0007FA24 File Offset: 0x0007DC24
		// (set) Token: 0x060019AC RID: 6572 RVA: 0x0007FA4A File Offset: 0x0007DC4A
		private ButtonState ButtonState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewButtonCell.PropButtonCellState, out flag);
				if (flag)
				{
					return (ButtonState)integer;
				}
				return ButtonState.Normal;
			}
			set
			{
				if (this.ButtonState != value)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellState, (int)value);
				}
			}
		}

		/// <summary>Gets the type of the cell's hosted editing control.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the underlying editing control.</returns>
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x060019AD RID: 6573 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override Type EditType
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets or sets the style determining the button's appearance.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlatStyle" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.FlatStyle" /> value. </exception>
		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x0007FA68 File Offset: 0x0007DC68
		// (set) Token: 0x060019AF RID: 6575 RVA: 0x0007FA90 File Offset: 0x0007DC90
		[DefaultValue(FlatStyle.Standard)]
		public FlatStyle FlatStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewButtonCell.PropButtonCellFlatStyle, out flag);
				if (flag)
				{
					return (FlatStyle)integer;
				}
				return FlatStyle.Standard;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				if (value != this.FlatStyle)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellFlatStyle, (int)value);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (set) Token: 0x060019B0 RID: 6576 RVA: 0x0007FAE3 File Offset: 0x0007DCE3
		internal FlatStyle FlatStyleInternal
		{
			set
			{
				if (value != this.FlatStyle)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellFlatStyle, (int)value);
				}
			}
		}

		/// <summary>Gets the type of the formatted value associated with the cell.</summary>
		/// <returns>A <see cref="T:System.Type" /> representing the type of the cell's formatted value.</returns>
		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0007FAFF File Offset: 0x0007DCFF
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewButtonCell.defaultFormattedValueType;
			}
		}

		/// <summary>Gets or sets a value indicating whether the owning column's text will appear on the button displayed by the cell.</summary>
		/// <returns>
		///     <see langword="true" /> if the value of the <see cref="P:System.Windows.Forms.DataGridViewCell.Value" /> property will automatically match the value of the <see cref="P:System.Windows.Forms.DataGridViewButtonColumn.Text" /> property of the owning column; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0007FB08 File Offset: 0x0007DD08
		// (set) Token: 0x060019B3 RID: 6579 RVA: 0x0007FB33 File Offset: 0x0007DD33
		[DefaultValue(false)]
		public bool UseColumnTextForButtonValue
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewButtonCell.PropButtonCellUseColumnTextForButtonValue, out flag);
				return flag && integer != 0;
			}
			set
			{
				if (value != this.UseColumnTextForButtonValue)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellUseColumnTextForButtonValue, value ? 1 : 0);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (set) Token: 0x060019B4 RID: 6580 RVA: 0x0007FB5B File Offset: 0x0007DD5B
		internal bool UseColumnTextForButtonValueInternal
		{
			set
			{
				if (value != this.UseColumnTextForButtonValue)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellUseColumnTextForButtonValue, value ? 1 : 0);
				}
			}
		}

		/// <summary>Gets or sets the data type of the values in the cell. </summary>
		/// <returns>A <see cref="T:System.Type" /> representing the data type of the value in the cell.</returns>
		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x0007FB80 File Offset: 0x0007DD80
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				return DataGridViewButtonCell.defaultValueType;
			}
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewButtonCell" />.</returns>
		// Token: 0x060019B6 RID: 6582 RVA: 0x0007FBA4 File Offset: 0x0007DDA4
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewButtonCell dataGridViewButtonCell;
			if (type == DataGridViewButtonCell.cellType)
			{
				dataGridViewButtonCell = new DataGridViewButtonCell();
			}
			else
			{
				dataGridViewButtonCell = (DataGridViewButtonCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewButtonCell);
			dataGridViewButtonCell.FlatStyleInternal = this.FlatStyle;
			dataGridViewButtonCell.UseColumnTextForButtonValueInternal = this.UseColumnTextForButtonValue;
			return dataGridViewButtonCell;
		}

		/// <summary>Creates a new accessible object for the <see cref="T:System.Windows.Forms.DataGridViewButtonCell" />. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.DataGridViewButtonCell.DataGridViewButtonCellAccessibleObject" /> for the <see cref="T:System.Windows.Forms.DataGridViewButtonCell" />. </returns>
		// Token: 0x060019B7 RID: 6583 RVA: 0x0007FBF9 File Offset: 0x0007DDF9
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewButtonCell.DataGridViewButtonCellAccessibleObject(this);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		// Token: 0x060019B8 RID: 6584 RVA: 0x0007FC04 File Offset: 0x0007DE04
		protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || base.OwningColumn == null)
			{
				return Rectangle.Empty;
			}
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates elementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out elementState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, elementState, null, null, cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, true, false, false);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's error icon, if one is displayed.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's error icon, if one is displayed; otherwise, <see cref="F:System.Drawing.Rectangle.Empty" />.</returns>
		// Token: 0x060019B9 RID: 6585 RVA: 0x0007FC5C File Offset: 0x0007DE5C
		protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || base.OwningColumn == null || !base.DataGridView.ShowCellErrors || string.IsNullOrEmpty(this.GetErrorText(rowIndex)))
			{
				return Rectangle.Empty;
			}
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates elementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out elementState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, elementState, null, this.GetErrorText(rowIndex), cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		// Token: 0x060019BA RID: 6586 RVA: 0x0007FCD4 File Offset: 0x0007DED4
		protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			Rectangle stdBorderWidths = base.StdBorderWidths;
			int num = stdBorderWidths.Left + stdBorderWidths.Width + cellStyle.Padding.Horizontal;
			int num2 = stdBorderWidths.Top + stdBorderWidths.Height + cellStyle.Padding.Vertical;
			DataGridViewFreeDimension freeDimensionFromConstraint = DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize);
			string text = base.GetFormattedValue(rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.PreferredSize) as string;
			if (string.IsNullOrEmpty(text))
			{
				text = " ";
			}
			TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			int num3;
			int num4;
			if (base.DataGridView.ApplyVisualStylesToInnerCells)
			{
				Rectangle themeMargins = DataGridViewButtonCell.GetThemeMargins(graphics);
				num3 = themeMargins.X + themeMargins.Width;
				num4 = themeMargins.Y + themeMargins.Height;
			}
			else
			{
				num4 = (num3 = 5);
			}
			Size result;
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
				{
					if (cellStyle.WrapMode == DataGridViewTriState.True && text.Length > 1 && constraintSize.Height - num2 - num4 - 2 > 0)
					{
						result = new Size(DataGridViewCell.MeasureTextWidth(graphics, text, cellStyle.Font, constraintSize.Height - num2 - num4 - 2, flags), 0);
					}
					else
					{
						result = new Size(DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Width, 0);
					}
				}
				else if (cellStyle.WrapMode == DataGridViewTriState.True && text.Length > 1)
				{
					result = DataGridViewCell.MeasureTextPreferredSize(graphics, text, cellStyle.Font, 5f, flags);
				}
				else
				{
					result = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags);
				}
			}
			else if (cellStyle.WrapMode == DataGridViewTriState.True && text.Length > 1 && constraintSize.Width - num - num3 - 4 > 0)
			{
				result = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, constraintSize.Width - num - num3 - 4, flags));
			}
			else
			{
				result = new Size(0, DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Height);
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				result.Width += num + num3 + 4;
				if (base.DataGridView.ShowCellErrors)
				{
					result.Width = Math.Max(result.Width, num + 8 + (int)DataGridViewCell.iconsWidth);
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				result.Height += num2 + num4 + 2;
				if (base.DataGridView.ShowCellErrors)
				{
					result.Height = Math.Max(result.Height, num2 + 8 + (int)DataGridViewCell.iconsHeight);
				}
			}
			return result;
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x0007FF80 File Offset: 0x0007E180
		private static Rectangle GetThemeMargins(Graphics g)
		{
			if (DataGridViewButtonCell.rectThemeMargins.X == -1)
			{
				Rectangle bounds = new Rectangle(0, 0, 100, 100);
				Rectangle backgroundContentRectangle = DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.GetBackgroundContentRectangle(g, bounds);
				DataGridViewButtonCell.rectThemeMargins.X = backgroundContentRectangle.X;
				DataGridViewButtonCell.rectThemeMargins.Y = backgroundContentRectangle.Y;
				DataGridViewButtonCell.rectThemeMargins.Width = 100 - backgroundContentRectangle.Right;
				DataGridViewButtonCell.rectThemeMargins.Height = 100 - backgroundContentRectangle.Bottom;
			}
			return DataGridViewButtonCell.rectThemeMargins;
		}

		/// <summary>Retrieves the text associated with the button.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The value of the <see cref="T:System.Windows.Forms.DataGridViewButtonCell" /> or the <see cref="P:System.Windows.Forms.DataGridViewButtonColumn.Text" /> value of the owning column if <see cref="P:System.Windows.Forms.DataGridViewButtonCell.UseColumnTextForButtonValue" /> is <see langword="true" />. </returns>
		// Token: 0x060019BC RID: 6588 RVA: 0x00080004 File Offset: 0x0007E204
		protected override object GetValue(int rowIndex)
		{
			if (this.UseColumnTextForButtonValue && base.DataGridView != null && base.DataGridView.NewRowIndex != rowIndex && base.OwningColumn != null && base.OwningColumn is DataGridViewButtonColumn)
			{
				return ((DataGridViewButtonColumn)base.OwningColumn).Text;
			}
			return base.GetValue(rowIndex);
		}

		/// <summary>Indicates whether a row is unshared if a key is pressed while the focus is on a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <returns>
		///     <see langword="true" /> if the user pressed the SPACE key without modifier keys; otherwise, <see langword="false" />.</returns>
		// Token: 0x060019BD RID: 6589 RVA: 0x0008005C File Offset: 0x0007E25C
		protected override bool KeyDownUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode == Keys.Space && !e.Alt && !e.Control && !e.Shift;
		}

		/// <summary>Indicates whether a row is unshared when a key is released while the focus is on a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <returns>
		///     <see langword="true" /> if the user released the SPACE key; otherwise, <see langword="false" />.</returns>
		// Token: 0x060019BE RID: 6590 RVA: 0x00080083 File Offset: 0x0007E283
		protected override bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode == Keys.Space;
		}

		/// <summary>Indicates whether a row will be unshared when the mouse button is held down while the pointer is on a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		/// <returns>
		///     <see langword="true" /> if the user pressed the left mouse button; otherwise, <see langword="false" />.</returns>
		// Token: 0x060019BF RID: 6591 RVA: 0x0008008F File Offset: 0x0007E28F
		protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left;
		}

		/// <summary>Indicates whether a row will be unshared when the mouse pointer moves over a cell in the row.</summary>
		/// <param name="rowIndex">The row index of the current cell, or -1 if the cell is not owned by a row.</param>
		/// <returns>
		///     <see langword="true" /> if the cell was the last cell receiving a mouse click; otherwise, <see langword="false" />.</returns>
		// Token: 0x060019C0 RID: 6592 RVA: 0x000800A0 File Offset: 0x0007E2A0
		protected override bool MouseEnterUnsharesRow(int rowIndex)
		{
			return base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y;
		}

		/// <summary>Indicates whether a row will be unshared when the mouse pointer leaves the row.</summary>
		/// <param name="rowIndex">The row index of the current cell, or -1 if the cell is not owned by a row.</param>
		/// <returns>
		///     <see langword="true" /> if the button displayed by the cell is in the pressed state; otherwise, <see langword="false" />.</returns>
		// Token: 0x060019C1 RID: 6593 RVA: 0x000800E0 File Offset: 0x0007E2E0
		protected override bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return (this.ButtonState & ButtonState.Pushed) > ButtonState.Normal;
		}

		/// <summary>Indicates whether a row will be unshared when the mouse button is released while the pointer is on a cell in the row. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		/// <returns>
		///     <see langword="true" /> if the left mouse button was released; otherwise, <see langword="false" />.</returns>
		// Token: 0x060019C2 RID: 6594 RVA: 0x0008008F File Offset: 0x0007E28F
		protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left;
		}

		/// <summary>Called when a character key is pressed while the focus is on the cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		/// <param name="rowIndex">The row index of the current cell, or -1 if the cell is not owned by a row.</param>
		// Token: 0x060019C3 RID: 6595 RVA: 0x000800F4 File Offset: 0x0007E2F4
		protected override void OnKeyDown(KeyEventArgs e, int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.KeyCode == Keys.Space && !e.Alt && !e.Control && !e.Shift)
			{
				this.UpdateButtonState(this.ButtonState | ButtonState.Checked, rowIndex);
				e.Handled = true;
			}
		}

		/// <summary>Called when a character key is released while the focus is on the cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data</param>
		/// <param name="rowIndex">The row index of the current cell, or -1 if the cell is not owned by a row.</param>
		// Token: 0x060019C4 RID: 6596 RVA: 0x00080148 File Offset: 0x0007E348
		protected override void OnKeyUp(KeyEventArgs e, int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.KeyCode == Keys.Space)
			{
				this.UpdateButtonState(this.ButtonState & ~ButtonState.Checked, rowIndex);
				if (!e.Alt && !e.Control && !e.Shift)
				{
					base.RaiseCellClick(new DataGridViewCellEventArgs(base.ColumnIndex, rowIndex));
					if (base.DataGridView != null && base.ColumnIndex < base.DataGridView.Columns.Count && rowIndex < base.DataGridView.Rows.Count)
					{
						base.RaiseCellContentClick(new DataGridViewCellEventArgs(base.ColumnIndex, rowIndex));
					}
					e.Handled = true;
				}
			}
		}

		/// <summary>Called when the focus moves from the cell.</summary>
		/// <param name="rowIndex">The row index of the current cell, or -1 if the cell is not owned by a row.</param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if focus left the cell as a result of user mouse click; <see langword="false" /> if focus left due to a programmatic cell change.</param>
		// Token: 0x060019C5 RID: 6597 RVA: 0x000801F4 File Offset: 0x0007E3F4
		protected override void OnLeave(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.ButtonState != ButtonState.Normal)
			{
				this.UpdateButtonState(ButtonState.Normal, rowIndex);
			}
		}

		/// <summary>Called when the mouse button is held down while the pointer is on the cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x060019C6 RID: 6598 RVA: 0x0008020F File Offset: 0x0007E40F
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left && DataGridViewButtonCell.mouseInContentBounds)
			{
				this.UpdateButtonState(this.ButtonState | ButtonState.Pushed, e.RowIndex);
			}
		}

		/// <summary>Called when the mouse pointer moves out of the cell.</summary>
		/// <param name="rowIndex">The row index of the current cell, or -1 if the cell is not owned by a row.</param>
		// Token: 0x060019C7 RID: 6599 RVA: 0x00080248 File Offset: 0x0007E448
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (DataGridViewButtonCell.mouseInContentBounds)
			{
				DataGridViewButtonCell.mouseInContentBounds = false;
				if (base.ColumnIndex >= 0 && rowIndex >= 0 && (base.DataGridView.ApplyVisualStylesToInnerCells || this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup))
				{
					base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
				}
			}
			if ((this.ButtonState & ButtonState.Pushed) != ButtonState.Normal && base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y)
			{
				this.UpdateButtonState(this.ButtonState & ~ButtonState.Pushed, rowIndex);
			}
		}

		/// <summary>Called when the mouse pointer moves while it is over the cell. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x060019C8 RID: 6600 RVA: 0x000802FC File Offset: 0x0007E4FC
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			bool flag = DataGridViewButtonCell.mouseInContentBounds;
			DataGridViewButtonCell.mouseInContentBounds = base.GetContentBounds(e.RowIndex).Contains(e.X, e.Y);
			if (flag != DataGridViewButtonCell.mouseInContentBounds)
			{
				if (base.DataGridView.ApplyVisualStylesToInnerCells || this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup)
				{
					base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
				}
				if (e.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && e.RowIndex == base.DataGridView.MouseDownCellAddress.Y && Control.MouseButtons == MouseButtons.Left)
				{
					if ((this.ButtonState & ButtonState.Pushed) == ButtonState.Normal && DataGridViewButtonCell.mouseInContentBounds && base.DataGridView.CellMouseDownInContentBounds)
					{
						this.UpdateButtonState(this.ButtonState | ButtonState.Pushed, e.RowIndex);
					}
					else if ((this.ButtonState & ButtonState.Pushed) != ButtonState.Normal && !DataGridViewButtonCell.mouseInContentBounds)
					{
						this.UpdateButtonState(this.ButtonState & ~ButtonState.Pushed, e.RowIndex);
					}
				}
			}
			base.OnMouseMove(e);
		}

		/// <summary>Called when the mouse button is released while the pointer is on the cell. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x060019C9 RID: 6601 RVA: 0x00080432 File Offset: 0x0007E632
		protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left)
			{
				this.UpdateButtonState(this.ButtonState & ~ButtonState.Pushed, e.RowIndex);
			}
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewButtonCell" />.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the cell.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
		/// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the bounds of the cell that is being painted.</param>
		/// <param name="rowIndex">The row index of the cell that is being painted.</param>
		/// <param name="elementState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the cell.</param>
		/// <param name="value">The data of the cell that is being painted.</param>
		/// <param name="formattedValue">The formatted data of the cell that is being painted.</param>
		/// <param name="errorText">An error message that is associated with the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the cell.</param>
		/// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that contains border styles for the cell that is being painted.</param>
		/// <param name="paintParts">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values that specifies which parts of the cell need to be painted.</param>
		// Token: 0x060019CA RID: 6602 RVA: 0x00080464 File Offset: 0x0007E664
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, elementState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x0008049C File Offset: 0x0007E69C
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			bool flag = (elementState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
			bool flag2 = currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex;
			string text = formattedValue as string;
			SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
			SolidBrush cachedBrush2 = base.DataGridView.GetCachedBrush(flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle = cellBounds;
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			rectangle.Offset(rectangle2.X, rectangle2.Y);
			rectangle.Width -= rectangle2.Right;
			rectangle.Height -= rectangle2.Bottom;
			Rectangle result;
			if (rectangle.Height > 0 && rectangle.Width > 0)
			{
				if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
				{
					g.FillRectangle(cachedBrush, rectangle);
				}
				if (cellStyle.Padding != Padding.Empty)
				{
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
					}
					else
					{
						rectangle.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
					}
					rectangle.Width -= cellStyle.Padding.Horizontal;
					rectangle.Height -= cellStyle.Padding.Vertical;
				}
				Rectangle cellValueBounds = rectangle;
				if (rectangle.Height > 0 && rectangle.Width > 0 && (paint || computeContentBounds))
				{
					if (this.FlatStyle == FlatStyle.Standard || this.FlatStyle == FlatStyle.System)
					{
						if (base.DataGridView.ApplyVisualStylesToInnerCells)
						{
							if (paint && DataGridViewCell.PaintContentBackground(paintParts))
							{
								PushButtonState pushButtonState = PushButtonState.Normal;
								if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
								{
									pushButtonState = PushButtonState.Pressed;
								}
								else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewButtonCell.mouseInContentBounds)
								{
									pushButtonState = PushButtonState.Hot;
								}
								if (DataGridViewCell.PaintFocus(paintParts) && flag2 && base.DataGridView.ShowFocusCues && base.DataGridView.Focused)
								{
									pushButtonState |= PushButtonState.Default;
								}
								DataGridViewButtonCell.DataGridViewButtonCellRenderer.DrawButton(g, rectangle, (int)pushButtonState);
							}
							result = rectangle;
							rectangle = DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.GetBackgroundContentRectangle(g, rectangle);
						}
						else
						{
							if (paint && DataGridViewCell.PaintContentBackground(paintParts))
							{
								ControlPaint.DrawBorder(g, rectangle, SystemColors.Control, (this.ButtonState == ButtonState.Normal) ? ButtonBorderStyle.Outset : ButtonBorderStyle.Inset);
							}
							result = rectangle;
							rectangle.Inflate(-SystemInformation.Border3DSize.Width, -SystemInformation.Border3DSize.Height);
						}
					}
					else if (this.FlatStyle == FlatStyle.Flat)
					{
						rectangle.Inflate(-1, -1);
						if (paint && DataGridViewCell.PaintContentBackground(paintParts))
						{
							ButtonBaseAdapter.DrawDefaultBorder(g, rectangle, cachedBrush2.Color, true);
							if (cachedBrush.Color.A == 255)
							{
								if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
								{
									ButtonBaseAdapter.ColorData colorData = ButtonBaseAdapter.PaintFlatRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
									IntPtr hdc = g.GetHdc();
									try
									{
										using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
										{
											WindowsBrush windowsBrush;
											if (colorData.options.highContrast)
											{
												windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, colorData.buttonShadow);
											}
											else
											{
												windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, colorData.lowHighlight);
											}
											try
											{
												ButtonBaseAdapter.PaintButtonBackground(windowsGraphics, rectangle, windowsBrush);
												goto IL_4C2;
											}
											finally
											{
												windowsBrush.Dispose();
											}
										}
									}
									finally
									{
										g.ReleaseHdc();
									}
								}
								if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewButtonCell.mouseInContentBounds)
								{
									IntPtr hdc2 = g.GetHdc();
									try
									{
										using (WindowsGraphics windowsGraphics2 = WindowsGraphics.FromHdc(hdc2))
										{
											Color controlDark = SystemColors.ControlDark;
											using (WindowsBrush windowsBrush2 = new WindowsSolidBrush(windowsGraphics2.DeviceContext, controlDark))
											{
												ButtonBaseAdapter.PaintButtonBackground(windowsGraphics2, rectangle, windowsBrush2);
											}
										}
									}
									finally
									{
										g.ReleaseHdc();
									}
								}
							}
						}
						IL_4C2:
						result = rectangle;
					}
					else
					{
						rectangle.Inflate(-1, -1);
						if (paint && DataGridViewCell.PaintContentBackground(paintParts))
						{
							if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
							{
								ButtonBaseAdapter.ColorData colorData2 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
								ButtonBaseAdapter.DrawDefaultBorder(g, rectangle, colorData2.options.highContrast ? colorData2.windowText : colorData2.windowFrame, true);
								ControlPaint.DrawBorder(g, rectangle, colorData2.options.highContrast ? colorData2.windowText : colorData2.buttonShadow, ButtonBorderStyle.Solid);
							}
							else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewButtonCell.mouseInContentBounds)
							{
								ButtonBaseAdapter.ColorData colorData3 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
								ButtonBaseAdapter.DrawDefaultBorder(g, rectangle, colorData3.options.highContrast ? colorData3.windowText : colorData3.buttonShadow, false);
								ButtonBaseAdapter.Draw3DLiteBorder(g, rectangle, colorData3, true);
							}
							else
							{
								ButtonBaseAdapter.ColorData colorData4 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
								ButtonBaseAdapter.DrawDefaultBorder(g, rectangle, colorData4.options.highContrast ? colorData4.windowText : colorData4.buttonShadow, false);
								ButtonBaseAdapter.DrawFlatBorder(g, rectangle, colorData4.options.highContrast ? colorData4.windowText : colorData4.buttonShadow);
							}
						}
						result = rectangle;
					}
				}
				else if (computeErrorIconBounds)
				{
					if (!string.IsNullOrEmpty(errorText))
					{
						result = base.ComputeErrorIconBounds(cellValueBounds);
					}
					else
					{
						result = Rectangle.Empty;
					}
				}
				else
				{
					result = Rectangle.Empty;
				}
				if (paint && DataGridViewCell.PaintFocus(paintParts) && flag2 && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && rectangle.Width > 2 * SystemInformation.Border3DSize.Width + 1 && rectangle.Height > 2 * SystemInformation.Border3DSize.Height + 1)
				{
					if (this.FlatStyle == FlatStyle.System || this.FlatStyle == FlatStyle.Standard)
					{
						ControlPaint.DrawFocusRectangle(g, Rectangle.Inflate(rectangle, -1, -1), Color.Empty, SystemColors.Control);
					}
					else if (this.FlatStyle == FlatStyle.Flat)
					{
						if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal || (base.DataGridView.CurrentCellAddress.Y == rowIndex && base.DataGridView.CurrentCellAddress.X == base.ColumnIndex))
						{
							ButtonBaseAdapter.ColorData colorData5 = ButtonBaseAdapter.PaintFlatRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
							string text2 = (text != null) ? text : string.Empty;
							ButtonBaseAdapter.LayoutOptions layoutOptions = ButtonFlatAdapter.PaintFlatLayout(g, true, SystemInformation.HighContrast, 1, rectangle, Padding.Empty, false, cellStyle.Font, text2, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft);
							layoutOptions.everettButtonCompat = false;
							ButtonBaseAdapter.LayoutData layoutData = layoutOptions.Layout();
							ButtonBaseAdapter.DrawFlatFocus(g, layoutData.focus, colorData5.options.highContrast ? colorData5.windowText : colorData5.constrastButtonShadow);
						}
					}
					else if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal || (base.DataGridView.CurrentCellAddress.Y == rowIndex && base.DataGridView.CurrentCellAddress.X == base.ColumnIndex))
					{
						bool up = this.ButtonState == ButtonState.Normal;
						string text3 = (text != null) ? text : string.Empty;
						ButtonBaseAdapter.LayoutOptions layoutOptions2 = ButtonPopupAdapter.PaintPopupLayout(g, up, SystemInformation.HighContrast ? 2 : 1, rectangle, Padding.Empty, false, cellStyle.Font, text3, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft);
						layoutOptions2.everettButtonCompat = false;
						ButtonBaseAdapter.LayoutData layoutData2 = layoutOptions2.Layout();
						ControlPaint.DrawFocusRectangle(g, layoutData2.focus, cellStyle.ForeColor, cellStyle.BackColor);
					}
				}
				if (text != null && paint && DataGridViewCell.PaintContentForeground(paintParts))
				{
					rectangle.Offset(2, 1);
					rectangle.Width -= 4;
					rectangle.Height -= 2;
					if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal && this.FlatStyle != FlatStyle.Flat && this.FlatStyle != FlatStyle.Popup)
					{
						rectangle.Offset(1, 1);
						int num = rectangle.Width;
						rectangle.Width = num - 1;
						num = rectangle.Height;
						rectangle.Height = num - 1;
					}
					if (rectangle.Width > 0 && rectangle.Height > 0)
					{
						Color color;
						if (base.DataGridView.ApplyVisualStylesToInnerCells && (this.FlatStyle == FlatStyle.System || this.FlatStyle == FlatStyle.Standard))
						{
							color = DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.GetColor(ColorProperty.TextColor);
						}
						else
						{
							color = cachedBrush2.Color;
						}
						TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
						TextRenderer.DrawText(g, text, cellStyle.Font, rectangle, color, flags);
					}
				}
				if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
				{
					base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, cellValueBounds, errorText);
				}
			}
			else
			{
				result = Rectangle.Empty;
			}
			return result;
		}

		/// <summary>Returns the string representation of the cell.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the current cell.</returns>
		// Token: 0x060019CC RID: 6604 RVA: 0x00080F5C File Offset: 0x0007F15C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewButtonCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x00080FB8 File Offset: 0x0007F1B8
		private void UpdateButtonState(ButtonState newButtonState, int rowIndex)
		{
			if (this.ButtonState != newButtonState)
			{
				this.ButtonState = newButtonState;
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
		}

		// Token: 0x04000BC8 RID: 3016
		private static readonly int PropButtonCellFlatStyle = PropertyStore.CreateKey();

		// Token: 0x04000BC9 RID: 3017
		private static readonly int PropButtonCellState = PropertyStore.CreateKey();

		// Token: 0x04000BCA RID: 3018
		private static readonly int PropButtonCellUseColumnTextForButtonValue = PropertyStore.CreateKey();

		// Token: 0x04000BCB RID: 3019
		private static readonly VisualStyleElement ButtonElement = VisualStyleElement.Button.PushButton.Normal;

		// Token: 0x04000BCC RID: 3020
		private const byte DATAGRIDVIEWBUTTONCELL_themeMargin = 100;

		// Token: 0x04000BCD RID: 3021
		private const byte DATAGRIDVIEWBUTTONCELL_horizontalTextMargin = 2;

		// Token: 0x04000BCE RID: 3022
		private const byte DATAGRIDVIEWBUTTONCELL_verticalTextMargin = 1;

		// Token: 0x04000BCF RID: 3023
		private const byte DATAGRIDVIEWBUTTONCELL_textPadding = 5;

		// Token: 0x04000BD0 RID: 3024
		private static Rectangle rectThemeMargins = new Rectangle(-1, -1, 0, 0);

		// Token: 0x04000BD1 RID: 3025
		private static bool mouseInContentBounds = false;

		// Token: 0x04000BD2 RID: 3026
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04000BD3 RID: 3027
		private static Type defaultValueType = typeof(object);

		// Token: 0x04000BD4 RID: 3028
		private static Type cellType = typeof(DataGridViewButtonCell);

		// Token: 0x020005A9 RID: 1449
		private class DataGridViewButtonCellRenderer
		{
			// Token: 0x06005908 RID: 22792 RVA: 0x000027DB File Offset: 0x000009DB
			private DataGridViewButtonCellRenderer()
			{
			}

			// Token: 0x17001577 RID: 5495
			// (get) Token: 0x06005909 RID: 22793 RVA: 0x00176CE5 File Offset: 0x00174EE5
			public static VisualStyleRenderer DataGridViewButtonRenderer
			{
				get
				{
					if (DataGridViewButtonCell.DataGridViewButtonCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewButtonCell.DataGridViewButtonCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewButtonCell.ButtonElement);
					}
					return DataGridViewButtonCell.DataGridViewButtonCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x0600590A RID: 22794 RVA: 0x00176D02 File Offset: 0x00174F02
			public static void DrawButton(Graphics g, Rectangle bounds, int buttonState)
			{
				DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.SetParameters(DataGridViewButtonCell.ButtonElement.ClassName, DataGridViewButtonCell.ButtonElement.Part, buttonState);
				DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.DrawBackground(g, bounds, Rectangle.Truncate(g.ClipBounds));
			}

			// Token: 0x04003924 RID: 14628
			private static VisualStyleRenderer visualStyleRenderer;
		}

		/// <summary>Provides information about a <see cref="T:System.Windows.Forms.DataGridViewButtonCell" /> to accessibility client applications.</summary>
		// Token: 0x020005AA RID: 1450
		protected class DataGridViewButtonCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewButtonCell.DataGridViewButtonCellAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewButtonCell.DataGridViewButtonCellAccessibleObject" />.</param>
			// Token: 0x0600590B RID: 22795 RVA: 0x00176D3A File Offset: 0x00174F3A
			public DataGridViewButtonCellAccessibleObject(DataGridViewCell owner) : base(owner)
			{
			}

			/// <summary>Gets a <see cref="T:System.String" /> that represents the default action of the <see cref="T:System.Windows.Forms.DataGridViewButtonCell.DataGridViewButtonCellAccessibleObject" />.</summary>
			/// <returns>The <see cref="T:System.String" /> "Press" if the <see cref="P:System.Windows.Forms.DataGridViewCell.ReadOnly" /> property is set to <see langword="false" />; otherwise, an empty <see cref="T:System.String" /> ("").</returns>
			// Token: 0x17001578 RID: 5496
			// (get) Token: 0x0600590C RID: 22796 RVA: 0x00176D43 File Offset: 0x00174F43
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("DataGridView_AccButtonCellDefaultAction");
				}
			}

			/// <summary>Performs the default action of the <see cref="T:System.Windows.Forms.DataGridViewButtonCell.DataGridViewButtonCellAccessibleObject" /></summary>
			/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Forms.DataGridViewButtonCell" /> returned by the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property does not belong to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-The <see cref="T:System.Windows.Forms.DataGridViewButtonCell" /> returned by the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property belongs to a shared row.</exception>
			// Token: 0x0600590D RID: 22797 RVA: 0x00176D50 File Offset: 0x00174F50
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				DataGridViewButtonCell dataGridViewButtonCell = (DataGridViewButtonCell)base.Owner;
				DataGridView dataGridView = dataGridViewButtonCell.DataGridView;
				if (dataGridView != null && dataGridViewButtonCell.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				if (dataGridViewButtonCell.OwningColumn != null && dataGridViewButtonCell.OwningRow != null)
				{
					dataGridView.OnCellClickInternal(new DataGridViewCellEventArgs(dataGridViewButtonCell.ColumnIndex, dataGridViewButtonCell.RowIndex));
					dataGridView.OnCellContentClickInternal(new DataGridViewCellEventArgs(dataGridViewButtonCell.ColumnIndex, dataGridViewButtonCell.RowIndex));
				}
			}

			/// <summary>Gets the number of child accessible objects that belong to the <see cref="T:System.Windows.Forms.DataGridViewButtonCell.DataGridViewButtonCellAccessibleObject" />.</summary>
			/// <returns>The value –1.</returns>
			// Token: 0x0600590E RID: 22798 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public override int GetChildCount()
			{
				return 0;
			}

			// Token: 0x0600590F RID: 22799 RVA: 0x00176DCA File Offset: 0x00174FCA
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level2 || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005910 RID: 22800 RVA: 0x00176DDB File Offset: 0x00174FDB
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50000;
				}
				return base.GetPropertyValue(propertyID);
			}
		}
	}
}
