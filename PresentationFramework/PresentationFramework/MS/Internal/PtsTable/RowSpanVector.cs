using System;
using System.Windows.Documents;

namespace MS.Internal.PtsTable
{
	// Token: 0x0200060B RID: 1547
	internal sealed class RowSpanVector
	{
		// Token: 0x0600671C RID: 26396 RVA: 0x001CD8EC File Offset: 0x001CBAEC
		internal RowSpanVector()
		{
			this._entries = new RowSpanVector.Entry[8];
			this._entries[0].Cell = null;
			this._entries[0].Start = 1073741823;
			this._entries[0].Range = 1073741823;
			this._entries[0].Ttl = int.MaxValue;
			this._size = 1;
		}

		// Token: 0x0600671D RID: 26397 RVA: 0x001CD968 File Offset: 0x001CBB68
		internal void Register(TableCell cell)
		{
			int columnIndex = cell.ColumnIndex;
			if (this._size == this._entries.Length)
			{
				this.InflateCapacity();
			}
			for (int i = this._size - 1; i >= this._index; i--)
			{
				this._entries[i + 1] = this._entries[i];
			}
			this._entries[this._index].Cell = cell;
			this._entries[this._index].Start = columnIndex;
			this._entries[this._index].Range = cell.ColumnSpan;
			this._entries[this._index].Ttl = cell.RowSpan - 1;
			this._size++;
			this._index++;
		}

		// Token: 0x0600671E RID: 26398 RVA: 0x001CDA48 File Offset: 0x001CBC48
		internal void GetFirstAvailableRange(out int firstAvailableIndex, out int firstOccupiedIndex)
		{
			this._index = 0;
			firstAvailableIndex = 0;
			firstOccupiedIndex = this._entries[this._index].Start;
		}

		// Token: 0x0600671F RID: 26399 RVA: 0x001CDA6C File Offset: 0x001CBC6C
		internal void GetNextAvailableRange(out int firstAvailableIndex, out int firstOccupiedIndex)
		{
			firstAvailableIndex = this._entries[this._index].Start + this._entries[this._index].Range;
			RowSpanVector.Entry[] entries = this._entries;
			int index = this._index;
			entries[index].Ttl = entries[index].Ttl - 1;
			this._index++;
			firstOccupiedIndex = this._entries[this._index].Start;
		}

		// Token: 0x06006720 RID: 26400 RVA: 0x001CDAEC File Offset: 0x001CBCEC
		internal void GetSpanCells(out TableCell[] cells, out bool isLastRowOfAnySpan)
		{
			cells = RowSpanVector.s_noCells;
			isLastRowOfAnySpan = false;
			while (this._index < this._size)
			{
				RowSpanVector.Entry[] entries = this._entries;
				int index = this._index;
				entries[index].Ttl = entries[index].Ttl - 1;
				this._index++;
			}
			if (this._size > 1)
			{
				cells = new TableCell[this._size - 1];
				int num = 0;
				int num2 = 0;
				do
				{
					cells[num] = this._entries[num].Cell;
					if (this._entries[num].Ttl > 0)
					{
						if (num != num2)
						{
							this._entries[num2] = this._entries[num];
						}
						num2++;
					}
					num++;
				}
				while (num < this._size - 1);
				if (num != num2)
				{
					this._entries[num2] = this._entries[num];
					isLastRowOfAnySpan = true;
				}
				this._size = num2 + 1;
			}
		}

		// Token: 0x06006721 RID: 26401 RVA: 0x001CDBDB File Offset: 0x001CBDDB
		internal bool Empty()
		{
			return this._size == 1;
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x001CDBE8 File Offset: 0x001CBDE8
		private void InflateCapacity()
		{
			RowSpanVector.Entry[] array = new RowSpanVector.Entry[this._entries.Length * 2];
			Array.Copy(this._entries, array, this._entries.Length);
			this._entries = array;
		}

		// Token: 0x0400334B RID: 13131
		private RowSpanVector.Entry[] _entries;

		// Token: 0x0400334C RID: 13132
		private int _size;

		// Token: 0x0400334D RID: 13133
		private int _index;

		// Token: 0x0400334E RID: 13134
		private const int c_defaultCapacity = 8;

		// Token: 0x0400334F RID: 13135
		private static TableCell[] s_noCells = new TableCell[0];

		// Token: 0x02000A1D RID: 2589
		private struct Entry
		{
			// Token: 0x040046F7 RID: 18167
			internal TableCell Cell;

			// Token: 0x040046F8 RID: 18168
			internal int Start;

			// Token: 0x040046F9 RID: 18169
			internal int Range;

			// Token: 0x040046FA RID: 18170
			internal int Ttl;
		}
	}
}
