using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Represents an individual cell in a <see cref="T:System.Windows.Forms.DataGridView" /> control. </summary>
	// Token: 0x02000192 RID: 402
	[TypeConverter(typeof(DataGridViewCellConverter))]
	public abstract class DataGridViewCell : DataGridViewElement, ICloneable, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> class. </summary>
		// Token: 0x060019DE RID: 6622 RVA: 0x0008149C File Offset: 0x0007F69C
		protected DataGridViewCell()
		{
			if (!DataGridViewCell.isScalingInitialized)
			{
				if (DpiHelper.IsScalingRequired)
				{
					DataGridViewCell.iconsWidth = (byte)DpiHelper.LogicalToDeviceUnitsX(12);
					DataGridViewCell.iconsHeight = (byte)DpiHelper.LogicalToDeviceUnitsY(11);
				}
				DataGridViewCell.isScalingInitialized = true;
			}
			this.propertyStore = new PropertyStore();
			base.StateInternal = DataGridViewElementStates.None;
		}

		/// <summary>Releases the unmanaged resources and performs other cleanup operations before the <see cref="T:System.Windows.Forms.DataGridViewCell" /> is reclaimed by garbage collection.</summary>
		// Token: 0x060019DF RID: 6623 RVA: 0x000814F0 File Offset: 0x0007F6F0
		~DataGridViewCell()
		{
			this.Dispose(false);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" /> assigned to the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" /> assigned to the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x060019E0 RID: 6624 RVA: 0x00081520 File Offset: 0x0007F720
		[Browsable(false)]
		public AccessibleObject AccessibilityObject
		{
			get
			{
				AccessibleObject accessibleObject = (AccessibleObject)this.Properties.GetObject(DataGridViewCell.PropCellAccessibilityObject);
				if (accessibleObject == null)
				{
					accessibleObject = this.CreateAccessibilityInstance();
					this.Properties.SetObject(DataGridViewCell.PropCellAccessibilityObject, accessibleObject);
				}
				return accessibleObject;
			}
		}

		/// <summary>Gets the column index for this cell. </summary>
		/// <returns>The index of the column that contains the cell; -1 if the cell is not contained within a column.</returns>
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x0008155F File Offset: 0x0007F75F
		public int ColumnIndex
		{
			get
			{
				if (this.owningColumn == null)
				{
					return -1;
				}
				return this.owningColumn.Index;
			}
		}

		/// <summary>Gets the bounding rectangle that encloses the cell's content area.</summary>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The row containing the cell is a shared row.-or-The cell is a column header cell.</exception>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> property is less than 0, indicating that the cell is a row header cell.</exception>
		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060019E2 RID: 6626 RVA: 0x00081576 File Offset: 0x0007F776
		[Browsable(false)]
		public Rectangle ContentBounds
		{
			get
			{
				return this.GetContentBounds(this.RowIndex);
			}
		}

		/// <summary>Gets or sets the shortcut menu associated with the cell. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> associated with the cell.</returns>
		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x00081584 File Offset: 0x0007F784
		// (set) Token: 0x060019E4 RID: 6628 RVA: 0x00081592 File Offset: 0x0007F792
		[DefaultValue(null)]
		public virtual ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return this.GetContextMenuStrip(this.RowIndex);
			}
			set
			{
				this.ContextMenuStripInternal = value;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x0008159B File Offset: 0x0007F79B
		// (set) Token: 0x060019E6 RID: 6630 RVA: 0x000815B4 File Offset: 0x0007F7B4
		private ContextMenuStrip ContextMenuStripInternal
		{
			get
			{
				return (ContextMenuStrip)this.Properties.GetObject(DataGridViewCell.PropCellContextMenuStrip);
			}
			set
			{
				ContextMenuStrip contextMenuStrip = (ContextMenuStrip)this.Properties.GetObject(DataGridViewCell.PropCellContextMenuStrip);
				if (contextMenuStrip != value)
				{
					EventHandler value2 = new EventHandler(this.DetachContextMenuStrip);
					if (contextMenuStrip != null)
					{
						contextMenuStrip.Disposed -= value2;
					}
					this.Properties.SetObject(DataGridViewCell.PropCellContextMenuStrip, value);
					if (value != null)
					{
						value.Disposed += value2;
					}
					if (base.DataGridView != null)
					{
						base.DataGridView.OnCellContextMenuStripChanged(this);
					}
				}
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x00081621 File Offset: 0x0007F821
		// (set) Token: 0x060019E8 RID: 6632 RVA: 0x0008162C File Offset: 0x0007F82C
		private byte CurrentMouseLocation
		{
			get
			{
				return this.flags & 3;
			}
			set
			{
				this.flags = (byte)((int)this.flags & -4);
				this.flags |= value;
			}
		}

		/// <summary>Gets the default value for a cell in the row for new records.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing the default value.</returns>
		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x060019E9 RID: 6633 RVA: 0x0000DE5C File Offset: 0x0000C05C
		[Browsable(false)]
		public virtual object DefaultNewRowValue
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets a value that indicates whether the cell is currently displayed on-screen. </summary>
		/// <returns>
		///     <see langword="true" /> if the cell is on-screen or partially on-screen; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x060019EA RID: 6634 RVA: 0x00081650 File Offset: 0x0007F850
		[Browsable(false)]
		public virtual bool Displayed
		{
			get
			{
				return base.DataGridView != null && (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0) && this.owningColumn.Displayed && this.owningRow.Displayed;
			}
		}

		/// <summary>Gets the current, formatted value of the cell, regardless of whether the cell is in edit mode and the value has not been committed. </summary>
		/// <returns>The current, formatted value of the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The row containing the cell is a shared row.-or-The cell is a column header cell.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell. </exception>
		/// <exception cref="T:System.Exception">Formatting failed and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError" /> event of the <see cref="T:System.Windows.Forms.DataGridView" /> control or the handler set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException" /> property to <see langword="true" />. The exception object can typically be cast to type <see cref="T:System.FormatException" />.</exception>
		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x000816A0 File Offset: 0x0007F8A0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object EditedFormattedValue
		{
			get
			{
				if (base.DataGridView == null)
				{
					return null;
				}
				DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, this.RowIndex, false);
				return this.GetEditedFormattedValue(this.GetValue(this.RowIndex), this.RowIndex, ref inheritedStyle, DataGridViewDataErrorContexts.Formatting);
			}
		}

		/// <summary>Gets the type of the cell's hosted editing control. </summary>
		/// <returns>A <see cref="T:System.Type" /> representing the <see cref="T:System.Windows.Forms.DataGridViewTextBoxEditingControl" /> type.</returns>
		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x000816E1 File Offset: 0x0007F8E1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual Type EditType
		{
			get
			{
				return typeof(DataGridViewTextBoxEditingControl);
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x000816ED File Offset: 0x0007F8ED
		private static Bitmap ErrorBitmap
		{
			get
			{
				if (DataGridViewCell.errorBmp == null)
				{
					DataGridViewCell.errorBmp = DataGridViewCell.GetBitmap("DataGridViewRow.error.bmp");
				}
				return DataGridViewCell.errorBmp;
			}
		}

		/// <summary>Gets the bounds of the error icon for the cell.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the error icon for the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The cell does not belong to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or- 
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The row containing the cell is a shared row.-or-The cell is a column header cell.</exception>
		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x060019EE RID: 6638 RVA: 0x0008170A File Offset: 0x0007F90A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Rectangle ErrorIconBounds
		{
			get
			{
				return this.GetErrorIconBounds(this.RowIndex);
			}
		}

		/// <summary>Gets or sets the text describing an error condition associated with the cell. </summary>
		/// <returns>The text that describes an error condition associated with the cell.</returns>
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x00081718 File Offset: 0x0007F918
		// (set) Token: 0x060019F0 RID: 6640 RVA: 0x00081726 File Offset: 0x0007F926
		[Browsable(false)]
		public string ErrorText
		{
			get
			{
				return this.GetErrorText(this.RowIndex);
			}
			set
			{
				this.ErrorTextInternal = value;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x060019F1 RID: 6641 RVA: 0x00081730 File Offset: 0x0007F930
		// (set) Token: 0x060019F2 RID: 6642 RVA: 0x00081760 File Offset: 0x0007F960
		private string ErrorTextInternal
		{
			get
			{
				object @object = this.Properties.GetObject(DataGridViewCell.PropCellErrorText);
				if (@object != null)
				{
					return (string)@object;
				}
				return string.Empty;
			}
			set
			{
				string errorTextInternal = this.ErrorTextInternal;
				if (!string.IsNullOrEmpty(value) || this.Properties.ContainsObject(DataGridViewCell.PropCellErrorText))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellErrorText, value);
				}
				if (base.DataGridView != null && !errorTextInternal.Equals(this.ErrorTextInternal))
				{
					base.DataGridView.OnCellErrorTextChanged(this);
				}
			}
		}

		/// <summary>Gets the value of the cell as formatted for display.</summary>
		/// <returns>The formatted value of the cell or <see langword="null" /> if the cell does not belong to a <see cref="T:System.Windows.Forms.DataGridView" /> control.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The row containing the cell is a shared row.-or-The cell is a column header cell.</exception>
		/// <exception cref="T:System.Exception">Formatting failed and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError" /> event of the <see cref="T:System.Windows.Forms.DataGridView" /> control or the handler set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException" /> property to <see langword="true" />. The exception object can typically be cast to type <see cref="T:System.FormatException" />.</exception>
		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x000817C4 File Offset: 0x0007F9C4
		[Browsable(false)]
		public object FormattedValue
		{
			get
			{
				if (base.DataGridView == null)
				{
					return null;
				}
				DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, this.RowIndex, false);
				return this.GetFormattedValue(this.RowIndex, ref inheritedStyle, DataGridViewDataErrorContexts.Formatting);
			}
		}

		/// <summary>Gets the type of the formatted value associated with the cell. </summary>
		/// <returns>A <see cref="T:System.Type" /> representing the type of the cell's formatted value.</returns>
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x000817F9 File Offset: 0x0007F9F9
		[Browsable(false)]
		public virtual Type FormattedValueType
		{
			get
			{
				return this.ValueType;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060019F5 RID: 6645 RVA: 0x00081804 File Offset: 0x0007FA04
		private TypeConverter FormattedValueTypeConverter
		{
			get
			{
				TypeConverter result = null;
				if (this.FormattedValueType != null)
				{
					if (base.DataGridView != null)
					{
						result = base.DataGridView.GetCachedTypeConverter(this.FormattedValueType);
					}
					else
					{
						result = TypeDescriptor.GetConverter(this.FormattedValueType);
					}
				}
				return result;
			}
		}

		/// <summary>Gets a value indicating whether the cell is frozen. </summary>
		/// <returns>
		///     <see langword="true" /> if the cell is frozen; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x0008184C File Offset: 0x0007FA4C
		[Browsable(false)]
		public virtual bool Frozen
		{
			get
			{
				if (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0)
				{
					return this.owningColumn.Frozen && this.owningRow.Frozen;
				}
				return this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.Frozen;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x000818B9 File Offset: 0x0007FAB9
		internal bool HasErrorText
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellErrorText) && this.Properties.GetObject(DataGridViewCell.PropCellErrorText) != null;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.DataGridViewCell.Style" /> property has been set.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.DataGridViewCell.Style" /> property has been set; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x000818E2 File Offset: 0x0007FAE2
		[Browsable(false)]
		public bool HasStyle
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellStyle) && this.Properties.GetObject(DataGridViewCell.PropCellStyle) != null;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x0008190B File Offset: 0x0007FB0B
		internal bool HasToolTipText
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellToolTipText) && this.Properties.GetObject(DataGridViewCell.PropCellToolTipText) != null;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x00081934 File Offset: 0x0007FB34
		internal bool HasValue
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellValue) && this.Properties.GetObject(DataGridViewCell.PropCellValue) != null;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x0008195D File Offset: 0x0007FB5D
		internal virtual bool HasValueType
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellValueType) && this.Properties.GetObject(DataGridViewCell.PropCellValueType) != null;
			}
		}

		/// <summary>Gets the current state of the cell as inherited from the state of its row and column.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values representing the current state of the cell.</returns>
		/// <exception cref="T:System.ArgumentException">The cell is not contained within a <see cref="T:System.Windows.Forms.DataGridView" /> control and the value of its <see cref="P:System.Windows.Forms.DataGridViewCell.RowIndex" /> property is not -1.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The cell is contained within a <see cref="T:System.Windows.Forms.DataGridView" /> control and the value of its <see cref="P:System.Windows.Forms.DataGridViewCell.RowIndex" /> property is -1.</exception>
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x00081986 File Offset: 0x0007FB86
		[Browsable(false)]
		public DataGridViewElementStates InheritedState
		{
			get
			{
				return this.GetInheritedState(this.RowIndex);
			}
		}

		/// <summary>Gets the style currently applied to the cell. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> currently applied to the cell.</returns>
		/// <exception cref="T:System.InvalidOperationException">The cell does not belong to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or- 
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The row containing the cell is a shared row.-or-The cell is a column header cell.</exception>
		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x00081994 File Offset: 0x0007FB94
		[Browsable(false)]
		public DataGridViewCellStyle InheritedStyle
		{
			get
			{
				return this.GetInheritedStyleInternal(this.RowIndex);
			}
		}

		/// <summary>Gets a value indicating whether this cell is currently being edited.</summary>
		/// <returns>
		///     <see langword="true" /> if the cell is in edit mode; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The row containing the cell is a shared row.</exception>
		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x000819A4 File Offset: 0x0007FBA4
		[Browsable(false)]
		public bool IsInEditMode
		{
			get
			{
				if (base.DataGridView == null)
				{
					return false;
				}
				if (this.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				Point currentCellAddress = base.DataGridView.CurrentCellAddress;
				return currentCellAddress.X != -1 && currentCellAddress.X == this.ColumnIndex && currentCellAddress.Y == this.RowIndex && base.DataGridView.IsCurrentCellInEditMode;
			}
		}

		/// <summary>Gets the column that contains this cell.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewColumn" /> that contains the cell, or <see langword="null" /> if the cell is not in a column.</returns>
		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x00081A15 File Offset: 0x0007FC15
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public DataGridViewColumn OwningColumn
		{
			get
			{
				return this.owningColumn;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (set) Token: 0x06001A00 RID: 6656 RVA: 0x00081A1D File Offset: 0x0007FC1D
		internal DataGridViewColumn OwningColumnInternal
		{
			set
			{
				this.owningColumn = value;
			}
		}

		/// <summary>Gets the row that contains this cell. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewRow" /> that contains the cell, or <see langword="null" /> if the cell is not in a row.</returns>
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x00081A26 File Offset: 0x0007FC26
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public DataGridViewRow OwningRow
		{
			get
			{
				return this.owningRow;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (set) Token: 0x06001A02 RID: 6658 RVA: 0x00081A2E File Offset: 0x0007FC2E
		internal DataGridViewRow OwningRowInternal
		{
			set
			{
				this.owningRow = value;
			}
		}

		/// <summary>Gets the size, in pixels, of a rectangular area into which the cell can fit. </summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> containing the height and width, in pixels.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The row containing the cell is a shared row.-or-The cell is a column header cell.</exception>
		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x00081A37 File Offset: 0x0007FC37
		[Browsable(false)]
		public Size PreferredSize
		{
			get
			{
				return this.GetPreferredSize(this.RowIndex);
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001A04 RID: 6660 RVA: 0x00081A45 File Offset: 0x0007FC45
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		/// <summary>Gets or sets a value indicating whether the cell's data can be edited. </summary>
		/// <returns>
		///     <see langword="true" /> if the cell's data cannot be edited; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">There is no owning row when setting this property. -or-The owning row is shared when setting this property.</exception>
		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x00081A50 File Offset: 0x0007FC50
		// (set) Token: 0x06001A06 RID: 6662 RVA: 0x00081AC0 File Offset: 0x0007FCC0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool ReadOnly
		{
			get
			{
				return (this.State & DataGridViewElementStates.ReadOnly) != DataGridViewElementStates.None || (this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.ReadOnly) || (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0 && this.owningColumn.ReadOnly);
			}
			set
			{
				if (base.DataGridView != null)
				{
					if (this.RowIndex == -1)
					{
						throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
					}
					if (value != this.ReadOnly && !base.DataGridView.ReadOnly)
					{
						base.DataGridView.OnDataGridViewElementStateChanging(this, -1, DataGridViewElementStates.ReadOnly);
						base.DataGridView.SetReadOnlyCellCore(this.ColumnIndex, this.RowIndex, value);
						return;
					}
				}
				else if (this.owningRow == null)
				{
					if (value != this.ReadOnly)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCell_CannotSetReadOnlyState"));
					}
				}
				else
				{
					this.owningRow.SetReadOnlyCellCore(this, value);
				}
			}
		}

		// Token: 0x170005FB RID: 1531
		// (set) Token: 0x06001A07 RID: 6663 RVA: 0x00081B59 File Offset: 0x0007FD59
		internal bool ReadOnlyInternal
		{
			set
			{
				if (value)
				{
					base.StateInternal = (this.State | DataGridViewElementStates.ReadOnly);
				}
				else
				{
					base.StateInternal = (this.State & ~DataGridViewElementStates.ReadOnly);
				}
				if (base.DataGridView != null)
				{
					base.DataGridView.OnDataGridViewElementStateChanged(this, -1, DataGridViewElementStates.ReadOnly);
				}
			}
		}

		/// <summary>Gets a value indicating whether the cell can be resized. </summary>
		/// <returns>
		///     <see langword="true" /> if the cell can be resized; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x00081B94 File Offset: 0x0007FD94
		[Browsable(false)]
		public virtual bool Resizable
		{
			get
			{
				return (this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.Resizable == DataGridViewTriState.True) || (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0 && this.owningColumn.Resizable == DataGridViewTriState.True);
			}
		}

		/// <summary>Gets the index of the cell's parent row. </summary>
		/// <returns>The index of the row that contains the cell; -1 if there is no owning row.</returns>
		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x00081BF9 File Offset: 0x0007FDF9
		[Browsable(false)]
		public int RowIndex
		{
			get
			{
				if (this.owningRow == null)
				{
					return -1;
				}
				return this.owningRow.Index;
			}
		}

		/// <summary>Gets or sets a value indicating whether the cell has been selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the cell has been selected; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">There is no associated <see cref="T:System.Windows.Forms.DataGridView" /> when setting this property. -or-The owning row is shared when setting this property.</exception>
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001A0A RID: 6666 RVA: 0x00081C10 File Offset: 0x0007FE10
		// (set) Token: 0x06001A0B RID: 6667 RVA: 0x00081C80 File Offset: 0x0007FE80
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Selected
		{
			get
			{
				return (this.State & DataGridViewElementStates.Selected) != DataGridViewElementStates.None || (this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.Selected) || (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0 && this.owningColumn.Selected);
			}
			set
			{
				if (base.DataGridView != null)
				{
					if (this.RowIndex == -1)
					{
						throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
					}
					base.DataGridView.SetSelectedCellCoreInternal(this.ColumnIndex, this.RowIndex, value);
					return;
				}
				else
				{
					if (value)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCell_CannotSetSelectedState"));
					}
					return;
				}
			}
		}

		// Token: 0x170005FF RID: 1535
		// (set) Token: 0x06001A0C RID: 6668 RVA: 0x00081CDA File Offset: 0x0007FEDA
		internal bool SelectedInternal
		{
			set
			{
				if (value)
				{
					base.StateInternal = (this.State | DataGridViewElementStates.Selected);
				}
				else
				{
					base.StateInternal = (this.State & ~DataGridViewElementStates.Selected);
				}
				if (base.DataGridView != null)
				{
					base.DataGridView.OnDataGridViewElementStateChanged(this, -1, DataGridViewElementStates.Selected);
				}
			}
		}

		/// <summary>Gets the size of the cell.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> set to the owning row's height and the owning column's width. </returns>
		/// <exception cref="T:System.InvalidOperationException">The row containing the cell is a shared row.-or-The cell is a column header cell.</exception>
		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x00081D16 File Offset: 0x0007FF16
		[Browsable(false)]
		public Size Size
		{
			get
			{
				return this.GetSize(this.RowIndex);
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001A0E RID: 6670 RVA: 0x00081D24 File Offset: 0x0007FF24
		internal Rectangle StdBorderWidths
		{
			get
			{
				if (base.DataGridView != null)
				{
					DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
					DataGridViewAdvancedBorderStyle advancedBorderStyle = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, false, false, false, false);
					return this.BorderWidths(advancedBorderStyle);
				}
				return Rectangle.Empty;
			}
		}

		/// <summary>Gets or sets the style for the cell. </summary>
		/// <returns>The style associated with the cell.</returns>
		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001A0F RID: 6671 RVA: 0x00081D64 File Offset: 0x0007FF64
		// (set) Token: 0x06001A10 RID: 6672 RVA: 0x00081DB0 File Offset: 0x0007FFB0
		[Browsable(true)]
		public DataGridViewCellStyle Style
		{
			get
			{
				DataGridViewCellStyle dataGridViewCellStyle = (DataGridViewCellStyle)this.Properties.GetObject(DataGridViewCell.PropCellStyle);
				if (dataGridViewCellStyle == null)
				{
					dataGridViewCellStyle = new DataGridViewCellStyle();
					dataGridViewCellStyle.AddScope(base.DataGridView, DataGridViewCellStyleScopes.Cell);
					this.Properties.SetObject(DataGridViewCell.PropCellStyle, dataGridViewCellStyle);
				}
				return dataGridViewCellStyle;
			}
			set
			{
				DataGridViewCellStyle dataGridViewCellStyle = null;
				if (this.HasStyle)
				{
					dataGridViewCellStyle = this.Style;
					dataGridViewCellStyle.RemoveScope(DataGridViewCellStyleScopes.Cell);
				}
				if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellStyle))
				{
					if (value != null)
					{
						value.AddScope(base.DataGridView, DataGridViewCellStyleScopes.Cell);
					}
					this.Properties.SetObject(DataGridViewCell.PropCellStyle, value);
				}
				if (((dataGridViewCellStyle != null && value == null) || (dataGridViewCellStyle == null && value != null) || (dataGridViewCellStyle != null && value != null && !dataGridViewCellStyle.Equals(this.Style))) && base.DataGridView != null)
				{
					base.DataGridView.OnCellStyleChanged(this);
				}
			}
		}

		/// <summary>Gets or sets the object that contains supplemental data about the cell. </summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the cell. The default is <see langword="null" />.</returns>
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x00081E3F File Offset: 0x0008003F
		// (set) Token: 0x06001A12 RID: 6674 RVA: 0x00081E51 File Offset: 0x00080051
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.Properties.GetObject(DataGridViewCell.PropCellTag);
			}
			set
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellTag))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellTag, value);
				}
			}
		}

		/// <summary>Gets or sets the ToolTip text associated with this cell.</summary>
		/// <returns>The ToolTip text associated with the cell. The default is <see cref="F:System.String.Empty" />. </returns>
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001A13 RID: 6675 RVA: 0x00081E79 File Offset: 0x00080079
		// (set) Token: 0x06001A14 RID: 6676 RVA: 0x00081E87 File Offset: 0x00080087
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ToolTipText
		{
			get
			{
				return this.GetToolTipText(this.RowIndex);
			}
			set
			{
				this.ToolTipTextInternal = value;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001A15 RID: 6677 RVA: 0x00081E90 File Offset: 0x00080090
		// (set) Token: 0x06001A16 RID: 6678 RVA: 0x00081EC0 File Offset: 0x000800C0
		private string ToolTipTextInternal
		{
			get
			{
				object @object = this.Properties.GetObject(DataGridViewCell.PropCellToolTipText);
				if (@object != null)
				{
					return (string)@object;
				}
				return string.Empty;
			}
			set
			{
				string toolTipTextInternal = this.ToolTipTextInternal;
				if (!string.IsNullOrEmpty(value) || this.Properties.ContainsObject(DataGridViewCell.PropCellToolTipText))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellToolTipText, value);
				}
				if (base.DataGridView != null && !toolTipTextInternal.Equals(this.ToolTipTextInternal))
				{
					base.DataGridView.OnCellToolTipTextChanged(this);
				}
			}
		}

		/// <summary>Gets or sets the value associated with this cell. </summary>
		/// <returns>Gets or sets the data to be displayed by the cell. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.RowIndex" /> is outside the valid range of 0 to the number of rows in the control minus 1.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x00081F21 File Offset: 0x00080121
		// (set) Token: 0x06001A18 RID: 6680 RVA: 0x00081F2F File Offset: 0x0008012F
		[Browsable(false)]
		public object Value
		{
			get
			{
				return this.GetValue(this.RowIndex);
			}
			set
			{
				this.SetValue(this.RowIndex, value);
			}
		}

		/// <summary>Gets or sets the data type of the values in the cell. </summary>
		/// <returns>A <see cref="T:System.Type" /> representing the data type of the value in the cell.</returns>
		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x00081F40 File Offset: 0x00080140
		// (set) Token: 0x06001A1A RID: 6682 RVA: 0x00081F81 File Offset: 0x00080181
		[Browsable(false)]
		public virtual Type ValueType
		{
			get
			{
				Type type = (Type)this.Properties.GetObject(DataGridViewCell.PropCellValueType);
				if (type == null && this.OwningColumn != null)
				{
					type = this.OwningColumn.ValueType;
				}
				return type;
			}
			set
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellValueType))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellValueType, value);
				}
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001A1B RID: 6683 RVA: 0x00081FB0 File Offset: 0x000801B0
		private TypeConverter ValueTypeConverter
		{
			get
			{
				TypeConverter typeConverter = null;
				if (this.OwningColumn != null)
				{
					typeConverter = this.OwningColumn.BoundColumnConverter;
				}
				if (typeConverter == null && this.ValueType != null)
				{
					if (base.DataGridView != null)
					{
						typeConverter = base.DataGridView.GetCachedTypeConverter(this.ValueType);
					}
					else
					{
						typeConverter = TypeDescriptor.GetConverter(this.ValueType);
					}
				}
				return typeConverter;
			}
		}

		/// <summary>Gets a value indicating whether the cell is in a row or column that has been hidden. </summary>
		/// <returns>
		///     <see langword="true" /> if the cell is visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x00082010 File Offset: 0x00080210
		[Browsable(false)]
		public virtual bool Visible
		{
			get
			{
				if (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0)
				{
					return this.owningColumn.Visible && this.owningRow.Visible;
				}
				return this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.Visible;
			}
		}

		/// <summary>Modifies the input cell border style according to the specified criteria. </summary>
		/// <param name="dataGridViewAdvancedBorderStyleInput">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that represents the cell border style to modify.</param>
		/// <param name="dataGridViewAdvancedBorderStylePlaceholder">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that is used to store intermediate changes to the cell border style. </param>
		/// <param name="singleVerticalBorderAdded">
		///       <see langword="true" /> to add a vertical border to the cell; otherwise, <see langword="false" />. </param>
		/// <param name="singleHorizontalBorderAdded">
		///       <see langword="true" /> to add a horizontal border to the cell; otherwise, <see langword="false" />. </param>
		/// <param name="isFirstDisplayedColumn">
		///       <see langword="true" /> if the hosting cell is in the first visible column; otherwise, <see langword="false" />. </param>
		/// <param name="isFirstDisplayedRow">
		///       <see langword="true" /> if the hosting cell is in the first visible row; otherwise, <see langword="false" />. </param>
		/// <returns>The modified <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" />.</returns>
		// Token: 0x06001A1D RID: 6685 RVA: 0x00082080 File Offset: 0x00080280
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual DataGridViewAdvancedBorderStyle AdjustCellBorderStyle(DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput, DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
		{
			DataGridViewAdvancedCellBorderStyle all = dataGridViewAdvancedBorderStyleInput.All;
			if (all != DataGridViewAdvancedCellBorderStyle.NotSet)
			{
				if (all == DataGridViewAdvancedCellBorderStyle.Single)
				{
					if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Single;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = ((isFirstDisplayedColumn && singleVerticalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = ((isFirstDisplayedColumn && singleVerticalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Single;
					}
					dataGridViewAdvancedBorderStylePlaceholder.TopInternal = ((isFirstDisplayedRow && singleHorizontalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.Single;
					return dataGridViewAdvancedBorderStylePlaceholder;
				}
				if (all != DataGridViewAdvancedCellBorderStyle.OutsetPartial)
				{
				}
			}
			else if (base.DataGridView != null && base.DataGridView.AdvancedCellBorderStyle == dataGridViewAdvancedBorderStyleInput)
			{
				DataGridViewCellBorderStyle cellBorderStyle = base.DataGridView.CellBorderStyle;
				if (cellBorderStyle == DataGridViewCellBorderStyle.SingleVertical)
				{
					if (base.DataGridView.RightToLeftInternal)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Single;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = ((isFirstDisplayedColumn && singleVerticalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = ((isFirstDisplayedColumn && singleVerticalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Single;
					}
					dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.None;
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.None;
					return dataGridViewAdvancedBorderStylePlaceholder;
				}
				if (cellBorderStyle == DataGridViewCellBorderStyle.SingleHorizontal)
				{
					dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.None;
					dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.None;
					dataGridViewAdvancedBorderStylePlaceholder.TopInternal = ((isFirstDisplayedRow && singleHorizontalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.Single;
					return dataGridViewAdvancedBorderStylePlaceholder;
				}
			}
			return dataGridViewAdvancedBorderStyleInput;
		}

		/// <summary>Returns a <see cref="T:System.Drawing.Rectangle" /> that represents the widths of all the cell margins. </summary>
		/// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that the margins are to be calculated for. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the widths of all the cell margins.</returns>
		// Token: 0x06001A1E RID: 6686 RVA: 0x000821B0 File Offset: 0x000803B0
		protected virtual Rectangle BorderWidths(DataGridViewAdvancedBorderStyle advancedBorderStyle)
		{
			Rectangle result = default(Rectangle);
			result.X = ((advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.None) ? 0 : 1);
			if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
			{
				int num = result.X;
				result.X = num + 1;
			}
			result.Y = ((advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None) ? 0 : 1);
			if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.InsetDouble)
			{
				int num = result.Y;
				result.Y = num + 1;
			}
			result.Width = ((advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.None) ? 0 : 1);
			if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble)
			{
				int num = result.Width;
				result.Width = num + 1;
			}
			result.Height = ((advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.None) ? 0 : 1);
			if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.InsetDouble)
			{
				int num = result.Height;
				result.Height = num + 1;
			}
			if (this.owningColumn != null)
			{
				if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
				{
					result.X += this.owningColumn.DividerWidth;
				}
				else
				{
					result.Width += this.owningColumn.DividerWidth;
				}
			}
			if (this.owningRow != null)
			{
				result.Height += this.owningRow.DividerHeight;
			}
			return result;
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void CacheEditingControl()
		{
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x00082314 File Offset: 0x00080514
		internal DataGridViewElementStates CellStateFromColumnRowStates(DataGridViewElementStates rowState)
		{
			DataGridViewElementStates dataGridViewElementStates = DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected;
			DataGridViewElementStates dataGridViewElementStates2 = DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible;
			DataGridViewElementStates dataGridViewElementStates3 = this.owningColumn.State & dataGridViewElementStates;
			dataGridViewElementStates3 |= (rowState & dataGridViewElementStates);
			return dataGridViewElementStates3 | (this.owningColumn.State & dataGridViewElementStates2 & (rowState & dataGridViewElementStates2));
		}

		/// <summary>Indicates whether the cell's row will be unshared when the cell is clicked.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> containing the data passed to the <see cref="M:System.Windows.Forms.DataGridViewCell.OnClick(System.Windows.Forms.DataGridViewCellEventArgs)" /> method.</param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A21 RID: 6689 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool ClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return false;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00082350 File Offset: 0x00080550
		internal bool ClickUnsharesRowInternal(DataGridViewCellEventArgs e)
		{
			return this.ClickUnsharesRow(e);
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x0008235C File Offset: 0x0008055C
		internal void CloneInternal(DataGridViewCell dataGridViewCell)
		{
			if (this.HasValueType)
			{
				dataGridViewCell.ValueType = this.ValueType;
			}
			if (this.HasStyle)
			{
				dataGridViewCell.Style = new DataGridViewCellStyle(this.Style);
			}
			if (this.HasErrorText)
			{
				dataGridViewCell.ErrorText = this.ErrorTextInternal;
			}
			if (this.HasToolTipText)
			{
				dataGridViewCell.ToolTipText = this.ToolTipTextInternal;
			}
			if (this.ContextMenuStripInternal != null)
			{
				dataGridViewCell.ContextMenuStrip = this.ContextMenuStripInternal.Clone();
			}
			dataGridViewCell.StateInternal = (this.State & ~DataGridViewElementStates.Selected);
			dataGridViewCell.Tag = this.Tag;
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x06001A24 RID: 6692 RVA: 0x000823F4 File Offset: 0x000805F4
		public virtual object Clone()
		{
			DataGridViewCell dataGridViewCell = (DataGridViewCell)Activator.CreateInstance(base.GetType());
			this.CloneInternal(dataGridViewCell);
			return dataGridViewCell;
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0008241C File Offset: 0x0008061C
		internal static int ColorDistance(Color color1, Color color2)
		{
			int num = (int)(color1.R - color2.R);
			int num2 = (int)(color1.G - color2.G);
			int num3 = (int)(color1.B - color2.B);
			return num * num + num2 * num2 + num3 * num3;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x00082464 File Offset: 0x00080664
		internal void ComputeBorderStyleCellStateAndCellBounds(int rowIndex, out DataGridViewAdvancedBorderStyle dgvabsEffective, out DataGridViewElementStates cellState, out Rectangle cellBounds)
		{
			bool singleVerticalBorderAdded = !base.DataGridView.RowHeadersVisible && base.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
			bool singleHorizontalBorderAdded = !base.DataGridView.ColumnHeadersVisible && base.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
			if (rowIndex > -1 && this.OwningColumn != null)
			{
				dgvabsEffective = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, singleVerticalBorderAdded, singleHorizontalBorderAdded, this.ColumnIndex == base.DataGridView.FirstDisplayedColumnIndex, rowIndex == base.DataGridView.FirstDisplayedRowIndex);
				DataGridViewElementStates rowState = base.DataGridView.Rows.GetRowState(rowIndex);
				cellState = this.CellStateFromColumnRowStates(rowState);
				cellState |= this.State;
			}
			else if (this.OwningColumn != null)
			{
				DataGridViewColumn lastColumn = base.DataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None);
				bool isLastVisibleColumn = lastColumn != null && lastColumn.Index == this.ColumnIndex;
				dgvabsEffective = base.DataGridView.AdjustColumnHeaderBorderStyle(base.DataGridView.AdvancedColumnHeadersBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, this.ColumnIndex == base.DataGridView.FirstDisplayedColumnIndex, isLastVisibleColumn);
				cellState = (this.OwningColumn.State | this.State);
			}
			else if (this.OwningRow != null)
			{
				dgvabsEffective = this.OwningRow.AdjustRowHeaderBorderStyle(base.DataGridView.AdvancedRowHeadersBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, singleVerticalBorderAdded, singleHorizontalBorderAdded, rowIndex == base.DataGridView.FirstDisplayedRowIndex, rowIndex == base.DataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible));
				cellState = (this.OwningRow.GetState(rowIndex) | this.State);
			}
			else
			{
				dgvabsEffective = base.DataGridView.AdjustedTopLeftHeaderBorderStyle;
				cellState = this.State;
			}
			cellBounds = new Rectangle(new Point(0, 0), this.GetSize(rowIndex));
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x00082634 File Offset: 0x00080834
		internal Rectangle ComputeErrorIconBounds(Rectangle cellValueBounds)
		{
			if (cellValueBounds.Width >= (int)(8 + DataGridViewCell.iconsWidth) && cellValueBounds.Height >= (int)(8 + DataGridViewCell.iconsHeight))
			{
				Rectangle result = new Rectangle(base.DataGridView.RightToLeftInternal ? (cellValueBounds.Left + 4) : (cellValueBounds.Right - 4 - (int)DataGridViewCell.iconsWidth), cellValueBounds.Y + (cellValueBounds.Height - (int)DataGridViewCell.iconsHeight) / 2, (int)DataGridViewCell.iconsWidth, (int)DataGridViewCell.iconsHeight);
				return result;
			}
			return Rectangle.Empty;
		}

		/// <summary>Indicates whether the cell's row will be unshared when the cell's content is clicked.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> containing the data passed to the <see cref="M:System.Windows.Forms.DataGridViewCell.OnContentClick(System.Windows.Forms.DataGridViewCellEventArgs)" /> method.</param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A28 RID: 6696 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool ContentClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return false;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x000826B7 File Offset: 0x000808B7
		internal bool ContentClickUnsharesRowInternal(DataGridViewCellEventArgs e)
		{
			return this.ContentClickUnsharesRow(e);
		}

		/// <summary>Indicates whether the cell's row will be unshared when the cell's content is double-clicked.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> containing the data passed to the <see cref="M:System.Windows.Forms.DataGridViewCell.OnContentDoubleClick(System.Windows.Forms.DataGridViewCellEventArgs)" /> method.</param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A2A RID: 6698 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool ContentDoubleClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return false;
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x000826C0 File Offset: 0x000808C0
		internal bool ContentDoubleClickUnsharesRowInternal(DataGridViewCellEventArgs e)
		{
			return this.ContentDoubleClickUnsharesRow(e);
		}

		/// <summary>Creates a new accessible object for the <see cref="T:System.Windows.Forms.DataGridViewCell" />. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" /> for the <see cref="T:System.Windows.Forms.DataGridViewCell" />. </returns>
		// Token: 0x06001A2C RID: 6700 RVA: 0x000826C9 File Offset: 0x000808C9
		protected virtual AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewCell.DataGridViewCellAccessibleObject(this);
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x000826D1 File Offset: 0x000808D1
		private void DetachContextMenuStrip(object sender, EventArgs e)
		{
			this.ContextMenuStripInternal = null;
		}

		/// <summary>Removes the cell's editing control from the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">This cell is not associated with a <see cref="T:System.Windows.Forms.DataGridView" />.-or-The <see cref="P:System.Windows.Forms.DataGridView.EditingControl" /> property of the associated <see cref="T:System.Windows.Forms.DataGridView" /> has a value of <see langword="null" />. This is the case, for example, when the control is not in edit mode.</exception>
		// Token: 0x06001A2E RID: 6702 RVA: 0x000826DC File Offset: 0x000808DC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void DetachEditingControl()
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null || dataGridView.EditingControl == null)
			{
				throw new InvalidOperationException();
			}
			if (dataGridView.EditingControl.ParentInternal != null)
			{
				if (dataGridView.EditingControl.ContainsFocus)
				{
					ContainerControl containerControl = dataGridView.GetContainerControlInternal() as ContainerControl;
					if (containerControl != null && (dataGridView.EditingControl == containerControl.ActiveControl || dataGridView.EditingControl.Contains(containerControl.ActiveControl)))
					{
						dataGridView.FocusInternal();
					}
					else
					{
						UnsafeNativeMethods.SetFocus(new HandleRef(null, IntPtr.Zero));
					}
				}
				dataGridView.EditingPanel.Controls.Remove(dataGridView.EditingControl);
				if (AccessibilityImprovements.Level3 && this.AccessibleRestructuringNeeded)
				{
					dataGridView.EditingControlAccessibleObject.SetParent(null);
					this.AccessibilityObject.SetDetachableChild(null);
					this.AccessibilityObject.RaiseStructureChangedEvent(UnsafeNativeMethods.StructureChangeType.ChildRemoved, dataGridView.EditingControlAccessibleObject.RuntimeId);
				}
			}
			if (dataGridView.EditingPanel.ParentInternal != null)
			{
				((DataGridView.DataGridViewControlCollection)dataGridView.Controls).RemoveInternal(dataGridView.EditingPanel);
			}
			this.CurrentMouseLocation = 0;
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x000827E8 File Offset: 0x000809E8
		private bool AccessibleRestructuringNeeded
		{
			get
			{
				Type type = base.DataGridView.EditingControl.GetType();
				return (type == typeof(DataGridViewComboBoxEditingControl) && !type.IsSubclassOf(typeof(DataGridViewComboBoxEditingControl))) || (type == typeof(DataGridViewTextBoxEditingControl) && !type.IsSubclassOf(typeof(DataGridViewTextBoxEditingControl)));
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Forms.DataGridViewCell" />. </summary>
		// Token: 0x06001A30 RID: 6704 RVA: 0x00082853 File Offset: 0x00080A53
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.DataGridViewCell" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06001A31 RID: 6705 RVA: 0x00082864 File Offset: 0x00080A64
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ContextMenuStrip contextMenuStripInternal = this.ContextMenuStripInternal;
				if (contextMenuStripInternal != null)
				{
					contextMenuStripInternal.Disposed -= this.DetachContextMenuStrip;
				}
			}
		}

		/// <summary>Indicates whether the cell's row will be unshared when the cell is double-clicked.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> containing the data passed to the <see cref="M:System.Windows.Forms.DataGridViewCell.OnDoubleClick(System.Windows.Forms.DataGridViewCellEventArgs)" /> method.</param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A32 RID: 6706 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool DoubleClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return false;
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x00082890 File Offset: 0x00080A90
		internal bool DoubleClickUnsharesRowInternal(DataGridViewCellEventArgs e)
		{
			return this.DoubleClickUnsharesRow(e);
		}

		/// <summary>Indicates whether the parent row will be unshared when the focus moves to the cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if a user action moved focus to the cell; <see langword="false" /> if a programmatic operation moved focus to the cell.</param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared; otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A34 RID: 6708 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool EnterUnsharesRow(int rowIndex, bool throughMouseClick)
		{
			return false;
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x00082899 File Offset: 0x00080A99
		internal bool EnterUnsharesRowInternal(int rowIndex, bool throughMouseClick)
		{
			return this.EnterUnsharesRow(rowIndex, throughMouseClick);
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x000828A4 File Offset: 0x00080AA4
		internal static void FormatPlainText(string s, bool csv, TextWriter output, ref bool escapeApplied)
		{
			if (s == null)
			{
				return;
			}
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if (c != '\t')
				{
					if (c != '"')
					{
						if (c != ',')
						{
							output.Write(c);
						}
						else
						{
							if (csv)
							{
								escapeApplied = true;
							}
							output.Write(',');
						}
					}
					else if (csv)
					{
						output.Write("\"\"");
						escapeApplied = true;
					}
					else
					{
						output.Write('"');
					}
				}
				else if (!csv)
				{
					output.Write(' ');
				}
				else
				{
					output.Write('\t');
				}
			}
			if (escapeApplied)
			{
				output.Write('"');
			}
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x00082938 File Offset: 0x00080B38
		internal static void FormatPlainTextAsHtml(string s, TextWriter output)
		{
			if (s == null)
			{
				return;
			}
			int length = s.Length;
			char c = '\0';
			int i = 0;
			while (i < length)
			{
				char c2 = s[i];
				if (c2 <= ' ')
				{
					if (c2 != '\n')
					{
						if (c2 != '\r')
						{
							if (c2 != ' ')
							{
								goto IL_B7;
							}
							if (c == ' ')
							{
								output.Write("&nbsp;");
							}
							else
							{
								output.Write(c2);
							}
						}
					}
					else
					{
						output.Write("<br>");
					}
				}
				else if (c2 <= '&')
				{
					if (c2 != '"')
					{
						if (c2 != '&')
						{
							goto IL_B7;
						}
						output.Write("&amp;");
					}
					else
					{
						output.Write("&quot;");
					}
				}
				else if (c2 != '<')
				{
					if (c2 != '>')
					{
						goto IL_B7;
					}
					output.Write("&gt;");
				}
				else
				{
					output.Write("&lt;");
				}
				IL_F8:
				c = c2;
				i++;
				continue;
				IL_B7:
				if (c2 >= '\u00a0' && c2 < 'Ā')
				{
					output.Write("&#");
					int num = (int)c2;
					output.Write(num.ToString(NumberFormatInfo.InvariantInfo));
					output.Write(';');
					goto IL_F8;
				}
				output.Write(c2);
				goto IL_F8;
			}
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x00082A4C File Offset: 0x00080C4C
		private static Bitmap GetBitmap(string bitmapName)
		{
			Bitmap bitmap = new Bitmap(typeof(DataGridViewCell), bitmapName);
			bitmap.MakeTransparent();
			if (DpiHelper.IsScalingRequired)
			{
				Bitmap bitmap2 = DpiHelper.CreateResizedBitmap(bitmap, new Size((int)DataGridViewCell.iconsWidth, (int)DataGridViewCell.iconsHeight));
				if (bitmap2 != null)
				{
					bitmap.Dispose();
					bitmap = bitmap2;
				}
			}
			return bitmap;
		}

		/// <summary>Retrieves the formatted value of the cell to copy to the <see cref="T:System.Windows.Forms.Clipboard" />.</summary>
		/// <param name="rowIndex">The zero-based index of the row containing the cell.</param>
		/// <param name="firstCell">
		///       <see langword="true" /> to indicate that the cell is in the first column of the region defined by the selected cells; otherwise, <see langword="false" />.</param>
		/// <param name="lastCell">
		///       <see langword="true" /> to indicate that the cell is the last column of the region defined by the selected cells; otherwise, <see langword="false" />.</param>
		/// <param name="inFirstRow">
		///       <see langword="true" /> to indicate that the cell is in the first row of the region defined by the selected cells; otherwise, <see langword="false" />.</param>
		/// <param name="inLastRow">
		///       <see langword="true" /> to indicate that the cell is in the last row of the region defined by the selected cells; otherwise, <see langword="false" />.</param>
		/// <param name="format">The current format string of the cell.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the value of the cell to copy to the <see cref="T:System.Windows.Forms.Clipboard" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than 0 or greater than or equal to the number of rows in the control.</exception>
		/// <exception cref="T:System.InvalidOperationException">The value of the cell's <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property is <see langword="null" />.-or-
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		/// <exception cref="T:System.Exception">Formatting failed and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError" /> event of the <see cref="T:System.Windows.Forms.DataGridView" /> control or the handler set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException" /> property to <see langword="true" />. The exception object can typically be cast to type <see cref="T:System.FormatException" />.</exception>
		// Token: 0x06001A39 RID: 6713 RVA: 0x00082A9C File Offset: 0x00080C9C
		protected virtual object GetClipboardContent(int rowIndex, bool firstCell, bool lastCell, bool inFirstRow, bool inLastRow, string format)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			object obj = null;
			if (base.DataGridView.IsSharedCellSelected(this, rowIndex))
			{
				obj = this.GetEditedFormattedValue(this.GetValue(rowIndex), rowIndex, ref inheritedStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.ClipboardContent);
			}
			StringBuilder stringBuilder = new StringBuilder(64);
			if (string.Equals(format, DataFormats.Html, StringComparison.OrdinalIgnoreCase))
			{
				if (firstCell)
				{
					if (inFirstRow)
					{
						stringBuilder.Append("<TABLE>");
					}
					stringBuilder.Append("<TR>");
				}
				stringBuilder.Append("<TD>");
				if (obj != null)
				{
					DataGridViewCell.FormatPlainTextAsHtml(obj.ToString(), new StringWriter(stringBuilder, CultureInfo.CurrentCulture));
				}
				else
				{
					stringBuilder.Append("&nbsp;");
				}
				stringBuilder.Append("</TD>");
				if (lastCell)
				{
					stringBuilder.Append("</TR>");
					if (inLastRow)
					{
						stringBuilder.Append("</TABLE>");
					}
				}
				return stringBuilder.ToString();
			}
			bool flag = string.Equals(format, DataFormats.CommaSeparatedValue, StringComparison.OrdinalIgnoreCase);
			if (flag || string.Equals(format, DataFormats.Text, StringComparison.OrdinalIgnoreCase) || string.Equals(format, DataFormats.UnicodeText, StringComparison.OrdinalIgnoreCase))
			{
				if (obj != null)
				{
					if (firstCell && lastCell && inFirstRow && inLastRow)
					{
						stringBuilder.Append(obj.ToString());
					}
					else
					{
						bool flag2 = false;
						int length = stringBuilder.Length;
						DataGridViewCell.FormatPlainText(obj.ToString(), flag, new StringWriter(stringBuilder, CultureInfo.CurrentCulture), ref flag2);
						if (flag2)
						{
							stringBuilder.Insert(length, '"');
						}
					}
				}
				if (lastCell)
				{
					if (!inLastRow)
					{
						stringBuilder.Append('\r');
						stringBuilder.Append('\n');
					}
				}
				else
				{
					stringBuilder.Append(flag ? ',' : '\t');
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x00082C59 File Offset: 0x00080E59
		internal object GetClipboardContentInternal(int rowIndex, bool firstCell, bool lastCell, bool inFirstRow, bool inLastRow, string format)
		{
			return this.GetClipboardContent(rowIndex, firstCell, lastCell, inFirstRow, inLastRow, format);
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x00082C6C File Offset: 0x00080E6C
		internal ContextMenuStrip GetContextMenuStrip(int rowIndex)
		{
			ContextMenuStrip contextMenuStrip = this.ContextMenuStripInternal;
			if (base.DataGridView != null && (base.DataGridView.VirtualMode || base.DataGridView.DataSource != null))
			{
				contextMenuStrip = base.DataGridView.OnCellContextMenuStripNeeded(this.ColumnIndex, rowIndex, contextMenuStrip);
			}
			return contextMenuStrip;
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00082CB8 File Offset: 0x00080EB8
		internal void GetContrastedPens(Color baseline, ref Pen darkPen, ref Pen lightPen)
		{
			int num = DataGridViewCell.ColorDistance(baseline, SystemColors.ControlDark);
			int num2 = DataGridViewCell.ColorDistance(baseline, SystemColors.ControlLightLight);
			if (SystemInformation.HighContrast)
			{
				if (num < 2000)
				{
					darkPen = base.DataGridView.GetCachedPen(ControlPaint.DarkDark(baseline));
				}
				else
				{
					darkPen = base.DataGridView.GetCachedPen(SystemColors.ControlDark);
				}
				if (num2 < 2000)
				{
					lightPen = base.DataGridView.GetCachedPen(ControlPaint.LightLight(baseline));
					return;
				}
				lightPen = base.DataGridView.GetCachedPen(SystemColors.ControlLightLight);
				return;
			}
			else
			{
				if (num < 1000)
				{
					darkPen = base.DataGridView.GetCachedPen(ControlPaint.Dark(baseline));
				}
				else
				{
					darkPen = base.DataGridView.GetCachedPen(SystemColors.ControlDark);
				}
				if (num2 < 1000)
				{
					lightPen = base.DataGridView.GetCachedPen(ControlPaint.Light(baseline));
					return;
				}
				lightPen = base.DataGridView.GetCachedPen(SystemColors.ControlLightLight);
				return;
			}
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's content area using a default <see cref="T:System.Drawing.Graphics" /> and cell style currently in effect for the cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified <paramref name="rowIndex" /> is less than 0 or greater than the number of rows in the control minus 1. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		// Token: 0x06001A3D RID: 6717 RVA: 0x00082DA0 File Offset: 0x00080FA0
		public Rectangle GetContentBounds(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return Rectangle.Empty;
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			Rectangle contentBounds;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				contentBounds = this.GetContentBounds(graphics, inheritedStyle, rowIndex);
			}
			return contentBounds;
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		// Token: 0x06001A3E RID: 6718 RVA: 0x0004BFF9 File Offset: 0x0004A1F9
		protected virtual Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			return Rectangle.Empty;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x00082DF4 File Offset: 0x00080FF4
		internal object GetEditedFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle dataGridViewCellStyle, DataGridViewDataErrorContexts context)
		{
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (this.ColumnIndex != currentCellAddress.X || rowIndex != currentCellAddress.Y)
			{
				return this.GetFormattedValue(value, rowIndex, ref dataGridViewCellStyle, null, null, context);
			}
			IDataGridViewEditingControl dataGridViewEditingControl = (IDataGridViewEditingControl)base.DataGridView.EditingControl;
			if (dataGridViewEditingControl != null)
			{
				return dataGridViewEditingControl.GetEditingControlFormattedValue(context);
			}
			IDataGridViewEditingCell dataGridViewEditingCell = this as IDataGridViewEditingCell;
			if (dataGridViewEditingCell != null && base.DataGridView.IsCurrentCellInEditMode)
			{
				return dataGridViewEditingCell.GetEditingCellFormattedValue(context);
			}
			return this.GetFormattedValue(value, rowIndex, ref dataGridViewCellStyle, null, null, context);
		}

		/// <summary>Returns the current, formatted value of the cell, regardless of whether the cell is in edit mode and the value has not been committed.</summary>
		/// <param name="rowIndex">The row index of the cell.</param>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values that specifies the data error context.</param>
		/// <returns>The current, formatted value of the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified <paramref name="rowIndex" /> is less than 0 or greater than the number of rows in the control minus 1. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		/// <exception cref="T:System.Exception">Formatting failed and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError" /> event of the <see cref="T:System.Windows.Forms.DataGridView" /> control or the handler set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException" /> property to <see langword="true" />. The exception object can typically be cast to type <see cref="T:System.FormatException" />.</exception>
		// Token: 0x06001A40 RID: 6720 RVA: 0x00082E80 File Offset: 0x00081080
		public object GetEditedFormattedValue(int rowIndex, DataGridViewDataErrorContexts context)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			return this.GetEditedFormattedValue(this.GetValue(rowIndex), rowIndex, ref inheritedStyle, context);
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x00082EB4 File Offset: 0x000810B4
		internal Rectangle GetErrorIconBounds(int rowIndex)
		{
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			Rectangle errorIconBounds;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				errorIconBounds = this.GetErrorIconBounds(graphics, inheritedStyle, rowIndex);
			}
			return errorIconBounds;
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's error icon, if one is displayed.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's error icon, if one is displayed; otherwise, <see cref="F:System.Drawing.Rectangle.Empty" />.</returns>
		// Token: 0x06001A42 RID: 6722 RVA: 0x0004BFF9 File Offset: 0x0004A1F9
		protected virtual Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			return Rectangle.Empty;
		}

		/// <summary>Returns a string that represents the error for the cell.</summary>
		/// <param name="rowIndex">The row index of the cell.</param>
		/// <returns>A string that describes the error for the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x06001A43 RID: 6723 RVA: 0x00082EF8 File Offset: 0x000810F8
		protected internal virtual string GetErrorText(int rowIndex)
		{
			string text = string.Empty;
			object @object = this.Properties.GetObject(DataGridViewCell.PropCellErrorText);
			if (@object != null)
			{
				text = (string)@object;
			}
			else if (base.DataGridView != null && rowIndex != -1 && rowIndex != base.DataGridView.NewRowIndex && this.OwningColumn != null && this.OwningColumn.IsDataBound && base.DataGridView.DataConnection != null)
			{
				text = base.DataGridView.DataConnection.GetError(this.OwningColumn.BoundColumnIndex, this.ColumnIndex, rowIndex);
			}
			if (base.DataGridView != null && (base.DataGridView.VirtualMode || base.DataGridView.DataSource != null) && this.ColumnIndex >= 0 && rowIndex >= 0)
			{
				text = base.DataGridView.OnCellErrorTextNeeded(this.ColumnIndex, rowIndex, text);
			}
			return text;
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00082FCB File Offset: 0x000811CB
		internal object GetFormattedValue(int rowIndex, ref DataGridViewCellStyle cellStyle, DataGridViewDataErrorContexts context)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			return this.GetFormattedValue(this.GetValue(rowIndex), rowIndex, ref cellStyle, null, null, context);
		}

		/// <summary>Gets the value of the cell as formatted for display. </summary>
		/// <param name="value">The value to be formatted. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> in effect for the cell.</param>
		/// <param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> associated with the value type that provides custom conversion to the formatted value type, or <see langword="null" /> if no such custom conversion is needed.</param>
		/// <param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> associated with the formatted value type that provides custom conversion from the value type, or <see langword="null" /> if no such custom conversion is needed.</param>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values describing the context in which the formatted value is needed.</param>
		/// <returns>The formatted value of the cell or <see langword="null" /> if the cell does not belong to a <see cref="T:System.Windows.Forms.DataGridView" /> control.</returns>
		/// <exception cref="T:System.Exception">Formatting failed and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError" /> event of the <see cref="T:System.Windows.Forms.DataGridView" /> control or the handler set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException" /> property to <see langword="true" />. The exception object can typically be cast to type <see cref="T:System.FormatException" />.</exception>
		// Token: 0x06001A45 RID: 6725 RVA: 0x00082FEC File Offset: 0x000811EC
		protected virtual object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			DataGridViewCellFormattingEventArgs dataGridViewCellFormattingEventArgs = base.DataGridView.OnCellFormatting(this.ColumnIndex, rowIndex, value, this.FormattedValueType, cellStyle);
			cellStyle = dataGridViewCellFormattingEventArgs.CellStyle;
			bool formattingApplied = dataGridViewCellFormattingEventArgs.FormattingApplied;
			object obj = dataGridViewCellFormattingEventArgs.Value;
			bool flag = true;
			if (!formattingApplied && this.FormattedValueType != null && (obj == null || !this.FormattedValueType.IsAssignableFrom(obj.GetType())))
			{
				try
				{
					obj = Formatter.FormatObject(obj, this.FormattedValueType, (valueTypeConverter == null) ? this.ValueTypeConverter : valueTypeConverter, (formattedValueTypeConverter == null) ? this.FormattedValueTypeConverter : formattedValueTypeConverter, cellStyle.Format, cellStyle.FormatProvider, cellStyle.NullValue, cellStyle.DataSourceNullValue);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
					DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs = new DataGridViewDataErrorEventArgs(ex, this.ColumnIndex, rowIndex, context);
					base.RaiseDataError(dataGridViewDataErrorEventArgs);
					if (dataGridViewDataErrorEventArgs.ThrowException)
					{
						throw dataGridViewDataErrorEventArgs.Exception;
					}
					flag = false;
				}
			}
			if (flag && (obj == null || this.FormattedValueType == null || !this.FormattedValueType.IsAssignableFrom(obj.GetType())))
			{
				if (obj == null && cellStyle.NullValue == null && this.FormattedValueType != null && !typeof(ValueType).IsAssignableFrom(this.FormattedValueType))
				{
					return null;
				}
				Exception exception;
				if (this.FormattedValueType == null)
				{
					exception = new FormatException(SR.GetString("DataGridViewCell_FormattedValueTypeNull"));
				}
				else
				{
					exception = new FormatException(SR.GetString("DataGridViewCell_FormattedValueHasWrongType"));
				}
				DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs2 = new DataGridViewDataErrorEventArgs(exception, this.ColumnIndex, rowIndex, context);
				base.RaiseDataError(dataGridViewDataErrorEventArgs2);
				if (dataGridViewDataErrorEventArgs2.ThrowException)
				{
					throw dataGridViewDataErrorEventArgs2.Exception;
				}
			}
			return obj;
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x000831BC File Offset: 0x000813BC
		internal static DataGridViewFreeDimension GetFreeDimensionFromConstraint(Size constraintSize)
		{
			if (constraintSize.Width < 0 || constraintSize.Height < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"constraintSize",
					constraintSize.ToString()
				}));
			}
			if (constraintSize.Width == 0)
			{
				if (constraintSize.Height == 0)
				{
					return DataGridViewFreeDimension.Both;
				}
				return DataGridViewFreeDimension.Width;
			}
			else
			{
				if (constraintSize.Height == 0)
				{
					return DataGridViewFreeDimension.Height;
				}
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"constraintSize",
					constraintSize.ToString()
				}));
			}
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x00083259 File Offset: 0x00081459
		internal int GetHeight(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return -1;
			}
			return this.owningRow.GetHeight(rowIndex);
		}

		/// <summary>Gets the inherited shortcut menu for the current cell.</summary>
		/// <param name="rowIndex">The row index of the current cell.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.ContextMenuStrip" /> if the parent <see cref="T:System.Windows.Forms.DataGridView" />, <see cref="T:System.Windows.Forms.DataGridViewRow" />, or <see cref="T:System.Windows.Forms.DataGridViewColumn" /> has a <see cref="T:System.Windows.Forms.ContextMenuStrip" /> assigned; otherwise, <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the cell is not <see langword="null" /> and the specified <paramref name="rowIndex" /> is less than 0 or greater than the number of rows in the control minus 1. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		// Token: 0x06001A48 RID: 6728 RVA: 0x00083274 File Offset: 0x00081474
		public virtual ContextMenuStrip GetInheritedContextMenuStrip(int rowIndex)
		{
			if (base.DataGridView != null)
			{
				if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (this.ColumnIndex < 0)
				{
					throw new InvalidOperationException();
				}
			}
			ContextMenuStrip contextMenuStrip = this.GetContextMenuStrip(rowIndex);
			if (contextMenuStrip != null)
			{
				return contextMenuStrip;
			}
			if (this.owningRow != null)
			{
				contextMenuStrip = this.owningRow.GetContextMenuStrip(rowIndex);
				if (contextMenuStrip != null)
				{
					return contextMenuStrip;
				}
			}
			if (this.owningColumn != null)
			{
				contextMenuStrip = this.owningColumn.ContextMenuStrip;
				if (contextMenuStrip != null)
				{
					return contextMenuStrip;
				}
			}
			if (base.DataGridView != null)
			{
				return base.DataGridView.ContextMenuStrip;
			}
			return null;
		}

		/// <summary>Returns a value indicating the current state of the cell as inherited from the state of its row and column.</summary>
		/// <param name="rowIndex">The index of the row containing the cell.</param>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values representing the current state of the cell.</returns>
		/// <exception cref="T:System.ArgumentException">The cell is not contained within a <see cref="T:System.Windows.Forms.DataGridView" /> control and <paramref name="rowIndex" /> is not -1.-or-
		///         <paramref name="rowIndex" /> is not the index of the row containing this cell.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The cell is contained within a <see cref="T:System.Windows.Forms.DataGridView" /> control and <paramref name="rowIndex" /> is outside the valid range of 0 to the number of rows in the control minus 1.</exception>
		// Token: 0x06001A49 RID: 6729 RVA: 0x00083310 File Offset: 0x00081510
		public virtual DataGridViewElementStates GetInheritedState(int rowIndex)
		{
			DataGridViewElementStates dataGridViewElementStates = this.State | DataGridViewElementStates.ResizableSet;
			if (base.DataGridView == null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owningRow != null)
				{
					dataGridViewElementStates |= (this.owningRow.GetState(-1) & (DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible));
					if (this.owningRow.GetResizable(rowIndex) == DataGridViewTriState.True)
					{
						dataGridViewElementStates |= DataGridViewElementStates.Resizable;
					}
				}
				return dataGridViewElementStates;
			}
			else
			{
				if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (base.DataGridView.Rows.SharedRow(rowIndex) != this.owningRow)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				DataGridViewElementStates rowState = base.DataGridView.Rows.GetRowState(rowIndex);
				dataGridViewElementStates |= (rowState & (DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Selected));
				dataGridViewElementStates |= (this.owningColumn.State & (DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Selected));
				if (this.owningRow.GetResizable(rowIndex) == DataGridViewTriState.True || this.owningColumn.Resizable == DataGridViewTriState.True)
				{
					dataGridViewElementStates |= DataGridViewElementStates.Resizable;
				}
				if (this.owningColumn.Visible && this.owningRow.GetVisible(rowIndex))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Visible;
					if (this.owningColumn.Displayed && this.owningRow.GetDisplayed(rowIndex))
					{
						dataGridViewElementStates |= DataGridViewElementStates.Displayed;
					}
				}
				if (this.owningColumn.Frozen && this.owningRow.GetFrozen(rowIndex))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Frozen;
				}
				return dataGridViewElementStates;
			}
		}

		/// <summary>Gets the style applied to the cell. </summary>
		/// <param name="inheritedCellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be populated with the inherited cell style. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="includeColors">
		///       <see langword="true" /> to include inherited colors in the returned cell style; otherwise, <see langword="false" />. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that includes the style settings of the cell inherited from the cell's parent row, column, and <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The cell has no associated <see cref="T:System.Windows.Forms.DataGridView" />.-or-
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0, indicating that the cell is a row header cell.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than 0, or greater than or equal to the number of rows in the parent <see cref="T:System.Windows.Forms.DataGridView" />.</exception>
		// Token: 0x06001A4A RID: 6730 RVA: 0x000834A4 File Offset: 0x000816A4
		public virtual DataGridViewCellStyle GetInheritedStyle(DataGridViewCellStyle inheritedCellStyle, int rowIndex, bool includeColors)
		{
			if (base.DataGridView == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_CellNeedsDataGridViewForInheritedStyle"));
			}
			if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (this.ColumnIndex < 0)
			{
				throw new InvalidOperationException();
			}
			DataGridViewCellStyle dataGridViewCellStyle;
			if (inheritedCellStyle == null)
			{
				dataGridViewCellStyle = base.DataGridView.PlaceholderCellStyle;
				if (!includeColors)
				{
					dataGridViewCellStyle.BackColor = Color.Empty;
					dataGridViewCellStyle.ForeColor = Color.Empty;
					dataGridViewCellStyle.SelectionBackColor = Color.Empty;
					dataGridViewCellStyle.SelectionForeColor = Color.Empty;
				}
			}
			else
			{
				dataGridViewCellStyle = inheritedCellStyle;
			}
			DataGridViewCellStyle dataGridViewCellStyle2 = null;
			if (this.HasStyle)
			{
				dataGridViewCellStyle2 = this.Style;
			}
			DataGridViewCellStyle dataGridViewCellStyle3 = null;
			if (base.DataGridView.Rows.SharedRow(rowIndex).HasDefaultCellStyle)
			{
				dataGridViewCellStyle3 = base.DataGridView.Rows.SharedRow(rowIndex).DefaultCellStyle;
			}
			DataGridViewCellStyle dataGridViewCellStyle4 = null;
			if (this.owningColumn.HasDefaultCellStyle)
			{
				dataGridViewCellStyle4 = this.owningColumn.DefaultCellStyle;
			}
			DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
			if (includeColors)
			{
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle2.BackColor;
				}
				else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle3.BackColor;
				}
				else if (!base.DataGridView.RowsDefaultCellStyle.BackColor.IsEmpty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.BackColor.IsEmpty))
				{
					dataGridViewCellStyle.BackColor = base.DataGridView.RowsDefaultCellStyle.BackColor;
				}
				else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = base.DataGridView.AlternatingRowsDefaultCellStyle.BackColor;
				}
				else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle4.BackColor;
				}
				else
				{
					dataGridViewCellStyle.BackColor = defaultCellStyle.BackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle2.ForeColor;
				}
				else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle3.ForeColor;
				}
				else if (!base.DataGridView.RowsDefaultCellStyle.ForeColor.IsEmpty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.ForeColor.IsEmpty))
				{
					dataGridViewCellStyle.ForeColor = base.DataGridView.RowsDefaultCellStyle.ForeColor;
				}
				else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = base.DataGridView.AlternatingRowsDefaultCellStyle.ForeColor;
				}
				else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle4.ForeColor;
				}
				else
				{
					dataGridViewCellStyle.ForeColor = defaultCellStyle.ForeColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle2.SelectionBackColor;
				}
				else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle3.SelectionBackColor;
				}
				else if (!base.DataGridView.RowsDefaultCellStyle.SelectionBackColor.IsEmpty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor.IsEmpty))
				{
					dataGridViewCellStyle.SelectionBackColor = base.DataGridView.RowsDefaultCellStyle.SelectionBackColor;
				}
				else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor;
				}
				else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle4.SelectionBackColor;
				}
				else
				{
					dataGridViewCellStyle.SelectionBackColor = defaultCellStyle.SelectionBackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle2.SelectionForeColor;
				}
				else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle3.SelectionForeColor;
				}
				else if (!base.DataGridView.RowsDefaultCellStyle.SelectionForeColor.IsEmpty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor.IsEmpty))
				{
					dataGridViewCellStyle.SelectionForeColor = base.DataGridView.RowsDefaultCellStyle.SelectionForeColor;
				}
				else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor;
				}
				else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle4.SelectionForeColor;
				}
				else
				{
					dataGridViewCellStyle.SelectionForeColor = defaultCellStyle.SelectionForeColor;
				}
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Font != null)
			{
				dataGridViewCellStyle.Font = dataGridViewCellStyle2.Font;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Font != null)
			{
				dataGridViewCellStyle.Font = dataGridViewCellStyle3.Font;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Font != null && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Font == null))
			{
				dataGridViewCellStyle.Font = base.DataGridView.RowsDefaultCellStyle.Font;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Font != null)
			{
				dataGridViewCellStyle.Font = base.DataGridView.AlternatingRowsDefaultCellStyle.Font;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Font != null)
			{
				dataGridViewCellStyle.Font = dataGridViewCellStyle4.Font;
			}
			else
			{
				dataGridViewCellStyle.Font = defaultCellStyle.Font;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle2.NullValue;
			}
			else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle3.NullValue;
			}
			else if (!base.DataGridView.RowsDefaultCellStyle.IsNullValueDefault && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.IsNullValueDefault))
			{
				dataGridViewCellStyle.NullValue = base.DataGridView.RowsDefaultCellStyle.NullValue;
			}
			else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = base.DataGridView.AlternatingRowsDefaultCellStyle.NullValue;
			}
			else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle4.NullValue;
			}
			else
			{
				dataGridViewCellStyle.NullValue = defaultCellStyle.NullValue;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle2.DataSourceNullValue;
			}
			else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle3.DataSourceNullValue;
			}
			else if (!base.DataGridView.RowsDefaultCellStyle.IsDataSourceNullValueDefault && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.IsDataSourceNullValueDefault))
			{
				dataGridViewCellStyle.DataSourceNullValue = base.DataGridView.RowsDefaultCellStyle.DataSourceNullValue;
			}
			else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = base.DataGridView.AlternatingRowsDefaultCellStyle.DataSourceNullValue;
			}
			else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle4.DataSourceNullValue;
			}
			else
			{
				dataGridViewCellStyle.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle2.Format;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle3.Format;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Format.Length != 0 && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Format.Length == 0))
			{
				dataGridViewCellStyle.Format = base.DataGridView.RowsDefaultCellStyle.Format;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = base.DataGridView.AlternatingRowsDefaultCellStyle.Format;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle4.Format;
			}
			else
			{
				dataGridViewCellStyle.Format = defaultCellStyle.Format;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle2.FormatProvider;
			}
			else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle3.FormatProvider;
			}
			else if (!base.DataGridView.RowsDefaultCellStyle.IsFormatProviderDefault && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.IsFormatProviderDefault))
			{
				dataGridViewCellStyle.FormatProvider = base.DataGridView.RowsDefaultCellStyle.FormatProvider;
			}
			else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = base.DataGridView.AlternatingRowsDefaultCellStyle.FormatProvider;
			}
			else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle4.FormatProvider;
			}
			else
			{
				dataGridViewCellStyle.FormatProvider = defaultCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle2.Alignment;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle3.Alignment;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Alignment == DataGridViewContentAlignment.NotSet))
			{
				dataGridViewCellStyle.AlignmentInternal = base.DataGridView.RowsDefaultCellStyle.Alignment;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = base.DataGridView.AlternatingRowsDefaultCellStyle.Alignment;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle4.Alignment;
			}
			else
			{
				dataGridViewCellStyle.AlignmentInternal = defaultCellStyle.Alignment;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle2.WrapMode;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle3.WrapMode;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.WrapMode == DataGridViewTriState.NotSet))
			{
				dataGridViewCellStyle.WrapModeInternal = base.DataGridView.RowsDefaultCellStyle.WrapMode;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = base.DataGridView.AlternatingRowsDefaultCellStyle.WrapMode;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle4.WrapMode;
			}
			else
			{
				dataGridViewCellStyle.WrapModeInternal = defaultCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle2.Tag;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle3.Tag;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Tag != null && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Tag == null))
			{
				dataGridViewCellStyle.Tag = base.DataGridView.RowsDefaultCellStyle.Tag;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Tag != null)
			{
				dataGridViewCellStyle.Tag = base.DataGridView.AlternatingRowsDefaultCellStyle.Tag;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle4.Tag;
			}
			else
			{
				dataGridViewCellStyle.Tag = defaultCellStyle.Tag;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle2.Padding;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle3.Padding;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Padding != Padding.Empty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Padding == Padding.Empty))
			{
				dataGridViewCellStyle.PaddingInternal = base.DataGridView.RowsDefaultCellStyle.Padding;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = base.DataGridView.AlternatingRowsDefaultCellStyle.Padding;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle4.Padding;
			}
			else
			{
				dataGridViewCellStyle.PaddingInternal = defaultCellStyle.Padding;
			}
			return dataGridViewCellStyle;
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x00084162 File Offset: 0x00082362
		internal DataGridViewCellStyle GetInheritedStyleInternal(int rowIndex)
		{
			return this.GetInheritedStyle(null, rowIndex, true);
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x00084170 File Offset: 0x00082370
		internal int GetPreferredHeight(int rowIndex, int width)
		{
			if (base.DataGridView == null)
			{
				return -1;
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			int height;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				height = this.GetPreferredSize(graphics, inheritedStyle, rowIndex, new Size(width, 0)).Height;
			}
			return height;
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x000841D0 File Offset: 0x000823D0
		internal Size GetPreferredSize(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			Size preferredSize;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				preferredSize = this.GetPreferredSize(graphics, inheritedStyle, rowIndex, Size.Empty);
			}
			return preferredSize;
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		// Token: 0x06001A4E RID: 6734 RVA: 0x0008422C File Offset: 0x0008242C
		protected virtual Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			return new Size(-1, -1);
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x00084238 File Offset: 0x00082438
		internal static int GetPreferredTextHeight(Graphics g, bool rightToLeft, string text, DataGridViewCellStyle cellStyle, int maxWidth, out bool widthTruncated)
		{
			TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(rightToLeft, cellStyle.Alignment, cellStyle.WrapMode);
			if (cellStyle.WrapMode == DataGridViewTriState.True)
			{
				return DataGridViewCell.MeasureTextHeight(g, text, cellStyle.Font, maxWidth, textFormatFlags, out widthTruncated);
			}
			Size size = DataGridViewCell.MeasureTextSize(g, text, cellStyle.Font, textFormatFlags);
			widthTruncated = (size.Width > maxWidth);
			return size.Height;
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x00084298 File Offset: 0x00082498
		internal int GetPreferredWidth(int rowIndex, int height)
		{
			if (base.DataGridView == null)
			{
				return -1;
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			int width;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				width = this.GetPreferredSize(graphics, inheritedStyle, rowIndex, new Size(0, height)).Width;
			}
			return width;
		}

		/// <summary>Gets the size of the cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> representing the cell's dimensions.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="rowIndex" /> is -1</exception>
		// Token: 0x06001A51 RID: 6737 RVA: 0x000842F8 File Offset: 0x000824F8
		protected virtual Size GetSize(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			if (rowIndex == -1)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedCell", new object[]
				{
					"Size"
				}));
			}
			return new Size(this.owningColumn.Thickness, this.owningRow.GetHeight(rowIndex));
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x00084354 File Offset: 0x00082554
		private string GetToolTipText(int rowIndex)
		{
			string text = this.ToolTipTextInternal;
			if (base.DataGridView != null && (base.DataGridView.VirtualMode || base.DataGridView.DataSource != null))
			{
				text = base.DataGridView.OnCellToolTipTextNeeded(this.ColumnIndex, rowIndex, text);
			}
			return text;
		}

		/// <summary>Gets the value of the cell. </summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The value contained in the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the cell is not <see langword="null" /> and <paramref name="rowIndex" /> is less than 0 or greater than or equal to the number of rows in the parent <see cref="T:System.Windows.Forms.DataGridView" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the cell is not <see langword="null" /> and the value of the <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> property is less than 0, indicating that the cell is a row header cell.</exception>
		// Token: 0x06001A53 RID: 6739 RVA: 0x000843A0 File Offset: 0x000825A0
		protected virtual object GetValue(int rowIndex)
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView != null)
			{
				if (rowIndex < 0 || rowIndex >= dataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (this.ColumnIndex < 0)
				{
					throw new InvalidOperationException();
				}
			}
			if (dataGridView == null || (dataGridView.AllowUserToAddRowsInternal && rowIndex > -1 && rowIndex == dataGridView.NewRowIndex && rowIndex != dataGridView.CurrentCellAddress.Y) || (!dataGridView.VirtualMode && this.OwningColumn != null && !this.OwningColumn.IsDataBound) || rowIndex == -1 || this.ColumnIndex == -1)
			{
				return this.Properties.GetObject(DataGridViewCell.PropCellValue);
			}
			if (this.OwningColumn == null || !this.OwningColumn.IsDataBound)
			{
				return dataGridView.OnCellValueNeeded(this.ColumnIndex, rowIndex);
			}
			DataGridView.DataGridViewDataConnection dataConnection = dataGridView.DataConnection;
			if (dataConnection == null)
			{
				return null;
			}
			if (dataConnection.CurrencyManager.Count <= rowIndex)
			{
				return this.Properties.GetObject(DataGridViewCell.PropCellValue);
			}
			return dataConnection.GetValue(this.OwningColumn.BoundColumnIndex, this.ColumnIndex, rowIndex);
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x000844AD File Offset: 0x000826AD
		internal object GetValueInternal(int rowIndex)
		{
			return this.GetValue(rowIndex);
		}

		/// <summary>Initializes the control used to edit the cell.</summary>
		/// <param name="rowIndex">The zero-based row index of the cell's location.</param>
		/// <param name="initialFormattedValue">An <see cref="T:System.Object" /> that represents the value displayed by the cell when editing is started.</param>
		/// <param name="dataGridViewCellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <exception cref="T:System.InvalidOperationException">There is no associated <see cref="T:System.Windows.Forms.DataGridView" /> or if one is present, it does not have an associated editing control. </exception>
		// Token: 0x06001A55 RID: 6741 RVA: 0x000844B8 File Offset: 0x000826B8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null || dataGridView.EditingControl == null)
			{
				throw new InvalidOperationException();
			}
			if (dataGridView.EditingControl.ParentInternal == null)
			{
				dataGridView.EditingControl.CausesValidation = dataGridView.CausesValidation;
				dataGridView.EditingPanel.CausesValidation = dataGridView.CausesValidation;
				dataGridView.EditingControl.Visible = true;
				dataGridView.EditingPanel.Visible = false;
				dataGridView.Controls.Add(dataGridView.EditingPanel);
				dataGridView.EditingPanel.Controls.Add(dataGridView.EditingControl);
			}
			if (AccessibilityImprovements.Level3 && this.AccessibleRestructuringNeeded)
			{
				dataGridView.EditingControlAccessibleObject.SetParent(this.AccessibilityObject);
				this.AccessibilityObject.SetDetachableChild(dataGridView.EditingControl.AccessibilityObject);
				this.AccessibilityObject.RaiseStructureChangedEvent(UnsafeNativeMethods.StructureChangeType.ChildAdded, dataGridView.EditingControlAccessibleObject.RuntimeId);
			}
		}

		/// <summary>Indicates whether the parent row is unshared if the user presses a key while the focus is on the cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A56 RID: 6742 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool KeyDownUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return false;
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x00084599 File Offset: 0x00082799
		internal bool KeyDownUnsharesRowInternal(KeyEventArgs e, int rowIndex)
		{
			return this.KeyDownUnsharesRow(e, rowIndex);
		}

		/// <summary>Determines if edit mode should be started based on the given key.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that represents the key that was pressed.</param>
		/// <returns>
		///     <see langword="true" /> if edit mode should be started; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x06001A58 RID: 6744 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public virtual bool KeyEntersEditMode(KeyEventArgs e)
		{
			return false;
		}

		/// <summary>Indicates whether a row will be unshared if a key is pressed while a cell in the row has focus.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A59 RID: 6745 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool KeyPressUnsharesRow(KeyPressEventArgs e, int rowIndex)
		{
			return false;
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x000845A3 File Offset: 0x000827A3
		internal bool KeyPressUnsharesRowInternal(KeyPressEventArgs e, int rowIndex)
		{
			return this.KeyPressUnsharesRow(e, rowIndex);
		}

		/// <summary>Indicates whether the parent row is unshared when the user releases a key while the focus is on the cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A5B RID: 6747 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return false;
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x000845AD File Offset: 0x000827AD
		internal bool KeyUpUnsharesRowInternal(KeyEventArgs e, int rowIndex)
		{
			return this.KeyUpUnsharesRow(e, rowIndex);
		}

		/// <summary>Indicates whether a row will be unshared when the focus leaves a cell in the row.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if a user action moved focus to the cell; <see langword="false" /> if a programmatic operation moved focus to the cell.</param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A5D RID: 6749 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool LeaveUnsharesRow(int rowIndex, bool throughMouseClick)
		{
			return false;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x000845B7 File Offset: 0x000827B7
		internal bool LeaveUnsharesRowInternal(int rowIndex, bool throughMouseClick)
		{
			return this.LeaveUnsharesRow(rowIndex, throughMouseClick);
		}

		/// <summary>Gets the height, in pixels, of the specified text, given the specified characteristics.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to render the text.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> applied to the text.</param>
		/// <param name="maxWidth">The maximum width of the text.</param>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values to apply to the text.</param>
		/// <returns>The height, in pixels, of the text.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="graphics" /> is <see langword="null" />.-or-
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="maxWidth" /> is less than 1.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="flags" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values.</exception>
		// Token: 0x06001A5F RID: 6751 RVA: 0x000845C4 File Offset: 0x000827C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static int MeasureTextHeight(Graphics graphics, string text, Font font, int maxWidth, TextFormatFlags flags)
		{
			bool flag;
			return DataGridViewCell.MeasureTextHeight(graphics, text, font, maxWidth, flags, out flag);
		}

		/// <summary>Gets the height, in pixels, of the specified text, given the specified characteristics. Also indicates whether the required width is greater than the specified maximum width.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to render the text.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> applied to the text.</param>
		/// <param name="maxWidth">The maximum width of the text.</param>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values to apply to the text.</param>
		/// <param name="widthTruncated">Set to <see langword="true" /> if the required width of the text is greater than <paramref name="maxWidth" />.</param>
		/// <returns>The height, in pixels, of the text.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="graphics" /> is <see langword="null" />.-or-
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="maxWidth" /> is less than 1.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="flags" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values.</exception>
		// Token: 0x06001A60 RID: 6752 RVA: 0x000845E0 File Offset: 0x000827E0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static int MeasureTextHeight(Graphics graphics, string text, Font font, int maxWidth, TextFormatFlags flags, out bool widthTruncated)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			if (maxWidth <= 0)
			{
				throw new ArgumentOutOfRangeException("maxWidth", SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"maxWidth",
					maxWidth.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!DataGridViewUtilities.ValidTextFormatFlags(flags))
			{
				throw new InvalidEnumArgumentException("flags", (int)flags, typeof(TextFormatFlags));
			}
			flags &= (TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak);
			Size size = TextRenderer.MeasureText(text, font, new Size(maxWidth, int.MaxValue), flags);
			widthTruncated = (size.Width > maxWidth);
			return size.Height;
		}

		/// <summary>Gets the ideal height and width of the specified text given the specified characteristics.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to render the text.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> applied to the text.</param>
		/// <param name="maxRatio">The maximum width-to-height ratio of the block of text.</param>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values to apply to the text.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> representing the preferred height and width of the text.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="graphics" /> is <see langword="null" />.-or-
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="maxRatio" /> is less than or equal to 0.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="flags" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values.</exception>
		// Token: 0x06001A61 RID: 6753 RVA: 0x000846A4 File Offset: 0x000828A4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Size MeasureTextPreferredSize(Graphics graphics, string text, Font font, float maxRatio, TextFormatFlags flags)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			if (maxRatio <= 0f)
			{
				throw new ArgumentOutOfRangeException("maxRatio", SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"maxRatio",
					maxRatio.ToString(CultureInfo.CurrentCulture),
					"0.0"
				}));
			}
			if (!DataGridViewUtilities.ValidTextFormatFlags(flags))
			{
				throw new InvalidEnumArgumentException("flags", (int)flags, typeof(TextFormatFlags));
			}
			if (string.IsNullOrEmpty(text))
			{
				return new Size(0, 0);
			}
			Size result = DataGridViewCell.MeasureTextSize(graphics, text, font, flags);
			if ((float)(result.Width / result.Height) <= maxRatio)
			{
				return result;
			}
			flags &= (TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak);
			float num = (float)(result.Width * result.Width) / (float)result.Height / maxRatio * 1.1f;
			Size result2;
			for (;;)
			{
				result2 = TextRenderer.MeasureText(text, font, new Size((int)num, int.MaxValue), flags);
				if ((float)(result2.Width / result2.Height) <= maxRatio || result2.Width > (int)num)
				{
					break;
				}
				num = (float)result2.Width * 0.9f;
				if (num <= 1f)
				{
					return result2;
				}
			}
			return result2;
		}

		/// <summary>Gets the height and width of the specified text given the specified characteristics.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to render the text.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> applied to the text.</param>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values to apply to the text.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> representing the height and width of the text.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="graphics" /> is <see langword="null" />.-or-
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="flags" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values.</exception>
		// Token: 0x06001A62 RID: 6754 RVA: 0x000847DC File Offset: 0x000829DC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Size MeasureTextSize(Graphics graphics, string text, Font font, TextFormatFlags flags)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			if (!DataGridViewUtilities.ValidTextFormatFlags(flags))
			{
				throw new InvalidEnumArgumentException("flags", (int)flags, typeof(TextFormatFlags));
			}
			flags &= (TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak);
			return TextRenderer.MeasureText(text, font, new Size(int.MaxValue, int.MaxValue), flags);
		}

		/// <summary>Gets the width, in pixels, of the specified text given the specified characteristics.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to render the text.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> applied to the text.</param>
		/// <param name="maxHeight">The maximum height of the text.</param>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values to apply to the text.</param>
		/// <returns>The width, in pixels, of the text.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="graphics" /> is <see langword="null" />.-or-
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="maxHeight" /> is less than 1.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="flags" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" />  values.</exception>
		// Token: 0x06001A63 RID: 6755 RVA: 0x00084844 File Offset: 0x00082A44
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static int MeasureTextWidth(Graphics graphics, string text, Font font, int maxHeight, TextFormatFlags flags)
		{
			if (maxHeight <= 0)
			{
				throw new ArgumentOutOfRangeException("maxHeight", SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"maxHeight",
					maxHeight.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			Size size = DataGridViewCell.MeasureTextSize(graphics, text, font, flags);
			if (size.Height >= maxHeight || (flags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
			{
				return size.Width;
			}
			flags &= (TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak);
			int num = size.Width;
			float num2 = (float)num * 0.9f;
			for (;;)
			{
				Size size2 = TextRenderer.MeasureText(text, font, new Size((int)num2, maxHeight), flags);
				if (size2.Height > maxHeight || size2.Width > (int)num2)
				{
					break;
				}
				num = (int)num2;
				num2 = (float)size2.Width * 0.9f;
				if (num2 <= 1f)
				{
					return num;
				}
			}
			return num;
		}

		/// <summary>Indicates whether a row will be unshared if the user clicks a mouse button while the pointer is on a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data. </param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A64 RID: 6756 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool MouseClickUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0008491C File Offset: 0x00082B1C
		internal bool MouseClickUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseClickUnsharesRow(e);
		}

		/// <summary>Indicates whether a row will be unshared if the user double-clicks a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A66 RID: 6758 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool MouseDoubleClickUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00084925 File Offset: 0x00082B25
		internal bool MouseDoubleClickUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseDoubleClickUnsharesRow(e);
		}

		/// <summary>Indicates whether a row will be unshared when the user holds down a mouse button while the pointer is on a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data. </param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A68 RID: 6760 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0008492E File Offset: 0x00082B2E
		internal bool MouseDownUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseDownUnsharesRow(e);
		}

		/// <summary>Indicates whether a row will be unshared when the mouse pointer moves over a cell in the row.</summary>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A6A RID: 6762 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool MouseEnterUnsharesRow(int rowIndex)
		{
			return false;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00084937 File Offset: 0x00082B37
		internal bool MouseEnterUnsharesRowInternal(int rowIndex)
		{
			return this.MouseEnterUnsharesRow(rowIndex);
		}

		/// <summary>Indicates whether a row will be unshared when the mouse pointer leaves the row.</summary>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A6C RID: 6764 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return false;
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x00084940 File Offset: 0x00082B40
		internal bool MouseLeaveUnsharesRowInternal(int rowIndex)
		{
			return this.MouseLeaveUnsharesRow(rowIndex);
		}

		/// <summary>Indicates whether a row will be unshared when the mouse pointer moves over a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data. </param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A6E RID: 6766 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool MouseMoveUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00084949 File Offset: 0x00082B49
		internal bool MouseMoveUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseMoveUnsharesRow(e);
		}

		/// <summary>Indicates whether a row will be unshared when the user releases a mouse button while the pointer is on a cell in the row.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data. </param>
		/// <returns>
		///     <see langword="true" /> if the row will be unshared, otherwise, <see langword="false" />. The base <see cref="T:System.Windows.Forms.DataGridViewCell" /> class always returns <see langword="false" />.</returns>
		// Token: 0x06001A70 RID: 6768 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected virtual bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00084952 File Offset: 0x00082B52
		internal bool MouseUpUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseUpUnsharesRow(e);
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0008495C File Offset: 0x00082B5C
		private void OnCellDataAreaMouseEnterInternal(int rowIndex)
		{
			if (!base.DataGridView.ShowCellToolTips)
			{
				return;
			}
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X != -1 && currentCellAddress.X == this.ColumnIndex && currentCellAddress.Y == rowIndex && base.DataGridView.EditingControl != null)
			{
				return;
			}
			string text = this.GetToolTipText(rowIndex);
			if (string.IsNullOrEmpty(text))
			{
				if (!(this.FormattedValueType == DataGridViewCell.stringType))
				{
					goto IL_1E5;
				}
				if (rowIndex != -1 && this.OwningColumn != null)
				{
					int preferredWidth = this.GetPreferredWidth(rowIndex, this.OwningRow.Height);
					int preferredHeight = this.GetPreferredHeight(rowIndex, this.OwningColumn.Width);
					if (this.OwningColumn.Width >= preferredWidth && this.OwningRow.Height >= preferredHeight)
					{
						goto IL_1E5;
					}
					DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
					string text2 = this.GetEditedFormattedValue(this.GetValue(rowIndex), rowIndex, ref inheritedStyle, DataGridViewDataErrorContexts.Display) as string;
					if (!string.IsNullOrEmpty(text2))
					{
						text = DataGridViewCell.TruncateToolTipText(text2);
						goto IL_1E5;
					}
					goto IL_1E5;
				}
				else
				{
					if ((rowIndex == -1 || this.OwningRow == null || !base.DataGridView.RowHeadersVisible || base.DataGridView.RowHeadersWidth <= 0 || this.OwningColumn != null) && rowIndex != -1)
					{
						goto IL_1E5;
					}
					string text3 = this.GetValue(rowIndex) as string;
					if (string.IsNullOrEmpty(text3))
					{
						goto IL_1E5;
					}
					DataGridViewCellStyle inheritedStyle2 = this.GetInheritedStyle(null, rowIndex, false);
					using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
					{
						Rectangle contentBounds = this.GetContentBounds(graphics, inheritedStyle2, rowIndex);
						bool flag = false;
						int num = 0;
						if (contentBounds.Width > 0)
						{
							num = DataGridViewCell.GetPreferredTextHeight(graphics, base.DataGridView.RightToLeftInternal, text3, inheritedStyle2, contentBounds.Width, out flag);
						}
						else
						{
							flag = true;
						}
						if (num > contentBounds.Height || flag)
						{
							text = DataGridViewCell.TruncateToolTipText(text3);
						}
						goto IL_1E5;
					}
				}
			}
			if (base.DataGridView.IsRestricted)
			{
				text = DataGridViewCell.TruncateToolTipText(text);
			}
			IL_1E5:
			if (!string.IsNullOrEmpty(text))
			{
				base.DataGridView.ActivateToolTip(true, text, this.ColumnIndex, rowIndex);
			}
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00084B7C File Offset: 0x00082D7C
		private void OnCellDataAreaMouseLeaveInternal()
		{
			if (base.DataGridView.IsDisposed)
			{
				return;
			}
			base.DataGridView.ActivateToolTip(false, string.Empty, -1, -1);
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00084BA0 File Offset: 0x00082DA0
		private void OnCellErrorAreaMouseEnterInternal(int rowIndex)
		{
			string errorText = this.GetErrorText(rowIndex);
			base.DataGridView.ActivateToolTip(true, errorText, this.ColumnIndex, rowIndex);
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00084BC9 File Offset: 0x00082DC9
		private void OnCellErrorAreaMouseLeaveInternal()
		{
			base.DataGridView.ActivateToolTip(false, string.Empty, -1, -1);
		}

		/// <summary>Called when the cell is clicked.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data. </param>
		// Token: 0x06001A76 RID: 6774 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnClick(DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x00084BDE File Offset: 0x00082DDE
		internal void OnClickInternal(DataGridViewCellEventArgs e)
		{
			this.OnClick(e);
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x00084BE8 File Offset: 0x00082DE8
		internal void OnCommonChange()
		{
			if (base.DataGridView != null && !base.DataGridView.IsDisposed && !base.DataGridView.Disposing)
			{
				if (this.RowIndex == -1)
				{
					base.DataGridView.OnColumnCommonChange(this.ColumnIndex);
					return;
				}
				base.DataGridView.OnCellCommonChange(this.ColumnIndex, this.RowIndex);
			}
		}

		/// <summary>Called when the cell's contents are clicked.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data. </param>
		// Token: 0x06001A79 RID: 6777 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnContentClick(DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x00084C49 File Offset: 0x00082E49
		internal void OnContentClickInternal(DataGridViewCellEventArgs e)
		{
			this.OnContentClick(e);
		}

		/// <summary>Called when the cell's contents are double-clicked.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data. </param>
		// Token: 0x06001A7B RID: 6779 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnContentDoubleClick(DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00084C52 File Offset: 0x00082E52
		internal void OnContentDoubleClickInternal(DataGridViewCellEventArgs e)
		{
			this.OnContentDoubleClick(e);
		}

		/// <summary>Called when the cell is double-clicked.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data. </param>
		// Token: 0x06001A7D RID: 6781 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnDoubleClick(DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00084C5B File Offset: 0x00082E5B
		internal void OnDoubleClickInternal(DataGridViewCellEventArgs e)
		{
			this.OnDoubleClick(e);
		}

		/// <summary>Called when the focus moves to a cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if a user action moved focus to the cell; <see langword="false" /> if a programmatic operation moved focus to the cell.</param>
		// Token: 0x06001A7F RID: 6783 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnEnter(int rowIndex, bool throughMouseClick)
		{
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00084C64 File Offset: 0x00082E64
		internal void OnEnterInternal(int rowIndex, bool throughMouseClick)
		{
			this.OnEnter(rowIndex, throughMouseClick);
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x00084C6E File Offset: 0x00082E6E
		internal void OnKeyDownInternal(KeyEventArgs e, int rowIndex)
		{
			this.OnKeyDown(e, rowIndex);
		}

		/// <summary>Called when a character key is pressed while the focus is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		// Token: 0x06001A82 RID: 6786 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnKeyDown(KeyEventArgs e, int rowIndex)
		{
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x00084C78 File Offset: 0x00082E78
		internal void OnKeyPressInternal(KeyPressEventArgs e, int rowIndex)
		{
			this.OnKeyPress(e, rowIndex);
		}

		/// <summary>Called when a key is pressed while the focus is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		// Token: 0x06001A84 RID: 6788 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnKeyPress(KeyPressEventArgs e, int rowIndex)
		{
		}

		/// <summary>Called when a character key is released while the focus is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		// Token: 0x06001A85 RID: 6789 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnKeyUp(KeyEventArgs e, int rowIndex)
		{
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x00084C82 File Offset: 0x00082E82
		internal void OnKeyUpInternal(KeyEventArgs e, int rowIndex)
		{
			this.OnKeyUp(e, rowIndex);
		}

		/// <summary>Called when the focus moves from a cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if a user action moved focus from the cell; <see langword="false" /> if a programmatic operation moved focus from the cell.</param>
		// Token: 0x06001A87 RID: 6791 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnLeave(int rowIndex, bool throughMouseClick)
		{
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x00084C8C File Offset: 0x00082E8C
		internal void OnLeaveInternal(int rowIndex, bool throughMouseClick)
		{
			this.OnLeave(rowIndex, throughMouseClick);
		}

		/// <summary>Called when the user clicks a mouse button while the pointer is on a cell.  </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06001A89 RID: 6793 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x00084C96 File Offset: 0x00082E96
		internal void OnMouseClickInternal(DataGridViewCellMouseEventArgs e)
		{
			this.OnMouseClick(e);
		}

		/// <summary>Called when the user double-clicks a mouse button while the pointer is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06001A8B RID: 6795 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseDoubleClick(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x00084C9F File Offset: 0x00082E9F
		internal void OnMouseDoubleClickInternal(DataGridViewCellMouseEventArgs e)
		{
			this.OnMouseDoubleClick(e);
		}

		/// <summary>Called when the user holds down a mouse button while the pointer is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06001A8D RID: 6797 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x00084CA8 File Offset: 0x00082EA8
		internal void OnMouseDownInternal(DataGridViewCellMouseEventArgs e)
		{
			base.DataGridView.CellMouseDownInContentBounds = this.GetContentBounds(e.RowIndex).Contains(e.X, e.Y);
			if (((this.ColumnIndex < 0 || e.RowIndex < 0) && base.DataGridView.ApplyVisualStylesToHeaderCells) || (this.ColumnIndex >= 0 && e.RowIndex >= 0 && base.DataGridView.ApplyVisualStylesToInnerCells))
			{
				base.DataGridView.InvalidateCell(this.ColumnIndex, e.RowIndex);
			}
			this.OnMouseDown(e);
		}

		/// <summary>Called when the mouse pointer moves over a cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		// Token: 0x06001A8F RID: 6799 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseEnter(int rowIndex)
		{
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x00084D3C File Offset: 0x00082F3C
		internal void OnMouseEnterInternal(int rowIndex)
		{
			this.OnMouseEnter(rowIndex);
		}

		/// <summary>Called when the mouse pointer leaves the cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		// Token: 0x06001A91 RID: 6801 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseLeave(int rowIndex)
		{
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x00084D48 File Offset: 0x00082F48
		internal void OnMouseLeaveInternal(int rowIndex)
		{
			switch (this.CurrentMouseLocation)
			{
			case 1:
				this.OnCellDataAreaMouseLeaveInternal();
				break;
			case 2:
				this.OnCellErrorAreaMouseLeaveInternal();
				break;
			}
			this.CurrentMouseLocation = 0;
			this.OnMouseLeave(rowIndex);
		}

		/// <summary>Called when the mouse pointer moves within a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06001A93 RID: 6803 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00084D8C File Offset: 0x00082F8C
		internal void OnMouseMoveInternal(DataGridViewCellMouseEventArgs e)
		{
			byte currentMouseLocation = this.CurrentMouseLocation;
			this.UpdateCurrentMouseLocation(e);
			switch (currentMouseLocation)
			{
			case 0:
				if (this.CurrentMouseLocation == 1)
				{
					this.OnCellDataAreaMouseEnterInternal(e.RowIndex);
				}
				else
				{
					this.OnCellErrorAreaMouseEnterInternal(e.RowIndex);
				}
				break;
			case 1:
				if (this.CurrentMouseLocation == 2)
				{
					this.OnCellDataAreaMouseLeaveInternal();
					this.OnCellErrorAreaMouseEnterInternal(e.RowIndex);
				}
				break;
			case 2:
				if (this.CurrentMouseLocation == 1)
				{
					this.OnCellErrorAreaMouseLeaveInternal();
					this.OnCellDataAreaMouseEnterInternal(e.RowIndex);
				}
				break;
			}
			this.OnMouseMove(e);
		}

		/// <summary>Called when the user releases a mouse button while the pointer is on a cell. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06001A95 RID: 6805 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x00084E20 File Offset: 0x00083020
		internal void OnMouseUpInternal(DataGridViewCellMouseEventArgs e)
		{
			int x = e.X;
			int y = e.Y;
			if (((this.ColumnIndex < 0 || e.RowIndex < 0) && base.DataGridView.ApplyVisualStylesToHeaderCells) || (this.ColumnIndex >= 0 && e.RowIndex >= 0 && base.DataGridView.ApplyVisualStylesToInnerCells))
			{
				base.DataGridView.InvalidateCell(this.ColumnIndex, e.RowIndex);
			}
			if (e.Button == MouseButtons.Left && this.GetContentBounds(e.RowIndex).Contains(x, y))
			{
				base.DataGridView.OnCommonCellContentClick(e.ColumnIndex, e.RowIndex, e.Clicks > 1);
			}
			if (base.DataGridView != null && e.ColumnIndex < base.DataGridView.Columns.Count && e.RowIndex < base.DataGridView.Rows.Count)
			{
				this.OnMouseUp(e);
			}
		}

		/// <summary>Called when the <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the cell changes.</summary>
		// Token: 0x06001A97 RID: 6807 RVA: 0x00084F14 File Offset: 0x00083114
		protected override void OnDataGridViewChanged()
		{
			if (this.HasStyle)
			{
				if (base.DataGridView == null)
				{
					this.Style.RemoveScope(DataGridViewCellStyleScopes.Cell);
				}
				else
				{
					this.Style.AddScope(base.DataGridView, DataGridViewCellStyleScopes.Cell);
				}
			}
			base.OnDataGridViewChanged();
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
		// Token: 0x06001A98 RID: 6808 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00084F4C File Offset: 0x0008314C
		internal void PaintInternal(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			this.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x00084F72 File Offset: 0x00083172
		internal static bool PaintBackground(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.Background) > DataGridViewPaintParts.None;
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00084F7A File Offset: 0x0008317A
		internal static bool PaintBorder(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.Border) > DataGridViewPaintParts.None;
		}

		/// <summary>Paints the border of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the border.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the area of the border that is being painted.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the current cell.</param>
		/// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that contains border styles of the border that is being painted.</param>
		// Token: 0x06001A9C RID: 6812 RVA: 0x00084F84 File Offset: 0x00083184
		protected virtual void PaintBorder(Graphics graphics, Rectangle clipBounds, Rectangle bounds, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null)
			{
				return;
			}
			Pen pen = null;
			Pen pen2 = null;
			Pen cachedPen = base.DataGridView.GetCachedPen(cellStyle.BackColor);
			Pen gridPen = base.DataGridView.GridPen;
			this.GetContrastedPens(cellStyle.BackColor, ref pen, ref pen2);
			int num = (this.owningColumn == null) ? 0 : this.owningColumn.DividerWidth;
			if (num != 0)
			{
				if (num > bounds.Width)
				{
					num = bounds.Width;
				}
				DataGridViewAdvancedCellBorderStyle dataGridViewAdvancedCellBorderStyle = advancedBorderStyle.Right;
				Color color;
				if (dataGridViewAdvancedCellBorderStyle != DataGridViewAdvancedCellBorderStyle.Single)
				{
					if (dataGridViewAdvancedCellBorderStyle != DataGridViewAdvancedCellBorderStyle.Inset)
					{
						color = SystemColors.ControlDark;
					}
					else
					{
						color = SystemColors.ControlLightLight;
					}
				}
				else
				{
					color = base.DataGridView.GridPen.Color;
				}
				graphics.FillRectangle(base.DataGridView.GetCachedBrush(color), base.DataGridView.RightToLeftInternal ? bounds.X : (bounds.Right - num), bounds.Y, num, bounds.Height);
				if (base.DataGridView.RightToLeftInternal)
				{
					bounds.X += num;
				}
				bounds.Width -= num;
				if (bounds.Width <= 0)
				{
					return;
				}
			}
			num = ((this.owningRow == null) ? 0 : this.owningRow.DividerHeight);
			if (num != 0)
			{
				if (num > bounds.Height)
				{
					num = bounds.Height;
				}
				DataGridViewAdvancedCellBorderStyle dataGridViewAdvancedCellBorderStyle = advancedBorderStyle.Bottom;
				Color color2;
				if (dataGridViewAdvancedCellBorderStyle != DataGridViewAdvancedCellBorderStyle.Single)
				{
					if (dataGridViewAdvancedCellBorderStyle != DataGridViewAdvancedCellBorderStyle.Inset)
					{
						color2 = SystemColors.ControlDark;
					}
					else
					{
						color2 = SystemColors.ControlLightLight;
					}
				}
				else
				{
					color2 = base.DataGridView.GridPen.Color;
				}
				graphics.FillRectangle(base.DataGridView.GetCachedBrush(color2), bounds.X, bounds.Bottom - num, bounds.Width, num);
				bounds.Height -= num;
				if (bounds.Height <= 0)
				{
					return;
				}
			}
			if (advancedBorderStyle.All == DataGridViewAdvancedCellBorderStyle.None)
			{
				return;
			}
			switch (advancedBorderStyle.Left)
			{
			case DataGridViewAdvancedCellBorderStyle.Single:
				graphics.DrawLine(gridPen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.Inset:
				graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.InsetDouble:
			{
				int num2 = bounds.Y + 1;
				int num3 = bounds.Bottom - 1;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetPartial)
				{
					num3++;
				}
				graphics.DrawLine(pen2, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.X + 1, num2, bounds.X + 1, num3);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.Outset:
				graphics.DrawLine(pen2, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
			{
				int num2 = bounds.Y + 1;
				int num3 = bounds.Bottom - 1;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetPartial)
				{
					num3++;
				}
				graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				graphics.DrawLine(pen2, bounds.X + 1, num2, bounds.X + 1, num3);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
			{
				int num2 = bounds.Y + 2;
				int num3 = bounds.Bottom - 3;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num2++;
				}
				else if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				graphics.DrawLine(cachedPen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				graphics.DrawLine(pen2, bounds.X, num2, bounds.X, num3);
				break;
			}
			}
			switch (advancedBorderStyle.Right)
			{
			case DataGridViewAdvancedCellBorderStyle.Single:
				graphics.DrawLine(gridPen, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.Inset:
				graphics.DrawLine(pen2, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.InsetDouble:
			{
				int num2 = bounds.Y + 1;
				int num3 = bounds.Bottom - 1;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.Inset)
				{
					num3++;
				}
				graphics.DrawLine(pen2, bounds.Right - 2, bounds.Y, bounds.Right - 2, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.Right - 1, num2, bounds.Right - 1, num3);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.Outset:
				graphics.DrawLine(pen, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
			{
				int num2 = bounds.Y + 1;
				int num3 = bounds.Bottom - 1;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetPartial)
				{
					num3++;
				}
				graphics.DrawLine(pen, bounds.Right - 2, bounds.Y, bounds.Right - 2, bounds.Bottom - 1);
				graphics.DrawLine(pen2, bounds.Right - 1, num2, bounds.Right - 1, num3);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
			{
				int num2 = bounds.Y + 2;
				int num3 = bounds.Bottom - 3;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num2++;
				}
				else if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				graphics.DrawLine(cachedPen, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.Right - 1, num2, bounds.Right - 1, num3);
				break;
			}
			}
			switch (advancedBorderStyle.Top)
			{
			case DataGridViewAdvancedCellBorderStyle.Single:
				graphics.DrawLine(gridPen, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
				break;
			case DataGridViewAdvancedCellBorderStyle.Inset:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num4++;
				}
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.Inset || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.Outset)
				{
					num5--;
				}
				graphics.DrawLine(pen, num4, bounds.Y, num5, bounds.Y);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.InsetDouble:
			{
				int num4 = bounds.X;
				if (advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.OutsetPartial && advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.None)
				{
					num4++;
				}
				int num5 = bounds.Right - 2;
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.None)
				{
					num5++;
				}
				graphics.DrawLine(pen2, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
				graphics.DrawLine(pen, num4, bounds.Y + 1, num5, bounds.Y + 1);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.Outset:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num4++;
				}
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.Inset || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.Outset)
				{
					num5--;
				}
				graphics.DrawLine(pen2, num4, bounds.Y, num5, bounds.Y);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
			{
				int num4 = bounds.X;
				if (advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.OutsetPartial && advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.None)
				{
					num4++;
				}
				int num5 = bounds.Right - 2;
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.None)
				{
					num5++;
				}
				graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
				graphics.DrawLine(pen2, num4, bounds.Y + 1, num5, bounds.Y + 1);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.None)
				{
					num4++;
					if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
					{
						num4++;
					}
				}
				if (advancedBorderStyle.Right != DataGridViewAdvancedCellBorderStyle.None)
				{
					num5--;
					if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble)
					{
						num5--;
					}
				}
				graphics.DrawLine(cachedPen, num4, bounds.Y, num5, bounds.Y);
				graphics.DrawLine(pen2, num4 + 1, bounds.Y, num5 - 1, bounds.Y);
				break;
			}
			}
			switch (advancedBorderStyle.Bottom)
			{
			case DataGridViewAdvancedCellBorderStyle.Single:
				graphics.DrawLine(gridPen, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
				return;
			case DataGridViewAdvancedCellBorderStyle.Inset:
			{
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num5--;
				}
				graphics.DrawLine(pen2, bounds.X, bounds.Bottom - 1, num5, bounds.Bottom - 1);
				return;
			}
			case DataGridViewAdvancedCellBorderStyle.InsetDouble:
			case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
				break;
			case DataGridViewAdvancedCellBorderStyle.Outset:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
				{
					num5--;
				}
				graphics.DrawLine(pen, num4, bounds.Bottom - 1, num5, bounds.Bottom - 1);
				return;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.None)
				{
					num4++;
					if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
					{
						num4++;
					}
				}
				if (advancedBorderStyle.Right != DataGridViewAdvancedCellBorderStyle.None)
				{
					num5--;
					if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble)
					{
						num5--;
					}
				}
				graphics.DrawLine(cachedPen, num4, bounds.Bottom - 1, num5, bounds.Bottom - 1);
				graphics.DrawLine(pen, num4 + 1, bounds.Bottom - 1, num5 - 1, bounds.Bottom - 1);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00085A6A File Offset: 0x00083C6A
		internal static bool PaintContentBackground(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.ContentBackground) > DataGridViewPaintParts.None;
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00085A72 File Offset: 0x00083C72
		internal static bool PaintContentForeground(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.ContentForeground) > DataGridViewPaintParts.None;
		}

		/// <summary>Paints the error icon of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the border.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
		/// <param name="cellValueBounds">The bounding <see cref="T:System.Drawing.Rectangle" /> that encloses the cell's content area.</param>
		/// <param name="errorText">An error message that is associated with the cell.</param>
		// Token: 0x06001A9F RID: 6815 RVA: 0x00085A7A File Offset: 0x00083C7A
		protected virtual void PaintErrorIcon(Graphics graphics, Rectangle clipBounds, Rectangle cellValueBounds, string errorText)
		{
			if (!string.IsNullOrEmpty(errorText) && cellValueBounds.Width >= (int)(8 + DataGridViewCell.iconsWidth) && cellValueBounds.Height >= (int)(8 + DataGridViewCell.iconsHeight))
			{
				DataGridViewCell.PaintErrorIcon(graphics, this.ComputeErrorIconBounds(cellValueBounds));
			}
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00085AB4 File Offset: 0x00083CB4
		private static void PaintErrorIcon(Graphics graphics, Rectangle iconBounds)
		{
			Bitmap errorBitmap = DataGridViewCell.ErrorBitmap;
			if (errorBitmap != null)
			{
				Bitmap obj = errorBitmap;
				lock (obj)
				{
					graphics.DrawImage(errorBitmap, iconBounds, 0, 0, (int)DataGridViewCell.iconsWidth, (int)DataGridViewCell.iconsHeight, GraphicsUnit.Pixel);
				}
			}
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00085B08 File Offset: 0x00083D08
		internal void PaintErrorIcon(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Rectangle cellBounds, Rectangle cellValueBounds, string errorText)
		{
			if (!string.IsNullOrEmpty(errorText) && cellValueBounds.Width >= (int)(8 + DataGridViewCell.iconsWidth) && cellValueBounds.Height >= (int)(8 + DataGridViewCell.iconsHeight))
			{
				Rectangle errorIconBounds = this.GetErrorIconBounds(graphics, cellStyle, rowIndex);
				if (errorIconBounds.Width >= 4 && errorIconBounds.Height >= (int)DataGridViewCell.iconsHeight)
				{
					errorIconBounds.X += cellBounds.X;
					errorIconBounds.Y += cellBounds.Y;
					DataGridViewCell.PaintErrorIcon(graphics, errorIconBounds);
				}
			}
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x00085B91 File Offset: 0x00083D91
		internal static bool PaintErrorIcon(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.ErrorIcon) > DataGridViewPaintParts.None;
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00085B9A File Offset: 0x00083D9A
		internal static bool PaintFocus(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.Focus) > DataGridViewPaintParts.None;
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x00085BA4 File Offset: 0x00083DA4
		internal static void PaintPadding(Graphics graphics, Rectangle bounds, DataGridViewCellStyle cellStyle, Brush br, bool rightToLeft)
		{
			Rectangle rect;
			if (rightToLeft)
			{
				rect = new Rectangle(bounds.X, bounds.Y, cellStyle.Padding.Right, bounds.Height);
				graphics.FillRectangle(br, rect);
				rect.X = bounds.Right - cellStyle.Padding.Left;
				rect.Width = cellStyle.Padding.Left;
				graphics.FillRectangle(br, rect);
				rect.X = bounds.Left + cellStyle.Padding.Right;
			}
			else
			{
				rect = new Rectangle(bounds.X, bounds.Y, cellStyle.Padding.Left, bounds.Height);
				graphics.FillRectangle(br, rect);
				rect.X = bounds.Right - cellStyle.Padding.Right;
				rect.Width = cellStyle.Padding.Right;
				graphics.FillRectangle(br, rect);
				rect.X = bounds.Left + cellStyle.Padding.Left;
			}
			rect.Y = bounds.Y;
			rect.Width = bounds.Width - cellStyle.Padding.Horizontal;
			rect.Height = cellStyle.Padding.Top;
			graphics.FillRectangle(br, rect);
			rect.Y = bounds.Bottom - cellStyle.Padding.Bottom;
			rect.Height = cellStyle.Padding.Bottom;
			graphics.FillRectangle(br, rect);
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00085D51 File Offset: 0x00083F51
		internal static bool PaintSelectionBackground(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.SelectionBackground) > DataGridViewPaintParts.None;
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x00085D5C File Offset: 0x00083F5C
		internal void PaintWork(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			DataGridView dataGridView = base.DataGridView;
			int columnIndex = this.ColumnIndex;
			object value = this.GetValue(rowIndex);
			string errorText = this.GetErrorText(rowIndex);
			object formattedValue;
			if (columnIndex > -1 && rowIndex > -1)
			{
				formattedValue = this.GetEditedFormattedValue(value, rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.Display);
			}
			else
			{
				formattedValue = value;
			}
			DataGridViewCellPaintingEventArgs cellPaintingEventArgs = dataGridView.CellPaintingEventArgs;
			cellPaintingEventArgs.SetProperties(graphics, clipBounds, cellBounds, rowIndex, columnIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
			dataGridView.OnCellPainting(cellPaintingEventArgs);
			if (cellPaintingEventArgs.Handled)
			{
				return;
			}
			this.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
		}

		/// <summary>Converts a value formatted for display to an actual cell value.</summary>
		/// <param name="formattedValue">The display value of the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> in effect for the cell.</param>
		/// <param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> for the display value type, or <see langword="null" /> to use the default converter.</param>
		/// <param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> for the cell value type, or <see langword="null" /> to use the default converter.</param>
		/// <returns>The cell value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="cellStyle" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">The <see cref="P:System.Windows.Forms.DataGridViewCell.FormattedValueType" /> property value is <see langword="null" />.-or-The <see cref="P:System.Windows.Forms.DataGridViewCell.ValueType" /> property value is <see langword="null" />.-or-
		///         <paramref name="formattedValue" /> cannot be converted.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="formattedValue" /> is <see langword="null" />.-or-The type of <paramref name="formattedValue" /> does not match the type indicated by the <see cref="P:System.Windows.Forms.DataGridViewCell.FormattedValueType" /> property. </exception>
		// Token: 0x06001AA7 RID: 6823 RVA: 0x00085DEE File Offset: 0x00083FEE
		public virtual object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
		{
			return this.ParseFormattedValueInternal(this.ValueType, formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x00085E04 File Offset: 0x00084004
		internal object ParseFormattedValueInternal(Type valueType, object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (this.FormattedValueType == null)
			{
				throw new FormatException(SR.GetString("DataGridViewCell_FormattedValueTypeNull"));
			}
			if (valueType == null)
			{
				throw new FormatException(SR.GetString("DataGridViewCell_ValueTypeNull"));
			}
			if (formattedValue == null || !this.FormattedValueType.IsAssignableFrom(formattedValue.GetType()))
			{
				throw new ArgumentException(SR.GetString("DataGridViewCell_FormattedValueHasWrongType"), "formattedValue");
			}
			return Formatter.ParseObject(formattedValue, valueType, this.FormattedValueType, (valueTypeConverter == null) ? this.ValueTypeConverter : valueTypeConverter, (formattedValueTypeConverter == null) ? this.FormattedValueTypeConverter : formattedValueTypeConverter, cellStyle.FormatProvider, cellStyle.NullValue, cellStyle.IsDataSourceNullValueDefault ? Formatter.GetDefaultDataSourceNullValue(valueType) : cellStyle.DataSourceNullValue);
		}

		/// <summary>Sets the location and size of the editing control hosted by a cell in the <see cref="T:System.Windows.Forms.DataGridView" /> control. </summary>
		/// <param name="setLocation">
		///       <see langword="true" /> to have the control placed as specified by the other arguments; <see langword="false" /> to allow the control to place itself.</param>
		/// <param name="setSize">
		///       <see langword="true" /> to specify the size; <see langword="false" /> to allow the control to size itself. </param>
		/// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle" /> that defines the cell bounds. </param>
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
		/// <exception cref="T:System.InvalidOperationException">The cell is not contained within a <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		// Token: 0x06001AA9 RID: 6825 RVA: 0x00085ECC File Offset: 0x000840CC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void PositionEditingControl(bool setLocation, bool setSize, Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
		{
			Rectangle rectangle = this.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
			if (setLocation)
			{
				base.DataGridView.EditingControl.Location = new Point(rectangle.X, rectangle.Y);
			}
			if (setSize)
			{
				base.DataGridView.EditingControl.Size = new Size(rectangle.Width, rectangle.Height);
			}
		}

		/// <summary>Sets the location and size of the editing panel hosted by the cell, and returns the normal bounds of the editing control within the editing panel.</summary>
		/// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle" /> that defines the cell bounds. </param>
		/// <param name="cellClip">The area that will be used to paint the editing panel.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell being edited.</param>
		/// <param name="singleVerticalBorderAdded">
		///       <see langword="true" /> to add a vertical border to the cell; otherwise, <see langword="false" />.</param>
		/// <param name="singleHorizontalBorderAdded">
		///       <see langword="true" /> to add a horizontal border to the cell; otherwise, <see langword="false" />.</param>
		/// <param name="isFirstDisplayedColumn">
		///       <see langword="true" /> if the cell is in the first column currently displayed in the control; otherwise, <see langword="false" />.</param>
		/// <param name="isFirstDisplayedRow">
		///       <see langword="true" /> if the cell is in the first row currently displayed in the control; otherwise, <see langword="false" />.</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the normal bounds of the editing control within the editing panel.</returns>
		/// <exception cref="T:System.InvalidOperationException">The cell has not been added to a <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		// Token: 0x06001AAA RID: 6826 RVA: 0x00085F3C File Offset: 0x0008413C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual Rectangle PositionEditingPanel(Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
		{
			if (base.DataGridView == null)
			{
				throw new InvalidOperationException();
			}
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
			DataGridViewAdvancedBorderStyle advancedBorderStyle = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
			Rectangle rectangle = this.BorderWidths(advancedBorderStyle);
			rectangle.X += cellStyle.Padding.Left;
			rectangle.Y += cellStyle.Padding.Top;
			rectangle.Width += cellStyle.Padding.Right;
			rectangle.Height += cellStyle.Padding.Bottom;
			int num = cellBounds.Width;
			int num2 = cellBounds.Height;
			int x;
			if (cellClip.X - cellBounds.X >= rectangle.X)
			{
				x = cellClip.X;
				num -= cellClip.X - cellBounds.X;
			}
			else
			{
				x = cellBounds.X + rectangle.X;
				num -= rectangle.X;
			}
			if (cellClip.Right <= cellBounds.Right - rectangle.Width)
			{
				num -= cellBounds.Right - cellClip.Right;
			}
			else
			{
				num -= rectangle.Width;
			}
			int x2 = cellBounds.X - cellClip.X;
			int width = cellBounds.Width - rectangle.X - rectangle.Width;
			int y;
			if (cellClip.Y - cellBounds.Y >= rectangle.Y)
			{
				y = cellClip.Y;
				num2 -= cellClip.Y - cellBounds.Y;
			}
			else
			{
				y = cellBounds.Y + rectangle.Y;
				num2 -= rectangle.Y;
			}
			if (cellClip.Bottom <= cellBounds.Bottom - rectangle.Height)
			{
				num2 -= cellBounds.Bottom - cellClip.Bottom;
			}
			else
			{
				num2 -= rectangle.Height;
			}
			int y2 = cellBounds.Y - cellClip.Y;
			int height = cellBounds.Height - rectangle.Y - rectangle.Height;
			base.DataGridView.EditingPanel.Location = new Point(x, y);
			base.DataGridView.EditingPanel.Size = new Size(num, num2);
			return new Rectangle(x2, y2, width, height);
		}

		/// <summary>Sets the value of the cell. </summary>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="value">The cell value to set. </param>
		/// <returns>
		///     <see langword="true" /> if the value has been set; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than 0 or greater than or equal to the number of rows in the parent <see cref="T:System.Windows.Forms.DataGridView" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCell.ColumnIndex" /> is less than 0.</exception>
		// Token: 0x06001AAB RID: 6827 RVA: 0x000861B4 File Offset: 0x000843B4
		protected virtual bool SetValue(int rowIndex, object value)
		{
			object obj = null;
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView != null && !dataGridView.InSortOperation)
			{
				obj = this.GetValue(rowIndex);
			}
			if (dataGridView != null && this.OwningColumn != null && this.OwningColumn.IsDataBound)
			{
				DataGridView.DataGridViewDataConnection dataConnection = dataGridView.DataConnection;
				if (dataConnection == null)
				{
					return false;
				}
				if (dataConnection.CurrencyManager.Count <= rowIndex)
				{
					if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellValue))
					{
						this.Properties.SetObject(DataGridViewCell.PropCellValue, value);
					}
				}
				else
				{
					if (!dataConnection.PushValue(this.OwningColumn.BoundColumnIndex, this.ColumnIndex, rowIndex, value))
					{
						return false;
					}
					if (base.DataGridView == null || this.OwningRow == null || this.OwningRow.DataGridView == null)
					{
						return true;
					}
					if (this.OwningRow.Index == base.DataGridView.CurrentCellAddress.Y)
					{
						base.DataGridView.IsCurrentRowDirtyInternal = true;
					}
				}
			}
			else if (dataGridView == null || !dataGridView.VirtualMode || rowIndex == -1 || this.ColumnIndex == -1)
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellValue))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellValue, value);
				}
			}
			else
			{
				dataGridView.OnCellValuePushed(this.ColumnIndex, rowIndex, value);
			}
			if (dataGridView != null && !dataGridView.InSortOperation && ((obj == null && value != null) || (obj != null && value == null) || (obj != null && !value.Equals(obj))))
			{
				base.RaiseCellValueChanged(new DataGridViewCellEventArgs(this.ColumnIndex, rowIndex));
			}
			return true;
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x00086334 File Offset: 0x00084534
		internal bool SetValueInternal(int rowIndex, object value)
		{
			return this.SetValue(rowIndex, value);
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00086340 File Offset: 0x00084540
		internal static bool TextFitsInBounds(Graphics graphics, string text, Font font, Size maxBounds, TextFormatFlags flags)
		{
			bool flag;
			int num = DataGridViewCell.MeasureTextHeight(graphics, text, font, maxBounds.Width, flags, out flag);
			return num <= maxBounds.Height && !flag;
		}

		/// <summary>Returns a string that describes the current object. </summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x06001AAE RID: 6830 RVA: 0x00086374 File Offset: 0x00084574
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewCell { ColumnIndex=",
				this.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				this.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x000863D0 File Offset: 0x000845D0
		private static string TruncateToolTipText(string toolTipText)
		{
			if (toolTipText.Length > 288)
			{
				StringBuilder stringBuilder = new StringBuilder(toolTipText.Substring(0, 256), 259);
				stringBuilder.Append("...");
				return stringBuilder.ToString();
			}
			return toolTipText;
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x00086418 File Offset: 0x00084618
		private void UpdateCurrentMouseLocation(DataGridViewCellMouseEventArgs e)
		{
			if (this.GetErrorIconBounds(e.RowIndex).Contains(e.X, e.Y))
			{
				this.CurrentMouseLocation = 2;
				return;
			}
			this.CurrentMouseLocation = 1;
		}

		// Token: 0x04000BD7 RID: 3031
		private const TextFormatFlags textFormatSupportedFlags = TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak;

		// Token: 0x04000BD8 RID: 3032
		private const int DATAGRIDVIEWCELL_constrastThreshold = 1000;

		// Token: 0x04000BD9 RID: 3033
		private const int DATAGRIDVIEWCELL_highConstrastThreshold = 2000;

		// Token: 0x04000BDA RID: 3034
		private const int DATAGRIDVIEWCELL_maxToolTipLength = 288;

		// Token: 0x04000BDB RID: 3035
		private const int DATAGRIDVIEWCELL_maxToolTipCutOff = 256;

		// Token: 0x04000BDC RID: 3036
		private const int DATAGRIDVIEWCELL_toolTipEllipsisLength = 3;

		// Token: 0x04000BDD RID: 3037
		private const string DATAGRIDVIEWCELL_toolTipEllipsis = "...";

		// Token: 0x04000BDE RID: 3038
		private const byte DATAGRIDVIEWCELL_flagAreaNotSet = 0;

		// Token: 0x04000BDF RID: 3039
		private const byte DATAGRIDVIEWCELL_flagDataArea = 1;

		// Token: 0x04000BE0 RID: 3040
		private const byte DATAGRIDVIEWCELL_flagErrorArea = 2;

		// Token: 0x04000BE1 RID: 3041
		internal const byte DATAGRIDVIEWCELL_iconMarginWidth = 4;

		// Token: 0x04000BE2 RID: 3042
		internal const byte DATAGRIDVIEWCELL_iconMarginHeight = 4;

		// Token: 0x04000BE3 RID: 3043
		private const byte DATAGRIDVIEWCELL_iconsWidth = 12;

		// Token: 0x04000BE4 RID: 3044
		private const byte DATAGRIDVIEWCELL_iconsHeight = 11;

		// Token: 0x04000BE5 RID: 3045
		private static bool isScalingInitialized = false;

		// Token: 0x04000BE6 RID: 3046
		internal static byte iconsWidth = 12;

		// Token: 0x04000BE7 RID: 3047
		internal static byte iconsHeight = 11;

		// Token: 0x04000BE8 RID: 3048
		internal static readonly int PropCellValue = PropertyStore.CreateKey();

		// Token: 0x04000BE9 RID: 3049
		private static readonly int PropCellContextMenuStrip = PropertyStore.CreateKey();

		// Token: 0x04000BEA RID: 3050
		private static readonly int PropCellErrorText = PropertyStore.CreateKey();

		// Token: 0x04000BEB RID: 3051
		private static readonly int PropCellStyle = PropertyStore.CreateKey();

		// Token: 0x04000BEC RID: 3052
		private static readonly int PropCellValueType = PropertyStore.CreateKey();

		// Token: 0x04000BED RID: 3053
		private static readonly int PropCellTag = PropertyStore.CreateKey();

		// Token: 0x04000BEE RID: 3054
		private static readonly int PropCellToolTipText = PropertyStore.CreateKey();

		// Token: 0x04000BEF RID: 3055
		private static readonly int PropCellAccessibilityObject = PropertyStore.CreateKey();

		// Token: 0x04000BF0 RID: 3056
		private static Bitmap errorBmp = null;

		// Token: 0x04000BF1 RID: 3057
		private PropertyStore propertyStore;

		// Token: 0x04000BF2 RID: 3058
		private DataGridViewRow owningRow;

		// Token: 0x04000BF3 RID: 3059
		private DataGridViewColumn owningColumn;

		// Token: 0x04000BF4 RID: 3060
		private static Type stringType = typeof(string);

		// Token: 0x04000BF5 RID: 3061
		private byte flags;

		/// <summary>Provides information about a <see cref="T:System.Windows.Forms.DataGridViewCell" /> to accessibility client applications.</summary>
		// Token: 0x020005AB RID: 1451
		protected class DataGridViewCellAccessibleObject : AccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" /> class without initializing the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property.</summary>
			// Token: 0x06005911 RID: 22801 RVA: 0x001724E2 File Offset: 0x001706E2
			public DataGridViewCellAccessibleObject()
			{
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" /> class, setting the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property to the specified <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</param>
			// Token: 0x06005912 RID: 22802 RVA: 0x00176DF7 File Offset: 0x00174FF7
			public DataGridViewCellAccessibleObject(DataGridViewCell owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the location and size of the accessible object.</summary>
			/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the accessible object.</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x17001579 RID: 5497
			// (get) Token: 0x06005913 RID: 22803 RVA: 0x00176E06 File Offset: 0x00175006
			public override Rectangle Bounds
			{
				get
				{
					return this.GetAccessibleObjectBounds(this.GetAccessibleObjectParent());
				}
			}

			/// <summary>Gets a string that describes the default action of the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
			/// <returns>The string "Edit".</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x1700157A RID: 5498
			// (get) Token: 0x06005914 RID: 22804 RVA: 0x00176E14 File Offset: 0x00175014
			public override string DefaultAction
			{
				get
				{
					if (this.Owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					if (!this.Owner.ReadOnly)
					{
						return SR.GetString("DataGridView_AccCellDefaultAction");
					}
					return string.Empty;
				}
			}

			/// <summary>Gets the names of the owning cell's type and base type.</summary>
			/// <returns>The names of the owning cell's type and base type.</returns>
			// Token: 0x1700157B RID: 5499
			// (get) Token: 0x06005915 RID: 22805 RVA: 0x00176E4B File Offset: 0x0017504B
			public override string Help
			{
				get
				{
					if (AccessibilityImprovements.Level2)
					{
						return null;
					}
					return this.owner.GetType().Name + "(" + this.owner.GetType().BaseType.Name + ")";
				}
			}

			/// <summary>Gets the name of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</summary>
			/// <returns>The name of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x1700157C RID: 5500
			// (get) Token: 0x06005916 RID: 22806 RVA: 0x00176E8C File Offset: 0x0017508C
			public override string Name
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					if (this.owner.OwningColumn != null)
					{
						string text = SR.GetString("DataGridView_AccDataGridViewCellName", new object[]
						{
							this.owner.OwningColumn.HeaderText,
							this.owner.OwningRow.Index
						});
						if (AccessibilityImprovements.Level3 && this.owner.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
						{
							DataGridViewCell dataGridViewCell = this.Owner;
							DataGridView dataGridView = dataGridViewCell.DataGridView;
							if (dataGridViewCell.OwningColumn != null && dataGridViewCell.OwningColumn == dataGridView.SortedColumn)
							{
								text = text + ", " + ((dataGridView.SortOrder == SortOrder.Ascending) ? SR.GetString("SortedAscendingAccessibleStatus") : SR.GetString("SortedDescendingAccessibleStatus"));
							}
							else
							{
								text = text + ", " + SR.GetString("NotSortedAccessibleStatus");
							}
						}
						return text;
					}
					return string.Empty;
				}
			}

			/// <summary>Gets or sets the cell that owns the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</summary>
			/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCell" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</returns>
			/// <exception cref="T:System.InvalidOperationException">When setting this property, the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property has already been set.</exception>
			// Token: 0x1700157D RID: 5501
			// (get) Token: 0x06005917 RID: 22807 RVA: 0x00176F84 File Offset: 0x00175184
			// (set) Token: 0x06005918 RID: 22808 RVA: 0x00176F8C File Offset: 0x0017518C
			public DataGridViewCell Owner
			{
				get
				{
					return this.owner;
				}
				set
				{
					if (this.owner != null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerAlreadySet"));
					}
					this.owner = value;
				}
			}

			/// <summary>Gets the parent of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</summary>
			/// <returns>The parent of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x1700157E RID: 5502
			// (get) Token: 0x06005919 RID: 22809 RVA: 0x00176FAD File Offset: 0x001751AD
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.ParentPrivate;
				}
			}

			// Token: 0x1700157F RID: 5503
			// (get) Token: 0x0600591A RID: 22810 RVA: 0x00176FB5 File Offset: 0x001751B5
			private AccessibleObject ParentPrivate
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					if (this.owner.OwningRow == null)
					{
						return null;
					}
					return this.owner.OwningRow.AccessibilityObject;
				}
			}

			/// <summary>Gets the role of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</summary>
			/// <returns>The <see cref="F:System.Windows.Forms.AccessibleRole.Cell" /> value.</returns>
			// Token: 0x17001580 RID: 5504
			// (get) Token: 0x0600591B RID: 22811 RVA: 0x0017326C File Offset: 0x0017146C
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Cell;
				}
			}

			/// <summary>Gets the state of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</summary>
			/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.AccessibleStates" /> values. </returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x17001581 RID: 5505
			// (get) Token: 0x0600591C RID: 22812 RVA: 0x00176FF0 File Offset: 0x001751F0
			public override AccessibleStates State
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					if (this.owner == this.owner.DataGridView.CurrentCell)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					if (this.owner.Selected)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					if (AccessibilityImprovements.Level1 && this.owner.ReadOnly)
					{
						accessibleStates |= AccessibleStates.ReadOnly;
					}
					Rectangle cellDisplayRectangle;
					if (this.owner.OwningColumn != null && this.owner.OwningRow != null)
					{
						cellDisplayRectangle = this.owner.DataGridView.GetCellDisplayRectangle(this.owner.OwningColumn.Index, this.owner.OwningRow.Index, false);
					}
					else if (this.owner.OwningRow != null)
					{
						cellDisplayRectangle = this.owner.DataGridView.GetCellDisplayRectangle(-1, this.owner.OwningRow.Index, false);
					}
					else if (this.owner.OwningColumn != null)
					{
						cellDisplayRectangle = this.owner.DataGridView.GetCellDisplayRectangle(this.owner.OwningColumn.Index, -1, false);
					}
					else
					{
						cellDisplayRectangle = this.owner.DataGridView.GetCellDisplayRectangle(-1, -1, false);
					}
					if (!cellDisplayRectangle.IntersectsWith(this.owner.DataGridView.ClientRectangle))
					{
						accessibleStates |= AccessibleStates.Offscreen;
					}
					return accessibleStates;
				}
			}

			/// <summary>Gets or sets a string representing the formatted value of the owning cell. </summary>
			/// <returns>A <see cref="T:System.String" /> representation of the cell value.</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x17001582 RID: 5506
			// (get) Token: 0x0600591D RID: 22813 RVA: 0x0017714C File Offset: 0x0017534C
			// (set) Token: 0x0600591E RID: 22814 RVA: 0x001771E4 File Offset: 0x001753E4
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					object formattedValue = this.owner.FormattedValue;
					string text = formattedValue as string;
					if (formattedValue == null || (text != null && string.IsNullOrEmpty(text)))
					{
						return SR.GetString("DataGridView_AccNullValue");
					}
					if (text != null)
					{
						return text;
					}
					if (this.owner.OwningColumn == null)
					{
						return string.Empty;
					}
					TypeConverter formattedValueTypeConverter = this.owner.FormattedValueTypeConverter;
					if (formattedValueTypeConverter != null && formattedValueTypeConverter.CanConvertTo(typeof(string)))
					{
						return formattedValueTypeConverter.ConvertToString(formattedValue);
					}
					return formattedValue.ToString();
				}
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				set
				{
					if (this.owner is DataGridViewHeaderCell)
					{
						return;
					}
					if (this.owner.ReadOnly)
					{
						return;
					}
					if (this.owner.OwningRow == null)
					{
						return;
					}
					if (this.owner.DataGridView.IsCurrentCellInEditMode)
					{
						this.owner.DataGridView.EndEdit();
					}
					DataGridViewCellStyle inheritedStyle = this.owner.InheritedStyle;
					object formattedValue = this.owner.GetFormattedValue(value, this.owner.OwningRow.Index, ref inheritedStyle, null, null, DataGridViewDataErrorContexts.Formatting);
					this.owner.Value = this.owner.ParseFormattedValue(formattedValue, inheritedStyle, null, null);
				}
			}

			/// <summary>Performs the default action associated with the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</summary>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.-or-The value of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" /> property is not <see langword="null" /> and the <see cref="P:System.Windows.Forms.DataGridViewCell.RowIndex" /> property of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> returned by the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is equal to -1.</exception>
			// Token: 0x0600591F RID: 22815 RVA: 0x00177288 File Offset: 0x00175488
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				DataGridViewCell dataGridViewCell = this.Owner;
				DataGridView dataGridView = dataGridViewCell.DataGridView;
				if (dataGridViewCell is DataGridViewHeaderCell)
				{
					return;
				}
				if (dataGridView != null && dataGridViewCell.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				this.Select(AccessibleSelection.TakeFocus | AccessibleSelection.TakeSelection);
				if (dataGridViewCell.ReadOnly)
				{
					return;
				}
				if (dataGridViewCell.EditType != null)
				{
					if (dataGridView.InBeginEdit || dataGridView.InEndEdit)
					{
						return;
					}
					if (dataGridView.IsCurrentCellInEditMode)
					{
						dataGridView.EndEdit();
						return;
					}
					if (dataGridView.EditMode != DataGridViewEditMode.EditProgrammatically)
					{
						dataGridView.BeginEdit(true);
					}
				}
			}

			// Token: 0x06005920 RID: 22816 RVA: 0x00177330 File Offset: 0x00175530
			internal Rectangle GetAccessibleObjectBounds(AccessibleObject parentAccObject)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.OwningColumn == null)
				{
					return Rectangle.Empty;
				}
				Rectangle bounds = parentAccObject.Bounds;
				int num = this.owner.DataGridView.Columns.ColumnIndexToActualDisplayIndex(this.owner.DataGridView.FirstDisplayedScrollingColumnIndex, DataGridViewElementStates.Visible);
				int num2 = this.owner.DataGridView.Columns.ColumnIndexToActualDisplayIndex(this.owner.ColumnIndex, DataGridViewElementStates.Visible);
				bool rowHeadersVisible = this.owner.DataGridView.RowHeadersVisible;
				Rectangle r;
				if (num2 < num)
				{
					r = parentAccObject.GetChild(num2 + 1 + (rowHeadersVisible ? 1 : 0)).Bounds;
					if (this.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						r.X -= this.owner.OwningColumn.Width;
					}
					else
					{
						r.X = r.Right;
					}
					r.Width = this.owner.OwningColumn.Width;
				}
				else if (num2 == num)
				{
					r = this.owner.DataGridView.GetColumnDisplayRectangle(this.owner.ColumnIndex, false);
					int firstDisplayedScrollingColumnHiddenWidth = this.owner.DataGridView.FirstDisplayedScrollingColumnHiddenWidth;
					if (firstDisplayedScrollingColumnHiddenWidth != 0)
					{
						if (this.owner.DataGridView.RightToLeft == RightToLeft.No)
						{
							r.X -= firstDisplayedScrollingColumnHiddenWidth;
						}
						r.Width += firstDisplayedScrollingColumnHiddenWidth;
					}
					r = this.owner.DataGridView.RectangleToScreen(r);
				}
				else
				{
					r = parentAccObject.GetChild(num2 - 1 + (rowHeadersVisible ? 1 : 0)).Bounds;
					if (this.owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						r.X = r.Right;
					}
					else
					{
						r.X -= this.owner.OwningColumn.Width;
					}
					r.Width = this.owner.OwningColumn.Width;
				}
				bounds.X = r.X;
				bounds.Width = r.Width;
				return bounds;
			}

			// Token: 0x06005921 RID: 22817 RVA: 0x00177550 File Offset: 0x00175750
			private AccessibleObject GetAccessibleObjectParent()
			{
				if (this.owner is DataGridViewButtonCell || this.owner is DataGridViewCheckBoxCell || this.owner is DataGridViewComboBoxCell || this.owner is DataGridViewImageCell || this.owner is DataGridViewLinkCell || this.owner is DataGridViewTextBoxCell)
				{
					return this.ParentPrivate;
				}
				return this.Parent;
			}

			/// <summary>Returns the accessible object corresponding to the specified index.</summary>
			/// <param name="index">The zero-based index of the child accessible object.</param>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents the accessible child corresponding to the specified index.</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x06005922 RID: 22818 RVA: 0x001775B8 File Offset: 0x001757B8
			public override AccessibleObject GetChild(int index)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.DataGridView.EditingControl != null && this.owner.DataGridView.IsCurrentCellInEditMode && this.owner.DataGridView.CurrentCell == this.owner && index == 0)
				{
					return this.owner.DataGridView.EditingControl.AccessibilityObject;
				}
				return null;
			}

			/// <summary>Returns the number of children that belong to the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</summary>
			/// <returns>The value 1 if the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that owns <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" /> is being edited; otherwise, –1.</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x06005923 RID: 22819 RVA: 0x00177634 File Offset: 0x00175834
			public override int GetChildCount()
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.DataGridView.EditingControl != null && this.owner.DataGridView.IsCurrentCellInEditMode && this.owner.DataGridView.CurrentCell == this.owner)
				{
					return 1;
				}
				return 0;
			}

			/// <summary>Returns the child accessible object that has keyboard focus.</summary>
			/// <returns>
			///     <see langword="null" /> in all cases.</returns>
			// Token: 0x06005924 RID: 22820 RVA: 0x0000DE5C File Offset: 0x0000C05C
			public override AccessibleObject GetFocused()
			{
				return null;
			}

			/// <summary>Returns the child accessible object that is currently selected.</summary>
			/// <returns>
			///     <see langword="null" /> in all cases.</returns>
			// Token: 0x06005925 RID: 22821 RVA: 0x0000DE5C File Offset: 0x0000C05C
			public override AccessibleObject GetSelected()
			{
				return null;
			}

			/// <summary>Navigates to another accessible object.</summary>
			/// <param name="navigationDirection">One of the <see cref="T:System.Windows.Forms.AccessibleNavigation" /> values.</param>
			/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" /> that represents the <see cref="T:System.Windows.Forms.DataGridViewCell" /> at the specified <see cref="T:System.Windows.Forms.AccessibleNavigation" /> value.</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x06005926 RID: 22822 RVA: 0x00177698 File Offset: 0x00175898
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.OwningColumn == null || this.owner.OwningRow == null)
				{
					return null;
				}
				switch (navigationDirection)
				{
				case AccessibleNavigation.Up:
					if (this.owner.OwningRow.Index != this.owner.DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Visible))
					{
						int previousRow = this.owner.DataGridView.Rows.GetPreviousRow(this.owner.OwningRow.Index, DataGridViewElementStates.Visible);
						return this.owner.DataGridView.Rows[previousRow].Cells[this.owner.OwningColumn.Index].AccessibilityObject;
					}
					if (this.owner.DataGridView.ColumnHeadersVisible)
					{
						return this.owner.OwningColumn.HeaderCell.AccessibilityObject;
					}
					return null;
				case AccessibleNavigation.Down:
				{
					if (this.owner.OwningRow.Index == this.owner.DataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible))
					{
						return null;
					}
					int nextRow = this.owner.DataGridView.Rows.GetNextRow(this.owner.OwningRow.Index, DataGridViewElementStates.Visible);
					return this.owner.DataGridView.Rows[nextRow].Cells[this.owner.OwningColumn.Index].AccessibilityObject;
				}
				case AccessibleNavigation.Left:
					if (this.owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateBackward(true);
					}
					return this.NavigateForward(true);
				case AccessibleNavigation.Right:
					if (this.owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateForward(true);
					}
					return this.NavigateBackward(true);
				case AccessibleNavigation.Next:
					return this.NavigateForward(false);
				case AccessibleNavigation.Previous:
					return this.NavigateBackward(false);
				default:
					return null;
				}
			}

			// Token: 0x06005927 RID: 22823 RVA: 0x00177890 File Offset: 0x00175A90
			private AccessibleObject NavigateBackward(bool wrapAround)
			{
				if (this.owner.OwningColumn != this.owner.DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible))
				{
					int index = this.owner.DataGridView.Columns.GetPreviousColumn(this.owner.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None).Index;
					return this.owner.OwningRow.Cells[index].AccessibilityObject;
				}
				if (wrapAround)
				{
					AccessibleObject accessibleObject = this.Owner.OwningRow.AccessibilityObject.Navigate(AccessibleNavigation.Previous);
					if (accessibleObject != null && accessibleObject.GetChildCount() > 0)
					{
						return accessibleObject.GetChild(accessibleObject.GetChildCount() - 1);
					}
					return null;
				}
				else
				{
					if (this.owner.DataGridView.RowHeadersVisible)
					{
						return this.owner.OwningRow.AccessibilityObject.GetChild(0);
					}
					return null;
				}
			}

			// Token: 0x06005928 RID: 22824 RVA: 0x00177968 File Offset: 0x00175B68
			private AccessibleObject NavigateForward(bool wrapAround)
			{
				if (this.owner.OwningColumn != this.owner.DataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None))
				{
					int index = this.owner.DataGridView.Columns.GetNextColumn(this.owner.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None).Index;
					return this.owner.OwningRow.Cells[index].AccessibilityObject;
				}
				if (!wrapAround)
				{
					return null;
				}
				AccessibleObject accessibleObject = this.Owner.OwningRow.AccessibilityObject.Navigate(AccessibleNavigation.Next);
				if (accessibleObject == null || accessibleObject.GetChildCount() <= 0)
				{
					return null;
				}
				if (this.Owner.DataGridView.RowHeadersVisible)
				{
					return accessibleObject.GetChild(1);
				}
				return accessibleObject.GetChild(0);
			}

			/// <summary>Modifies the selection or moves the keyboard focus of the accessible object.</summary>
			/// <param name="flags">One of the <see cref="T:System.Windows.Forms.AccessibleSelection" /> values.</param>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x06005929 RID: 22825 RVA: 0x00177A2C File Offset: 0x00175C2C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					this.owner.DataGridView.FocusInternal();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.owner.Selected = true;
					this.owner.DataGridView.CurrentCell = this.owner;
				}
				if ((flags & AccessibleSelection.AddSelection) == AccessibleSelection.AddSelection)
				{
					this.owner.Selected = true;
				}
				if ((flags & AccessibleSelection.RemoveSelection) == AccessibleSelection.RemoveSelection && (flags & (AccessibleSelection.TakeSelection | AccessibleSelection.AddSelection)) == AccessibleSelection.None)
				{
					this.owner.Selected = false;
				}
			}

			// Token: 0x0600592A RID: 22826 RVA: 0x00177ABC File Offset: 0x00175CBC
			internal override void SetDetachableChild(AccessibleObject child)
			{
				this._child = child;
			}

			// Token: 0x0600592B RID: 22827 RVA: 0x0016B402 File Offset: 0x00169602
			internal override void SetFocus()
			{
				base.SetFocus();
				base.RaiseAutomationEvent(20005);
			}

			// Token: 0x17001583 RID: 5507
			// (get) Token: 0x0600592C RID: 22828 RVA: 0x00177AC5 File Offset: 0x00175CC5
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

			// Token: 0x17001584 RID: 5508
			// (get) Token: 0x0600592D RID: 22829 RVA: 0x00177AFC File Offset: 0x00175CFC
			private string AutomationId
			{
				get
				{
					string text = string.Empty;
					foreach (int num in this.RuntimeId)
					{
						text += num.ToString();
					}
					return text;
				}
			}

			// Token: 0x0600592E RID: 22830 RVA: 0x00177B37 File Offset: 0x00175D37
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level2 || base.IsIAccessibleExSupported();
			}

			// Token: 0x17001585 RID: 5509
			// (get) Token: 0x0600592F RID: 22831 RVA: 0x0000E209 File Offset: 0x0000C409
			internal override Rectangle BoundingRectangle
			{
				get
				{
					return this.Bounds;
				}
			}

			// Token: 0x17001586 RID: 5510
			// (get) Token: 0x06005930 RID: 22832 RVA: 0x00177B48 File Offset: 0x00175D48
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this.owner.DataGridView.AccessibilityObject;
				}
			}

			// Token: 0x06005931 RID: 22833 RVA: 0x00177B5C File Offset: 0x00175D5C
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.OwningColumn == null || this.owner.OwningRow == null)
				{
					return null;
				}
				switch (direction)
				{
				case UnsafeNativeMethods.NavigateDirection.Parent:
					return this.owner.OwningRow.AccessibilityObject;
				case UnsafeNativeMethods.NavigateDirection.NextSibling:
					return this.NavigateForward(false);
				case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
					return this.NavigateBackward(false);
				case UnsafeNativeMethods.NavigateDirection.FirstChild:
				case UnsafeNativeMethods.NavigateDirection.LastChild:
					if (this.owner.DataGridView.CurrentCell == this.owner && this.owner.DataGridView.IsCurrentCellInEditMode && this.owner.DataGridView.EditingControl != null)
					{
						return this._child;
					}
					return null;
				default:
					return null;
				}
			}

			// Token: 0x06005932 RID: 22834 RVA: 0x00177C20 File Offset: 0x00175E20
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3)
				{
					switch (propertyID)
					{
					case 30005:
						return this.Name;
					case 30006:
					case 30012:
					case 30014:
					case 30015:
					case 30016:
					case 30017:
					case 30018:
						break;
					case 30007:
						return string.Empty;
					case 30008:
						return (this.State & AccessibleStates.Focused) == AccessibleStates.Focused;
					case 30009:
						return (this.State & AccessibleStates.Focusable) == AccessibleStates.Focusable;
					case 30010:
						return this.owner.DataGridView.Enabled;
					case 30011:
						return this.AutomationId;
					case 30013:
						return this.Help ?? string.Empty;
					case 30019:
						return false;
					default:
						if (propertyID == 30022)
						{
							return (this.State & AccessibleStates.Offscreen) == AccessibleStates.Offscreen;
						}
						if (propertyID == 30068)
						{
							return this.Owner.DataGridView.AccessibilityObject;
						}
						break;
					}
				}
				if (propertyID == 30039)
				{
					return this.IsPatternSupported(10013);
				}
				if (propertyID == 30029)
				{
					return this.IsPatternSupported(10007);
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06005933 RID: 22835 RVA: 0x00177D68 File Offset: 0x00175F68
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level3 && (patternId.Equals(10018) || patternId.Equals(10000) || patternId.Equals(10002))) || ((patternId == 10013 || patternId == 10007) && this.owner.ColumnIndex != -1 && this.owner.RowIndex != -1) || base.IsPatternSupported(patternId);
			}

			// Token: 0x06005934 RID: 22836 RVA: 0x00177DE0 File Offset: 0x00175FE0
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal override UnsafeNativeMethods.IRawElementProviderSimple[] GetRowHeaderItems()
			{
				if (this.owner.DataGridView.RowHeadersVisible && this.owner.OwningRow.HasHeaderCell)
				{
					return new UnsafeNativeMethods.IRawElementProviderSimple[]
					{
						this.owner.OwningRow.HeaderCell.AccessibilityObject
					};
				}
				return null;
			}

			// Token: 0x06005935 RID: 22837 RVA: 0x00177E34 File Offset: 0x00176034
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			internal override UnsafeNativeMethods.IRawElementProviderSimple[] GetColumnHeaderItems()
			{
				if (this.owner.DataGridView.ColumnHeadersVisible && this.owner.OwningColumn.HasHeaderCell)
				{
					return new UnsafeNativeMethods.IRawElementProviderSimple[]
					{
						this.owner.OwningColumn.HeaderCell.AccessibilityObject
					};
				}
				return null;
			}

			// Token: 0x17001587 RID: 5511
			// (get) Token: 0x06005936 RID: 22838 RVA: 0x00177E85 File Offset: 0x00176085
			internal override int Row
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (this.owner.OwningRow == null)
					{
						return -1;
					}
					return this.owner.OwningRow.Index;
				}
			}

			// Token: 0x17001588 RID: 5512
			// (get) Token: 0x06005937 RID: 22839 RVA: 0x00177EA6 File Offset: 0x001760A6
			internal override int Column
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (this.owner.OwningColumn == null)
					{
						return -1;
					}
					return this.owner.OwningColumn.Index;
				}
			}

			// Token: 0x17001589 RID: 5513
			// (get) Token: 0x06005938 RID: 22840 RVA: 0x00177B48 File Offset: 0x00175D48
			internal override UnsafeNativeMethods.IRawElementProviderSimple ContainingGrid
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.DataGridView.AccessibilityObject;
				}
			}

			// Token: 0x1700158A RID: 5514
			// (get) Token: 0x06005939 RID: 22841 RVA: 0x00177EC7 File Offset: 0x001760C7
			internal override bool IsReadOnly
			{
				get
				{
					return this.owner.ReadOnly;
				}
			}

			// Token: 0x04003925 RID: 14629
			private int[] runtimeId;

			// Token: 0x04003926 RID: 14630
			private AccessibleObject _child;

			// Token: 0x04003927 RID: 14631
			private DataGridViewCell owner;
		}
	}
}
