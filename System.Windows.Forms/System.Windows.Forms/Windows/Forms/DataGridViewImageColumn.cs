using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Hosts a collection of <see cref="T:System.Windows.Forms.DataGridViewImageCell" /> objects.</summary>
	// Token: 0x020001F0 RID: 496
	[ToolboxBitmap(typeof(DataGridViewImageColumn), "DataGridViewImageColumn.bmp")]
	public class DataGridViewImageColumn : DataGridViewColumn
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewImageColumn" /> class, configuring it for use with cell values of type <see cref="T:System.Drawing.Image" />.</summary>
		// Token: 0x06001E04 RID: 7684 RVA: 0x00095489 File Offset: 0x00093689
		public DataGridViewImageColumn() : this(false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewImageColumn" /> class, optionally configuring it for use with <see cref="T:System.Drawing.Icon" /> cell values.</summary>
		/// <param name="valuesAreIcons">
		///       <see langword="true" /> to indicate that the <see cref="P:System.Windows.Forms.DataGridViewCell.Value" /> property of cells in this column will be set to values of type <see cref="T:System.Drawing.Icon" />; <see langword="false" /> to indicate that they will be set to values of type <see cref="T:System.Drawing.Image" />.</param>
		// Token: 0x06001E05 RID: 7685 RVA: 0x00095494 File Offset: 0x00093694
		public DataGridViewImageColumn(bool valuesAreIcons) : base(new DataGridViewImageCell(valuesAreIcons))
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			dataGridViewCellStyle.AlignmentInternal = DataGridViewContentAlignment.MiddleCenter;
			if (valuesAreIcons)
			{
				dataGridViewCellStyle.NullValue = DataGridViewImageCell.ErrorIcon;
			}
			else
			{
				dataGridViewCellStyle.NullValue = DataGridViewImageCell.ErrorBitmap;
			}
			this.DefaultCellStyle = dataGridViewCellStyle;
		}

		/// <summary>Gets or sets the template used to create new cells.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCell" /> that all other cells in the column are modeled after.</returns>
		/// <exception cref="T:System.InvalidCastException">The set type is not compatible with type <see cref="T:System.Windows.Forms.DataGridViewImageCell" />. </exception>
		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001E06 RID: 7686 RVA: 0x00081081 File Offset: 0x0007F281
		// (set) Token: 0x06001E07 RID: 7687 RVA: 0x000954DD File Offset: 0x000936DD
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
				if (value != null && !(value is DataGridViewImageCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[]
					{
						"System.Windows.Forms.DataGridViewImageCell"
					}));
				}
				base.CellTemplate = value;
			}
		}

		/// <summary>Gets or sets the column's default cell style.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied as the default style.</returns>
		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x000810BB File Offset: 0x0007F2BB
		// (set) Token: 0x06001E09 RID: 7689 RVA: 0x000810C3 File Offset: 0x0007F2C3
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

		/// <summary>Gets or sets a string that describes the column's image. </summary>
		/// <returns>The textual description of the column image. The default is <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewImageColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x0009550F File Offset: 0x0009370F
		// (set) Token: 0x06001E0B RID: 7691 RVA: 0x00095534 File Offset: 0x00093734
		[Browsable(true)]
		[DefaultValue("")]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridViewImageColumn_DescriptionDescr")]
		public string Description
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ImageCellTemplate.Description;
			}
			set
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ImageCellTemplate.Description = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewImageCell dataGridViewImageCell = dataGridViewRow.Cells[base.Index] as DataGridViewImageCell;
						if (dataGridViewImageCell != null)
						{
							dataGridViewImageCell.Description = value;
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the icon displayed in the cells of this column when the cell's <see cref="P:System.Windows.Forms.DataGridViewCell.Value" /> property is not set and the cell's <see cref="P:System.Windows.Forms.DataGridViewImageCell.ValueIsIcon" /> property is set to <see langword="true" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Icon" /> to display. The default is <see langword="null" />.</returns>
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x000955B8 File Offset: 0x000937B8
		// (set) Token: 0x06001E0D RID: 7693 RVA: 0x000955C0 File Offset: 0x000937C0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
				if (base.DataGridView != null)
				{
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		/// <summary>Gets or sets the image displayed in the cells of this column when the cell's <see cref="P:System.Windows.Forms.DataGridViewCell.Value" /> property is not set and the cell's <see cref="P:System.Windows.Forms.DataGridViewImageCell.ValueIsIcon" /> property is set to <see langword="false" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> to display. The default is <see langword="null" />.</returns>
		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001E0E RID: 7694 RVA: 0x000955E2 File Offset: 0x000937E2
		// (set) Token: 0x06001E0F RID: 7695 RVA: 0x000955EA File Offset: 0x000937EA
		[DefaultValue(null)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridViewImageColumn_ImageDescr")]
		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
				if (base.DataGridView != null)
				{
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001E10 RID: 7696 RVA: 0x0009560C File Offset: 0x0009380C
		private DataGridViewImageCell ImageCellTemplate
		{
			get
			{
				return (DataGridViewImageCell)this.CellTemplate;
			}
		}

		/// <summary>Gets or sets the image layout in the cells for this column.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewImageCellLayout" /> that specifies the cell layout. The default is <see cref="F:System.Windows.Forms.DataGridViewImageCellLayout.Normal" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewImageColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001E11 RID: 7697 RVA: 0x0009561C File Offset: 0x0009381C
		// (set) Token: 0x06001E12 RID: 7698 RVA: 0x00095654 File Offset: 0x00093854
		[DefaultValue(DataGridViewImageCellLayout.Normal)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridViewImageColumn_ImageLayoutDescr")]
		public DataGridViewImageCellLayout ImageLayout
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				DataGridViewImageCellLayout dataGridViewImageCellLayout = this.ImageCellTemplate.ImageLayout;
				if (dataGridViewImageCellLayout == DataGridViewImageCellLayout.NotSet)
				{
					dataGridViewImageCellLayout = DataGridViewImageCellLayout.Normal;
				}
				return dataGridViewImageCellLayout;
			}
			set
			{
				if (this.ImageLayout != value)
				{
					this.ImageCellTemplate.ImageLayout = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewImageCell dataGridViewImageCell = dataGridViewRow.Cells[base.Index] as DataGridViewImageCell;
							if (dataGridViewImageCell != null)
							{
								dataGridViewImageCell.ImageLayoutInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether cells in this column display <see cref="T:System.Drawing.Icon" /> values.</summary>
		/// <returns>
		///     <see langword="true" /> if cells display values of type <see cref="T:System.Drawing.Icon" />; <see langword="false" /> if cells display values of type <see cref="T:System.Drawing.Image" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewImageColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001E13 RID: 7699 RVA: 0x000956DA File Offset: 0x000938DA
		// (set) Token: 0x06001E14 RID: 7700 RVA: 0x00095700 File Offset: 0x00093900
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ValuesAreIcons
		{
			get
			{
				if (this.ImageCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ImageCellTemplate.ValueIsIcon;
			}
			set
			{
				if (this.ValuesAreIcons != value)
				{
					this.ImageCellTemplate.ValueIsIconInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewImageCell dataGridViewImageCell = dataGridViewRow.Cells[base.Index] as DataGridViewImageCell;
							if (dataGridViewImageCell != null)
							{
								dataGridViewImageCell.ValueIsIconInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
					if (value && this.DefaultCellStyle.NullValue is Bitmap && (Bitmap)this.DefaultCellStyle.NullValue == DataGridViewImageCell.ErrorBitmap)
					{
						this.DefaultCellStyle.NullValue = DataGridViewImageCell.ErrorIcon;
						return;
					}
					if (!value && this.DefaultCellStyle.NullValue is Icon && (Icon)this.DefaultCellStyle.NullValue == DataGridViewImageCell.ErrorIcon)
					{
						this.DefaultCellStyle.NullValue = DataGridViewImageCell.ErrorBitmap;
					}
				}
			}
		}

		/// <summary>Creates an exact copy of this column.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewImageColumn" />.</returns>
		// Token: 0x06001E15 RID: 7701 RVA: 0x00095804 File Offset: 0x00093A04
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewImageColumn dataGridViewImageColumn;
			if (type == DataGridViewImageColumn.columnType)
			{
				dataGridViewImageColumn = new DataGridViewImageColumn();
			}
			else
			{
				dataGridViewImageColumn = (DataGridViewImageColumn)Activator.CreateInstance(type);
			}
			if (dataGridViewImageColumn != null)
			{
				base.CloneInternal(dataGridViewImageColumn);
				dataGridViewImageColumn.Icon = this.icon;
				dataGridViewImageColumn.Image = this.image;
			}
			return dataGridViewImageColumn;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x0009585C File Offset: 0x00093A5C
		private bool ShouldSerializeDefaultCellStyle()
		{
			DataGridViewImageCell dataGridViewImageCell = this.CellTemplate as DataGridViewImageCell;
			if (dataGridViewImageCell == null)
			{
				return true;
			}
			if (!base.HasDefaultCellStyle)
			{
				return false;
			}
			object obj;
			if (dataGridViewImageCell.ValueIsIcon)
			{
				obj = DataGridViewImageCell.ErrorIcon;
			}
			else
			{
				obj = DataGridViewImageCell.ErrorBitmap;
			}
			DataGridViewCellStyle defaultCellStyle = this.DefaultCellStyle;
			return !defaultCellStyle.BackColor.IsEmpty || !defaultCellStyle.ForeColor.IsEmpty || !defaultCellStyle.SelectionBackColor.IsEmpty || !defaultCellStyle.SelectionForeColor.IsEmpty || defaultCellStyle.Font != null || !obj.Equals(defaultCellStyle.NullValue) || !defaultCellStyle.IsDataSourceNullValueDefault || !string.IsNullOrEmpty(defaultCellStyle.Format) || !defaultCellStyle.FormatProvider.Equals(CultureInfo.CurrentCulture) || defaultCellStyle.Alignment != DataGridViewContentAlignment.MiddleCenter || defaultCellStyle.WrapMode != DataGridViewTriState.NotSet || defaultCellStyle.Tag != null || !defaultCellStyle.Padding.Equals(Padding.Empty);
		}

		/// <summary>Gets a string that describes the column.</summary>
		/// <returns>A <see cref="T:System.String" /> that describes the column.</returns>
		// Token: 0x06001E17 RID: 7703 RVA: 0x00095968 File Offset: 0x00093B68
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewImageColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04000D3C RID: 3388
		private static Type columnType = typeof(DataGridViewImageColumn);

		// Token: 0x04000D3D RID: 3389
		private Image image;

		// Token: 0x04000D3E RID: 3390
		private Icon icon;
	}
}
