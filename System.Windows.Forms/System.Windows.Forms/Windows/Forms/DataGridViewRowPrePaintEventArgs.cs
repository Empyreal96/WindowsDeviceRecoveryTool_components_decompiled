using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.RowPrePaint" /> event. </summary>
	// Token: 0x02000203 RID: 515
	public class DataGridViewRowPrePaintEventArgs : HandledEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowPrePaintEventArgs" /> class. </summary>
		/// <param name="dataGridView">The <see cref="T:System.Windows.Forms.DataGridView" /> that owns the row that is being painted.</param>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the <see cref="T:System.Windows.Forms.DataGridViewRow" />.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be painted.</param>
		/// <param name="rowBounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the bounds of the <see cref="T:System.Windows.Forms.DataGridViewRow" /> that is being painted.</param>
		/// <param name="rowIndex">The row index of the cell that is being painted.</param>
		/// <param name="rowState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the row.</param>
		/// <param name="errorText">An error message that is associated with the row.</param>
		/// <param name="inheritedRowStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the row.</param>
		/// <param name="isFirstDisplayedRow">
		///       <see langword="true" /> to indicate whether the current row is the first row currently displayed in the <see cref="T:System.Windows.Forms.DataGridView" />; otherwise, <see langword="false" />.</param>
		/// <param name="isLastVisibleRow">
		///       <see langword="true" /> to indicate whether the current row is the last row in the <see cref="T:System.Windows.Forms.DataGridView" /> that has the <see cref="P:System.Windows.Forms.DataGridViewRow.Visible" /> property set to <see langword="true" />; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridView" /> is <see langword="null" />.-or-
		///         <paramref name="graphics" /> is <see langword="null" />.-or-
		///         <paramref name="inheritedRowStyle" /> is <see langword="null" />.</exception>
		// Token: 0x06001F65 RID: 8037 RVA: 0x0009E818 File Offset: 0x0009CA18
		public DataGridViewRowPrePaintEventArgs(DataGridView dataGridView, Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, string errorText, DataGridViewCellStyle inheritedRowStyle, bool isFirstDisplayedRow, bool isLastVisibleRow)
		{
			if (dataGridView == null)
			{
				throw new ArgumentNullException("dataGridView");
			}
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (inheritedRowStyle == null)
			{
				throw new ArgumentNullException("inheritedRowStyle");
			}
			this.dataGridView = dataGridView;
			this.graphics = graphics;
			this.clipBounds = clipBounds;
			this.rowBounds = rowBounds;
			this.rowIndex = rowIndex;
			this.rowState = rowState;
			this.errorText = errorText;
			this.inheritedRowStyle = inheritedRowStyle;
			this.isFirstDisplayedRow = isFirstDisplayedRow;
			this.isLastVisibleRow = isLastVisibleRow;
			this.paintParts = DataGridViewPaintParts.All;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0009E8AB File Offset: 0x0009CAAB
		internal DataGridViewRowPrePaintEventArgs(DataGridView dataGridView)
		{
			this.dataGridView = dataGridView;
		}

		/// <summary>Gets or sets the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</returns>
		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001F67 RID: 8039 RVA: 0x0009E8BA File Offset: 0x0009CABA
		// (set) Token: 0x06001F68 RID: 8040 RVA: 0x0009E8C2 File Offset: 0x0009CAC2
		public Rectangle ClipBounds
		{
			get
			{
				return this.clipBounds;
			}
			set
			{
				this.clipBounds = value;
			}
		}

		/// <summary>Gets a string that represents an error message for the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</summary>
		/// <returns>A string that represents an error message for the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</returns>
		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06001F69 RID: 8041 RVA: 0x0009E8CB File Offset: 0x0009CACB
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> used to paint the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> used to paint the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</returns>
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06001F6A RID: 8042 RVA: 0x0009E8D3 File Offset: 0x0009CAD3
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the cell style applied to the row.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains the cell style currently applied to the row.</returns>
		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06001F6B RID: 8043 RVA: 0x0009E8DB File Offset: 0x0009CADB
		public DataGridViewCellStyle InheritedRowStyle
		{
			get
			{
				return this.inheritedRowStyle;
			}
		}

		/// <summary>Gets a value indicating whether the current row is the first row currently displayed in the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the row being painted is currently the first row displayed in the <see cref="T:System.Windows.Forms.DataGridView" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001F6C RID: 8044 RVA: 0x0009E8E3 File Offset: 0x0009CAE3
		public bool IsFirstDisplayedRow
		{
			get
			{
				return this.isFirstDisplayedRow;
			}
		}

		/// <summary>Gets a value indicating whether the current row is the last visible row in the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the row being painted is currently the last row in the <see cref="T:System.Windows.Forms.DataGridView" /> that has the <see cref="P:System.Windows.Forms.DataGridViewRow.Visible" /> property set to <see langword="true" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x0009E8EB File Offset: 0x0009CAEB
		public bool IsLastVisibleRow
		{
			get
			{
				return this.isLastVisibleRow;
			}
		}

		/// <summary>The cell parts that are to be painted.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values specifying the parts to be painted.</returns>
		/// <exception cref="T:System.ArgumentException">The specified value when setting this property is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values.</exception>
		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x0009E8F3 File Offset: 0x0009CAF3
		// (set) Token: 0x06001F6F RID: 8047 RVA: 0x0009E8FB File Offset: 0x0009CAFB
		public DataGridViewPaintParts PaintParts
		{
			get
			{
				return this.paintParts;
			}
			set
			{
				if ((value & ~DataGridViewPaintParts.All) != DataGridViewPaintParts.None)
				{
					throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewPaintPartsCombination", new object[]
					{
						"value"
					}));
				}
				this.paintParts = value;
			}
		}

		/// <summary>Get the bounds of the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</returns>
		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001F70 RID: 8048 RVA: 0x0009E928 File Offset: 0x0009CB28
		public Rectangle RowBounds
		{
			get
			{
				return this.rowBounds;
			}
		}

		/// <summary>Gets the index of the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</summary>
		/// <returns>The index of the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</returns>
		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001F71 RID: 8049 RVA: 0x0009E930 File Offset: 0x0009CB30
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		/// <summary>Gets the state of the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the row.</returns>
		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001F72 RID: 8050 RVA: 0x0009E938 File Offset: 0x0009CB38
		public DataGridViewElementStates State
		{
			get
			{
				return this.rowState;
			}
		}

		/// <summary>Draws the focus rectangle around the specified bounds.</summary>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the focus area.</param>
		/// <param name="cellsPaintSelectionBackground">
		///       <see langword="true" /> to use the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.SelectionBackColor" /> property of the <see cref="P:System.Windows.Forms.DataGridViewRow.InheritedStyle" /> property to determine the color of the focus rectangle; <see langword="false" /> to use the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.BackColor" /> property of the <see cref="P:System.Windows.Forms.DataGridViewRow.InheritedStyle" />.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewRowPrePaintEventArgs.RowIndex" /> is less than zero or greater than the number of rows in the <see cref="T:System.Windows.Forms.DataGridView" /> control minus one.</exception>
		// Token: 0x06001F73 RID: 8051 RVA: 0x0009E940 File Offset: 0x0009CB40
		public void DrawFocus(Rectangle bounds, bool cellsPaintSelectionBackground)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).DrawFocus(this.graphics, this.clipBounds, bounds, this.rowIndex, this.rowState, this.inheritedRowStyle, cellsPaintSelectionBackground);
		}

		/// <summary>Paints the specified cell parts for the area in the specified bounds.</summary>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area of the <see cref="T:System.Windows.Forms.DataGridView" /> to be painted.</param>
		/// <param name="paintParts">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values specifying the parts to paint.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewRowPostPaintEventArgs.RowIndex" /> is less than zero or greater than the number of rows in the <see cref="T:System.Windows.Forms.DataGridView" /> control minus one.</exception>
		// Token: 0x06001F74 RID: 8052 RVA: 0x0009E9BC File Offset: 0x0009CBBC
		public void PaintCells(Rectangle clipBounds, DataGridViewPaintParts paintParts)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintCells(this.graphics, clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, paintParts);
		}

		/// <summary>Paints the cell backgrounds for the area in the specified bounds.</summary>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area of the <see cref="T:System.Windows.Forms.DataGridView" /> to be painted.</param>
		/// <param name="cellsPaintSelectionBackground">
		///       <see langword="true" /> to paint the background of the specified bounds with the color of the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.SelectionBackColor" /> property of the <see cref="P:System.Windows.Forms.DataGridViewRow.InheritedStyle" />; <see langword="false" /> to paint the background of the specified bounds with the color of the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.BackColor" /> property of the <see cref="P:System.Windows.Forms.DataGridViewRow.InheritedStyle" />.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewRowPostPaintEventArgs.RowIndex" /> is less than zero or greater than the number of rows in the <see cref="T:System.Windows.Forms.DataGridView" /> control minus one.</exception>
		// Token: 0x06001F75 RID: 8053 RVA: 0x0009EA3C File Offset: 0x0009CC3C
		public void PaintCellsBackground(Rectangle clipBounds, bool cellsPaintSelectionBackground)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			DataGridViewPaintParts dataGridViewPaintParts = DataGridViewPaintParts.Background | DataGridViewPaintParts.Border;
			if (cellsPaintSelectionBackground)
			{
				dataGridViewPaintParts |= DataGridViewPaintParts.SelectionBackground;
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintCells(this.graphics, clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, dataGridViewPaintParts);
		}

		/// <summary>Paints the cell contents for the area in the specified bounds.</summary>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area of the <see cref="T:System.Windows.Forms.DataGridView" /> to be painted.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewRowPostPaintEventArgs.RowIndex" /> is less than zero or greater than the number of rows in the <see cref="T:System.Windows.Forms.DataGridView" /> control minus one.</exception>
		// Token: 0x06001F76 RID: 8054 RVA: 0x0009EAC8 File Offset: 0x0009CCC8
		public void PaintCellsContent(Rectangle clipBounds)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintCells(this.graphics, clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ErrorIcon);
		}

		/// <summary>Paints the entire row header of the current <see cref="T:System.Windows.Forms.DataGridViewRow" />.</summary>
		/// <param name="paintSelectionBackground">
		///       <see langword="true" /> to paint the row header with the color of the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.SelectionBackColor" /> property of the <see cref="P:System.Windows.Forms.DataGridViewRow.InheritedStyle" />; <see langword="false" /> to paint the row header with the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.BackColor" /> of the <see cref="P:System.Windows.Forms.DataGridView.RowHeadersDefaultCellStyle" /> property.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewRowPostPaintEventArgs.RowIndex" /> is less than zero or greater than the number of rows in the <see cref="T:System.Windows.Forms.DataGridView" /> control minus one.</exception>
		// Token: 0x06001F77 RID: 8055 RVA: 0x0009EB48 File Offset: 0x0009CD48
		public void PaintHeader(bool paintSelectionBackground)
		{
			DataGridViewPaintParts dataGridViewPaintParts = DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ErrorIcon;
			if (paintSelectionBackground)
			{
				dataGridViewPaintParts |= DataGridViewPaintParts.SelectionBackground;
			}
			this.PaintHeader(dataGridViewPaintParts);
		}

		/// <summary>Paints the specified parts of the row header of the current row.</summary>
		/// <param name="paintParts">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values specifying the parts to paint.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.DataGridViewRowPostPaintEventArgs.RowIndex" /> is less than zero or greater than the number of rows in the <see cref="T:System.Windows.Forms.DataGridView" /> control minus one.</exception>
		// Token: 0x06001F78 RID: 8056 RVA: 0x0009EB68 File Offset: 0x0009CD68
		public void PaintHeader(DataGridViewPaintParts paintParts)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintHeader(this.graphics, this.clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, paintParts);
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x0009EBEC File Offset: 0x0009CDEC
		internal void SetProperties(Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, string errorText, DataGridViewCellStyle inheritedRowStyle, bool isFirstDisplayedRow, bool isLastVisibleRow)
		{
			this.graphics = graphics;
			this.clipBounds = clipBounds;
			this.rowBounds = rowBounds;
			this.rowIndex = rowIndex;
			this.rowState = rowState;
			this.errorText = errorText;
			this.inheritedRowStyle = inheritedRowStyle;
			this.isFirstDisplayedRow = isFirstDisplayedRow;
			this.isLastVisibleRow = isLastVisibleRow;
			this.paintParts = DataGridViewPaintParts.All;
			base.Handled = false;
		}

		// Token: 0x04000D9E RID: 3486
		private DataGridView dataGridView;

		// Token: 0x04000D9F RID: 3487
		private Graphics graphics;

		// Token: 0x04000DA0 RID: 3488
		private Rectangle clipBounds;

		// Token: 0x04000DA1 RID: 3489
		private Rectangle rowBounds;

		// Token: 0x04000DA2 RID: 3490
		private DataGridViewCellStyle inheritedRowStyle;

		// Token: 0x04000DA3 RID: 3491
		private int rowIndex;

		// Token: 0x04000DA4 RID: 3492
		private DataGridViewElementStates rowState;

		// Token: 0x04000DA5 RID: 3493
		private string errorText;

		// Token: 0x04000DA6 RID: 3494
		private bool isFirstDisplayedRow;

		// Token: 0x04000DA7 RID: 3495
		private bool isLastVisibleRow;

		// Token: 0x04000DA8 RID: 3496
		private DataGridViewPaintParts paintParts;
	}
}
