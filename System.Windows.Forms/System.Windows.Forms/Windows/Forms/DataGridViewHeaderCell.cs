using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Contains functionality common to row header cells and column header cells.</summary>
	// Token: 0x020001EB RID: 491
	public class DataGridViewHeaderCell : DataGridViewCell
	{
		/// <summary>Gets the buttonlike visual state of the header cell.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ButtonState" /> values; the default is <see cref="F:System.Windows.Forms.ButtonState.Normal" />.</returns>
		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001DC4 RID: 7620 RVA: 0x00093788 File Offset: 0x00091988
		protected ButtonState ButtonState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewHeaderCell.PropButtonState, out flag);
				if (flag)
				{
					return (ButtonState)integer;
				}
				return ButtonState.Normal;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (set) Token: 0x06001DC5 RID: 7621 RVA: 0x000937AE File Offset: 0x000919AE
		private ButtonState ButtonStatePrivate
		{
			set
			{
				if (this.ButtonState != value)
				{
					base.Properties.SetInteger(DataGridViewHeaderCell.PropButtonState, (int)value);
				}
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.DataGridViewHeaderCell" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06001DC6 RID: 7622 RVA: 0x000937CA File Offset: 0x000919CA
		protected override void Dispose(bool disposing)
		{
			if (this.FlipXPThemesBitmap != null && disposing)
			{
				this.FlipXPThemesBitmap.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>Gets a value that indicates whether the cell is currently displayed on-screen.</summary>
		/// <returns>
		///     <see langword="true" /> if the cell is on-screen or partially on-screen; otherwise, <see langword="false" />.</returns>
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x000937EC File Offset: 0x000919EC
		[Browsable(false)]
		public override bool Displayed
		{
			get
			{
				if (base.DataGridView == null || !base.DataGridView.Visible)
				{
					return false;
				}
				if (base.OwningRow != null)
				{
					return base.DataGridView.RowHeadersVisible && base.OwningRow.Displayed;
				}
				if (base.OwningColumn != null)
				{
					return base.DataGridView.ColumnHeadersVisible && base.OwningColumn.Displayed;
				}
				return base.DataGridView.LayoutInfo.TopLeftHeader != Rectangle.Empty;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x00093870 File Offset: 0x00091A70
		// (set) Token: 0x06001DC9 RID: 7625 RVA: 0x00093887 File Offset: 0x00091A87
		internal Bitmap FlipXPThemesBitmap
		{
			get
			{
				return (Bitmap)base.Properties.GetObject(DataGridViewHeaderCell.PropFlipXPThemesBitmap);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewHeaderCell.PropFlipXPThemesBitmap))
				{
					base.Properties.SetObject(DataGridViewHeaderCell.PropFlipXPThemesBitmap, value);
				}
			}
		}

		/// <summary>Gets the type of the formatted value of the cell.</summary>
		/// <returns>A <see cref="T:System.Type" /> object representing the <see cref="T:System.String" /> type.</returns>
		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x000938AF File Offset: 0x00091AAF
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewHeaderCell.defaultFormattedValueType;
			}
		}

		/// <summary>Gets a value indicating whether the cell is frozen. </summary>
		/// <returns>
		///     <see langword="true" /> if the cell is frozen; otherwise, <see langword="false" />. The default is <see langword="false" /> if the cell is detached from a <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001DCB RID: 7627 RVA: 0x000938B6 File Offset: 0x00091AB6
		[Browsable(false)]
		public override bool Frozen
		{
			get
			{
				if (base.OwningRow != null)
				{
					return base.OwningRow.Frozen;
				}
				if (base.OwningColumn != null)
				{
					return base.OwningColumn.Frozen;
				}
				return base.DataGridView != null;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001DCC RID: 7628 RVA: 0x000938EB File Offset: 0x00091AEB
		internal override bool HasValueType
		{
			get
			{
				return base.Properties.ContainsObject(DataGridViewHeaderCell.PropValueType) && base.Properties.GetObject(DataGridViewHeaderCell.PropValueType) != null;
			}
		}

		/// <summary>Gets a value indicating whether the header cell is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		/// <exception cref="T:System.InvalidOperationException">An operation tries to set this property.</exception>
		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001DCD RID: 7629 RVA: 0x0000E214 File Offset: 0x0000C414
		// (set) Token: 0x06001DCE RID: 7630 RVA: 0x00093914 File Offset: 0x00091B14
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool ReadOnly
		{
			get
			{
				return true;
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_HeaderCellReadOnlyProperty", new object[]
				{
					"ReadOnly"
				}));
			}
		}

		/// <summary>Gets a value indicating whether the cell is resizable.</summary>
		/// <returns>
		///     <see langword="true" /> if this cell can be resized; otherwise, <see langword="false" />. The default is <see langword="false" /> if the cell is not attached to a <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001DCF RID: 7631 RVA: 0x00093934 File Offset: 0x00091B34
		[Browsable(false)]
		public override bool Resizable
		{
			get
			{
				if (base.OwningRow != null)
				{
					return base.OwningRow.Resizable == DataGridViewTriState.True || (base.DataGridView != null && base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.EnableResizing);
				}
				if (base.OwningColumn != null)
				{
					return base.OwningColumn.Resizable == DataGridViewTriState.True || (base.DataGridView != null && base.DataGridView.ColumnHeadersHeightSizeMode == DataGridViewColumnHeadersHeightSizeMode.EnableResizing);
				}
				return base.DataGridView != null && (base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.EnableResizing || base.DataGridView.ColumnHeadersHeightSizeMode == DataGridViewColumnHeadersHeightSizeMode.EnableResizing);
			}
		}

		/// <summary>Gets or sets a value indicating whether the cell is selected.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		/// <exception cref="T:System.InvalidOperationException">This property is being set.</exception>
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001DD0 RID: 7632 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x06001DD1 RID: 7633 RVA: 0x000939CA File Offset: 0x00091BCA
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Selected
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_HeaderCellReadOnlyProperty", new object[]
				{
					"Selected"
				}));
			}
		}

		/// <summary>Gets the type of the value stored in the cell.</summary>
		/// <returns>A <see cref="T:System.Type" /> object representing the <see cref="T:System.String" /> type.</returns>
		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x000939EC File Offset: 0x00091BEC
		// (set) Token: 0x06001DD3 RID: 7635 RVA: 0x00093A1F File Offset: 0x00091C1F
		public override Type ValueType
		{
			get
			{
				Type type = (Type)base.Properties.GetObject(DataGridViewHeaderCell.PropValueType);
				if (type != null)
				{
					return type;
				}
				return DataGridViewHeaderCell.defaultValueType;
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewHeaderCell.PropValueType))
				{
					base.Properties.SetObject(DataGridViewHeaderCell.PropValueType, value);
				}
			}
		}

		/// <summary>Gets a value indicating whether or not the cell is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the cell is visible; otherwise, <see langword="false" />. The default is <see langword="false" /> if the cell is detached from a <see cref="T:System.Windows.Forms.DataGridView" /></returns>
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x00093A50 File Offset: 0x00091C50
		[Browsable(false)]
		public override bool Visible
		{
			get
			{
				if (base.OwningRow != null)
				{
					return base.OwningRow.Visible && (base.DataGridView == null || base.DataGridView.RowHeadersVisible);
				}
				if (base.OwningColumn != null)
				{
					return base.OwningColumn.Visible && (base.DataGridView == null || base.DataGridView.ColumnHeadersVisible);
				}
				return base.DataGridView != null && base.DataGridView.RowHeadersVisible && base.DataGridView.ColumnHeadersVisible;
			}
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewHeaderCell" />.</returns>
		// Token: 0x06001DD5 RID: 7637 RVA: 0x00093ADC File Offset: 0x00091CDC
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewHeaderCell dataGridViewHeaderCell;
			if (type == DataGridViewHeaderCell.cellType)
			{
				dataGridViewHeaderCell = new DataGridViewHeaderCell();
			}
			else
			{
				dataGridViewHeaderCell = (DataGridViewHeaderCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewHeaderCell);
			dataGridViewHeaderCell.Value = base.Value;
			return dataGridViewHeaderCell;
		}

		/// <summary>Gets the shortcut menu of the header cell.</summary>
		/// <param name="rowIndex">Ignored by this implementation.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.ContextMenuStrip" /> if the <see cref="T:System.Windows.Forms.DataGridViewHeaderCell" /> or <see cref="T:System.Windows.Forms.DataGridView" /> has a shortcut menu assigned; otherwise, <see langword="null" />.</returns>
		// Token: 0x06001DD6 RID: 7638 RVA: 0x00093B28 File Offset: 0x00091D28
		public override ContextMenuStrip GetInheritedContextMenuStrip(int rowIndex)
		{
			ContextMenuStrip contextMenuStrip = base.GetContextMenuStrip(rowIndex);
			if (contextMenuStrip != null)
			{
				return contextMenuStrip;
			}
			if (base.DataGridView != null)
			{
				return base.DataGridView.ContextMenuStrip;
			}
			return null;
		}

		/// <summary>Returns a value indicating the current state of the cell as inherited from the state of its row or column.</summary>
		/// <param name="rowIndex">The index of the row containing the cell or -1 if the cell is not a row header cell or is not contained within a <see cref="T:System.Windows.Forms.DataGridView" /> control.</param>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values representing the current state of the cell.</returns>
		/// <exception cref="T:System.ArgumentException">The cell is a row header cell, the cell is not contained within a <see cref="T:System.Windows.Forms.DataGridView" /> control, and <paramref name="rowIndex" /> is not -1.- or -The cell is a row header cell, the cell is contained within a <see cref="T:System.Windows.Forms.DataGridView" /> control, and <paramref name="rowIndex" /> is outside the valid range of 0 to the number of rows in the control minus 1.- or -The cell is a row header cell and <paramref name="rowIndex" /> is not the index of the row containing this cell.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The cell is a column header cell or the control's <see cref="P:System.Windows.Forms.DataGridView.TopLeftHeaderCell" />  and <paramref name="rowIndex" /> is not -1.</exception>
		// Token: 0x06001DD7 RID: 7639 RVA: 0x00093B58 File Offset: 0x00091D58
		public override DataGridViewElementStates GetInheritedState(int rowIndex)
		{
			DataGridViewElementStates dataGridViewElementStates = DataGridViewElementStates.ReadOnly | DataGridViewElementStates.ResizableSet;
			if (base.OwningRow != null)
			{
				if ((base.DataGridView == null && rowIndex != -1) || (base.DataGridView != null && (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)))
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (base.DataGridView != null && base.DataGridView.Rows.SharedRow(rowIndex) != base.OwningRow)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				dataGridViewElementStates |= (base.OwningRow.GetState(rowIndex) & DataGridViewElementStates.Frozen);
				if (base.OwningRow.GetResizable(rowIndex) == DataGridViewTriState.True || (base.DataGridView != null && base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.EnableResizing))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Resizable;
				}
				if (base.OwningRow.GetVisible(rowIndex) && (base.DataGridView == null || base.DataGridView.RowHeadersVisible))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Visible;
					if (base.OwningRow.GetDisplayed(rowIndex))
					{
						dataGridViewElementStates |= DataGridViewElementStates.Displayed;
					}
				}
			}
			else if (base.OwningColumn != null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				dataGridViewElementStates |= (base.OwningColumn.State & DataGridViewElementStates.Frozen);
				if (base.OwningColumn.Resizable == DataGridViewTriState.True || (base.DataGridView != null && base.DataGridView.ColumnHeadersHeightSizeMode == DataGridViewColumnHeadersHeightSizeMode.EnableResizing))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Resizable;
				}
				if (base.OwningColumn.Visible && (base.DataGridView == null || base.DataGridView.ColumnHeadersVisible))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Visible;
					if (base.OwningColumn.Displayed)
					{
						dataGridViewElementStates |= DataGridViewElementStates.Displayed;
					}
				}
			}
			else if (base.DataGridView != null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				dataGridViewElementStates |= DataGridViewElementStates.Frozen;
				if (base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.EnableResizing || base.DataGridView.ColumnHeadersHeightSizeMode == DataGridViewColumnHeadersHeightSizeMode.EnableResizing)
				{
					dataGridViewElementStates |= DataGridViewElementStates.Resizable;
				}
				if (base.DataGridView.RowHeadersVisible && base.DataGridView.ColumnHeadersVisible)
				{
					dataGridViewElementStates |= DataGridViewElementStates.Visible;
					if (base.DataGridView.LayoutInfo.TopLeftHeader != Rectangle.Empty)
					{
						dataGridViewElementStates |= DataGridViewElementStates.Displayed;
					}
				}
			}
			return dataGridViewElementStates;
		}

		/// <summary>Gets the size of the cell.</summary>
		/// <param name="rowIndex">The row index of the header cell.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the header cell.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property for this cell is <see langword="null" /> and <paramref name="rowIndex" /> does not equal -1. -or-The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.OwningColumn" /> property for this cell is not <see langword="null" /> and <paramref name="rowIndex" /> does not equal -1. -or-The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.OwningRow" /> property for this cell is not <see langword="null" /> and <paramref name="rowIndex" /> is less than zero or greater than or equal to the number of rows in the control.-or-The values of the <see cref="P:System.Windows.Forms.DataGridViewCell.OwningColumn" /> and <see cref="P:System.Windows.Forms.DataGridViewCell.OwningRow" /> properties of this cell are both <see langword="null" /> and <paramref name="rowIndex" /> does not equal -1.</exception>
		/// <exception cref="T:System.ArgumentException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.OwningRow" /> property for this cell is not <see langword="null" /> and <paramref name="rowIndex" /> indicates a row other than the <see cref="P:System.Windows.Forms.DataGridViewCell.OwningRow" />.</exception>
		// Token: 0x06001DD8 RID: 7640 RVA: 0x00093DA4 File Offset: 0x00091FA4
		protected override Size GetSize(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				return new Size(-1, -1);
			}
			else if (base.OwningColumn != null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				return new Size(base.OwningColumn.Thickness, base.DataGridView.ColumnHeadersHeight);
			}
			else if (base.OwningRow != null)
			{
				if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (base.DataGridView.Rows.SharedRow(rowIndex) != base.OwningRow)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				return new Size(base.DataGridView.RowHeadersWidth, base.OwningRow.GetHeight(rowIndex));
			}
			else
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				return new Size(base.DataGridView.RowHeadersWidth, base.DataGridView.ColumnHeadersHeight);
			}
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x00093EC0 File Offset: 0x000920C0
		internal static Rectangle GetThemeMargins(Graphics g)
		{
			if (DataGridViewHeaderCell.rectThemeMargins.X == -1)
			{
				Rectangle bounds = new Rectangle(0, 0, 100, 100);
				Rectangle backgroundContentRectangle = DataGridViewHeaderCell.DataGridViewHeaderCellRenderer.VisualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
				DataGridViewHeaderCell.rectThemeMargins.X = backgroundContentRectangle.X;
				DataGridViewHeaderCell.rectThemeMargins.Y = backgroundContentRectangle.Y;
				DataGridViewHeaderCell.rectThemeMargins.Width = 100 - backgroundContentRectangle.Right;
				DataGridViewHeaderCell.rectThemeMargins.Height = 100 - backgroundContentRectangle.Bottom;
				if (DataGridViewHeaderCell.rectThemeMargins.X == 3 && DataGridViewHeaderCell.rectThemeMargins.Y + DataGridViewHeaderCell.rectThemeMargins.Width + DataGridViewHeaderCell.rectThemeMargins.Height == 0)
				{
					DataGridViewHeaderCell.rectThemeMargins = new Rectangle(0, 0, 2, 3);
				}
				else
				{
					try
					{
						string fileName = Path.GetFileName(VisualStyleInformation.ThemeFilename);
						if (string.Equals(fileName, "Aero.msstyles", StringComparison.OrdinalIgnoreCase))
						{
							DataGridViewHeaderCell.rectThemeMargins = new Rectangle(2, 1, 0, 2);
						}
					}
					catch
					{
					}
				}
			}
			return DataGridViewHeaderCell.rectThemeMargins;
		}

		/// <summary>Gets the value of the cell. </summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The value of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is not -1.</exception>
		// Token: 0x06001DDA RID: 7642 RVA: 0x00093FC4 File Offset: 0x000921C4
		protected override object GetValue(int rowIndex)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			return base.Properties.GetObject(DataGridViewCell.PropCellValue);
		}

		/// <summary>Indicates whether a row will be unshared when the mouse button is held down while the pointer is on a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains information about the mouse position.</param>
		/// <returns>
		///     <see langword="true" /> if the user clicks with the left mouse button, visual styles are enabled, and the <see cref="P:System.Windows.Forms.DataGridView.EnableHeadersVisualStyles" /> property is <see langword="true" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001DDB RID: 7643 RVA: 0x00093FE5 File Offset: 0x000921E5
		protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left && base.DataGridView.ApplyVisualStylesToHeaderCells;
		}

		/// <summary>Indicates whether a row will be unshared when the mouse pointer moves over a cell in the row.</summary>
		/// <param name="rowIndex">The index of the row that the mouse pointer entered.</param>
		/// <returns>
		///     <see langword="true" /> if visual styles are enabled, and the <see cref="P:System.Windows.Forms.DataGridView.EnableHeadersVisualStyles" /> property is <see langword="true" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001DDC RID: 7644 RVA: 0x00094004 File Offset: 0x00092204
		protected override bool MouseEnterUnsharesRow(int rowIndex)
		{
			return base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y && base.DataGridView.ApplyVisualStylesToHeaderCells;
		}

		/// <summary>Indicates whether a row will be unshared when the mouse pointer leaves the row.</summary>
		/// <param name="rowIndex">The index of the row that the mouse pointer left.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.DataGridViewHeaderCell.ButtonState" /> property value is not <see cref="F:System.Windows.Forms.ButtonState.Normal" />, visual styles are enabled, and the <see cref="P:System.Windows.Forms.DataGridView.EnableHeadersVisualStyles" /> property is <see langword="true" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001DDD RID: 7645 RVA: 0x0009404F File Offset: 0x0009224F
		protected override bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return this.ButtonState != ButtonState.Normal && base.DataGridView.ApplyVisualStylesToHeaderCells;
		}

		/// <summary>Indicates whether a row will be unshared when the mouse button is released while the pointer is on a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains information about the mouse position.</param>
		/// <returns>
		///     <see langword="true" /> if the left mouse button was released, visual styles are enabled, and the <see cref="P:System.Windows.Forms.DataGridView.EnableHeadersVisualStyles" /> property is <see langword="true" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001DDE RID: 7646 RVA: 0x00093FE5 File Offset: 0x000921E5
		protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left && base.DataGridView.ApplyVisualStylesToHeaderCells;
		}

		/// <summary>Called when the mouse button is held down while the pointer is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains information about the mouse position.</param>
		// Token: 0x06001DDF RID: 7647 RVA: 0x00094068 File Offset: 0x00092268
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left && base.DataGridView.ApplyVisualStylesToHeaderCells && !base.DataGridView.ResizingOperationAboutToStart)
			{
				this.UpdateButtonState(ButtonState.Pushed, e.RowIndex);
			}
		}

		/// <summary>Called when the mouse pointer enters the cell.</summary>
		/// <param name="rowIndex">The index of the row containing the cell.</param>
		// Token: 0x06001DE0 RID: 7648 RVA: 0x000940B8 File Offset: 0x000922B8
		protected override void OnMouseEnter(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				if (base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y && this.ButtonState == ButtonState.Normal && Control.MouseButtons == MouseButtons.Left && !base.DataGridView.ResizingOperationAboutToStart)
				{
					this.UpdateButtonState(ButtonState.Pushed, rowIndex);
				}
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
		}

		/// <summary>Called when the mouse pointer leaves the cell.</summary>
		/// <param name="rowIndex">The index of the row containing the cell.</param>
		// Token: 0x06001DE1 RID: 7649 RVA: 0x0009414B File Offset: 0x0009234B
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				if (this.ButtonState != ButtonState.Normal)
				{
					this.UpdateButtonState(ButtonState.Normal, rowIndex);
				}
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
		}

		/// <summary>Called when the mouse button is released while the pointer is over the cell. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains information about the mouse position.</param>
		// Token: 0x06001DE2 RID: 7650 RVA: 0x00094185 File Offset: 0x00092385
		protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left && base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				this.UpdateButtonState(ButtonState.Normal, e.RowIndex);
			}
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewHeaderCell" />.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the cell.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
		/// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the bounds of the cell that is being painted.</param>
		/// <param name="rowIndex">The row index of the cell that is being painted.</param>
		/// <param name="dataGridViewElementState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the cell.</param>
		/// <param name="value">The data of the cell that is being painted.</param>
		/// <param name="formattedValue">The formatted data of the cell that is being painted.</param>
		/// <param name="errorText">An error message that is associated with the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the cell.</param>
		/// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that contains border styles for the cell that is being painted.</param>
		/// <param name="paintParts">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values that specifies which parts of the cell need to be painted.</param>
		// Token: 0x06001DE3 RID: 7651 RVA: 0x000941B8 File Offset: 0x000923B8
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			if (DataGridViewCell.PaintBackground(paintParts))
			{
				Rectangle rect = cellBounds;
				Rectangle rectangle = this.BorderWidths(advancedBorderStyle);
				rect.Offset(rectangle.X, rectangle.Y);
				rect.Width -= rectangle.Right;
				rect.Height -= rectangle.Bottom;
				bool flag = (dataGridViewElementState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
				SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
				if (cachedBrush.Color.A == 255)
				{
					graphics.FillRectangle(cachedBrush, rect);
				}
			}
		}

		/// <summary>Returns a string that describes the current object. </summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x06001DE4 RID: 7652 RVA: 0x00094290 File Offset: 0x00092490
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewHeaderCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000942EC File Offset: 0x000924EC
		private void UpdateButtonState(ButtonState newButtonState, int rowIndex)
		{
			this.ButtonStatePrivate = newButtonState;
			base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
		}

		// Token: 0x04000D16 RID: 3350
		private const byte DATAGRIDVIEWHEADERCELL_themeMargin = 100;

		// Token: 0x04000D17 RID: 3351
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04000D18 RID: 3352
		private static Type defaultValueType = typeof(object);

		// Token: 0x04000D19 RID: 3353
		private static Type cellType = typeof(DataGridViewHeaderCell);

		// Token: 0x04000D1A RID: 3354
		private static Rectangle rectThemeMargins = new Rectangle(-1, -1, 0, 0);

		// Token: 0x04000D1B RID: 3355
		private static readonly int PropValueType = PropertyStore.CreateKey();

		// Token: 0x04000D1C RID: 3356
		private static readonly int PropButtonState = PropertyStore.CreateKey();

		// Token: 0x04000D1D RID: 3357
		private static readonly int PropFlipXPThemesBitmap = PropertyStore.CreateKey();

		// Token: 0x04000D1E RID: 3358
		private const string AEROTHEMEFILENAME = "Aero.msstyles";

		// Token: 0x020005B6 RID: 1462
		private class DataGridViewHeaderCellRenderer
		{
			// Token: 0x06005987 RID: 22919 RVA: 0x000027DB File Offset: 0x000009DB
			private DataGridViewHeaderCellRenderer()
			{
			}

			// Token: 0x170015A3 RID: 5539
			// (get) Token: 0x06005988 RID: 22920 RVA: 0x0017901C File Offset: 0x0017721C
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewHeaderCell.DataGridViewHeaderCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewHeaderCell.DataGridViewHeaderCellRenderer.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);
					}
					return DataGridViewHeaderCell.DataGridViewHeaderCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x04003939 RID: 14649
			private static VisualStyleRenderer visualStyleRenderer;
		}
	}
}
