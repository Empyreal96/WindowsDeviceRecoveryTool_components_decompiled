using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000176 RID: 374
	internal class DataGridRelationshipRow : DataGridRow
	{
		// Token: 0x060013BD RID: 5053 RVA: 0x00046CB0 File Offset: 0x00044EB0
		public DataGridRelationshipRow(DataGrid dataGrid, DataGridTableStyle dgTable, int rowNumber) : base(dataGrid, dgTable, rowNumber)
		{
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0004AC08 File Offset: 0x00048E08
		protected internal override int MinimumRowHeight(GridColumnStylesCollection cols)
		{
			return base.MinimumRowHeight(cols) + (this.expanded ? this.GetRelationshipRect().Height : 0);
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0004AC38 File Offset: 0x00048E38
		protected internal override int MinimumRowHeight(DataGridTableStyle dgTable)
		{
			return base.MinimumRowHeight(dgTable) + (this.expanded ? this.GetRelationshipRect().Height : 0);
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0004AC66 File Offset: 0x00048E66
		// (set) Token: 0x060013C1 RID: 5057 RVA: 0x0004AC6E File Offset: 0x00048E6E
		public virtual bool Expanded
		{
			get
			{
				return this.expanded;
			}
			set
			{
				if (this.expanded == value)
				{
					return;
				}
				if (this.expanded)
				{
					this.Collapse();
					return;
				}
				this.Expand();
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x0004AC8F File Offset: 0x00048E8F
		// (set) Token: 0x060013C3 RID: 5059 RVA: 0x0004AC9C File Offset: 0x00048E9C
		private int FocusedRelation
		{
			get
			{
				return this.dgTable.FocusedRelation;
			}
			set
			{
				this.dgTable.FocusedRelation = value;
			}
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0004ACAA File Offset: 0x00048EAA
		private void Collapse()
		{
			if (this.expanded)
			{
				this.expanded = false;
				this.FocusedRelation = -1;
				base.DataGrid.OnRowHeightChanged(this);
			}
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0004ACCE File Offset: 0x00048ECE
		protected override AccessibleObject CreateAccessibleObject()
		{
			return new DataGridRelationshipRow.DataGridRelationshipRowAccessibleObject(this);
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x0004ACD8 File Offset: 0x00048ED8
		private void Expand()
		{
			if (!this.expanded && base.DataGrid != null && this.dgTable != null && this.dgTable.RelationsList.Count > 0)
			{
				this.expanded = true;
				this.FocusedRelation = -1;
				base.DataGrid.OnRowHeightChanged(this);
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x0004AD2C File Offset: 0x00048F2C
		// (set) Token: 0x060013C8 RID: 5064 RVA: 0x0004AD5C File Offset: 0x00048F5C
		public override int Height
		{
			get
			{
				int height = base.Height;
				if (this.expanded)
				{
					return height + this.GetRelationshipRect().Height;
				}
				return height;
			}
			set
			{
				if (this.expanded)
				{
					base.Height = value - this.GetRelationshipRect().Height;
					return;
				}
				base.Height = value;
			}
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0004AD90 File Offset: 0x00048F90
		public override Rectangle GetCellBounds(int col)
		{
			Rectangle cellBounds = base.GetCellBounds(col);
			cellBounds.Height = base.Height - 1;
			return cellBounds;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0004ADB8 File Offset: 0x00048FB8
		private Rectangle GetOutlineRect(int xOrigin, int yOrigin)
		{
			Rectangle result = new Rectangle(xOrigin + 2, yOrigin + 2, 9, 9);
			return result;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0004ADD7 File Offset: 0x00048FD7
		public override Rectangle GetNonScrollableArea()
		{
			if (this.expanded)
			{
				return this.GetRelationshipRect();
			}
			return Rectangle.Empty;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0004ADF0 File Offset: 0x00048FF0
		private Rectangle GetRelationshipRect()
		{
			Rectangle relationshipRect = this.dgTable.RelationshipRect;
			relationshipRect.Y = base.Height - this.dgTable.BorderWidth;
			return relationshipRect;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0004AE24 File Offset: 0x00049024
		private Rectangle GetRelationshipRectWithMirroring()
		{
			Rectangle relationshipRect = this.GetRelationshipRect();
			bool flag = this.dgTable.IsDefault ? base.DataGrid.RowHeadersVisible : this.dgTable.RowHeadersVisible;
			if (flag)
			{
				int num = this.dgTable.IsDefault ? base.DataGrid.RowHeaderWidth : this.dgTable.RowHeaderWidth;
				relationshipRect.X += base.DataGrid.GetRowHeaderRect().X + num;
			}
			relationshipRect.X = this.MirrorRelationshipRectangle(relationshipRect, base.DataGrid.GetRowHeaderRect(), base.DataGrid.RightToLeft == RightToLeft.Yes);
			return relationshipRect;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0004AED4 File Offset: 0x000490D4
		private bool PointOverPlusMinusGlyph(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			if (this.dgTable == null || this.dgTable.DataGrid == null || !this.dgTable.DataGrid.AllowNavigation)
			{
				return false;
			}
			Rectangle rect = rowHeaders;
			if (!base.DataGrid.FlatMode)
			{
				rect.Inflate(-1, -1);
			}
			Rectangle outlineRect = this.GetOutlineRect(rect.Right - 14, 0);
			outlineRect.X = this.MirrorRectangle(outlineRect.X, outlineRect.Width, rect, alignToRight);
			return outlineRect.Contains(x, y);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0004AF5C File Offset: 0x0004915C
		public override bool OnMouseDown(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			bool flag = this.dgTable.IsDefault ? base.DataGrid.RowHeadersVisible : this.dgTable.RowHeadersVisible;
			if (flag && this.PointOverPlusMinusGlyph(x, y, rowHeaders, alignToRight))
			{
				if (this.dgTable.RelationsList.Count == 0)
				{
					return false;
				}
				if (this.expanded)
				{
					this.Collapse();
				}
				else
				{
					this.Expand();
				}
				base.DataGrid.OnNodeClick(EventArgs.Empty);
				return true;
			}
			else
			{
				if (!this.expanded)
				{
					return base.OnMouseDown(x, y, rowHeaders, alignToRight);
				}
				if (this.GetRelationshipRectWithMirroring().Contains(x, y))
				{
					int num = this.RelationFromY(y);
					if (num != -1)
					{
						this.FocusedRelation = -1;
						base.DataGrid.NavigateTo((string)this.dgTable.RelationsList[num], this, true);
					}
					return true;
				}
				return base.OnMouseDown(x, y, rowHeaders, alignToRight);
			}
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0004B044 File Offset: 0x00049244
		public override bool OnMouseMove(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			if (!this.expanded)
			{
				return false;
			}
			if (this.GetRelationshipRectWithMirroring().Contains(x, y))
			{
				base.DataGrid.Cursor = Cursors.Hand;
				return true;
			}
			base.DataGrid.Cursor = Cursors.Default;
			return base.OnMouseMove(x, y, rowHeaders, alignToRight);
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0004B09C File Offset: 0x0004929C
		public override void OnMouseLeft(Rectangle rowHeaders, bool alignToRight)
		{
			if (!this.expanded)
			{
				return;
			}
			Rectangle relationshipRect = this.GetRelationshipRect();
			relationshipRect.X += rowHeaders.X + this.dgTable.RowHeaderWidth;
			relationshipRect.X = this.MirrorRelationshipRectangle(relationshipRect, rowHeaders, alignToRight);
			if (this.FocusedRelation != -1)
			{
				this.InvalidateRowRect(relationshipRect);
				this.FocusedRelation = -1;
			}
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0004B101 File Offset: 0x00049301
		public override void OnMouseLeft()
		{
			if (!this.expanded)
			{
				return;
			}
			if (this.FocusedRelation != -1)
			{
				this.InvalidateRow();
				this.FocusedRelation = -1;
			}
			base.OnMouseLeft();
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0004B128 File Offset: 0x00049328
		public override bool OnKeyPress(Keys keyData)
		{
			if ((keyData & Keys.Modifiers) == Keys.Shift && (keyData & Keys.KeyCode) != Keys.Tab)
			{
				return false;
			}
			Keys keys = keyData & Keys.KeyCode;
			if (keys <= Keys.Return)
			{
				if (keys == Keys.Tab)
				{
					return false;
				}
				if (keys == Keys.Return)
				{
					if (this.FocusedRelation != -1)
					{
						base.DataGrid.NavigateTo((string)this.dgTable.RelationsList[this.FocusedRelation], this, true);
						this.FocusedRelation = -1;
						return true;
					}
					return false;
				}
			}
			else if (keys != Keys.F5)
			{
				if (keys == Keys.NumLock)
				{
					return this.FocusedRelation == -1 && base.OnKeyPress(keyData);
				}
			}
			else
			{
				if (this.dgTable == null || this.dgTable.DataGrid == null || !this.dgTable.DataGrid.AllowNavigation)
				{
					return false;
				}
				if (this.expanded)
				{
					this.Collapse();
				}
				else
				{
					this.Expand();
				}
				this.FocusedRelation = -1;
				return true;
			}
			this.FocusedRelation = -1;
			return base.OnKeyPress(keyData);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0004B228 File Offset: 0x00049428
		internal override void LoseChildFocus(Rectangle rowHeaders, bool alignToRight)
		{
			if (this.FocusedRelation == -1 || !this.expanded)
			{
				return;
			}
			this.FocusedRelation = -1;
			Rectangle relationshipRect = this.GetRelationshipRect();
			relationshipRect.X += rowHeaders.X + this.dgTable.RowHeaderWidth;
			relationshipRect.X = this.MirrorRelationshipRectangle(relationshipRect, rowHeaders, alignToRight);
			this.InvalidateRowRect(relationshipRect);
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0004B290 File Offset: 0x00049490
		internal override bool ProcessTabKey(Keys keyData, Rectangle rowHeaders, bool alignToRight)
		{
			if (this.dgTable.RelationsList.Count == 0 || this.dgTable.DataGrid == null || !this.dgTable.DataGrid.AllowNavigation)
			{
				return false;
			}
			if (!this.expanded)
			{
				this.Expand();
			}
			if ((keyData & Keys.Shift) == Keys.Shift)
			{
				if (this.FocusedRelation == 0)
				{
					this.FocusedRelation = -1;
					return false;
				}
				Rectangle relationshipRect = this.GetRelationshipRect();
				relationshipRect.X += rowHeaders.X + this.dgTable.RowHeaderWidth;
				relationshipRect.X = this.MirrorRelationshipRectangle(relationshipRect, rowHeaders, alignToRight);
				this.InvalidateRowRect(relationshipRect);
				if (this.FocusedRelation == -1)
				{
					this.FocusedRelation = this.dgTable.RelationsList.Count - 1;
				}
				else
				{
					int focusedRelation = this.FocusedRelation;
					this.FocusedRelation = focusedRelation - 1;
				}
				return true;
			}
			else
			{
				if (this.FocusedRelation == this.dgTable.RelationsList.Count - 1)
				{
					this.FocusedRelation = -1;
					return false;
				}
				Rectangle relationshipRect2 = this.GetRelationshipRect();
				relationshipRect2.X += rowHeaders.X + this.dgTable.RowHeaderWidth;
				relationshipRect2.X = this.MirrorRelationshipRectangle(relationshipRect2, rowHeaders, alignToRight);
				this.InvalidateRowRect(relationshipRect2);
				int focusedRelation = this.FocusedRelation;
				this.FocusedRelation = focusedRelation + 1;
				return true;
			}
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00046CF2 File Offset: 0x00044EF2
		public override int Paint(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int firstVisibleColumn, int numVisibleColumns)
		{
			return this.Paint(g, bounds, trueRowBounds, firstVisibleColumn, numVisibleColumns, false);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0004B3E8 File Offset: 0x000495E8
		public override int Paint(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int firstVisibleColumn, int numVisibleColumns, bool alignToRight)
		{
			bool traceVerbose = CompModSwitches.DGRelationShpRowPaint.TraceVerbose;
			int borderWidth = this.dgTable.BorderWidth;
			Rectangle bounds2 = bounds;
			bounds2.Height = base.Height - borderWidth;
			int num = this.PaintData(g, bounds2, firstVisibleColumn, numVisibleColumns, alignToRight);
			int dataWidth = num + bounds.X - trueRowBounds.X;
			bounds2.Offset(0, borderWidth);
			if (borderWidth > 0)
			{
				this.PaintBottomBorder(g, bounds2, num, borderWidth, alignToRight);
			}
			if (this.expanded && this.dgTable.RelationsList.Count > 0)
			{
				Rectangle bounds3 = new Rectangle(trueRowBounds.X, bounds2.Bottom, trueRowBounds.Width, trueRowBounds.Height - bounds2.Height - 2 * borderWidth);
				this.PaintRelations(g, bounds3, trueRowBounds, dataWidth, firstVisibleColumn, numVisibleColumns, alignToRight);
				bounds3.Height++;
				if (borderWidth > 0)
				{
					this.PaintBottomBorder(g, bounds3, dataWidth, borderWidth, alignToRight);
				}
			}
			return num;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0004B4D4 File Offset: 0x000496D4
		protected override void PaintCellContents(Graphics g, Rectangle cellBounds, DataGridColumnStyle column, Brush backBr, Brush foreBrush, bool alignToRight)
		{
			CurrencyManager listManager = base.DataGrid.ListManager;
			string text = string.Empty;
			Rectangle rectangle = cellBounds;
			object obj = base.DataGrid.ListManager[this.number];
			if (obj is IDataErrorInfo)
			{
				text = ((IDataErrorInfo)obj)[column.PropertyDescriptor.Name];
			}
			if (!string.IsNullOrEmpty(text))
			{
				Bitmap errorBitmap = base.GetErrorBitmap();
				Bitmap obj2 = errorBitmap;
				Rectangle iconBounds;
				lock (obj2)
				{
					iconBounds = base.PaintIcon(g, rectangle, true, alignToRight, errorBitmap, backBr);
				}
				if (alignToRight)
				{
					rectangle.Width -= iconBounds.Width + 3;
				}
				else
				{
					rectangle.X += iconBounds.Width + 3;
				}
				DataGridToolTip toolTipProvider = base.DataGrid.ToolTipProvider;
				string toolTipString = text;
				DataGrid dataGrid = base.DataGrid;
				int toolTipId = dataGrid.ToolTipId;
				dataGrid.ToolTipId = toolTipId + 1;
				toolTipProvider.AddToolTip(toolTipString, (IntPtr)toolTipId, iconBounds);
			}
			column.Paint(g, rectangle, listManager, base.RowNumber, backBr, foreBrush, alignToRight);
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0004B5F8 File Offset: 0x000497F8
		public override void PaintHeader(Graphics g, Rectangle bounds, bool alignToRight, bool isDirty)
		{
			DataGrid dataGrid = base.DataGrid;
			Rectangle rectangle = bounds;
			if (!dataGrid.FlatMode)
			{
				ControlPaint.DrawBorder3D(g, rectangle, Border3DStyle.RaisedInner);
				rectangle.Inflate(-1, -1);
			}
			if (this.dgTable.IsDefault)
			{
				this.PaintHeaderInside(g, rectangle, base.DataGrid.HeaderBackBrush, alignToRight, isDirty);
				return;
			}
			this.PaintHeaderInside(g, rectangle, this.dgTable.HeaderBackBrush, alignToRight, isDirty);
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0004B664 File Offset: 0x00049864
		public void PaintHeaderInside(Graphics g, Rectangle bounds, Brush backBr, bool alignToRight, bool isDirty)
		{
			bool flag = this.dgTable.RelationsList.Count > 0 && this.dgTable.DataGrid.AllowNavigation;
			int x = this.MirrorRectangle(bounds.X, bounds.Width - (flag ? 14 : 0), bounds, alignToRight);
			Rectangle visualBounds = new Rectangle(x, bounds.Y, bounds.Width - (flag ? 14 : 0), bounds.Height);
			base.PaintHeader(g, visualBounds, alignToRight, isDirty);
			int x2 = this.MirrorRectangle(bounds.X + visualBounds.Width, 14, bounds, alignToRight);
			Rectangle bounds2 = new Rectangle(x2, bounds.Y, 14, bounds.Height);
			if (flag)
			{
				this.PaintPlusMinusGlyph(g, bounds2, backBr, alignToRight);
			}
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0004B734 File Offset: 0x00049934
		private void PaintRelations(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int dataWidth, int firstCol, int nCols, bool alignToRight)
		{
			Rectangle relationshipRect = this.GetRelationshipRect();
			relationshipRect.X = (alignToRight ? (bounds.Right - relationshipRect.Width) : bounds.X);
			relationshipRect.Y = bounds.Y;
			int num = Math.Max(dataWidth, relationshipRect.Width);
			Region clip = g.Clip;
			g.ExcludeClip(relationshipRect);
			g.FillRectangle(base.GetBackBrush(), alignToRight ? (bounds.Right - dataWidth) : bounds.X, bounds.Y, dataWidth, bounds.Height);
			g.SetClip(bounds);
			relationshipRect.Height -= this.dgTable.BorderWidth;
			g.DrawRectangle(SystemPens.ControlText, relationshipRect.X, relationshipRect.Y, relationshipRect.Width - 1, relationshipRect.Height - 1);
			relationshipRect.Inflate(-1, -1);
			int num2 = this.PaintRelationText(g, relationshipRect, alignToRight);
			if (num2 < relationshipRect.Height)
			{
				g.FillRectangle(base.GetBackBrush(), relationshipRect.X, relationshipRect.Y + num2, relationshipRect.Width, relationshipRect.Height - num2);
			}
			g.Clip = clip;
			if (num < bounds.Width)
			{
				int gridLineWidth;
				if (this.dgTable.IsDefault)
				{
					gridLineWidth = base.DataGrid.GridLineWidth;
				}
				else
				{
					gridLineWidth = this.dgTable.GridLineWidth;
				}
				g.FillRectangle(base.DataGrid.BackgroundBrush, alignToRight ? bounds.X : (bounds.X + num), bounds.Y, bounds.Width - num - gridLineWidth + 1, bounds.Height);
				if (gridLineWidth > 0)
				{
					Brush gridLineBrush;
					if (this.dgTable.IsDefault)
					{
						gridLineBrush = base.DataGrid.GridLineBrush;
					}
					else
					{
						gridLineBrush = this.dgTable.GridLineBrush;
					}
					g.FillRectangle(gridLineBrush, alignToRight ? (bounds.Right - gridLineWidth - num) : (bounds.X + num - gridLineWidth), bounds.Y, gridLineWidth, bounds.Height);
				}
			}
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0004B944 File Offset: 0x00049B44
		private int PaintRelationText(Graphics g, Rectangle bounds, bool alignToRight)
		{
			g.FillRectangle(base.GetBackBrush(), bounds.X, bounds.Y, bounds.Width, 1);
			int relationshipHeight = this.dgTable.RelationshipHeight;
			Rectangle rectangle = new Rectangle(bounds.X, bounds.Y + 1, bounds.Width, relationshipHeight);
			int num = 1;
			int num2 = 0;
			while (num2 < this.dgTable.RelationsList.Count && num <= bounds.Height)
			{
				Brush brush = this.dgTable.IsDefault ? base.DataGrid.LinkBrush : this.dgTable.LinkBrush;
				Font font = base.DataGrid.Font;
				Brush brush2 = this.dgTable.IsDefault ? base.DataGrid.LinkBrush : this.dgTable.LinkBrush;
				font = base.DataGrid.LinkFont;
				g.FillRectangle(base.GetBackBrush(), rectangle);
				StringFormat stringFormat = new StringFormat();
				if (alignToRight)
				{
					stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
					stringFormat.Alignment = StringAlignment.Far;
				}
				g.DrawString((string)this.dgTable.RelationsList[num2], font, brush2, rectangle, stringFormat);
				if (num2 == this.FocusedRelation && this.number == base.DataGrid.CurrentCell.RowNumber)
				{
					rectangle.Width = this.dgTable.FocusedTextWidth;
					ControlPaint.DrawFocusRectangle(g, rectangle, ((SolidBrush)brush2).Color, ((SolidBrush)base.GetBackBrush()).Color);
					rectangle.Width = bounds.Width;
				}
				stringFormat.Dispose();
				rectangle.Y += relationshipHeight;
				num += rectangle.Height;
				num2++;
			}
			return num;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0004BB18 File Offset: 0x00049D18
		private void PaintPlusMinusGlyph(Graphics g, Rectangle bounds, Brush backBr, bool alignToRight)
		{
			bool traceVerbose = CompModSwitches.DGRelationShpRowPaint.TraceVerbose;
			Rectangle b = this.GetOutlineRect(bounds.X, bounds.Y);
			b = Rectangle.Intersect(bounds, b);
			if (b.IsEmpty)
			{
				return;
			}
			g.FillRectangle(backBr, bounds);
			bool traceVerbose2 = CompModSwitches.DGRelationShpRowPaint.TraceVerbose;
			Pen pen = this.dgTable.IsDefault ? base.DataGrid.HeaderForePen : this.dgTable.HeaderForePen;
			g.DrawRectangle(pen, b.X, b.Y, b.Width - 1, b.Height - 1);
			int num = 2;
			g.DrawLine(pen, b.X + num, b.Y + b.Width / 2, b.Right - num - 1, b.Y + b.Width / 2);
			if (!this.expanded)
			{
				g.DrawLine(pen, b.X + b.Height / 2, b.Y + num, b.X + b.Height / 2, b.Bottom - num - 1);
				return;
			}
			Point[] array = new Point[3];
			array[0] = new Point(b.X + b.Height / 2, b.Bottom);
			array[1] = new Point(array[0].X, bounds.Y + 2 * num + base.Height);
			array[2] = new Point(alignToRight ? bounds.X : bounds.Right, array[1].Y);
			g.DrawLines(pen, array);
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0004BCC8 File Offset: 0x00049EC8
		private int RelationFromY(int y)
		{
			int num = -1;
			int relationshipHeight = this.dgTable.RelationshipHeight;
			Rectangle relationshipRect = this.GetRelationshipRect();
			int num2 = base.Height - this.dgTable.BorderWidth + 1;
			while (num2 < relationshipRect.Bottom && num2 <= y)
			{
				num2 += relationshipHeight;
				num++;
			}
			if (num >= this.dgTable.RelationsList.Count)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0004BD2D File Offset: 0x00049F2D
		private int MirrorRelationshipRectangle(Rectangle relRect, Rectangle rowHeader, bool alignToRight)
		{
			if (alignToRight)
			{
				return rowHeader.X - relRect.Width;
			}
			return relRect.X;
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0004BD49 File Offset: 0x00049F49
		private int MirrorRectangle(int x, int width, Rectangle rect, bool alignToRight)
		{
			if (alignToRight)
			{
				return rect.Right + rect.X - width - x;
			}
			return x;
		}

		// Token: 0x040009BA RID: 2490
		private const bool defaultOpen = false;

		// Token: 0x040009BB RID: 2491
		private const int expandoBoxWidth = 14;

		// Token: 0x040009BC RID: 2492
		private const int indentWidth = 20;

		// Token: 0x040009BD RID: 2493
		private const int triangleSize = 5;

		// Token: 0x040009BE RID: 2494
		private bool expanded;

		// Token: 0x02000595 RID: 1429
		[ComVisible(true)]
		protected class DataGridRelationshipRowAccessibleObject : DataGridRow.DataGridRowAccessibleObject
		{
			// Token: 0x06005816 RID: 22550 RVA: 0x001728FA File Offset: 0x00170AFA
			public DataGridRelationshipRowAccessibleObject(DataGridRow owner) : base(owner)
			{
			}

			// Token: 0x06005817 RID: 22551 RVA: 0x00172904 File Offset: 0x00170B04
			protected override void AddChildAccessibleObjects(IList children)
			{
				base.AddChildAccessibleObjects(children);
				DataGridRelationshipRow dataGridRelationshipRow = (DataGridRelationshipRow)base.Owner;
				if (dataGridRelationshipRow.dgTable.RelationsList != null)
				{
					for (int i = 0; i < dataGridRelationshipRow.dgTable.RelationsList.Count; i++)
					{
						children.Add(new DataGridRelationshipRow.DataGridRelationshipAccessibleObject(dataGridRelationshipRow, i));
					}
				}
			}

			// Token: 0x1700150F RID: 5391
			// (get) Token: 0x06005818 RID: 22552 RVA: 0x0017295A File Offset: 0x00170B5A
			private DataGridRelationshipRow RelationshipRow
			{
				get
				{
					return (DataGridRelationshipRow)base.Owner;
				}
			}

			// Token: 0x17001510 RID: 5392
			// (get) Token: 0x06005819 RID: 22553 RVA: 0x00172967 File Offset: 0x00170B67
			public override string DefaultAction
			{
				get
				{
					if (this.RelationshipRow.dgTable.RelationsList.Count <= 0)
					{
						return null;
					}
					if (this.RelationshipRow.Expanded)
					{
						return SR.GetString("AccDGCollapse");
					}
					return SR.GetString("AccDGExpand");
				}
			}

			// Token: 0x17001511 RID: 5393
			// (get) Token: 0x0600581A RID: 22554 RVA: 0x001729A8 File Offset: 0x00170BA8
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = base.State;
					if (this.RelationshipRow.dgTable.RelationsList.Count > 0)
					{
						if (((DataGridRelationshipRow)base.Owner).Expanded)
						{
							accessibleStates |= AccessibleStates.Expanded;
						}
						else
						{
							accessibleStates |= AccessibleStates.Collapsed;
						}
					}
					return accessibleStates;
				}
			}

			// Token: 0x0600581B RID: 22555 RVA: 0x001729F9 File Offset: 0x00170BF9
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				if (this.RelationshipRow.dgTable.RelationsList.Count > 0)
				{
					((DataGridRelationshipRow)base.Owner).Expanded = !((DataGridRelationshipRow)base.Owner).Expanded;
				}
			}

			// Token: 0x0600581C RID: 22556 RVA: 0x00172A38 File Offset: 0x00170C38
			public override AccessibleObject GetFocused()
			{
				DataGridRelationshipRow dataGridRelationshipRow = (DataGridRelationshipRow)base.Owner;
				int focusedRelation = dataGridRelationshipRow.dgTable.FocusedRelation;
				if (focusedRelation == -1)
				{
					return base.GetFocused();
				}
				return this.GetChild(this.GetChildCount() - dataGridRelationshipRow.dgTable.RelationsList.Count + focusedRelation);
			}
		}

		// Token: 0x02000596 RID: 1430
		[ComVisible(true)]
		protected class DataGridRelationshipAccessibleObject : AccessibleObject
		{
			// Token: 0x0600581D RID: 22557 RVA: 0x00172A87 File Offset: 0x00170C87
			public DataGridRelationshipAccessibleObject(DataGridRelationshipRow owner, int relationship)
			{
				this.owner = owner;
				this.relationship = relationship;
			}

			// Token: 0x17001512 RID: 5394
			// (get) Token: 0x0600581E RID: 22558 RVA: 0x00172AA0 File Offset: 0x00170CA0
			public override Rectangle Bounds
			{
				get
				{
					Rectangle rowBounds = this.DataGrid.GetRowBounds(this.owner);
					Rectangle r = this.owner.Expanded ? this.owner.GetRelationshipRectWithMirroring() : Rectangle.Empty;
					r.Y += this.owner.dgTable.RelationshipHeight * this.relationship;
					r.Height = (this.owner.Expanded ? this.owner.dgTable.RelationshipHeight : 0);
					if (!this.owner.Expanded)
					{
						r.X += rowBounds.X;
					}
					r.Y += rowBounds.Y;
					return this.owner.DataGrid.RectangleToScreen(r);
				}
			}

			// Token: 0x17001513 RID: 5395
			// (get) Token: 0x0600581F RID: 22559 RVA: 0x00172B73 File Offset: 0x00170D73
			public override string Name
			{
				get
				{
					return (string)this.owner.dgTable.RelationsList[this.relationship];
				}
			}

			// Token: 0x17001514 RID: 5396
			// (get) Token: 0x06005820 RID: 22560 RVA: 0x00172B95 File Offset: 0x00170D95
			protected DataGridRelationshipRow Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x17001515 RID: 5397
			// (get) Token: 0x06005821 RID: 22561 RVA: 0x00172B9D File Offset: 0x00170D9D
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.AccessibleObject;
				}
			}

			// Token: 0x17001516 RID: 5398
			// (get) Token: 0x06005822 RID: 22562 RVA: 0x00172BAA File Offset: 0x00170DAA
			protected DataGrid DataGrid
			{
				get
				{
					return this.owner.DataGrid;
				}
			}

			// Token: 0x17001517 RID: 5399
			// (get) Token: 0x06005823 RID: 22563 RVA: 0x00172BB7 File Offset: 0x00170DB7
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Link;
				}
			}

			// Token: 0x17001518 RID: 5400
			// (get) Token: 0x06005824 RID: 22564 RVA: 0x00172BBC File Offset: 0x00170DBC
			public override AccessibleStates State
			{
				get
				{
					DataGridRow[] dataGridRows = this.DataGrid.DataGridRows;
					if (Array.IndexOf<DataGridRow>(dataGridRows, this.owner) == -1)
					{
						return AccessibleStates.Unavailable;
					}
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable | AccessibleStates.Linked;
					if (!this.owner.Expanded)
					{
						accessibleStates |= AccessibleStates.Invisible;
					}
					if (this.DataGrid.Focused && this.Owner.dgTable.FocusedRelation == this.relationship)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17001519 RID: 5401
			// (get) Token: 0x06005825 RID: 22565 RVA: 0x00172C2C File Offset: 0x00170E2C
			// (set) Token: 0x06005826 RID: 22566 RVA: 0x0000701A File Offset: 0x0000521A
			public override string Value
			{
				get
				{
					DataGridRow[] dataGridRows = this.DataGrid.DataGridRows;
					if (Array.IndexOf<DataGridRow>(dataGridRows, this.owner) == -1)
					{
						return null;
					}
					return (string)this.owner.dgTable.RelationsList[this.relationship];
				}
				set
				{
				}
			}

			// Token: 0x1700151A RID: 5402
			// (get) Token: 0x06005827 RID: 22567 RVA: 0x00172C76 File Offset: 0x00170E76
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("AccDGNavigate");
				}
			}

			// Token: 0x06005828 RID: 22568 RVA: 0x00172C84 File Offset: 0x00170E84
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.Owner.Expanded = true;
				this.owner.FocusedRelation = -1;
				this.DataGrid.NavigateTo((string)this.owner.dgTable.RelationsList[this.relationship], this.owner, true);
				this.DataGrid.BeginInvoke(new MethodInvoker(this.ResetAccessibilityLayer));
			}

			// Token: 0x06005829 RID: 22569 RVA: 0x00172CF4 File Offset: 0x00170EF4
			private void ResetAccessibilityLayer()
			{
				((DataGrid.DataGridAccessibleObject)this.DataGrid.AccessibilityObject).NotifyClients(AccessibleEvents.Reorder, 0);
				((DataGrid.DataGridAccessibleObject)this.DataGrid.AccessibilityObject).NotifyClients(AccessibleEvents.Focus, this.DataGrid.CurrentCellAccIndex);
				((DataGrid.DataGridAccessibleObject)this.DataGrid.AccessibilityObject).NotifyClients(AccessibleEvents.Selection, this.DataGrid.CurrentCellAccIndex);
			}

			// Token: 0x0600582A RID: 22570 RVA: 0x00172D68 File Offset: 0x00170F68
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					if (this.relationship > 0)
					{
						return this.Parent.GetChild(this.Parent.GetChildCount() - this.owner.dgTable.RelationsList.Count + this.relationship - 1);
					}
					break;
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					if (this.relationship + 1 < this.owner.dgTable.RelationsList.Count)
					{
						return this.Parent.GetChild(this.Parent.GetChildCount() - this.owner.dgTable.RelationsList.Count + this.relationship + 1);
					}
					break;
				}
				return null;
			}

			// Token: 0x0600582B RID: 22571 RVA: 0x00172E2F File Offset: 0x0017102F
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					this.DataGrid.Focus();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.Owner.FocusedRelation = this.relationship;
				}
			}

			// Token: 0x040038A5 RID: 14501
			private DataGridRelationshipRow owner;

			// Token: 0x040038A6 RID: 14502
			private int relationship;
		}
	}
}
