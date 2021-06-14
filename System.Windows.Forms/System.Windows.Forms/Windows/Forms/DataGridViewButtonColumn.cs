using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Hosts a collection of <see cref="T:System.Windows.Forms.DataGridViewButtonCell" /> objects.</summary>
	// Token: 0x02000191 RID: 401
	[ToolboxBitmap(typeof(DataGridViewButtonColumn), "DataGridViewButtonColumn.bmp")]
	public class DataGridViewButtonColumn : DataGridViewColumn
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewButtonColumn" /> class to the default state.</summary>
		// Token: 0x060019CF RID: 6607 RVA: 0x00081054 File Offset: 0x0007F254
		public DataGridViewButtonColumn() : base(new DataGridViewButtonCell())
		{
			this.DefaultCellStyle = new DataGridViewCellStyle
			{
				AlignmentInternal = DataGridViewContentAlignment.MiddleCenter
			};
		}

		/// <summary>Gets or sets the template used to create new cells.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCell" /> that all other cells in the column are modeled after.</returns>
		/// <exception cref="T:System.InvalidCastException">The specified value when setting this property could not be cast to a <see cref="T:System.Windows.Forms.DataGridViewButtonCell" />. </exception>
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x00081081 File Offset: 0x0007F281
		// (set) Token: 0x060019D1 RID: 6609 RVA: 0x00081089 File Offset: 0x0007F289
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
				if (value != null && !(value is DataGridViewButtonCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[]
					{
						"System.Windows.Forms.DataGridViewButtonCell"
					}));
				}
				base.CellTemplate = value;
			}
		}

		/// <summary>Gets or sets the column's default cell style.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied as the default style.</returns>
		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x000810BB File Offset: 0x0007F2BB
		// (set) Token: 0x060019D3 RID: 6611 RVA: 0x000810C3 File Offset: 0x0007F2C3
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

		/// <summary>Gets or sets the flat-style appearance of the button cells in the column.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FlatStyle" /> value indicating the appearance of the buttons in the column. The default is <see cref="F:System.Windows.Forms.FlatStyle.Standard" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewButtonColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x000810CC File Offset: 0x0007F2CC
		// (set) Token: 0x060019D5 RID: 6613 RVA: 0x000810F8 File Offset: 0x0007F2F8
		[DefaultValue(FlatStyle.Standard)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ButtonColumnFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewButtonCell)this.CellTemplate).FlatStyle;
			}
			set
			{
				if (this.FlatStyle != value)
				{
					((DataGridViewButtonCell)this.CellTemplate).FlatStyle = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewButtonCell dataGridViewButtonCell = dataGridViewRow.Cells[base.Index] as DataGridViewButtonCell;
							if (dataGridViewButtonCell != null)
							{
								dataGridViewButtonCell.FlatStyleInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		/// <summary>Gets or sets the default text displayed on the button cell.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the text. The default is <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">When setting this property, the value of the <see cref="P:System.Windows.Forms.DataGridViewButtonColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x00081183 File Offset: 0x0007F383
		// (set) Token: 0x060019D7 RID: 6615 RVA: 0x0008118C File Offset: 0x0007F38C
		[DefaultValue(null)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ButtonColumnTextDescr")]
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (!string.Equals(value, this.text, StringComparison.Ordinal))
				{
					this.text = value;
					if (base.DataGridView != null)
					{
						if (this.UseColumnTextForButtonValue)
						{
							base.DataGridView.OnColumnCommonChange(base.Index);
							return;
						}
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewButtonCell dataGridViewButtonCell = dataGridViewRow.Cells[base.Index] as DataGridViewButtonCell;
							if (dataGridViewButtonCell != null && dataGridViewButtonCell.UseColumnTextForButtonValue)
							{
								base.DataGridView.OnColumnCommonChange(base.Index);
								return;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="P:System.Windows.Forms.DataGridViewButtonColumn.Text" /> property value is displayed as the button text for cells in this column.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.DataGridViewButtonColumn.Text" /> property value is displayed on buttons in the column; <see langword="false" /> if the <see cref="P:System.Windows.Forms.DataGridViewCell.FormattedValue" /> property value of each cell is displayed on its button. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewButtonColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x00081246 File Offset: 0x0007F446
		// (set) Token: 0x060019D9 RID: 6617 RVA: 0x00081270 File Offset: 0x0007F470
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ButtonColumnUseColumnTextForButtonValueDescr")]
		public bool UseColumnTextForButtonValue
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewButtonCell)this.CellTemplate).UseColumnTextForButtonValue;
			}
			set
			{
				if (this.UseColumnTextForButtonValue != value)
				{
					((DataGridViewButtonCell)this.CellTemplate).UseColumnTextForButtonValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewButtonCell dataGridViewButtonCell = dataGridViewRow.Cells[base.Index] as DataGridViewButtonCell;
							if (dataGridViewButtonCell != null)
							{
								dataGridViewButtonCell.UseColumnTextForButtonValueInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		/// <summary>Creates an exact copy of this column.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewButtonColumn" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewButtonColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x060019DA RID: 6618 RVA: 0x000812FC File Offset: 0x0007F4FC
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewButtonColumn dataGridViewButtonColumn;
			if (type == DataGridViewButtonColumn.columnType)
			{
				dataGridViewButtonColumn = new DataGridViewButtonColumn();
			}
			else
			{
				dataGridViewButtonColumn = (DataGridViewButtonColumn)Activator.CreateInstance(type);
			}
			if (dataGridViewButtonColumn != null)
			{
				base.CloneInternal(dataGridViewButtonColumn);
				dataGridViewButtonColumn.Text = this.text;
			}
			return dataGridViewButtonColumn;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00081348 File Offset: 0x0007F548
		private bool ShouldSerializeDefaultCellStyle()
		{
			if (!base.HasDefaultCellStyle)
			{
				return false;
			}
			DataGridViewCellStyle defaultCellStyle = this.DefaultCellStyle;
			return !defaultCellStyle.BackColor.IsEmpty || !defaultCellStyle.ForeColor.IsEmpty || !defaultCellStyle.SelectionBackColor.IsEmpty || !defaultCellStyle.SelectionForeColor.IsEmpty || defaultCellStyle.Font != null || !defaultCellStyle.IsNullValueDefault || !defaultCellStyle.IsDataSourceNullValueDefault || !string.IsNullOrEmpty(defaultCellStyle.Format) || !defaultCellStyle.FormatProvider.Equals(CultureInfo.CurrentCulture) || defaultCellStyle.Alignment != DataGridViewContentAlignment.MiddleCenter || defaultCellStyle.WrapMode != DataGridViewTriState.NotSet || defaultCellStyle.Tag != null || !defaultCellStyle.Padding.Equals(Padding.Empty);
		}

		/// <summary>Gets a string that describes the column.</summary>
		/// <returns>A <see cref="T:System.String" /> that describes the column.</returns>
		// Token: 0x060019DC RID: 6620 RVA: 0x00081424 File Offset: 0x0007F624
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewButtonColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04000BD5 RID: 3029
		private static Type columnType = typeof(DataGridViewButtonColumn);

		// Token: 0x04000BD6 RID: 3030
		private string text;
	}
}
