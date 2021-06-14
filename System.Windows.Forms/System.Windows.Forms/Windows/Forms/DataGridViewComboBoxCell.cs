using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Displays a combo box in a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001B7 RID: 439
	public class DataGridViewComboBoxCell : DataGridViewCell
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> class.</summary>
		// Token: 0x06001C73 RID: 7283 RVA: 0x0008F37C File Offset: 0x0008D57C
		public DataGridViewComboBoxCell()
		{
			this.flags = 8;
			if (!DataGridViewComboBoxCell.isScalingInitialized)
			{
				if (DpiHelper.IsScalingRequired)
				{
					DataGridViewComboBoxCell.offset2X = DpiHelper.LogicalToDeviceUnitsX(DataGridViewComboBoxCell.OFFSET_2PIXELS);
					DataGridViewComboBoxCell.offset2Y = DpiHelper.LogicalToDeviceUnitsY(DataGridViewComboBoxCell.OFFSET_2PIXELS);
					DataGridViewComboBoxCell.nonXPTriangleWidth = (byte)DpiHelper.LogicalToDeviceUnitsX(7);
					DataGridViewComboBoxCell.nonXPTriangleHeight = (byte)DpiHelper.LogicalToDeviceUnitsY(4);
				}
				DataGridViewComboBoxCell.isScalingInitialized = true;
			}
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Forms.AccessibleObject" /> for this <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> instance. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> instance that supports the ControlType UIA property. </returns>
		// Token: 0x06001C74 RID: 7284 RVA: 0x0008F3E0 File Offset: 0x0008D5E0
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level2)
			{
				return new DataGridViewComboBoxCell.DataGridViewComboBoxCellAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		/// <summary>Gets or sets a value indicating whether the cell will match the characters being entered in the cell with a selection from the drop-down list. </summary>
		/// <returns>
		///     <see langword="true" /> if automatic completion is activated; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001C75 RID: 7285 RVA: 0x0008F3F6 File Offset: 0x0008D5F6
		// (set) Token: 0x06001C76 RID: 7286 RVA: 0x0008F404 File Offset: 0x0008D604
		[DefaultValue(true)]
		public virtual bool AutoComplete
		{
			get
			{
				return (this.flags & 8) > 0;
			}
			set
			{
				if (value != this.AutoComplete)
				{
					if (value)
					{
						this.flags |= 8;
					}
					else
					{
						this.flags = (byte)((int)this.flags & -9);
					}
					if (this.OwnsEditingComboBox(base.RowIndex))
					{
						if (value)
						{
							this.EditingComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
							this.EditingComboBox.AutoCompleteMode = AutoCompleteMode.Append;
							return;
						}
						this.EditingComboBox.AutoCompleteMode = AutoCompleteMode.None;
						this.EditingComboBox.AutoCompleteSource = AutoCompleteSource.None;
					}
				}
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x0008F488 File Offset: 0x0008D688
		// (set) Token: 0x06001C78 RID: 7288 RVA: 0x0008F496 File Offset: 0x0008D696
		private CurrencyManager DataManager
		{
			get
			{
				return this.GetDataManager(base.DataGridView);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellDataManager))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellDataManager, value);
				}
			}
		}

		/// <summary>Gets or sets the data source whose data contains the possible selections shown in the drop-down list.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> or <see cref="T:System.ComponentModel.IListSource" /> that contains a collection of values used to supply data to the drop-down list. The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is not <see langword="null" /> and is not of type <see cref="T:System.Collections.IList" /> nor <see cref="T:System.ComponentModel.IListSource" />.</exception>
		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001C79 RID: 7289 RVA: 0x0008F4BE File Offset: 0x0008D6BE
		// (set) Token: 0x06001C7A RID: 7290 RVA: 0x0008F4D0 File Offset: 0x0008D6D0
		public virtual object DataSource
		{
			get
			{
				return base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellDataSource);
			}
			set
			{
				if (value != null && !(value is IList) && !(value is IListSource))
				{
					throw new ArgumentException(SR.GetString("BadDataSourceForComplexBinding"));
				}
				if (this.DataSource != value)
				{
					this.DataManager = null;
					this.UnwireDataSource();
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellDataSource, value);
					this.WireDataSource(value);
					this.CreateItemsFromDataSource = true;
					DataGridViewComboBoxCell.cachedDropDownWidth = -1;
					try
					{
						this.InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
						this.DisplayMemberInternal = null;
					}
					try
					{
						this.InitializeValueMemberPropertyDescriptor(this.ValueMember);
					}
					catch (Exception ex2)
					{
						if (ClientUtils.IsCriticalException(ex2))
						{
							throw;
						}
						this.ValueMemberInternal = null;
					}
					if (value == null)
					{
						this.DisplayMemberInternal = null;
						this.ValueMemberInternal = null;
					}
					if (this.OwnsEditingComboBox(base.RowIndex))
					{
						this.EditingComboBox.DataSource = value;
						this.InitializeComboBoxText();
						return;
					}
					base.OnCommonChange();
				}
			}
		}

		/// <summary>Gets or sets a string that specifies where to gather selections to display in the drop-down list.</summary>
		/// <returns>A string specifying the name of a property or column in the data source specified in the <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property. The default value is <see cref="F:System.String.Empty" />, which indicates that the <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DisplayMember" /> property will not be used.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property is not <see langword="null" /> and the specified value when setting this property is not <see langword="null" /> or <see cref="F:System.String.Empty" /> and does not name a valid property or column in the data source.</exception>
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001C7B RID: 7291 RVA: 0x0008F5D8 File Offset: 0x0008D7D8
		// (set) Token: 0x06001C7C RID: 7292 RVA: 0x0008F605 File Offset: 0x0008D805
		[DefaultValue("")]
		public virtual string DisplayMember
		{
			get
			{
				object @object = base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMember);
				if (@object == null)
				{
					return string.Empty;
				}
				return (string)@object;
			}
			set
			{
				this.DisplayMemberInternal = value;
				if (this.OwnsEditingComboBox(base.RowIndex))
				{
					this.EditingComboBox.DisplayMember = value;
					this.InitializeComboBoxText();
					return;
				}
				base.OnCommonChange();
			}
		}

		// Token: 0x170006AA RID: 1706
		// (set) Token: 0x06001C7D RID: 7293 RVA: 0x0008F635 File Offset: 0x0008D835
		private string DisplayMemberInternal
		{
			set
			{
				this.InitializeDisplayMemberPropertyDescriptor(value);
				if ((value != null && value.Length > 0) || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMember))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMember, value);
				}
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x0008F66D File Offset: 0x0008D86D
		// (set) Token: 0x06001C7F RID: 7295 RVA: 0x0008F684 File Offset: 0x0008D884
		private PropertyDescriptor DisplayMemberProperty
		{
			get
			{
				return (PropertyDescriptor)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMemberProp);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMemberProp))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMemberProp, value);
				}
			}
		}

		/// <summary>Gets or sets a value that determines how the combo box is displayed when it is not in edit mode.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewComboBoxDisplayStyle" /> values. The default is <see cref="F:System.Windows.Forms.DataGridViewComboBoxDisplayStyle.DropDownButton" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.DataGridViewComboBoxDisplayStyle" /> value.</exception>
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x0008F6AC File Offset: 0x0008D8AC
		// (set) Token: 0x06001C81 RID: 7297 RVA: 0x0008F6D4 File Offset: 0x0008D8D4
		[DefaultValue(DataGridViewComboBoxDisplayStyle.DropDownButton)]
		public DataGridViewComboBoxDisplayStyle DisplayStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyle, out flag);
				if (flag)
				{
					return (DataGridViewComboBoxDisplayStyle)integer;
				}
				return DataGridViewComboBoxDisplayStyle.DropDownButton;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewComboBoxDisplayStyle));
				}
				if (value != this.DisplayStyle)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyle, (int)value);
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

		// Token: 0x170006AD RID: 1709
		// (set) Token: 0x06001C82 RID: 7298 RVA: 0x0008F750 File Offset: 0x0008D950
		internal DataGridViewComboBoxDisplayStyle DisplayStyleInternal
		{
			set
			{
				if (value != this.DisplayStyle)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyle, (int)value);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DisplayStyle" /> property value applies to the cell only when it is the current cell in the <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if the display style applies to the cell only when it is the current cell; otherwise <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x0008F76C File Offset: 0x0008D96C
		// (set) Token: 0x06001C84 RID: 7300 RVA: 0x0008F798 File Offset: 0x0008D998
		[DefaultValue(false)]
		public bool DisplayStyleForCurrentCellOnly
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyleForCurrentCellOnly, out flag);
				return flag && integer != 0;
			}
			set
			{
				if (value != this.DisplayStyleForCurrentCellOnly)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyleForCurrentCellOnly, value ? 1 : 0);
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

		// Token: 0x170006AF RID: 1711
		// (set) Token: 0x06001C85 RID: 7301 RVA: 0x0008F7F4 File Offset: 0x0008D9F4
		internal bool DisplayStyleForCurrentCellOnlyInternal
		{
			set
			{
				if (value != this.DisplayStyleForCurrentCellOnly)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyleForCurrentCellOnly, value ? 1 : 0);
				}
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x0008F816 File Offset: 0x0008DA16
		private Type DisplayType
		{
			get
			{
				if (this.DisplayMemberProperty != null)
				{
					return this.DisplayMemberProperty.PropertyType;
				}
				if (this.ValueMemberProperty != null)
				{
					return this.ValueMemberProperty.PropertyType;
				}
				return DataGridViewComboBoxCell.defaultFormattedValueType;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001C87 RID: 7303 RVA: 0x0008F845 File Offset: 0x0008DA45
		private TypeConverter DisplayTypeConverter
		{
			get
			{
				if (base.DataGridView != null)
				{
					return base.DataGridView.GetCachedTypeConverter(this.DisplayType);
				}
				return TypeDescriptor.GetConverter(this.DisplayType);
			}
		}

		/// <summary>Gets or sets the width of the of the drop-down list portion of a combo box.</summary>
		/// <returns>The width, in pixels, of the drop-down list. The default is 1.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified value is less than one.</exception>
		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001C88 RID: 7304 RVA: 0x0008F86C File Offset: 0x0008DA6C
		// (set) Token: 0x06001C89 RID: 7305 RVA: 0x0008F894 File Offset: 0x0008DA94
		[DefaultValue(1)]
		public virtual int DropDownWidth
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellDropDownWidth, out flag);
				if (!flag)
				{
					return 1;
				}
				return integer;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("DropDownWidth", value, SR.GetString("DataGridViewComboBoxCell_DropDownWidthOutOfRange", new object[]
					{
						1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDropDownWidth, value);
				if (this.OwnsEditingComboBox(base.RowIndex))
				{
					this.EditingComboBox.DropDownWidth = value;
				}
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x0008F902 File Offset: 0x0008DB02
		// (set) Token: 0x06001C8B RID: 7307 RVA: 0x0008F919 File Offset: 0x0008DB19
		private DataGridViewComboBoxEditingControl EditingComboBox
		{
			get
			{
				return (DataGridViewComboBoxEditingControl)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellEditingComboBox);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellEditingComboBox))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellEditingComboBox, value);
				}
			}
		}

		/// <summary>Gets the type of the cell's hosted editing control.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the underlying editing control. This property always returns <see cref="T:System.Windows.Forms.DataGridViewComboBoxEditingControl" />.</returns>
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06001C8C RID: 7308 RVA: 0x0008F941 File Offset: 0x0008DB41
		public override Type EditType
		{
			get
			{
				return DataGridViewComboBoxCell.defaultEditType;
			}
		}

		/// <summary>Gets or sets the flat style appearance of the cell.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlatStyle" /> values. The default value is <see cref="F:System.Windows.Forms.FlatStyle.Standard" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not a valid <see cref="T:System.Windows.Forms.FlatStyle" /> value.</exception>
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06001C8D RID: 7309 RVA: 0x0008F948 File Offset: 0x0008DB48
		// (set) Token: 0x06001C8E RID: 7310 RVA: 0x0008F970 File Offset: 0x0008DB70
		[DefaultValue(FlatStyle.Standard)]
		public FlatStyle FlatStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellFlatStyle, out flag);
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
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellFlatStyle, (int)value);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (set) Token: 0x06001C8F RID: 7311 RVA: 0x0008F9C3 File Offset: 0x0008DBC3
		internal FlatStyle FlatStyleInternal
		{
			set
			{
				if (value != this.FlatStyle)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellFlatStyle, (int)value);
				}
			}
		}

		/// <summary>Gets the class type of the formatted value associated with the cell.</summary>
		/// <returns>The type of the cell's formatted value. This property always returns <see cref="T:System.String" />.</returns>
		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001C90 RID: 7312 RVA: 0x0008F9DF File Offset: 0x0008DBDF
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewComboBoxCell.defaultFormattedValueType;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x0008F9E6 File Offset: 0x0008DBE6
		internal bool HasItems
		{
			get
			{
				return base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellItems) && base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellItems) != null;
			}
		}

		/// <summary>Gets the objects that represent the selection displayed in the drop-down list. </summary>
		/// <returns>An <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" /> containing the selection. </returns>
		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001C92 RID: 7314 RVA: 0x0008FA0F File Offset: 0x0008DC0F
		[Browsable(false)]
		public virtual DataGridViewComboBoxCell.ObjectCollection Items
		{
			get
			{
				return this.GetItems(base.DataGridView);
			}
		}

		/// <summary>Gets or sets the maximum number of items shown in the drop-down list.</summary>
		/// <returns>The number of drop-down list items to allow. The minimum is 1 and the maximum is 100; the default is 8.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 1 or greater than 100 when setting this property.</exception>
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x0008FA20 File Offset: 0x0008DC20
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x0008FA48 File Offset: 0x0008DC48
		[DefaultValue(8)]
		public virtual int MaxDropDownItems
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellMaxDropDownItems, out flag);
				if (flag)
				{
					return integer;
				}
				return 8;
			}
			set
			{
				if (value < 1 || value > 100)
				{
					throw new ArgumentOutOfRangeException("MaxDropDownItems", value, SR.GetString("DataGridViewComboBoxCell_MaxDropDownItemsOutOfRange", new object[]
					{
						1.ToString(CultureInfo.CurrentCulture),
						100.ToString(CultureInfo.CurrentCulture)
					}));
				}
				base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellMaxDropDownItems, value);
				if (this.OwnsEditingComboBox(base.RowIndex))
				{
					this.EditingComboBox.MaxDropDownItems = value;
				}
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x0008FAD0 File Offset: 0x0008DCD0
		private bool PaintXPThemes
		{
			get
			{
				return this.FlatStyle != FlatStyle.Flat && this.FlatStyle != FlatStyle.Popup && base.DataGridView.ApplyVisualStylesToInnerCells;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001C96 RID: 7318 RVA: 0x0008FB02 File Offset: 0x0008DD02
		private static bool PostXPThemesExist
		{
			get
			{
				return VisualStyleRenderer.IsElementDefined(VisualStyleElement.ComboBox.ReadOnlyButton.Normal);
			}
		}

		/// <summary>Gets or sets a value indicating whether the items in the combo box are automatically sorted.</summary>
		/// <returns>
		///     <see langword="true" /> if the combo box is sorted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt was made to sort a cell that is attached to a data source.</exception>
		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x0008FB0E File Offset: 0x0008DD0E
		// (set) Token: 0x06001C98 RID: 7320 RVA: 0x0008FB1C File Offset: 0x0008DD1C
		[DefaultValue(false)]
		public virtual bool Sorted
		{
			get
			{
				return (this.flags & 2) > 0;
			}
			set
			{
				if (value != this.Sorted)
				{
					if (value)
					{
						if (this.DataSource != null)
						{
							throw new ArgumentException(SR.GetString("ComboBoxSortWithDataSource"));
						}
						this.Items.SortInternal();
						this.flags |= 2;
					}
					else
					{
						this.flags = (byte)((int)this.flags & -3);
					}
					if (this.OwnsEditingComboBox(base.RowIndex))
					{
						this.EditingComboBox.Sorted = value;
					}
				}
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x0008FB95 File Offset: 0x0008DD95
		// (set) Token: 0x06001C9A RID: 7322 RVA: 0x0008FBAC File Offset: 0x0008DDAC
		internal DataGridViewComboBoxColumn TemplateComboBoxColumn
		{
			get
			{
				return (DataGridViewComboBoxColumn)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellColumnTemplate);
			}
			set
			{
				base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellColumnTemplate, value);
			}
		}

		/// <summary>Gets or sets a string that specifies where to gather the underlying values used in the drop-down list.</summary>
		/// <returns>A string specifying the name of a property or column. The default value is <see cref="F:System.String.Empty" />, which indicates that this property is ignored.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property is not <see langword="null" /> and the specified value when setting this property is not <see langword="null" /> or <see cref="F:System.String.Empty" /> and does not name a valid property or column in the data source.</exception>
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x0008FBC0 File Offset: 0x0008DDC0
		// (set) Token: 0x06001C9C RID: 7324 RVA: 0x0008FBED File Offset: 0x0008DDED
		[DefaultValue("")]
		public virtual string ValueMember
		{
			get
			{
				object @object = base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellValueMember);
				if (@object == null)
				{
					return string.Empty;
				}
				return (string)@object;
			}
			set
			{
				this.ValueMemberInternal = value;
				if (this.OwnsEditingComboBox(base.RowIndex))
				{
					this.EditingComboBox.ValueMember = value;
					this.InitializeComboBoxText();
					return;
				}
				base.OnCommonChange();
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (set) Token: 0x06001C9D RID: 7325 RVA: 0x0008FC1D File Offset: 0x0008DE1D
		private string ValueMemberInternal
		{
			set
			{
				this.InitializeValueMemberPropertyDescriptor(value);
				if ((value != null && value.Length > 0) || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellValueMember))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellValueMember, value);
				}
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x0008FC55 File Offset: 0x0008DE55
		// (set) Token: 0x06001C9F RID: 7327 RVA: 0x0008FC6C File Offset: 0x0008DE6C
		private PropertyDescriptor ValueMemberProperty
		{
			get
			{
				return (PropertyDescriptor)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellValueMemberProp);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellValueMemberProp))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellValueMemberProp, value);
				}
			}
		}

		/// <summary>Gets or sets the data type of the values in the cell. </summary>
		/// <returns>A <see cref="T:System.Type" /> representing the data type of the value in the cell.</returns>
		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x0008FC94 File Offset: 0x0008DE94
		public override Type ValueType
		{
			get
			{
				if (this.ValueMemberProperty != null)
				{
					return this.ValueMemberProperty.PropertyType;
				}
				if (this.DisplayMemberProperty != null)
				{
					return this.DisplayMemberProperty.PropertyType;
				}
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				return DataGridViewComboBoxCell.defaultValueType;
			}
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0008FCE0 File Offset: 0x0008DEE0
		internal override void CacheEditingControl()
		{
			this.EditingComboBox = (base.DataGridView.EditingControl as DataGridViewComboBoxEditingControl);
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x0008FCF8 File Offset: 0x0008DEF8
		private void CheckDropDownList(int x, int y, int rowIndex)
		{
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
			DataGridViewAdvancedBorderStyle advancedBorderStyle = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, false, false, false, false);
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			Rectangle rectangle = this.BorderWidths(advancedBorderStyle);
			rectangle.X += inheritedStyle.Padding.Left;
			rectangle.Y += inheritedStyle.Padding.Top;
			rectangle.Width += inheritedStyle.Padding.Right;
			rectangle.Height += inheritedStyle.Padding.Bottom;
			Size size = this.GetSize(rowIndex);
			Size size2 = new Size(size.Width - rectangle.X - rectangle.Width, size.Height - rectangle.Y - rectangle.Height);
			int num;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				num = Math.Min(this.GetDropDownButtonHeight(graphics, inheritedStyle), size2.Height - 2);
			}
			int num2 = Math.Min(SystemInformation.HorizontalScrollBarThumbWidth, size2.Width - 6 - 1);
			if (num > 0 && num2 > 0 && y >= rectangle.Y + 1 && y <= rectangle.Y + 1 + num)
			{
				if (base.DataGridView.RightToLeftInternal)
				{
					if (x >= rectangle.X + 1 && x <= rectangle.X + num2 + 1)
					{
						this.EditingComboBox.DroppedDown = true;
						return;
					}
				}
				else if (x >= size.Width - rectangle.Width - num2 - 1 && x <= size.Width - rectangle.Width - 1)
				{
					this.EditingComboBox.DroppedDown = true;
				}
			}
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0008FED4 File Offset: 0x0008E0D4
		private void CheckNoDataSource()
		{
			if (this.DataSource != null)
			{
				throw new ArgumentException(SR.GetString("DataSourceLocksItems"));
			}
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0008FEF0 File Offset: 0x0008E0F0
		private void ComboBox_DropDown(object sender, EventArgs e)
		{
			ComboBox editingComboBox = this.EditingComboBox;
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn = base.OwningColumn as DataGridViewComboBoxColumn;
			if (dataGridViewComboBoxColumn != null)
			{
				DataGridViewAutoSizeColumnMode inheritedAutoSizeMode = dataGridViewComboBoxColumn.GetInheritedAutoSizeMode(base.DataGridView);
				if (inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.ColumnHeader && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.Fill && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.None)
				{
					if (this.DropDownWidth == 1)
					{
						if (DataGridViewComboBoxCell.cachedDropDownWidth == -1)
						{
							int num = -1;
							if ((this.HasItems || this.CreateItemsFromDataSource) && this.Items.Count > 0)
							{
								foreach (object item in this.Items)
								{
									Size size = TextRenderer.MeasureText(editingComboBox.GetItemText(item), editingComboBox.Font);
									if (size.Width > num)
									{
										num = size.Width;
									}
								}
							}
							DataGridViewComboBoxCell.cachedDropDownWidth = num + 2 + SystemInformation.VerticalScrollBarWidth;
						}
						UnsafeNativeMethods.SendMessage(new HandleRef(editingComboBox, editingComboBox.Handle), 352, DataGridViewComboBoxCell.cachedDropDownWidth, 0);
						return;
					}
				}
				else
				{
					int num2 = (int)((long)UnsafeNativeMethods.SendMessage(new HandleRef(editingComboBox, editingComboBox.Handle), 351, 0, 0));
					if (num2 != this.DropDownWidth)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(editingComboBox, editingComboBox.Handle), 352, this.DropDownWidth, 0);
					}
				}
			}
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</returns>
		// Token: 0x06001CA5 RID: 7333 RVA: 0x00090054 File Offset: 0x0008E254
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewComboBoxCell dataGridViewComboBoxCell;
			if (type == DataGridViewComboBoxCell.cellType)
			{
				dataGridViewComboBoxCell = new DataGridViewComboBoxCell();
			}
			else
			{
				dataGridViewComboBoxCell = (DataGridViewComboBoxCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewComboBoxCell);
			dataGridViewComboBoxCell.DropDownWidth = this.DropDownWidth;
			dataGridViewComboBoxCell.MaxDropDownItems = this.MaxDropDownItems;
			dataGridViewComboBoxCell.CreateItemsFromDataSource = false;
			dataGridViewComboBoxCell.DataSource = this.DataSource;
			dataGridViewComboBoxCell.DisplayMember = this.DisplayMember;
			dataGridViewComboBoxCell.ValueMember = this.ValueMember;
			if (this.HasItems && this.DataSource == null && this.Items.Count > 0)
			{
				dataGridViewComboBoxCell.Items.AddRangeInternal(this.Items.InnerArray.ToArray());
			}
			dataGridViewComboBoxCell.AutoComplete = this.AutoComplete;
			dataGridViewComboBoxCell.Sorted = this.Sorted;
			dataGridViewComboBoxCell.FlatStyleInternal = this.FlatStyle;
			dataGridViewComboBoxCell.DisplayStyleInternal = this.DisplayStyle;
			dataGridViewComboBoxCell.DisplayStyleForCurrentCellOnlyInternal = this.DisplayStyleForCurrentCellOnly;
			return dataGridViewComboBoxCell;
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x00090149 File Offset: 0x0008E349
		// (set) Token: 0x06001CA7 RID: 7335 RVA: 0x00090156 File Offset: 0x0008E356
		private bool CreateItemsFromDataSource
		{
			get
			{
				return (this.flags & 4) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 4;
					return;
				}
				this.flags = (byte)((int)this.flags & -5);
			}
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0009017B File Offset: 0x0008E37B
		private void DataSource_Disposed(object sender, EventArgs e)
		{
			this.DataSource = null;
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00090184 File Offset: 0x0008E384
		private void DataSource_Initialized(object sender, EventArgs e)
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
			}
			this.flags = (byte)((int)this.flags & -17);
			this.InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
			this.InitializeValueMemberPropertyDescriptor(this.ValueMember);
		}

		/// <summary>Removes the cell's editing control from the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x06001CAA RID: 7338 RVA: 0x000901DC File Offset: 0x0008E3DC
		public override void DetachEditingControl()
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null || dataGridView.EditingControl == null)
			{
				throw new InvalidOperationException();
			}
			if (this.EditingComboBox != null && (this.flags & 32) != 0)
			{
				this.EditingComboBox.DropDown -= this.ComboBox_DropDown;
				this.flags = (byte)((int)this.flags & -33);
			}
			this.EditingComboBox = null;
			base.DetachEditingControl();
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		// Token: 0x06001CAB RID: 7339 RVA: 0x00090248 File Offset: 0x0008E448
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
			object editedFormattedValue = base.GetEditedFormattedValue(value, rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting);
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates elementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out elementState, out rectangle);
			Rectangle rectangle2;
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, elementState, editedFormattedValue, null, cellStyle, advancedBorderStyle, out rectangle2, DataGridViewPaintParts.ContentForeground, true, false, false, false);
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000902BC File Offset: 0x0008E4BC
		private CurrencyManager GetDataManager(DataGridView dataGridView)
		{
			CurrencyManager currencyManager = (CurrencyManager)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellDataManager);
			if (currencyManager == null && this.DataSource != null && dataGridView != null && dataGridView.BindingContext != null && this.DataSource != Convert.DBNull)
			{
				ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
				if (supportInitializeNotification != null && !supportInitializeNotification.IsInitialized)
				{
					if ((this.flags & 16) == 0)
					{
						supportInitializeNotification.Initialized += this.DataSource_Initialized;
						this.flags |= 16;
					}
				}
				else
				{
					currencyManager = (CurrencyManager)dataGridView.BindingContext[this.DataSource];
					this.DataManager = currencyManager;
				}
			}
			return currencyManager;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x0009036C File Offset: 0x0008E56C
		private int GetDropDownButtonHeight(Graphics graphics, DataGridViewCellStyle cellStyle)
		{
			int num = 4;
			if (this.PaintXPThemes)
			{
				if (DataGridViewComboBoxCell.PostXPThemesExist)
				{
					num = 8;
				}
				else
				{
					num = 6;
				}
			}
			return DataGridViewCell.MeasureTextHeight(graphics, " ", cellStyle.Font, int.MaxValue, TextFormatFlags.Default) + num;
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's error icon, if one is displayed.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's error icon, if one is displayed; otherwise, <see cref="F:System.Drawing.Rectangle.Empty" />.</returns>
		// Token: 0x06001CAE RID: 7342 RVA: 0x000903AC File Offset: 0x0008E5AC
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
			object value = this.GetValue(rowIndex);
			object editedFormattedValue = base.GetEditedFormattedValue(value, rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting);
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates elementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out elementState, out rectangle);
			Rectangle rectangle2;
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, elementState, editedFormattedValue, this.GetErrorText(rowIndex), cellStyle, advancedBorderStyle, out rectangle2, DataGridViewPaintParts.ContentForeground, false, true, false, false);
		}

		/// <summary>Gets the formatted value of the cell's data. </summary>
		/// <param name="value">The value to be formatted. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> in effect for the cell.</param>
		/// <param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> associated with the value type that provides custom conversion to the formatted value type, or <see langword="null" /> if no such custom conversion is needed.</param>
		/// <param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> associated with the formatted value type that provides custom conversion from the value type, or <see langword="null" /> if no such custom conversion is needed.</param>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values describing the context in which the formatted value is needed.</param>
		/// <returns>The value of the cell's data after formatting has been applied or <see langword="null" /> if the cell is not part of a <see cref="T:System.Windows.Forms.DataGridView" /> control.</returns>
		/// <exception cref="T:System.Exception">Formatting failed and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError" /> event of the <see cref="T:System.Windows.Forms.DataGridView" /> control or the handler set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException" /> property to <see langword="true" />. The exception object can typically be cast to type <see cref="T:System.FormatException" /> for type conversion errors or to type <see cref="T:System.ArgumentException" /> if <paramref name="value" /> cannot be found in the <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> or the <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.Items" /> collection. </exception>
		// Token: 0x06001CAF RID: 7343 RVA: 0x00090440 File Offset: 0x0008E640
		protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
		{
			if (valueTypeConverter == null)
			{
				if (this.ValueMemberProperty != null)
				{
					valueTypeConverter = this.ValueMemberProperty.Converter;
				}
				else if (this.DisplayMemberProperty != null)
				{
					valueTypeConverter = this.DisplayMemberProperty.Converter;
				}
			}
			if (value == null || (this.ValueType != null && !this.ValueType.IsAssignableFrom(value.GetType()) && value != DBNull.Value))
			{
				if (value == null)
				{
					return base.GetFormattedValue(null, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
				}
				if (base.DataGridView != null)
				{
					DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs = new DataGridViewDataErrorEventArgs(new FormatException(SR.GetString("DataGridViewComboBoxCell_InvalidValue")), base.ColumnIndex, rowIndex, context);
					base.RaiseDataError(dataGridViewDataErrorEventArgs);
					if (dataGridViewDataErrorEventArgs.ThrowException)
					{
						throw dataGridViewDataErrorEventArgs.Exception;
					}
				}
				return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
			}
			else
			{
				string text = value as string;
				if ((this.DataManager != null && (this.ValueMemberProperty != null || this.DisplayMemberProperty != null)) || !string.IsNullOrEmpty(this.ValueMember) || !string.IsNullOrEmpty(this.DisplayMember))
				{
					object value2;
					if (!this.LookupDisplayValue(rowIndex, value, out value2))
					{
						if (value == DBNull.Value)
						{
							value2 = DBNull.Value;
						}
						else if (text != null && string.IsNullOrEmpty(text) && this.DisplayType == typeof(string))
						{
							value2 = string.Empty;
						}
						else if (base.DataGridView != null)
						{
							DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs2 = new DataGridViewDataErrorEventArgs(new ArgumentException(SR.GetString("DataGridViewComboBoxCell_InvalidValue")), base.ColumnIndex, rowIndex, context);
							base.RaiseDataError(dataGridViewDataErrorEventArgs2);
							if (dataGridViewDataErrorEventArgs2.ThrowException)
							{
								throw dataGridViewDataErrorEventArgs2.Exception;
							}
							if (this.OwnsEditingComboBox(rowIndex))
							{
								((IDataGridViewEditingControl)this.EditingComboBox).EditingControlValueChanged = true;
								base.DataGridView.NotifyCurrentCellDirty(true);
							}
						}
					}
					return base.GetFormattedValue(value2, rowIndex, ref cellStyle, this.DisplayTypeConverter, formattedValueTypeConverter, context);
				}
				if (!this.Items.Contains(value) && value != DBNull.Value && (!(value is string) || !string.IsNullOrEmpty(text)))
				{
					if (base.DataGridView != null)
					{
						DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs3 = new DataGridViewDataErrorEventArgs(new ArgumentException(SR.GetString("DataGridViewComboBoxCell_InvalidValue")), base.ColumnIndex, rowIndex, context);
						base.RaiseDataError(dataGridViewDataErrorEventArgs3);
						if (dataGridViewDataErrorEventArgs3.ThrowException)
						{
							throw dataGridViewDataErrorEventArgs3.Exception;
						}
					}
					if (this.Items.Count > 0)
					{
						value = this.Items[0];
					}
					else
					{
						value = string.Empty;
					}
				}
				return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
			}
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x0009069C File Offset: 0x0008E89C
		internal string GetItemDisplayText(object item)
		{
			object itemDisplayValue = this.GetItemDisplayValue(item);
			if (itemDisplayValue == null)
			{
				return string.Empty;
			}
			return Convert.ToString(itemDisplayValue, CultureInfo.CurrentCulture);
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000906C8 File Offset: 0x0008E8C8
		internal object GetItemDisplayValue(object item)
		{
			bool flag = false;
			object result = null;
			if (this.DisplayMemberProperty != null)
			{
				result = this.DisplayMemberProperty.GetValue(item);
				flag = true;
			}
			else if (this.ValueMemberProperty != null)
			{
				result = this.ValueMemberProperty.GetValue(item);
				flag = true;
			}
			else if (!string.IsNullOrEmpty(this.DisplayMember))
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(item).Find(this.DisplayMember, true);
				if (propertyDescriptor != null)
				{
					result = propertyDescriptor.GetValue(item);
					flag = true;
				}
			}
			else if (!string.IsNullOrEmpty(this.ValueMember))
			{
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(item).Find(this.ValueMember, true);
				if (propertyDescriptor2 != null)
				{
					result = propertyDescriptor2.GetValue(item);
					flag = true;
				}
			}
			if (!flag)
			{
				result = item;
			}
			return result;
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x00090770 File Offset: 0x0008E970
		internal DataGridViewComboBoxCell.ObjectCollection GetItems(DataGridView dataGridView)
		{
			DataGridViewComboBoxCell.ObjectCollection objectCollection = (DataGridViewComboBoxCell.ObjectCollection)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellItems);
			if (objectCollection == null)
			{
				objectCollection = new DataGridViewComboBoxCell.ObjectCollection(this);
				base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellItems, objectCollection);
			}
			if (this.CreateItemsFromDataSource)
			{
				objectCollection.ClearInternal();
				CurrencyManager dataManager = this.GetDataManager(dataGridView);
				if (dataManager != null && dataManager.Count != -1)
				{
					object[] array = new object[dataManager.Count];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = dataManager[i];
					}
					objectCollection.AddRangeInternal(array);
				}
				if (dataManager != null || (this.flags & 16) == 0)
				{
					this.CreateItemsFromDataSource = false;
				}
			}
			return objectCollection;
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x00090814 File Offset: 0x0008EA14
		internal object GetItemValue(object item)
		{
			bool flag = false;
			object result = null;
			if (this.ValueMemberProperty != null)
			{
				result = this.ValueMemberProperty.GetValue(item);
				flag = true;
			}
			else if (this.DisplayMemberProperty != null)
			{
				result = this.DisplayMemberProperty.GetValue(item);
				flag = true;
			}
			else if (!string.IsNullOrEmpty(this.ValueMember))
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(item).Find(this.ValueMember, true);
				if (propertyDescriptor != null)
				{
					result = propertyDescriptor.GetValue(item);
					flag = true;
				}
			}
			if (!flag && !string.IsNullOrEmpty(this.DisplayMember))
			{
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(item).Find(this.DisplayMember, true);
				if (propertyDescriptor2 != null)
				{
					result = propertyDescriptor2.GetValue(item);
					flag = true;
				}
			}
			if (!flag)
			{
				result = item;
			}
			return result;
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		// Token: 0x06001CB4 RID: 7348 RVA: 0x000908BC File Offset: 0x0008EABC
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
			Size result = Size.Empty;
			DataGridViewFreeDimension freeDimensionFromConstraint = DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize);
			Rectangle stdBorderWidths = base.StdBorderWidths;
			int num = stdBorderWidths.Left + stdBorderWidths.Width + cellStyle.Padding.Horizontal;
			int num2 = stdBorderWidths.Top + stdBorderWidths.Height + cellStyle.Padding.Vertical;
			TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			string text = base.GetFormattedValue(rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.PreferredSize) as string;
			if (!string.IsNullOrEmpty(text))
			{
				result = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags);
			}
			else
			{
				result = DataGridViewCell.MeasureTextSize(graphics, " ", cellStyle.Font, textFormatFlags);
			}
			if (freeDimensionFromConstraint == DataGridViewFreeDimension.Height)
			{
				result.Width = 0;
			}
			else if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
			{
				result.Height = 0;
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				result.Width += SystemInformation.HorizontalScrollBarThumbWidth + 1 + 6 + num;
				if (base.DataGridView.ShowCellErrors)
				{
					result.Width = Math.Max(result.Width, num + SystemInformation.HorizontalScrollBarThumbWidth + 1 + 8 + (int)DataGridViewCell.iconsWidth);
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				if (this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup)
				{
					result.Height += 6;
				}
				else
				{
					result.Height += 8;
				}
				result.Height += num2;
				if (base.DataGridView.ShowCellErrors)
				{
					result.Height = Math.Max(result.Height, num2 + 8 + (int)DataGridViewCell.iconsHeight);
				}
			}
			return result;
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x00090A74 File Offset: 0x0008EC74
		private void InitializeComboBoxText()
		{
			((IDataGridViewEditingControl)this.EditingComboBox).EditingControlValueChanged = false;
			int editingControlRowIndex = ((IDataGridViewEditingControl)this.EditingComboBox).EditingControlRowIndex;
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, editingControlRowIndex, false);
			this.EditingComboBox.Text = (string)this.GetFormattedValue(this.GetValue(editingControlRowIndex), editingControlRowIndex, ref inheritedStyle, null, null, DataGridViewDataErrorContexts.Formatting);
		}

		/// <summary>Attaches and initializes the hosted editing control.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <param name="initialFormattedValue">The initial value to be displayed in the control.</param>
		/// <param name="dataGridViewCellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that determines the appearance of the hosted control.</param>
		// Token: 0x06001CB6 RID: 7350 RVA: 0x00090AC8 File Offset: 0x0008ECC8
		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			ComboBox comboBox = base.DataGridView.EditingControl as ComboBox;
			if (comboBox != null)
			{
				if ((this.GetInheritedState(rowIndex) & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
				{
					base.DataGridView.EditingPanel.BackColor = dataGridViewCellStyle.SelectionBackColor;
				}
				IntPtr handle;
				if (comboBox.ParentInternal != null)
				{
					handle = comboBox.ParentInternal.Handle;
				}
				handle = comboBox.Handle;
				comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				comboBox.FormattingEnabled = true;
				comboBox.MaxDropDownItems = this.MaxDropDownItems;
				comboBox.DropDownWidth = this.DropDownWidth;
				comboBox.DataSource = null;
				comboBox.ValueMember = null;
				comboBox.Items.Clear();
				comboBox.DataSource = this.DataSource;
				comboBox.DisplayMember = this.DisplayMember;
				comboBox.ValueMember = this.ValueMember;
				if (this.HasItems && this.DataSource == null && this.Items.Count > 0)
				{
					comboBox.Items.AddRange(this.Items.InnerArray.ToArray());
				}
				comboBox.Sorted = this.Sorted;
				comboBox.FlatStyle = this.FlatStyle;
				if (this.AutoComplete)
				{
					comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
					comboBox.AutoCompleteMode = AutoCompleteMode.Append;
				}
				else
				{
					comboBox.AutoCompleteMode = AutoCompleteMode.None;
					comboBox.AutoCompleteSource = AutoCompleteSource.None;
				}
				string text = initialFormattedValue as string;
				if (text == null)
				{
					text = string.Empty;
				}
				comboBox.Text = text;
				if ((this.flags & 32) == 0)
				{
					comboBox.DropDown += this.ComboBox_DropDown;
					this.flags |= 32;
				}
				DataGridViewComboBoxCell.cachedDropDownWidth = -1;
				this.EditingComboBox = (base.DataGridView.EditingControl as DataGridViewComboBoxEditingControl);
				if (base.GetHeight(rowIndex) > 21)
				{
					Rectangle cellDisplayRectangle = base.DataGridView.GetCellDisplayRectangle(base.ColumnIndex, rowIndex, true);
					cellDisplayRectangle.Y += 21;
					cellDisplayRectangle.Height -= 21;
					base.DataGridView.Invalidate(cellDisplayRectangle);
				}
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x00090CC4 File Offset: 0x0008EEC4
		private void InitializeDisplayMemberPropertyDescriptor(string displayMember)
		{
			if (this.DataManager != null)
			{
				if (string.IsNullOrEmpty(displayMember))
				{
					this.DisplayMemberProperty = null;
					return;
				}
				BindingMemberInfo bindingMemberInfo = new BindingMemberInfo(displayMember);
				this.DataManager = (base.DataGridView.BindingContext[this.DataSource, bindingMemberInfo.BindingPath] as CurrencyManager);
				PropertyDescriptorCollection itemProperties = this.DataManager.GetItemProperties();
				PropertyDescriptor propertyDescriptor = itemProperties.Find(bindingMemberInfo.BindingField, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewComboBoxCell_FieldNotFound", new object[]
					{
						displayMember
					}));
				}
				this.DisplayMemberProperty = propertyDescriptor;
			}
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x00090D58 File Offset: 0x0008EF58
		private void InitializeValueMemberPropertyDescriptor(string valueMember)
		{
			if (this.DataManager != null)
			{
				if (string.IsNullOrEmpty(valueMember))
				{
					this.ValueMemberProperty = null;
					return;
				}
				BindingMemberInfo bindingMemberInfo = new BindingMemberInfo(valueMember);
				this.DataManager = (base.DataGridView.BindingContext[this.DataSource, bindingMemberInfo.BindingPath] as CurrencyManager);
				PropertyDescriptorCollection itemProperties = this.DataManager.GetItemProperties();
				PropertyDescriptor propertyDescriptor = itemProperties.Find(bindingMemberInfo.BindingField, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewComboBoxCell_FieldNotFound", new object[]
					{
						valueMember
					}));
				}
				this.ValueMemberProperty = propertyDescriptor;
			}
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x00090DEC File Offset: 0x0008EFEC
		private object ItemFromComboBoxDataSource(PropertyDescriptor property, object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			object result = null;
			if (this.DataManager.List is IBindingList && ((IBindingList)this.DataManager.List).SupportsSearching)
			{
				int num = ((IBindingList)this.DataManager.List).Find(property, key);
				if (num != -1)
				{
					result = this.DataManager.List[num];
				}
			}
			else
			{
				for (int i = 0; i < this.DataManager.List.Count; i++)
				{
					object obj = this.DataManager.List[i];
					object value = property.GetValue(obj);
					if (key.Equals(value))
					{
						result = obj;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x00090EA8 File Offset: 0x0008F0A8
		private object ItemFromComboBoxItems(int rowIndex, string field, object key)
		{
			object obj = null;
			if (this.OwnsEditingComboBox(rowIndex))
			{
				obj = this.EditingComboBox.SelectedItem;
				object obj2 = null;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj).Find(field, true);
				if (propertyDescriptor != null)
				{
					obj2 = propertyDescriptor.GetValue(obj);
				}
				if (obj2 == null || !obj2.Equals(key))
				{
					obj = null;
				}
			}
			if (obj == null)
			{
				foreach (object obj3 in this.Items)
				{
					object obj4 = null;
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(obj3).Find(field, true);
					if (propertyDescriptor2 != null)
					{
						obj4 = propertyDescriptor2.GetValue(obj3);
					}
					if (obj4 != null && obj4.Equals(key))
					{
						obj = obj3;
						break;
					}
				}
			}
			if (obj == null)
			{
				if (this.OwnsEditingComboBox(rowIndex))
				{
					obj = this.EditingComboBox.SelectedItem;
					if (obj == null || !obj.Equals(key))
					{
						obj = null;
					}
				}
				if (obj == null && this.Items.Contains(key))
				{
					obj = key;
				}
			}
			return obj;
		}

		/// <summary>Determines if edit mode should be started based on the given key.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that represents the key that was pressed.</param>
		/// <returns>
		///     <see langword="true" /> if edit mode should be started; otherwise, <see langword="false" />. </returns>
		// Token: 0x06001CBB RID: 7355 RVA: 0x00090FAC File Offset: 0x0008F1AC
		public override bool KeyEntersEditMode(KeyEventArgs e)
		{
			return (((char.IsLetterOrDigit((char)e.KeyCode) && (e.KeyCode < Keys.F1 || e.KeyCode > Keys.F24)) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.Divide) || (e.KeyCode >= Keys.OemSemicolon && e.KeyCode <= Keys.OemBackslash) || (e.KeyCode == Keys.Space && !e.Shift) || e.KeyCode == Keys.F4 || ((e.KeyCode == Keys.Down || e.KeyCode == Keys.Up) && e.Alt)) && (!e.Alt || e.KeyCode == Keys.Down || e.KeyCode == Keys.Up) && !e.Control) || base.KeyEntersEditMode(e);
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x00091074 File Offset: 0x0008F274
		private bool LookupDisplayValue(int rowIndex, object value, out object displayValue)
		{
			object obj;
			if (this.DisplayMemberProperty != null || this.ValueMemberProperty != null)
			{
				obj = this.ItemFromComboBoxDataSource((this.ValueMemberProperty != null) ? this.ValueMemberProperty : this.DisplayMemberProperty, value);
			}
			else
			{
				obj = this.ItemFromComboBoxItems(rowIndex, string.IsNullOrEmpty(this.ValueMember) ? this.DisplayMember : this.ValueMember, value);
			}
			if (obj == null)
			{
				displayValue = null;
				return false;
			}
			displayValue = this.GetItemDisplayValue(obj);
			return true;
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000910EC File Offset: 0x0008F2EC
		private bool LookupValue(object formattedValue, out object value)
		{
			if (formattedValue == null)
			{
				value = null;
				return true;
			}
			object obj;
			if (this.DisplayMemberProperty != null || this.ValueMemberProperty != null)
			{
				obj = this.ItemFromComboBoxDataSource((this.DisplayMemberProperty != null) ? this.DisplayMemberProperty : this.ValueMemberProperty, formattedValue);
			}
			else
			{
				obj = this.ItemFromComboBoxItems(base.RowIndex, string.IsNullOrEmpty(this.DisplayMember) ? this.ValueMember : this.DisplayMember, formattedValue);
			}
			if (obj == null)
			{
				value = null;
				return false;
			}
			value = this.GetItemValue(obj);
			return true;
		}

		/// <summary>Called when the <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the cell changes.</summary>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property is not <see langword="null" /> and the value of either the <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DisplayMember" /> property or the <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.ValueMember" /> property is not <see langword="null" /> or <see cref="F:System.String.Empty" /> and does not name a valid property or column in the data source.</exception>
		// Token: 0x06001CBE RID: 7358 RVA: 0x0009116E File Offset: 0x0008F36E
		protected override void OnDataGridViewChanged()
		{
			if (base.DataGridView != null)
			{
				this.InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
				this.InitializeValueMemberPropertyDescriptor(this.ValueMember);
			}
			base.OnDataGridViewChanged();
		}

		/// <summary>Called when the focus moves to a cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if a user action moved focus to the cell; <see langword="false" /> if a programmatic operation moved focus to the cell.</param>
		// Token: 0x06001CBF RID: 7359 RVA: 0x00091196 File Offset: 0x0008F396
		protected override void OnEnter(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (throughMouseClick && base.DataGridView.EditMode != DataGridViewEditMode.EditOnEnter)
			{
				this.flags |= 1;
			}
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000911C0 File Offset: 0x0008F3C0
		private void OnItemsCollectionChanged()
		{
			if (this.TemplateComboBoxColumn != null)
			{
				this.TemplateComboBoxColumn.OnItemsCollectionChanged();
			}
			DataGridViewComboBoxCell.cachedDropDownWidth = -1;
			if (this.OwnsEditingComboBox(base.RowIndex))
			{
				this.InitializeComboBoxText();
				return;
			}
			base.OnCommonChange();
		}

		/// <summary>Called when the focus moves from a cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <param name="throughMouseClick">
		///       <see langword="true" /> if a user action moved focus from the cell; <see langword="false" /> if a programmatic operation moved focus from the cell.</param>
		// Token: 0x06001CC1 RID: 7361 RVA: 0x000911F6 File Offset: 0x0008F3F6
		protected override void OnLeave(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			this.flags = (byte)((int)this.flags & -2);
		}

		/// <summary>Called when the user clicks a mouse button while the pointer is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06001CC2 RID: 7362 RVA: 0x00091214 File Offset: 0x0008F414
		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == e.ColumnIndex && currentCellAddress.Y == e.RowIndex)
			{
				if ((this.flags & 1) != 0)
				{
					this.flags = (byte)((int)this.flags & -2);
					return;
				}
				if ((this.EditingComboBox == null || !this.EditingComboBox.DroppedDown) && base.DataGridView.EditMode != DataGridViewEditMode.EditProgrammatically && base.DataGridView.BeginEdit(true) && this.EditingComboBox != null && this.DisplayStyle != DataGridViewComboBoxDisplayStyle.Nothing)
				{
					this.CheckDropDownList(e.X, e.Y, e.RowIndex);
				}
			}
		}

		/// <summary>Called when the mouse pointer moves over a cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		// Token: 0x06001CC3 RID: 7363 RVA: 0x000912CC File Offset: 0x0008F4CC
		protected override void OnMouseEnter(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.DisplayStyle == DataGridViewComboBoxDisplayStyle.ComboBox && this.FlatStyle == FlatStyle.Popup)
			{
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
			base.OnMouseEnter(rowIndex);
		}

		/// <summary>Called when the mouse pointer leaves the cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		// Token: 0x06001CC4 RID: 7364 RVA: 0x00091304 File Offset: 0x0008F504
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (DataGridViewComboBoxCell.mouseInDropDownButtonBounds)
			{
				DataGridViewComboBoxCell.mouseInDropDownButtonBounds = false;
				if (base.ColumnIndex >= 0 && rowIndex >= 0 && (this.FlatStyle == FlatStyle.Standard || this.FlatStyle == FlatStyle.System) && base.DataGridView.ApplyVisualStylesToInnerCells)
				{
					base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
				}
			}
			if (this.DisplayStyle == DataGridViewComboBoxDisplayStyle.ComboBox && this.FlatStyle == FlatStyle.Popup)
			{
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
			base.OnMouseEnter(rowIndex);
		}

		/// <summary>Called when the mouse pointer moves within a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06001CC5 RID: 7365 RVA: 0x00091390 File Offset: 0x0008F590
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if ((this.FlatStyle == FlatStyle.Standard || this.FlatStyle == FlatStyle.System) && base.DataGridView.ApplyVisualStylesToInnerCells)
			{
				int rowIndex = e.RowIndex;
				DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
				bool singleVerticalBorderAdded = !base.DataGridView.RowHeadersVisible && base.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
				bool singleHorizontalBorderAdded = !base.DataGridView.ColumnHeadersVisible && base.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
				bool isFirstDisplayedColumn = rowIndex == base.DataGridView.FirstDisplayedScrollingRowIndex;
				bool isFirstDisplayedRow = base.OwningColumn.Index == base.DataGridView.FirstDisplayedColumnIndex;
				bool flag = base.OwningColumn.Index == base.DataGridView.FirstDisplayedScrollingColumnIndex;
				DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
				DataGridViewAdvancedBorderStyle advancedBorderStyle = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
				Rectangle cellDisplayRectangle = base.DataGridView.GetCellDisplayRectangle(base.OwningColumn.Index, rowIndex, false);
				if (flag)
				{
					cellDisplayRectangle.X -= base.DataGridView.FirstDisplayedScrollingColumnHiddenWidth;
					cellDisplayRectangle.Width += base.DataGridView.FirstDisplayedScrollingColumnHiddenWidth;
				}
				DataGridViewElementStates rowState = base.DataGridView.Rows.GetRowState(rowIndex);
				DataGridViewElementStates dataGridViewElementStates = base.CellStateFromColumnRowStates(rowState);
				dataGridViewElementStates |= this.State;
				Rectangle rectangle;
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					this.PaintPrivate(graphics, cellDisplayRectangle, cellDisplayRectangle, rowIndex, dataGridViewElementStates, null, null, inheritedStyle, advancedBorderStyle, out rectangle, DataGridViewPaintParts.ContentForeground, false, false, true, false);
				}
				bool flag2 = rectangle.Contains(base.DataGridView.PointToClient(Control.MousePosition));
				if (flag2 != DataGridViewComboBoxCell.mouseInDropDownButtonBounds)
				{
					DataGridViewComboBoxCell.mouseInDropDownButtonBounds = flag2;
					base.DataGridView.InvalidateCell(e.ColumnIndex, rowIndex);
				}
			}
			base.OnMouseMove(e);
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x0009158C File Offset: 0x0008F78C
		private bool OwnsEditingComboBox(int rowIndex)
		{
			return rowIndex != -1 && this.EditingComboBox != null && rowIndex == ((IDataGridViewEditingControl)this.EditingComboBox).EditingControlRowIndex;
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</summary>
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
		// Token: 0x06001CC7 RID: 7367 RVA: 0x000915AC File Offset: 0x0008F7AC
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			Rectangle rectangle;
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, elementState, formattedValue, errorText, cellStyle, advancedBorderStyle, out rectangle, paintParts, false, false, false, true);
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000915E8 File Offset: 0x0008F7E8
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, out Rectangle dropDownButtonRect, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool computeDropDownButtonRect, bool paint)
		{
			Rectangle result = Rectangle.Empty;
			dropDownButtonRect = Rectangle.Empty;
			bool flag = this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup;
			bool flag2 = this.FlatStyle == FlatStyle.Popup && base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex;
			bool flag3 = !flag && base.DataGridView.ApplyVisualStylesToInnerCells;
			bool flag4 = flag3 && DataGridViewComboBoxCell.PostXPThemesExist;
			ComboBoxState state = ComboBoxState.Normal;
			if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewComboBoxCell.mouseInDropDownButtonBounds)
			{
				state = ComboBoxState.Hot;
			}
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle = this.BorderWidths(advancedBorderStyle);
			Rectangle rectangle2 = cellBounds;
			rectangle2.Offset(rectangle.X, rectangle.Y);
			rectangle2.Width -= rectangle.Right;
			rectangle2.Height -= rectangle.Bottom;
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			bool flag5 = currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex;
			bool flag6 = flag5 && base.DataGridView.EditingControl != null;
			bool flag7 = (elementState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
			bool flag8 = this.DisplayStyle == DataGridViewComboBoxDisplayStyle.ComboBox && ((this.DisplayStyleForCurrentCellOnly && flag5) || !this.DisplayStyleForCurrentCellOnly);
			bool flag9 = this.DisplayStyle != DataGridViewComboBoxDisplayStyle.Nothing && ((this.DisplayStyleForCurrentCellOnly && flag5) || !this.DisplayStyleForCurrentCellOnly);
			SolidBrush cachedBrush;
			if (DataGridViewCell.PaintSelectionBackground(paintParts) && flag7 && !flag6)
			{
				cachedBrush = base.DataGridView.GetCachedBrush(cellStyle.SelectionBackColor);
			}
			else
			{
				cachedBrush = base.DataGridView.GetCachedBrush(cellStyle.BackColor);
			}
			if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255 && rectangle2.Width > 0 && rectangle2.Height > 0)
			{
				DataGridViewCell.PaintPadding(g, rectangle2, cellStyle, cachedBrush, base.DataGridView.RightToLeftInternal);
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
			if (paint && rectangle2.Width > 0 && rectangle2.Height > 0)
			{
				if (flag3 && flag8)
				{
					if (flag4 && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
					{
						g.FillRectangle(cachedBrush, rectangle2.Left, rectangle2.Top, rectangle2.Width, rectangle2.Height);
					}
					if (DataGridViewCell.PaintContentBackground(paintParts))
					{
						if (flag4)
						{
							DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawBorder(g, rectangle2);
						}
						else
						{
							DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawTextBox(g, rectangle2, state);
						}
					}
					if (!flag4 && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255 && rectangle2.Width > 2 && rectangle2.Height > 2)
					{
						g.FillRectangle(cachedBrush, rectangle2.Left + 1, rectangle2.Top + 1, rectangle2.Width - 2, rectangle2.Height - 2);
					}
				}
				else if (DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
				{
					if (flag4 && flag9 && !flag8)
					{
						g.DrawRectangle(SystemPens.ControlLightLight, new Rectangle(rectangle2.X, rectangle2.Y, rectangle2.Width - 1, rectangle2.Height - 1));
					}
					else
					{
						g.FillRectangle(cachedBrush, rectangle2.Left, rectangle2.Top, rectangle2.Width, rectangle2.Height);
					}
				}
			}
			int num = Math.Min(SystemInformation.HorizontalScrollBarThumbWidth, rectangle2.Width - 6 - 1);
			if (!flag6)
			{
				int num2;
				if (flag3 || flag)
				{
					num2 = Math.Min(this.GetDropDownButtonHeight(g, cellStyle), flag4 ? rectangle2.Height : (rectangle2.Height - 2));
				}
				else
				{
					num2 = Math.Min(this.GetDropDownButtonHeight(g, cellStyle), rectangle2.Height - 4);
				}
				if (num > 0 && num2 > 0)
				{
					Rectangle rectangle3;
					if (flag3 || flag)
					{
						if (flag4)
						{
							rectangle3 = new Rectangle(base.DataGridView.RightToLeftInternal ? rectangle2.Left : (rectangle2.Right - num), rectangle2.Top, num, num2);
						}
						else
						{
							rectangle3 = new Rectangle(base.DataGridView.RightToLeftInternal ? (rectangle2.Left + 1) : (rectangle2.Right - num - 1), rectangle2.Top + 1, num, num2);
						}
					}
					else
					{
						rectangle3 = new Rectangle(base.DataGridView.RightToLeftInternal ? (rectangle2.Left + 2) : (rectangle2.Right - num - 2), rectangle2.Top + 2, num, num2);
					}
					if (flag4 && flag9 && !flag8)
					{
						dropDownButtonRect = rectangle2;
					}
					else
					{
						dropDownButtonRect = rectangle3;
					}
					if (paint && DataGridViewCell.PaintContentBackground(paintParts))
					{
						if (flag9)
						{
							if (flag)
							{
								g.FillRectangle(SystemBrushes.Control, rectangle3);
							}
							else if (flag3)
							{
								if (flag4)
								{
									if (flag8)
									{
										DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawDropDownButton(g, rectangle3, state, base.DataGridView.RightToLeftInternal);
									}
									else
									{
										DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawReadOnlyButton(g, rectangle2, state);
										DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawDropDownButton(g, rectangle3, ComboBoxState.Normal);
									}
									if (SystemInformation.HighContrast && AccessibilityImprovements.Level1)
									{
										cachedBrush = base.DataGridView.GetCachedBrush(cellStyle.BackColor);
									}
								}
								else
								{
									DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawDropDownButton(g, rectangle3, state);
								}
							}
							else
							{
								g.FillRectangle(SystemBrushes.Control, rectangle3);
							}
						}
						if (!flag && !flag3 && (flag8 || flag9))
						{
							Color control = SystemColors.Control;
							Color color = control;
							bool flag10 = control.ToKnownColor() == SystemColors.Control.ToKnownColor();
							bool highContrast = SystemInformation.HighContrast;
							Color color2;
							Color color3;
							Color color4;
							if (control == SystemColors.Control)
							{
								color2 = SystemColors.ControlDark;
								color3 = SystemColors.ControlDarkDark;
								color4 = SystemColors.ControlLightLight;
							}
							else
							{
								color2 = ControlPaint.Dark(control);
								color4 = ControlPaint.LightLight(control);
								if (highContrast)
								{
									color3 = ControlPaint.LightLight(control);
								}
								else
								{
									color3 = ControlPaint.DarkDark(control);
								}
							}
							color2 = g.GetNearestColor(color2);
							color3 = g.GetNearestColor(color3);
							color = g.GetNearestColor(color);
							color4 = g.GetNearestColor(color4);
							Pen pen;
							if (flag10)
							{
								if (SystemInformation.HighContrast)
								{
									pen = SystemPens.ControlLight;
								}
								else
								{
									pen = SystemPens.Control;
								}
							}
							else
							{
								pen = new Pen(color4);
							}
							if (flag9)
							{
								g.DrawLine(pen, rectangle3.X, rectangle3.Y, rectangle3.X + rectangle3.Width - 1, rectangle3.Y);
								g.DrawLine(pen, rectangle3.X, rectangle3.Y, rectangle3.X, rectangle3.Y + rectangle3.Height - 1);
							}
							if (flag8)
							{
								g.DrawLine(pen, rectangle2.X, rectangle2.Y + rectangle2.Height - 1, rectangle2.X + rectangle2.Width - 1, rectangle2.Y + rectangle2.Height - 1);
								g.DrawLine(pen, rectangle2.X + rectangle2.Width - 1, rectangle2.Y, rectangle2.X + rectangle2.Width - 1, rectangle2.Y + rectangle2.Height - 1);
							}
							if (flag10)
							{
								pen = SystemPens.ControlDarkDark;
							}
							else
							{
								pen.Color = color3;
							}
							if (flag9)
							{
								g.DrawLine(pen, rectangle3.X, rectangle3.Y + rectangle3.Height - 1, rectangle3.X + rectangle3.Width - 1, rectangle3.Y + rectangle3.Height - 1);
								g.DrawLine(pen, rectangle3.X + rectangle3.Width - 1, rectangle3.Y, rectangle3.X + rectangle3.Width - 1, rectangle3.Y + rectangle3.Height - 1);
							}
							if (flag8)
							{
								g.DrawLine(pen, rectangle2.X, rectangle2.Y, rectangle2.X + rectangle2.Width - 2, rectangle2.Y);
								g.DrawLine(pen, rectangle2.X, rectangle2.Y, rectangle2.X, rectangle2.Y + rectangle2.Height - 1);
							}
							if (flag10)
							{
								pen = SystemPens.ControlLightLight;
							}
							else
							{
								pen.Color = color;
							}
							if (flag9)
							{
								g.DrawLine(pen, rectangle3.X + 1, rectangle3.Y + 1, rectangle3.X + rectangle3.Width - 2, rectangle3.Y + 1);
								g.DrawLine(pen, rectangle3.X + 1, rectangle3.Y + 1, rectangle3.X + 1, rectangle3.Y + rectangle3.Height - 2);
							}
							if (flag10)
							{
								pen = SystemPens.ControlDark;
							}
							else
							{
								pen.Color = color2;
							}
							if (flag9)
							{
								g.DrawLine(pen, rectangle3.X + 1, rectangle3.Y + rectangle3.Height - 2, rectangle3.X + rectangle3.Width - 2, rectangle3.Y + rectangle3.Height - 2);
								g.DrawLine(pen, rectangle3.X + rectangle3.Width - 2, rectangle3.Y + 1, rectangle3.X + rectangle3.Width - 2, rectangle3.Y + rectangle3.Height - 2);
							}
							if (!flag10)
							{
								pen.Dispose();
							}
						}
						if (num >= 5 && num2 >= 3 && flag9)
						{
							if (flag)
							{
								Point point = new Point(rectangle3.Left + rectangle3.Width / 2, rectangle3.Top + rectangle3.Height / 2);
								point.X += rectangle3.Width % 2;
								point.Y += rectangle3.Height % 2;
								g.FillPolygon(SystemBrushes.ControlText, new Point[]
								{
									new Point(point.X - DataGridViewComboBoxCell.offset2X, point.Y - 1),
									new Point(point.X + DataGridViewComboBoxCell.offset2X + 1, point.Y - 1),
									new Point(point.X, point.Y + DataGridViewComboBoxCell.offset2Y)
								});
							}
							else if (!flag3)
							{
								int num3 = rectangle3.X;
								rectangle3.X = num3 - 1;
								num3 = rectangle3.Width;
								rectangle3.Width = num3 + 1;
								Point point2 = new Point(rectangle3.Left + (rectangle3.Width - 1) / 2, rectangle3.Top + (rectangle3.Height + (int)DataGridViewComboBoxCell.nonXPTriangleHeight) / 2);
								point2.X += (rectangle3.Width + 1) % 2;
								point2.Y += rectangle3.Height % 2;
								Point point3 = new Point(point2.X - (int)((DataGridViewComboBoxCell.nonXPTriangleWidth - 1) / 2), point2.Y - (int)DataGridViewComboBoxCell.nonXPTriangleHeight);
								Point point4 = new Point(point2.X + (int)((DataGridViewComboBoxCell.nonXPTriangleWidth - 1) / 2), point2.Y - (int)DataGridViewComboBoxCell.nonXPTriangleHeight);
								g.FillPolygon(SystemBrushes.ControlText, new Point[]
								{
									point3,
									point4,
									point2
								});
								g.DrawLine(SystemPens.ControlText, point3.X, point3.Y, point4.X, point4.Y);
								num3 = rectangle3.X;
								rectangle3.X = num3 + 1;
								num3 = rectangle3.Width;
								rectangle3.Width = num3 - 1;
							}
						}
						if (flag2 && flag8)
						{
							int num3 = rectangle3.Y;
							rectangle3.Y = num3 - 1;
							num3 = rectangle3.Height;
							rectangle3.Height = num3 + 1;
							g.DrawRectangle(SystemPens.ControlDark, rectangle3);
						}
					}
				}
			}
			Rectangle cellValueBounds = rectangle2;
			Rectangle rectangle4 = Rectangle.Inflate(rectangle2, -2, -2);
			if (flag4)
			{
				int num3;
				if (!base.DataGridView.RightToLeftInternal)
				{
					num3 = rectangle4.X;
					rectangle4.X = num3 - 1;
				}
				num3 = rectangle4.Width;
				rectangle4.Width = num3 + 1;
			}
			if (flag9)
			{
				if (flag3 || flag)
				{
					cellValueBounds.Width -= num;
					rectangle4.Width -= num;
					if (base.DataGridView.RightToLeftInternal)
					{
						cellValueBounds.X += num;
						rectangle4.X += num;
					}
				}
				else
				{
					cellValueBounds.Width -= num + 1;
					rectangle4.Width -= num + 1;
					if (base.DataGridView.RightToLeftInternal)
					{
						cellValueBounds.X += num + 1;
						rectangle4.X += num + 1;
					}
				}
			}
			if (rectangle4.Width > 1 && rectangle4.Height > 1)
			{
				if (flag5 && !flag6 && DataGridViewCell.PaintFocus(paintParts) && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && paint)
				{
					if (flag)
					{
						Rectangle rectangle5 = rectangle4;
						int num3;
						if (!base.DataGridView.RightToLeftInternal)
						{
							num3 = rectangle5.X;
							rectangle5.X = num3 - 1;
						}
						num3 = rectangle5.Width;
						rectangle5.Width = num3 + 1;
						num3 = rectangle5.Y;
						rectangle5.Y = num3 - 1;
						rectangle5.Height += 2;
						ControlPaint.DrawFocusRectangle(g, rectangle5, Color.Empty, cachedBrush.Color);
					}
					else if (flag4)
					{
						Rectangle rectangle6 = rectangle4;
						int num3 = rectangle6.X;
						rectangle6.X = num3 + 1;
						rectangle6.Width -= 2;
						num3 = rectangle6.Y;
						rectangle6.Y = num3 + 1;
						rectangle6.Height -= 2;
						if (rectangle6.Width > 0 && rectangle6.Height > 0)
						{
							ControlPaint.DrawFocusRectangle(g, rectangle6, Color.Empty, cachedBrush.Color);
						}
					}
					else
					{
						ControlPaint.DrawFocusRectangle(g, rectangle4, Color.Empty, cachedBrush.Color);
					}
				}
				if (flag2)
				{
					int num3 = rectangle2.Width;
					rectangle2.Width = num3 - 1;
					num3 = rectangle2.Height;
					rectangle2.Height = num3 - 1;
					if (!flag6 && paint && DataGridViewCell.PaintContentBackground(paintParts) && flag8)
					{
						g.DrawRectangle(SystemPens.ControlDark, rectangle2);
					}
				}
				string text = formattedValue as string;
				if (text != null)
				{
					int num4 = (cellStyle.WrapMode == DataGridViewTriState.True) ? 0 : 1;
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle4.Offset(0, num4);
						rectangle4.Width += 2;
					}
					else
					{
						rectangle4.Offset(-1, num4);
						rectangle4.Width++;
					}
					rectangle4.Height -= num4;
					if (rectangle4.Width > 0 && rectangle4.Height > 0)
					{
						TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
						if (!flag6 && paint)
						{
							if (DataGridViewCell.PaintContentForeground(paintParts))
							{
								if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
								{
									textFormatFlags |= TextFormatFlags.EndEllipsis;
								}
								Color foreColor;
								if (flag4 && (flag9 || flag8))
								{
									foreColor = DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
								}
								else
								{
									foreColor = (flag7 ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
								}
								TextRenderer.DrawText(g, text, cellStyle.Font, rectangle4, foreColor, textFormatFlags);
							}
						}
						else if (computeContentBounds)
						{
							result = DataGridViewUtilities.GetTextBounds(rectangle4, text, textFormatFlags, cellStyle);
						}
					}
				}
				if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
				{
					base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, cellValueBounds, errorText);
					if (flag6)
					{
						return Rectangle.Empty;
					}
				}
			}
			if (computeErrorIconBounds)
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
			return result;
		}

		/// <summary>Converts a value formatted for display to an actual cell value.</summary>
		/// <param name="formattedValue">The display value of the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> in effect for the cell.</param>
		/// <param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> for the display value type, or null to use the default converter.</param>
		/// <param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> for the cell value type, or null to use the default converter.</param>
		/// <returns>The cell value.</returns>
		// Token: 0x06001CC9 RID: 7369 RVA: 0x000926E8 File Offset: 0x000908E8
		public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
		{
			if (valueTypeConverter == null)
			{
				if (this.ValueMemberProperty != null)
				{
					valueTypeConverter = this.ValueMemberProperty.Converter;
				}
				else if (this.DisplayMemberProperty != null)
				{
					valueTypeConverter = this.DisplayMemberProperty.Converter;
				}
			}
			if ((this.DataManager != null && (this.DisplayMemberProperty != null || this.ValueMemberProperty != null)) || !string.IsNullOrEmpty(this.DisplayMember) || !string.IsNullOrEmpty(this.ValueMember))
			{
				object obj = base.ParseFormattedValueInternal(this.DisplayType, formattedValue, cellStyle, formattedValueTypeConverter, this.DisplayTypeConverter);
				object obj2 = obj;
				if (!this.LookupValue(obj2, out obj))
				{
					if (obj2 != DBNull.Value)
					{
						throw new FormatException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Formatter_CantConvert"), new object[]
						{
							obj,
							this.DisplayType
						}));
					}
					obj = DBNull.Value;
				}
				return obj;
			}
			return base.ParseFormattedValueInternal(this.ValueType, formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
		}

		/// <summary>Returns a string that describes the current object. </summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x06001CCA RID: 7370 RVA: 0x000927CC File Offset: 0x000909CC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewComboBoxCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x00092828 File Offset: 0x00090A28
		private void UnwireDataSource()
		{
			IComponent component = this.DataSource as IComponent;
			if (component != null)
			{
				component.Disposed -= this.DataSource_Disposed;
			}
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && (this.flags & 16) != 0)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
				this.flags = (byte)((int)this.flags & -17);
			}
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x00092894 File Offset: 0x00090A94
		private void WireDataSource(object dataSource)
		{
			IComponent component = dataSource as IComponent;
			if (component != null)
			{
				component.Disposed += this.DataSource_Disposed;
			}
		}

		// Token: 0x04000CAD RID: 3245
		private static readonly int PropComboBoxCellDataSource = PropertyStore.CreateKey();

		// Token: 0x04000CAE RID: 3246
		private static readonly int PropComboBoxCellDisplayMember = PropertyStore.CreateKey();

		// Token: 0x04000CAF RID: 3247
		private static readonly int PropComboBoxCellValueMember = PropertyStore.CreateKey();

		// Token: 0x04000CB0 RID: 3248
		private static readonly int PropComboBoxCellItems = PropertyStore.CreateKey();

		// Token: 0x04000CB1 RID: 3249
		private static readonly int PropComboBoxCellDropDownWidth = PropertyStore.CreateKey();

		// Token: 0x04000CB2 RID: 3250
		private static readonly int PropComboBoxCellMaxDropDownItems = PropertyStore.CreateKey();

		// Token: 0x04000CB3 RID: 3251
		private static readonly int PropComboBoxCellEditingComboBox = PropertyStore.CreateKey();

		// Token: 0x04000CB4 RID: 3252
		private static readonly int PropComboBoxCellValueMemberProp = PropertyStore.CreateKey();

		// Token: 0x04000CB5 RID: 3253
		private static readonly int PropComboBoxCellDisplayMemberProp = PropertyStore.CreateKey();

		// Token: 0x04000CB6 RID: 3254
		private static readonly int PropComboBoxCellDataManager = PropertyStore.CreateKey();

		// Token: 0x04000CB7 RID: 3255
		private static readonly int PropComboBoxCellColumnTemplate = PropertyStore.CreateKey();

		// Token: 0x04000CB8 RID: 3256
		private static readonly int PropComboBoxCellFlatStyle = PropertyStore.CreateKey();

		// Token: 0x04000CB9 RID: 3257
		private static readonly int PropComboBoxCellDisplayStyle = PropertyStore.CreateKey();

		// Token: 0x04000CBA RID: 3258
		private static readonly int PropComboBoxCellDisplayStyleForCurrentCellOnly = PropertyStore.CreateKey();

		// Token: 0x04000CBB RID: 3259
		private const byte DATAGRIDVIEWCOMBOBOXCELL_margin = 3;

		// Token: 0x04000CBC RID: 3260
		private const byte DATAGRIDVIEWCOMBOBOXCELL_nonXPTriangleHeight = 4;

		// Token: 0x04000CBD RID: 3261
		private const byte DATAGRIDVIEWCOMBOBOXCELL_nonXPTriangleWidth = 7;

		// Token: 0x04000CBE RID: 3262
		private const byte DATAGRIDVIEWCOMBOBOXCELL_horizontalTextMarginLeft = 0;

		// Token: 0x04000CBF RID: 3263
		private const byte DATAGRIDVIEWCOMBOBOXCELL_verticalTextMarginTopWithWrapping = 0;

		// Token: 0x04000CC0 RID: 3264
		private const byte DATAGRIDVIEWCOMBOBOXCELL_verticalTextMarginTopWithoutWrapping = 1;

		// Token: 0x04000CC1 RID: 3265
		private const byte DATAGRIDVIEWCOMBOBOXCELL_ignoreNextMouseClick = 1;

		// Token: 0x04000CC2 RID: 3266
		private const byte DATAGRIDVIEWCOMBOBOXCELL_sorted = 2;

		// Token: 0x04000CC3 RID: 3267
		private const byte DATAGRIDVIEWCOMBOBOXCELL_createItemsFromDataSource = 4;

		// Token: 0x04000CC4 RID: 3268
		private const byte DATAGRIDVIEWCOMBOBOXCELL_autoComplete = 8;

		// Token: 0x04000CC5 RID: 3269
		private const byte DATAGRIDVIEWCOMBOBOXCELL_dataSourceInitializedHookedUp = 16;

		// Token: 0x04000CC6 RID: 3270
		private const byte DATAGRIDVIEWCOMBOBOXCELL_dropDownHookedUp = 32;

		// Token: 0x04000CC7 RID: 3271
		internal const int DATAGRIDVIEWCOMBOBOXCELL_defaultMaxDropDownItems = 8;

		// Token: 0x04000CC8 RID: 3272
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04000CC9 RID: 3273
		private static Type defaultEditType = typeof(DataGridViewComboBoxEditingControl);

		// Token: 0x04000CCA RID: 3274
		private static Type defaultValueType = typeof(object);

		// Token: 0x04000CCB RID: 3275
		private static Type cellType = typeof(DataGridViewComboBoxCell);

		// Token: 0x04000CCC RID: 3276
		private byte flags;

		// Token: 0x04000CCD RID: 3277
		private static bool mouseInDropDownButtonBounds = false;

		// Token: 0x04000CCE RID: 3278
		private static int cachedDropDownWidth = -1;

		// Token: 0x04000CCF RID: 3279
		private static bool isScalingInitialized = false;

		// Token: 0x04000CD0 RID: 3280
		private static int OFFSET_2PIXELS = 2;

		// Token: 0x04000CD1 RID: 3281
		private static int offset2X = DataGridViewComboBoxCell.OFFSET_2PIXELS;

		// Token: 0x04000CD2 RID: 3282
		private static int offset2Y = DataGridViewComboBoxCell.OFFSET_2PIXELS;

		// Token: 0x04000CD3 RID: 3283
		private static byte nonXPTriangleHeight = 4;

		// Token: 0x04000CD4 RID: 3284
		private static byte nonXPTriangleWidth = 7;

		/// <summary>Represents the collection of selection choices in a <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</summary>
		// Token: 0x020005B2 RID: 1458
		[ListBindable(false)]
		public class ObjectCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> that owns the collection.</param>
			// Token: 0x0600595E RID: 22878 RVA: 0x00178897 File Offset: 0x00176A97
			public ObjectCollection(DataGridViewComboBoxCell owner)
			{
				this.owner = owner;
			}

			// Token: 0x17001599 RID: 5529
			// (get) Token: 0x0600595F RID: 22879 RVA: 0x001788A6 File Offset: 0x00176AA6
			private IComparer Comparer
			{
				get
				{
					if (this.comparer == null)
					{
						this.comparer = new DataGridViewComboBoxCell.ItemComparer(this.owner);
					}
					return this.comparer;
				}
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x1700159A RID: 5530
			// (get) Token: 0x06005960 RID: 22880 RVA: 0x001788C7 File Offset: 0x00176AC7
			public int Count
			{
				get
				{
					return this.InnerArray.Count;
				}
			}

			// Token: 0x1700159B RID: 5531
			// (get) Token: 0x06005961 RID: 22881 RVA: 0x001788D4 File Offset: 0x00176AD4
			internal ArrayList InnerArray
			{
				get
				{
					if (this.items == null)
					{
						this.items = new ArrayList();
					}
					return this.items;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" />.</returns>
			// Token: 0x1700159C RID: 5532
			// (get) Token: 0x06005962 RID: 22882 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x1700159D RID: 5533
			// (get) Token: 0x06005963 RID: 22883 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x1700159E RID: 5534
			// (get) Token: 0x06005964 RID: 22884 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x1700159F RID: 5535
			// (get) Token: 0x06005965 RID: 22885 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Adds an item to the list of items for a <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</summary>
			/// <param name="item">An object representing the item to add to the collection.</param>
			/// <returns>The position into which the new element was inserted.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="item" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentException">The cell's <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property value is not <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The cell is in a shared row.</exception>
			// Token: 0x06005966 RID: 22886 RVA: 0x001788F0 File Offset: 0x00176AF0
			public int Add(object item)
			{
				this.owner.CheckNoDataSource();
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				int result = this.InnerArray.Add(item);
				bool flag = false;
				if (this.owner.Sorted)
				{
					try
					{
						this.InnerArray.Sort(this.Comparer);
						result = this.InnerArray.IndexOf(item);
						flag = true;
					}
					finally
					{
						if (!flag)
						{
							this.InnerArray.Remove(item);
						}
					}
				}
				this.owner.OnItemsCollectionChanged();
				return result;
			}

			/// <summary>Adds an object to the collection.</summary>
			/// <param name="item">The object to add to the collection.</param>
			/// <returns>The position in which to insert the new element.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="item" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentException">The cell's <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property value is not <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The cell is in a shared row.</exception>
			// Token: 0x06005967 RID: 22887 RVA: 0x00178980 File Offset: 0x00176B80
			int IList.Add(object item)
			{
				return this.Add(item);
			}

			/// <summary>Adds one or more items to the list of items for a <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</summary>
			/// <param name="items">One or more objects that represent items for the drop-down list.-or-An <see cref="T:System.Array" /> of <see cref="T:System.Object" /> values. </param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="items" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">One or more of the items in the <paramref name="items" /> array is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentException">The cell's <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property value is not <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The cell is in a shared row.</exception>
			// Token: 0x06005968 RID: 22888 RVA: 0x00178989 File Offset: 0x00176B89
			public void AddRange(params object[] items)
			{
				this.owner.CheckNoDataSource();
				this.AddRangeInternal(items);
				this.owner.OnItemsCollectionChanged();
			}

			/// <summary>Adds the items of an existing <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" /> to the list of items in a <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" /> to load into this collection.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">One or more of the items in the <paramref name="value" /> collection is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentException">The cell's <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property value is not <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The cell is in a shared row.</exception>
			// Token: 0x06005969 RID: 22889 RVA: 0x00178989 File Offset: 0x00176B89
			public void AddRange(DataGridViewComboBoxCell.ObjectCollection value)
			{
				this.owner.CheckNoDataSource();
				this.AddRangeInternal(value);
				this.owner.OnItemsCollectionChanged();
			}

			// Token: 0x0600596A RID: 22890 RVA: 0x001789A8 File Offset: 0x00176BA8
			internal void AddRangeInternal(ICollection items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				using (IEnumerator enumerator = items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == null)
						{
							throw new InvalidOperationException(SR.GetString("InvalidNullItemInCollection"));
						}
					}
				}
				this.InnerArray.AddRange(items);
				if (this.owner.Sorted)
				{
					this.InnerArray.Sort(this.Comparer);
				}
			}

			// Token: 0x0600596B RID: 22891 RVA: 0x00178A3C File Offset: 0x00176C3C
			internal void SortInternal()
			{
				this.InnerArray.Sort(this.Comparer);
			}

			/// <summary>Gets or sets the item at the current index location. In C#, this property is the indexer for the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" /> class.</summary>
			/// <param name="index">The zero-based index of the element to get or set.</param>
			/// <returns>The <see cref="T:System.Object" /> stored at the given index.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than the number of items in the collection minus one. </exception>
			/// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentException">When setting this property, the cell's <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property value is not <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">When setting this property, the cell is in a shared row.</exception>
			// Token: 0x170015A0 RID: 5536
			public virtual object this[int index]
			{
				get
				{
					if (index < 0 || index >= this.InnerArray.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.InnerArray[index];
				}
				set
				{
					this.owner.CheckNoDataSource();
					if (value == null)
					{
						throw new ArgumentNullException("value");
					}
					if (index < 0 || index >= this.InnerArray.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.InnerArray[index] = value;
					this.owner.OnItemsCollectionChanged();
				}
			}

			/// <summary>Clears all items from the collection.</summary>
			/// <exception cref="T:System.ArgumentException">The collection contains at least one item and the cell's <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property value is not <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The collection contains at least one item and the cell is in a shared row.</exception>
			// Token: 0x0600596E RID: 22894 RVA: 0x00178B32 File Offset: 0x00176D32
			public void Clear()
			{
				if (this.InnerArray.Count > 0)
				{
					this.owner.CheckNoDataSource();
					this.InnerArray.Clear();
					this.owner.OnItemsCollectionChanged();
				}
			}

			// Token: 0x0600596F RID: 22895 RVA: 0x00178B63 File Offset: 0x00176D63
			internal void ClearInternal()
			{
				this.InnerArray.Clear();
			}

			/// <summary>Determines whether the specified item is contained in the collection.</summary>
			/// <param name="value">An object representing the item to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the <paramref name="item" /> is in the collection; otherwise, <see langword="false" />.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			// Token: 0x06005970 RID: 22896 RVA: 0x00178B70 File Offset: 0x00176D70
			public bool Contains(object value)
			{
				return this.IndexOf(value) != -1;
			}

			/// <summary>Copies the entire collection into an existing array of objects at a specified location within the array.</summary>
			/// <param name="destination">The destination array to which the contents will be copied.</param>
			/// <param name="arrayIndex">The index of the element in <paramref name="dest" /> at which to start copying.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="destination" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="arrayIndex" /> is less than 0 or equal to or greater than the length of <paramref name="destination" />.-or-The number of elements in the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of <paramref name="destination" />.</exception>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="destination" /> is multidimensional.</exception>
			// Token: 0x06005971 RID: 22897 RVA: 0x00178B80 File Offset: 0x00176D80
			public void CopyTo(object[] destination, int arrayIndex)
			{
				int count = this.InnerArray.Count;
				for (int i = 0; i < count; i++)
				{
					destination[i + arrayIndex] = this.InnerArray[i];
				}
			}

			/// <summary>Copies the elements of the collection to the specified array, starting at the specified index.</summary>
			/// <param name="destination">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in the array at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="destination" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or equal to or greater than the length of <paramref name="destination" />.-or-The number of elements in the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" /> is greater than the available space from <paramref name="index" /> to the end of <paramref name="destination" />.</exception>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="destination" /> is multidimensional.</exception>
			// Token: 0x06005972 RID: 22898 RVA: 0x00178BB8 File Offset: 0x00176DB8
			void ICollection.CopyTo(Array destination, int index)
			{
				int count = this.InnerArray.Count;
				for (int i = 0; i < count; i++)
				{
					destination.SetValue(this.InnerArray[i], i + index);
				}
			}

			/// <summary>Returns an enumerator that can iterate through a <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" />.</summary>
			/// <returns>An enumerator of type <see cref="T:System.Collections.IEnumerator" />.</returns>
			// Token: 0x06005973 RID: 22899 RVA: 0x00178BF2 File Offset: 0x00176DF2
			public IEnumerator GetEnumerator()
			{
				return this.InnerArray.GetEnumerator();
			}

			/// <summary>Returns the index of the specified item in the collection.</summary>
			/// <param name="value">An object representing the item to locate in the collection.</param>
			/// <returns>The zero-based index of the <paramref name="value" /> parameter if it is found in the collection; otherwise, -1.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			// Token: 0x06005974 RID: 22900 RVA: 0x00178BFF File Offset: 0x00176DFF
			public int IndexOf(object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.InnerArray.IndexOf(value);
			}

			/// <summary>Inserts an item into the collection at the specified index. </summary>
			/// <param name="index">The zero-based index at which to place <paramref name="item" /> within an unsorted <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</param>
			/// <param name="item">An object representing the item to insert.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="item" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than the number of items in the collection. </exception>
			/// <exception cref="T:System.ArgumentException">The cell's <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property value is not <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The cell is in a shared row.</exception>
			// Token: 0x06005975 RID: 22901 RVA: 0x00178C1C File Offset: 0x00176E1C
			public void Insert(int index, object item)
			{
				this.owner.CheckNoDataSource();
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (index < 0 || index > this.InnerArray.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.Sorted)
				{
					this.Add(item);
					return;
				}
				this.InnerArray.Insert(index, item);
				this.owner.OnItemsCollectionChanged();
			}

			/// <summary>Removes the specified object from the collection.</summary>
			/// <param name="value">An object representing the item to remove from the collection.</param>
			/// <exception cref="T:System.ArgumentException">The cell's <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property value is not <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The cell is in a shared row.</exception>
			// Token: 0x06005976 RID: 22902 RVA: 0x00178CB4 File Offset: 0x00176EB4
			public void Remove(object value)
			{
				int num = this.InnerArray.IndexOf(value);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			/// <summary>Removes the object at the specified index.</summary>
			/// <param name="index">The zero-based index of the object to be removed.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than the number of items in the collection minus one. </exception>
			/// <exception cref="T:System.ArgumentException">The cell's <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource" /> property value is not <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The cell is in a shared row.</exception>
			// Token: 0x06005977 RID: 22903 RVA: 0x00178CDC File Offset: 0x00176EDC
			public void RemoveAt(int index)
			{
				this.owner.CheckNoDataSource();
				if (index < 0 || index >= this.InnerArray.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.InnerArray.RemoveAt(index);
				this.owner.OnItemsCollectionChanged();
			}

			// Token: 0x04003930 RID: 14640
			private DataGridViewComboBoxCell owner;

			// Token: 0x04003931 RID: 14641
			private ArrayList items;

			// Token: 0x04003932 RID: 14642
			private IComparer comparer;
		}

		// Token: 0x020005B3 RID: 1459
		private sealed class ItemComparer : IComparer
		{
			// Token: 0x06005978 RID: 22904 RVA: 0x00178D4F File Offset: 0x00176F4F
			public ItemComparer(DataGridViewComboBoxCell dataGridViewComboBoxCell)
			{
				this.dataGridViewComboBoxCell = dataGridViewComboBoxCell;
			}

			// Token: 0x06005979 RID: 22905 RVA: 0x00178D60 File Offset: 0x00176F60
			public int Compare(object item1, object item2)
			{
				if (item1 == null)
				{
					if (item2 == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (item2 == null)
					{
						return 1;
					}
					string itemDisplayText = this.dataGridViewComboBoxCell.GetItemDisplayText(item1);
					string itemDisplayText2 = this.dataGridViewComboBoxCell.GetItemDisplayText(item2);
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					return compareInfo.Compare(itemDisplayText, itemDisplayText2, CompareOptions.StringSort);
				}
			}

			// Token: 0x04003933 RID: 14643
			private DataGridViewComboBoxCell dataGridViewComboBoxCell;
		}

		// Token: 0x020005B4 RID: 1460
		private class DataGridViewComboBoxCellRenderer
		{
			// Token: 0x0600597A RID: 22906 RVA: 0x000027DB File Offset: 0x000009DB
			private DataGridViewComboBoxCellRenderer()
			{
			}

			// Token: 0x170015A1 RID: 5537
			// (get) Token: 0x0600597B RID: 22907 RVA: 0x00178DAE File Offset: 0x00176FAE
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton);
					}
					return DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x0600597C RID: 22908 RVA: 0x00178DCB File Offset: 0x00176FCB
			public static void DrawTextBox(Graphics g, Rectangle bounds, ComboBoxState state)
			{
				ComboBoxRenderer.DrawTextBox(g, bounds, state);
			}

			// Token: 0x0600597D RID: 22909 RVA: 0x00178DD5 File Offset: 0x00176FD5
			public static void DrawDropDownButton(Graphics g, Rectangle bounds, ComboBoxState state)
			{
				ComboBoxRenderer.DrawDropDownButton(g, bounds, state);
			}

			// Token: 0x0600597E RID: 22910 RVA: 0x00178DE0 File Offset: 0x00176FE0
			public static void DrawBorder(Graphics g, Rectangle bounds)
			{
				if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxBorder);
				}
				else
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.SetParameters(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxBorder.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxBorder.Part, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxBorder.State);
				}
				DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			}

			// Token: 0x0600597F RID: 22911 RVA: 0x00178E3C File Offset: 0x0017703C
			public static void DrawDropDownButton(Graphics g, Rectangle bounds, ComboBoxState state, bool rightToLeft)
			{
				if (rightToLeft)
				{
					if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonLeft.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonLeft.Part, (int)state);
					}
					else
					{
						DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.SetParameters(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonLeft.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonLeft.Part, (int)state);
					}
				}
				else if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonRight.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonRight.Part, (int)state);
				}
				else
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.SetParameters(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonRight.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonRight.Part, (int)state);
				}
				DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			}

			// Token: 0x06005980 RID: 22912 RVA: 0x00178EE8 File Offset: 0x001770E8
			public static void DrawReadOnlyButton(Graphics g, Rectangle bounds, ComboBoxState state)
			{
				if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton.Part, (int)state);
				}
				else
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.SetParameters(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton.Part, (int)state);
				}
				DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			}

			// Token: 0x04003934 RID: 14644
			[ThreadStatic]
			private static VisualStyleRenderer visualStyleRenderer;

			// Token: 0x04003935 RID: 14645
			private static readonly VisualStyleElement ComboBoxBorder = VisualStyleElement.ComboBox.Border.Normal;

			// Token: 0x04003936 RID: 14646
			private static readonly VisualStyleElement ComboBoxDropDownButtonRight = VisualStyleElement.ComboBox.DropDownButtonRight.Normal;

			// Token: 0x04003937 RID: 14647
			private static readonly VisualStyleElement ComboBoxDropDownButtonLeft = VisualStyleElement.ComboBox.DropDownButtonLeft.Normal;

			// Token: 0x04003938 RID: 14648
			private static readonly VisualStyleElement ComboBoxReadOnlyButton = VisualStyleElement.ComboBox.ReadOnlyButton.Normal;
		}

		/// <summary>Represents the accessibility object for the current <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> object. </summary>
		// Token: 0x020005B5 RID: 1461
		[ComVisible(true)]
		protected class DataGridViewComboBoxCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			/// <summary>Instantiates a new instance of the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.DataGridViewComboBoxCellAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> control to which this object belongs. </param>
			// Token: 0x06005982 RID: 22914 RVA: 0x00176D3A File Offset: 0x00174F3A
			public DataGridViewComboBoxCellAccessibleObject(DataGridViewCell owner) : base(owner)
			{
			}

			// Token: 0x06005983 RID: 22915 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override bool IsIAccessibleExSupported()
			{
				return true;
			}

			// Token: 0x06005984 RID: 22916 RVA: 0x00178F72 File Offset: 0x00177172
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50003;
				}
				if (AccessibilityImprovements.Level4 && propertyID == 30028)
				{
					return this.IsPatternSupported(10005);
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06005985 RID: 22917 RVA: 0x00178FAE File Offset: 0x001771AE
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level4 && patternId == 10005) || base.IsPatternSupported(patternId);
			}

			// Token: 0x170015A2 RID: 5538
			// (get) Token: 0x06005986 RID: 22918 RVA: 0x00178FC8 File Offset: 0x001771C8
			internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
			{
				get
				{
					if (!AccessibilityImprovements.Level4)
					{
						return base.ExpandCollapseState;
					}
					DataGridViewCell owner = base.Owner;
					object obj;
					if (owner == null)
					{
						obj = null;
					}
					else
					{
						PropertyStore properties = owner.Properties;
						obj = ((properties != null) ? properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellEditingComboBox) : null);
					}
					DataGridViewComboBoxEditingControl dataGridViewComboBoxEditingControl = obj as DataGridViewComboBoxEditingControl;
					if (dataGridViewComboBoxEditingControl == null)
					{
						return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
					}
					if (!dataGridViewComboBoxEditingControl.DroppedDown)
					{
						return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
					}
					return UnsafeNativeMethods.ExpandCollapseState.Expanded;
				}
			}
		}
	}
}
