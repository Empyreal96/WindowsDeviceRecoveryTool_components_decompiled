using System;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020006EA RID: 1770
	internal class RowInfo
	{
		// Token: 0x060071E8 RID: 29160 RVA: 0x002094A3 File Offset: 0x002076A3
		public RowInfo()
		{
			this._rowSize = new Size(0.0, 0.0);
		}

		// Token: 0x060071E9 RID: 29161 RVA: 0x002094C8 File Offset: 0x002076C8
		public void AddPage(Size pageSize)
		{
			this._pageCount++;
			this._rowSize.Width = this._rowSize.Width + pageSize.Width;
			this._rowSize.Height = Math.Max(pageSize.Height, this._rowSize.Height);
		}

		// Token: 0x060071EA RID: 29162 RVA: 0x0020951E File Offset: 0x0020771E
		public void ClearPages()
		{
			this._pageCount = 0;
			this._rowSize.Width = 0.0;
			this._rowSize.Height = 0.0;
		}

		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x060071EB RID: 29163 RVA: 0x0020954F File Offset: 0x0020774F
		public Size RowSize
		{
			get
			{
				return this._rowSize;
			}
		}

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x060071EC RID: 29164 RVA: 0x00209557 File Offset: 0x00207757
		// (set) Token: 0x060071ED RID: 29165 RVA: 0x0020955F File Offset: 0x0020775F
		public double VerticalOffset
		{
			get
			{
				return this._verticalOffset;
			}
			set
			{
				this._verticalOffset = value;
			}
		}

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x060071EE RID: 29166 RVA: 0x00209568 File Offset: 0x00207768
		// (set) Token: 0x060071EF RID: 29167 RVA: 0x00209570 File Offset: 0x00207770
		public int FirstPage
		{
			get
			{
				return this._firstPage;
			}
			set
			{
				this._firstPage = value;
			}
		}

		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x060071F0 RID: 29168 RVA: 0x00209579 File Offset: 0x00207779
		public int PageCount
		{
			get
			{
				return this._pageCount;
			}
		}

		// Token: 0x0400374B RID: 14155
		private Size _rowSize;

		// Token: 0x0400374C RID: 14156
		private double _verticalOffset;

		// Token: 0x0400374D RID: 14157
		private int _firstPage;

		// Token: 0x0400374E RID: 14158
		private int _pageCount;
	}
}
