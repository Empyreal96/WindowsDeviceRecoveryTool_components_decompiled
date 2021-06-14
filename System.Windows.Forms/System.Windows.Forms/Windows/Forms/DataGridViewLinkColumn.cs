using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Represents a column of cells that contain links in a <see cref="T:System.Windows.Forms.DataGridView" /> control. </summary>
	// Token: 0x020001F5 RID: 501
	[ToolboxBitmap(typeof(DataGridViewLinkColumn), "DataGridViewLinkColumn.bmp")]
	public class DataGridViewLinkColumn : DataGridViewColumn
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewLinkColumn" /> class. </summary>
		// Token: 0x06001E63 RID: 7779 RVA: 0x00097250 File Offset: 0x00095450
		public DataGridViewLinkColumn() : base(new DataGridViewLinkCell())
		{
		}

		/// <summary>Gets or sets the color used to display an active link within cells in the column. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to display a link that is being selected. The default value is the user's Internet Explorer setting for the color of links in the hover state.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x0009725D File Offset: 0x0009545D
		// (set) Token: 0x06001E65 RID: 7781 RVA: 0x00097288 File Offset: 0x00095488
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_LinkColumnActiveLinkColorDescr")]
		public Color ActiveLinkColor
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).ActiveLinkColor;
			}
			set
			{
				if (!this.ActiveLinkColor.Equals(value))
				{
					((DataGridViewLinkCell)this.CellTemplate).ActiveLinkColorInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.ActiveLinkColorInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x00097328 File Offset: 0x00095528
		private bool ShouldSerializeActiveLinkColor()
		{
			if (SystemInformation.HighContrast && AccessibilityImprovements.Level2)
			{
				return !this.ActiveLinkColor.Equals(SystemColors.HotTrack);
			}
			return !this.ActiveLinkColor.Equals(LinkUtilities.IEActiveLinkColor);
		}

		/// <summary>Gets or sets the template used to create new cells.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCell" /> that all other cells in the column are modeled after. The default value is a new <see cref="T:System.Windows.Forms.DataGridViewLinkCell" /> instance.</returns>
		/// <exception cref="T:System.InvalidCastException">When setting this property to a value that is not of type <see cref="T:System.Windows.Forms.DataGridViewLinkCell" />.</exception>
		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x00081081 File Offset: 0x0007F281
		// (set) Token: 0x06001E68 RID: 7784 RVA: 0x00097386 File Offset: 0x00095586
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
				if (value != null && !(value is DataGridViewLinkCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[]
					{
						"System.Windows.Forms.DataGridViewLinkCell"
					}));
				}
				base.CellTemplate = value;
			}
		}

		/// <summary>Gets or sets a value that represents the behavior of links within cells in the column.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.LinkBehavior" /> value indicating the link behavior. The default is <see cref="F:System.Windows.Forms.LinkBehavior.SystemDefault" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x000973B8 File Offset: 0x000955B8
		// (set) Token: 0x06001E6A RID: 7786 RVA: 0x000973E4 File Offset: 0x000955E4
		[DefaultValue(LinkBehavior.SystemDefault)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_LinkColumnLinkBehaviorDescr")]
		public LinkBehavior LinkBehavior
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).LinkBehavior;
			}
			set
			{
				if (!this.LinkBehavior.Equals(value))
				{
					((DataGridViewLinkCell)this.CellTemplate).LinkBehavior = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.LinkBehaviorInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		/// <summary>Gets or sets the color used to display an unselected link within cells in the column.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to initially display a link. The default value is the user's Internet Explorer setting for the link color. </returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x00097484 File Offset: 0x00095684
		// (set) Token: 0x06001E6C RID: 7788 RVA: 0x000974B0 File Offset: 0x000956B0
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_LinkColumnLinkColorDescr")]
		public Color LinkColor
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).LinkColor;
			}
			set
			{
				if (!this.LinkColor.Equals(value))
				{
					((DataGridViewLinkCell)this.CellTemplate).LinkColorInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.LinkColorInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x00097550 File Offset: 0x00095750
		private bool ShouldSerializeLinkColor()
		{
			if (SystemInformation.HighContrast && AccessibilityImprovements.Level2)
			{
				return !this.LinkColor.Equals(SystemColors.HotTrack);
			}
			return !this.LinkColor.Equals(LinkUtilities.IELinkColor);
		}

		/// <summary>Gets or sets the link text displayed in a column's cells if <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.UseColumnTextForLinkValue" /> is <see langword="true" />.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the link text.</returns>
		/// <exception cref="T:System.InvalidOperationException">When setting this property, the value of the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x000975AE File Offset: 0x000957AE
		// (set) Token: 0x06001E6F RID: 7791 RVA: 0x000975B8 File Offset: 0x000957B8
		[DefaultValue(null)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_LinkColumnTextDescr")]
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
						if (this.UseColumnTextForLinkValue)
						{
							base.DataGridView.OnColumnCommonChange(base.Index);
							return;
						}
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null && dataGridViewLinkCell.UseColumnTextForLinkValue)
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

		/// <summary>Gets or sets a value indicating whether the link changes color if it has been visited.</summary>
		/// <returns>
		///     <see langword="true" /> if the link changes color when it is selected; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x00097672 File Offset: 0x00095872
		// (set) Token: 0x06001E71 RID: 7793 RVA: 0x0009769C File Offset: 0x0009589C
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_LinkColumnTrackVisitedStateDescr")]
		public bool TrackVisitedState
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).TrackVisitedState;
			}
			set
			{
				if (this.TrackVisitedState != value)
				{
					((DataGridViewLinkCell)this.CellTemplate).TrackVisitedStateInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.TrackVisitedStateInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.Text" /> property value is displayed as the link text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.Text" /> property value is displayed as the link text; <see langword="false" /> if the cell <see cref="P:System.Windows.Forms.DataGridViewCell.FormattedValue" /> property value is displayed as the link text. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x00097727 File Offset: 0x00095927
		// (set) Token: 0x06001E73 RID: 7795 RVA: 0x00097754 File Offset: 0x00095954
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_LinkColumnUseColumnTextForLinkValueDescr")]
		public bool UseColumnTextForLinkValue
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).UseColumnTextForLinkValue;
			}
			set
			{
				if (this.UseColumnTextForLinkValue != value)
				{
					((DataGridViewLinkCell)this.CellTemplate).UseColumnTextForLinkValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.UseColumnTextForLinkValueInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		/// <summary>Gets or sets the color used to display a link that has been previously visited.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to display a link that has been visited. The default value is the user's Internet Explorer setting for the visited link color. </returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.CellTemplate" /> property is <see langword="null" />.</exception>
		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x000977DF File Offset: 0x000959DF
		// (set) Token: 0x06001E75 RID: 7797 RVA: 0x0009780C File Offset: 0x00095A0C
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_LinkColumnVisitedLinkColorDescr")]
		public Color VisitedLinkColor
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).VisitedLinkColor;
			}
			set
			{
				if (!this.VisitedLinkColor.Equals(value))
				{
					((DataGridViewLinkCell)this.CellTemplate).VisitedLinkColorInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.VisitedLinkColorInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000978AC File Offset: 0x00095AAC
		private bool ShouldSerializeVisitedLinkColor()
		{
			if (SystemInformation.HighContrast && AccessibilityImprovements.Level2)
			{
				return !this.VisitedLinkColor.Equals(SystemColors.HotTrack);
			}
			return !this.VisitedLinkColor.Equals(LinkUtilities.IEVisitedLinkColor);
		}

		/// <summary>Creates an exact copy of this column.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewLinkColumn" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.CellTemplate" /> property is <see langword="null" />. </exception>
		// Token: 0x06001E77 RID: 7799 RVA: 0x0009790C File Offset: 0x00095B0C
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewLinkColumn dataGridViewLinkColumn;
			if (type == DataGridViewLinkColumn.columnType)
			{
				dataGridViewLinkColumn = new DataGridViewLinkColumn();
			}
			else
			{
				dataGridViewLinkColumn = (DataGridViewLinkColumn)Activator.CreateInstance(type);
			}
			if (dataGridViewLinkColumn != null)
			{
				base.CloneInternal(dataGridViewLinkColumn);
				dataGridViewLinkColumn.Text = this.text;
			}
			return dataGridViewLinkColumn;
		}

		/// <summary>Gets a string that describes the column.</summary>
		/// <returns>A <see cref="T:System.String" /> that describes the column.</returns>
		// Token: 0x06001E78 RID: 7800 RVA: 0x00097958 File Offset: 0x00095B58
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewLinkColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04000D5C RID: 3420
		private static Type columnType = typeof(DataGridViewLinkColumn);

		// Token: 0x04000D5D RID: 3421
		private string text;
	}
}
