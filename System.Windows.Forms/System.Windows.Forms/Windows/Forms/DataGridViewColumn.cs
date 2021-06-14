using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Represents a column in a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001AE RID: 430
	[Designer("System.Windows.Forms.Design.DataGridViewColumnDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[TypeConverter(typeof(DataGridViewColumnConverter))]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	public class DataGridViewColumn : DataGridViewBand, IComponent, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewColumn" /> class to the default state.</summary>
		// Token: 0x06001BC2 RID: 7106 RVA: 0x0008B0DA File Offset: 0x000892DA
		public DataGridViewColumn() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewColumn" /> class using an existing <see cref="T:System.Windows.Forms.DataGridViewCell" /> as a template.</summary>
		/// <param name="cellTemplate">An existing <see cref="T:System.Windows.Forms.DataGridViewCell" /> to use as a template. </param>
		// Token: 0x06001BC3 RID: 7107 RVA: 0x0008B0E4 File Offset: 0x000892E4
		public DataGridViewColumn(DataGridViewCell cellTemplate)
		{
			this.fillWeight = 100f;
			this.usedFillWeight = 100f;
			base.Thickness = this.ScaleToCurrentDpi(100);
			base.MinimumThickness = this.ScaleToCurrentDpi(5);
			this.name = string.Empty;
			this.bandIsRow = false;
			this.displayIndex = -1;
			this.cellTemplate = cellTemplate;
			this.autoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x0008B161 File Offset: 0x00089361
		private int ScaleToCurrentDpi(int value)
		{
			if (!DpiHelper.EnableDataGridViewControlHighDpiImprovements)
			{
				return value;
			}
			return DpiHelper.LogicalToDeviceUnits(value, 0);
		}

		/// <summary>Gets or sets the mode by which the column automatically adjusts its width.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnMode" /> value that determines whether the column will automatically adjust its width and how it will determine its preferred width. The default is <see cref="F:System.Windows.Forms.DataGridViewAutoSizeColumnMode.NotSet" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is a <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnMode" /> that is not valid. </exception>
		/// <exception cref="T:System.InvalidOperationException">The specified value when setting this property results in an <see cref="P:System.Windows.Forms.DataGridViewColumn.InheritedAutoSizeMode" /> value of <see cref="F:System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader" /> for a visible column when column headers are hidden.-or-The specified value when setting this property results in an <see cref="P:System.Windows.Forms.DataGridViewColumn.InheritedAutoSizeMode" /> value of <see cref="F:System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill" /> for a visible column that is frozen.</exception>
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x0008B173 File Offset: 0x00089373
		// (set) Token: 0x06001BC6 RID: 7110 RVA: 0x0008B17C File Offset: 0x0008937C
		[SRCategory("CatLayout")]
		[DefaultValue(DataGridViewAutoSizeColumnMode.NotSet)]
		[SRDescription("DataGridViewColumn_AutoSizeModeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public DataGridViewAutoSizeColumnMode AutoSizeMode
		{
			get
			{
				return this.autoSizeMode;
			}
			set
			{
				switch (value)
				{
				case DataGridViewAutoSizeColumnMode.NotSet:
				case DataGridViewAutoSizeColumnMode.None:
				case DataGridViewAutoSizeColumnMode.ColumnHeader:
				case DataGridViewAutoSizeColumnMode.AllCellsExceptHeader:
				case DataGridViewAutoSizeColumnMode.AllCells:
				case DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader:
				case DataGridViewAutoSizeColumnMode.DisplayedCells:
					goto IL_4D;
				case (DataGridViewAutoSizeColumnMode)3:
				case (DataGridViewAutoSizeColumnMode)5:
				case (DataGridViewAutoSizeColumnMode)7:
				case (DataGridViewAutoSizeColumnMode)9:
					break;
				default:
					if (value == DataGridViewAutoSizeColumnMode.Fill)
					{
						goto IL_4D;
					}
					break;
				}
				throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAutoSizeColumnMode));
				IL_4D:
				if (this.autoSizeMode != value)
				{
					if (this.Visible && base.DataGridView != null)
					{
						if (!base.DataGridView.ColumnHeadersVisible && (value == DataGridViewAutoSizeColumnMode.ColumnHeader || (value == DataGridViewAutoSizeColumnMode.NotSet && base.DataGridView.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.ColumnHeader)))
						{
							throw new InvalidOperationException(SR.GetString("DataGridViewColumn_AutoSizeCriteriaCannotUseInvisibleHeaders"));
						}
						if (this.Frozen && (value == DataGridViewAutoSizeColumnMode.Fill || (value == DataGridViewAutoSizeColumnMode.NotSet && base.DataGridView.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.Fill)))
						{
							throw new InvalidOperationException(SR.GetString("DataGridViewColumn_FrozenColumnCannotAutoFill"));
						}
					}
					DataGridViewAutoSizeColumnMode inheritedAutoSizeMode = this.InheritedAutoSizeMode;
					bool flag = inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.Fill && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.None && inheritedAutoSizeMode > DataGridViewAutoSizeColumnMode.NotSet;
					this.autoSizeMode = value;
					if (base.DataGridView == null)
					{
						if (this.InheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.Fill && this.InheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.None && this.InheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.NotSet)
						{
							if (!flag)
							{
								base.CachedThickness = base.Thickness;
								return;
							}
						}
						else if (base.Thickness != base.CachedThickness && flag)
						{
							base.ThicknessInternal = base.CachedThickness;
							return;
						}
					}
					else
					{
						base.DataGridView.OnAutoSizeColumnModeChanged(this, inheritedAutoSizeMode);
					}
				}
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x0008B2D4 File Offset: 0x000894D4
		// (set) Token: 0x06001BC8 RID: 7112 RVA: 0x0008B2DC File Offset: 0x000894DC
		internal TypeConverter BoundColumnConverter
		{
			get
			{
				return this.boundColumnConverter;
			}
			set
			{
				this.boundColumnConverter = value;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x0008B2E5 File Offset: 0x000894E5
		// (set) Token: 0x06001BCA RID: 7114 RVA: 0x0008B2ED File Offset: 0x000894ED
		internal int BoundColumnIndex
		{
			get
			{
				return this.boundColumnIndex;
			}
			set
			{
				this.boundColumnIndex = value;
			}
		}

		/// <summary>Gets or sets the template used to create new cells.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCell" /> that all other cells in the column are modeled after. The default is <see langword="null" />.</returns>
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x0008B2F6 File Offset: 0x000894F6
		// (set) Token: 0x06001BCC RID: 7116 RVA: 0x0008B2FE File Offset: 0x000894FE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual DataGridViewCell CellTemplate
		{
			get
			{
				return this.cellTemplate;
			}
			set
			{
				this.cellTemplate = value;
			}
		}

		/// <summary>Gets the run-time type of the cell template.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> used as a template for this column. The default is <see langword="null" />.</returns>
		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x0008B307 File Offset: 0x00089507
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Type CellType
		{
			get
			{
				if (this.cellTemplate != null)
				{
					return this.cellTemplate.GetType();
				}
				return null;
			}
		}

		/// <summary>Gets or sets the shortcut menu for the column.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> associated with the current <see cref="T:System.Windows.Forms.DataGridViewColumn" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001BCE RID: 7118 RVA: 0x0008B31E File Offset: 0x0008951E
		// (set) Token: 0x06001BCF RID: 7119 RVA: 0x0008B326 File Offset: 0x00089526
		[DefaultValue(null)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ColumnContextMenuStripDescr")]
		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return base.ContextMenuStrip;
			}
			set
			{
				base.ContextMenuStrip = value;
			}
		}

		/// <summary>Gets or sets the name of the data source property or database column to which the <see cref="T:System.Windows.Forms.DataGridViewColumn" /> is bound.</summary>
		/// <returns>The case-insensitive name of the property or database column associated with the <see cref="T:System.Windows.Forms.DataGridViewColumn" />.</returns>
		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x0008B32F File Offset: 0x0008952F
		// (set) Token: 0x06001BD1 RID: 7121 RVA: 0x0008B337 File Offset: 0x00089537
		[Browsable(true)]
		[DefaultValue("")]
		[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Windows.Forms.Design.DataGridViewColumnDataPropertyNameEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("DataGridView_ColumnDataPropertyNameDescr")]
		[SRCategory("CatData")]
		public string DataPropertyName
		{
			get
			{
				return this.dataPropertyName;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value != this.dataPropertyName)
				{
					this.dataPropertyName = value;
					if (base.DataGridView != null)
					{
						base.DataGridView.OnColumnDataPropertyNameChanged(this);
					}
				}
			}
		}

		/// <summary>Gets or sets the column's default cell style.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the default style of the cells in the column.</returns>
		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x0008B36C File Offset: 0x0008956C
		// (set) Token: 0x06001BD3 RID: 7123 RVA: 0x0008B374 File Offset: 0x00089574
		[Browsable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ColumnDefaultCellStyleDescr")]
		public override DataGridViewCellStyle DefaultCellStyle
		{
			get
			{
				return base.DefaultCellStyle;
			}
			set
			{
				base.DefaultCellStyle = value;
			}
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0008B380 File Offset: 0x00089580
		private bool ShouldSerializeDefaultCellStyle()
		{
			if (!base.HasDefaultCellStyle)
			{
				return false;
			}
			DataGridViewCellStyle defaultCellStyle = this.DefaultCellStyle;
			return !defaultCellStyle.BackColor.IsEmpty || !defaultCellStyle.ForeColor.IsEmpty || !defaultCellStyle.SelectionBackColor.IsEmpty || !defaultCellStyle.SelectionForeColor.IsEmpty || defaultCellStyle.Font != null || !defaultCellStyle.IsNullValueDefault || !defaultCellStyle.IsDataSourceNullValueDefault || !string.IsNullOrEmpty(defaultCellStyle.Format) || !defaultCellStyle.FormatProvider.Equals(CultureInfo.CurrentCulture) || defaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet || defaultCellStyle.WrapMode != DataGridViewTriState.NotSet || defaultCellStyle.Tag != null || !defaultCellStyle.Padding.Equals(Padding.Empty);
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x0008B459 File Offset: 0x00089659
		// (set) Token: 0x06001BD6 RID: 7126 RVA: 0x0008B461 File Offset: 0x00089661
		internal int DesiredFillWidth
		{
			get
			{
				return this.desiredFillWidth;
			}
			set
			{
				this.desiredFillWidth = value;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x0008B46A File Offset: 0x0008966A
		// (set) Token: 0x06001BD8 RID: 7128 RVA: 0x0008B472 File Offset: 0x00089672
		internal int DesiredMinimumWidth
		{
			get
			{
				return this.desiredMinimumWidth;
			}
			set
			{
				this.desiredMinimumWidth = value;
			}
		}

		/// <summary>Gets or sets the display order of the column relative to the currently displayed columns.</summary>
		/// <returns>The zero-based position of the column as it is displayed in the associated <see cref="T:System.Windows.Forms.DataGridView" />, or -1 if the band is not contained within a control. </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> is not <see langword="null" /> and the specified value when setting this property is less than 0 or greater than or equal to the number of columns in the control.-or-
		///         <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> is <see langword="null" /> and the specified value when setting this property is less than -1.-or-The specified value when setting this property is equal to <see cref="F:System.Int32.MaxValue" />. </exception>
		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x0008B47B File Offset: 0x0008967B
		// (set) Token: 0x06001BDA RID: 7130 RVA: 0x0008B484 File Offset: 0x00089684
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int DisplayIndex
		{
			get
			{
				return this.displayIndex;
			}
			set
			{
				if (this.displayIndex != value)
				{
					if (value == 2147483647)
					{
						throw new ArgumentOutOfRangeException("DisplayIndex", value, SR.GetString("DataGridViewColumn_DisplayIndexTooLarge", new object[]
						{
							int.MaxValue.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (base.DataGridView != null)
					{
						if (value < 0)
						{
							throw new ArgumentOutOfRangeException("DisplayIndex", value, SR.GetString("DataGridViewColumn_DisplayIndexNegative"));
						}
						if (value >= base.DataGridView.Columns.Count)
						{
							throw new ArgumentOutOfRangeException("DisplayIndex", value, SR.GetString("DataGridViewColumn_DisplayIndexExceedsColumnCount"));
						}
						base.DataGridView.OnColumnDisplayIndexChanging(this, value);
						this.displayIndex = value;
						try
						{
							base.DataGridView.InDisplayIndexAdjustments = true;
							base.DataGridView.OnColumnDisplayIndexChanged_PreNotification();
							base.DataGridView.OnColumnDisplayIndexChanged(this);
							base.DataGridView.OnColumnDisplayIndexChanged_PostNotification();
							return;
						}
						finally
						{
							base.DataGridView.InDisplayIndexAdjustments = false;
						}
					}
					if (value < -1)
					{
						throw new ArgumentOutOfRangeException("DisplayIndex", value, SR.GetString("DataGridViewColumn_DisplayIndexTooNegative"));
					}
					this.displayIndex = value;
				}
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x0008B5BC File Offset: 0x000897BC
		// (set) Token: 0x06001BDC RID: 7132 RVA: 0x0008B5CA File Offset: 0x000897CA
		internal bool DisplayIndexHasChanged
		{
			get
			{
				return (this.flags & 16) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 16;
					return;
				}
				this.flags = (byte)((int)this.flags & -17);
			}
		}

		// Token: 0x1700067C RID: 1660
		// (set) Token: 0x06001BDD RID: 7133 RVA: 0x0008B5F0 File Offset: 0x000897F0
		internal int DisplayIndexInternal
		{
			set
			{
				this.displayIndex = value;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.DataGridViewColumn" /> is disposed.</summary>
		// Token: 0x14000169 RID: 361
		// (add) Token: 0x06001BDE RID: 7134 RVA: 0x0008B5F9 File Offset: 0x000897F9
		// (remove) Token: 0x06001BDF RID: 7135 RVA: 0x0008B612 File Offset: 0x00089812
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Disposed
		{
			add
			{
				this.disposed = (EventHandler)Delegate.Combine(this.disposed, value);
			}
			remove
			{
				this.disposed = (EventHandler)Delegate.Remove(this.disposed, value);
			}
		}

		/// <summary>Gets or sets the width, in pixels, of the column divider.</summary>
		/// <returns>The thickness, in pixels, of the divider (the column's right margin). </returns>
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x0008B62B File Offset: 0x0008982B
		// (set) Token: 0x06001BE1 RID: 7137 RVA: 0x0008B633 File Offset: 0x00089833
		[DefaultValue(0)]
		[SRCategory("CatLayout")]
		[SRDescription("DataGridView_ColumnDividerWidthDescr")]
		public int DividerWidth
		{
			get
			{
				return base.DividerThickness;
			}
			set
			{
				base.DividerThickness = value;
			}
		}

		/// <summary>Gets or sets a value that represents the width of the column when it is in fill mode relative to the widths of other fill-mode columns in the control.</summary>
		/// <returns>A <see cref="T:System.Single" /> representing the width of the column when it is in fill mode relative to the widths of other fill-mode columns. The default is 100.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is less than or equal to 0. </exception>
		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x0008B63C File Offset: 0x0008983C
		// (set) Token: 0x06001BE3 RID: 7139 RVA: 0x0008B644 File Offset: 0x00089844
		[SRCategory("CatLayout")]
		[DefaultValue(100f)]
		[SRDescription("DataGridViewColumn_FillWeightDescr")]
		public float FillWeight
		{
			get
			{
				return this.fillWeight;
			}
			set
			{
				if (value <= 0f)
				{
					throw new ArgumentOutOfRangeException("FillWeight", SR.GetString("InvalidLowBoundArgument", new object[]
					{
						"FillWeight",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value > 65535f)
				{
					throw new ArgumentOutOfRangeException("FillWeight", SR.GetString("InvalidHighBoundArgumentEx", new object[]
					{
						"FillWeight",
						value.ToString(CultureInfo.CurrentCulture),
						ushort.MaxValue.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (base.DataGridView != null)
				{
					base.DataGridView.OnColumnFillWeightChanging(this, value);
					this.fillWeight = value;
					base.DataGridView.OnColumnFillWeightChanged(this);
					return;
				}
				this.fillWeight = value;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (set) Token: 0x06001BE4 RID: 7140 RVA: 0x0008B71B File Offset: 0x0008991B
		internal float FillWeightInternal
		{
			set
			{
				this.fillWeight = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether a column will move when a user scrolls the <see cref="T:System.Windows.Forms.DataGridView" /> control horizontally.</summary>
		/// <returns>
		///     <see langword="true" /> to freeze the column; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x0008B724 File Offset: 0x00089924
		// (set) Token: 0x06001BE6 RID: 7142 RVA: 0x0008B72C File Offset: 0x0008992C
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.All)]
		[SRCategory("CatLayout")]
		[SRDescription("DataGridView_ColumnFrozenDescr")]
		public override bool Frozen
		{
			get
			{
				return base.Frozen;
			}
			set
			{
				base.Frozen = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" /> that represents the column header.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" /> that represents the header cell for the column.</returns>
		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x0008B735 File Offset: 0x00089935
		// (set) Token: 0x06001BE8 RID: 7144 RVA: 0x0008B742 File Offset: 0x00089942
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataGridViewColumnHeaderCell HeaderCell
		{
			get
			{
				return (DataGridViewColumnHeaderCell)base.HeaderCellCore;
			}
			set
			{
				base.HeaderCellCore = value;
			}
		}

		/// <summary>Gets or sets the caption text on the column's header cell.</summary>
		/// <returns>A <see cref="T:System.String" /> with the desired text. The default is an empty string ("").</returns>
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x0008B74C File Offset: 0x0008994C
		// (set) Token: 0x06001BEA RID: 7146 RVA: 0x0008B784 File Offset: 0x00089984
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ColumnHeaderTextDescr")]
		[Localizable(true)]
		public string HeaderText
		{
			get
			{
				if (!base.HasHeaderCell)
				{
					return string.Empty;
				}
				string text = this.HeaderCell.Value as string;
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				if ((value != null || base.HasHeaderCell) && this.HeaderCell.ValueType != null && this.HeaderCell.ValueType.IsAssignableFrom(typeof(string)))
				{
					this.HeaderCell.Value = value;
				}
			}
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0008B7D7 File Offset: 0x000899D7
		private bool ShouldSerializeHeaderText()
		{
			return base.HasHeaderCell && this.HeaderCell.ContainsLocalValue;
		}

		/// <summary>Gets the sizing mode in effect for the column.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnMode" /> value in effect for the column.</returns>
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001BEC RID: 7148 RVA: 0x0008B7EE File Offset: 0x000899EE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataGridViewAutoSizeColumnMode InheritedAutoSizeMode
		{
			get
			{
				return this.GetInheritedAutoSizeMode(base.DataGridView);
			}
		}

		/// <summary>Gets the cell style currently applied to the column.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the cell style used to display the column.</returns>
		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x0008B7FC File Offset: 0x000899FC
		[Browsable(false)]
		public override DataGridViewCellStyle InheritedStyle
		{
			get
			{
				DataGridViewCellStyle dataGridViewCellStyle = null;
				if (base.HasDefaultCellStyle)
				{
					dataGridViewCellStyle = this.DefaultCellStyle;
				}
				if (base.DataGridView == null)
				{
					return dataGridViewCellStyle;
				}
				DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
				DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.BackColor.IsEmpty)
				{
					dataGridViewCellStyle2.BackColor = dataGridViewCellStyle.BackColor;
				}
				else
				{
					dataGridViewCellStyle2.BackColor = defaultCellStyle.BackColor;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle2.ForeColor = dataGridViewCellStyle.ForeColor;
				}
				else
				{
					dataGridViewCellStyle2.ForeColor = defaultCellStyle.ForeColor;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle2.SelectionBackColor = dataGridViewCellStyle.SelectionBackColor;
				}
				else
				{
					dataGridViewCellStyle2.SelectionBackColor = defaultCellStyle.SelectionBackColor;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle2.SelectionForeColor = dataGridViewCellStyle.SelectionForeColor;
				}
				else
				{
					dataGridViewCellStyle2.SelectionForeColor = defaultCellStyle.SelectionForeColor;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Font != null)
				{
					dataGridViewCellStyle2.Font = dataGridViewCellStyle.Font;
				}
				else
				{
					dataGridViewCellStyle2.Font = defaultCellStyle.Font;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsNullValueDefault)
				{
					dataGridViewCellStyle2.NullValue = dataGridViewCellStyle.NullValue;
				}
				else
				{
					dataGridViewCellStyle2.NullValue = defaultCellStyle.NullValue;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsDataSourceNullValueDefault)
				{
					dataGridViewCellStyle2.DataSourceNullValue = dataGridViewCellStyle.DataSourceNullValue;
				}
				else
				{
					dataGridViewCellStyle2.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Format.Length != 0)
				{
					dataGridViewCellStyle2.Format = dataGridViewCellStyle.Format;
				}
				else
				{
					dataGridViewCellStyle2.Format = defaultCellStyle.Format;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsFormatProviderDefault)
				{
					dataGridViewCellStyle2.FormatProvider = dataGridViewCellStyle.FormatProvider;
				}
				else
				{
					dataGridViewCellStyle2.FormatProvider = defaultCellStyle.FormatProvider;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
				{
					dataGridViewCellStyle2.AlignmentInternal = dataGridViewCellStyle.Alignment;
				}
				else
				{
					dataGridViewCellStyle2.AlignmentInternal = defaultCellStyle.Alignment;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.WrapMode != DataGridViewTriState.NotSet)
				{
					dataGridViewCellStyle2.WrapModeInternal = dataGridViewCellStyle.WrapMode;
				}
				else
				{
					dataGridViewCellStyle2.WrapModeInternal = defaultCellStyle.WrapMode;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Tag != null)
				{
					dataGridViewCellStyle2.Tag = dataGridViewCellStyle.Tag;
				}
				else
				{
					dataGridViewCellStyle2.Tag = defaultCellStyle.Tag;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Padding != Padding.Empty)
				{
					dataGridViewCellStyle2.PaddingInternal = dataGridViewCellStyle.Padding;
				}
				else
				{
					dataGridViewCellStyle2.PaddingInternal = defaultCellStyle.Padding;
				}
				return dataGridViewCellStyle2;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001BEE RID: 7150 RVA: 0x0008BA47 File Offset: 0x00089C47
		// (set) Token: 0x06001BEF RID: 7151 RVA: 0x0008BA54 File Offset: 0x00089C54
		internal bool IsBrowsableInternal
		{
			get
			{
				return (this.flags & 8) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 8;
					return;
				}
				this.flags = (byte)((int)this.flags & -9);
			}
		}

		/// <summary>Gets a value indicating whether the column is bound to a data source.</summary>
		/// <returns>
		///     <see langword="true" /> if the column is connected to a data source; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001BF0 RID: 7152 RVA: 0x0008BA79 File Offset: 0x00089C79
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsDataBound
		{
			get
			{
				return this.IsDataBoundInternal;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x0008BA81 File Offset: 0x00089C81
		// (set) Token: 0x06001BF2 RID: 7154 RVA: 0x0008BA8E File Offset: 0x00089C8E
		internal bool IsDataBoundInternal
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

		/// <summary>Gets or sets the minimum width, in pixels, of the column.</summary>
		/// <returns>The number of pixels, from 2 to <see cref="F:System.Int32.MaxValue" />, that specifies the minimum width of the column. The default is 5.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 2 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x0008BAB3 File Offset: 0x00089CB3
		// (set) Token: 0x06001BF4 RID: 7156 RVA: 0x0008BABB File Offset: 0x00089CBB
		[DefaultValue(5)]
		[Localizable(true)]
		[SRCategory("CatLayout")]
		[SRDescription("DataGridView_ColumnMinimumWidthDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int MinimumWidth
		{
			get
			{
				return base.MinimumThickness;
			}
			set
			{
				base.MinimumThickness = value;
			}
		}

		/// <summary>Gets or sets the name of the column.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name of the column. The default is an empty string ("").</returns>
		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x0008BAC4 File Offset: 0x00089CC4
		// (set) Token: 0x06001BF6 RID: 7158 RVA: 0x0008BAF8 File Offset: 0x00089CF8
		[Browsable(false)]
		public string Name
		{
			get
			{
				if (this.Site != null && !string.IsNullOrEmpty(this.Site.Name))
				{
					this.name = this.Site.Name;
				}
				return this.name;
			}
			set
			{
				string b = this.name;
				if (string.IsNullOrEmpty(value))
				{
					this.name = string.Empty;
				}
				else
				{
					this.name = value;
				}
				if (base.DataGridView != null && !string.Equals(this.name, b, StringComparison.Ordinal))
				{
					base.DataGridView.OnColumnNameChanged(this);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can edit the column's cells.</summary>
		/// <returns>
		///     <see langword="true" /> if the user cannot edit the column's cells; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">This property is set to <see langword="false" /> for a column that is bound to a read-only data source. </exception>
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x0008BB4B File Offset: 0x00089D4B
		// (set) Token: 0x06001BF8 RID: 7160 RVA: 0x0008BB54 File Offset: 0x00089D54
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ColumnReadOnlyDescr")]
		public override bool ReadOnly
		{
			get
			{
				return base.ReadOnly;
			}
			set
			{
				if (this.IsDataBound && base.DataGridView != null && base.DataGridView.DataConnection != null && this.boundColumnIndex != -1 && base.DataGridView.DataConnection.DataFieldIsReadOnly(this.boundColumnIndex) && !value)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_ColumnBoundToAReadOnlyFieldMustRemainReadOnly"));
				}
				base.ReadOnly = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the column is resizable.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewTriState" /> values. The default is <see cref="F:System.Windows.Forms.DataGridViewTriState.True" />.</returns>
		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x0008BBB9 File Offset: 0x00089DB9
		// (set) Token: 0x06001BFA RID: 7162 RVA: 0x0008BBC1 File Offset: 0x00089DC1
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ColumnResizableDescr")]
		public override DataGridViewTriState Resizable
		{
			get
			{
				return base.Resizable;
			}
			set
			{
				base.Resizable = value;
			}
		}

		/// <summary>Gets or sets the site of the column.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> associated with the column, if any.</returns>
		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x0008BBCA File Offset: 0x00089DCA
		// (set) Token: 0x06001BFC RID: 7164 RVA: 0x0008BBD2 File Offset: 0x00089DD2
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ISite Site
		{
			get
			{
				return this.site;
			}
			set
			{
				this.site = value;
			}
		}

		/// <summary>Gets or sets the sort mode for the column.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewColumnSortMode" /> that specifies the criteria used to order the rows based on the cell values in a column.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value assigned to the property conflicts with <see cref="P:System.Windows.Forms.DataGridView.SelectionMode" />. </exception>
		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06001BFD RID: 7165 RVA: 0x0008BBDB File Offset: 0x00089DDB
		// (set) Token: 0x06001BFE RID: 7166 RVA: 0x0008BBF8 File Offset: 0x00089DF8
		[DefaultValue(DataGridViewColumnSortMode.NotSortable)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ColumnSortModeDescr")]
		public DataGridViewColumnSortMode SortMode
		{
			get
			{
				if ((this.flags & 1) != 0)
				{
					return DataGridViewColumnSortMode.Automatic;
				}
				if ((this.flags & 2) != 0)
				{
					return DataGridViewColumnSortMode.Programmatic;
				}
				return DataGridViewColumnSortMode.NotSortable;
			}
			set
			{
				if (value != this.SortMode)
				{
					if (value != DataGridViewColumnSortMode.NotSortable)
					{
						if (base.DataGridView != null && !base.DataGridView.InInitialization && value == DataGridViewColumnSortMode.Automatic && (base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect))
						{
							throw new InvalidOperationException(SR.GetString("DataGridViewColumn_SortModeAndSelectionModeClash", new object[]
							{
								value.ToString(),
								base.DataGridView.SelectionMode.ToString()
							}));
						}
						if (value == DataGridViewColumnSortMode.Automatic)
						{
							this.flags = (byte)((int)this.flags & -3);
							this.flags |= 1;
						}
						else
						{
							this.flags = (byte)((int)this.flags & -2);
							this.flags |= 2;
						}
					}
					else
					{
						this.flags = (byte)((int)this.flags & -2);
						this.flags = (byte)((int)this.flags & -3);
					}
					if (base.DataGridView != null)
					{
						base.DataGridView.OnColumnSortModeChanged(this);
					}
				}
			}
		}

		/// <summary>Gets or sets the text used for ToolTips.</summary>
		/// <returns>The text to display as a ToolTip for the column.</returns>
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x0008BD08 File Offset: 0x00089F08
		// (set) Token: 0x06001C00 RID: 7168 RVA: 0x0008BD15 File Offset: 0x00089F15
		[DefaultValue("")]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ColumnToolTipTextDescr")]
		public string ToolTipText
		{
			get
			{
				return this.HeaderCell.ToolTipText;
			}
			set
			{
				if (string.Compare(this.ToolTipText, value, false, CultureInfo.InvariantCulture) != 0)
				{
					this.HeaderCell.ToolTipText = value;
					if (base.DataGridView != null)
					{
						base.DataGridView.OnColumnToolTipTextChanged(this);
					}
				}
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x0008BD4B File Offset: 0x00089F4B
		// (set) Token: 0x06001C02 RID: 7170 RVA: 0x0008BD53 File Offset: 0x00089F53
		internal float UsedFillWeight
		{
			get
			{
				return this.usedFillWeight;
			}
			set
			{
				this.usedFillWeight = value;
			}
		}

		/// <summary>Gets or sets the data type of the values in the column's cells.</summary>
		/// <returns>A <see cref="T:System.Type" /> that describes the run-time class of the values stored in the column's cells.</returns>
		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x0008BD5C File Offset: 0x00089F5C
		// (set) Token: 0x06001C04 RID: 7172 RVA: 0x0008BD73 File Offset: 0x00089F73
		[Browsable(false)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Type ValueType
		{
			get
			{
				return (Type)base.Properties.GetObject(DataGridViewColumn.PropDataGridViewColumnValueType);
			}
			set
			{
				base.Properties.SetObject(DataGridViewColumn.PropDataGridViewColumnValueType, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the column is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the column is visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001C05 RID: 7173 RVA: 0x0008BD86 File Offset: 0x00089F86
		// (set) Token: 0x06001C06 RID: 7174 RVA: 0x0008BD8E File Offset: 0x00089F8E
		[DefaultValue(true)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ColumnVisibleDescr")]
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		/// <summary>Gets or sets the current width of the column.</summary>
		/// <returns>The width, in pixels, of the column. The default is 100.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is greater than 65536.</exception>
		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001C07 RID: 7175 RVA: 0x0008BD97 File Offset: 0x00089F97
		// (set) Token: 0x06001C08 RID: 7176 RVA: 0x0008BD9F File Offset: 0x00089F9F
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("DataGridView_ColumnWidthDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Width
		{
			get
			{
				return base.Thickness;
			}
			set
			{
				base.Thickness = value;
			}
		}

		/// <summary>Creates an exact copy of this band.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewBand" />.</returns>
		// Token: 0x06001C09 RID: 7177 RVA: 0x0008BDA8 File Offset: 0x00089FA8
		public override object Clone()
		{
			DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)Activator.CreateInstance(base.GetType());
			if (dataGridViewColumn != null)
			{
				this.CloneInternal(dataGridViewColumn);
			}
			return dataGridViewColumn;
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x0008BDD4 File Offset: 0x00089FD4
		internal void CloneInternal(DataGridViewColumn dataGridViewColumn)
		{
			base.CloneInternal(dataGridViewColumn);
			dataGridViewColumn.name = this.Name;
			dataGridViewColumn.displayIndex = -1;
			dataGridViewColumn.HeaderText = this.HeaderText;
			dataGridViewColumn.DataPropertyName = this.DataPropertyName;
			if (dataGridViewColumn.CellTemplate != null)
			{
				dataGridViewColumn.cellTemplate = (DataGridViewCell)this.CellTemplate.Clone();
			}
			else
			{
				dataGridViewColumn.cellTemplate = null;
			}
			if (base.HasHeaderCell)
			{
				dataGridViewColumn.HeaderCell = (DataGridViewColumnHeaderCell)this.HeaderCell.Clone();
			}
			dataGridViewColumn.AutoSizeMode = this.AutoSizeMode;
			dataGridViewColumn.SortMode = this.SortMode;
			dataGridViewColumn.FillWeightInternal = this.FillWeight;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.DataGridViewBand" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06001C0B RID: 7179 RVA: 0x0008BE7C File Offset: 0x0008A07C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					lock (this)
					{
						if (this.site != null && this.site.Container != null)
						{
							this.site.Container.Remove(this);
						}
						if (this.disposed != null)
						{
							this.disposed(this, EventArgs.Empty);
						}
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x0008BF0C File Offset: 0x0008A10C
		internal DataGridViewAutoSizeColumnMode GetInheritedAutoSizeMode(DataGridView dataGridView)
		{
			if (dataGridView != null && this.autoSizeMode == DataGridViewAutoSizeColumnMode.NotSet)
			{
				DataGridViewAutoSizeColumnsMode autoSizeColumnsMode = dataGridView.AutoSizeColumnsMode;
				switch (autoSizeColumnsMode)
				{
				case DataGridViewAutoSizeColumnsMode.ColumnHeader:
					return DataGridViewAutoSizeColumnMode.ColumnHeader;
				case (DataGridViewAutoSizeColumnsMode)3:
				case (DataGridViewAutoSizeColumnsMode)5:
				case (DataGridViewAutoSizeColumnsMode)7:
				case (DataGridViewAutoSizeColumnsMode)9:
					break;
				case DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader:
					return DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
				case DataGridViewAutoSizeColumnsMode.AllCells:
					return DataGridViewAutoSizeColumnMode.AllCells;
				case DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader:
					return DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
				case DataGridViewAutoSizeColumnsMode.DisplayedCells:
					return DataGridViewAutoSizeColumnMode.DisplayedCells;
				default:
					if (autoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.Fill)
					{
						return DataGridViewAutoSizeColumnMode.Fill;
					}
					break;
				}
				return DataGridViewAutoSizeColumnMode.None;
			}
			return this.autoSizeMode;
		}

		/// <summary>Calculates the ideal width of the column based on the specified criteria.</summary>
		/// <param name="autoSizeColumnMode">A <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnMode" /> value that specifies an automatic sizing mode. </param>
		/// <param name="fixedHeight">
		///       <see langword="true" /> to calculate the width of the column based on the current row heights; <see langword="false" /> to calculate the width with the expectation that the row heights will be adjusted.</param>
		/// <returns>The ideal width, in pixels, of the column.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="autoSizeColumnMode" /> is <see cref="F:System.Windows.Forms.DataGridViewAutoSizeColumnMode.NotSet" />, <see cref="F:System.Windows.Forms.DataGridViewAutoSizeColumnMode.None" />, or <see cref="F:System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill" />. </exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="autoSizeColumnMode" /> is not a valid <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnMode" /> value. </exception>
		// Token: 0x06001C0D RID: 7181 RVA: 0x0008BF74 File Offset: 0x0008A174
		public virtual int GetPreferredWidth(DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool fixedHeight)
		{
			if (autoSizeColumnMode == DataGridViewAutoSizeColumnMode.NotSet || autoSizeColumnMode == DataGridViewAutoSizeColumnMode.None || autoSizeColumnMode == DataGridViewAutoSizeColumnMode.Fill)
			{
				throw new ArgumentException(SR.GetString("DataGridView_NeedColumnAutoSizingCriteria", new object[]
				{
					"autoSizeColumnMode"
				}));
			}
			switch (autoSizeColumnMode)
			{
			case DataGridViewAutoSizeColumnMode.NotSet:
			case DataGridViewAutoSizeColumnMode.None:
			case DataGridViewAutoSizeColumnMode.ColumnHeader:
			case DataGridViewAutoSizeColumnMode.AllCellsExceptHeader:
			case DataGridViewAutoSizeColumnMode.AllCells:
			case DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader:
			case DataGridViewAutoSizeColumnMode.DisplayedCells:
				goto IL_77;
			case (DataGridViewAutoSizeColumnMode)3:
			case (DataGridViewAutoSizeColumnMode)5:
			case (DataGridViewAutoSizeColumnMode)7:
			case (DataGridViewAutoSizeColumnMode)9:
				break;
			default:
				if (autoSizeColumnMode == DataGridViewAutoSizeColumnMode.Fill)
				{
					goto IL_77;
				}
				break;
			}
			throw new InvalidEnumArgumentException("value", (int)autoSizeColumnMode, typeof(DataGridViewAutoSizeColumnMode));
			IL_77:
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null)
			{
				return -1;
			}
			int num = 0;
			if (dataGridView.ColumnHeadersVisible && (autoSizeColumnMode & DataGridViewAutoSizeColumnMode.ColumnHeader) != DataGridViewAutoSizeColumnMode.NotSet)
			{
				int num2;
				if (fixedHeight)
				{
					num2 = this.HeaderCell.GetPreferredWidth(-1, dataGridView.ColumnHeadersHeight);
				}
				else
				{
					num2 = this.HeaderCell.GetPreferredSize(-1).Width;
				}
				if (num < num2)
				{
					num = num2;
				}
			}
			if ((autoSizeColumnMode & DataGridViewAutoSizeColumnMode.AllCellsExceptHeader) != DataGridViewAutoSizeColumnMode.NotSet)
			{
				for (int num3 = dataGridView.Rows.GetFirstRow(DataGridViewElementStates.Visible); num3 != -1; num3 = dataGridView.Rows.GetNextRow(num3, DataGridViewElementStates.Visible))
				{
					DataGridViewRow dataGridViewRow = dataGridView.Rows.SharedRow(num3);
					int num2;
					if (fixedHeight)
					{
						num2 = dataGridViewRow.Cells[base.Index].GetPreferredWidth(num3, dataGridViewRow.Thickness);
					}
					else
					{
						num2 = dataGridViewRow.Cells[base.Index].GetPreferredSize(num3).Width;
					}
					if (num < num2)
					{
						num = num2;
					}
				}
			}
			else if ((autoSizeColumnMode & DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader) != DataGridViewAutoSizeColumnMode.NotSet)
			{
				int height = dataGridView.LayoutInfo.Data.Height;
				int num4 = 0;
				int num3 = dataGridView.Rows.GetFirstRow(DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible);
				while (num3 != -1 && num4 < height)
				{
					DataGridViewRow dataGridViewRow = dataGridView.Rows.SharedRow(num3);
					int num2;
					if (fixedHeight)
					{
						num2 = dataGridViewRow.Cells[base.Index].GetPreferredWidth(num3, dataGridViewRow.Thickness);
					}
					else
					{
						num2 = dataGridViewRow.Cells[base.Index].GetPreferredSize(num3).Width;
					}
					if (num < num2)
					{
						num = num2;
					}
					num4 += dataGridViewRow.Thickness;
					num3 = dataGridView.Rows.GetNextRow(num3, DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible);
				}
				if (num4 < height)
				{
					num3 = dataGridView.DisplayedBandsInfo.FirstDisplayedScrollingRow;
					while (num3 != -1 && num4 < height)
					{
						DataGridViewRow dataGridViewRow = dataGridView.Rows.SharedRow(num3);
						int num2;
						if (fixedHeight)
						{
							num2 = dataGridViewRow.Cells[base.Index].GetPreferredWidth(num3, dataGridViewRow.Thickness);
						}
						else
						{
							num2 = dataGridViewRow.Cells[base.Index].GetPreferredSize(num3).Width;
						}
						if (num < num2)
						{
							num = num2;
						}
						num4 += dataGridViewRow.Thickness;
						num3 = dataGridView.Rows.GetNextRow(num3, DataGridViewElementStates.Visible);
					}
				}
			}
			return num;
		}

		/// <summary>Gets a string that describes the column.</summary>
		/// <returns>A <see cref="T:System.String" /> that describes the column.</returns>
		// Token: 0x06001C0E RID: 7182 RVA: 0x0008C234 File Offset: 0x0008A434
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewColumn { Name=");
			stringBuilder.Append(this.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04000C71 RID: 3185
		private const float DATAGRIDVIEWCOLUMN_defaultFillWeight = 100f;

		// Token: 0x04000C72 RID: 3186
		private const int DATAGRIDVIEWCOLUMN_defaultWidth = 100;

		// Token: 0x04000C73 RID: 3187
		private const int DATAGRIDVIEWCOLUMN_defaultMinColumnThickness = 5;

		// Token: 0x04000C74 RID: 3188
		private const byte DATAGRIDVIEWCOLUMN_automaticSort = 1;

		// Token: 0x04000C75 RID: 3189
		private const byte DATAGRIDVIEWCOLUMN_programmaticSort = 2;

		// Token: 0x04000C76 RID: 3190
		private const byte DATAGRIDVIEWCOLUMN_isDataBound = 4;

		// Token: 0x04000C77 RID: 3191
		private const byte DATAGRIDVIEWCOLUMN_isBrowsableInternal = 8;

		// Token: 0x04000C78 RID: 3192
		private const byte DATAGRIDVIEWCOLUMN_displayIndexHasChangedInternal = 16;

		// Token: 0x04000C79 RID: 3193
		private byte flags;

		// Token: 0x04000C7A RID: 3194
		private DataGridViewCell cellTemplate;

		// Token: 0x04000C7B RID: 3195
		private string name;

		// Token: 0x04000C7C RID: 3196
		private int displayIndex;

		// Token: 0x04000C7D RID: 3197
		private int desiredFillWidth;

		// Token: 0x04000C7E RID: 3198
		private int desiredMinimumWidth;

		// Token: 0x04000C7F RID: 3199
		private float fillWeight;

		// Token: 0x04000C80 RID: 3200
		private float usedFillWeight;

		// Token: 0x04000C81 RID: 3201
		private DataGridViewAutoSizeColumnMode autoSizeMode;

		// Token: 0x04000C82 RID: 3202
		private int boundColumnIndex = -1;

		// Token: 0x04000C83 RID: 3203
		private string dataPropertyName = string.Empty;

		// Token: 0x04000C84 RID: 3204
		private TypeConverter boundColumnConverter;

		// Token: 0x04000C85 RID: 3205
		private ISite site;

		// Token: 0x04000C86 RID: 3206
		private EventHandler disposed;

		// Token: 0x04000C87 RID: 3207
		private static readonly int PropDataGridViewColumnValueType = PropertyStore.CreateKey();
	}
}
