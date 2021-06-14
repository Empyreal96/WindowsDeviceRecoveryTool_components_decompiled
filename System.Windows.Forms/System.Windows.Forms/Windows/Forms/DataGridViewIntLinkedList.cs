using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x020001F1 RID: 497
	internal class DataGridViewIntLinkedList : IEnumerable
	{
		// Token: 0x06001E19 RID: 7705 RVA: 0x000959DF File Offset: 0x00093BDF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DataGridViewIntLinkedListEnumerator(this.headElement);
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000959EC File Offset: 0x00093BEC
		public DataGridViewIntLinkedList()
		{
			this.lastAccessedIndex = -1;
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000959FC File Offset: 0x00093BFC
		public DataGridViewIntLinkedList(DataGridViewIntLinkedList source)
		{
			int num = source.Count;
			for (int i = 0; i < num; i++)
			{
				this.Add(source[i]);
			}
		}

		// Token: 0x1700070C RID: 1804
		public int this[int index]
		{
			get
			{
				if (this.lastAccessedIndex == -1 || index < this.lastAccessedIndex)
				{
					DataGridViewIntLinkedListElement next = this.headElement;
					for (int i = index; i > 0; i--)
					{
						next = next.Next;
					}
					this.lastAccessedElement = next;
					this.lastAccessedIndex = index;
					return next.Int;
				}
				while (this.lastAccessedIndex < index)
				{
					this.lastAccessedElement = this.lastAccessedElement.Next;
					this.lastAccessedIndex++;
				}
				return this.lastAccessedElement.Int;
			}
			set
			{
				if (index != this.lastAccessedIndex)
				{
					int num = this[index];
				}
				this.lastAccessedElement.Int = value;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06001E1E RID: 7710 RVA: 0x00095ADE File Offset: 0x00093CDE
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x00095AE6 File Offset: 0x00093CE6
		public int HeadInt
		{
			get
			{
				return this.headElement.Int;
			}
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x00095AF4 File Offset: 0x00093CF4
		public void Add(int integer)
		{
			DataGridViewIntLinkedListElement dataGridViewIntLinkedListElement = new DataGridViewIntLinkedListElement(integer);
			if (this.headElement != null)
			{
				dataGridViewIntLinkedListElement.Next = this.headElement;
			}
			this.headElement = dataGridViewIntLinkedListElement;
			this.count++;
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x00095B3F File Offset: 0x00093D3F
		public void Clear()
		{
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
			this.headElement = null;
			this.count = 0;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x00095B60 File Offset: 0x00093D60
		public bool Contains(int integer)
		{
			int num = 0;
			DataGridViewIntLinkedListElement next = this.headElement;
			while (next != null)
			{
				if (next.Int == integer)
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

		// Token: 0x06001E23 RID: 7715 RVA: 0x00095BA0 File Offset: 0x00093DA0
		public int IndexOf(int integer)
		{
			if (this.Contains(integer))
			{
				return this.lastAccessedIndex;
			}
			return -1;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x00095BB4 File Offset: 0x00093DB4
		public bool Remove(int integer)
		{
			DataGridViewIntLinkedListElement dataGridViewIntLinkedListElement = null;
			DataGridViewIntLinkedListElement next = this.headElement;
			while (next != null && next.Int != integer)
			{
				dataGridViewIntLinkedListElement = next;
				next = next.Next;
			}
			if (next.Int == integer)
			{
				DataGridViewIntLinkedListElement next2 = next.Next;
				if (dataGridViewIntLinkedListElement == null)
				{
					this.headElement = next2;
				}
				else
				{
					dataGridViewIntLinkedListElement.Next = next2;
				}
				this.count--;
				this.lastAccessedElement = null;
				this.lastAccessedIndex = -1;
				return true;
			}
			return false;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x00095C24 File Offset: 0x00093E24
		public void RemoveAt(int index)
		{
			DataGridViewIntLinkedListElement dataGridViewIntLinkedListElement = null;
			DataGridViewIntLinkedListElement next = this.headElement;
			while (index > 0)
			{
				dataGridViewIntLinkedListElement = next;
				next = next.Next;
				index--;
			}
			DataGridViewIntLinkedListElement next2 = next.Next;
			if (dataGridViewIntLinkedListElement == null)
			{
				this.headElement = next2;
			}
			else
			{
				dataGridViewIntLinkedListElement.Next = next2;
			}
			this.count--;
			this.lastAccessedElement = null;
			this.lastAccessedIndex = -1;
		}

		// Token: 0x04000D3F RID: 3391
		private DataGridViewIntLinkedListElement lastAccessedElement;

		// Token: 0x04000D40 RID: 3392
		private DataGridViewIntLinkedListElement headElement;

		// Token: 0x04000D41 RID: 3393
		private int count;

		// Token: 0x04000D42 RID: 3394
		private int lastAccessedIndex;
	}
}
