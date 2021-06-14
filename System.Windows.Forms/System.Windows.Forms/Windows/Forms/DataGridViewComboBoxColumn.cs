using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Represents a column of <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> objects.</summary>
	// Token: 0x020001B8 RID: 440
	[Designer("System.Windows.Forms.Design.DataGridViewComboBoxColumnDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxBitmap(typeof(DataGridViewComboBoxColumn), "DataGridViewComboBoxColumn.bmp")]
	public class DataGridViewComboBoxColumn : DataGridViewColumn
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewTextBoxColumn" /> class to the default state.</summary>
		// Token: 0x06001CCE RID: 7374 RVA: 0x000929CD File Offset: 0x00090BCD
		public DataGridViewComboBoxColumn() : base(new DataGridViewComboBoxCell())
		{
			((DataGridViewComboBoxCell)base.CellTemplate).TemplateComboBoxColumn = this;
		}

		/// <summary>Gets or sets a value indicating whether cells in the column will match the characters being entered in the cell with one from the possible selections. </summary>
		/// <returns>
		///     <see langword="true" /> if auto completion is activated; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x000929EB File Offset: 0x00090BEB
		// (set) Token: 0x06001CD0 RID: 7376 RVA: 0x00092A10 File Offset: 0x00090C10
		[Browsable(true)]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ComboBoxColumnAutoCompleteDescr")]
		public bool AutoComplete
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.AutoComplete;
			}
			set
			{
				if (this.AutoComplete != value)
				{
					this.ComboBoxCellTemplate.AutoComplete = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.AutoComplete = value;
							}
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the template used to create cells.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCell" /> that all other cells in the column are modeled after. The default value is a new <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</returns>
		/// <exception cref="T:System.InvalidCastException">When setting this property to a value that is not of type <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />. </exception>
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x00081081 File Offset: 0x0007F281
		// (set) Token: 0x06001CD2 RID: 7378 RVA: 0x00092A88 File Offset: 0x00090C88
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				DataGridViewComboBoxCell dataGridViewComboBoxCell = value as DataGridViewComboBoxCell;
				if (value != null && dataGridViewComboBoxCell == null)
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[]
					{
						"System.Windows.Forms.DataGridViewComboBoxCell"
					}));
				}
				base.CellTemplate = value;
				if (value != null)
				{
					dataGridViewComboBoxCell.TemplateComboBoxColumn = this;
				}
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x00092AD1 File Offset: 0x00090CD1
		private DataGridViewComboBoxCell ComboBoxCellTemplate
		{
			get
			{
				return (DataGridViewComboBoxCell)this.CellTemplate;
			}
		}

		/// <summary>Gets or sets the data source that populates the selections for the combo boxes.</summary>
		/// <returns>An object that represents a data source. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x00092ADE File Offset: 0x00090CDE
		// (set) Token: 0x06001CD5 RID: 7381 RVA: 0x00092B04 File Offset: 0x00090D04
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_ComboBoxColumnDataSourceDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		public object DataSource
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DataSource;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.DataSource = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.DataSource = value;
						}
					}
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		/// <summary>Gets or sets a string that specifies the property or column from which to retrieve strings for display in the combo boxes.</summary>
		/// <returns>A <see cref="T:System.String" /> that specifies the name of a property or column in the data source specified in the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.DataSource" /> property. The default is <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001CD6 RID: 7382 RVA: 0x00092B99 File Offset: 0x00090D99
		// (set) Token: 0x06001CD7 RID: 7383 RVA: 0x00092BC0 File Offset: 0x00090DC0
		[DefaultValue("")]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_ComboBoxColumnDisplayMemberDescr")]
		[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string DisplayMember
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DisplayMember;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.DisplayMember = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.DisplayMember = value;
						}
					}
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		/// <summary>Gets or sets a value that determines how the combo box is displayed when not editing.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewComboBoxDisplayStyle" /> value indicating the combo box appearance. The default is <see cref="F:System.Windows.Forms.DataGridViewComboBoxDisplayStyle.DropDownButton" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001CD8 RID: 7384 RVA: 0x00092C55 File Offset: 0x00090E55
		// (set) Token: 0x06001CD9 RID: 7385 RVA: 0x00092C7C File Offset: 0x00090E7C
		[DefaultValue(DataGridViewComboBoxDisplayStyle.DropDownButton)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ComboBoxColumnDisplayStyleDescr")]
		public DataGridViewComboBoxDisplayStyle DisplayStyle
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DisplayStyle;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.DisplayStyle = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.DisplayStyleInternal = value;
						}
					}
					base.DataGridView.InvalidateColumn(base.Index);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.DisplayStyle" /> property value applies only to the current cell in the <see cref="T:System.Windows.Forms.DataGridView" /> control when the current cell is in this column.</summary>
		/// <returns>
		///     <see langword="true" /> if the display style applies only to the current cell; otherwise <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001CDA RID: 7386 RVA: 0x00092D11 File Offset: 0x00090F11
		// (set) Token: 0x06001CDB RID: 7387 RVA: 0x00092D38 File Offset: 0x00090F38
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ComboBoxColumnDisplayStyleForCurrentCellOnlyDescr")]
		public bool DisplayStyleForCurrentCellOnly
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DisplayStyleForCurrentCellOnly;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.DisplayStyleForCurrentCellOnly = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.DisplayStyleForCurrentCellOnlyInternal = value;
						}
					}
					base.DataGridView.InvalidateColumn(base.Index);
				}
			}
		}

		/// <summary>Gets or sets the width of the drop-down lists of the combo boxes.</summary>
		/// <returns>The width, in pixels, of the drop-down lists. The default is 1.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001CDC RID: 7388 RVA: 0x00092DCD File Offset: 0x00090FCD
		// (set) Token: 0x06001CDD RID: 7389 RVA: 0x00092DF4 File Offset: 0x00090FF4
		[DefaultValue(1)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ComboBoxColumnDropDownWidthDescr")]
		public int DropDownWidth
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DropDownWidth;
			}
			set
			{
				if (this.DropDownWidth != value)
				{
					this.ComboBoxCellTemplate.DropDownWidth = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.DropDownWidth = value;
							}
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the flat style appearance of the column's cells.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FlatStyle" /> value indicating the cell appearance. The default is <see cref="F:System.Windows.Forms.FlatStyle.Standard" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001CDE RID: 7390 RVA: 0x00092E69 File Offset: 0x00091069
		// (set) Token: 0x06001CDF RID: 7391 RVA: 0x00092E94 File Offset: 0x00091094
		[DefaultValue(FlatStyle.Standard)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ComboBoxColumnFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewComboBoxCell)this.CellTemplate).FlatStyle;
			}
			set
			{
				if (this.FlatStyle != value)
				{
					((DataGridViewComboBoxCell)this.CellTemplate).FlatStyle = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.FlatStyleInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		/// <summary>Gets the collection of objects used as selections in the combo boxes.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell.ObjectCollection" /> that represents the selections in the combo boxes. </returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001CE0 RID: 7392 RVA: 0x00092F1F File Offset: 0x0009111F
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_ComboBoxColumnItemsDescr")]
		public DataGridViewComboBoxCell.ObjectCollection Items
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.GetItems(base.DataGridView);
			}
		}

		/// <summary>Gets or sets a string that specifies the property or column from which to get values that correspond to the selections in the drop-down list.</summary>
		/// <returns>A <see cref="T:System.String" /> that specifies the name of a property or column used in the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.DataSource" /> property. The default is <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x00092F4A File Offset: 0x0009114A
		// (set) Token: 0x06001CE2 RID: 7394 RVA: 0x00092F70 File Offset: 0x00091170
		[DefaultValue("")]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_ComboBoxColumnValueMemberDescr")]
		[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string ValueMember
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.ValueMember;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.ValueMember = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.ValueMember = value;
						}
					}
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		/// <summary>Gets or sets the maximum number of items in the drop-down list of the cells in the column.</summary>
		/// <returns>The maximum number of drop-down list items, from 1 to 100. The default is 8.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x00093005 File Offset: 0x00091205
		// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x0009302C File Offset: 0x0009122C
		[DefaultValue(8)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ComboBoxColumnMaxDropDownItemsDescr")]
		public int MaxDropDownItems
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.MaxDropDownItems;
			}
			set
			{
				if (this.MaxDropDownItems != value)
				{
					this.ComboBoxCellTemplate.MaxDropDownItems = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.MaxDropDownItems = value;
							}
						}
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the items in the combo box are sorted.</summary>
		/// <returns>
		///     <see langword="true" /> if the combo box is sorted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewComboBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x000930A1 File Offset: 0x000912A1
		// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x000930C8 File Offset: 0x000912C8
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ComboBoxColumnSortedDescr")]
		public bool Sorted
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.Sorted;
			}
			set
			{
				if (this.Sorted != value)
				{
					this.ComboBoxCellTemplate.Sorted = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.Sorted = value;
							}
						}
					}
				}
			}
		}

		/// <summary>Creates an exact copy of this column.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewComboBoxColumn" />.</returns>
		// Token: 0x06001CE7 RID: 7399 RVA: 0x00093140 File Offset: 0x00091340
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn;
			if (type == DataGridViewComboBoxColumn.columnType)
			{
				dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
			}
			else
			{
				dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)Activator.CreateInstance(type);
			}
			if (dataGridViewComboBoxColumn != null)
			{
				base.CloneInternal(dataGridViewComboBoxColumn);
				((DataGridViewComboBoxCell)dataGridViewComboBoxColumn.CellTemplate).TemplateComboBoxColumn = dataGridViewComboBoxColumn;
			}
			return dataGridViewComboBoxColumn;
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x00093194 File Offset: 0x00091394
		internal void OnItemsCollectionChanged()
		{
			if (base.DataGridView != null)
			{
				DataGridViewRowCollection rows = base.DataGridView.Rows;
				int count = rows.Count;
				object[] items = ((DataGridViewComboBoxCell)this.CellTemplate).Items.InnerArray.ToArray();
				for (int i = 0; i < count; i++)
				{
					DataGridViewRow dataGridViewRow = rows.SharedRow(i);
					DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
					if (dataGridViewComboBoxCell != null)
					{
						dataGridViewComboBoxCell.Items.ClearInternal();
						dataGridViewComboBoxCell.Items.AddRangeInternal(items);
					}
				}
				base.DataGridView.OnColumnCommonChange(base.Index);
			}
		}

		/// <summary>Gets a string that describes the column.</summary>
		/// <returns>A <see cref="T:System.String" /> that describes the column.</returns>
		// Token: 0x06001CE9 RID: 7401 RVA: 0x00093238 File Offset: 0x00091438
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewComboBoxColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04000CD5 RID: 3285
		private static Type columnType = typeof(DataGridViewComboBoxColumn);
	}
}
