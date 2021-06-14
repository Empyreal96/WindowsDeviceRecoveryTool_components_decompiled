using System;
using System.Collections.Generic;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020006E9 RID: 1769
	internal class RowCache
	{
		// Token: 0x060071C2 RID: 29122 RVA: 0x002081A0 File Offset: 0x002063A0
		public RowCache()
		{
			this._rowCache = new List<RowInfo>(this._defaultRowCacheSize);
		}

		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x060071C4 RID: 29124 RVA: 0x0020828D File Offset: 0x0020648D
		// (set) Token: 0x060071C3 RID: 29123 RVA: 0x002081F4 File Offset: 0x002063F4
		public PageCache PageCache
		{
			get
			{
				return this._pageCache;
			}
			set
			{
				this._rowCache.Clear();
				this._isLayoutCompleted = false;
				this._isLayoutRequested = false;
				if (this._pageCache != null)
				{
					this._pageCache.PageCacheChanged -= this.OnPageCacheChanged;
					this._pageCache.PaginationCompleted -= this.OnPaginationCompleted;
				}
				this._pageCache = value;
				if (this._pageCache != null)
				{
					this._pageCache.PageCacheChanged += this.OnPageCacheChanged;
					this._pageCache.PaginationCompleted += this.OnPaginationCompleted;
				}
			}
		}

		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x060071C5 RID: 29125 RVA: 0x00208295 File Offset: 0x00206495
		public int RowCount
		{
			get
			{
				return this._rowCache.Count;
			}
		}

		// Token: 0x17001B11 RID: 6929
		// (get) Token: 0x060071C7 RID: 29127 RVA: 0x002082D1 File Offset: 0x002064D1
		// (set) Token: 0x060071C6 RID: 29126 RVA: 0x002082A2 File Offset: 0x002064A2
		public double VerticalPageSpacing
		{
			get
			{
				return this._verticalPageSpacing;
			}
			set
			{
				if (value < 0.0)
				{
					value = 0.0;
				}
				if (value != this._verticalPageSpacing)
				{
					this._verticalPageSpacing = value;
					this.RecalcLayoutForScaleOrSpacing();
				}
			}
		}

		// Token: 0x17001B12 RID: 6930
		// (get) Token: 0x060071C9 RID: 29129 RVA: 0x00208308 File Offset: 0x00206508
		// (set) Token: 0x060071C8 RID: 29128 RVA: 0x002082D9 File Offset: 0x002064D9
		public double HorizontalPageSpacing
		{
			get
			{
				return this._horizontalPageSpacing;
			}
			set
			{
				if (value < 0.0)
				{
					value = 0.0;
				}
				if (value != this._horizontalPageSpacing)
				{
					this._horizontalPageSpacing = value;
					this.RecalcLayoutForScaleOrSpacing();
				}
			}
		}

		// Token: 0x17001B13 RID: 6931
		// (get) Token: 0x060071CB RID: 29131 RVA: 0x00208328 File Offset: 0x00206528
		// (set) Token: 0x060071CA RID: 29130 RVA: 0x00208310 File Offset: 0x00206510
		public double Scale
		{
			get
			{
				return this._scale;
			}
			set
			{
				if (this._scale != value)
				{
					this._scale = value;
					this.RecalcLayoutForScaleOrSpacing();
				}
			}
		}

		// Token: 0x17001B14 RID: 6932
		// (get) Token: 0x060071CC RID: 29132 RVA: 0x00208330 File Offset: 0x00206530
		public double ExtentHeight
		{
			get
			{
				return this._extentHeight;
			}
		}

		// Token: 0x17001B15 RID: 6933
		// (get) Token: 0x060071CD RID: 29133 RVA: 0x00208338 File Offset: 0x00206538
		public double ExtentWidth
		{
			get
			{
				return this._extentWidth;
			}
		}

		// Token: 0x17001B16 RID: 6934
		// (get) Token: 0x060071CE RID: 29134 RVA: 0x00208340 File Offset: 0x00206540
		public bool HasValidLayout
		{
			get
			{
				return this._hasValidLayout;
			}
		}

		// Token: 0x1400014D RID: 333
		// (add) Token: 0x060071CF RID: 29135 RVA: 0x00208348 File Offset: 0x00206548
		// (remove) Token: 0x060071D0 RID: 29136 RVA: 0x00208380 File Offset: 0x00206580
		public event RowCacheChangedEventHandler RowCacheChanged;

		// Token: 0x1400014E RID: 334
		// (add) Token: 0x060071D1 RID: 29137 RVA: 0x002083B8 File Offset: 0x002065B8
		// (remove) Token: 0x060071D2 RID: 29138 RVA: 0x002083F0 File Offset: 0x002065F0
		public event RowLayoutCompletedEventHandler RowLayoutCompleted;

		// Token: 0x060071D3 RID: 29139 RVA: 0x00208425 File Offset: 0x00206625
		public RowInfo GetRow(int index)
		{
			if (index < 0 || index > this._rowCache.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._rowCache[index];
		}

		// Token: 0x060071D4 RID: 29140 RVA: 0x00208450 File Offset: 0x00206650
		public RowInfo GetRowForPageNumber(int pageNumber)
		{
			if (pageNumber < 0 || pageNumber > this.LastPageInCache)
			{
				throw new ArgumentOutOfRangeException("pageNumber");
			}
			return this._rowCache[this.GetRowIndexForPageNumber(pageNumber)];
		}

		// Token: 0x060071D5 RID: 29141 RVA: 0x0020847C File Offset: 0x0020667C
		public int GetRowIndexForPageNumber(int pageNumber)
		{
			if (pageNumber < 0 || pageNumber > this.LastPageInCache)
			{
				throw new ArgumentOutOfRangeException("pageNumber");
			}
			for (int i = 0; i < this._rowCache.Count; i++)
			{
				RowInfo rowInfo = this._rowCache[i];
				if (pageNumber >= rowInfo.FirstPage && pageNumber < rowInfo.FirstPage + rowInfo.PageCount)
				{
					return i;
				}
			}
			throw new InvalidOperationException(SR.Get("RowCachePageNotFound"));
		}

		// Token: 0x060071D6 RID: 29142 RVA: 0x002084F0 File Offset: 0x002066F0
		public int GetRowIndexForVerticalOffset(double offset)
		{
			if (offset < 0.0 || offset > this.ExtentHeight)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (this._rowCache.Count == 0)
			{
				return 0;
			}
			double num = Math.Round(offset, this._findOffsetPrecision);
			int i = 0;
			while (i < this._rowCache.Count)
			{
				double num2 = Math.Round(this._rowCache[i].VerticalOffset, this._findOffsetPrecision);
				double num3 = Math.Round(this._rowCache[i].RowSize.Height, this._findOffsetPrecision);
				bool flag = false;
				if (DoubleUtil.AreClose(num2, num2 + num3))
				{
					flag = true;
				}
				if (flag && DoubleUtil.AreClose(num, num2))
				{
					return i;
				}
				if (num >= num2 && num < num2 + num3)
				{
					if (this.WithinVisibleDelta(num2 + num3, num) || i == this._rowCache.Count - 1)
					{
						return i;
					}
					return i + 1;
				}
				else
				{
					i++;
				}
			}
			DoubleUtil.AreClose(offset, this.ExtentHeight);
			return this._rowCache.Count - 1;
		}

		// Token: 0x060071D7 RID: 29143 RVA: 0x00208600 File Offset: 0x00206800
		public void GetVisibleRowIndices(double startOffset, double endOffset, out int startRowIndex, out int rowCount)
		{
			startRowIndex = 0;
			rowCount = 0;
			if (endOffset < startOffset)
			{
				throw new ArgumentOutOfRangeException("endOffset");
			}
			if (startOffset < 0.0 || startOffset > this.ExtentHeight)
			{
				return;
			}
			if (this._rowCache.Count == 0)
			{
				return;
			}
			startRowIndex = this.GetRowIndexForVerticalOffset(startOffset);
			rowCount = 1;
			startOffset = Math.Round(startOffset, this._findOffsetPrecision);
			endOffset = Math.Round(endOffset, this._findOffsetPrecision);
			for (int i = startRowIndex + 1; i < this._rowCache.Count; i++)
			{
				double num = Math.Round(this._rowCache[i].VerticalOffset, this._findOffsetPrecision);
				if (num >= endOffset || !this.WithinVisibleDelta(endOffset, num))
				{
					break;
				}
				rowCount++;
			}
		}

		// Token: 0x060071D8 RID: 29144 RVA: 0x002086BC File Offset: 0x002068BC
		public void RecalcLayoutForScaleOrSpacing()
		{
			if (this.PageCache == null)
			{
				throw new InvalidOperationException(SR.Get("RowCacheRecalcWithNoPageCache"));
			}
			this._extentWidth = 0.0;
			this._extentHeight = 0.0;
			double num = 0.0;
			for (int i = 0; i < this._rowCache.Count; i++)
			{
				RowInfo rowInfo = this._rowCache[i];
				int pageCount = rowInfo.PageCount;
				rowInfo.ClearPages();
				rowInfo.VerticalOffset = num;
				for (int j = rowInfo.FirstPage; j < rowInfo.FirstPage + pageCount; j++)
				{
					Size scaledPageSize = this.GetScaledPageSize(j);
					rowInfo.AddPage(scaledPageSize);
				}
				this._extentWidth = Math.Max(rowInfo.RowSize.Width, this._extentWidth);
				num += rowInfo.RowSize.Height;
				this._extentHeight += rowInfo.RowSize.Height;
				this._rowCache[i] = rowInfo;
			}
			RowCacheChangedEventArgs e = new RowCacheChangedEventArgs(new List<RowCacheChange>(1)
			{
				new RowCacheChange(0, this._rowCache.Count)
			});
			this.RowCacheChanged(this, e);
		}

		// Token: 0x060071D9 RID: 29145 RVA: 0x00208810 File Offset: 0x00206A10
		public void RecalcRows(int pivotPage, int columns)
		{
			if (this.PageCache == null)
			{
				throw new InvalidOperationException(SR.Get("RowCacheRecalcWithNoPageCache"));
			}
			if (pivotPage < 0 || pivotPage > this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("pivotPage");
			}
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			this._layoutColumns = columns;
			this._layoutPivotPage = pivotPage;
			this._hasValidLayout = false;
			if (this.PageCache.PageCount < this._layoutColumns)
			{
				if (!this.PageCache.IsPaginationCompleted || this.PageCache.PageCount == 0)
				{
					this._isLayoutRequested = true;
					this._isLayoutCompleted = false;
					return;
				}
				this._layoutColumns = Math.Min(this._layoutColumns, this.PageCache.PageCount);
				this._layoutColumns = Math.Max(1, this._layoutColumns);
				this._layoutPivotPage = 0;
			}
			this._extentHeight = 0.0;
			this._extentWidth = 0.0;
			if (this.PageCache.DynamicPageSizes)
			{
				this._pivotRowIndex = this.RecalcRowsForDynamicPageSizes(this._layoutPivotPage, this._layoutColumns);
			}
			else
			{
				this._pivotRowIndex = this.RecalcRowsForFixedPageSizes(this._layoutPivotPage, this._layoutColumns);
			}
			this._isLayoutCompleted = true;
			this._isLayoutRequested = false;
			this._hasValidLayout = true;
			RowLayoutCompletedEventArgs e = new RowLayoutCompletedEventArgs(this._pivotRowIndex);
			this.RowLayoutCompleted(this, e);
			RowCacheChangedEventArgs e2 = new RowCacheChangedEventArgs(new List<RowCacheChange>(1)
			{
				new RowCacheChange(0, this._rowCache.Count)
			});
			this.RowCacheChanged(this, e2);
		}

		// Token: 0x17001B17 RID: 6935
		// (get) Token: 0x060071DA RID: 29146 RVA: 0x002089A4 File Offset: 0x00206BA4
		private int LastPageInCache
		{
			get
			{
				if (this._rowCache.Count == 0)
				{
					return -1;
				}
				RowInfo rowInfo = this._rowCache[this._rowCache.Count - 1];
				return rowInfo.FirstPage + rowInfo.PageCount - 1;
			}
		}

		// Token: 0x060071DB RID: 29147 RVA: 0x002089E8 File Offset: 0x00206BE8
		private bool WithinVisibleDelta(double offset1, double offset2)
		{
			return offset1 - offset2 > this._visibleDelta;
		}

		// Token: 0x060071DC RID: 29148 RVA: 0x002089F8 File Offset: 0x00206BF8
		private int RecalcRowsForDynamicPageSizes(int pivotPage, int columns)
		{
			if (pivotPage < 0 || pivotPage >= this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("pivotPage");
			}
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			if (pivotPage + columns > this.PageCache.PageCount)
			{
				pivotPage = Math.Max(0, this.PageCache.PageCount - columns);
			}
			this._rowCache.Clear();
			RowInfo rowInfo = this.CreateFixedRow(pivotPage, columns);
			double width = rowInfo.RowSize.Width;
			List<RowInfo> list = new List<RowInfo>(pivotPage / columns);
			int i = pivotPage;
			while (i > 0)
			{
				RowInfo rowInfo2 = this.CreateDynamicRow(i - 1, width, false);
				i = rowInfo2.FirstPage;
				list.Add(rowInfo2);
			}
			for (int j = list.Count - 1; j >= 0; j--)
			{
				this.AddRow(list[j]);
			}
			int count = this._rowCache.Count;
			this.AddRow(rowInfo);
			i = pivotPage + columns;
			while (i < this.PageCache.PageCount)
			{
				RowInfo rowInfo3 = this.CreateDynamicRow(i, width, true);
				i += rowInfo3.PageCount;
				this.AddRow(rowInfo3);
			}
			return count;
		}

		// Token: 0x060071DD RID: 29149 RVA: 0x00208B20 File Offset: 0x00206D20
		private RowInfo CreateDynamicRow(int startPage, double rowWidth, bool createForward)
		{
			if (startPage >= this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("startPage");
			}
			RowInfo rowInfo = new RowInfo();
			Size scaledPageSize = this.GetScaledPageSize(startPage);
			rowInfo.AddPage(scaledPageSize);
			do
			{
				if (createForward)
				{
					scaledPageSize = this.GetScaledPageSize(startPage + rowInfo.PageCount);
					if (startPage + rowInfo.PageCount >= this.PageCache.PageCount)
					{
						break;
					}
					if (rowInfo.RowSize.Width + scaledPageSize.Width > rowWidth)
					{
						break;
					}
				}
				else
				{
					scaledPageSize = this.GetScaledPageSize(startPage - rowInfo.PageCount);
					if (startPage - rowInfo.PageCount < 0 || rowInfo.RowSize.Width + scaledPageSize.Width > rowWidth)
					{
						break;
					}
				}
				rowInfo.AddPage(scaledPageSize);
			}
			while (rowInfo.PageCount != DocumentViewerConstants.MaximumMaxPagesAcross);
			if (!createForward)
			{
				rowInfo.FirstPage = startPage - (rowInfo.PageCount - 1);
			}
			else
			{
				rowInfo.FirstPage = startPage;
			}
			return rowInfo;
		}

		// Token: 0x060071DE RID: 29150 RVA: 0x00208C04 File Offset: 0x00206E04
		private int RecalcRowsForFixedPageSizes(int startPage, int columns)
		{
			if (startPage < 0 || startPage > this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("startPage");
			}
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			this._rowCache.Clear();
			for (int i = 0; i < this.PageCache.PageCount; i += columns)
			{
				RowInfo newRow = this.CreateFixedRow(i, columns);
				this.AddRow(newRow);
			}
			return this.GetRowIndexForPageNumber(startPage);
		}

		// Token: 0x060071DF RID: 29151 RVA: 0x00208C78 File Offset: 0x00206E78
		private RowInfo CreateFixedRow(int startPage, int columns)
		{
			if (startPage >= this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("startPage");
			}
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			RowInfo rowInfo = new RowInfo();
			rowInfo.FirstPage = startPage;
			int num = startPage;
			while (num < startPage + columns && num <= this.PageCache.PageCount - 1)
			{
				Size scaledPageSize = this.GetScaledPageSize(num);
				rowInfo.AddPage(scaledPageSize);
				num++;
			}
			return rowInfo;
		}

		// Token: 0x060071E0 RID: 29152 RVA: 0x00208CE8 File Offset: 0x00206EE8
		private RowCacheChange AddPageRange(int startPage, int count)
		{
			if (!this._isLayoutCompleted)
			{
				throw new InvalidOperationException(SR.Get("RowCacheCannotModifyNonExistentLayout"));
			}
			int i = startPage;
			int num = startPage + count;
			int num2 = 0;
			if (startPage > this.LastPageInCache + 1)
			{
				i = this.LastPageInCache + 1;
			}
			RowInfo rowInfo = this._rowCache[this._rowCache.Count - 1];
			Size scaledPageSize = this.GetScaledPageSize(i);
			RowInfo row = this.GetRow(this._pivotRowIndex);
			bool flag = false;
			while (i < num && rowInfo.RowSize.Width + scaledPageSize.Width <= row.RowSize.Width)
			{
				rowInfo.AddPage(scaledPageSize);
				i++;
				scaledPageSize = this.GetScaledPageSize(i);
				flag = true;
			}
			int num3;
			if (flag)
			{
				num3 = this._rowCache.Count - 1;
				this.UpdateRow(num3, rowInfo);
			}
			else
			{
				num3 = this._rowCache.Count;
			}
			while (i < num)
			{
				RowInfo rowInfo2 = new RowInfo();
				rowInfo2.FirstPage = i;
				do
				{
					scaledPageSize = this.GetScaledPageSize(i);
					rowInfo2.AddPage(scaledPageSize);
					i++;
				}
				while (rowInfo2.RowSize.Width + scaledPageSize.Width <= row.RowSize.Width && i < num);
				this.AddRow(rowInfo2);
				num2++;
			}
			return new RowCacheChange(num3, num2);
		}

		// Token: 0x060071E1 RID: 29153 RVA: 0x00208E40 File Offset: 0x00207040
		private void AddRow(RowInfo newRow)
		{
			if (this._rowCache.Count == 0)
			{
				newRow.VerticalOffset = 0.0;
				this._extentWidth = newRow.RowSize.Width;
			}
			else
			{
				RowInfo rowInfo = this._rowCache[this._rowCache.Count - 1];
				newRow.VerticalOffset = rowInfo.VerticalOffset + rowInfo.RowSize.Height;
				this._extentWidth = Math.Max(newRow.RowSize.Width, this._extentWidth);
			}
			this._extentHeight += newRow.RowSize.Height;
			this._rowCache.Add(newRow);
		}

		// Token: 0x060071E2 RID: 29154 RVA: 0x00208EFC File Offset: 0x002070FC
		private RowCacheChange UpdatePageRange(int startPage, int count)
		{
			if (!this._isLayoutCompleted)
			{
				throw new InvalidOperationException(SR.Get("RowCacheCannotModifyNonExistentLayout"));
			}
			int rowIndexForPageNumber = this.GetRowIndexForPageNumber(startPage);
			int num = rowIndexForPageNumber;
			int num2 = startPage;
			while (num2 < startPage + count && num < this._rowCache.Count)
			{
				RowInfo rowInfo = this._rowCache[num];
				RowInfo rowInfo2 = new RowInfo();
				rowInfo2.VerticalOffset = rowInfo.VerticalOffset;
				rowInfo2.FirstPage = rowInfo.FirstPage;
				for (int i = rowInfo.FirstPage; i < rowInfo.FirstPage + rowInfo.PageCount; i++)
				{
					Size scaledPageSize = this.GetScaledPageSize(i);
					rowInfo2.AddPage(scaledPageSize);
				}
				this.UpdateRow(num, rowInfo2);
				num2 = rowInfo2.FirstPage + rowInfo2.PageCount;
				num++;
			}
			return new RowCacheChange(rowIndexForPageNumber, num - rowIndexForPageNumber);
		}

		// Token: 0x060071E3 RID: 29155 RVA: 0x00208FD0 File Offset: 0x002071D0
		private void UpdateRow(int index, RowInfo newRow)
		{
			if (!this._isLayoutCompleted)
			{
				throw new InvalidOperationException(SR.Get("RowCacheCannotModifyNonExistentLayout"));
			}
			if (index > this._rowCache.Count)
			{
				return;
			}
			RowInfo rowInfo = this._rowCache[index];
			this._rowCache[index] = newRow;
			if (rowInfo.RowSize.Height != newRow.RowSize.Height)
			{
				double num = newRow.RowSize.Height - rowInfo.RowSize.Height;
				for (int i = index + 1; i < this._rowCache.Count; i++)
				{
					RowInfo rowInfo2 = this._rowCache[i];
					rowInfo2.VerticalOffset += num;
					this._rowCache[i] = rowInfo2;
				}
				this._extentHeight += num;
			}
			if (newRow.RowSize.Width > this._extentWidth)
			{
				this._extentWidth = newRow.RowSize.Width;
				return;
			}
			if (rowInfo.RowSize.Width != newRow.RowSize.Width)
			{
				this._extentWidth = 0.0;
				for (int j = 0; j < this._rowCache.Count; j++)
				{
					RowInfo rowInfo3 = this._rowCache[j];
					this._extentWidth = Math.Max(rowInfo3.RowSize.Width, this._extentWidth);
				}
			}
		}

		// Token: 0x060071E4 RID: 29156 RVA: 0x0020914C File Offset: 0x0020734C
		private RowCacheChange TrimPageRange(int startPage)
		{
			int num = this.GetRowIndexForPageNumber(startPage);
			RowInfo row = this.GetRow(num);
			if (row.FirstPage < startPage)
			{
				RowInfo rowInfo = new RowInfo();
				rowInfo.VerticalOffset = row.VerticalOffset;
				rowInfo.FirstPage = row.FirstPage;
				for (int i = row.FirstPage; i < startPage; i++)
				{
					Size scaledPageSize = this.GetScaledPageSize(i);
					rowInfo.AddPage(scaledPageSize);
				}
				this.UpdateRow(num, rowInfo);
				num++;
			}
			int count = this._rowCache.Count - num;
			if (num < this._rowCache.Count)
			{
				this._rowCache.RemoveRange(num, count);
			}
			this._extentHeight = row.VerticalOffset;
			return new RowCacheChange(num, count);
		}

		// Token: 0x060071E5 RID: 29157 RVA: 0x00209200 File Offset: 0x00207400
		private Size GetScaledPageSize(int pageNumber)
		{
			Size pageSize = this.PageCache.GetPageSize(pageNumber);
			if (pageSize.IsEmpty)
			{
				pageSize = new Size(0.0, 0.0);
			}
			pageSize.Width *= this.Scale;
			pageSize.Height *= this.Scale;
			pageSize.Width += this.HorizontalPageSpacing;
			pageSize.Height += this.VerticalPageSpacing;
			return pageSize;
		}

		// Token: 0x060071E6 RID: 29158 RVA: 0x00209290 File Offset: 0x00207490
		private void OnPageCacheChanged(object sender, PageCacheChangedEventArgs args)
		{
			if (this._isLayoutCompleted)
			{
				List<RowCacheChange> list = new List<RowCacheChange>(args.Changes.Count);
				for (int i = 0; i < args.Changes.Count; i++)
				{
					PageCacheChange pageCacheChange = args.Changes[i];
					switch (pageCacheChange.Type)
					{
					case PageCacheChangeType.Add:
					case PageCacheChangeType.Update:
						if (pageCacheChange.Start > this.LastPageInCache)
						{
							RowCacheChange rowCacheChange = this.AddPageRange(pageCacheChange.Start, pageCacheChange.Count);
							if (rowCacheChange != null)
							{
								list.Add(rowCacheChange);
							}
						}
						else if (pageCacheChange.Start + pageCacheChange.Count - 1 <= this.LastPageInCache)
						{
							RowCacheChange rowCacheChange2 = this.UpdatePageRange(pageCacheChange.Start, pageCacheChange.Count);
							if (rowCacheChange2 != null)
							{
								list.Add(rowCacheChange2);
							}
						}
						else
						{
							RowCacheChange rowCacheChange3 = this.UpdatePageRange(pageCacheChange.Start, this.LastPageInCache - pageCacheChange.Start);
							if (rowCacheChange3 != null)
							{
								list.Add(rowCacheChange3);
							}
							rowCacheChange3 = this.AddPageRange(this.LastPageInCache + 1, pageCacheChange.Count - (this.LastPageInCache - pageCacheChange.Start));
							if (rowCacheChange3 != null)
							{
								list.Add(rowCacheChange3);
							}
						}
						break;
					case PageCacheChangeType.Remove:
						if (this.PageCache.PageCount - 1 < this.LastPageInCache)
						{
							RowCacheChange rowCacheChange4 = this.TrimPageRange(this.PageCache.PageCount);
							if (rowCacheChange4 != null)
							{
								list.Add(rowCacheChange4);
							}
						}
						if (this._rowCache.Count <= 1 && (this._rowCache.Count == 0 || this._rowCache[0].PageCount < this._layoutColumns))
						{
							this.RecalcRows(0, this._layoutColumns);
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("args");
					}
				}
				RowCacheChangedEventArgs e = new RowCacheChangedEventArgs(list);
				this.RowCacheChanged(this, e);
				return;
			}
			if (this._isLayoutRequested)
			{
				this.RecalcRows(this._layoutPivotPage, this._layoutColumns);
			}
		}

		// Token: 0x060071E7 RID: 29159 RVA: 0x00209487 File Offset: 0x00207687
		private void OnPaginationCompleted(object sender, EventArgs args)
		{
			if (this._isLayoutRequested)
			{
				this.RecalcRows(this._layoutPivotPage, this._layoutColumns);
			}
		}

		// Token: 0x0400373B RID: 14139
		private List<RowInfo> _rowCache;

		// Token: 0x0400373C RID: 14140
		private int _layoutPivotPage;

		// Token: 0x0400373D RID: 14141
		private int _layoutColumns;

		// Token: 0x0400373E RID: 14142
		private int _pivotRowIndex;

		// Token: 0x0400373F RID: 14143
		private PageCache _pageCache;

		// Token: 0x04003740 RID: 14144
		private bool _isLayoutRequested;

		// Token: 0x04003741 RID: 14145
		private bool _isLayoutCompleted;

		// Token: 0x04003742 RID: 14146
		private double _verticalPageSpacing;

		// Token: 0x04003743 RID: 14147
		private double _horizontalPageSpacing;

		// Token: 0x04003744 RID: 14148
		private double _scale = 1.0;

		// Token: 0x04003745 RID: 14149
		private double _extentHeight;

		// Token: 0x04003746 RID: 14150
		private double _extentWidth;

		// Token: 0x04003747 RID: 14151
		private bool _hasValidLayout;

		// Token: 0x04003748 RID: 14152
		private readonly int _defaultRowCacheSize = 32;

		// Token: 0x04003749 RID: 14153
		private readonly int _findOffsetPrecision = 2;

		// Token: 0x0400374A RID: 14154
		private readonly double _visibleDelta = 0.5;
	}
}
