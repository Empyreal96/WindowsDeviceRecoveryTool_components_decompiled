using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Data.Services.Client
{
	// Token: 0x020000E1 RID: 225
	[DebuggerDisplay("Count = {count}")]
	internal struct ArraySet<T> : IEnumerable<!0>, IEnumerable where T : class
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x0001F50C File Offset: 0x0001D70C
		public ArraySet(int capacity)
		{
			this.items = new T[capacity];
			this.count = 0;
			this.version = 0;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001F528 File Offset: 0x0001D728
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170001A9 RID: 425
		public T this[int index]
		{
			get
			{
				return this.items[index];
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001F540 File Offset: 0x0001D740
		public bool Add(T item, Func<T, T, bool> equalityComparer)
		{
			if (equalityComparer != null && this.Contains(item, equalityComparer))
			{
				return false;
			}
			int num = this.count++;
			if (this.items == null || num == this.items.Length)
			{
				Array.Resize<T>(ref this.items, Math.Min(Math.Max(num, 16), 1073741823) * 2);
			}
			this.items[num] = item;
			this.version++;
			return true;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001F5BC File Offset: 0x0001D7BC
		public bool Contains(T item, Func<T, T, bool> equalityComparer)
		{
			return 0 <= this.IndexOf(item, equalityComparer);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001F67C File Offset: 0x0001D87C
		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < this.count; i++)
			{
				yield return this.items[i];
			}
			yield break;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001F69D File Offset: 0x0001D89D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001F6A5 File Offset: 0x0001D8A5
		public int IndexOf(T item, Func<T, T, bool> comparer)
		{
			return this.IndexOf<T>(item, new Func<T, T>(ArraySet<T>.IdentitySelect), comparer);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001F6BC File Offset: 0x0001D8BC
		public int IndexOf<K>(K item, Func<T, K> select, Func<K, K, bool> comparer)
		{
			T[] array = this.items;
			if (array != null)
			{
				int num = this.count;
				for (int i = 0; i < num; i++)
				{
					if (comparer(item, select(array[i])))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001F700 File Offset: 0x0001D900
		public T Remove(T item, Func<T, T, bool> equalityComparer)
		{
			int num = this.IndexOf(item, equalityComparer);
			if (0 <= num)
			{
				item = this.items[num];
				this.RemoveAt(num);
				return item;
			}
			return default(T);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001F73C File Offset: 0x0001D93C
		public void RemoveAt(int index)
		{
			T[] array = this.items;
			int num = --this.count;
			array[index] = array[num];
			array[num] = default(T);
			if (num == 0 && 256 <= array.Length)
			{
				this.items = null;
			}
			else if (256 < array.Length && num < array.Length / 4)
			{
				Array.Resize<T>(ref this.items, array.Length / 2);
			}
			this.version++;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0001F7C8 File Offset: 0x0001D9C8
		public void Sort<K>(Func<T, K> selector, Func<K, K, int> comparer)
		{
			if (this.items != null)
			{
				ArraySet<T>.SelectorComparer<K> selectorComparer;
				selectorComparer.Selector = selector;
				selectorComparer.Comparer = comparer;
				Array.Sort<T>(this.items, 0, this.count, selectorComparer);
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001F805 File Offset: 0x0001DA05
		public void TrimToSize()
		{
			Array.Resize<T>(ref this.items, this.count);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001F818 File Offset: 0x0001DA18
		private static T IdentitySelect(T arg)
		{
			return arg;
		}

		// Token: 0x04000488 RID: 1160
		private T[] items;

		// Token: 0x04000489 RID: 1161
		private int count;

		// Token: 0x0400048A RID: 1162
		private int version;

		// Token: 0x020000E2 RID: 226
		private struct SelectorComparer<K> : IComparer<T>
		{
			// Token: 0x0600075C RID: 1884 RVA: 0x0001F81B File Offset: 0x0001DA1B
			int IComparer<!0>.Compare(T x, T y)
			{
				return this.Comparer(this.Selector(x), this.Selector(y));
			}

			// Token: 0x0400048B RID: 1163
			internal Func<T, K> Selector;

			// Token: 0x0400048C RID: 1164
			internal Func<K, K, int> Comparer;
		}
	}
}
