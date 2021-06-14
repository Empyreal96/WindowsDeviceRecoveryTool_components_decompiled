using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000174 RID: 372
	internal class DataGridParentRows
	{
		// Token: 0x06001384 RID: 4996 RVA: 0x00049548 File Offset: 0x00047748
		internal DataGridParentRows(DataGrid dataGrid)
		{
			this.colorMap[0].OldColor = Color.Black;
			this.dataGrid = dataGrid;
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x000495E1 File Offset: 0x000477E1
		public AccessibleObject AccessibleObject
		{
			get
			{
				if (this.accessibleObject == null)
				{
					this.accessibleObject = new DataGridParentRows.DataGridParentRowsAccessibleObject(this);
				}
				return this.accessibleObject;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x000495FD File Offset: 0x000477FD
		// (set) Token: 0x06001387 RID: 4999 RVA: 0x0004960C File Offset: 0x0004780C
		internal Color BackColor
		{
			get
			{
				return this.backBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"Parent Rows BackColor"
					}));
				}
				if (value != this.backBrush.Color)
				{
					this.backBrush = new SolidBrush(value);
					this.Invalidate();
				}
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x00049665 File Offset: 0x00047865
		// (set) Token: 0x06001389 RID: 5001 RVA: 0x0004966D File Offset: 0x0004786D
		internal SolidBrush BackBrush
		{
			get
			{
				return this.backBrush;
			}
			set
			{
				if (value != this.backBrush)
				{
					this.CheckNull(value, "BackBrush");
					this.backBrush = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x00049691 File Offset: 0x00047891
		// (set) Token: 0x0600138B RID: 5003 RVA: 0x00049699 File Offset: 0x00047899
		internal SolidBrush ForeBrush
		{
			get
			{
				return this.foreBrush;
			}
			set
			{
				if (value != this.foreBrush)
				{
					this.CheckNull(value, "BackBrush");
					this.foreBrush = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x000496C0 File Offset: 0x000478C0
		internal Rectangle GetBoundsForDataGridStateAccesibility(DataGridState dgs)
		{
			Rectangle empty = Rectangle.Empty;
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				int num2 = (int)this.rowHeights[i];
				if (this.parents[i] == dgs)
				{
					empty.X = (this.layout.leftArrow.IsEmpty ? this.layout.data.X : this.layout.leftArrow.Right);
					empty.Height = num2;
					empty.Y = num;
					empty.Width = this.layout.data.Width;
					return empty;
				}
				num += num2;
			}
			return empty;
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x00049776 File Offset: 0x00047976
		// (set) Token: 0x0600138E RID: 5006 RVA: 0x0004977E File Offset: 0x0004797E
		internal Brush BorderBrush
		{
			get
			{
				return this.borderBrush;
			}
			set
			{
				if (value != this.borderBrush)
				{
					this.borderBrush = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x00049796 File Offset: 0x00047996
		internal int Height
		{
			get
			{
				return this.totalHeight;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x0004979E File Offset: 0x0004799E
		// (set) Token: 0x06001391 RID: 5009 RVA: 0x000497AC File Offset: 0x000479AC
		internal Color ForeColor
		{
			get
			{
				return this.foreBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"Parent Rows ForeColor"
					}));
				}
				if (value != this.foreBrush.Color)
				{
					this.foreBrush = new SolidBrush(value);
					this.Invalidate();
				}
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x00049805 File Offset: 0x00047A05
		// (set) Token: 0x06001393 RID: 5011 RVA: 0x00049812 File Offset: 0x00047A12
		internal bool Visible
		{
			get
			{
				return this.dataGrid.ParentRowsVisible;
			}
			set
			{
				this.dataGrid.ParentRowsVisible = value;
			}
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00049820 File Offset: 0x00047A20
		internal void AddParent(DataGridState dgs)
		{
			CurrencyManager currencyManager = (CurrencyManager)this.dataGrid.BindingContext[dgs.DataSource, dgs.DataMember];
			this.parents.Add(dgs);
			this.SetParentCount(this.parentsCount + 1);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0004986C File Offset: 0x00047A6C
		internal void Clear()
		{
			for (int i = 0; i < this.parents.Count; i++)
			{
				DataGridState dataGridState = this.parents[i] as DataGridState;
				dataGridState.RemoveChangeNotification();
			}
			this.parents.Clear();
			this.rowHeights.Clear();
			this.totalHeight = 0;
			this.SetParentCount(0);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x000498CB File Offset: 0x00047ACB
		internal void SetParentCount(int count)
		{
			this.parentsCount = count;
			this.dataGrid.Caption.BackButtonVisible = (this.parentsCount > 0 && this.dataGrid.AllowNavigation);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x000498FB File Offset: 0x00047AFB
		internal void CheckNull(object value, string propName)
		{
			if (value == null)
			{
				throw new ArgumentNullException("propName");
			}
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0004990B File Offset: 0x00047B0B
		internal void Dispose()
		{
			this.gridLinePen.Dispose();
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00049918 File Offset: 0x00047B18
		internal DataGridState GetTopParent()
		{
			if (this.parentsCount < 1)
			{
				return null;
			}
			return (DataGridState)((ICloneable)this.parents[this.parentsCount - 1]).Clone();
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00049947 File Offset: 0x00047B47
		internal bool IsEmpty()
		{
			return this.parentsCount == 0;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00049954 File Offset: 0x00047B54
		internal DataGridState PopTop()
		{
			if (this.parentsCount < 1)
			{
				return null;
			}
			this.SetParentCount(this.parentsCount - 1);
			DataGridState dataGridState = (DataGridState)this.parents[this.parentsCount];
			dataGridState.RemoveChangeNotification();
			this.parents.RemoveAt(this.parentsCount);
			return dataGridState;
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000499A9 File Offset: 0x00047BA9
		internal void Invalidate()
		{
			if (this.dataGrid != null)
			{
				this.dataGrid.InvalidateParentRows();
			}
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x000499C0 File Offset: 0x00047BC0
		internal void InvalidateRect(Rectangle rect)
		{
			if (this.dataGrid != null)
			{
				Rectangle r = new Rectangle(rect.X, rect.Y, rect.Width + this.borderWidth, rect.Height + this.borderWidth);
				this.dataGrid.InvalidateParentRowsRect(r);
			}
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00049A14 File Offset: 0x00047C14
		internal void OnLayout()
		{
			if (this.parentsCount == this.rowHeights.Count)
			{
				return;
			}
			if (this.totalHeight == 0)
			{
				this.totalHeight += 2 * this.borderWidth;
			}
			this.textRegionHeight = this.dataGrid.Font.Height + 2;
			if (this.parentsCount > this.rowHeights.Count)
			{
				int count = this.rowHeights.Count;
				for (int i = count; i < this.parentsCount; i++)
				{
					DataGridState dataGridState = (DataGridState)this.parents[i];
					GridColumnStylesCollection gridColumnStyles = dataGridState.GridColumnStyles;
					int val = 0;
					for (int j = 0; j < gridColumnStyles.Count; j++)
					{
						val = Math.Max(val, gridColumnStyles[j].GetMinimumHeight());
					}
					int num = Math.Max(val, this.textRegionHeight);
					num++;
					this.rowHeights.Add(num);
					this.totalHeight += num;
				}
				return;
			}
			if (this.parentsCount == 0)
			{
				this.totalHeight = 0;
			}
			else
			{
				this.totalHeight -= (int)this.rowHeights[this.rowHeights.Count - 1];
			}
			this.rowHeights.RemoveAt(this.rowHeights.Count - 1);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00049B78 File Offset: 0x00047D78
		private int CellCount()
		{
			int num = this.ColsCount();
			if (this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.TableName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00049BAF File Offset: 0x00047DAF
		private void ResetMouseInfo()
		{
			this.downLeftArrow = false;
			this.downRightArrow = false;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00049BBF File Offset: 0x00047DBF
		private void LeftArrowClick(int cellCount)
		{
			if (this.horizOffset > 0)
			{
				this.ResetMouseInfo();
				this.horizOffset--;
				this.Invalidate();
				return;
			}
			this.ResetMouseInfo();
			this.InvalidateRect(this.layout.leftArrow);
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00049BFC File Offset: 0x00047DFC
		private void RightArrowClick(int cellCount)
		{
			if (this.horizOffset < cellCount - 1)
			{
				this.ResetMouseInfo();
				this.horizOffset++;
				this.Invalidate();
				return;
			}
			this.ResetMouseInfo();
			this.InvalidateRect(this.layout.rightArrow);
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00049C3C File Offset: 0x00047E3C
		internal void OnMouseDown(int x, int y, bool alignToRight)
		{
			if (this.layout.rightArrow.IsEmpty)
			{
				return;
			}
			int cellCount = this.CellCount();
			if (this.layout.rightArrow.Contains(x, y))
			{
				this.downRightArrow = true;
				if (alignToRight)
				{
					this.LeftArrowClick(cellCount);
					return;
				}
				this.RightArrowClick(cellCount);
				return;
			}
			else
			{
				if (!this.layout.leftArrow.Contains(x, y))
				{
					if (this.downLeftArrow)
					{
						this.downLeftArrow = false;
						this.InvalidateRect(this.layout.leftArrow);
					}
					if (this.downRightArrow)
					{
						this.downRightArrow = false;
						this.InvalidateRect(this.layout.rightArrow);
					}
					return;
				}
				this.downLeftArrow = true;
				if (alignToRight)
				{
					this.RightArrowClick(cellCount);
					return;
				}
				this.LeftArrowClick(cellCount);
				return;
			}
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00049D00 File Offset: 0x00047F00
		internal void OnMouseLeave()
		{
			if (this.downLeftArrow)
			{
				this.downLeftArrow = false;
				this.InvalidateRect(this.layout.leftArrow);
			}
			if (this.downRightArrow)
			{
				this.downRightArrow = false;
				this.InvalidateRect(this.layout.rightArrow);
			}
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00049D50 File Offset: 0x00047F50
		internal void OnMouseMove(int x, int y)
		{
			if (this.downLeftArrow)
			{
				this.downLeftArrow = false;
				this.InvalidateRect(this.layout.leftArrow);
			}
			if (this.downRightArrow)
			{
				this.downRightArrow = false;
				this.InvalidateRect(this.layout.rightArrow);
			}
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00049DA0 File Offset: 0x00047FA0
		internal void OnMouseUp(int x, int y)
		{
			this.ResetMouseInfo();
			if (!this.layout.rightArrow.IsEmpty && this.layout.rightArrow.Contains(x, y))
			{
				this.InvalidateRect(this.layout.rightArrow);
				return;
			}
			if (!this.layout.leftArrow.IsEmpty && this.layout.leftArrow.Contains(x, y))
			{
				this.InvalidateRect(this.layout.leftArrow);
				return;
			}
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00049E23 File Offset: 0x00048023
		internal void OnResize(Rectangle oldBounds)
		{
			this.Invalidate();
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00049E2C File Offset: 0x0004802C
		internal void Paint(Graphics g, Rectangle visualbounds, bool alignRight)
		{
			Rectangle bounds = visualbounds;
			if (this.borderWidth > 0)
			{
				this.PaintBorder(g, bounds);
				bounds.Inflate(-this.borderWidth, -this.borderWidth);
			}
			this.PaintParentRows(g, bounds, alignRight);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00049E6C File Offset: 0x0004806C
		private void PaintBorder(Graphics g, Rectangle bounds)
		{
			Rectangle rect = bounds;
			rect.Height = this.borderWidth;
			g.FillRectangle(this.borderBrush, rect);
			rect.Y = bounds.Bottom - this.borderWidth;
			g.FillRectangle(this.borderBrush, rect);
			rect = new Rectangle(bounds.X, bounds.Y + this.borderWidth, this.borderWidth, bounds.Height - 2 * this.borderWidth);
			g.FillRectangle(this.borderBrush, rect);
			rect.X = bounds.Right - this.borderWidth;
			g.FillRectangle(this.borderBrush, rect);
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00049F18 File Offset: 0x00048118
		private int GetTableBoxWidth(Graphics g, Font font)
		{
			Font font2 = font;
			try
			{
				font2 = new Font(font, FontStyle.Bold);
			}
			catch
			{
			}
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				DataGridState dataGridState = (DataGridState)this.parents[i];
				string text = dataGridState.ListManager.GetListName() + " :";
				int val = (int)g.MeasureString(text, font2).Width;
				num = Math.Max(val, num);
			}
			return num;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00049FA0 File Offset: 0x000481A0
		private int GetColBoxWidth(Graphics g, Font font, int colNum)
		{
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				DataGridState dataGridState = (DataGridState)this.parents[i];
				GridColumnStylesCollection gridColumnStyles = dataGridState.GridColumnStyles;
				if (colNum < gridColumnStyles.Count)
				{
					string text = gridColumnStyles[colNum].HeaderText + " :";
					int val = (int)g.MeasureString(text, font).Width;
					num = Math.Max(val, num);
				}
			}
			return num;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0004A01C File Offset: 0x0004821C
		private int GetColDataBoxWidth(Graphics g, int colNum)
		{
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				DataGridState dataGridState = (DataGridState)this.parents[i];
				GridColumnStylesCollection gridColumnStyles = dataGridState.GridColumnStyles;
				if (colNum < gridColumnStyles.Count)
				{
					object columnValueAtRow = gridColumnStyles[colNum].GetColumnValueAtRow((CurrencyManager)this.dataGrid.BindingContext[dataGridState.DataSource, dataGridState.DataMember], dataGridState.LinkingRow.RowNumber);
					int width = gridColumnStyles[colNum].GetPreferredSize(g, columnValueAtRow).Width;
					num = Math.Max(width, num);
				}
			}
			return num;
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0004A0C4 File Offset: 0x000482C4
		private int ColsCount()
		{
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				DataGridState dataGridState = (DataGridState)this.parents[i];
				num = Math.Max(num, dataGridState.GridColumnStyles.Count);
			}
			return num;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0004A10C File Offset: 0x0004830C
		private int TotalWidth(int tableNameBoxWidth, int[] colsNameWidths, int[] colsDataWidths)
		{
			int num = 0;
			num += tableNameBoxWidth;
			for (int i = 0; i < colsNameWidths.Length; i++)
			{
				num += colsNameWidths[i];
				num += colsDataWidths[i];
			}
			return num + 3 * (colsNameWidths.Length - 1);
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0004A144 File Offset: 0x00048344
		private void ComputeLayout(Rectangle bounds, int tableNameBoxWidth, int[] colsNameWidths, int[] colsDataWidths)
		{
			int num = this.TotalWidth(tableNameBoxWidth, colsNameWidths, colsDataWidths);
			if (num > bounds.Width)
			{
				this.layout.leftArrow = new Rectangle(bounds.X, bounds.Y, 15, bounds.Height);
				this.layout.data = new Rectangle(this.layout.leftArrow.Right, bounds.Y, bounds.Width - 30, bounds.Height);
				this.layout.rightArrow = new Rectangle(this.layout.data.Right, bounds.Y, 15, bounds.Height);
				return;
			}
			this.layout.data = bounds;
			this.layout.leftArrow = Rectangle.Empty;
			this.layout.rightArrow = Rectangle.Empty;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0004A228 File Offset: 0x00048428
		private void PaintParentRows(Graphics g, Rectangle bounds, bool alignToRight)
		{
			int tableNameBoxWidth = 0;
			int num = this.ColsCount();
			int[] array = new int[num];
			int[] array2 = new int[num];
			if (this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.TableName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both)
			{
				tableNameBoxWidth = this.GetTableBoxWidth(g, this.dataGrid.Font);
			}
			for (int i = 0; i < num; i++)
			{
				if (this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.ColumnName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both)
				{
					array[i] = this.GetColBoxWidth(g, this.dataGrid.Font, i);
				}
				else
				{
					array[i] = 0;
				}
				array2[i] = this.GetColDataBoxWidth(g, i);
			}
			this.ComputeLayout(bounds, tableNameBoxWidth, array, array2);
			if (!this.layout.leftArrow.IsEmpty)
			{
				g.FillRectangle(this.BackBrush, this.layout.leftArrow);
				this.PaintLeftArrow(g, this.layout.leftArrow, alignToRight);
			}
			Rectangle data = this.layout.data;
			for (int j = 0; j < this.parentsCount; j++)
			{
				data.Height = (int)this.rowHeights[j];
				if (data.Y > bounds.Bottom)
				{
					break;
				}
				int num2 = this.PaintRow(g, data, j, this.dataGrid.Font, alignToRight, tableNameBoxWidth, array, array2);
				if (j == this.parentsCount - 1)
				{
					break;
				}
				g.DrawLine(this.gridLinePen, data.X, data.Bottom, data.X + num2, data.Bottom);
				data.Y += data.Height;
			}
			if (!this.layout.rightArrow.IsEmpty)
			{
				g.FillRectangle(this.BackBrush, this.layout.rightArrow);
				this.PaintRightArrow(g, this.layout.rightArrow, alignToRight);
			}
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x0004A410 File Offset: 0x00048610
		private Bitmap GetBitmap(string bitmapName, Color transparentColor)
		{
			Bitmap bitmap = null;
			try
			{
				bitmap = new Bitmap(typeof(DataGridParentRows), bitmapName);
				bitmap.MakeTransparent(transparentColor);
			}
			catch (Exception ex)
			{
			}
			return bitmap;
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0004A450 File Offset: 0x00048650
		private Bitmap GetRightArrowBitmap()
		{
			if (DataGridParentRows.rightArrow == null)
			{
				DataGridParentRows.rightArrow = this.GetBitmap("DataGridParentRows.RightArrow.bmp", Color.White);
			}
			return DataGridParentRows.rightArrow;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0004A473 File Offset: 0x00048673
		private Bitmap GetLeftArrowBitmap()
		{
			if (DataGridParentRows.leftArrow == null)
			{
				DataGridParentRows.leftArrow = this.GetBitmap("DataGridParentRows.LeftArrow.bmp", Color.White);
			}
			return DataGridParentRows.leftArrow;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0004A498 File Offset: 0x00048698
		private void PaintBitmap(Graphics g, Bitmap b, Rectangle bounds)
		{
			int x = bounds.X + (bounds.Width - b.Width) / 2;
			int y = bounds.Y + (bounds.Height - b.Height) / 2;
			Rectangle rectangle = new Rectangle(x, y, b.Width, b.Height);
			g.FillRectangle(this.BackBrush, rectangle);
			ImageAttributes imageAttributes = new ImageAttributes();
			this.colorMap[0].NewColor = this.ForeColor;
			imageAttributes.SetRemapTable(this.colorMap, ColorAdjustType.Bitmap);
			g.DrawImage(b, rectangle, 0, 0, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, imageAttributes);
			imageAttributes.Dispose();
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0004A540 File Offset: 0x00048740
		private void PaintDownButton(Graphics g, Rectangle bounds)
		{
			g.DrawLine(Pens.Black, bounds.X, bounds.Y, bounds.X + bounds.Width, bounds.Y);
			g.DrawLine(Pens.White, bounds.X + bounds.Width, bounds.Y, bounds.X + bounds.Width, bounds.Y + bounds.Height);
			g.DrawLine(Pens.White, bounds.X + bounds.Width, bounds.Y + bounds.Height, bounds.X, bounds.Y + bounds.Height);
			g.DrawLine(Pens.Black, bounds.X, bounds.Y + bounds.Height, bounds.X, bounds.Y);
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0004A62C File Offset: 0x0004882C
		private void PaintLeftArrow(Graphics g, Rectangle bounds, bool alignToRight)
		{
			Bitmap leftArrowBitmap = this.GetLeftArrowBitmap();
			if (this.downLeftArrow)
			{
				this.PaintDownButton(g, bounds);
				this.layout.leftArrow.Inflate(-1, -1);
				Bitmap obj = leftArrowBitmap;
				lock (obj)
				{
					this.PaintBitmap(g, leftArrowBitmap, bounds);
				}
				this.layout.leftArrow.Inflate(1, 1);
				return;
			}
			Bitmap obj2 = leftArrowBitmap;
			lock (obj2)
			{
				this.PaintBitmap(g, leftArrowBitmap, bounds);
			}
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0004A6D8 File Offset: 0x000488D8
		private void PaintRightArrow(Graphics g, Rectangle bounds, bool alignToRight)
		{
			Bitmap rightArrowBitmap = this.GetRightArrowBitmap();
			if (this.downRightArrow)
			{
				this.PaintDownButton(g, bounds);
				this.layout.rightArrow.Inflate(-1, -1);
				Bitmap obj = rightArrowBitmap;
				lock (obj)
				{
					this.PaintBitmap(g, rightArrowBitmap, bounds);
				}
				this.layout.rightArrow.Inflate(1, 1);
				return;
			}
			Bitmap obj2 = rightArrowBitmap;
			lock (obj2)
			{
				this.PaintBitmap(g, rightArrowBitmap, bounds);
			}
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x0004A784 File Offset: 0x00048984
		private int PaintRow(Graphics g, Rectangle bounds, int row, Font font, bool alignToRight, int tableNameBoxWidth, int[] colsNameWidths, int[] colsDataWidths)
		{
			DataGridState dataGridState = (DataGridState)this.parents[row];
			Rectangle rectangle = bounds;
			Rectangle bounds2 = bounds;
			rectangle.Height = (int)this.rowHeights[row];
			bounds2.Height = (int)this.rowHeights[row];
			int num = 0;
			int num2 = 0;
			if (this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.TableName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both)
			{
				if (num2 < this.horizOffset)
				{
					num2++;
				}
				else
				{
					rectangle.Width = Math.Min(rectangle.Width, tableNameBoxWidth);
					rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
					string text = dataGridState.ListManager.GetListName() + ": ";
					this.PaintText(g, rectangle, text, font, true, alignToRight);
					num += rectangle.Width;
				}
			}
			if (num >= bounds.Width)
			{
				return bounds.Width;
			}
			bounds2.Width -= num;
			bounds2.X += (alignToRight ? 0 : num);
			num += this.PaintColumns(g, bounds2, dataGridState, font, alignToRight, colsNameWidths, colsDataWidths, num2);
			if (num < bounds.Width)
			{
				rectangle.X = bounds.X + num;
				rectangle.Width = bounds.Width - num;
				rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
				g.FillRectangle(this.BackBrush, rectangle);
			}
			return num;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0004A8F8 File Offset: 0x00048AF8
		private int PaintColumns(Graphics g, Rectangle bounds, DataGridState dgs, Font font, bool alignToRight, int[] colsNameWidths, int[] colsDataWidths, int skippedCells)
		{
			Rectangle rectangle = bounds;
			GridColumnStylesCollection gridColumnStyles = dgs.GridColumnStyles;
			int num = 0;
			int num2 = 0;
			while (num2 < gridColumnStyles.Count && num < bounds.Width)
			{
				if ((this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.ColumnName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both) && skippedCells >= this.horizOffset)
				{
					rectangle.X = bounds.X + num;
					rectangle.Width = Math.Min(bounds.Width - num, colsNameWidths[num2]);
					rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
					string text = gridColumnStyles[num2].HeaderText + ": ";
					this.PaintText(g, rectangle, text, font, false, alignToRight);
					num += rectangle.Width;
				}
				if (num >= bounds.Width)
				{
					break;
				}
				if (skippedCells < this.horizOffset)
				{
					skippedCells++;
				}
				else
				{
					rectangle.X = bounds.X + num;
					rectangle.Width = Math.Min(bounds.Width - num, colsDataWidths[num2]);
					rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
					gridColumnStyles[num2].Paint(g, rectangle, (CurrencyManager)this.dataGrid.BindingContext[dgs.DataSource, dgs.DataMember], this.dataGrid.BindingContext[dgs.DataSource, dgs.DataMember].Position, this.BackBrush, this.ForeBrush, alignToRight);
					num += rectangle.Width;
					g.DrawLine(new Pen(SystemColors.ControlDark), alignToRight ? rectangle.X : rectangle.Right, rectangle.Y, alignToRight ? rectangle.X : rectangle.Right, rectangle.Bottom);
					num++;
					if (num2 < gridColumnStyles.Count - 1)
					{
						rectangle.X = bounds.X + num;
						rectangle.Width = Math.Min(bounds.Width - num, 3);
						rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
						g.FillRectangle(this.BackBrush, rectangle);
						num += 3;
					}
				}
				num2++;
			}
			return num;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0004AB34 File Offset: 0x00048D34
		private int PaintText(Graphics g, Rectangle textBounds, string text, Font font, bool bold, bool alignToRight)
		{
			Font font2 = font;
			if (bold)
			{
				try
				{
					font2 = new Font(font, FontStyle.Bold);
					goto IL_18;
				}
				catch
				{
					goto IL_18;
				}
			}
			font2 = font;
			IL_18:
			g.FillRectangle(this.BackBrush, textBounds);
			StringFormat stringFormat = new StringFormat();
			if (alignToRight)
			{
				stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
				stringFormat.Alignment = StringAlignment.Far;
			}
			stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
			textBounds.Offset(0, 2);
			textBounds.Height -= 2;
			g.DrawString(text, font2, this.ForeBrush, textBounds, stringFormat);
			stringFormat.Dispose();
			return textBounds.Width;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0004ABE4 File Offset: 0x00048DE4
		private int MirrorRect(Rectangle surroundingRect, Rectangle containedRect, bool alignToRight)
		{
			if (alignToRight)
			{
				return surroundingRect.Right - containedRect.Right + surroundingRect.X;
			}
			return containedRect.X;
		}

		// Token: 0x040009A2 RID: 2466
		private DataGrid dataGrid;

		// Token: 0x040009A3 RID: 2467
		private SolidBrush backBrush = DataGrid.DefaultParentRowsBackBrush;

		// Token: 0x040009A4 RID: 2468
		private SolidBrush foreBrush = DataGrid.DefaultParentRowsForeBrush;

		// Token: 0x040009A5 RID: 2469
		private int borderWidth = 1;

		// Token: 0x040009A6 RID: 2470
		private Brush borderBrush = new SolidBrush(SystemColors.WindowFrame);

		// Token: 0x040009A7 RID: 2471
		private static Bitmap rightArrow;

		// Token: 0x040009A8 RID: 2472
		private static Bitmap leftArrow;

		// Token: 0x040009A9 RID: 2473
		private ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x040009AA RID: 2474
		private Pen gridLinePen = SystemPens.Control;

		// Token: 0x040009AB RID: 2475
		private int totalHeight;

		// Token: 0x040009AC RID: 2476
		private int textRegionHeight;

		// Token: 0x040009AD RID: 2477
		private DataGridParentRows.Layout layout = new DataGridParentRows.Layout();

		// Token: 0x040009AE RID: 2478
		private bool downLeftArrow;

		// Token: 0x040009AF RID: 2479
		private bool downRightArrow;

		// Token: 0x040009B0 RID: 2480
		private int horizOffset;

		// Token: 0x040009B1 RID: 2481
		private ArrayList parents = new ArrayList();

		// Token: 0x040009B2 RID: 2482
		private int parentsCount;

		// Token: 0x040009B3 RID: 2483
		private ArrayList rowHeights = new ArrayList();

		// Token: 0x040009B4 RID: 2484
		private AccessibleObject accessibleObject;

		// Token: 0x02000593 RID: 1427
		private class Layout
		{
			// Token: 0x06005803 RID: 22531 RVA: 0x00172642 File Offset: 0x00170842
			public Layout()
			{
				this.data = Rectangle.Empty;
				this.leftArrow = Rectangle.Empty;
				this.rightArrow = Rectangle.Empty;
			}

			// Token: 0x06005804 RID: 22532 RVA: 0x0017266C File Offset: 0x0017086C
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder(200);
				stringBuilder.Append("ParentRows Layout: \n");
				stringBuilder.Append("data = ");
				stringBuilder.Append(this.data.ToString());
				stringBuilder.Append("\n leftArrow = ");
				stringBuilder.Append(this.leftArrow.ToString());
				stringBuilder.Append("\n rightArrow = ");
				stringBuilder.Append(this.rightArrow.ToString());
				stringBuilder.Append("\n");
				return stringBuilder.ToString();
			}

			// Token: 0x040038A1 RID: 14497
			public Rectangle data;

			// Token: 0x040038A2 RID: 14498
			public Rectangle leftArrow;

			// Token: 0x040038A3 RID: 14499
			public Rectangle rightArrow;
		}

		// Token: 0x02000594 RID: 1428
		[ComVisible(true)]
		protected internal class DataGridParentRowsAccessibleObject : AccessibleObject
		{
			// Token: 0x06005805 RID: 22533 RVA: 0x0017270E File Offset: 0x0017090E
			public DataGridParentRowsAccessibleObject(DataGridParentRows owner)
			{
				this.owner = owner;
			}

			// Token: 0x17001507 RID: 5383
			// (get) Token: 0x06005806 RID: 22534 RVA: 0x0017271D File Offset: 0x0017091D
			internal DataGridParentRows Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x17001508 RID: 5384
			// (get) Token: 0x06005807 RID: 22535 RVA: 0x00172725 File Offset: 0x00170925
			public override Rectangle Bounds
			{
				get
				{
					return this.owner.dataGrid.RectangleToScreen(this.owner.dataGrid.ParentRowsBounds);
				}
			}

			// Token: 0x17001509 RID: 5385
			// (get) Token: 0x06005808 RID: 22536 RVA: 0x00172747 File Offset: 0x00170947
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("AccDGNavigateBack");
				}
			}

			// Token: 0x1700150A RID: 5386
			// (get) Token: 0x06005809 RID: 22537 RVA: 0x00172753 File Offset: 0x00170953
			public override string Name
			{
				get
				{
					return SR.GetString("AccDGParentRows");
				}
			}

			// Token: 0x1700150B RID: 5387
			// (get) Token: 0x0600580A RID: 22538 RVA: 0x0017275F File Offset: 0x0017095F
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.dataGrid.AccessibilityObject;
				}
			}

			// Token: 0x1700150C RID: 5388
			// (get) Token: 0x0600580B RID: 22539 RVA: 0x00172771 File Offset: 0x00170971
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.List;
				}
			}

			// Token: 0x1700150D RID: 5389
			// (get) Token: 0x0600580C RID: 22540 RVA: 0x00172778 File Offset: 0x00170978
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.ReadOnly;
					if (this.owner.parentsCount == 0)
					{
						accessibleStates |= AccessibleStates.Invisible;
					}
					if (this.owner.dataGrid.ParentRowsVisible)
					{
						accessibleStates |= AccessibleStates.Expanded;
					}
					else
					{
						accessibleStates |= AccessibleStates.Collapsed;
					}
					return accessibleStates;
				}
			}

			// Token: 0x1700150E RID: 5390
			// (get) Token: 0x0600580D RID: 22541 RVA: 0x0000DE5C File Offset: 0x0000C05C
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return null;
				}
			}

			// Token: 0x0600580E RID: 22542 RVA: 0x001727C2 File Offset: 0x001709C2
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.owner.dataGrid.NavigateBack();
			}

			// Token: 0x0600580F RID: 22543 RVA: 0x001727D4 File Offset: 0x001709D4
			public override AccessibleObject GetChild(int index)
			{
				return ((DataGridState)this.owner.parents[index]).ParentRowAccessibleObject;
			}

			// Token: 0x06005810 RID: 22544 RVA: 0x001727F1 File Offset: 0x001709F1
			public override int GetChildCount()
			{
				return this.owner.parentsCount;
			}

			// Token: 0x06005811 RID: 22545 RVA: 0x0000DE5C File Offset: 0x0000C05C
			public override AccessibleObject GetFocused()
			{
				return null;
			}

			// Token: 0x06005812 RID: 22546 RVA: 0x00172800 File Offset: 0x00170A00
			internal AccessibleObject GetNext(AccessibleObject child)
			{
				int childCount = this.GetChildCount();
				bool flag = false;
				for (int i = 0; i < childCount; i++)
				{
					if (flag)
					{
						return this.GetChild(i);
					}
					if (this.GetChild(i) == child)
					{
						flag = true;
					}
				}
				return null;
			}

			// Token: 0x06005813 RID: 22547 RVA: 0x0017283C File Offset: 0x00170A3C
			internal AccessibleObject GetPrev(AccessibleObject child)
			{
				int childCount = this.GetChildCount();
				bool flag = false;
				for (int i = childCount - 1; i >= 0; i--)
				{
					if (flag)
					{
						return this.GetChild(i);
					}
					if (this.GetChild(i) == child)
					{
						flag = true;
					}
				}
				return null;
			}

			// Token: 0x06005814 RID: 22548 RVA: 0x00172878 File Offset: 0x00170A78
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return this.Parent.GetChild(this.GetChildCount() - 1);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return this.Parent.GetChild(1);
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

			// Token: 0x06005815 RID: 22549 RVA: 0x0000701A File Offset: 0x0000521A
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
			}

			// Token: 0x040038A4 RID: 14500
			private DataGridParentRows owner;
		}
	}
}
