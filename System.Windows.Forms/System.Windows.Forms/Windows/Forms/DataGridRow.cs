using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000177 RID: 375
	internal abstract class DataGridRow : MarshalByRefObject
	{
		// Token: 0x060013E1 RID: 5089 RVA: 0x0004BD64 File Offset: 0x00049F64
		public DataGridRow(DataGrid dataGrid, DataGridTableStyle dgTable, int rowNumber)
		{
			if (dataGrid == null || dgTable.DataGrid == null)
			{
				throw new ArgumentNullException("dataGrid");
			}
			if (rowNumber < 0)
			{
				throw new ArgumentException(SR.GetString("DataGridRowRowNumber"), "rowNumber");
			}
			this.number = rowNumber;
			DataGridRow.colorMap[0].OldColor = Color.Black;
			DataGridRow.colorMap[0].NewColor = dgTable.HeaderForeColor;
			this.dgTable = dgTable;
			this.height = this.MinimumRowHeight(dgTable);
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0004BDFB File Offset: 0x00049FFB
		public AccessibleObject AccessibleObject
		{
			get
			{
				if (this.accessibleObject == null)
				{
					this.accessibleObject = this.CreateAccessibleObject();
				}
				return this.accessibleObject;
			}
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0004BE17 File Offset: 0x0004A017
		protected virtual AccessibleObject CreateAccessibleObject()
		{
			return new DataGridRow.DataGridRowAccessibleObject(this);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0004BE1F File Offset: 0x0004A01F
		protected internal virtual int MinimumRowHeight(DataGridTableStyle dgTable)
		{
			return this.MinimumRowHeight(dgTable.GridColumnStyles);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0004BE30 File Offset: 0x0004A030
		protected internal virtual int MinimumRowHeight(GridColumnStylesCollection columns)
		{
			int num = this.dgTable.IsDefault ? this.DataGrid.PreferredRowHeight : this.dgTable.PreferredRowHeight;
			try
			{
				if (this.dgTable.DataGrid.DataSource != null)
				{
					int count = columns.Count;
					for (int i = 0; i < count; i++)
					{
						if (columns[i].PropertyDescriptor != null)
						{
							num = Math.Max(num, columns[i].GetMinimumHeight());
						}
					}
				}
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x0004BEC0 File Offset: 0x0004A0C0
		public DataGrid DataGrid
		{
			get
			{
				return this.dgTable.DataGrid;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x0004BECD File Offset: 0x0004A0CD
		// (set) Token: 0x060013E8 RID: 5096 RVA: 0x0004BED5 File Offset: 0x0004A0D5
		internal DataGridTableStyle DataGridTableStyle
		{
			get
			{
				return this.dgTable;
			}
			set
			{
				this.dgTable = value;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x0004BEDE File Offset: 0x0004A0DE
		// (set) Token: 0x060013EA RID: 5098 RVA: 0x0004BEE6 File Offset: 0x0004A0E6
		public virtual int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = Math.Max(0, value);
				this.dgTable.DataGrid.OnRowHeightChanged(this);
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x0004BF06 File Offset: 0x0004A106
		public int RowNumber
		{
			get
			{
				return this.number;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x0004BF0E File Offset: 0x0004A10E
		// (set) Token: 0x060013ED RID: 5101 RVA: 0x0004BF16 File Offset: 0x0004A116
		public virtual bool Selected
		{
			get
			{
				return this.selected;
			}
			set
			{
				this.selected = value;
				this.InvalidateRow();
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0004BF28 File Offset: 0x0004A128
		protected Bitmap GetBitmap(string bitmapName)
		{
			Bitmap bitmap = null;
			try
			{
				bitmap = new Bitmap(typeof(DataGridCaption), bitmapName);
				bitmap.MakeTransparent();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return bitmap;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0004BF64 File Offset: 0x0004A164
		public virtual Rectangle GetCellBounds(int col)
		{
			int firstVisibleColumn = this.dgTable.DataGrid.FirstVisibleColumn;
			int num = 0;
			Rectangle result = default(Rectangle);
			GridColumnStylesCollection gridColumnStyles = this.dgTable.GridColumnStyles;
			if (gridColumnStyles != null)
			{
				for (int i = firstVisibleColumn; i < col; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num += gridColumnStyles[i].Width;
					}
				}
				int gridLineWidth = this.dgTable.GridLineWidth;
				result = new Rectangle(num, 0, gridColumnStyles[col].Width - gridLineWidth, this.Height - gridLineWidth);
			}
			return result;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0004BFF9 File Offset: 0x0004A1F9
		public virtual Rectangle GetNonScrollableArea()
		{
			return Rectangle.Empty;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0004C000 File Offset: 0x0004A200
		protected Bitmap GetStarBitmap()
		{
			if (DataGridRow.starBmp == null)
			{
				DataGridRow.starBmp = this.GetBitmap("DataGridRow.star.bmp");
			}
			return DataGridRow.starBmp;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0004C01E File Offset: 0x0004A21E
		protected Bitmap GetPencilBitmap()
		{
			if (DataGridRow.pencilBmp == null)
			{
				DataGridRow.pencilBmp = this.GetBitmap("DataGridRow.pencil.bmp");
			}
			return DataGridRow.pencilBmp;
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0004C03C File Offset: 0x0004A23C
		protected Bitmap GetErrorBitmap()
		{
			if (DataGridRow.errorBmp == null)
			{
				DataGridRow.errorBmp = this.GetBitmap("DataGridRow.error.bmp");
			}
			DataGridRow.errorBmp.MakeTransparent();
			return DataGridRow.errorBmp;
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0004C064 File Offset: 0x0004A264
		protected Bitmap GetLeftArrowBitmap()
		{
			if (DataGridRow.leftArrow == null)
			{
				DataGridRow.leftArrow = this.GetBitmap("DataGridRow.left.bmp");
			}
			return DataGridRow.leftArrow;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0004C082 File Offset: 0x0004A282
		protected Bitmap GetRightArrowBitmap()
		{
			if (DataGridRow.rightArrow == null)
			{
				DataGridRow.rightArrow = this.GetBitmap("DataGridRow.right.bmp");
			}
			return DataGridRow.rightArrow;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0004C0A0 File Offset: 0x0004A2A0
		public virtual void InvalidateRow()
		{
			this.dgTable.DataGrid.InvalidateRow(this.number);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0004C0B8 File Offset: 0x0004A2B8
		public virtual void InvalidateRowRect(Rectangle r)
		{
			this.dgTable.DataGrid.InvalidateRowRect(this.number, r);
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0000701A File Offset: 0x0000521A
		public virtual void OnEdit()
		{
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0004C0D4 File Offset: 0x0004A2D4
		public virtual bool OnKeyPress(Keys keyData)
		{
			int columnNumber = this.dgTable.DataGrid.CurrentCell.ColumnNumber;
			GridColumnStylesCollection gridColumnStyles = this.dgTable.GridColumnStyles;
			if (gridColumnStyles != null && columnNumber >= 0 && columnNumber < gridColumnStyles.Count)
			{
				DataGridColumnStyle dataGridColumnStyle = gridColumnStyles[columnNumber];
				if (dataGridColumnStyle.KeyPress(this.RowNumber, keyData))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0004C130 File Offset: 0x0004A330
		public virtual bool OnMouseDown(int x, int y, Rectangle rowHeaders)
		{
			return this.OnMouseDown(x, y, rowHeaders, false);
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0004C13C File Offset: 0x0004A33C
		public virtual bool OnMouseDown(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			this.LoseChildFocus(rowHeaders, alignToRight);
			return false;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public virtual bool OnMouseMove(int x, int y, Rectangle rowHeaders)
		{
			return false;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public virtual bool OnMouseMove(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			return false;
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0000701A File Offset: 0x0000521A
		public virtual void OnMouseLeft(Rectangle rowHeaders, bool alignToRight)
		{
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0000701A File Offset: 0x0000521A
		public virtual void OnMouseLeft()
		{
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0000701A File Offset: 0x0000521A
		public virtual void OnRowEnter()
		{
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0000701A File Offset: 0x0000521A
		public virtual void OnRowLeave()
		{
		}

		// Token: 0x06001402 RID: 5122
		internal abstract bool ProcessTabKey(Keys keyData, Rectangle rowHeaders, bool alignToRight);

		// Token: 0x06001403 RID: 5123
		internal abstract void LoseChildFocus(Rectangle rowHeaders, bool alignToRight);

		// Token: 0x06001404 RID: 5124
		public abstract int Paint(Graphics g, Rectangle dataBounds, Rectangle rowBounds, int firstVisibleColumn, int numVisibleColumns);

		// Token: 0x06001405 RID: 5125
		public abstract int Paint(Graphics g, Rectangle dataBounds, Rectangle rowBounds, int firstVisibleColumn, int numVisibleColumns, bool alignToRight);

		// Token: 0x06001406 RID: 5126 RVA: 0x0004C148 File Offset: 0x0004A348
		protected virtual void PaintBottomBorder(Graphics g, Rectangle bounds, int dataWidth)
		{
			this.PaintBottomBorder(g, bounds, dataWidth, this.dgTable.GridLineWidth, false);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0004C160 File Offset: 0x0004A360
		protected virtual void PaintBottomBorder(Graphics g, Rectangle bounds, int dataWidth, int borderWidth, bool alignToRight)
		{
			Rectangle rect = new Rectangle(alignToRight ? (bounds.Right - dataWidth) : bounds.X, bounds.Bottom - borderWidth, dataWidth, borderWidth);
			g.FillRectangle(this.dgTable.IsDefault ? this.DataGrid.GridLineBrush : this.dgTable.GridLineBrush, rect);
			if (dataWidth < bounds.Width)
			{
				g.FillRectangle(this.dgTable.DataGrid.BackgroundBrush, alignToRight ? bounds.X : rect.Right, rect.Y, bounds.Width - rect.Width, borderWidth);
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0004C20F File Offset: 0x0004A40F
		public virtual int PaintData(Graphics g, Rectangle bounds, int firstVisibleColumn, int columnCount)
		{
			return this.PaintData(g, bounds, firstVisibleColumn, columnCount, false);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0004C220 File Offset: 0x0004A420
		public virtual int PaintData(Graphics g, Rectangle bounds, int firstVisibleColumn, int columnCount, bool alignToRight)
		{
			Rectangle cellBounds = bounds;
			int num = this.dgTable.IsDefault ? this.DataGrid.GridLineWidth : this.dgTable.GridLineWidth;
			int num2 = 0;
			DataGridCell currentCell = this.dgTable.DataGrid.CurrentCell;
			GridColumnStylesCollection gridColumnStyles = this.dgTable.GridColumnStyles;
			int count = gridColumnStyles.Count;
			int num3 = firstVisibleColumn;
			while (num3 < count && num2 <= bounds.Width)
			{
				if (gridColumnStyles[num3].PropertyDescriptor != null && gridColumnStyles[num3].Width > 0)
				{
					cellBounds.Width = gridColumnStyles[num3].Width - num;
					if (alignToRight)
					{
						cellBounds.X = bounds.Right - num2 - cellBounds.Width;
					}
					else
					{
						cellBounds.X = bounds.X + num2;
					}
					Brush backBr = this.BackBrushForDataPaint(ref currentCell, gridColumnStyles[num3], num3);
					Brush foreBrush = this.ForeBrushForDataPaint(ref currentCell, gridColumnStyles[num3], num3);
					this.PaintCellContents(g, cellBounds, gridColumnStyles[num3], backBr, foreBrush, alignToRight);
					if (num > 0)
					{
						g.FillRectangle(this.dgTable.IsDefault ? this.DataGrid.GridLineBrush : this.dgTable.GridLineBrush, alignToRight ? (cellBounds.X - num) : cellBounds.Right, cellBounds.Y, num, cellBounds.Height);
					}
					num2 += cellBounds.Width + num;
				}
				num3++;
			}
			if (num2 < bounds.Width)
			{
				g.FillRectangle(this.dgTable.DataGrid.BackgroundBrush, alignToRight ? bounds.X : (bounds.X + num2), bounds.Y, bounds.Width - num2, bounds.Height);
			}
			return num2;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0004C3FD File Offset: 0x0004A5FD
		protected virtual void PaintCellContents(Graphics g, Rectangle cellBounds, DataGridColumnStyle column, Brush backBr, Brush foreBrush)
		{
			this.PaintCellContents(g, cellBounds, column, backBr, foreBrush, false);
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0004C40D File Offset: 0x0004A60D
		protected virtual void PaintCellContents(Graphics g, Rectangle cellBounds, DataGridColumnStyle column, Brush backBr, Brush foreBrush, bool alignToRight)
		{
			g.FillRectangle(backBr, cellBounds);
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0004C418 File Offset: 0x0004A618
		protected Rectangle PaintIcon(Graphics g, Rectangle visualBounds, bool paintIcon, bool alignToRight, Bitmap bmp)
		{
			return this.PaintIcon(g, visualBounds, paintIcon, alignToRight, bmp, this.dgTable.IsDefault ? this.DataGrid.HeaderBackBrush : this.dgTable.HeaderBackBrush);
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0004C44C File Offset: 0x0004A64C
		protected Rectangle PaintIcon(Graphics g, Rectangle visualBounds, bool paintIcon, bool alignToRight, Bitmap bmp, Brush backBrush)
		{
			Size size = bmp.Size;
			Rectangle rectangle = new Rectangle(alignToRight ? (visualBounds.Right - 3 - size.Width) : (visualBounds.X + 3), visualBounds.Y + 2, size.Width, size.Height);
			g.FillRectangle(backBrush, visualBounds);
			if (paintIcon)
			{
				DataGridRow.colorMap[0].NewColor = (this.dgTable.IsDefault ? this.DataGrid.HeaderForeColor : this.dgTable.HeaderForeColor);
				DataGridRow.colorMap[0].OldColor = Color.Black;
				ImageAttributes imageAttributes = new ImageAttributes();
				imageAttributes.SetRemapTable(DataGridRow.colorMap, ColorAdjustType.Bitmap);
				g.DrawImage(bmp, rectangle, 0, 0, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, imageAttributes);
				imageAttributes.Dispose();
			}
			return rectangle;
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0004C521 File Offset: 0x0004A721
		public virtual void PaintHeader(Graphics g, Rectangle visualBounds)
		{
			this.PaintHeader(g, visualBounds, false);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0004C52C File Offset: 0x0004A72C
		public virtual void PaintHeader(Graphics g, Rectangle visualBounds, bool alignToRight)
		{
			this.PaintHeader(g, visualBounds, alignToRight, false);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0004C538 File Offset: 0x0004A738
		public virtual void PaintHeader(Graphics g, Rectangle visualBounds, bool alignToRight, bool rowIsDirty)
		{
			Rectangle visualBounds2 = visualBounds;
			Bitmap bitmap;
			if (this is DataGridAddNewRow)
			{
				bitmap = this.GetStarBitmap();
				Bitmap obj = bitmap;
				lock (obj)
				{
					visualBounds2.X += this.PaintIcon(g, visualBounds2, true, alignToRight, bitmap).Width + 3;
				}
				return;
			}
			if (rowIsDirty)
			{
				bitmap = this.GetPencilBitmap();
				Bitmap obj2 = bitmap;
				lock (obj2)
				{
					visualBounds2.X += this.PaintIcon(g, visualBounds2, this.RowNumber == this.DataGrid.CurrentCell.RowNumber, alignToRight, bitmap).Width + 3;
					goto IL_128;
				}
			}
			bitmap = (alignToRight ? this.GetLeftArrowBitmap() : this.GetRightArrowBitmap());
			Bitmap obj3 = bitmap;
			lock (obj3)
			{
				visualBounds2.X += this.PaintIcon(g, visualBounds2, this.RowNumber == this.DataGrid.CurrentCell.RowNumber, alignToRight, bitmap).Width + 3;
			}
			IL_128:
			object obj4 = this.DataGrid.ListManager[this.number];
			if (!(obj4 is IDataErrorInfo))
			{
				return;
			}
			string text = ((IDataErrorInfo)obj4).Error;
			if (text == null)
			{
				text = string.Empty;
			}
			if (this.tooltip != text && !string.IsNullOrEmpty(this.tooltip))
			{
				this.DataGrid.ToolTipProvider.RemoveToolTip(this.tooltipID);
				this.tooltip = string.Empty;
				this.tooltipID = new IntPtr(-1);
			}
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			bitmap = this.GetErrorBitmap();
			Bitmap obj5 = bitmap;
			Rectangle iconBounds;
			lock (obj5)
			{
				iconBounds = this.PaintIcon(g, visualBounds2, true, alignToRight, bitmap);
			}
			visualBounds2.X += iconBounds.Width + 3;
			this.tooltip = text;
			DataGrid dataGrid = this.DataGrid;
			int toolTipId = dataGrid.ToolTipId;
			dataGrid.ToolTipId = toolTipId + 1;
			this.tooltipID = (IntPtr)toolTipId;
			this.DataGrid.ToolTipProvider.AddToolTip(this.tooltip, this.tooltipID, iconBounds);
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0004C7B8 File Offset: 0x0004A9B8
		protected Brush GetBackBrush()
		{
			Brush result = this.dgTable.IsDefault ? this.DataGrid.BackBrush : this.dgTable.BackBrush;
			if (this.DataGrid.LedgerStyle && this.RowNumber % 2 == 1)
			{
				result = (this.dgTable.IsDefault ? this.DataGrid.AlternatingBackBrush : this.dgTable.AlternatingBackBrush);
			}
			return result;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0004C82C File Offset: 0x0004AA2C
		protected Brush BackBrushForDataPaint(ref DataGridCell current, DataGridColumnStyle gridColumn, int column)
		{
			Brush result = this.GetBackBrush();
			if (this.Selected)
			{
				result = (this.dgTable.IsDefault ? this.DataGrid.SelectionBackBrush : this.dgTable.SelectionBackBrush);
			}
			return result;
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0004C870 File Offset: 0x0004AA70
		protected Brush ForeBrushForDataPaint(ref DataGridCell current, DataGridColumnStyle gridColumn, int column)
		{
			Brush result = this.dgTable.IsDefault ? this.DataGrid.ForeBrush : this.dgTable.ForeBrush;
			if (this.Selected)
			{
				result = (this.dgTable.IsDefault ? this.DataGrid.SelectionForeBrush : this.dgTable.SelectionForeBrush);
			}
			return result;
		}

		// Token: 0x040009BF RID: 2495
		protected internal int number;

		// Token: 0x040009C0 RID: 2496
		private bool selected;

		// Token: 0x040009C1 RID: 2497
		private int height;

		// Token: 0x040009C2 RID: 2498
		private IntPtr tooltipID = new IntPtr(-1);

		// Token: 0x040009C3 RID: 2499
		private string tooltip = string.Empty;

		// Token: 0x040009C4 RID: 2500
		private AccessibleObject accessibleObject;

		// Token: 0x040009C5 RID: 2501
		protected DataGridTableStyle dgTable;

		// Token: 0x040009C6 RID: 2502
		private static ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x040009C7 RID: 2503
		private static Bitmap rightArrow = null;

		// Token: 0x040009C8 RID: 2504
		private static Bitmap leftArrow = null;

		// Token: 0x040009C9 RID: 2505
		private static Bitmap errorBmp = null;

		// Token: 0x040009CA RID: 2506
		private static Bitmap pencilBmp = null;

		// Token: 0x040009CB RID: 2507
		private static Bitmap starBmp = null;

		// Token: 0x040009CC RID: 2508
		protected const int xOffset = 3;

		// Token: 0x040009CD RID: 2509
		protected const int yOffset = 2;

		// Token: 0x02000597 RID: 1431
		[ComVisible(true)]
		protected class DataGridRowAccessibleObject : AccessibleObject
		{
			// Token: 0x0600582C RID: 22572 RVA: 0x00172E5C File Offset: 0x0017105C
			internal static string CellToDisplayString(DataGrid grid, int row, int column)
			{
				if (column < grid.myGridTable.GridColumnStyles.Count)
				{
					return grid.myGridTable.GridColumnStyles[column].PropertyDescriptor.Converter.ConvertToString(grid[row, column]);
				}
				return "";
			}

			// Token: 0x0600582D RID: 22573 RVA: 0x00172EAA File Offset: 0x001710AA
			internal static object DisplayStringToCell(DataGrid grid, int row, int column, string value)
			{
				if (column < grid.myGridTable.GridColumnStyles.Count)
				{
					return grid.myGridTable.GridColumnStyles[column].PropertyDescriptor.Converter.ConvertFromString(value);
				}
				return null;
			}

			// Token: 0x0600582E RID: 22574 RVA: 0x00172EE4 File Offset: 0x001710E4
			public DataGridRowAccessibleObject(DataGridRow owner)
			{
				this.owner = owner;
				DataGrid dataGrid = this.DataGrid;
				this.EnsureChildren();
			}

			// Token: 0x0600582F RID: 22575 RVA: 0x00172F0B File Offset: 0x0017110B
			private void EnsureChildren()
			{
				if (this.cells == null)
				{
					this.cells = new ArrayList(this.DataGrid.myGridTable.GridColumnStyles.Count + 2);
					this.AddChildAccessibleObjects(this.cells);
				}
			}

			// Token: 0x06005830 RID: 22576 RVA: 0x00172F44 File Offset: 0x00171144
			protected virtual void AddChildAccessibleObjects(IList children)
			{
				GridColumnStylesCollection gridColumnStyles = this.DataGrid.myGridTable.GridColumnStyles;
				int count = gridColumnStyles.Count;
				for (int i = 0; i < count; i++)
				{
					children.Add(this.CreateCellAccessibleObject(i));
				}
			}

			// Token: 0x06005831 RID: 22577 RVA: 0x00172F83 File Offset: 0x00171183
			protected virtual AccessibleObject CreateCellAccessibleObject(int column)
			{
				return new DataGridRow.DataGridCellAccessibleObject(this.owner, column);
			}

			// Token: 0x1700151B RID: 5403
			// (get) Token: 0x06005832 RID: 22578 RVA: 0x00172F91 File Offset: 0x00171191
			public override Rectangle Bounds
			{
				get
				{
					return this.DataGrid.RectangleToScreen(this.DataGrid.GetRowBounds(this.owner));
				}
			}

			// Token: 0x1700151C RID: 5404
			// (get) Token: 0x06005833 RID: 22579 RVA: 0x00172FAF File Offset: 0x001711AF
			public override string Name
			{
				get
				{
					if (this.owner is DataGridAddNewRow)
					{
						return SR.GetString("AccDGNewRow");
					}
					return DataGridRow.DataGridRowAccessibleObject.CellToDisplayString(this.DataGrid, this.owner.RowNumber, 0);
				}
			}

			// Token: 0x1700151D RID: 5405
			// (get) Token: 0x06005834 RID: 22580 RVA: 0x00172FE0 File Offset: 0x001711E0
			protected DataGridRow Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x1700151E RID: 5406
			// (get) Token: 0x06005835 RID: 22581 RVA: 0x00172FE8 File Offset: 0x001711E8
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.DataGrid.AccessibilityObject;
				}
			}

			// Token: 0x1700151F RID: 5407
			// (get) Token: 0x06005836 RID: 22582 RVA: 0x00172FF5 File Offset: 0x001711F5
			private DataGrid DataGrid
			{
				get
				{
					return this.owner.DataGrid;
				}
			}

			// Token: 0x17001520 RID: 5408
			// (get) Token: 0x06005837 RID: 22583 RVA: 0x00173002 File Offset: 0x00171202
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Row;
				}
			}

			// Token: 0x17001521 RID: 5409
			// (get) Token: 0x06005838 RID: 22584 RVA: 0x00173008 File Offset: 0x00171208
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					if (this.DataGrid.CurrentCell.RowNumber == this.owner.RowNumber)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					if (this.DataGrid.CurrentRowIndex == this.owner.RowNumber)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17001522 RID: 5410
			// (get) Token: 0x06005839 RID: 22585 RVA: 0x0000E334 File Offset: 0x0000C534
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.Name;
				}
			}

			// Token: 0x0600583A RID: 22586 RVA: 0x0017305C File Offset: 0x0017125C
			public override AccessibleObject GetChild(int index)
			{
				if (index < this.cells.Count)
				{
					return (AccessibleObject)this.cells[index];
				}
				return null;
			}

			// Token: 0x0600583B RID: 22587 RVA: 0x0017307F File Offset: 0x0017127F
			public override int GetChildCount()
			{
				return this.cells.Count;
			}

			// Token: 0x0600583C RID: 22588 RVA: 0x0017308C File Offset: 0x0017128C
			public override AccessibleObject GetFocused()
			{
				if (this.DataGrid.Focused)
				{
					DataGridCell currentCell = this.DataGrid.CurrentCell;
					if (currentCell.RowNumber == this.owner.RowNumber)
					{
						return (AccessibleObject)this.cells[currentCell.ColumnNumber];
					}
				}
				return null;
			}

			// Token: 0x0600583D RID: 22589 RVA: 0x001730E0 File Offset: 0x001712E0
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber - 1);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber + 1);
				case AccessibleNavigation.FirstChild:
					if (this.GetChildCount() > 0)
					{
						return this.GetChild(0);
					}
					break;
				case AccessibleNavigation.LastChild:
					if (this.GetChildCount() > 0)
					{
						return this.GetChild(this.GetChildCount() - 1);
					}
					break;
				}
				return null;
			}

			// Token: 0x0600583E RID: 22590 RVA: 0x001731B0 File Offset: 0x001713B0
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					this.DataGrid.Focus();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.DataGrid.CurrentRowIndex = this.owner.RowNumber;
				}
			}

			// Token: 0x040038A7 RID: 14503
			private ArrayList cells;

			// Token: 0x040038A8 RID: 14504
			private DataGridRow owner;
		}

		// Token: 0x02000598 RID: 1432
		[ComVisible(true)]
		protected class DataGridCellAccessibleObject : AccessibleObject
		{
			// Token: 0x0600583F RID: 22591 RVA: 0x001731E0 File Offset: 0x001713E0
			public DataGridCellAccessibleObject(DataGridRow owner, int column)
			{
				this.owner = owner;
				this.column = column;
			}

			// Token: 0x17001523 RID: 5411
			// (get) Token: 0x06005840 RID: 22592 RVA: 0x001731F6 File Offset: 0x001713F6
			public override Rectangle Bounds
			{
				get
				{
					return this.DataGrid.RectangleToScreen(this.DataGrid.GetCellBounds(new DataGridCell(this.owner.RowNumber, this.column)));
				}
			}

			// Token: 0x17001524 RID: 5412
			// (get) Token: 0x06005841 RID: 22593 RVA: 0x00173224 File Offset: 0x00171424
			public override string Name
			{
				get
				{
					return this.DataGrid.myGridTable.GridColumnStyles[this.column].HeaderText;
				}
			}

			// Token: 0x17001525 RID: 5413
			// (get) Token: 0x06005842 RID: 22594 RVA: 0x00173246 File Offset: 0x00171446
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.AccessibleObject;
				}
			}

			// Token: 0x17001526 RID: 5414
			// (get) Token: 0x06005843 RID: 22595 RVA: 0x00173253 File Offset: 0x00171453
			protected DataGrid DataGrid
			{
				get
				{
					return this.owner.DataGrid;
				}
			}

			// Token: 0x17001527 RID: 5415
			// (get) Token: 0x06005844 RID: 22596 RVA: 0x00173260 File Offset: 0x00171460
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("AccDGEdit");
				}
			}

			// Token: 0x17001528 RID: 5416
			// (get) Token: 0x06005845 RID: 22597 RVA: 0x0017326C File Offset: 0x0017146C
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Cell;
				}
			}

			// Token: 0x17001529 RID: 5417
			// (get) Token: 0x06005846 RID: 22598 RVA: 0x00173270 File Offset: 0x00171470
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					if (this.DataGrid.CurrentCell.RowNumber == this.owner.RowNumber && this.DataGrid.CurrentCell.ColumnNumber == this.column)
					{
						if (this.DataGrid.Focused)
						{
							accessibleStates |= AccessibleStates.Focused;
						}
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			// Token: 0x1700152A RID: 5418
			// (get) Token: 0x06005847 RID: 22599 RVA: 0x001732D4 File Offset: 0x001714D4
			// (set) Token: 0x06005848 RID: 22600 RVA: 0x00173304 File Offset: 0x00171504
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (this.owner is DataGridAddNewRow)
					{
						return null;
					}
					return DataGridRow.DataGridRowAccessibleObject.CellToDisplayString(this.DataGrid, this.owner.RowNumber, this.column);
				}
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				set
				{
					if (!(this.owner is DataGridAddNewRow))
					{
						object value2 = DataGridRow.DataGridRowAccessibleObject.DisplayStringToCell(this.DataGrid, this.owner.RowNumber, this.column, value);
						this.DataGrid[this.owner.RowNumber, this.column] = value2;
					}
				}
			}

			// Token: 0x06005849 RID: 22601 RVA: 0x00173359 File Offset: 0x00171559
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.Select(AccessibleSelection.TakeFocus | AccessibleSelection.TakeSelection);
			}

			// Token: 0x0600584A RID: 22602 RVA: 0x00173362 File Offset: 0x00171562
			public override AccessibleObject GetFocused()
			{
				return this.DataGrid.AccessibilityObject.GetFocused();
			}

			// Token: 0x0600584B RID: 22603 RVA: 0x00173374 File Offset: 0x00171574
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
					return this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber - 1).Navigate(AccessibleNavigation.FirstChild);
				case AccessibleNavigation.Down:
					return this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber + 1).Navigate(AccessibleNavigation.FirstChild);
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
				{
					if (this.column > 0)
					{
						return this.owner.AccessibleObject.GetChild(this.column - 1);
					}
					AccessibleObject child = this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber - 1);
					if (child != null)
					{
						return child.Navigate(AccessibleNavigation.LastChild);
					}
					break;
				}
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
				{
					if (this.column < this.owner.AccessibleObject.GetChildCount() - 1)
					{
						return this.owner.AccessibleObject.GetChild(this.column + 1);
					}
					AccessibleObject child2 = this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber + 1);
					if (child2 != null)
					{
						return child2.Navigate(AccessibleNavigation.FirstChild);
					}
					break;
				}
				}
				return null;
			}

			// Token: 0x0600584C RID: 22604 RVA: 0x00173501 File Offset: 0x00171701
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					this.DataGrid.Focus();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.DataGrid.CurrentCell = new DataGridCell(this.owner.RowNumber, this.column);
				}
			}

			// Token: 0x040038A9 RID: 14505
			private DataGridRow owner;

			// Token: 0x040038AA RID: 14506
			private int column;
		}
	}
}
