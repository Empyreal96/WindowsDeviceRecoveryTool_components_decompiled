using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Represents a row header of a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001FF RID: 511
	public class DataGridViewRowHeaderCell : DataGridViewHeaderCell
	{
		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001F2C RID: 7980 RVA: 0x0009CD11 File Offset: 0x0009AF11
		private static Bitmap LeftArrowBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.leftArrowBmp == null)
				{
					DataGridViewRowHeaderCell.leftArrowBmp = DataGridViewRowHeaderCell.GetBitmapFromIcon("DataGridViewRow.left.ico");
				}
				return DataGridViewRowHeaderCell.leftArrowBmp;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x0009CD2E File Offset: 0x0009AF2E
		private static Bitmap LeftArrowStarBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.leftArrowStarBmp == null)
				{
					DataGridViewRowHeaderCell.leftArrowStarBmp = DataGridViewRowHeaderCell.GetBitmapFromIcon("DataGridViewRow.leftstar.ico");
				}
				return DataGridViewRowHeaderCell.leftArrowStarBmp;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06001F2E RID: 7982 RVA: 0x0009CD4B File Offset: 0x0009AF4B
		private static Bitmap PencilLTRBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.pencilLTRBmp == null)
				{
					DataGridViewRowHeaderCell.pencilLTRBmp = DataGridViewRowHeaderCell.GetBitmapFromIcon("DataGridViewRow.pencil_ltr.ico");
				}
				return DataGridViewRowHeaderCell.pencilLTRBmp;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x0009CD68 File Offset: 0x0009AF68
		private static Bitmap PencilRTLBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.pencilRTLBmp == null)
				{
					DataGridViewRowHeaderCell.pencilRTLBmp = DataGridViewRowHeaderCell.GetBitmapFromIcon("DataGridViewRow.pencil_rtl.ico");
				}
				return DataGridViewRowHeaderCell.pencilRTLBmp;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x0009CD85 File Offset: 0x0009AF85
		private static Bitmap RightArrowBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.rightArrowBmp == null)
				{
					DataGridViewRowHeaderCell.rightArrowBmp = DataGridViewRowHeaderCell.GetBitmapFromIcon("DataGridViewRow.right.ico");
				}
				return DataGridViewRowHeaderCell.rightArrowBmp;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x0009CDA2 File Offset: 0x0009AFA2
		private static Bitmap RightArrowStarBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.rightArrowStarBmp == null)
				{
					DataGridViewRowHeaderCell.rightArrowStarBmp = DataGridViewRowHeaderCell.GetBitmapFromIcon("DataGridViewRow.rightstar.ico");
				}
				return DataGridViewRowHeaderCell.rightArrowStarBmp;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06001F32 RID: 7986 RVA: 0x0009CDBF File Offset: 0x0009AFBF
		private static Bitmap StarBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.starBmp == null)
				{
					DataGridViewRowHeaderCell.starBmp = DataGridViewRowHeaderCell.GetBitmapFromIcon("DataGridViewRow.star.ico");
				}
				return DataGridViewRowHeaderCell.starBmp;
			}
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewHeaderCell" />.</returns>
		// Token: 0x06001F33 RID: 7987 RVA: 0x0009CDDC File Offset: 0x0009AFDC
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewRowHeaderCell dataGridViewRowHeaderCell;
			if (type == DataGridViewRowHeaderCell.cellType)
			{
				dataGridViewRowHeaderCell = new DataGridViewRowHeaderCell();
			}
			else
			{
				dataGridViewRowHeaderCell = (DataGridViewRowHeaderCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewRowHeaderCell);
			dataGridViewRowHeaderCell.Value = base.Value;
			return dataGridViewRowHeaderCell;
		}

		/// <summary>Creates a new accessible object for the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell" />. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" /> for the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell" />. </returns>
		// Token: 0x06001F34 RID: 7988 RVA: 0x0009CE25 File Offset: 0x0009B025
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject(this);
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0009CE2D File Offset: 0x0009B02D
		private static Bitmap GetArrowBitmap(bool rightToLeft)
		{
			if (!rightToLeft)
			{
				return DataGridViewRowHeaderCell.RightArrowBitmap;
			}
			return DataGridViewRowHeaderCell.LeftArrowBitmap;
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0009CE3D File Offset: 0x0009B03D
		private static Bitmap GetArrowStarBitmap(bool rightToLeft)
		{
			if (!rightToLeft)
			{
				return DataGridViewRowHeaderCell.RightArrowStarBitmap;
			}
			return DataGridViewRowHeaderCell.LeftArrowStarBitmap;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x0009CE50 File Offset: 0x0009B050
		private static Bitmap GetBitmapFromIcon(string iconName)
		{
			Size size = new Size((int)DataGridViewCell.iconsWidth, (int)DataGridViewCell.iconsHeight);
			Icon icon = new Icon(BitmapSelector.GetResourceStream(typeof(DataGridViewRowHeaderCell), iconName), size);
			Bitmap bitmap = icon.ToBitmap();
			icon.Dispose();
			if (DpiHelper.IsScalingRequired && (bitmap.Size.Width != (int)DataGridViewCell.iconsWidth || bitmap.Size.Height != (int)DataGridViewCell.iconsHeight))
			{
				Bitmap bitmap2 = DpiHelper.CreateResizedBitmap(bitmap, size);
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
		/// <returns>A <see cref="T:System.Object" /> that represents the value of the cell to copy to the <see cref="T:System.Windows.Forms.Clipboard" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than zero or greater than or equal to the number of rows in the control.</exception>
		// Token: 0x06001F38 RID: 7992 RVA: 0x0009CEDC File Offset: 0x0009B0DC
		protected override object GetClipboardContent(int rowIndex, bool firstCell, bool lastCell, bool inFirstRow, bool inLastRow, string format)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			object value = this.GetValue(rowIndex);
			StringBuilder stringBuilder = new StringBuilder(64);
			if (string.Equals(format, DataFormats.Html, StringComparison.OrdinalIgnoreCase))
			{
				if (inFirstRow)
				{
					stringBuilder.Append("<TABLE>");
				}
				stringBuilder.Append("<TR>");
				stringBuilder.Append("<TD ALIGN=\"center\">");
				if (value != null)
				{
					stringBuilder.Append("<B>");
					DataGridViewCell.FormatPlainTextAsHtml(value.ToString(), new StringWriter(stringBuilder, CultureInfo.CurrentCulture));
					stringBuilder.Append("</B>");
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

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		// Token: 0x06001F39 RID: 7993 RVA: 0x0009D068 File Offset: 0x0009B268
		protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || base.OwningRow == null)
			{
				return Rectangle.Empty;
			}
			object value = this.GetValue(rowIndex);
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates dataGridViewElementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out dataGridViewElementState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementState, value, null, cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, true, false, false);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's error icon, if one is displayed.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's error icon, if one is displayed; otherwise, <see cref="F:System.Drawing.Rectangle.Empty" />.</returns>
		// Token: 0x06001F3A RID: 7994 RVA: 0x0009D0C8 File Offset: 0x0009B2C8
		protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || !base.DataGridView.ShowRowErrors || string.IsNullOrEmpty(this.GetErrorText(rowIndex)))
			{
				return Rectangle.Empty;
			}
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates dataGridViewElementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out dataGridViewElementState, out rectangle);
			object value = this.GetValue(rowIndex);
			object formattedValue = this.GetFormattedValue(value, rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementState, formattedValue, this.GetErrorText(rowIndex), cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		/// <summary>Returns a string that represents the error for the cell.</summary>
		/// <param name="rowIndex">The row index of the cell.</param>
		/// <returns>A string that describes the error for the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x06001F3B RID: 7995 RVA: 0x0009D152 File Offset: 0x0009B352
		protected internal override string GetErrorText(int rowIndex)
		{
			if (base.OwningRow == null)
			{
				return base.GetErrorText(rowIndex);
			}
			return base.OwningRow.GetErrorText(rowIndex);
		}

		/// <summary>Retrieves the inherited shortcut menu for the specified row.</summary>
		/// <param name="rowIndex">The index of the row to get the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> of. The index must be -1 to indicate the row of column headers.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> of the row if one exists; otherwise, the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> inherited from <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the cell is not <see langword="null" /> and the specified <paramref name="rowIndex" /> is less than 0 or greater than the number of rows in the control minus 1.</exception>
		// Token: 0x06001F3C RID: 7996 RVA: 0x0009D170 File Offset: 0x0009B370
		public override ContextMenuStrip GetInheritedContextMenuStrip(int rowIndex)
		{
			if (base.DataGridView != null && (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count))
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			ContextMenuStrip contextMenuStrip = base.GetContextMenuStrip(rowIndex);
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

		/// <summary>Gets the style applied to the cell.</summary>
		/// <param name="inheritedCellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be populated with the inherited cell style.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <param name="includeColors">
		///       <see langword="true" /> to include inherited colors in the returned cell style; otherwise, <see langword="false" />.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that includes the style settings of the cell inherited from the cell's parent row, column, and <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
		// Token: 0x06001F3D RID: 7997 RVA: 0x0009D1CC File Offset: 0x0009B3CC
		public override DataGridViewCellStyle GetInheritedStyle(DataGridViewCellStyle inheritedCellStyle, int rowIndex, bool includeColors)
		{
			DataGridViewCellStyle dataGridViewCellStyle = (inheritedCellStyle == null) ? new DataGridViewCellStyle() : inheritedCellStyle;
			DataGridViewCellStyle dataGridViewCellStyle2 = null;
			if (base.HasStyle)
			{
				dataGridViewCellStyle2 = base.Style;
			}
			DataGridViewCellStyle rowHeadersDefaultCellStyle = base.DataGridView.RowHeadersDefaultCellStyle;
			DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
			if (includeColors)
			{
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle2.BackColor;
				}
				else if (!rowHeadersDefaultCellStyle.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = rowHeadersDefaultCellStyle.BackColor;
				}
				else
				{
					dataGridViewCellStyle.BackColor = defaultCellStyle.BackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle2.ForeColor;
				}
				else if (!rowHeadersDefaultCellStyle.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = rowHeadersDefaultCellStyle.ForeColor;
				}
				else
				{
					dataGridViewCellStyle.ForeColor = defaultCellStyle.ForeColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle2.SelectionBackColor;
				}
				else if (!rowHeadersDefaultCellStyle.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = rowHeadersDefaultCellStyle.SelectionBackColor;
				}
				else
				{
					dataGridViewCellStyle.SelectionBackColor = defaultCellStyle.SelectionBackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle2.SelectionForeColor;
				}
				else if (!rowHeadersDefaultCellStyle.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = rowHeadersDefaultCellStyle.SelectionForeColor;
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
			else if (rowHeadersDefaultCellStyle.Font != null)
			{
				dataGridViewCellStyle.Font = rowHeadersDefaultCellStyle.Font;
			}
			else
			{
				dataGridViewCellStyle.Font = defaultCellStyle.Font;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle2.NullValue;
			}
			else if (!rowHeadersDefaultCellStyle.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = rowHeadersDefaultCellStyle.NullValue;
			}
			else
			{
				dataGridViewCellStyle.NullValue = defaultCellStyle.NullValue;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle2.DataSourceNullValue;
			}
			else if (!rowHeadersDefaultCellStyle.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = rowHeadersDefaultCellStyle.DataSourceNullValue;
			}
			else
			{
				dataGridViewCellStyle.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle2.Format;
			}
			else if (rowHeadersDefaultCellStyle.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = rowHeadersDefaultCellStyle.Format;
			}
			else
			{
				dataGridViewCellStyle.Format = defaultCellStyle.Format;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle2.FormatProvider;
			}
			else if (!rowHeadersDefaultCellStyle.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = rowHeadersDefaultCellStyle.FormatProvider;
			}
			else
			{
				dataGridViewCellStyle.FormatProvider = defaultCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle2.Alignment;
			}
			else if (rowHeadersDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = rowHeadersDefaultCellStyle.Alignment;
			}
			else
			{
				dataGridViewCellStyle.AlignmentInternal = defaultCellStyle.Alignment;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle2.WrapMode;
			}
			else if (rowHeadersDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = rowHeadersDefaultCellStyle.WrapMode;
			}
			else
			{
				dataGridViewCellStyle.WrapModeInternal = defaultCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle2.Tag;
			}
			else if (rowHeadersDefaultCellStyle.Tag != null)
			{
				dataGridViewCellStyle.Tag = rowHeadersDefaultCellStyle.Tag;
			}
			else
			{
				dataGridViewCellStyle.Tag = defaultCellStyle.Tag;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle2.Padding;
			}
			else if (rowHeadersDefaultCellStyle.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = rowHeadersDefaultCellStyle.Padding;
			}
			else
			{
				dataGridViewCellStyle.PaddingInternal = defaultCellStyle.Padding;
			}
			return dataGridViewCellStyle;
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x0009D57A File Offset: 0x0009B77A
		private static Bitmap GetPencilBitmap(bool rightToLeft)
		{
			if (!rightToLeft)
			{
				return DataGridViewRowHeaderCell.PencilLTRBitmap;
			}
			return DataGridViewRowHeaderCell.PencilRTLBitmap;
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		// Token: 0x06001F3F RID: 7999 RVA: 0x0009D58C File Offset: 0x0009B78C
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
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
			DataGridViewAdvancedBorderStyle advancedBorderStyle = base.OwningRow.AdjustRowHeaderBorderStyle(base.DataGridView.AdvancedRowHeadersBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, false, false, false, false);
			Rectangle rectangle = this.BorderWidths(advancedBorderStyle);
			int num = rectangle.Left + rectangle.Width + cellStyle.Padding.Horizontal;
			int num2 = rectangle.Top + rectangle.Height + cellStyle.Padding.Vertical;
			TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				Rectangle themeMargins = DataGridViewHeaderCell.GetThemeMargins(graphics);
				num += themeMargins.Y;
				num += themeMargins.Height;
				num2 += themeMargins.X;
				num2 += themeMargins.Width;
			}
			object obj = this.GetValue(rowIndex);
			if (!(obj is string))
			{
				obj = null;
			}
			return DataGridViewUtilities.GetPreferredRowHeaderSize(graphics, (string)obj, cellStyle, num, num2, base.DataGridView.ShowRowErrors, true, constraintSize, flags);
		}

		/// <summary>Gets the value of the cell. </summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The value contained in the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the cell is not <see langword="null" /> and <paramref name="rowIndex" /> is less than -1 or greater than or equal to the number of rows in the parent <see cref="T:System.Windows.Forms.DataGridView" />.</exception>
		// Token: 0x06001F40 RID: 8000 RVA: 0x0009D6B7 File Offset: 0x0009B8B7
		protected override object GetValue(int rowIndex)
		{
			if (base.DataGridView != null && (rowIndex < -1 || rowIndex >= base.DataGridView.Rows.Count))
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			return base.Properties.GetObject(DataGridViewCell.PropCellValue);
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell" />.</summary>
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
		// Token: 0x06001F41 RID: 8001 RVA: 0x0009D6F4 File Offset: 0x0009B8F4
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, cellState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0009D72C File Offset: 0x0009B92C
		private Rectangle PaintPrivate(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			Rectangle result = Rectangle.Empty;
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle = cellBounds;
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			rectangle.Offset(rectangle2.X, rectangle2.Y);
			rectangle.Width -= rectangle2.Right;
			rectangle.Height -= rectangle2.Bottom;
			Rectangle destRect = rectangle;
			bool flag = (dataGridViewElementState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
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
				if (destRect.Width > 0 && destRect.Height > 0)
				{
					if (paint && DataGridViewCell.PaintBackground(paintParts))
					{
						int headerState = 1;
						if (base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect)
						{
							if (base.ButtonState != ButtonState.Normal)
							{
								headerState = 3;
							}
							else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == -1)
							{
								headerState = 2;
							}
							else if (flag)
							{
								headerState = 3;
							}
						}
						using (Bitmap bitmap = new Bitmap(destRect.Height, destRect.Width))
						{
							using (Graphics graphics2 = Graphics.FromImage(bitmap))
							{
								DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.DrawHeader(graphics2, new Rectangle(0, 0, destRect.Height, destRect.Width), headerState);
								bitmap.RotateFlip(base.DataGridView.RightToLeftInternal ? RotateFlipType.Rotate90FlipNone : RotateFlipType.Rotate90FlipX);
								graphics.DrawImage(bitmap, destRect, new Rectangle(0, 0, destRect.Width, destRect.Height), GraphicsUnit.Pixel);
							}
						}
					}
					Rectangle themeMargins = DataGridViewHeaderCell.GetThemeMargins(graphics);
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle.X += themeMargins.Height;
					}
					else
					{
						rectangle.X += themeMargins.Y;
					}
					rectangle.Width -= themeMargins.Y + themeMargins.Height;
					rectangle.Height -= themeMargins.X + themeMargins.Width;
					rectangle.Y += themeMargins.X;
				}
			}
			else
			{
				if (rectangle.Width > 0 && rectangle.Height > 0)
				{
					SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
					if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
					{
						graphics.FillRectangle(cachedBrush, rectangle);
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
			Bitmap bitmap2 = null;
			if (rectangle.Width > 0 && rectangle.Height > 0)
			{
				Rectangle cellValueBounds = rectangle;
				string text = formattedValue as string;
				if (!string.IsNullOrEmpty(text))
				{
					if (rectangle.Width >= (int)(DataGridViewCell.iconsWidth + 6) && rectangle.Height >= (int)(DataGridViewCell.iconsHeight + 4))
					{
						if (paint && DataGridViewCell.PaintContentBackground(paintParts))
						{
							if (base.DataGridView.CurrentCellAddress.Y == rowIndex)
							{
								if (base.DataGridView.VirtualMode)
								{
									if (base.DataGridView.IsCurrentRowDirty && base.DataGridView.ShowEditingIcon)
									{
										bitmap2 = DataGridViewRowHeaderCell.GetPencilBitmap(base.DataGridView.RightToLeftInternal);
									}
									else if (base.DataGridView.NewRowIndex == rowIndex)
									{
										bitmap2 = DataGridViewRowHeaderCell.GetArrowStarBitmap(base.DataGridView.RightToLeftInternal);
									}
									else
									{
										bitmap2 = DataGridViewRowHeaderCell.GetArrowBitmap(base.DataGridView.RightToLeftInternal);
									}
								}
								else if (base.DataGridView.IsCurrentCellDirty && base.DataGridView.ShowEditingIcon)
								{
									bitmap2 = DataGridViewRowHeaderCell.GetPencilBitmap(base.DataGridView.RightToLeftInternal);
								}
								else if (base.DataGridView.NewRowIndex == rowIndex)
								{
									bitmap2 = DataGridViewRowHeaderCell.GetArrowStarBitmap(base.DataGridView.RightToLeftInternal);
								}
								else
								{
									bitmap2 = DataGridViewRowHeaderCell.GetArrowBitmap(base.DataGridView.RightToLeftInternal);
								}
							}
							else if (base.DataGridView.NewRowIndex == rowIndex)
							{
								bitmap2 = DataGridViewRowHeaderCell.StarBitmap;
							}
							if (bitmap2 != null)
							{
								Color foreColor;
								if (base.DataGridView.ApplyVisualStylesToHeaderCells)
								{
									foreColor = DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
								}
								else
								{
									foreColor = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
								}
								Bitmap obj = bitmap2;
								lock (obj)
								{
									this.PaintIcon(graphics, bitmap2, rectangle, foreColor);
								}
							}
						}
						if (!base.DataGridView.RightToLeftInternal)
						{
							rectangle.X += (int)(DataGridViewCell.iconsWidth + 6);
						}
						rectangle.Width -= (int)(DataGridViewCell.iconsWidth + 6);
					}
					rectangle.Offset(4, 1);
					rectangle.Width -= 9;
					rectangle.Height -= 2;
					if (rectangle.Width > 0 && rectangle.Height > 0)
					{
						TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
						if (base.DataGridView.ShowRowErrors && rectangle.Width > (int)(DataGridViewCell.iconsWidth + 6))
						{
							Size maxBounds = new Size(rectangle.Width - (int)DataGridViewCell.iconsWidth - 6, rectangle.Height);
							if (DataGridViewCell.TextFitsInBounds(graphics, text, cellStyle.Font, maxBounds, textFormatFlags))
							{
								if (base.DataGridView.RightToLeftInternal)
								{
									rectangle.X += (int)(DataGridViewCell.iconsWidth + 6);
								}
								rectangle.Width -= (int)(DataGridViewCell.iconsWidth + 6);
							}
						}
						if (DataGridViewCell.PaintContentForeground(paintParts))
						{
							if (paint)
							{
								Color foreColor2;
								if (base.DataGridView.ApplyVisualStylesToHeaderCells)
								{
									foreColor2 = DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
								}
								else
								{
									foreColor2 = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
								}
								if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
								{
									textFormatFlags |= TextFormatFlags.EndEllipsis;
								}
								TextRenderer.DrawText(graphics, text, cellStyle.Font, rectangle, foreColor2, textFormatFlags);
							}
							else if (computeContentBounds)
							{
								result = DataGridViewUtilities.GetTextBounds(rectangle, text, textFormatFlags, cellStyle);
							}
						}
					}
					if (cellValueBounds.Width >= (int)(9 + 2 * DataGridViewCell.iconsWidth))
					{
						if (paint && base.DataGridView.ShowRowErrors && DataGridViewCell.PaintErrorIcon(paintParts))
						{
							this.PaintErrorIcon(graphics, clipBounds, cellValueBounds, errorText);
						}
						else if (computeErrorIconBounds && !string.IsNullOrEmpty(errorText))
						{
							result = base.ComputeErrorIconBounds(cellValueBounds);
						}
					}
				}
				else
				{
					if (rectangle.Width >= (int)(DataGridViewCell.iconsWidth + 6) && rectangle.Height >= (int)(DataGridViewCell.iconsHeight + 4) && paint && DataGridViewCell.PaintContentBackground(paintParts))
					{
						if (base.DataGridView.CurrentCellAddress.Y == rowIndex)
						{
							if (base.DataGridView.VirtualMode)
							{
								if (base.DataGridView.IsCurrentRowDirty && base.DataGridView.ShowEditingIcon)
								{
									bitmap2 = DataGridViewRowHeaderCell.GetPencilBitmap(base.DataGridView.RightToLeftInternal);
								}
								else if (base.DataGridView.NewRowIndex == rowIndex)
								{
									bitmap2 = DataGridViewRowHeaderCell.GetArrowStarBitmap(base.DataGridView.RightToLeftInternal);
								}
								else
								{
									bitmap2 = DataGridViewRowHeaderCell.GetArrowBitmap(base.DataGridView.RightToLeftInternal);
								}
							}
							else if (base.DataGridView.IsCurrentCellDirty && base.DataGridView.ShowEditingIcon)
							{
								bitmap2 = DataGridViewRowHeaderCell.GetPencilBitmap(base.DataGridView.RightToLeftInternal);
							}
							else if (base.DataGridView.NewRowIndex == rowIndex)
							{
								bitmap2 = DataGridViewRowHeaderCell.GetArrowStarBitmap(base.DataGridView.RightToLeftInternal);
							}
							else
							{
								bitmap2 = DataGridViewRowHeaderCell.GetArrowBitmap(base.DataGridView.RightToLeftInternal);
							}
						}
						else if (base.DataGridView.NewRowIndex == rowIndex)
						{
							bitmap2 = DataGridViewRowHeaderCell.StarBitmap;
						}
						if (bitmap2 != null)
						{
							Bitmap obj2 = bitmap2;
							lock (obj2)
							{
								Color foreColor3;
								if (base.DataGridView.ApplyVisualStylesToHeaderCells)
								{
									foreColor3 = DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
								}
								else
								{
									foreColor3 = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
								}
								this.PaintIcon(graphics, bitmap2, rectangle, foreColor3);
							}
						}
					}
					if (cellValueBounds.Width >= (int)(9 + 2 * DataGridViewCell.iconsWidth))
					{
						if (paint && base.DataGridView.ShowRowErrors && DataGridViewCell.PaintErrorIcon(paintParts))
						{
							base.PaintErrorIcon(graphics, cellStyle, rowIndex, cellBounds, cellValueBounds, errorText);
						}
						else if (computeErrorIconBounds && !string.IsNullOrEmpty(errorText))
						{
							result = base.ComputeErrorIconBounds(cellValueBounds);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0009E150 File Offset: 0x0009C350
		private void PaintIcon(Graphics g, Bitmap bmp, Rectangle bounds, Color foreColor)
		{
			Rectangle destRect = new Rectangle(base.DataGridView.RightToLeftInternal ? (bounds.Right - 3 - (int)DataGridViewCell.iconsWidth) : (bounds.Left + 3), bounds.Y + (bounds.Height - (int)DataGridViewCell.iconsHeight) / 2, (int)DataGridViewCell.iconsWidth, (int)DataGridViewCell.iconsHeight);
			DataGridViewRowHeaderCell.colorMap[0].NewColor = foreColor;
			DataGridViewRowHeaderCell.colorMap[0].OldColor = Color.Black;
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetRemapTable(DataGridViewRowHeaderCell.colorMap, ColorAdjustType.Bitmap);
			g.DrawImage(bmp, destRect, 0, 0, (int)DataGridViewCell.iconsWidth, (int)DataGridViewCell.iconsHeight, GraphicsUnit.Pixel, imageAttributes);
			imageAttributes.Dispose();
		}

		/// <summary>Sets the value of the cell. </summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <param name="value">The cell value to set. </param>
		/// <returns>
		///     <see langword="true" /> if the value has been set; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001F44 RID: 8004 RVA: 0x0009E1FC File Offset: 0x0009C3FC
		protected override bool SetValue(int rowIndex, object value)
		{
			object value2 = this.GetValue(rowIndex);
			if (value != null || base.Properties.ContainsObject(DataGridViewCell.PropCellValue))
			{
				base.Properties.SetObject(DataGridViewCell.PropCellValue, value);
			}
			if (base.DataGridView != null && value2 != value)
			{
				base.RaiseCellValueChanged(new DataGridViewCellEventArgs(-1, rowIndex));
			}
			return true;
		}

		/// <summary>Returns the string representation of the cell.</summary>
		/// <returns>A string that represents the current cell.</returns>
		// Token: 0x06001F45 RID: 8005 RVA: 0x0009E254 File Offset: 0x0009C454
		public override string ToString()
		{
			return "DataGridViewRowHeaderCell { RowIndex=" + base.RowIndex.ToString(CultureInfo.CurrentCulture) + " }";
		}

		// Token: 0x04000D7E RID: 3454
		private static readonly VisualStyleElement HeaderElement = VisualStyleElement.Header.Item.Normal;

		// Token: 0x04000D7F RID: 3455
		private static ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x04000D80 RID: 3456
		private static Bitmap rightArrowBmp = null;

		// Token: 0x04000D81 RID: 3457
		private static Bitmap leftArrowBmp = null;

		// Token: 0x04000D82 RID: 3458
		private static Bitmap rightArrowStarBmp;

		// Token: 0x04000D83 RID: 3459
		private static Bitmap leftArrowStarBmp;

		// Token: 0x04000D84 RID: 3460
		private static Bitmap pencilLTRBmp = null;

		// Token: 0x04000D85 RID: 3461
		private static Bitmap pencilRTLBmp = null;

		// Token: 0x04000D86 RID: 3462
		private static Bitmap starBmp = null;

		// Token: 0x04000D87 RID: 3463
		private static Type cellType = typeof(DataGridViewRowHeaderCell);

		// Token: 0x04000D88 RID: 3464
		private const byte DATAGRIDVIEWROWHEADERCELL_iconMarginWidth = 3;

		// Token: 0x04000D89 RID: 3465
		private const byte DATAGRIDVIEWROWHEADERCELL_iconMarginHeight = 2;

		// Token: 0x04000D8A RID: 3466
		private const byte DATAGRIDVIEWROWHEADERCELL_contentMarginWidth = 3;

		// Token: 0x04000D8B RID: 3467
		private const byte DATAGRIDVIEWROWHEADERCELL_horizontalTextMarginLeft = 1;

		// Token: 0x04000D8C RID: 3468
		private const byte DATAGRIDVIEWROWHEADERCELL_horizontalTextMarginRight = 2;

		// Token: 0x04000D8D RID: 3469
		private const byte DATAGRIDVIEWROWHEADERCELL_verticalTextMargin = 1;

		// Token: 0x020005BE RID: 1470
		private class DataGridViewRowHeaderCellRenderer
		{
			// Token: 0x060059C7 RID: 22983 RVA: 0x000027DB File Offset: 0x000009DB
			private DataGridViewRowHeaderCellRenderer()
			{
			}

			// Token: 0x170015B9 RID: 5561
			// (get) Token: 0x060059C8 RID: 22984 RVA: 0x0017A17E File Offset: 0x0017837E
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewRowHeaderCell.HeaderElement);
					}
					return DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x060059C9 RID: 22985 RVA: 0x0017A19B File Offset: 0x0017839B
			public static void DrawHeader(Graphics g, Rectangle bounds, int headerState)
			{
				DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.SetParameters(DataGridViewRowHeaderCell.HeaderElement.ClassName, DataGridViewRowHeaderCell.HeaderElement.Part, headerState);
				DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, Rectangle.Truncate(g.ClipBounds));
			}

			// Token: 0x04003949 RID: 14665
			private static VisualStyleRenderer visualStyleRenderer;
		}

		/// <summary>Provides information about a <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell" /> to accessibility client applications.</summary>
		// Token: 0x020005BF RID: 1471
		protected class DataGridViewRowHeaderCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell" /> that owns this accessible object.</param>
			// Token: 0x060059CA RID: 22986 RVA: 0x00176D3A File Offset: 0x00174F3A
			public DataGridViewRowHeaderCellAccessibleObject(DataGridViewRowHeaderCell owner) : base(owner)
			{
			}

			/// <summary>Gets the location and size of the accessible object.</summary>
			/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the accessible object.</returns>
			// Token: 0x170015BA RID: 5562
			// (get) Token: 0x060059CB RID: 22987 RVA: 0x0017A1D4 File Offset: 0x001783D4
			public override Rectangle Bounds
			{
				get
				{
					if (base.Owner.OwningRow == null)
					{
						return Rectangle.Empty;
					}
					Rectangle bounds = this.ParentPrivate.Bounds;
					bounds.Width = base.Owner.DataGridView.RowHeadersWidth;
					return bounds;
				}
			}

			/// <summary>Gets a description of the default action of the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" />.</summary>
			/// <returns>An empty string ("").</returns>
			// Token: 0x170015BB RID: 5563
			// (get) Token: 0x060059CC RID: 22988 RVA: 0x0017A218 File Offset: 0x00178418
			public override string DefaultAction
			{
				get
				{
					if (base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect)
					{
						return SR.GetString("DataGridView_RowHeaderCellAccDefaultAction");
					}
					return string.Empty;
				}
			}

			/// <summary>Gets the name of the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" />.</summary>
			/// <returns>The name of the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" />.</returns>
			// Token: 0x170015BC RID: 5564
			// (get) Token: 0x060059CD RID: 22989 RVA: 0x0017A250 File Offset: 0x00178450
			public override string Name
			{
				get
				{
					if (this.ParentPrivate != null)
					{
						return this.ParentPrivate.Name;
					}
					return string.Empty;
				}
			}

			/// <summary>Gets the parent of the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" />.</summary>
			/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewRow.DataGridViewRowAccessibleObject" /> that belongs to the <see cref="T:System.Windows.Forms.DataGridViewRow" /> of the current <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" />.</returns>
			// Token: 0x170015BD RID: 5565
			// (get) Token: 0x060059CE RID: 22990 RVA: 0x0017A26B File Offset: 0x0017846B
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.ParentPrivate;
				}
			}

			// Token: 0x170015BE RID: 5566
			// (get) Token: 0x060059CF RID: 22991 RVA: 0x0017A273 File Offset: 0x00178473
			private AccessibleObject ParentPrivate
			{
				get
				{
					if (base.Owner.OwningRow == null)
					{
						return null;
					}
					return base.Owner.OwningRow.AccessibilityObject;
				}
			}

			/// <summary>Gets the role of the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" />.</summary>
			/// <returns>The <see cref="F:System.Windows.Forms.AccessibleRole.RowHeader" /> value.</returns>
			// Token: 0x170015BF RID: 5567
			// (get) Token: 0x060059D0 RID: 22992 RVA: 0x0017A294 File Offset: 0x00178494
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.RowHeader;
				}
			}

			/// <summary>Gets the state of the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" />.</summary>
			/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.AccessibleStates" /> values. </returns>
			// Token: 0x170015C0 RID: 5568
			// (get) Token: 0x060059D1 RID: 22993 RVA: 0x0017A298 File Offset: 0x00178498
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
					if ((base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect) && base.Owner.OwningRow != null && base.Owner.OwningRow.Selected)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			/// <summary>Gets the value of the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" />.</summary>
			/// <returns>An empty string ("").</returns>
			// Token: 0x170015C1 RID: 5569
			// (get) Token: 0x060059D2 RID: 22994 RVA: 0x00179039 File Offset: 0x00177239
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return string.Empty;
				}
			}

			/// <summary>Performs the default action of the <see cref="T:System.Windows.Forms.DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject" />.</summary>
			// Token: 0x060059D3 RID: 22995 RVA: 0x0017A314 File Offset: 0x00178514
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				if ((base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect) && base.Owner.OwningRow != null)
				{
					base.Owner.OwningRow.Selected = true;
				}
			}

			/// <summary>Navigates to another accessible object.</summary>
			/// <param name="navigationDirection">One of the <see cref="T:System.Windows.Forms.AccessibleNavigation" /> values.</param>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents an object in the specified direction.</returns>
			// Token: 0x060059D4 RID: 22996 RVA: 0x0017A368 File Offset: 0x00178568
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				switch (navigationDirection)
				{
				case AccessibleNavigation.Up:
					if (base.Owner.OwningRow == null)
					{
						return null;
					}
					if (base.Owner.OwningRow.Index == base.Owner.DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Visible))
					{
						if (base.Owner.DataGridView.ColumnHeadersVisible)
						{
							return base.Owner.DataGridView.AccessibilityObject.GetChild(0).GetChild(0);
						}
						return null;
					}
					else
					{
						int previousRow = base.Owner.DataGridView.Rows.GetPreviousRow(base.Owner.OwningRow.Index, DataGridViewElementStates.Visible);
						int rowCount = base.Owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, previousRow);
						if (base.Owner.DataGridView.ColumnHeadersVisible)
						{
							return base.Owner.DataGridView.AccessibilityObject.GetChild(rowCount + 1).GetChild(0);
						}
						return base.Owner.DataGridView.AccessibilityObject.GetChild(rowCount).GetChild(0);
					}
					break;
				case AccessibleNavigation.Down:
				{
					if (base.Owner.OwningRow == null)
					{
						return null;
					}
					if (base.Owner.OwningRow.Index == base.Owner.DataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible))
					{
						return null;
					}
					int nextRow = base.Owner.DataGridView.Rows.GetNextRow(base.Owner.OwningRow.Index, DataGridViewElementStates.Visible);
					int rowCount2 = base.Owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, nextRow);
					if (base.Owner.DataGridView.ColumnHeadersVisible)
					{
						return base.Owner.DataGridView.AccessibilityObject.GetChild(1 + rowCount2).GetChild(0);
					}
					return base.Owner.DataGridView.AccessibilityObject.GetChild(rowCount2).GetChild(0);
				}
				case AccessibleNavigation.Next:
					if (base.Owner.OwningRow != null && base.Owner.DataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible) > 0)
					{
						return this.ParentPrivate.GetChild(1);
					}
					return null;
				case AccessibleNavigation.Previous:
					return null;
				}
				return null;
			}

			/// <summary>Modifies the row selection depending on the selection mode.</summary>
			/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.AccessibleSelection" /> values.</param>
			/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property value is <see langword="null" />.</exception>
			// Token: 0x060059D5 RID: 22997 RVA: 0x0017A598 File Offset: 0x00178798
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (base.Owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				DataGridViewRowHeaderCell dataGridViewRowHeaderCell = (DataGridViewRowHeaderCell)base.Owner;
				DataGridView dataGridView = dataGridViewRowHeaderCell.DataGridView;
				if (dataGridView == null)
				{
					return;
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					dataGridView.FocusInternal();
				}
				if (dataGridViewRowHeaderCell.OwningRow != null && (dataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || dataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect))
				{
					if ((flags & (AccessibleSelection.TakeSelection | AccessibleSelection.AddSelection)) != AccessibleSelection.None)
					{
						dataGridViewRowHeaderCell.OwningRow.Selected = true;
						return;
					}
					if ((flags & AccessibleSelection.RemoveSelection) == AccessibleSelection.RemoveSelection)
					{
						dataGridViewRowHeaderCell.OwningRow.Selected = false;
					}
				}
			}

			// Token: 0x060059D6 RID: 22998 RVA: 0x0017A624 File Offset: 0x00178824
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (base.Owner.OwningRow == null)
				{
					return null;
				}
				switch (direction)
				{
				case UnsafeNativeMethods.NavigateDirection.Parent:
					return base.Owner.OwningRow.AccessibilityObject;
				case UnsafeNativeMethods.NavigateDirection.NextSibling:
					if (base.Owner.DataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible) > 0)
					{
						return base.Owner.OwningRow.AccessibilityObject.GetChild(1);
					}
					return null;
				}
				return null;
			}

			// Token: 0x060059D7 RID: 22999 RVA: 0x0017A69C File Offset: 0x0017889C
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
