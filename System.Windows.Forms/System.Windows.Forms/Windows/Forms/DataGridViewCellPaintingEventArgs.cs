using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellPainting" /> event. </summary>
	// Token: 0x0200019F RID: 415
	public class DataGridViewCellPaintingEventArgs : HandledEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellPaintingEventArgs" /> class. </summary>
		/// <param name="dataGridView">The <see cref="T:System.Windows.Forms.DataGridView" /> that contains the cell to be painted.</param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
		/// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the bounds of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being painted.</param>
		/// <param name="rowIndex">The row index of the cell that is being painted.</param>
		/// <param name="columnIndex">The column index of the cell that is being painted.</param>
		/// <param name="cellState">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the cell.</param>
		/// <param name="value">The data of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being painted.</param>
		/// <param name="formattedValue">The formatted data of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being painted.</param>
		/// <param name="errorText">An error message that is associated with the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the cell.</param>
		/// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that contains border styles for the cell that is being painted.</param>
		/// <param name="paintParts">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values specifying the parts to paint.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridView" /> is <see langword="null" />.-or-
		///         <paramref name="graphics" /> is <see langword="null" />.-or-
		///         <paramref name="cellStyle" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="paintParts" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values.</exception>
		// Token: 0x06001B05 RID: 6917 RVA: 0x00087148 File Offset: 0x00085348
		public DataGridViewCellPaintingEventArgs(DataGridView dataGridView, Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, int columnIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (dataGridView == null)
			{
				throw new ArgumentNullException("dataGridView");
			}
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if ((paintParts & ~DataGridViewPaintParts.All) != DataGridViewPaintParts.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewPaintPartsCombination", new object[]
				{
					"paintParts"
				}));
			}
			this.graphics = graphics;
			this.clipBounds = clipBounds;
			this.cellBounds = cellBounds;
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.cellState = cellState;
			this.value = value;
			this.formattedValue = formattedValue;
			this.errorText = errorText;
			this.cellStyle = cellStyle;
			this.advancedBorderStyle = advancedBorderStyle;
			this.paintParts = paintParts;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x00087209 File Offset: 0x00085409
		internal DataGridViewCellPaintingEventArgs(DataGridView dataGridView)
		{
			this.dataGridView = dataGridView;
		}

		/// <summary>Gets the border style of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that represents the border style of the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x00087218 File Offset: 0x00085418
		public DataGridViewAdvancedBorderStyle AdvancedBorderStyle
		{
			get
			{
				return this.advancedBorderStyle;
			}
		}

		/// <summary>Get the bounds of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x00087220 File Offset: 0x00085420
		public Rectangle CellBounds
		{
			get
			{
				return this.cellBounds;
			}
		}

		/// <summary>Gets the cell style of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains the cell style of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x00087228 File Offset: 0x00085428
		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.cellStyle;
			}
		}

		/// <summary>Gets the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</returns>
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x00087230 File Offset: 0x00085430
		public Rectangle ClipBounds
		{
			get
			{
				return this.clipBounds;
			}
		}

		/// <summary>Gets the column index of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>The column index of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001B0B RID: 6923 RVA: 0x00087238 File Offset: 0x00085438
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets a string that represents an error message for the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>A string that represents an error message for the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001B0C RID: 6924 RVA: 0x00087240 File Offset: 0x00085440
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		/// <summary>Gets the formatted value of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>The formatted value of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x00087248 File Offset: 0x00085448
		public object FormattedValue
		{
			get
			{
				return this.formattedValue;
			}
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> used to paint the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> used to paint the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001B0E RID: 6926 RVA: 0x00087250 File Offset: 0x00085450
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>The cell parts that are to be painted.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values specifying the parts to be painted.</returns>
		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x00087258 File Offset: 0x00085458
		public DataGridViewPaintParts PaintParts
		{
			get
			{
				return this.paintParts;
			}
		}

		/// <summary>Gets the row index of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>The row index of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001B10 RID: 6928 RVA: 0x00087260 File Offset: 0x00085460
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		/// <summary>Gets the state of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the cell.</returns>
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x00087268 File Offset: 0x00085468
		public DataGridViewElementStates State
		{
			get
			{
				return this.cellState;
			}
		}

		/// <summary>Gets the value of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <returns>The value of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x00087270 File Offset: 0x00085470
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		/// <summary>Paints the specified parts of the cell for the area in the specified bounds.</summary>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area of the <see cref="T:System.Windows.Forms.DataGridView" /> to be painted.</param>
		/// <param name="paintParts">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values specifying the parts to paint.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCellPaintingEventArgs.RowIndex" /> is less than -1 or greater than or equal to the number of rows in the <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-
		///         <see cref="P:System.Windows.Forms.DataGridViewCellPaintingEventArgs.ColumnIndex" /> is less than -1 or greater than or equal to the number of columns in the <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		// Token: 0x06001B13 RID: 6931 RVA: 0x00087278 File Offset: 0x00085478
		public void Paint(Rectangle clipBounds, DataGridViewPaintParts paintParts)
		{
			if (this.rowIndex < -1 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			if (this.columnIndex < -1 || this.columnIndex >= this.dataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange"));
			}
			this.dataGridView.GetCellInternal(this.columnIndex, this.rowIndex).PaintInternal(this.graphics, clipBounds, this.cellBounds, this.rowIndex, this.cellState, this.value, this.formattedValue, this.errorText, this.cellStyle, this.advancedBorderStyle, paintParts);
		}

		/// <summary>Paints the cell background for the area in the specified bounds.</summary>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area of the <see cref="T:System.Windows.Forms.DataGridView" /> to be painted.</param>
		/// <param name="cellsPaintSelectionBackground">
		///       <see langword="true" /> to paint the background of the specified bounds with the color of the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.SelectionBackColor" /> property of the <see cref="P:System.Windows.Forms.DataGridViewCell.InheritedStyle" />; <see langword="false" /> to paint the background of the specified bounds with the color of the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.BackColor" /> property of the <see cref="P:System.Windows.Forms.DataGridViewCell.InheritedStyle" />.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCellPaintingEventArgs.RowIndex" /> is less than -1 or greater than or equal to the number of rows in the <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-
		///         <see cref="P:System.Windows.Forms.DataGridViewCellPaintingEventArgs.ColumnIndex" /> is less than -1 or greater than or equal to the number of columns in the <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		// Token: 0x06001B14 RID: 6932 RVA: 0x0008733C File Offset: 0x0008553C
		public void PaintBackground(Rectangle clipBounds, bool cellsPaintSelectionBackground)
		{
			if (this.rowIndex < -1 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			if (this.columnIndex < -1 || this.columnIndex >= this.dataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange"));
			}
			DataGridViewPaintParts dataGridViewPaintParts = DataGridViewPaintParts.Background | DataGridViewPaintParts.Border;
			if (cellsPaintSelectionBackground)
			{
				dataGridViewPaintParts |= DataGridViewPaintParts.SelectionBackground;
			}
			this.dataGridView.GetCellInternal(this.columnIndex, this.rowIndex).PaintInternal(this.graphics, clipBounds, this.cellBounds, this.rowIndex, this.cellState, this.value, this.formattedValue, this.errorText, this.cellStyle, this.advancedBorderStyle, dataGridViewPaintParts);
		}

		/// <summary>Paints the cell content for the area in the specified bounds.</summary>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area of the <see cref="T:System.Windows.Forms.DataGridView" /> to be painted.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewCellPaintingEventArgs.RowIndex" /> is less than -1 or greater than or equal to the number of rows in the <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-
		///         <see cref="P:System.Windows.Forms.DataGridViewCellPaintingEventArgs.ColumnIndex" /> is less than -1 or greater than or equal to the number of columns in the <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		// Token: 0x06001B15 RID: 6933 RVA: 0x0008740C File Offset: 0x0008560C
		public void PaintContent(Rectangle clipBounds)
		{
			if (this.rowIndex < -1 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			if (this.columnIndex < -1 || this.columnIndex >= this.dataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange"));
			}
			this.dataGridView.GetCellInternal(this.columnIndex, this.rowIndex).PaintInternal(this.graphics, clipBounds, this.cellBounds, this.rowIndex, this.cellState, this.value, this.formattedValue, this.errorText, this.cellStyle, this.advancedBorderStyle, DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ErrorIcon);
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000874D0 File Offset: 0x000856D0
		internal void SetProperties(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, int columnIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			this.graphics = graphics;
			this.clipBounds = clipBounds;
			this.cellBounds = cellBounds;
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.cellState = cellState;
			this.value = value;
			this.formattedValue = formattedValue;
			this.errorText = errorText;
			this.cellStyle = cellStyle;
			this.advancedBorderStyle = advancedBorderStyle;
			this.paintParts = paintParts;
			base.Handled = false;
		}

		// Token: 0x04000C1A RID: 3098
		private DataGridView dataGridView;

		// Token: 0x04000C1B RID: 3099
		private Graphics graphics;

		// Token: 0x04000C1C RID: 3100
		private Rectangle clipBounds;

		// Token: 0x04000C1D RID: 3101
		private Rectangle cellBounds;

		// Token: 0x04000C1E RID: 3102
		private int rowIndex;

		// Token: 0x04000C1F RID: 3103
		private int columnIndex;

		// Token: 0x04000C20 RID: 3104
		private DataGridViewElementStates cellState;

		// Token: 0x04000C21 RID: 3105
		private object value;

		// Token: 0x04000C22 RID: 3106
		private object formattedValue;

		// Token: 0x04000C23 RID: 3107
		private string errorText;

		// Token: 0x04000C24 RID: 3108
		private DataGridViewCellStyle cellStyle;

		// Token: 0x04000C25 RID: 3109
		private DataGridViewAdvancedBorderStyle advancedBorderStyle;

		// Token: 0x04000C26 RID: 3110
		private DataGridViewPaintParts paintParts;
	}
}
