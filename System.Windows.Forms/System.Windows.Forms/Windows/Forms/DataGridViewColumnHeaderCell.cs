using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Represents a column header in a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001B4 RID: 436
	public class DataGridViewColumnHeaderCell : DataGridViewHeaderCell
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" /> class.</summary>
		// Token: 0x06001C5D RID: 7261 RVA: 0x0008D824 File Offset: 0x0008BA24
		public DataGridViewColumnHeaderCell()
		{
			if (!DataGridViewColumnHeaderCell.isScalingInitialized)
			{
				if (DpiHelper.IsScalingRequired)
				{
					DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth = (byte)DpiHelper.LogicalToDeviceUnitsX(2);
					DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin = (byte)DpiHelper.LogicalToDeviceUnitsX(4);
					DataGridViewColumnHeaderCell.sortGlyphWidth = (byte)DpiHelper.LogicalToDeviceUnitsX(9);
					if (DataGridViewColumnHeaderCell.sortGlyphWidth % 2 == 0)
					{
						DataGridViewColumnHeaderCell.sortGlyphWidth += 1;
					}
					DataGridViewColumnHeaderCell.sortGlyphHeight = (byte)DpiHelper.LogicalToDeviceUnitsY(7);
				}
				DataGridViewColumnHeaderCell.isScalingInitialized = true;
			}
			this.sortGlyphDirection = SortOrder.None;
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001C5E RID: 7262 RVA: 0x0008D899 File Offset: 0x0008BA99
		internal bool ContainsLocalValue
		{
			get
			{
				return base.Properties.ContainsObject(DataGridViewCell.PropCellValue);
			}
		}

		/// <summary>Gets or sets a value indicating which sort glyph is displayed.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.SortOrder" /> value representing the current glyph. The default is <see cref="F:System.Windows.Forms.SortOrder.None" />. </returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.SortOrder" /> value.</exception>
		/// <exception cref="T:System.InvalidOperationException">When setting this property, the value of either the <see cref="P:System.Windows.Forms.DataGridViewCell.OwningColumn" /> property or the <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the cell is <see langword="null" />.-or-When changing the value of this property, the specified value is not <see cref="F:System.Windows.Forms.SortOrder.None" /> and the value of the <see cref="P:System.Windows.Forms.DataGridViewColumn.SortMode" /> property of the owning column is <see cref="F:System.Windows.Forms.DataGridViewColumnSortMode.NotSortable" />.</exception>
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001C5F RID: 7263 RVA: 0x0008D8AB File Offset: 0x0008BAAB
		// (set) Token: 0x06001C60 RID: 7264 RVA: 0x0008D8B4 File Offset: 0x0008BAB4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SortOrder SortGlyphDirection
		{
			get
			{
				return this.sortGlyphDirection;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SortOrder));
				}
				if (base.OwningColumn == null || base.DataGridView == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_CellDoesNotYetBelongToDataGridView"));
				}
				if (value != this.sortGlyphDirection)
				{
					if (base.OwningColumn.SortMode == DataGridViewColumnSortMode.NotSortable && value != SortOrder.None)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewColumnHeaderCell_SortModeAndSortGlyphDirectionClash", new object[]
						{
							value.ToString()
						}));
					}
					this.sortGlyphDirection = value;
					base.DataGridView.OnSortGlyphDirectionChanged(this);
				}
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (set) Token: 0x06001C61 RID: 7265 RVA: 0x0008D959 File Offset: 0x0008BB59
		internal SortOrder SortGlyphDirectionInternal
		{
			set
			{
				this.sortGlyphDirection = value;
			}
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewHeaderCell" />.</returns>
		// Token: 0x06001C62 RID: 7266 RVA: 0x0008D964 File Offset: 0x0008BB64
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewColumnHeaderCell dataGridViewColumnHeaderCell;
			if (type == DataGridViewColumnHeaderCell.cellType)
			{
				dataGridViewColumnHeaderCell = new DataGridViewColumnHeaderCell();
			}
			else
			{
				dataGridViewColumnHeaderCell = (DataGridViewColumnHeaderCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewColumnHeaderCell);
			dataGridViewColumnHeaderCell.Value = base.Value;
			return dataGridViewColumnHeaderCell;
		}

		/// <summary>Creates a new accessible object for the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" />. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" />. </returns>
		// Token: 0x06001C63 RID: 7267 RVA: 0x0008D9AD File Offset: 0x0008BBAD
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject(this);
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
		/// <returns>A <see cref="T:System.Object" /> that represents the value of the cell to copy to the <see cref="T:System.Windows.Forms.Clipboard" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is not -1.</exception>
		// Token: 0x06001C64 RID: 7268 RVA: 0x0008D9B8 File Offset: 0x0008BBB8
		protected override object GetClipboardContent(int rowIndex, bool firstCell, bool lastCell, bool inFirstRow, bool inLastRow, string format)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return null;
			}
			object value = this.GetValue(rowIndex);
			StringBuilder stringBuilder = new StringBuilder(64);
			if (string.Equals(format, DataFormats.Html, StringComparison.OrdinalIgnoreCase))
			{
				if (firstCell)
				{
					stringBuilder.Append("<TABLE>");
					stringBuilder.Append("<THEAD>");
				}
				stringBuilder.Append("<TH>");
				if (value != null)
				{
					DataGridViewCell.FormatPlainTextAsHtml(value.ToString(), new StringWriter(stringBuilder, CultureInfo.CurrentCulture));
				}
				else
				{
					stringBuilder.Append("&nbsp;");
				}
				stringBuilder.Append("</TH>");
				if (lastCell)
				{
					stringBuilder.Append("</THEAD>");
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
				if (value != null)
				{
					bool flag2 = false;
					int length = stringBuilder.Length;
					DataGridViewCell.FormatPlainText(value.ToString(), flag, new StringWriter(stringBuilder, CultureInfo.CurrentCulture), ref flag2);
					if (flag2)
					{
						stringBuilder.Insert(length, '"');
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

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> object and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is not -1.</exception>
		// Token: 0x06001C65 RID: 7269 RVA: 0x0008DB18 File Offset: 0x0008BD18
		protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null || base.OwningColumn == null)
			{
				return Rectangle.Empty;
			}
			object value = this.GetValue(rowIndex);
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates dataGridViewElementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out dataGridViewElementState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementState, value, cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, false);
		}

		/// <summary>Retrieves the inherited shortcut menu for the specified row.</summary>
		/// <param name="rowIndex">The index of the row to get the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> of. The index must be -1 to indicate the row of column headers.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> of the column headers if one exists; otherwise, the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> inherited from <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is not -1.</exception>
		// Token: 0x06001C66 RID: 7270 RVA: 0x0008DB84 File Offset: 0x0008BD84
		public override ContextMenuStrip GetInheritedContextMenuStrip(int rowIndex)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			ContextMenuStrip contextMenuStrip = base.GetContextMenuStrip(-1);
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

		/// <summary>Gets the style applied to the cell. </summary>
		/// <param name="inheritedCellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be populated with the inherited cell style. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="includeColors">
		///       <see langword="true" /> to include inherited colors in the returned cell style; otherwise, <see langword="false" />. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that includes the style settings of the cell inherited from the cell's parent row, column, and <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is not -1.</exception>
		// Token: 0x06001C67 RID: 7271 RVA: 0x0008DBC4 File Offset: 0x0008BDC4
		public override DataGridViewCellStyle GetInheritedStyle(DataGridViewCellStyle inheritedCellStyle, int rowIndex, bool includeColors)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			DataGridViewCellStyle dataGridViewCellStyle = (inheritedCellStyle == null) ? new DataGridViewCellStyle() : inheritedCellStyle;
			DataGridViewCellStyle dataGridViewCellStyle2 = null;
			if (base.HasStyle)
			{
				dataGridViewCellStyle2 = base.Style;
			}
			DataGridViewCellStyle columnHeadersDefaultCellStyle = base.DataGridView.ColumnHeadersDefaultCellStyle;
			DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
			if (includeColors)
			{
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle2.BackColor;
				}
				else if (!columnHeadersDefaultCellStyle.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = columnHeadersDefaultCellStyle.BackColor;
				}
				else
				{
					dataGridViewCellStyle.BackColor = defaultCellStyle.BackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle2.ForeColor;
				}
				else if (!columnHeadersDefaultCellStyle.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = columnHeadersDefaultCellStyle.ForeColor;
				}
				else
				{
					dataGridViewCellStyle.ForeColor = defaultCellStyle.ForeColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle2.SelectionBackColor;
				}
				else if (!columnHeadersDefaultCellStyle.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = columnHeadersDefaultCellStyle.SelectionBackColor;
				}
				else
				{
					dataGridViewCellStyle.SelectionBackColor = defaultCellStyle.SelectionBackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle2.SelectionForeColor;
				}
				else if (!columnHeadersDefaultCellStyle.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = columnHeadersDefaultCellStyle.SelectionForeColor;
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
			else if (columnHeadersDefaultCellStyle.Font != null)
			{
				dataGridViewCellStyle.Font = columnHeadersDefaultCellStyle.Font;
			}
			else
			{
				dataGridViewCellStyle.Font = defaultCellStyle.Font;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle2.NullValue;
			}
			else if (!columnHeadersDefaultCellStyle.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = columnHeadersDefaultCellStyle.NullValue;
			}
			else
			{
				dataGridViewCellStyle.NullValue = defaultCellStyle.NullValue;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle2.DataSourceNullValue;
			}
			else if (!columnHeadersDefaultCellStyle.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = columnHeadersDefaultCellStyle.DataSourceNullValue;
			}
			else
			{
				dataGridViewCellStyle.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle2.Format;
			}
			else if (columnHeadersDefaultCellStyle.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = columnHeadersDefaultCellStyle.Format;
			}
			else
			{
				dataGridViewCellStyle.Format = defaultCellStyle.Format;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle2.FormatProvider;
			}
			else if (!columnHeadersDefaultCellStyle.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = columnHeadersDefaultCellStyle.FormatProvider;
			}
			else
			{
				dataGridViewCellStyle.FormatProvider = defaultCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle2.Alignment;
			}
			else if (columnHeadersDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = columnHeadersDefaultCellStyle.Alignment;
			}
			else
			{
				dataGridViewCellStyle.AlignmentInternal = defaultCellStyle.Alignment;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle2.WrapMode;
			}
			else if (columnHeadersDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = columnHeadersDefaultCellStyle.WrapMode;
			}
			else
			{
				dataGridViewCellStyle.WrapModeInternal = defaultCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle2.Tag;
			}
			else if (columnHeadersDefaultCellStyle.Tag != null)
			{
				dataGridViewCellStyle.Tag = columnHeadersDefaultCellStyle.Tag;
			}
			else
			{
				dataGridViewCellStyle.Tag = defaultCellStyle.Tag;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle2.Padding;
			}
			else if (columnHeadersDefaultCellStyle.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = columnHeadersDefaultCellStyle.Padding;
			}
			else
			{
				dataGridViewCellStyle.PaddingInternal = defaultCellStyle.Padding;
			}
			return dataGridViewCellStyle;
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is not -1.</exception>
		// Token: 0x06001C68 RID: 7272 RVA: 0x0008DF84 File Offset: 0x0008C184
		protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			DataGridViewFreeDimension freeDimensionFromConstraint = DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize);
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
			DataGridViewAdvancedBorderStyle advancedBorderStyle = base.DataGridView.AdjustColumnHeaderBorderStyle(base.DataGridView.AdvancedColumnHeadersBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, false, false);
			Rectangle rectangle = this.BorderWidths(advancedBorderStyle);
			int num = rectangle.Left + rectangle.Width + cellStyle.Padding.Horizontal;
			int num2 = rectangle.Top + rectangle.Height + cellStyle.Padding.Vertical;
			TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			string text = this.GetValue(rowIndex) as string;
			Size result;
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
				{
					result = new Size(0, 0);
					if (!string.IsNullOrEmpty(text))
					{
						if (cellStyle.WrapMode == DataGridViewTriState.True)
						{
							result = new Size(DataGridViewCell.MeasureTextWidth(graphics, text, cellStyle.Font, Math.Max(1, constraintSize.Height - num2 - 2), flags), 0);
						}
						else
						{
							result = new Size(DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Width, 0);
						}
					}
					if (constraintSize.Height - num2 - 2 > (int)DataGridViewColumnHeaderCell.sortGlyphHeight && base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
					{
						result.Width += (int)(DataGridViewColumnHeaderCell.sortGlyphWidth + 2 * DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin);
						if (!string.IsNullOrEmpty(text))
						{
							result.Width += (int)DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth;
						}
					}
					result.Width = Math.Max(result.Width, 1);
				}
				else
				{
					if (!string.IsNullOrEmpty(text))
					{
						if (cellStyle.WrapMode == DataGridViewTriState.True)
						{
							result = DataGridViewCell.MeasureTextPreferredSize(graphics, text, cellStyle.Font, 5f, flags);
						}
						else
						{
							result = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags);
						}
					}
					else
					{
						result = new Size(0, 0);
					}
					if (base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
					{
						result.Width += (int)(DataGridViewColumnHeaderCell.sortGlyphWidth + 2 * DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin);
						if (!string.IsNullOrEmpty(text))
						{
							result.Width += (int)DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth;
						}
						result.Height = Math.Max(result.Height, (int)DataGridViewColumnHeaderCell.sortGlyphHeight);
					}
					result.Width = Math.Max(result.Width, 1);
					result.Height = Math.Max(result.Height, 1);
				}
			}
			else
			{
				int num3 = constraintSize.Width - num;
				result = new Size(0, 0);
				Size empty;
				if (num3 >= (int)(DataGridViewColumnHeaderCell.sortGlyphWidth + 2 * DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin) && base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
				{
					empty = new Size((int)(DataGridViewColumnHeaderCell.sortGlyphWidth + 2 * DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin), (int)DataGridViewColumnHeaderCell.sortGlyphHeight);
				}
				else
				{
					empty = Size.Empty;
				}
				if (num3 - 2 - 2 > 0 && !string.IsNullOrEmpty(text))
				{
					if (cellStyle.WrapMode == DataGridViewTriState.True)
					{
						if (empty.Width > 0 && num3 - 2 - 2 - (int)DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth - empty.Width > 0)
						{
							result = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, num3 - 2 - 2 - (int)DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth - empty.Width, flags));
						}
						else
						{
							result = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, num3 - 2 - 2, flags));
						}
					}
					else
					{
						result = new Size(0, DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Height);
					}
				}
				result.Height = Math.Max(result.Height, empty.Height);
				result.Height = Math.Max(result.Height, 1);
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				if (!string.IsNullOrEmpty(text))
				{
					result.Width += 4;
				}
				result.Width += num;
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				result.Height += 2 + num2;
			}
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				Rectangle themeMargins = DataGridViewHeaderCell.GetThemeMargins(graphics);
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
				{
					result.Width += themeMargins.X + themeMargins.Width;
				}
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
				{
					result.Height += themeMargins.Y + themeMargins.Height;
				}
			}
			return result;
		}

		/// <summary>Gets the value of the cell. </summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The value contained in the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is not -1.</exception>
		// Token: 0x06001C69 RID: 7273 RVA: 0x0008E3FA File Offset: 0x0008C5FA
		protected override object GetValue(int rowIndex)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (this.ContainsLocalValue)
			{
				return base.Properties.GetObject(DataGridViewCell.PropCellValue);
			}
			if (base.OwningColumn != null)
			{
				return base.OwningColumn.Name;
			}
			return null;
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" />.</summary>
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
		// Token: 0x06001C6A RID: 7274 RVA: 0x0008E43C File Offset: 0x0008C63C
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, formattedValue, cellStyle, advancedBorderStyle, paintParts, true);
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x0008E470 File Offset: 0x0008C670
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object formattedValue, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool paint)
		{
			Rectangle result = Rectangle.Empty;
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle = cellBounds;
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			rectangle.Offset(rectangle2.X, rectangle2.Y);
			rectangle.Width -= rectangle2.Right;
			rectangle.Height -= rectangle2.Bottom;
			Rectangle rectangle3 = rectangle;
			bool flag = (dataGridViewElementState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				if (cellStyle.Padding != Padding.Empty && cellStyle.Padding != Padding.Empty)
				{
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
					}
					else
					{
						rectangle.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
					}
					rectangle.Width -= cellStyle.Padding.Horizontal;
					rectangle.Height -= cellStyle.Padding.Vertical;
				}
				if (paint && DataGridViewCell.PaintBackground(paintParts) && rectangle3.Width > 0 && rectangle3.Height > 0)
				{
					int headerState = 1;
					if ((base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable) || base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect)
					{
						if (base.ButtonState != ButtonState.Normal)
						{
							headerState = 3;
						}
						else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex)
						{
							headerState = 2;
						}
						else if (flag)
						{
							headerState = 3;
						}
					}
					if (this.IsHighlighted())
					{
						headerState = 3;
					}
					if (base.DataGridView.RightToLeftInternal)
					{
						Bitmap bitmap = base.FlipXPThemesBitmap;
						if (bitmap == null || bitmap.Width < rectangle3.Width || bitmap.Width > 2 * rectangle3.Width || bitmap.Height < rectangle3.Height || bitmap.Height > 2 * rectangle3.Height)
						{
							bitmap = (base.FlipXPThemesBitmap = new Bitmap(rectangle3.Width, rectangle3.Height));
						}
						Graphics g2 = Graphics.FromImage(bitmap);
						DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.DrawHeader(g2, new Rectangle(0, 0, rectangle3.Width, rectangle3.Height), headerState);
						bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
						g.DrawImage(bitmap, rectangle3, new Rectangle(bitmap.Width - rectangle3.Width, 0, rectangle3.Width, rectangle3.Height), GraphicsUnit.Pixel);
					}
					else
					{
						DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.DrawHeader(g, rectangle3, headerState);
					}
				}
				Rectangle themeMargins = DataGridViewHeaderCell.GetThemeMargins(g);
				rectangle.Y += themeMargins.Y;
				rectangle.Height -= themeMargins.Y + themeMargins.Height;
				if (base.DataGridView.RightToLeftInternal)
				{
					rectangle.X += themeMargins.Width;
					rectangle.Width -= themeMargins.X + themeMargins.Width;
				}
				else
				{
					rectangle.X += themeMargins.X;
					rectangle.Width -= themeMargins.X + themeMargins.Width;
				}
			}
			else
			{
				if (paint && DataGridViewCell.PaintBackground(paintParts) && rectangle3.Width > 0 && rectangle3.Height > 0)
				{
					SolidBrush cachedBrush = base.DataGridView.GetCachedBrush(((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) || this.IsHighlighted()) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
					if (cachedBrush.Color.A == 255)
					{
						g.FillRectangle(cachedBrush, rectangle3);
					}
				}
				if (cellStyle.Padding != Padding.Empty)
				{
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
					}
					else
					{
						rectangle.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
					}
					rectangle.Width -= cellStyle.Padding.Horizontal;
					rectangle.Height -= cellStyle.Padding.Vertical;
				}
			}
			bool flag2 = false;
			Point point = new Point(0, 0);
			string text = formattedValue as string;
			rectangle.Y++;
			rectangle.Height -= 2;
			if (rectangle.Width - 2 - 2 > 0 && rectangle.Height > 0 && !string.IsNullOrEmpty(text))
			{
				rectangle.Offset(2, 0);
				rectangle.Width -= 4;
				Color foreColor;
				if (base.DataGridView.ApplyVisualStylesToHeaderCells)
				{
					foreColor = DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
				}
				else
				{
					foreColor = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
				}
				if (base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
				{
					int num = rectangle.Width - (int)DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth - (int)DataGridViewColumnHeaderCell.sortGlyphWidth - (int)(2 * DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin);
					if (num > 0)
					{
						bool flag3;
						int preferredTextHeight = DataGridViewCell.GetPreferredTextHeight(g, base.DataGridView.RightToLeftInternal, text, cellStyle, num, out flag3);
						if (preferredTextHeight <= rectangle.Height && !flag3)
						{
							flag2 = (this.SortGlyphDirection > SortOrder.None);
							rectangle.Width -= (int)(DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth + DataGridViewColumnHeaderCell.sortGlyphWidth + 2 * DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin);
							if (base.DataGridView.RightToLeftInternal)
							{
								rectangle.X += (int)(DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth + DataGridViewColumnHeaderCell.sortGlyphWidth + 2 * DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin);
								point = new Point(rectangle.Left - 2 - (int)DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth - (int)DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin - (int)DataGridViewColumnHeaderCell.sortGlyphWidth, rectangle.Top + (rectangle.Height - (int)DataGridViewColumnHeaderCell.sortGlyphHeight) / 2);
							}
							else
							{
								point = new Point(rectangle.Right + 2 + (int)DataGridViewColumnHeaderCell.sortGlyphSeparatorWidth + (int)DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin, rectangle.Top + (rectangle.Height - (int)DataGridViewColumnHeaderCell.sortGlyphHeight) / 2);
							}
						}
					}
				}
				TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
				if (paint)
				{
					if (DataGridViewCell.PaintContentForeground(paintParts))
					{
						if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
						{
							textFormatFlags |= TextFormatFlags.EndEllipsis;
						}
						TextRenderer.DrawText(g, text, cellStyle.Font, rectangle, foreColor, textFormatFlags);
					}
				}
				else
				{
					result = DataGridViewUtilities.GetTextBounds(rectangle, text, textFormatFlags, cellStyle);
				}
			}
			else if (paint && this.SortGlyphDirection != SortOrder.None && rectangle.Width >= (int)(DataGridViewColumnHeaderCell.sortGlyphWidth + 2 * DataGridViewColumnHeaderCell.sortGlyphHorizontalMargin) && rectangle.Height >= (int)DataGridViewColumnHeaderCell.sortGlyphHeight)
			{
				flag2 = true;
				point = new Point(rectangle.Left + (rectangle.Width - (int)DataGridViewColumnHeaderCell.sortGlyphWidth) / 2, rectangle.Top + (rectangle.Height - (int)DataGridViewColumnHeaderCell.sortGlyphHeight) / 2);
			}
			if (paint && flag2 && DataGridViewCell.PaintContentBackground(paintParts))
			{
				Pen pen = null;
				Pen pen2 = null;
				base.GetContrastedPens(cellStyle.BackColor, ref pen, ref pen2);
				if (this.SortGlyphDirection == SortOrder.Ascending)
				{
					DataGridViewAdvancedCellBorderStyle right = advancedBorderStyle.Right;
					if (right != DataGridViewAdvancedCellBorderStyle.Inset)
					{
						if (right - DataGridViewAdvancedCellBorderStyle.Outset <= 2)
						{
							g.DrawLine(pen, point.X, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 2, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) - 1, point.Y);
							g.DrawLine(pen, point.X + 1, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 2, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) - 1, point.Y);
							g.DrawLine(pen2, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 2, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 2);
							g.DrawLine(pen2, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 3, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 2);
							g.DrawLine(pen2, point.X, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 2, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1);
						}
						else
						{
							for (int i = 0; i < (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2); i++)
							{
								g.DrawLine(pen, point.X + i, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - i - 1, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - i - 1, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - i - 1);
							}
							g.DrawLine(pen, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) - 1, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2));
						}
					}
					else
					{
						g.DrawLine(pen2, point.X, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 2, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) - 1, point.Y);
						g.DrawLine(pen2, point.X + 1, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 2, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) - 1, point.Y);
						g.DrawLine(pen, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 2, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 2);
						g.DrawLine(pen, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 3, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 2);
						g.DrawLine(pen, point.X, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 2, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1);
					}
				}
				else
				{
					DataGridViewAdvancedCellBorderStyle right = advancedBorderStyle.Right;
					if (right != DataGridViewAdvancedCellBorderStyle.Inset)
					{
						if (right - DataGridViewAdvancedCellBorderStyle.Outset <= 2)
						{
							g.DrawLine(pen, point.X, point.Y + 1, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) - 1, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1);
							g.DrawLine(pen, point.X + 1, point.Y + 1, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) - 1, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1);
							g.DrawLine(pen2, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 2, point.Y + 1);
							g.DrawLine(pen2, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 3, point.Y + 1);
							g.DrawLine(pen2, point.X, point.Y, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 2, point.Y);
						}
						else
						{
							for (int j = 0; j < (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2); j++)
							{
								g.DrawLine(pen, point.X + j, point.Y + j + 2, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - j - 1, point.Y + j + 2);
							}
							g.DrawLine(pen, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) + 1, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) + 2);
						}
					}
					else
					{
						g.DrawLine(pen2, point.X, point.Y + 1, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) - 1, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1);
						g.DrawLine(pen2, point.X + 1, point.Y + 1, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2) - 1, point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1);
						g.DrawLine(pen, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 2, point.Y + 1);
						g.DrawLine(pen, point.X + (int)(DataGridViewColumnHeaderCell.sortGlyphWidth / 2), point.Y + (int)DataGridViewColumnHeaderCell.sortGlyphHeight - 1, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 3, point.Y + 1);
						g.DrawLine(pen, point.X, point.Y, point.X + (int)DataGridViewColumnHeaderCell.sortGlyphWidth - 2, point.Y);
					}
				}
			}
			return result;
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x0008F23C File Offset: 0x0008D43C
		private bool IsHighlighted()
		{
			return base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect && base.DataGridView.CurrentCell != null && base.DataGridView.CurrentCell.Selected && base.DataGridView.CurrentCell.OwningColumn == base.OwningColumn && AccessibilityImprovements.Level2;
		}

		/// <summary>Sets the value of the cell. </summary>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="value">The cell value to set. </param>
		/// <returns>
		///     <see langword="true" /> if the value has been set; otherwise <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is not -1.</exception>
		// Token: 0x06001C6D RID: 7277 RVA: 0x0008F298 File Offset: 0x0008D498
		protected override bool SetValue(int rowIndex, object value)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			object value2 = this.GetValue(rowIndex);
			base.Properties.SetObject(DataGridViewCell.PropCellValue, value);
			if (base.DataGridView != null && value2 != value)
			{
				base.RaiseCellValueChanged(new DataGridViewCellEventArgs(base.ColumnIndex, -1));
			}
			return true;
		}

		/// <summary>Returns the string representation of the cell.</summary>
		/// <returns>A string that represents the current cell.</returns>
		// Token: 0x06001C6E RID: 7278 RVA: 0x0008F2EC File Offset: 0x0008D4EC
		public override string ToString()
		{
			return "DataGridViewColumnHeaderCell { ColumnIndex=" + base.ColumnIndex.ToString(CultureInfo.CurrentCulture) + " }";
		}

		// Token: 0x04000C98 RID: 3224
		private static readonly VisualStyleElement HeaderElement = VisualStyleElement.Header.Item.Normal;

		// Token: 0x04000C99 RID: 3225
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_sortGlyphSeparatorWidth = 2;

		// Token: 0x04000C9A RID: 3226
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_sortGlyphHorizontalMargin = 4;

		// Token: 0x04000C9B RID: 3227
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_sortGlyphWidth = 9;

		// Token: 0x04000C9C RID: 3228
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_sortGlyphHeight = 7;

		// Token: 0x04000C9D RID: 3229
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_horizontalTextMarginLeft = 2;

		// Token: 0x04000C9E RID: 3230
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_horizontalTextMarginRight = 2;

		// Token: 0x04000C9F RID: 3231
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_verticalMargin = 1;

		// Token: 0x04000CA0 RID: 3232
		private static bool isScalingInitialized = false;

		// Token: 0x04000CA1 RID: 3233
		private static byte sortGlyphSeparatorWidth = 2;

		// Token: 0x04000CA2 RID: 3234
		private static byte sortGlyphHorizontalMargin = 4;

		// Token: 0x04000CA3 RID: 3235
		private static byte sortGlyphWidth = 9;

		// Token: 0x04000CA4 RID: 3236
		private static byte sortGlyphHeight = 7;

		// Token: 0x04000CA5 RID: 3237
		private static Type cellType = typeof(DataGridViewColumnHeaderCell);

		// Token: 0x04000CA6 RID: 3238
		private SortOrder sortGlyphDirection;

		// Token: 0x020005B0 RID: 1456
		private class DataGridViewColumnHeaderCellRenderer
		{
			// Token: 0x0600594A RID: 22858 RVA: 0x000027DB File Offset: 0x000009DB
			private DataGridViewColumnHeaderCellRenderer()
			{
			}

			// Token: 0x17001590 RID: 5520
			// (get) Token: 0x0600594B RID: 22859 RVA: 0x001781FC File Offset: 0x001763FC
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewColumnHeaderCell.HeaderElement);
					}
					return DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x0600594C RID: 22860 RVA: 0x0017821C File Offset: 0x0017641C
			public static void DrawHeader(Graphics g, Rectangle bounds, int headerState)
			{
				Rectangle rectangle = Rectangle.Truncate(g.ClipBounds);
				if (2 == headerState)
				{
					DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.SetParameters(DataGridViewColumnHeaderCell.HeaderElement);
					Rectangle clipRectangle = new Rectangle(bounds.Left, bounds.Bottom - 2, 2, 2);
					clipRectangle.Intersect(rectangle);
					DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, clipRectangle);
					clipRectangle = new Rectangle(bounds.Right - 2, bounds.Bottom - 2, 2, 2);
					clipRectangle.Intersect(rectangle);
					DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, clipRectangle);
				}
				DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.SetParameters(DataGridViewColumnHeaderCell.HeaderElement.ClassName, DataGridViewColumnHeaderCell.HeaderElement.Part, headerState);
				DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, rectangle);
			}

			// Token: 0x0400392F RID: 14639
			private static VisualStyleRenderer visualStyleRenderer;
		}

		/// <summary>Provides information about a <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" /> to accessibility client applications.</summary>
		// Token: 0x020005B1 RID: 1457
		protected class DataGridViewColumnHeaderCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" />.</param>
			// Token: 0x0600594D RID: 22861 RVA: 0x00176D3A File Offset: 0x00174F3A
			public DataGridViewColumnHeaderCellAccessibleObject(DataGridViewColumnHeaderCell owner) : base(owner)
			{
			}

			/// <summary>Gets the location and size of the accessible object.</summary>
			/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the accessible object.</returns>
			// Token: 0x17001591 RID: 5521
			// (get) Token: 0x0600594E RID: 22862 RVA: 0x001782D2 File Offset: 0x001764D2
			public override Rectangle Bounds
			{
				get
				{
					return base.GetAccessibleObjectBounds(this.ParentPrivate);
				}
			}

			/// <summary>Gets a string that describes the default action of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" />.</summary>
			/// <returns>A string that describes the default action of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" /></returns>
			// Token: 0x17001592 RID: 5522
			// (get) Token: 0x0600594F RID: 22863 RVA: 0x001782E0 File Offset: 0x001764E0
			public override string DefaultAction
			{
				get
				{
					if (base.Owner.OwningColumn == null)
					{
						return string.Empty;
					}
					if (base.Owner.OwningColumn.SortMode == DataGridViewColumnSortMode.Automatic)
					{
						return SR.GetString("DataGridView_AccColumnHeaderCellDefaultAction");
					}
					if (base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect)
					{
						return SR.GetString("DataGridView_AccColumnHeaderCellSelectDefaultAction");
					}
					return string.Empty;
				}
			}

			/// <summary>Gets the name of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" />.</summary>
			/// <returns>The name of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" />.</returns>
			// Token: 0x17001593 RID: 5523
			// (get) Token: 0x06005950 RID: 22864 RVA: 0x00178354 File Offset: 0x00176554
			public override string Name
			{
				get
				{
					if (base.Owner.OwningColumn != null)
					{
						return base.Owner.OwningColumn.HeaderText;
					}
					return string.Empty;
				}
			}

			/// <summary>Gets the parent of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</summary>
			/// <returns>The parent of the <see cref="T:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject" />.</returns>
			// Token: 0x17001594 RID: 5524
			// (get) Token: 0x06005951 RID: 22865 RVA: 0x00178379 File Offset: 0x00176579
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.ParentPrivate;
				}
			}

			// Token: 0x17001595 RID: 5525
			// (get) Token: 0x06005952 RID: 22866 RVA: 0x00178381 File Offset: 0x00176581
			private AccessibleObject ParentPrivate
			{
				get
				{
					return base.Owner.DataGridView.AccessibilityObject.GetChild(0);
				}
			}

			/// <summary>Gets the role of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" />.</summary>
			/// <returns>The <see cref="F:System.Windows.Forms.AccessibleRole.RowHeader" /> value.</returns>
			// Token: 0x17001596 RID: 5526
			// (get) Token: 0x06005953 RID: 22867 RVA: 0x001725B8 File Offset: 0x001707B8
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.ColumnHeader;
				}
			}

			/// <summary>Gets the state of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" />.</summary>
			/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.AccessibleStates" /> values. The default is <see cref="F:System.Windows.Forms.AccessibleStates.Selectable" />.</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x17001597 RID: 5527
			// (get) Token: 0x06005954 RID: 22868 RVA: 0x0017839C File Offset: 0x0017659C
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Selectable;
					AccessibleStates state = base.State;
					if ((state & AccessibleStates.Offscreen) == AccessibleStates.Offscreen)
					{
						accessibleStates |= AccessibleStates.Offscreen;
					}
					if ((base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect) && base.Owner.OwningColumn != null && base.Owner.OwningColumn.Selected)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			/// <summary>Gets the value of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" />.</summary>
			/// <returns>The value of the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" />.</returns>
			// Token: 0x17001598 RID: 5528
			// (get) Token: 0x06005955 RID: 22869 RVA: 0x0000E334 File Offset: 0x0000C534
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.Name;
				}
			}

			/// <summary>Performs the default action associated with the <see cref="T:System.Windows.Forms.DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject" />.</summary>
			// Token: 0x06005956 RID: 22870 RVA: 0x00178418 File Offset: 0x00176618
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				DataGridViewColumnHeaderCell dataGridViewColumnHeaderCell = (DataGridViewColumnHeaderCell)base.Owner;
				DataGridView dataGridView = dataGridViewColumnHeaderCell.DataGridView;
				if (dataGridViewColumnHeaderCell.OwningColumn != null)
				{
					if (dataGridViewColumnHeaderCell.OwningColumn.SortMode == DataGridViewColumnSortMode.Automatic)
					{
						ListSortDirection direction = ListSortDirection.Ascending;
						if (dataGridView.SortedColumn == dataGridViewColumnHeaderCell.OwningColumn && dataGridView.SortOrder == SortOrder.Ascending)
						{
							direction = ListSortDirection.Descending;
						}
						dataGridView.Sort(dataGridViewColumnHeaderCell.OwningColumn, direction);
						return;
					}
					if (dataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || dataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect)
					{
						dataGridViewColumnHeaderCell.OwningColumn.Selected = true;
					}
				}
			}

			/// <summary>Navigates to another accessible object.</summary>
			/// <param name="navigationDirection">One of the <see cref="T:System.Windows.Forms.AccessibleNavigation" /> values.</param>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents an object in the specified direction.</returns>
			// Token: 0x06005957 RID: 22871 RVA: 0x00178498 File Offset: 0x00176698
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				if (base.Owner.OwningColumn == null)
				{
					return null;
				}
				switch (navigationDirection)
				{
				case AccessibleNavigation.Left:
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateBackward();
					}
					return this.NavigateForward();
				case AccessibleNavigation.Right:
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateForward();
					}
					return this.NavigateBackward();
				case AccessibleNavigation.Next:
					return this.NavigateForward();
				case AccessibleNavigation.Previous:
					return this.NavigateBackward();
				case AccessibleNavigation.FirstChild:
					return base.Owner.DataGridView.AccessibilityObject.GetChild(0).GetChild(0);
				case AccessibleNavigation.LastChild:
				{
					AccessibleObject child = base.Owner.DataGridView.AccessibilityObject.GetChild(0);
					return child.GetChild(child.GetChildCount() - 1);
				}
				default:
					return null;
				}
			}

			// Token: 0x06005958 RID: 22872 RVA: 0x0017856C File Offset: 0x0017676C
			private AccessibleObject NavigateBackward()
			{
				if (base.Owner.OwningColumn == base.Owner.DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible))
				{
					if (base.Owner.DataGridView.RowHeadersVisible)
					{
						return this.Parent.GetChild(0);
					}
					return null;
				}
				else
				{
					int index = base.Owner.DataGridView.Columns.GetPreviousColumn(base.Owner.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None).Index;
					int num = base.Owner.DataGridView.Columns.ColumnIndexToActualDisplayIndex(index, DataGridViewElementStates.Visible);
					if (base.Owner.DataGridView.RowHeadersVisible)
					{
						return this.Parent.GetChild(num + 1);
					}
					return this.Parent.GetChild(num);
				}
			}

			// Token: 0x06005959 RID: 22873 RVA: 0x00178630 File Offset: 0x00176830
			private AccessibleObject NavigateForward()
			{
				if (base.Owner.OwningColumn == base.Owner.DataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None))
				{
					return null;
				}
				int index = base.Owner.DataGridView.Columns.GetNextColumn(base.Owner.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None).Index;
				int num = base.Owner.DataGridView.Columns.ColumnIndexToActualDisplayIndex(index, DataGridViewElementStates.Visible);
				if (base.Owner.DataGridView.RowHeadersVisible)
				{
					return this.Parent.GetChild(num + 1);
				}
				return this.Parent.GetChild(num);
			}

			/// <summary>Modifies the column selection depending on the selection mode.</summary>
			/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.AccessibleSelection" /> values. </param>
			// Token: 0x0600595A RID: 22874 RVA: 0x001786D4 File Offset: 0x001768D4
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (base.Owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				DataGridViewColumnHeaderCell dataGridViewColumnHeaderCell = (DataGridViewColumnHeaderCell)base.Owner;
				DataGridView dataGridView = dataGridViewColumnHeaderCell.DataGridView;
				if (dataGridView == null)
				{
					return;
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					dataGridView.FocusInternal();
				}
				if (dataGridViewColumnHeaderCell.OwningColumn != null && (dataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || dataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect))
				{
					if ((flags & (AccessibleSelection.TakeSelection | AccessibleSelection.AddSelection)) != AccessibleSelection.None)
					{
						dataGridViewColumnHeaderCell.OwningColumn.Selected = true;
						return;
					}
					if ((flags & AccessibleSelection.RemoveSelection) == AccessibleSelection.RemoveSelection)
					{
						dataGridViewColumnHeaderCell.OwningColumn.Selected = false;
					}
				}
			}

			// Token: 0x0600595B RID: 22875 RVA: 0x0017875E File Offset: 0x0017695E
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (base.Owner.OwningColumn == null)
				{
					return null;
				}
				switch (direction)
				{
				case UnsafeNativeMethods.NavigateDirection.Parent:
					return this.Parent;
				case UnsafeNativeMethods.NavigateDirection.NextSibling:
					return this.NavigateForward();
				case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
					return this.NavigateBackward();
				default:
					return null;
				}
			}

			// Token: 0x0600595C RID: 22876 RVA: 0x00178799 File Offset: 0x00176999
			internal override bool IsPatternSupported(int patternId)
			{
				return patternId.Equals(10018) || patternId.Equals(10000);
			}

			// Token: 0x0600595D RID: 22877 RVA: 0x001787B8 File Offset: 0x001769B8
			internal override object GetPropertyValue(int propertyId)
			{
				if (AccessibilityImprovements.Level3)
				{
					switch (propertyId)
					{
					case 30003:
						return 50034;
					case 30004:
					case 30006:
					case 30011:
					case 30012:
						goto IL_CB;
					case 30005:
						return this.Name;
					case 30007:
						return string.Empty;
					case 30008:
						break;
					case 30009:
						return (this.State & AccessibleStates.Focusable) == AccessibleStates.Focusable;
					case 30010:
						return base.Owner.DataGridView.Enabled;
					case 30013:
						return this.Help ?? string.Empty;
					default:
						if (propertyId != 30019)
						{
							if (propertyId != 30022)
							{
								goto IL_CB;
							}
							return (this.State & AccessibleStates.Offscreen) == AccessibleStates.Offscreen;
						}
						break;
					}
					return false;
				}
				IL_CB:
				return base.GetPropertyValue(propertyId);
			}
		}
	}
}
