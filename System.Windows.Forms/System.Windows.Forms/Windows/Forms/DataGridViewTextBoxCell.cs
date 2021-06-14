using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Displays editable text information in a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x0200020C RID: 524
	public class DataGridViewTextBoxCell : DataGridViewCell
	{
		/// <summary>Creates a new <see cref="T:System.Windows.Forms.AccessibleObject" /> for this <see cref="T:System.Windows.Forms.DataGridViewTextBoxCell" /> instance. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> instance that supports the ControlType UIA property. </returns>
		// Token: 0x06001FD5 RID: 8149 RVA: 0x0009F018 File Offset: 0x0009D218
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level1)
			{
				return new DataGridViewTextBoxCell.DataGridViewTextBoxCellAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x0009F02E File Offset: 0x0009D22E
		// (set) Token: 0x06001FD7 RID: 8151 RVA: 0x0009F045 File Offset: 0x0009D245
		private DataGridViewTextBoxEditingControl EditingTextBox
		{
			get
			{
				return (DataGridViewTextBoxEditingControl)base.Properties.GetObject(DataGridViewTextBoxCell.PropTextBoxCellEditingTextBox);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewTextBoxCell.PropTextBoxCellEditingTextBox))
				{
					base.Properties.SetObject(DataGridViewTextBoxCell.PropTextBoxCellEditingTextBox, value);
				}
			}
		}

		/// <summary>Gets the type of the formatted value associated with the cell.</summary>
		/// <returns>A <see cref="T:System.Type" /> representing the <see cref="T:System.String" /> type in all cases.</returns>
		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x0009F06D File Offset: 0x0009D26D
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewTextBoxCell.defaultFormattedValueType;
			}
		}

		/// <summary>Gets or sets the maximum number of characters that can be entered into the text box.</summary>
		/// <returns>The maximum number of characters that can be entered into the text box; the default value is 32767.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0.</exception>
		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x0009F074 File Offset: 0x0009D274
		// (set) Token: 0x06001FDA RID: 8154 RVA: 0x0009F0A0 File Offset: 0x0009D2A0
		[DefaultValue(32767)]
		public virtual int MaxInputLength
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewTextBoxCell.PropTextBoxCellMaxInputLength, out flag);
				if (flag)
				{
					return integer;
				}
				return 32767;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MaxInputLength", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"MaxInputLength",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				base.Properties.SetInteger(DataGridViewTextBoxCell.PropTextBoxCellMaxInputLength, value);
				if (this.OwnsEditingTextBox(base.RowIndex))
				{
					this.EditingTextBox.MaxLength = value;
				}
			}
		}

		/// <summary>Gets or sets the data type of the values in the cell. </summary>
		/// <returns>A <see cref="T:System.Type" /> representing the data type of the value in the cell.</returns>
		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x0009F120 File Offset: 0x0009D320
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				return DataGridViewTextBoxCell.defaultValueType;
			}
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x0009F144 File Offset: 0x0009D344
		internal override void CacheEditingControl()
		{
			this.EditingTextBox = (base.DataGridView.EditingControl as DataGridViewTextBoxEditingControl);
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewTextBoxCell" />.</returns>
		// Token: 0x06001FDD RID: 8157 RVA: 0x0009F15C File Offset: 0x0009D35C
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewTextBoxCell dataGridViewTextBoxCell;
			if (type == DataGridViewTextBoxCell.cellType)
			{
				dataGridViewTextBoxCell = new DataGridViewTextBoxCell();
			}
			else
			{
				dataGridViewTextBoxCell = (DataGridViewTextBoxCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewTextBoxCell);
			dataGridViewTextBoxCell.MaxInputLength = this.MaxInputLength;
			return dataGridViewTextBoxCell;
		}

		/// <summary>Removes the cell's editing control from the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x06001FDE RID: 8158 RVA: 0x0009F1A8 File Offset: 0x0009D3A8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override void DetachEditingControl()
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null || dataGridView.EditingControl == null)
			{
				throw new InvalidOperationException();
			}
			TextBox textBox = dataGridView.EditingControl as TextBox;
			if (textBox != null)
			{
				textBox.ClearUndo();
			}
			this.EditingTextBox = null;
			base.DetachEditingControl();
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x0009F1F0 File Offset: 0x0009D3F0
		private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
		{
			TextBox textBox = base.DataGridView.EditingControl as TextBox;
			int width = editingControlBounds.Width;
			if (textBox != null)
			{
				DataGridViewContentAlignment alignment = cellStyle.Alignment;
				if (alignment <= DataGridViewContentAlignment.MiddleCenter)
				{
					switch (alignment)
					{
					case DataGridViewContentAlignment.TopLeft:
						break;
					case DataGridViewContentAlignment.TopCenter:
						goto IL_EF;
					case (DataGridViewContentAlignment)3:
						goto IL_171;
					case DataGridViewContentAlignment.TopRight:
						goto IL_116;
					default:
						if (alignment != DataGridViewContentAlignment.MiddleLeft)
						{
							if (alignment != DataGridViewContentAlignment.MiddleCenter)
							{
								goto IL_171;
							}
							goto IL_EF;
						}
						break;
					}
				}
				else if (alignment <= DataGridViewContentAlignment.BottomLeft)
				{
					if (alignment == DataGridViewContentAlignment.MiddleRight)
					{
						goto IL_116;
					}
					if (alignment != DataGridViewContentAlignment.BottomLeft)
					{
						goto IL_171;
					}
				}
				else
				{
					if (alignment == DataGridViewContentAlignment.BottomCenter)
					{
						goto IL_EF;
					}
					if (alignment != DataGridViewContentAlignment.BottomRight)
					{
						goto IL_171;
					}
					goto IL_116;
				}
				if (base.DataGridView.RightToLeftInternal)
				{
					editingControlBounds.X++;
					editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 3 - 2);
					goto IL_171;
				}
				editingControlBounds.X += 3;
				editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 3 - 1);
				goto IL_171;
				IL_EF:
				editingControlBounds.X++;
				editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 3);
				goto IL_171;
				IL_116:
				if (base.DataGridView.RightToLeftInternal)
				{
					editingControlBounds.X += 3;
					editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 4);
				}
				else
				{
					editingControlBounds.X++;
					editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 4 - 1);
				}
				IL_171:
				alignment = cellStyle.Alignment;
				if (alignment > DataGridViewContentAlignment.MiddleCenter)
				{
					if (alignment <= DataGridViewContentAlignment.BottomLeft)
					{
						if (alignment == DataGridViewContentAlignment.MiddleRight)
						{
							goto IL_1EC;
						}
						if (alignment != DataGridViewContentAlignment.BottomLeft)
						{
							goto IL_217;
						}
					}
					else if (alignment != DataGridViewContentAlignment.BottomCenter && alignment != DataGridViewContentAlignment.BottomRight)
					{
						goto IL_217;
					}
					editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 1);
					goto IL_217;
				}
				if (alignment <= DataGridViewContentAlignment.TopRight)
				{
					if (alignment - DataGridViewContentAlignment.TopLeft > 1 && alignment != DataGridViewContentAlignment.TopRight)
					{
						goto IL_217;
					}
					editingControlBounds.Y += 2;
					editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 2);
					goto IL_217;
				}
				else if (alignment != DataGridViewContentAlignment.MiddleLeft && alignment != DataGridViewContentAlignment.MiddleCenter)
				{
					goto IL_217;
				}
				IL_1EC:
				int height = editingControlBounds.Height;
				editingControlBounds.Height = height + 1;
				IL_217:
				int num;
				if (cellStyle.WrapMode == DataGridViewTriState.False)
				{
					num = textBox.PreferredSize.Height;
				}
				else
				{
					string text = (string)((IDataGridViewEditingControl)textBox).GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
					if (string.IsNullOrEmpty(text))
					{
						text = " ";
					}
					TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
					using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
					{
						num = DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, width, flags);
					}
				}
				if (num < editingControlBounds.Height)
				{
					alignment = cellStyle.Alignment;
					if (alignment > DataGridViewContentAlignment.MiddleCenter)
					{
						if (alignment <= DataGridViewContentAlignment.BottomLeft)
						{
							if (alignment == DataGridViewContentAlignment.MiddleRight)
							{
								goto IL_2F9;
							}
							if (alignment != DataGridViewContentAlignment.BottomLeft)
							{
								return editingControlBounds;
							}
						}
						else if (alignment != DataGridViewContentAlignment.BottomCenter && alignment != DataGridViewContentAlignment.BottomRight)
						{
							return editingControlBounds;
						}
						editingControlBounds.Y += editingControlBounds.Height - num;
						return editingControlBounds;
					}
					if (alignment <= DataGridViewContentAlignment.TopRight)
					{
						if (alignment - DataGridViewContentAlignment.TopLeft > 1 && alignment != DataGridViewContentAlignment.TopRight)
						{
							return editingControlBounds;
						}
						return editingControlBounds;
					}
					else if (alignment != DataGridViewContentAlignment.MiddleLeft && alignment != DataGridViewContentAlignment.MiddleCenter)
					{
						return editingControlBounds;
					}
					IL_2F9:
					editingControlBounds.Y += (editingControlBounds.Height - num) / 2;
				}
			}
			return editingControlBounds;
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		// Token: 0x06001FE0 RID: 8160 RVA: 0x0009F53C File Offset: 0x0009D73C
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
			object value = this.GetValue(rowIndex);
			object formattedValue = this.GetFormattedValue(value, rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting);
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates cellState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out cellState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, cellState, formattedValue, null, cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, true, false, false);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's error icon, if one is displayed.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's error icon, if one is displayed; otherwise, <see cref="F:System.Drawing.Rectangle.Empty" />.</returns>
		// Token: 0x06001FE1 RID: 8161 RVA: 0x0009F5B0 File Offset: 0x0009D7B0
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
			DataGridViewElementStates cellState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out cellState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, cellState, null, this.GetErrorText(rowIndex), cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		// Token: 0x06001FE2 RID: 8162 RVA: 0x0009F628 File Offset: 0x0009D828
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
			object formattedValue = base.GetFormattedValue(rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.PreferredSize);
			string text = formattedValue as string;
			if (string.IsNullOrEmpty(text))
			{
				text = " ";
			}
			TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			Size result;
			if (cellStyle.WrapMode == DataGridViewTriState.True && text.Length > 1)
			{
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
				{
					if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
					{
						result = new Size(DataGridViewCell.MeasureTextWidth(graphics, text, cellStyle.Font, Math.Max(1, constraintSize.Height - num2 - 1 - 1), flags), 0);
					}
					else
					{
						result = DataGridViewCell.MeasureTextPreferredSize(graphics, text, cellStyle.Font, 5f, flags);
					}
				}
				else
				{
					result = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, Math.Max(1, constraintSize.Width - num), flags));
				}
			}
			else if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
				{
					result = new Size(DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Width, 0);
				}
				else
				{
					result = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags);
				}
			}
			else
			{
				result = new Size(0, DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Height);
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				result.Width += num;
				if (base.DataGridView.ShowCellErrors)
				{
					result.Width = Math.Max(result.Width, num + 8 + (int)DataGridViewCell.iconsWidth);
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				int num3 = (cellStyle.WrapMode == DataGridViewTriState.True) ? 1 : 2;
				result.Height += num3 + 1 + num2;
				if (base.DataGridView.ShowCellErrors)
				{
					result.Height = Math.Max(result.Height, num2 + 8 + (int)DataGridViewCell.iconsHeight);
				}
			}
			return result;
		}

		/// <summary>Attaches and initializes the hosted editing control.</summary>
		/// <param name="rowIndex">The index of the row being edited.</param>
		/// <param name="initialFormattedValue">The initial value to be displayed in the control.</param>
		/// <param name="dataGridViewCellStyle">A cell style that is used to determine the appearance of the hosted control.</param>
		// Token: 0x06001FE3 RID: 8163 RVA: 0x0009F868 File Offset: 0x0009DA68
		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			TextBox textBox = base.DataGridView.EditingControl as TextBox;
			if (textBox != null)
			{
				textBox.BorderStyle = BorderStyle.None;
				textBox.AcceptsReturn = (textBox.Multiline = (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True));
				textBox.MaxLength = this.MaxInputLength;
				string text = initialFormattedValue as string;
				if (text == null)
				{
					textBox.Text = string.Empty;
				}
				else
				{
					textBox.Text = text;
				}
				this.EditingTextBox = (base.DataGridView.EditingControl as DataGridViewTextBoxEditingControl);
			}
		}

		/// <summary>Determines if edit mode should be started based on the given key.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that represents the key that was pressed.</param>
		/// <returns>
		///     <see langword="true" /> if edit mode should be started; otherwise, <see langword="false" />. </returns>
		// Token: 0x06001FE4 RID: 8164 RVA: 0x0009F8F4 File Offset: 0x0009DAF4
		public override bool KeyEntersEditMode(KeyEventArgs e)
		{
			return (((char.IsLetterOrDigit((char)e.KeyCode) && (e.KeyCode < Keys.F1 || e.KeyCode > Keys.F24)) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.Divide) || (e.KeyCode >= Keys.OemSemicolon && e.KeyCode <= Keys.OemBackslash) || (e.KeyCode == Keys.Space && !e.Shift)) && !e.Alt && !e.Control) || base.KeyEntersEditMode(e);
		}

		/// <summary>Called by <see cref="T:System.Windows.Forms.DataGridView" /> when the selection cursor moves onto a cell.</summary>
		/// <param name="rowIndex">The index of the row entered by the mouse.</param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if the cell was entered as a result of a mouse click; otherwise, <see langword="false" />.</param>
		// Token: 0x06001FE5 RID: 8165 RVA: 0x0009F97F File Offset: 0x0009DB7F
		protected override void OnEnter(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (throughMouseClick)
			{
				this.flagsState |= 1;
			}
		}

		/// <summary>Called by the <see cref="T:System.Windows.Forms.DataGridView" /> when the mouse leaves a cell.</summary>
		/// <param name="rowIndex">The index of the row the mouse has left.</param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if the cell was left as a result of a mouse click; otherwise, <see langword="false" />.</param>
		// Token: 0x06001FE6 RID: 8166 RVA: 0x0009F99C File Offset: 0x0009DB9C
		protected override void OnLeave(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			this.flagsState = (byte)((int)this.flagsState & -2);
		}

		/// <summary>Called by <see cref="T:System.Windows.Forms.DataGridView" /> when the mouse leaves a cell.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06001FE7 RID: 8167 RVA: 0x0009F9B8 File Offset: 0x0009DBB8
		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == e.ColumnIndex && currentCellAddress.Y == e.RowIndex && e.Button == MouseButtons.Left)
			{
				if ((this.flagsState & 1) != 0)
				{
					this.flagsState = (byte)((int)this.flagsState & -2);
					return;
				}
				if (base.DataGridView.EditMode != DataGridViewEditMode.EditProgrammatically)
				{
					base.DataGridView.BeginEdit(true);
				}
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x0009FA3B File Offset: 0x0009DC3B
		private bool OwnsEditingTextBox(int rowIndex)
		{
			return rowIndex != -1 && this.EditingTextBox != null && rowIndex == ((IDataGridViewEditingControl)this.EditingTextBox).EditingControlRowIndex;
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
		/// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the bounds of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being painted.</param>
		/// <param name="rowIndex">The row index of the cell that is being painted.</param>
		/// <param name="cellState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the cell.</param>
		/// <param name="value">The data of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being painted.</param>
		/// <param name="formattedValue">The formatted data of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being painted.</param>
		/// <param name="errorText">An error message that is associated with the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the cell.</param>
		/// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that contains border styles for the cell that is being painted.</param>
		/// <param name="paintParts">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values that specifies which parts of the cell need to be painted.</param>
		// Token: 0x06001FE9 RID: 8169 RVA: 0x0009FA5C File Offset: 0x0009DC5C
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, cellState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x0009FA94 File Offset: 0x0009DC94
		private Rectangle PaintPrivate(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			Rectangle result = Rectangle.Empty;
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle = this.BorderWidths(advancedBorderStyle);
			Rectangle rectangle2 = cellBounds;
			rectangle2.Offset(rectangle.X, rectangle.Y);
			rectangle2.Width -= rectangle.Right;
			rectangle2.Height -= rectangle.Bottom;
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			bool flag = currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex;
			bool flag2 = flag && base.DataGridView.EditingControl != null;
			bool flag3 = (cellState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
			SolidBrush cachedBrush;
			if (DataGridViewCell.PaintSelectionBackground(paintParts) && flag3 && !flag2)
			{
				cachedBrush = base.DataGridView.GetCachedBrush(cellStyle.SelectionBackColor);
			}
			else
			{
				cachedBrush = base.DataGridView.GetCachedBrush(cellStyle.BackColor);
			}
			if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255 && rectangle2.Width > 0 && rectangle2.Height > 0)
			{
				graphics.FillRectangle(cachedBrush, rectangle2);
			}
			if (cellStyle.Padding != Padding.Empty)
			{
				if (base.DataGridView.RightToLeftInternal)
				{
					rectangle2.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
				}
				else
				{
					rectangle2.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
				}
				rectangle2.Width -= cellStyle.Padding.Horizontal;
				rectangle2.Height -= cellStyle.Padding.Vertical;
			}
			if (paint && flag && !flag2 && DataGridViewCell.PaintFocus(paintParts) && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && rectangle2.Width > 0 && rectangle2.Height > 0)
			{
				ControlPaint.DrawFocusRectangle(graphics, rectangle2, Color.Empty, cachedBrush.Color);
			}
			Rectangle cellValueBounds = rectangle2;
			string text = formattedValue as string;
			if (text != null && ((paint && !flag2) || computeContentBounds))
			{
				int num = (cellStyle.WrapMode == DataGridViewTriState.True) ? 1 : 2;
				rectangle2.Offset(0, num);
				rectangle2.Width = rectangle2.Width;
				rectangle2.Height -= num + 1;
				if (rectangle2.Width > 0 && rectangle2.Height > 0)
				{
					TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
					if (paint)
					{
						if (DataGridViewCell.PaintContentForeground(paintParts))
						{
							if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
							{
								textFormatFlags |= TextFormatFlags.EndEllipsis;
							}
							TextRenderer.DrawText(graphics, text, cellStyle.Font, rectangle2, flag3 ? cellStyle.SelectionForeColor : cellStyle.ForeColor, textFormatFlags);
						}
					}
					else
					{
						result = DataGridViewUtilities.GetTextBounds(rectangle2, text, textFormatFlags, cellStyle);
					}
				}
			}
			else if (computeErrorIconBounds && !string.IsNullOrEmpty(errorText))
			{
				result = base.ComputeErrorIconBounds(cellValueBounds);
			}
			if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
			{
				base.PaintErrorIcon(graphics, cellStyle, rowIndex, cellBounds, cellValueBounds, errorText);
			}
			return result;
		}

		/// <summary>Sets the location and size of the editing control hosted by a cell in the DataGridView control. </summary>
		/// <param name="setLocation">
		///       <see langword="true" /> to have the control placed as specified by the other arguments; <see langword="false" /> to allow the control to place itself.</param>
		/// <param name="setSize">
		///       <see langword="true" /> to specify the size; <see langword="false" /> to allow the control to size itself.</param>
		/// <param name="cellBounds"> A <see cref="T:System.Drawing.Rectangle" /> that defines the cell bounds.</param>
		/// <param name="cellClip">The area that will be used to paint the editing control.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell being edited.</param>
		/// <param name="singleVerticalBorderAdded">
		///       <see langword="true" /> to add a vertical border to the cell; otherwise, <see langword="false" />.</param>
		/// <param name="singleHorizontalBorderAdded">
		///       <see langword="true" /> to add a horizontal border to the cell; otherwise, <see langword="false" />.</param>
		/// <param name="isFirstDisplayedColumn">
		///       <see langword="true" /> if the hosting cell is in the first visible column; otherwise, <see langword="false" />.</param>
		/// <param name="isFirstDisplayedRow">
		///       <see langword="true" /> if the hosting cell is in the first visible row; otherwise, <see langword="false" />.</param>
		// Token: 0x06001FEB RID: 8171 RVA: 0x0009FE04 File Offset: 0x0009E004
		public override void PositionEditingControl(bool setLocation, bool setSize, Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
		{
			Rectangle editingControlBounds = this.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
			editingControlBounds = this.GetAdjustedEditingControlBounds(editingControlBounds, cellStyle);
			base.DataGridView.EditingControl.Location = new Point(editingControlBounds.X, editingControlBounds.Y);
			base.DataGridView.EditingControl.Size = new Size(editingControlBounds.Width, editingControlBounds.Height);
		}

		/// <summary>Returns a string that describes the current object. </summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x06001FEC RID: 8172 RVA: 0x0009FE78 File Offset: 0x0009E078
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewTextBoxCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x04000DBE RID: 3518
		private static readonly int PropTextBoxCellMaxInputLength = PropertyStore.CreateKey();

		// Token: 0x04000DBF RID: 3519
		private static readonly int PropTextBoxCellEditingTextBox = PropertyStore.CreateKey();

		// Token: 0x04000DC0 RID: 3520
		private const byte DATAGRIDVIEWTEXTBOXCELL_ignoreNextMouseClick = 1;

		// Token: 0x04000DC1 RID: 3521
		private const byte DATAGRIDVIEWTEXTBOXCELL_horizontalTextOffsetLeft = 3;

		// Token: 0x04000DC2 RID: 3522
		private const byte DATAGRIDVIEWTEXTBOXCELL_horizontalTextOffsetRight = 4;

		// Token: 0x04000DC3 RID: 3523
		private const byte DATAGRIDVIEWTEXTBOXCELL_horizontalTextMarginLeft = 0;

		// Token: 0x04000DC4 RID: 3524
		private const byte DATAGRIDVIEWTEXTBOXCELL_horizontalTextMarginRight = 0;

		// Token: 0x04000DC5 RID: 3525
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextOffsetTop = 2;

		// Token: 0x04000DC6 RID: 3526
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextOffsetBottom = 1;

		// Token: 0x04000DC7 RID: 3527
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextMarginTopWithWrapping = 1;

		// Token: 0x04000DC8 RID: 3528
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextMarginTopWithoutWrapping = 2;

		// Token: 0x04000DC9 RID: 3529
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextMarginBottom = 1;

		// Token: 0x04000DCA RID: 3530
		private const int DATAGRIDVIEWTEXTBOXCELL_maxInputLength = 32767;

		// Token: 0x04000DCB RID: 3531
		private byte flagsState;

		// Token: 0x04000DCC RID: 3532
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04000DCD RID: 3533
		private static Type defaultValueType = typeof(object);

		// Token: 0x04000DCE RID: 3534
		private static Type cellType = typeof(DataGridViewTextBoxCell);

		/// <summary>Represents the accessibility object for the current <see cref="T:System.Windows.Forms.DataGridViewTextBoxCell" /> object. </summary>
		// Token: 0x020005C0 RID: 1472
		protected class DataGridViewTextBoxCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			/// <summary>Instantiates a new instance of the <see cref="T:System.Windows.Forms.DataGridViewTextBoxCell.DataGridViewTextBoxCellAccessibleObject" /> class. </summary>
			/// <param name="owner">The control to which this object belongs. </param>
			// Token: 0x060059D8 RID: 23000 RVA: 0x00176D3A File Offset: 0x00174F3A
			public DataGridViewTextBoxCellAccessibleObject(DataGridViewCell owner) : base(owner)
			{
			}

			// Token: 0x060059D9 RID: 23001 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override bool IsIAccessibleExSupported()
			{
				return true;
			}

			// Token: 0x060059DA RID: 23002 RVA: 0x0017A77B File Offset: 0x0017897B
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50004;
				}
				return base.GetPropertyValue(propertyID);
			}
		}
	}
}
