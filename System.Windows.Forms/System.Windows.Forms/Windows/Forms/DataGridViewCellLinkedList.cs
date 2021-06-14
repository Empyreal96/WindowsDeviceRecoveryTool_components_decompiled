using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x0200019B RID: 411
	internal class DataGridViewCellLinkedList : IEnumerable
	{
		// Token: 0x06001AF0 RID: 6896 RVA: 0x00086DF2 File Offset: 0x00084FF2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DataGridViewCellLinkedListEnumerator(this.headElement);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x00086DFF File Offset: 0x00084FFF
		public DataGridViewCellLinkedList()
		{
			this.lastAccessedIndex = -1;
		}

		// Token: 0x1700061E RID: 1566
		public DataGridViewCell this[int index]
		{
			get
			{
				if (this.lastAccessedIndex == -1 || index < this.lastAccessedIndex)
				{
					DataGridViewCellLinkedListElement next = this.headElement;
					for (int i = index; i > 0; i--)
					{
						next = next.Next;
					}
					this.lastAccessedElement = next;
					this.lastAccessedIndex = index;
					return next.DataGridViewCell;
				}
				while (this.lastAccessedIndex < index)
				{
					this.lastAccessedElement = this.lastAccessedElement.Next;
					this.lastAccessedIndex++;
				}
				return this.lastAccessedElement.DataGridViewCell;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00086E91 File Offset: 0x00085091
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x00086E99 File Offset: 0x00085099
		public DataGridViewCell HeadCell
		{
			get
			{
				return this.headElement.DataGridViewCell;
			}
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00086EA8 File Offset: 0x000850A8
		public void Add(DataGridViewCell dataGridViewCell)
		{
			DataGridViewCellLinkedListElement dataGridViewCellLinkedListElement = new DataGridViewCellLinkedListElement(dataGridViewCell);
			if (this.headElement != null)
			{
				dataGridViewCellLinkedListElement.Next = this.headElement;
			}
			this.headElement = dataGridViewCellLinkedListElement;
			this.count++;
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x00086EF3 File Offset: 0x000850F3
		public void Clear()
		{
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
			this.headElement = null;
			this.count = 0;
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x00086F14 File Offset: 0x00085114
		public bool Contains(DataGridViewCell dataGridViewCell)
		{
			int num = 0;
			DataGridViewCellLinkedListElement next = this.headElement;
			while (next != null)
			{
				if (next.DataGridViewCell == dataGridViewCell)
				{
					this.lastAccessedElement = next;
					this.lastAccessedIndex = num;
					return true;
				}
				next = next.Next;
				num++;
			}
			return false;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x00086F54 File Offset: 0x00085154
		public bool Remove(DataGridViewCell dataGridViewCell)
		{
			DataGridViewCellLinkedListElement dataGridViewCellLinkedListElement = null;
			DataGridViewCellLinkedListElement next = this.headElement;
			while (next != null && next.DataGridViewCell != dataGridViewCell)
			{
				dataGridViewCellLinkedListElement = next;
				next = next.Next;
			}
			if (next.DataGridViewCell == dataGridViewCell)
			{
				DataGridViewCellLinkedListElement next2 = next.Next;
				if (dataGridViewCellLinkedListElement == null)
				{
					this.headElement = next2;
				}
				else
				{
					dataGridViewCellLinkedListElement.Next = next2;
				}
				this.count--;
				this.lastAccessedElement = null;
				this.lastAccessedIndex = -1;
				return true;
			}
			return false;
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x00086FC4 File Offset: 0x000851C4
		public int RemoveAllCellsAtBand(bool column, int bandIndex)
		{
			int num = 0;
			DataGridViewCellLinkedListElement dataGridViewCellLinkedListElement = null;
			DataGridViewCellLinkedListElement dataGridViewCellLinkedListElement2 = this.headElement;
			while (dataGridViewCellLinkedListElement2 != null)
			{
				if ((column && dataGridViewCellLinkedListElement2.DataGridViewCell.ColumnIndex == bandIndex) || (!column && dataGridViewCellLinkedListElement2.DataGridViewCell.RowIndex == bandIndex))
				{
					DataGridViewCellLinkedListElement next = dataGridViewCellLinkedListElement2.Next;
					if (dataGridViewCellLinkedListElement == null)
					{
						this.headElement = next;
					}
					else
					{
						dataGridViewCellLinkedListElement.Next = next;
					}
					dataGridViewCellLinkedListElement2 = next;
					this.count--;
					this.lastAccessedElement = null;
					this.lastAccessedIndex = -1;
					num++;
				}
				else
				{
					dataGridViewCellLinkedListElement = dataGridViewCellLinkedListElement2;
					dataGridViewCellLinkedListElement2 = dataGridViewCellLinkedListElement2.Next;
				}
			}
			return num;
		}

		// Token: 0x04000C0F RID: 3087
		private DataGridViewCellLinkedListElement lastAccessedElement;

		// Token: 0x04000C10 RID: 3088
		private DataGridViewCellLinkedListElement headElement;

		// Token: 0x04000C11 RID: 3089
		private int count;

		// Token: 0x04000C12 RID: 3090
		private int lastAccessedIndex;
	}
}
