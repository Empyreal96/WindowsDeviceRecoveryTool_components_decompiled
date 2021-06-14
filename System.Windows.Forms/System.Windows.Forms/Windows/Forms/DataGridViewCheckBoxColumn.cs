using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Hosts a collection of <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" /> objects.</summary>
	// Token: 0x020001AC RID: 428
	[ToolboxBitmap(typeof(DataGridViewCheckBoxColumn), "DataGridViewCheckBoxColumn.bmp")]
	public class DataGridViewCheckBoxColumn : DataGridViewColumn
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxColumn" /> class to the default state.</summary>
		// Token: 0x06001BAF RID: 7087 RVA: 0x0008AB01 File Offset: 0x00088D01
		public DataGridViewCheckBoxColumn() : this(false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCheckBoxColumn" /> and configures it to display check boxes with two or three states. </summary>
		/// <param name="threeState">
		///       <see langword="true" /> to display check boxes with three states; <see langword="false" /> to display check boxes with two states. </param>
		// Token: 0x06001BB0 RID: 7088 RVA: 0x0008AB0C File Offset: 0x00088D0C
		public DataGridViewCheckBoxColumn(bool threeState) : base(new DataGridViewCheckBoxCell(threeState))
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			dataGridViewCellStyle.AlignmentInternal = DataGridViewContentAlignment.MiddleCenter;
			if (threeState)
			{
				dataGridViewCellStyle.NullValue = CheckState.Indeterminate;
			}
			else
			{
				dataGridViewCellStyle.NullValue = false;
			}
			this.DefaultCellStyle = dataGridViewCellStyle;
		}

		/// <summary>Gets or sets the template used to create new cells.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCell" /> that all other cells in the column are modeled after. The default value is a new <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" /> instance.</returns>
		/// <exception cref="T:System.InvalidCastException">The property is set to a value that is not of type <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" />. </exception>
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001BB1 RID: 7089 RVA: 0x00081081 File Offset: 0x0007F281
		// (set) Token: 0x06001BB2 RID: 7090 RVA: 0x0008AB57 File Offset: 0x00088D57
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
				if (value != null && !(value is DataGridViewCheckBoxCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[]
					{
						"System.Windows.Forms.DataGridViewCheckBoxCell"
					}));
				}
				base.CellTemplate = value;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001BB3 RID: 7091 RVA: 0x0008AB89 File Offset: 0x00088D89
		private DataGridViewCheckBoxCell CheckBoxCellTemplate
		{
			get
			{
				return (DataGridViewCheckBoxCell)this.CellTemplate;
			}
		}

		/// <summary>Gets or sets the column's default cell style.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied as the default style.</returns>
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x000810BB File Offset: 0x0007F2BB
		// (set) Token: 0x06001BB5 RID: 7093 RVA: 0x000810C3 File Offset: 0x0007F2C3
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

		/// <summary>Gets or sets the underlying value corresponding to a cell value of <see langword="false" />, which appears as an unchecked box.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing a value that the cells in this column will treat as a <see langword="false" /> value. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCheckBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x0008AB96 File Offset: 0x00088D96
		// (set) Token: 0x06001BB7 RID: 7095 RVA: 0x0008ABBC File Offset: 0x00088DBC
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_CheckBoxColumnFalseValueDescr")]
		[TypeConverter(typeof(StringConverter))]
		public object FalseValue
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.FalseValue;
			}
			set
			{
				if (this.FalseValue != value)
				{
					this.CheckBoxCellTemplate.FalseValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.FalseValueInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		/// <summary>Gets or sets the flat style appearance of the check box cells.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FlatStyle" /> value indicating the appearance of cells in the column. The default is <see cref="F:System.Windows.Forms.FlatStyle.Standard" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCheckBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x0008AC42 File Offset: 0x00088E42
		// (set) Token: 0x06001BB9 RID: 7097 RVA: 0x0008AC68 File Offset: 0x00088E68
		[DefaultValue(FlatStyle.Standard)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_CheckBoxColumnFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.FlatStyle;
			}
			set
			{
				if (this.FlatStyle != value)
				{
					this.CheckBoxCellTemplate.FlatStyle = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.FlatStyleInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		/// <summary>Gets or sets the underlying value corresponding to an indeterminate or <see langword="null" /> cell value, which appears as a disabled checkbox.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing a value that the cells in this column will treat as an indeterminate value. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCheckBoxColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x0008ACEE File Offset: 0x00088EEE
		// (set) Token: 0x06001BBB RID: 7099 RVA: 0x0008AD14 File Offset: 0x00088F14
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_CheckBoxColumnIndeterminateValueDescr")]
		[TypeConverter(typeof(StringConverter))]
		public object IndeterminateValue
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.IndeterminateValue;
			}
			set
			{
				if (this.IndeterminateValue != value)
				{
					this.CheckBoxCellTemplate.IndeterminateValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.IndeterminateValueInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the hosted check box cells will allow three check states rather than two.</summary>
		/// <returns>
		///     <see langword="true" /> if the hosted <see cref="T:System.Windows.Forms.DataGridViewCheckBoxCell" /> objects are able to have a third, indeterminate, state; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCheckBoxColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x0008AD9A File Offset: 0x00088F9A
		// (set) Token: 0x06001BBD RID: 7101 RVA: 0x0008ADC0 File Offset: 0x00088FC0
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_CheckBoxColumnThreeStateDescr")]
		public bool ThreeState
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.ThreeState;
			}
			set
			{
				if (this.ThreeState != value)
				{
					this.CheckBoxCellTemplate.ThreeStateInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.ThreeStateInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
					if (value && this.DefaultCellStyle.NullValue is bool && !(bool)this.DefaultCellStyle.NullValue)
					{
						this.DefaultCellStyle.NullValue = CheckState.Indeterminate;
						return;
					}
					if (!value && this.DefaultCellStyle.NullValue is CheckState && (CheckState)this.DefaultCellStyle.NullValue == CheckState.Indeterminate)
					{
						this.DefaultCellStyle.NullValue = false;
					}
				}
			}
		}

		/// <summary>Gets or sets the underlying value corresponding to a cell value of <see langword="true" />, which appears as a checked box.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing a value that the cell will treat as a <see langword="true" /> value. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCheckBoxColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x0008AEBB File Offset: 0x000890BB
		// (set) Token: 0x06001BBF RID: 7103 RVA: 0x0008AEE0 File Offset: 0x000890E0
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_CheckBoxColumnTrueValueDescr")]
		[TypeConverter(typeof(StringConverter))]
		public object TrueValue
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.TrueValue;
			}
			set
			{
				if (this.TrueValue != value)
				{
					this.CheckBoxCellTemplate.TrueValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.TrueValueInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x0008AF68 File Offset: 0x00089168
		private bool ShouldSerializeDefaultCellStyle()
		{
			DataGridViewCheckBoxCell dataGridViewCheckBoxCell = this.CellTemplate as DataGridViewCheckBoxCell;
			if (dataGridViewCheckBoxCell == null)
			{
				return true;
			}
			object obj;
			if (dataGridViewCheckBoxCell.ThreeState)
			{
				obj = CheckState.Indeterminate;
			}
			else
			{
				obj = false;
			}
			if (!base.HasDefaultCellStyle)
			{
				return false;
			}
			DataGridViewCellStyle defaultCellStyle = this.DefaultCellStyle;
			return !defaultCellStyle.BackColor.IsEmpty || !defaultCellStyle.ForeColor.IsEmpty || !defaultCellStyle.SelectionBackColor.IsEmpty || !defaultCellStyle.SelectionForeColor.IsEmpty || defaultCellStyle.Font != null || !defaultCellStyle.NullValue.Equals(obj) || !defaultCellStyle.IsDataSourceNullValueDefault || !string.IsNullOrEmpty(defaultCellStyle.Format) || !defaultCellStyle.FormatProvider.Equals(CultureInfo.CurrentCulture) || defaultCellStyle.Alignment != DataGridViewContentAlignment.MiddleCenter || defaultCellStyle.WrapMode != DataGridViewTriState.NotSet || defaultCellStyle.Tag != null || !defaultCellStyle.Padding.Equals(Padding.Empty);
		}

		/// <summary>Gets a string that describes the column.</summary>
		/// <returns>A <see cref="T:System.String" /> that describes the column.</returns>
		// Token: 0x06001BC1 RID: 7105 RVA: 0x0008B074 File Offset: 0x00089274
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewCheckBoxColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}
	}
}
