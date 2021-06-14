using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Hosts a collection of <see cref="T:System.Windows.Forms.DataGridViewTextBoxCell" /> cells.</summary>
	// Token: 0x0200020D RID: 525
	[ToolboxBitmap(typeof(DataGridViewTextBoxColumn), "DataGridViewTextBoxColumn.bmp")]
	public class DataGridViewTextBoxColumn : DataGridViewColumn
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewTextBoxColumn" /> class to the default state.</summary>
		// Token: 0x06001FEE RID: 8174 RVA: 0x0009FF22 File Offset: 0x0009E122
		public DataGridViewTextBoxColumn() : base(new DataGridViewTextBoxCell())
		{
			this.SortMode = DataGridViewColumnSortMode.Automatic;
		}

		/// <summary>Gets or sets the template used to model cell appearance.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCell" /> that all other cells in the column are modeled after.</returns>
		/// <exception cref="T:System.InvalidCastException">The set type is not compatible with type <see cref="T:System.Windows.Forms.DataGridViewTextBoxCell" />. </exception>
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x00081081 File Offset: 0x0007F281
		// (set) Token: 0x06001FF0 RID: 8176 RVA: 0x0009FF36 File Offset: 0x0009E136
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
				if (value != null && !(value is DataGridViewTextBoxCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[]
					{
						"System.Windows.Forms.DataGridViewTextBoxCell"
					}));
				}
				base.CellTemplate = value;
			}
		}

		/// <summary>Gets or sets the maximum number of characters that can be entered into the text box.</summary>
		/// <returns>The maximum number of characters that can be entered into the text box; the default value is 32767.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewTextBoxColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x0009FF68 File Offset: 0x0009E168
		// (set) Token: 0x06001FF2 RID: 8178 RVA: 0x0009FF90 File Offset: 0x0009E190
		[DefaultValue(32767)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_TextBoxColumnMaxInputLengthDescr")]
		public int MaxInputLength
		{
			get
			{
				if (this.TextBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.TextBoxCellTemplate.MaxInputLength;
			}
			set
			{
				if (this.MaxInputLength != value)
				{
					this.TextBoxCellTemplate.MaxInputLength = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewTextBoxCell dataGridViewTextBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewTextBoxCell;
							if (dataGridViewTextBoxCell != null)
							{
								dataGridViewTextBoxCell.MaxInputLength = value;
							}
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the sort mode for the column.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewColumnSortMode" /> that specifies the criteria used to order the rows based on the cell values in a column.</returns>
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x000A0005 File Offset: 0x0009E205
		// (set) Token: 0x06001FF4 RID: 8180 RVA: 0x000A000D File Offset: 0x0009E20D
		[DefaultValue(DataGridViewColumnSortMode.Automatic)]
		public new DataGridViewColumnSortMode SortMode
		{
			get
			{
				return base.SortMode;
			}
			set
			{
				base.SortMode = value;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x000A0016 File Offset: 0x0009E216
		private DataGridViewTextBoxCell TextBoxCellTemplate
		{
			get
			{
				return (DataGridViewTextBoxCell)this.CellTemplate;
			}
		}

		/// <summary>Gets a string that describes the column.</summary>
		/// <returns>A <see cref="T:System.String" /> that describes the column.</returns>
		// Token: 0x06001FF6 RID: 8182 RVA: 0x000A0024 File Offset: 0x0009E224
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewTextBoxColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04000DCF RID: 3535
		private const int DATAGRIDVIEWTEXTBOXCOLUMN_maxInputLength = 32767;
	}
}
