using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Displays a check box user interface (UI) to use in a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001AB RID: 427
	public class DataGridViewCheckBoxCell : DataGridViewCell, IDataGridViewEditingCell
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" /> class to its default state.</summary>
		// Token: 0x06001B70 RID: 7024 RVA: 0x0008881F File Offset: 0x00086A1F
		public DataGridViewCheckBoxCell() : this(false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" /> class, enabling binary or ternary state.</summary>
		/// <param name="threeState">
		///       <see langword="true" /> to enable ternary state; <see langword="false" /> to enable binary state.</param>
		// Token: 0x06001B71 RID: 7025 RVA: 0x00088828 File Offset: 0x00086A28
		public DataGridViewCheckBoxCell(bool threeState)
		{
			if (threeState)
			{
				this.flags = 1;
			}
		}

		/// <summary>Gets or sets the formatted value of the control hosted by the cell when it is in edit mode.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing the cell's value.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Windows.Forms.DataGridViewCheckBoxCell.FormattedValueType" /> property value is <see langword="null" />.-or-The assigned value is <see langword="null" /> or is not of the type indicated by the <see cref="P:System.Windows.Forms.DataGridViewCheckBoxCell.FormattedValueType" /> property.-or- The assigned value is not of type <see cref="T:System.Boolean" /> nor of type <see cref="T:System.Windows.Forms.CheckState" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.DataGridViewCheckBoxCell.FormattedValueType" /> property value is <see langword="null" />.</exception>
		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001B72 RID: 7026 RVA: 0x0008883A File Offset: 0x00086A3A
		// (set) Token: 0x06001B73 RID: 7027 RVA: 0x00088844 File Offset: 0x00086A44
		public virtual object EditingCellFormattedValue
		{
			get
			{
				return this.GetEditingCellFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			set
			{
				if (this.FormattedValueType == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewCell_FormattedValueTypeNull"));
				}
				if (value == null || !this.FormattedValueType.IsAssignableFrom(value.GetType()))
				{
					throw new ArgumentException(SR.GetString("DataGridViewCheckBoxCell_InvalidValueType"));
				}
				if (value is CheckState)
				{
					if ((CheckState)value == CheckState.Checked)
					{
						this.flags |= 16;
						this.flags = (byte)((int)this.flags & -33);
						return;
					}
					if ((CheckState)value == CheckState.Indeterminate)
					{
						this.flags |= 32;
						this.flags = (byte)((int)this.flags & -17);
						return;
					}
					this.flags = (byte)((int)this.flags & -17);
					this.flags = (byte)((int)this.flags & -33);
					return;
				}
				else
				{
					if (value is bool)
					{
						if ((bool)value)
						{
							this.flags |= 16;
						}
						else
						{
							this.flags = (byte)((int)this.flags & -17);
						}
						this.flags = (byte)((int)this.flags & -33);
						return;
					}
					throw new ArgumentException(SR.GetString("DataGridViewCheckBoxCell_InvalidValueType"));
				}
			}
		}

		/// <summary>Gets or sets a flag indicating that the value has been changed for this cell.</summary>
		/// <returns>
		///     <see langword="true" /> if the cell's value has changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001B74 RID: 7028 RVA: 0x00088964 File Offset: 0x00086B64
		// (set) Token: 0x06001B75 RID: 7029 RVA: 0x00088971 File Offset: 0x00086B71
		public virtual bool EditingCellValueChanged
		{
			get
			{
				return (this.flags & 2) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 2;
					return;
				}
				this.flags = (byte)((int)this.flags & -3);
			}
		}

		/// <summary>Gets the formatted value of the cell while it is in edit mode.</summary>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values that describes the context in which any formatting error occurs. </param>
		/// <returns>An <see cref="T:System.Object" /> representing the formatted value of the editing cell. </returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.DataGridViewCheckBoxCell.FormattedValueType" /> property value is <see langword="null" />.</exception>
		// Token: 0x06001B76 RID: 7030 RVA: 0x00088998 File Offset: 0x00086B98
		public virtual object GetEditingCellFormattedValue(DataGridViewDataErrorContexts context)
		{
			if (this.FormattedValueType == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCell_FormattedValueTypeNull"));
			}
			if (this.FormattedValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
			{
				if ((this.flags & 16) != 0)
				{
					if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
					{
						return SR.GetString("DataGridViewCheckBoxCell_ClipboardChecked");
					}
					return CheckState.Checked;
				}
				else if ((this.flags & 32) != 0)
				{
					if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
					{
						return SR.GetString("DataGridViewCheckBoxCell_ClipboardIndeterminate");
					}
					return CheckState.Indeterminate;
				}
				else
				{
					if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
					{
						return SR.GetString("DataGridViewCheckBoxCell_ClipboardUnchecked");
					}
					return CheckState.Unchecked;
				}
			}
			else
			{
				if (!this.FormattedValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
				{
					return null;
				}
				bool flag = (this.flags & 16) > 0;
				if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
				{
					return SR.GetString(flag ? "DataGridViewCheckBoxCell_ClipboardTrue" : "DataGridViewCheckBoxCell_ClipboardFalse");
				}
				return flag;
			}
		}

		/// <summary>This method is not meaningful for this type.</summary>
		/// <param name="selectAll">This parameter is ignored.</param>
		// Token: 0x06001B77 RID: 7031 RVA: 0x0000701A File Offset: 0x0000521A
		public virtual void PrepareEditingCellForEdit(bool selectAll)
		{
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x00088A84 File Offset: 0x00086C84
		// (set) Token: 0x06001B79 RID: 7033 RVA: 0x00088AAA File Offset: 0x00086CAA
		private ButtonState ButtonState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewCheckBoxCell.PropButtonCellState, out flag);
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
					base.Properties.SetInteger(DataGridViewCheckBoxCell.PropButtonCellState, (int)value);
				}
			}
		}

		/// <summary>Gets the type of the cell's hosted editing control.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the underlying editing control.</returns>
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001B7A RID: 7034 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override Type EditType
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets or sets the underlying value corresponding to a cell value of <see langword="false" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> corresponding to a cell value of <see langword="false" />. The default is <see langword="null" />.</returns>
		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001B7B RID: 7035 RVA: 0x00088AC6 File Offset: 0x00086CC6
		// (set) Token: 0x06001B7C RID: 7036 RVA: 0x00088AD8 File Offset: 0x00086CD8
		[DefaultValue(null)]
		public object FalseValue
		{
			get
			{
				return base.Properties.GetObject(DataGridViewCheckBoxCell.PropFalseValue);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropFalseValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropFalseValue, value);
					if (base.DataGridView != null)
					{
						if (base.RowIndex != -1)
						{
							base.DataGridView.InvalidateCell(this);
							return;
						}
						base.DataGridView.InvalidateColumnInternal(base.ColumnIndex);
					}
				}
			}
		}

		// Token: 0x1700065D RID: 1629
		// (set) Token: 0x06001B7D RID: 7037 RVA: 0x00088B3A File Offset: 0x00086D3A
		internal object FalseValueInternal
		{
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropFalseValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropFalseValue, value);
				}
			}
		}

		/// <summary>Gets or sets the flat style appearance of the check box user interface (UI).</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlatStyle" /> values. The default is <see cref="F:System.Windows.Forms.FlatStyle.Standard" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.FlatStyle" /> value.</exception>
		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001B7E RID: 7038 RVA: 0x00088B64 File Offset: 0x00086D64
		// (set) Token: 0x06001B7F RID: 7039 RVA: 0x00088B8C File Offset: 0x00086D8C
		[DefaultValue(FlatStyle.Standard)]
		public FlatStyle FlatStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewCheckBoxCell.PropFlatStyle, out flag);
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
					base.Properties.SetInteger(DataGridViewCheckBoxCell.PropFlatStyle, (int)value);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x1700065F RID: 1631
		// (set) Token: 0x06001B80 RID: 7040 RVA: 0x00088BDF File Offset: 0x00086DDF
		internal FlatStyle FlatStyleInternal
		{
			set
			{
				if (value != this.FlatStyle)
				{
					base.Properties.SetInteger(DataGridViewCheckBoxCell.PropFlatStyle, (int)value);
				}
			}
		}

		/// <summary>Gets the type of the cell display value. </summary>
		/// <returns>A <see cref="T:System.Type" /> representing the display type of the cell.</returns>
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001B81 RID: 7041 RVA: 0x00088BFB File Offset: 0x00086DFB
		public override Type FormattedValueType
		{
			get
			{
				if (this.ThreeState)
				{
					return DataGridViewCheckBoxCell.defaultCheckStateType;
				}
				return DataGridViewCheckBoxCell.defaultBooleanType;
			}
		}

		/// <summary>Gets or sets the underlying value corresponding to an indeterminate or <see langword="null" /> cell value.</summary>
		/// <returns>An <see cref="T:System.Object" /> corresponding to an indeterminate or <see langword="null" /> cell value. The default is <see langword="null" />.</returns>
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x00088C10 File Offset: 0x00086E10
		// (set) Token: 0x06001B83 RID: 7043 RVA: 0x00088C24 File Offset: 0x00086E24
		[DefaultValue(null)]
		public object IndeterminateValue
		{
			get
			{
				return base.Properties.GetObject(DataGridViewCheckBoxCell.PropIndeterminateValue);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropIndeterminateValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropIndeterminateValue, value);
					if (base.DataGridView != null)
					{
						if (base.RowIndex != -1)
						{
							base.DataGridView.InvalidateCell(this);
							return;
						}
						base.DataGridView.InvalidateColumnInternal(base.ColumnIndex);
					}
				}
			}
		}

		// Token: 0x17000662 RID: 1634
		// (set) Token: 0x06001B84 RID: 7044 RVA: 0x00088C86 File Offset: 0x00086E86
		internal object IndeterminateValueInternal
		{
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropIndeterminateValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropIndeterminateValue, value);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether ternary mode has been enabled for the hosted check box control.</summary>
		/// <returns>
		///     <see langword="true" /> if ternary mode is enabled; <see langword="false" /> if binary mode is enabled. The default is <see langword="false" />.</returns>
		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001B85 RID: 7045 RVA: 0x00088CAE File Offset: 0x00086EAE
		// (set) Token: 0x06001B86 RID: 7046 RVA: 0x00088CBC File Offset: 0x00086EBC
		[DefaultValue(false)]
		public bool ThreeState
		{
			get
			{
				return (this.flags & 1) > 0;
			}
			set
			{
				if (this.ThreeState != value)
				{
					this.ThreeStateInternal = value;
					if (base.DataGridView != null)
					{
						if (base.RowIndex != -1)
						{
							base.DataGridView.InvalidateCell(this);
							return;
						}
						base.DataGridView.InvalidateColumnInternal(base.ColumnIndex);
					}
				}
			}
		}

		// Token: 0x17000664 RID: 1636
		// (set) Token: 0x06001B87 RID: 7047 RVA: 0x00088D08 File Offset: 0x00086F08
		internal bool ThreeStateInternal
		{
			set
			{
				if (this.ThreeState != value)
				{
					if (value)
					{
						this.flags |= 1;
						return;
					}
					this.flags = (byte)((int)this.flags & -2);
				}
			}
		}

		/// <summary>Gets or sets the underlying value corresponding to a cell value of <see langword="true" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> corresponding to a cell value of <see langword="true" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x00088D36 File Offset: 0x00086F36
		// (set) Token: 0x06001B89 RID: 7049 RVA: 0x00088D48 File Offset: 0x00086F48
		[DefaultValue(null)]
		public object TrueValue
		{
			get
			{
				return base.Properties.GetObject(DataGridViewCheckBoxCell.PropTrueValue);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropTrueValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropTrueValue, value);
					if (base.DataGridView != null)
					{
						if (base.RowIndex != -1)
						{
							base.DataGridView.InvalidateCell(this);
							return;
						}
						base.DataGridView.InvalidateColumnInternal(base.ColumnIndex);
					}
				}
			}
		}

		// Token: 0x17000666 RID: 1638
		// (set) Token: 0x06001B8A RID: 7050 RVA: 0x00088DAA File Offset: 0x00086FAA
		internal object TrueValueInternal
		{
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropTrueValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropTrueValue, value);
				}
			}
		}

		/// <summary>Gets the data type of the values in the cell.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the underlying value of the cell.</returns>
		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001B8B RID: 7051 RVA: 0x00088DD4 File Offset: 0x00086FD4
		// (set) Token: 0x06001B8C RID: 7052 RVA: 0x00088E06 File Offset: 0x00087006
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				if (this.ThreeState)
				{
					return DataGridViewCheckBoxCell.defaultCheckStateType;
				}
				return DataGridViewCheckBoxCell.defaultBooleanType;
			}
			set
			{
				base.ValueType = value;
				this.ThreeState = (value != null && DataGridViewCheckBoxCell.defaultCheckStateType.IsAssignableFrom(value));
			}
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" />.</returns>
		// Token: 0x06001B8D RID: 7053 RVA: 0x00088E2C File Offset: 0x0008702C
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewCheckBoxCell dataGridViewCheckBoxCell;
			if (type == DataGridViewCheckBoxCell.cellType)
			{
				dataGridViewCheckBoxCell = new DataGridViewCheckBoxCell();
			}
			else
			{
				dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewCheckBoxCell);
			dataGridViewCheckBoxCell.ThreeStateInternal = this.ThreeState;
			dataGridViewCheckBoxCell.TrueValueInternal = this.TrueValue;
			dataGridViewCheckBoxCell.FalseValueInternal = this.FalseValue;
			dataGridViewCheckBoxCell.IndeterminateValueInternal = this.IndeterminateValue;
			dataGridViewCheckBoxCell.FlatStyleInternal = this.FlatStyle;
			return dataGridViewCheckBoxCell;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x00088EA8 File Offset: 0x000870A8
		private bool CommonContentClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			return currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == e.RowIndex && base.DataGridView.IsCurrentCellInEditMode;
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the cell content is clicked.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains data about the mouse click.</param>
		/// <returns>
		///     <see langword="true" /> if the cell is in edit mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B8F RID: 7055 RVA: 0x00088EEC File Offset: 0x000870EC
		protected override bool ContentClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return this.CommonContentClickUnsharesRow(e);
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the cell content is double-clicked.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains data about the double-click.</param>
		/// <returns>
		///     <see langword="true" /> if the cell is in edit mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B90 RID: 7056 RVA: 0x00088EEC File Offset: 0x000870EC
		protected override bool ContentDoubleClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return this.CommonContentClickUnsharesRow(e);
		}

		/// <summary>Creates a new accessible object for the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" />. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell.DataGridViewCheckBoxCellAccessibleObject" /> for the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" />. </returns>
		// Token: 0x06001B91 RID: 7057 RVA: 0x00088EF5 File Offset: 0x000870F5
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewCheckBoxCell.DataGridViewCheckBoxCellAccessibleObject(this);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		// Token: 0x06001B92 RID: 7058 RVA: 0x00088F00 File Offset: 0x00087100
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
		// Token: 0x06001B93 RID: 7059 RVA: 0x00088F58 File Offset: 0x00087158
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
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex && base.DataGridView.IsCurrentCellInEditMode)
			{
				return Rectangle.Empty;
			}
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates elementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out elementState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, elementState, null, this.GetErrorText(rowIndex), cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		/// <summary>Gets the formatted value of the cell's data. </summary>
		/// <param name="value">The value to be formatted. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> in effect for the cell.</param>
		/// <param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> associated with the value type that provides custom conversion to the formatted value type, or <see langword="null" /> if no such custom conversion is needed.</param>
		/// <param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> associated with the formatted value type that provides custom conversion from the value type, or <see langword="null" /> if no such custom conversion is needed.</param>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values describing the context in which the formatted value is needed.</param>
		/// <returns>The value of the cell's data after formatting has been applied or <see langword="null" /> if the cell is not part of a <see cref="T:System.Windows.Forms.DataGridView" /> control.</returns>
		// Token: 0x06001B94 RID: 7060 RVA: 0x0008900C File Offset: 0x0008720C
		protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
		{
			if (value != null)
			{
				if (this.ThreeState)
				{
					if (value.Equals(this.TrueValue) || (value is int && (int)value == 1))
					{
						value = CheckState.Checked;
					}
					else if (value.Equals(this.FalseValue) || (value is int && (int)value == 0))
					{
						value = CheckState.Unchecked;
					}
					else if (value.Equals(this.IndeterminateValue) || (value is int && (int)value == 2))
					{
						value = CheckState.Indeterminate;
					}
				}
				else if (value.Equals(this.TrueValue) || (value is int && (int)value != 0))
				{
					value = true;
				}
				else if (value.Equals(this.FalseValue) || (value is int && (int)value == 0))
				{
					value = false;
				}
			}
			object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
			if (formattedValue != null && (context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
			{
				if (formattedValue is bool)
				{
					bool flag = (bool)formattedValue;
					if (flag)
					{
						return SR.GetString(this.ThreeState ? "DataGridViewCheckBoxCell_ClipboardChecked" : "DataGridViewCheckBoxCell_ClipboardTrue");
					}
					return SR.GetString(this.ThreeState ? "DataGridViewCheckBoxCell_ClipboardUnchecked" : "DataGridViewCheckBoxCell_ClipboardFalse");
				}
				else if (formattedValue is CheckState)
				{
					CheckState checkState = (CheckState)formattedValue;
					if (checkState == CheckState.Checked)
					{
						return SR.GetString(this.ThreeState ? "DataGridViewCheckBoxCell_ClipboardChecked" : "DataGridViewCheckBoxCell_ClipboardTrue");
					}
					if (checkState == CheckState.Unchecked)
					{
						return SR.GetString(this.ThreeState ? "DataGridViewCheckBoxCell_ClipboardUnchecked" : "DataGridViewCheckBoxCell_ClipboardFalse");
					}
					return SR.GetString("DataGridViewCheckBoxCell_ClipboardIndeterminate");
				}
			}
			return formattedValue;
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		// Token: 0x06001B95 RID: 7061 RVA: 0x000891B4 File Offset: 0x000873B4
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
			DataGridViewFreeDimension freeDimensionFromConstraint = DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize);
			Rectangle stdBorderWidths = base.StdBorderWidths;
			int num = stdBorderWidths.Left + stdBorderWidths.Width + cellStyle.Padding.Horizontal;
			int num2 = stdBorderWidths.Top + stdBorderWidths.Height + cellStyle.Padding.Vertical;
			Size result;
			if (base.DataGridView.ApplyVisualStylesToInnerCells)
			{
				Size glyphSize = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal);
				switch (this.FlatStyle)
				{
				case FlatStyle.Flat:
					glyphSize.Width -= 3;
					glyphSize.Height -= 3;
					break;
				case FlatStyle.Popup:
					glyphSize.Width -= 2;
					glyphSize.Height -= 2;
					break;
				}
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
				{
					if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
					{
						result = new Size(glyphSize.Width + num + 4, 0);
					}
					else
					{
						result = new Size(glyphSize.Width + num + 4, glyphSize.Height + num2 + 4);
					}
				}
				else
				{
					result = new Size(0, glyphSize.Height + num2 + 4);
				}
			}
			else
			{
				FlatStyle flatStyle = this.FlatStyle;
				int num3;
				if (flatStyle != FlatStyle.Flat)
				{
					if (flatStyle != FlatStyle.Popup)
					{
						num3 = SystemInformation.Border3DSize.Width * 2 + 9 + 4;
					}
					else
					{
						num3 = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal).Width - 2;
					}
				}
				else
				{
					num3 = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal).Width - 3;
				}
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
				{
					if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
					{
						result = new Size(num3 + num, 0);
					}
					else
					{
						result = new Size(num3 + num, num3 + num2);
					}
				}
				else
				{
					result = new Size(0, num3 + num2);
				}
			}
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out dataGridViewElementStates, out rectangle);
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			result.Width += rectangle2.X;
			result.Height += rectangle2.Y;
			if (base.DataGridView.ShowCellErrors)
			{
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
				{
					result.Width = Math.Max(result.Width, num + 8 + (int)DataGridViewCell.iconsWidth);
				}
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
				{
					result.Height = Math.Max(result.Height, num2 + 8 + (int)DataGridViewCell.iconsHeight);
				}
			}
			return result;
		}

		/// <summary>Indicates whether the row containing the cell is unshared when a key is pressed while the cell has focus.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains data about the key press. </param>
		/// <param name="rowIndex">The index of the row containing the cell. </param>
		/// <returns>
		///     <see langword="true" /> if the SPACE key is pressed and the CTRL, ALT, and SHIFT keys are all not pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B96 RID: 7062 RVA: 0x0008005C File Offset: 0x0007E25C
		protected override bool KeyDownUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode == Keys.Space && !e.Alt && !e.Control && !e.Shift;
		}

		/// <summary>Indicates whether the row containing the cell is unshared when a key is released while the cell has focus.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains data about the key press. </param>
		/// <param name="rowIndex">The index of the row containing the cell. </param>
		/// <returns>
		///     <see langword="true" /> if the SPACE key is released; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B97 RID: 7063 RVA: 0x00080083 File Offset: 0x0007E283
		protected override bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode == Keys.Space;
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the mouse button is pressed while the pointer is over the cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains data about the mouse click.</param>
		/// <returns>Always <see langword="true" />.</returns>
		// Token: 0x06001B98 RID: 7064 RVA: 0x0008008F File Offset: 0x0007E28F
		protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left;
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the mouse pointer moves over the cell.</summary>
		/// <param name="rowIndex">The index of the row containing the cell.</param>
		/// <returns>
		///     <see langword="true" /> if the cell was the last cell receiving a mouse click; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B99 RID: 7065 RVA: 0x0008941C File Offset: 0x0008761C
		protected override bool MouseEnterUnsharesRow(int rowIndex)
		{
			return base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y;
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the mouse pointer leaves the cell.</summary>
		/// <param name="rowIndex">The index of the row containing the cell.</param>
		/// <returns>
		///     <see langword="true" /> if the button is not in the normal state; <see langword="false" /> if the button is in the pressed state.</returns>
		// Token: 0x06001B9A RID: 7066 RVA: 0x0008945C File Offset: 0x0008765C
		protected override bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return (this.ButtonState & ButtonState.Pushed) > ButtonState.Normal;
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the mouse button is released while the pointer is over the cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains data about the mouse click.</param>
		/// <returns>Always <see langword="true" />.</returns>
		// Token: 0x06001B9B RID: 7067 RVA: 0x0008008F File Offset: 0x0007E28F
		protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left;
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x0008946D File Offset: 0x0008766D
		private void NotifyDataGridViewOfValueChange()
		{
			this.flags |= 2;
			base.DataGridView.NotifyCurrentCellDirty(true);
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x0008948C File Offset: 0x0008768C
		private void OnCommonContentClick(DataGridViewCellEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == e.RowIndex && base.DataGridView.IsCurrentCellInEditMode && this.SwitchFormattedValue())
			{
				this.NotifyDataGridViewOfValueChange();
			}
		}

		/// <summary>Called when the cell's contents are clicked.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data.</param>
		// Token: 0x06001B9E RID: 7070 RVA: 0x000894E7 File Offset: 0x000876E7
		protected override void OnContentClick(DataGridViewCellEventArgs e)
		{
			this.OnCommonContentClick(e);
		}

		/// <summary>Called when the cell's contents are double-clicked.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data. </param>
		// Token: 0x06001B9F RID: 7071 RVA: 0x000894E7 File Offset: 0x000876E7
		protected override void OnContentDoubleClick(DataGridViewCellEventArgs e)
		{
			this.OnCommonContentClick(e);
		}

		/// <summary>Called when a character key is pressed while the focus is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data</param>
		/// <param name="rowIndex">The row index of the current cell, or -1 if the cell is not owned by a row.</param>
		// Token: 0x06001BA0 RID: 7072 RVA: 0x000894F0 File Offset: 0x000876F0
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

		/// <summary>Called when a character key is released while the focus is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data</param>
		/// <param name="rowIndex">The row index of the current cell, or -1 if the cell is not owned by a row.</param>
		// Token: 0x06001BA1 RID: 7073 RVA: 0x00089544 File Offset: 0x00087744
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
				this.NotifyMASSClient(new Point(base.ColumnIndex, rowIndex));
			}
		}

		/// <summary>Called when the focus moves from a cell.</summary>
		/// <param name="rowIndex">The row index of the current cell, or -1 if the cell is not owned by a row.</param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if the cell was left as a result of user mouse click rather than a programmatic cell change; otherwise, <see langword="false" />.</param>
		// Token: 0x06001BA2 RID: 7074 RVA: 0x00089602 File Offset: 0x00087802
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

		/// <summary>Called when the mouse button is held down while the pointer is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06001BA3 RID: 7075 RVA: 0x0008961D File Offset: 0x0008781D
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left && DataGridViewCheckBoxCell.mouseInContentBounds)
			{
				this.UpdateButtonState(this.ButtonState | ButtonState.Pushed, e.RowIndex);
			}
		}

		/// <summary>Called when the mouse pointer moves from a cell.</summary>
		/// <param name="rowIndex">The row index of the current cell or -1 if the cell is not owned by a row.</param>
		// Token: 0x06001BA4 RID: 7076 RVA: 0x00089654 File Offset: 0x00087854
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (DataGridViewCheckBoxCell.mouseInContentBounds)
			{
				DataGridViewCheckBoxCell.mouseInContentBounds = false;
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

		/// <summary>Called when the mouse pointer moves within a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06001BA5 RID: 7077 RVA: 0x00089708 File Offset: 0x00087908
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			bool flag = DataGridViewCheckBoxCell.mouseInContentBounds;
			DataGridViewCheckBoxCell.mouseInContentBounds = base.GetContentBounds(e.RowIndex).Contains(e.X, e.Y);
			if (flag != DataGridViewCheckBoxCell.mouseInContentBounds)
			{
				if (base.DataGridView.ApplyVisualStylesToInnerCells || this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup)
				{
					base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
				}
				if (e.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && e.RowIndex == base.DataGridView.MouseDownCellAddress.Y && Control.MouseButtons == MouseButtons.Left)
				{
					if ((this.ButtonState & ButtonState.Pushed) == ButtonState.Normal && DataGridViewCheckBoxCell.mouseInContentBounds && base.DataGridView.CellMouseDownInContentBounds)
					{
						this.UpdateButtonState(this.ButtonState | ButtonState.Pushed, e.RowIndex);
					}
					else if ((this.ButtonState & ButtonState.Pushed) != ButtonState.Normal && !DataGridViewCheckBoxCell.mouseInContentBounds)
					{
						this.UpdateButtonState(this.ButtonState & ~ButtonState.Pushed, e.RowIndex);
					}
				}
			}
			base.OnMouseMove(e);
		}

		/// <summary>Called when the mouse button is released while the pointer is on a cell. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06001BA6 RID: 7078 RVA: 0x00089840 File Offset: 0x00087A40
		protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left)
			{
				this.UpdateButtonState(this.ButtonState & ~ButtonState.Pushed, e.RowIndex);
				this.NotifyMASSClient(new Point(e.ColumnIndex, e.RowIndex));
			}
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x00089894 File Offset: 0x00087A94
		private void NotifyMASSClient(Point position)
		{
			int rowCount = base.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, position.Y);
			int num = base.DataGridView.Columns.ColumnIndexToActualDisplayIndex(position.X, DataGridViewElementStates.Visible);
			int num2 = base.DataGridView.ColumnHeadersVisible ? 1 : 0;
			int num3 = base.DataGridView.RowHeadersVisible ? 1 : 0;
			int objectID = rowCount + num2 + 1;
			int childID = num + num3;
			(base.DataGridView.AccessibilityObject as Control.ControlAccessibleObject).NotifyClients(AccessibleEvents.StateChange, objectID, childID);
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" />.</summary>
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
		// Token: 0x06001BA8 RID: 7080 RVA: 0x00089924 File Offset: 0x00087B24
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, elementState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0008995C File Offset: 0x00087B5C
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle = cellBounds;
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			rectangle.Offset(rectangle2.X, rectangle2.Y);
			rectangle.Width -= rectangle2.Right;
			rectangle.Height -= rectangle2.Bottom;
			bool flag = (elementState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
			bool flag2 = false;
			bool flag3 = true;
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex && base.DataGridView.IsCurrentCellInEditMode)
			{
				flag3 = false;
			}
			CheckState checkState;
			ButtonState buttonState;
			if (formattedValue != null && formattedValue is CheckState)
			{
				checkState = (CheckState)formattedValue;
				buttonState = ((checkState == CheckState.Unchecked) ? ButtonState.Normal : ButtonState.Checked);
				flag2 = (checkState == CheckState.Indeterminate);
			}
			else if (formattedValue != null && formattedValue is bool)
			{
				if ((bool)formattedValue)
				{
					checkState = CheckState.Checked;
					buttonState = ButtonState.Checked;
				}
				else
				{
					checkState = CheckState.Unchecked;
					buttonState = ButtonState.Normal;
				}
			}
			else
			{
				buttonState = ButtonState.Normal;
				checkState = CheckState.Unchecked;
			}
			if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
			{
				buttonState |= ButtonState.Pushed;
			}
			SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
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
			if (paint && DataGridViewCell.PaintFocus(paintParts) && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle, Color.Empty, cachedBrush.Color);
			}
			Rectangle cellValueBounds = rectangle;
			rectangle.Inflate(-2, -2);
			CheckBoxState state = CheckBoxState.UncheckedNormal;
			Size glyphSize;
			if (base.DataGridView.ApplyVisualStylesToInnerCells)
			{
				state = CheckBoxRenderer.ConvertFromButtonState(buttonState, flag2, base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewCheckBoxCell.mouseInContentBounds);
				glyphSize = CheckBoxRenderer.GetGlyphSize(g, state);
				switch (this.FlatStyle)
				{
				case FlatStyle.Flat:
					glyphSize.Width -= 3;
					glyphSize.Height -= 3;
					break;
				case FlatStyle.Popup:
					glyphSize.Width -= 2;
					glyphSize.Height -= 2;
					break;
				}
			}
			else
			{
				FlatStyle flatStyle = this.FlatStyle;
				if (flatStyle != FlatStyle.Flat)
				{
					if (flatStyle != FlatStyle.Popup)
					{
						glyphSize = new Size(SystemInformation.Border3DSize.Width * 2 + 9, SystemInformation.Border3DSize.Width * 2 + 9);
					}
					else
					{
						glyphSize = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
						glyphSize.Width -= 2;
						glyphSize.Height -= 2;
					}
				}
				else
				{
					glyphSize = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
					glyphSize.Width -= 3;
					glyphSize.Height -= 3;
				}
			}
			Rectangle result;
			if (rectangle.Width >= glyphSize.Width && rectangle.Height >= glyphSize.Height && (paint || computeContentBounds))
			{
				int num = 0;
				int num2 = 0;
				if ((!base.DataGridView.RightToLeftInternal && (cellStyle.Alignment & DataGridViewCheckBoxCell.anyRight) != DataGridViewContentAlignment.NotSet) || (base.DataGridView.RightToLeftInternal && (cellStyle.Alignment & DataGridViewCheckBoxCell.anyLeft) != DataGridViewContentAlignment.NotSet))
				{
					num = rectangle.Right - glyphSize.Width;
				}
				else if ((cellStyle.Alignment & DataGridViewCheckBoxCell.anyCenter) != DataGridViewContentAlignment.NotSet)
				{
					num = rectangle.Left + (rectangle.Width - glyphSize.Width) / 2;
				}
				else
				{
					num = rectangle.Left;
				}
				if ((cellStyle.Alignment & DataGridViewCheckBoxCell.anyBottom) != DataGridViewContentAlignment.NotSet)
				{
					num2 = rectangle.Bottom - glyphSize.Height;
				}
				else if ((cellStyle.Alignment & DataGridViewCheckBoxCell.anyMiddle) != DataGridViewContentAlignment.NotSet)
				{
					num2 = rectangle.Top + (rectangle.Height - glyphSize.Height) / 2;
				}
				else
				{
					num2 = rectangle.Top;
				}
				if (base.DataGridView.ApplyVisualStylesToInnerCells && this.FlatStyle != FlatStyle.Flat && this.FlatStyle != FlatStyle.Popup)
				{
					if (paint && DataGridViewCell.PaintContentForeground(paintParts))
					{
						DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.DrawCheckBox(g, new Rectangle(num, num2, glyphSize.Width, glyphSize.Height), (int)state);
					}
					result = new Rectangle(num, num2, glyphSize.Width, glyphSize.Height);
				}
				else if (this.FlatStyle == FlatStyle.System || this.FlatStyle == FlatStyle.Standard)
				{
					if (paint && DataGridViewCell.PaintContentForeground(paintParts))
					{
						if (flag2)
						{
							ControlPaint.DrawMixedCheckBox(g, num, num2, glyphSize.Width, glyphSize.Height, buttonState);
						}
						else
						{
							ControlPaint.DrawCheckBox(g, num, num2, glyphSize.Width, glyphSize.Height, buttonState);
						}
					}
					result = new Rectangle(num, num2, glyphSize.Width, glyphSize.Height);
				}
				else if (this.FlatStyle == FlatStyle.Flat)
				{
					Rectangle rectangle3 = new Rectangle(num, num2, glyphSize.Width, glyphSize.Height);
					SolidBrush solidBrush = null;
					SolidBrush solidBrush2 = null;
					Color color = Color.Empty;
					if (paint && DataGridViewCell.PaintContentForeground(paintParts))
					{
						solidBrush = base.DataGridView.GetCachedBrush(flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
						solidBrush2 = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
						color = ControlPaint.LightLight(solidBrush2.Color);
						if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewCheckBoxCell.mouseInContentBounds)
						{
							float percentage = 0.9f;
							if ((double)color.GetBrightness() < 0.5)
							{
								percentage = 1.2f;
							}
							color = Color.FromArgb(ButtonBaseAdapter.ColorOptions.Adjust255(percentage, (int)color.R), ButtonBaseAdapter.ColorOptions.Adjust255(percentage, (int)color.G), ButtonBaseAdapter.ColorOptions.Adjust255(percentage, (int)color.B));
						}
						color = g.GetNearestColor(color);
						using (Pen pen = new Pen(solidBrush.Color))
						{
							g.DrawLine(pen, rectangle3.Left, rectangle3.Top, rectangle3.Right - 1, rectangle3.Top);
							g.DrawLine(pen, rectangle3.Left, rectangle3.Top, rectangle3.Left, rectangle3.Bottom - 1);
						}
					}
					rectangle3.Inflate(-1, -1);
					int num3 = rectangle3.Width;
					rectangle3.Width = num3 + 1;
					num3 = rectangle3.Height;
					rectangle3.Height = num3 + 1;
					if (paint && DataGridViewCell.PaintContentForeground(paintParts))
					{
						if (checkState == CheckState.Indeterminate)
						{
							ButtonBaseAdapter.DrawDitheredFill(g, solidBrush2.Color, color, rectangle3);
						}
						else
						{
							using (SolidBrush solidBrush3 = new SolidBrush(color))
							{
								g.FillRectangle(solidBrush3, rectangle3);
							}
						}
						if (checkState != CheckState.Unchecked)
						{
							Rectangle destination = new Rectangle(num - 1, num2 - 1, glyphSize.Width + 3, glyphSize.Height + 3);
							num3 = destination.Width;
							destination.Width = num3 + 1;
							num3 = destination.Height;
							destination.Height = num3 + 1;
							if (DataGridViewCheckBoxCell.checkImage == null || DataGridViewCheckBoxCell.checkImage.Width != destination.Width || DataGridViewCheckBoxCell.checkImage.Height != destination.Height)
							{
								if (DataGridViewCheckBoxCell.checkImage != null)
								{
									DataGridViewCheckBoxCell.checkImage.Dispose();
									DataGridViewCheckBoxCell.checkImage = null;
								}
								NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(0, 0, destination.Width, destination.Height);
								Bitmap bitmap = new Bitmap(destination.Width, destination.Height);
								using (Graphics graphics = Graphics.FromImage(bitmap))
								{
									graphics.Clear(Color.Transparent);
									IntPtr hdc = graphics.GetHdc();
									try
									{
										SafeNativeMethods.DrawFrameControl(new HandleRef(graphics, hdc), ref rect, 2, 1);
									}
									finally
									{
										graphics.ReleaseHdcInternal(hdc);
									}
								}
								bitmap.MakeTransparent();
								DataGridViewCheckBoxCell.checkImage = bitmap;
							}
							num3 = destination.Y;
							destination.Y = num3 - 1;
							ControlPaint.DrawImageColorized(g, DataGridViewCheckBoxCell.checkImage, destination, (checkState == CheckState.Indeterminate) ? ControlPaint.LightLight(solidBrush.Color) : solidBrush.Color);
						}
					}
					result = rectangle3;
				}
				else
				{
					Rectangle clientRectangle = new Rectangle(num, num2, glyphSize.Width - 1, glyphSize.Height - 1);
					clientRectangle.Y -= 3;
					if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
					{
						ButtonBaseAdapter.LayoutOptions layoutOptions = CheckBoxPopupAdapter.PaintPopupLayout(g, true, glyphSize.Width, clientRectangle, Padding.Empty, false, cellStyle.Font, string.Empty, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft, null);
						layoutOptions.everettButtonCompat = false;
						ButtonBaseAdapter.LayoutData layoutData = layoutOptions.Layout();
						if (paint && DataGridViewCell.PaintContentForeground(paintParts))
						{
							ButtonBaseAdapter.ColorData colorData = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
							CheckBoxBaseAdapter.DrawCheckBackground(base.DataGridView.Enabled, checkState, g, layoutData.checkBounds, colorData.windowText, colorData.buttonFace, true, colorData);
							CheckBoxBaseAdapter.DrawPopupBorder(g, layoutData.checkBounds, colorData);
							CheckBoxBaseAdapter.DrawCheckOnly(glyphSize.Width, checkState == CheckState.Checked || checkState == CheckState.Indeterminate, base.DataGridView.Enabled, checkState, g, layoutData, colorData, colorData.windowText, colorData.buttonFace);
						}
						result = layoutData.checkBounds;
					}
					else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewCheckBoxCell.mouseInContentBounds)
					{
						ButtonBaseAdapter.LayoutOptions layoutOptions2 = CheckBoxPopupAdapter.PaintPopupLayout(g, true, glyphSize.Width, clientRectangle, Padding.Empty, false, cellStyle.Font, string.Empty, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft, null);
						layoutOptions2.everettButtonCompat = false;
						ButtonBaseAdapter.LayoutData layoutData2 = layoutOptions2.Layout();
						if (paint && DataGridViewCell.PaintContentForeground(paintParts))
						{
							ButtonBaseAdapter.ColorData colorData2 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
							CheckBoxBaseAdapter.DrawCheckBackground(base.DataGridView.Enabled, checkState, g, layoutData2.checkBounds, colorData2.windowText, colorData2.options.highContrast ? colorData2.buttonFace : colorData2.highlight, true, colorData2);
							CheckBoxBaseAdapter.DrawPopupBorder(g, layoutData2.checkBounds, colorData2);
							CheckBoxBaseAdapter.DrawCheckOnly(glyphSize.Width, checkState == CheckState.Checked || checkState == CheckState.Indeterminate, base.DataGridView.Enabled, checkState, g, layoutData2, colorData2, colorData2.windowText, colorData2.highlight);
						}
						result = layoutData2.checkBounds;
					}
					else
					{
						ButtonBaseAdapter.LayoutOptions layoutOptions3 = CheckBoxPopupAdapter.PaintPopupLayout(g, false, glyphSize.Width, clientRectangle, Padding.Empty, false, cellStyle.Font, string.Empty, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft, null);
						layoutOptions3.everettButtonCompat = false;
						ButtonBaseAdapter.LayoutData layoutData3 = layoutOptions3.Layout();
						if (paint && DataGridViewCell.PaintContentForeground(paintParts))
						{
							ButtonBaseAdapter.ColorData colorData3 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
							CheckBoxBaseAdapter.DrawCheckBackground(base.DataGridView.Enabled, checkState, g, layoutData3.checkBounds, colorData3.windowText, colorData3.options.highContrast ? colorData3.buttonFace : colorData3.highlight, true, colorData3);
							ButtonBaseAdapter.DrawFlatBorder(g, layoutData3.checkBounds, colorData3.buttonShadow);
							CheckBoxBaseAdapter.DrawCheckOnly(glyphSize.Width, checkState == CheckState.Checked || checkState == CheckState.Indeterminate, base.DataGridView.Enabled, checkState, g, layoutData3, colorData3, colorData3.windowText, colorData3.highlight);
						}
						result = layoutData3.checkBounds;
					}
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
			if (paint && DataGridViewCell.PaintErrorIcon(paintParts) && flag3 && base.DataGridView.ShowCellErrors)
			{
				base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, cellValueBounds, errorText);
			}
			return result;
		}

		/// <summary>Converts a value formatted for display to an actual cell value.</summary>
		/// <param name="formattedValue">The display value of the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> in effect for the cell.</param>
		/// <param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> for the display value type, or <see langword="null" /> to use the default converter.</param>
		/// <param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> for the cell value type, or <see langword="null" /> to use the default converter.</param>
		/// <returns>The cell value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="cellStyle" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The <see cref="P:System.Windows.Forms.DataGridViewCell.FormattedValueType" /> property value is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="formattedValue" /> is <see langword="null" />.- or -The type of <paramref name="formattedValue" /> does not match the type indicated by the <see cref="P:System.Windows.Forms.DataGridViewCell.FormattedValueType" /> property. </exception>
		// Token: 0x06001BAA RID: 7082 RVA: 0x0008A71C File Offset: 0x0008891C
		public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
		{
			if (formattedValue != null)
			{
				if (formattedValue is bool)
				{
					if ((bool)formattedValue)
					{
						if (this.TrueValue != null)
						{
							return this.TrueValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
						{
							return true;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Checked;
						}
					}
					else
					{
						if (this.FalseValue != null)
						{
							return this.FalseValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
						{
							return false;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Unchecked;
						}
					}
				}
				else if (formattedValue is CheckState)
				{
					switch ((CheckState)formattedValue)
					{
					case CheckState.Unchecked:
						if (this.FalseValue != null)
						{
							return this.FalseValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
						{
							return false;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Unchecked;
						}
						break;
					case CheckState.Checked:
						if (this.TrueValue != null)
						{
							return this.TrueValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
						{
							return true;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Checked;
						}
						break;
					case CheckState.Indeterminate:
						if (this.IndeterminateValue != null)
						{
							return this.IndeterminateValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Indeterminate;
						}
						break;
					}
				}
			}
			return base.ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x0008A934 File Offset: 0x00088B34
		private bool SwitchFormattedValue()
		{
			if (this.FormattedValueType == null)
			{
				return false;
			}
			if (this.FormattedValueType.IsAssignableFrom(typeof(CheckState)))
			{
				if ((this.flags & 16) != 0)
				{
					((IDataGridViewEditingCell)this).EditingCellFormattedValue = CheckState.Indeterminate;
				}
				else if ((this.flags & 32) != 0)
				{
					((IDataGridViewEditingCell)this).EditingCellFormattedValue = CheckState.Unchecked;
				}
				else
				{
					((IDataGridViewEditingCell)this).EditingCellFormattedValue = CheckState.Checked;
				}
			}
			else if (this.FormattedValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
			{
				((IDataGridViewEditingCell)this).EditingCellFormattedValue = !(bool)((IDataGridViewEditingCell)this).GetEditingCellFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			return true;
		}

		/// <summary>Returns the string representation of the cell.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the current cell.</returns>
		// Token: 0x06001BAC RID: 7084 RVA: 0x0008A9D8 File Offset: 0x00088BD8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewCheckBoxCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x0008AA34 File Offset: 0x00088C34
		private void UpdateButtonState(ButtonState newButtonState, int rowIndex)
		{
			this.ButtonState = newButtonState;
			base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
		}

		// Token: 0x04000C56 RID: 3158
		private static readonly DataGridViewContentAlignment anyLeft = (DataGridViewContentAlignment)273;

		// Token: 0x04000C57 RID: 3159
		private static readonly DataGridViewContentAlignment anyRight = (DataGridViewContentAlignment)1092;

		// Token: 0x04000C58 RID: 3160
		private static readonly DataGridViewContentAlignment anyCenter = (DataGridViewContentAlignment)546;

		// Token: 0x04000C59 RID: 3161
		private static readonly DataGridViewContentAlignment anyBottom = (DataGridViewContentAlignment)1792;

		// Token: 0x04000C5A RID: 3162
		private static readonly DataGridViewContentAlignment anyMiddle = (DataGridViewContentAlignment)112;

		// Token: 0x04000C5B RID: 3163
		private static readonly VisualStyleElement CheckBoxElement = VisualStyleElement.Button.CheckBox.UncheckedNormal;

		// Token: 0x04000C5C RID: 3164
		private static readonly int PropButtonCellState = PropertyStore.CreateKey();

		// Token: 0x04000C5D RID: 3165
		private static readonly int PropTrueValue = PropertyStore.CreateKey();

		// Token: 0x04000C5E RID: 3166
		private static readonly int PropFalseValue = PropertyStore.CreateKey();

		// Token: 0x04000C5F RID: 3167
		private static readonly int PropFlatStyle = PropertyStore.CreateKey();

		// Token: 0x04000C60 RID: 3168
		private static readonly int PropIndeterminateValue = PropertyStore.CreateKey();

		// Token: 0x04000C61 RID: 3169
		private static Bitmap checkImage = null;

		// Token: 0x04000C62 RID: 3170
		private const byte DATAGRIDVIEWCHECKBOXCELL_threeState = 1;

		// Token: 0x04000C63 RID: 3171
		private const byte DATAGRIDVIEWCHECKBOXCELL_valueChanged = 2;

		// Token: 0x04000C64 RID: 3172
		private const byte DATAGRIDVIEWCHECKBOXCELL_checked = 16;

		// Token: 0x04000C65 RID: 3173
		private const byte DATAGRIDVIEWCHECKBOXCELL_indeterminate = 32;

		// Token: 0x04000C66 RID: 3174
		private const byte DATAGRIDVIEWCHECKBOXCELL_margin = 2;

		// Token: 0x04000C67 RID: 3175
		private byte flags;

		// Token: 0x04000C68 RID: 3176
		private static bool mouseInContentBounds = false;

		// Token: 0x04000C69 RID: 3177
		private static Type defaultCheckStateType = typeof(CheckState);

		// Token: 0x04000C6A RID: 3178
		private static Type defaultBooleanType = typeof(bool);

		// Token: 0x04000C6B RID: 3179
		private static Type cellType = typeof(DataGridViewCheckBoxCell);

		// Token: 0x020005AD RID: 1453
		private class DataGridViewCheckBoxCellRenderer
		{
			// Token: 0x0600593A RID: 22842 RVA: 0x000027DB File Offset: 0x000009DB
			private DataGridViewCheckBoxCellRenderer()
			{
			}

			// Token: 0x1700158B RID: 5515
			// (get) Token: 0x0600593B RID: 22843 RVA: 0x00177ED4 File Offset: 0x001760D4
			public static VisualStyleRenderer CheckBoxRenderer
			{
				get
				{
					if (DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewCheckBoxCell.CheckBoxElement);
					}
					return DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x0600593C RID: 22844 RVA: 0x00177EF1 File Offset: 0x001760F1
			public static void DrawCheckBox(Graphics g, Rectangle bounds, int state)
			{
				DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.CheckBoxRenderer.SetParameters(DataGridViewCheckBoxCell.CheckBoxElement.ClassName, DataGridViewCheckBoxCell.CheckBoxElement.Part, state);
				DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.CheckBoxRenderer.DrawBackground(g, bounds, Rectangle.Truncate(g.ClipBounds));
			}

			// Token: 0x0400392D RID: 14637
			private static VisualStyleRenderer visualStyleRenderer;
		}

		/// <summary>Provides information about a <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" /> to accessibility client applications.</summary>
		// Token: 0x020005AE RID: 1454
		protected class DataGridViewCheckBoxCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell.DataGridViewCheckBoxCellAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell.DataGridViewCheckBoxCellAccessibleObject" />.</param>
			// Token: 0x0600593D RID: 22845 RVA: 0x00176D3A File Offset: 0x00174F3A
			public DataGridViewCheckBoxCellAccessibleObject(DataGridViewCell owner) : base(owner)
			{
			}

			/// <summary>Gets the state of this <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell.DataGridViewCheckBoxCellAccessibleObject" />.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleStates" /> values.</returns>
			// Token: 0x1700158C RID: 5516
			// (get) Token: 0x0600593E RID: 22846 RVA: 0x00177F2C File Offset: 0x0017612C
			public override AccessibleStates State
			{
				get
				{
					if (((DataGridViewCheckBoxCell)base.Owner).EditedFormattedValue is CheckState)
					{
						CheckState checkState = (CheckState)((DataGridViewCheckBoxCell)base.Owner).EditedFormattedValue;
						if (checkState == CheckState.Checked)
						{
							return AccessibleStates.Checked | base.State;
						}
						if (checkState == CheckState.Indeterminate)
						{
							return AccessibleStates.Mixed | base.State;
						}
					}
					else if (((DataGridViewCheckBoxCell)base.Owner).EditedFormattedValue is bool)
					{
						bool flag = (bool)((DataGridViewCheckBoxCell)base.Owner).EditedFormattedValue;
						if (flag)
						{
							return AccessibleStates.Checked | base.State;
						}
					}
					return base.State;
				}
			}

			/// <summary>Gets a string that represents the default action of the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell.DataGridViewCheckBoxCellAccessibleObject" />.</summary>
			/// <returns>A description of the default action.</returns>
			// Token: 0x1700158D RID: 5517
			// (get) Token: 0x0600593F RID: 22847 RVA: 0x00177FC4 File Offset: 0x001761C4
			public override string DefaultAction
			{
				get
				{
					if (base.Owner.ReadOnly)
					{
						return string.Empty;
					}
					bool flag = true;
					object formattedValue = base.Owner.FormattedValue;
					if (formattedValue is CheckState)
					{
						flag = ((CheckState)formattedValue == CheckState.Unchecked);
					}
					else if (formattedValue is bool)
					{
						flag = !(bool)formattedValue;
					}
					if (flag)
					{
						return SR.GetString("DataGridView_AccCheckBoxCellDefaultActionCheck");
					}
					return SR.GetString("DataGridView_AccCheckBoxCellDefaultActionUncheck");
				}
			}

			/// <summary>Performs the default action of the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell.DataGridViewCheckBoxCellAccessibleObject" />.</summary>
			/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" /> returned by the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property does not belong to a <see langword="DataGridView" /> control.-or-The <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" /> returned by the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property belongs to a shared row.</exception>
			// Token: 0x06005940 RID: 22848 RVA: 0x00178030 File Offset: 0x00176230
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)base.Owner;
				DataGridView dataGridView = dataGridViewCheckBoxCell.DataGridView;
				if (dataGridView != null && dataGridViewCheckBoxCell.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				if (!dataGridViewCheckBoxCell.ReadOnly && dataGridViewCheckBoxCell.OwningColumn != null && dataGridViewCheckBoxCell.OwningRow != null)
				{
					dataGridView.CurrentCell = dataGridViewCheckBoxCell;
					bool flag = false;
					if (!dataGridView.IsCurrentCellInEditMode)
					{
						flag = true;
						dataGridView.BeginEdit(false);
					}
					if (dataGridView.IsCurrentCellInEditMode)
					{
						if (dataGridViewCheckBoxCell.SwitchFormattedValue())
						{
							dataGridViewCheckBoxCell.NotifyDataGridViewOfValueChange();
							dataGridView.InvalidateCell(dataGridViewCheckBoxCell.ColumnIndex, dataGridViewCheckBoxCell.RowIndex);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell2 = base.Owner as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell2 != null)
							{
								dataGridViewCheckBoxCell2.NotifyMASSClient(new Point(dataGridViewCheckBoxCell.ColumnIndex, dataGridViewCheckBoxCell.RowIndex));
							}
						}
						if (flag)
						{
							dataGridView.EndEdit();
						}
					}
				}
			}

			/// <summary>Gets the number of child accessible objects that belong to the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell.DataGridViewCheckBoxCellAccessibleObject" />.</summary>
			/// <returns>The value –1.</returns>
			// Token: 0x06005941 RID: 22849 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public override int GetChildCount()
			{
				return 0;
			}

			// Token: 0x06005942 RID: 22850 RVA: 0x001780FA File Offset: 0x001762FA
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level1;
			}

			// Token: 0x1700158E RID: 5518
			// (get) Token: 0x06005943 RID: 22851 RVA: 0x00178106 File Offset: 0x00176306
			internal override int[] RuntimeId
			{
				get
				{
					if (this.runtimeId == null)
					{
						this.runtimeId = new int[2];
						this.runtimeId[0] = 42;
						this.runtimeId[1] = this.GetHashCode();
					}
					return this.runtimeId;
				}
			}

			// Token: 0x06005944 RID: 22852 RVA: 0x0017813A File Offset: 0x0017633A
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30041)
				{
					return this.IsPatternSupported(10015);
				}
				if (propertyID == 30003 && AccessibilityImprovements.Level2)
				{
					return 50002;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06005945 RID: 22853 RVA: 0x00178176 File Offset: 0x00176376
			internal override bool IsPatternSupported(int patternId)
			{
				return patternId == 10015 || base.IsPatternSupported(patternId);
			}

			// Token: 0x06005946 RID: 22854 RVA: 0x0000E217 File Offset: 0x0000C417
			internal override void Toggle()
			{
				this.DoDefaultAction();
			}

			// Token: 0x1700158F RID: 5519
			// (get) Token: 0x06005947 RID: 22855 RVA: 0x0017818C File Offset: 0x0017638C
			internal override UnsafeNativeMethods.ToggleState ToggleState
			{
				get
				{
					object formattedValue = base.Owner.FormattedValue;
					bool flag;
					if (formattedValue is CheckState)
					{
						flag = ((CheckState)formattedValue == CheckState.Checked);
					}
					else
					{
						if (!(formattedValue is bool))
						{
							return UnsafeNativeMethods.ToggleState.ToggleState_Indeterminate;
						}
						flag = (bool)formattedValue;
					}
					if (!flag)
					{
						return UnsafeNativeMethods.ToggleState.ToggleState_Off;
					}
					return UnsafeNativeMethods.ToggleState.ToggleState_On;
				}
			}

			// Token: 0x0400392E RID: 14638
			private int[] runtimeId;
		}
	}
}
