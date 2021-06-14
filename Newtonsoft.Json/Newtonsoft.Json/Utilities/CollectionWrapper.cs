using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000D5 RID: 213
	internal class CollectionWrapper<T> : ICollection<T>, IEnumerable<!0>, IWrappedCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x06000A6B RID: 2667 RVA: 0x00028FDE File Offset: 0x000271DE
		public CollectionWrapper(IList list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			if (list is ICollection<T>)
			{
				this._genericCollection = (ICollection<T>)list;
				return;
			}
			this._list = list;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0002900D File Offset: 0x0002720D
		public CollectionWrapper(ICollection<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			this._genericCollection = list;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00029027 File Offset: 0x00027227
		public virtual void Add(T item)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Add(item);
				return;
			}
			this._list.Add(item);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00029050 File Offset: 0x00027250
		public virtual void Clear()
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Clear();
				return;
			}
			this._list.Clear();
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00029071 File Offset: 0x00027271
		public virtual bool Contains(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Contains(item);
			}
			return this._list.Contains(item);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00029099 File Offset: 0x00027299
		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.CopyTo(array, arrayIndex);
				return;
			}
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x000290BE File Offset: 0x000272BE
		public virtual int Count
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.Count;
				}
				return this._list.Count;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x000290DF File Offset: 0x000272DF
		public virtual bool IsReadOnly
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsReadOnly;
			}
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00029100 File Offset: 0x00027300
		public virtual bool Remove(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Remove(item);
			}
			bool flag = this._list.Contains(item);
			if (flag)
			{
				this._list.Remove(item);
			}
			return flag;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00029149 File Offset: 0x00027349
		public virtual IEnumerator<T> GetEnumerator()
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.GetEnumerator();
			}
			return this._list.Cast<T>().GetEnumerator();
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0002916F File Offset: 0x0002736F
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.GetEnumerator();
			}
			return this._list.GetEnumerator();
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00029190 File Offset: 0x00027390
		int IList.Add(object value)
		{
			CollectionWrapper<T>.VerifyValueType(value);
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x000291AC File Offset: 0x000273AC
		bool IList.Contains(object value)
		{
			return CollectionWrapper<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x000291C4 File Offset: 0x000273C4
		int IList.IndexOf(object value)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support IndexOf.");
			}
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				return this._list.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x000291F9 File Offset: 0x000273F9
		void IList.RemoveAt(int index)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support RemoveAt.");
			}
			this._list.RemoveAt(index);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0002921A File Offset: 0x0002741A
		void IList.Insert(int index, object value)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support Insert.");
			}
			CollectionWrapper<T>.VerifyValueType(value);
			this._list.Insert(index, (T)((object)value));
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0002924C File Offset: 0x0002744C
		bool IList.IsFixedSize
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsFixedSize;
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0002926D File Offset: 0x0002746D
		void IList.Remove(object value)
		{
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x1700023E RID: 574
		object IList.this[int index]
		{
			get
			{
				if (this._genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				return this._list[index];
			}
			set
			{
				if (this._genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				CollectionWrapper<T>.VerifyValueType(value);
				this._list[index] = (T)((object)value);
			}
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x000292D7 File Offset: 0x000274D7
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo((T[])array, arrayIndex);
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x000292E6 File Offset: 0x000274E6
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x000292E9 File Offset: 0x000274E9
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0002930B File Offset: 0x0002750B
		private static void VerifyValueType(object value)
		{
			if (!CollectionWrapper<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(CultureInfo.InvariantCulture, value, typeof(T)), "value");
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0002933A File Offset: 0x0002753A
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && (!typeof(T).IsValueType() || ReflectionUtils.IsNullableType(typeof(T))));
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0002936C File Offset: 0x0002756C
		public object UnderlyingCollection
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection;
				}
				return this._list;
			}
		}

		// Token: 0x0400038A RID: 906
		private readonly IList _list;

		// Token: 0x0400038B RID: 907
		private readonly ICollection<T> _genericCollection;

		// Token: 0x0400038C RID: 908
		private object _syncRoot;
	}
}
