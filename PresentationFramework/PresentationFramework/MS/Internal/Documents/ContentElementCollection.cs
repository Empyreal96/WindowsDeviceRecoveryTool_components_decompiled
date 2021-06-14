using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006BA RID: 1722
	internal abstract class ContentElementCollection<TParent, TItem> : IList<TItem>, ICollection<TItem>, IEnumerable<TItem>, IEnumerable, IList, ICollection where TParent : TextElement, IAcceptInsertion where TItem : FrameworkContentElement, IIndexedChild<TParent>
	{
		// Token: 0x06006EDA RID: 28378 RVA: 0x001FE10E File Offset: 0x001FC30E
		internal ContentElementCollection(TParent owner)
		{
			this._owner = owner;
			this.Items = new TItem[this.DefaultCapacity];
		}

		// Token: 0x06006EDB RID: 28379 RVA: 0x001FE130 File Offset: 0x001FC330
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.Get("TableCollectionRankMultiDimNotSupported"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.Get("TableCollectionOutOfRangeNeedNonNegNum"));
			}
			if (array.Length - index < this.Size)
			{
				throw new ArgumentException(SR.Get("TableCollectionInvalidOffLen"));
			}
			Array.Copy(this.Items, 0, array, index, this.Size);
		}

		// Token: 0x06006EDC RID: 28380 RVA: 0x001FE1B4 File Offset: 0x001FC3B4
		public void CopyTo(TItem[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.Get("TableCollectionOutOfRangeNeedNonNegNum"));
			}
			if (array.Length - index < this.Size)
			{
				throw new ArgumentException(SR.Get("TableCollectionInvalidOffLen"));
			}
			Array.Copy(this.Items, 0, array, index, this.Size);
		}

		// Token: 0x06006EDD RID: 28381 RVA: 0x001FE219 File Offset: 0x001FC419
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06006EDE RID: 28382 RVA: 0x001FE221 File Offset: 0x001FC421
		internal IEnumerator GetEnumerator()
		{
			return new ContentElementCollection<TParent, TItem>.ContentElementCollectionEnumeratorSimple(this);
		}

		// Token: 0x06006EDF RID: 28383 RVA: 0x001FE221 File Offset: 0x001FC421
		IEnumerator<TItem> IEnumerable<!1>.GetEnumerator()
		{
			return new ContentElementCollection<TParent, TItem>.ContentElementCollectionEnumeratorSimple(this);
		}

		// Token: 0x06006EE0 RID: 28384
		public abstract void Add(TItem item);

		// Token: 0x06006EE1 RID: 28385
		public abstract void Clear();

		// Token: 0x06006EE2 RID: 28386 RVA: 0x001FE229 File Offset: 0x001FC429
		public bool Contains(TItem item)
		{
			return this.BelongsToOwner(item);
		}

		// Token: 0x06006EE3 RID: 28387 RVA: 0x001FE237 File Offset: 0x001FC437
		public int IndexOf(TItem item)
		{
			if (this.BelongsToOwner(item))
			{
				return item.Index;
			}
			return -1;
		}

		// Token: 0x06006EE4 RID: 28388
		public abstract void Insert(int index, TItem item);

		// Token: 0x06006EE5 RID: 28389
		public abstract bool Remove(TItem item);

		// Token: 0x06006EE6 RID: 28390
		public abstract void RemoveAt(int index);

		// Token: 0x06006EE7 RID: 28391
		public abstract void RemoveRange(int index, int count);

		// Token: 0x06006EE8 RID: 28392 RVA: 0x001FE24F File Offset: 0x001FC44F
		public void TrimToSize()
		{
			this.PrivateCapacity = this.Size;
		}

		// Token: 0x06006EE9 RID: 28393 RVA: 0x001FE260 File Offset: 0x001FC460
		int IList.Add(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TItem titem = value as TItem;
			this.Add(titem);
			return ((IList)this).IndexOf(titem);
		}

		// Token: 0x06006EEA RID: 28394 RVA: 0x001FE29A File Offset: 0x001FC49A
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06006EEB RID: 28395 RVA: 0x001FE2A4 File Offset: 0x001FC4A4
		bool IList.Contains(object value)
		{
			TItem titem = value as TItem;
			return titem != null && this.Contains(titem);
		}

		// Token: 0x06006EEC RID: 28396 RVA: 0x001FE2D0 File Offset: 0x001FC4D0
		int IList.IndexOf(object value)
		{
			TItem titem = value as TItem;
			if (titem == null)
			{
				return -1;
			}
			return this.IndexOf(titem);
		}

		// Token: 0x06006EED RID: 28397 RVA: 0x001FE2FC File Offset: 0x001FC4FC
		void IList.Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TItem titem = value as TItem;
			if (titem == null)
			{
				throw new ArgumentException(SR.Get("TableCollectionElementTypeExpected", new object[]
				{
					typeof(TItem).Name
				}), "value");
			}
			this.Insert(index, titem);
		}

		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x06006EEE RID: 28398 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A50 RID: 6736
		// (get) Token: 0x06006EEF RID: 28399 RVA: 0x001FE360 File Offset: 0x001FC560
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x06006EF0 RID: 28400 RVA: 0x001FE368 File Offset: 0x001FC568
		void IList.Remove(object value)
		{
			TItem titem = value as TItem;
			if (titem == null)
			{
				return;
			}
			this.Remove(titem);
		}

		// Token: 0x06006EF1 RID: 28401 RVA: 0x001FE392 File Offset: 0x001FC592
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x17001A51 RID: 6737
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				TItem titem = value as TItem;
				if (titem == null)
				{
					throw new ArgumentException(SR.Get("TableCollectionElementTypeExpected", new object[]
					{
						typeof(TItem).Name
					}), "value");
				}
				this[index] = titem;
			}
		}

		// Token: 0x17001A52 RID: 6738
		public abstract TItem this[int index]
		{
			get;
			set;
		}

		// Token: 0x17001A53 RID: 6739
		// (get) Token: 0x06006EF6 RID: 28406 RVA: 0x001FE410 File Offset: 0x001FC610
		public int Count
		{
			get
			{
				return this.Size;
			}
		}

		// Token: 0x17001A54 RID: 6740
		// (get) Token: 0x06006EF7 RID: 28407 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A55 RID: 6741
		// (get) Token: 0x06006EF8 RID: 28408 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A56 RID: 6742
		// (get) Token: 0x06006EF9 RID: 28409 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x06006EFA RID: 28410 RVA: 0x001FE418 File Offset: 0x001FC618
		// (set) Token: 0x06006EFB RID: 28411 RVA: 0x001FE420 File Offset: 0x001FC620
		public int Capacity
		{
			get
			{
				return this.PrivateCapacity;
			}
			set
			{
				this.PrivateCapacity = value;
			}
		}

		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x06006EFC RID: 28412 RVA: 0x001FE429 File Offset: 0x001FC629
		public TParent Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x06006EFD RID: 28413 RVA: 0x001FE431 File Offset: 0x001FC631
		// (set) Token: 0x06006EFE RID: 28414 RVA: 0x001FE439 File Offset: 0x001FC639
		private protected TItem[] Items
		{
			protected get
			{
				return this._items;
			}
			private set
			{
				this._items = value;
			}
		}

		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x06006EFF RID: 28415 RVA: 0x001FE442 File Offset: 0x001FC642
		// (set) Token: 0x06006F00 RID: 28416 RVA: 0x001FE44A File Offset: 0x001FC64A
		protected int Size
		{
			get
			{
				return this._size;
			}
			set
			{
				this._size = value;
			}
		}

		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x06006F01 RID: 28417 RVA: 0x001FE453 File Offset: 0x001FC653
		// (set) Token: 0x06006F02 RID: 28418 RVA: 0x001FE45B File Offset: 0x001FC65B
		protected int Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x06006F03 RID: 28419 RVA: 0x0009580C File Offset: 0x00093A0C
		protected int DefaultCapacity
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06006F04 RID: 28420 RVA: 0x001FE464 File Offset: 0x001FC664
		internal void EnsureCapacity(int min)
		{
			if (this.PrivateCapacity < min)
			{
				this.PrivateCapacity = Math.Max(min, this.PrivateCapacity * 2);
			}
		}

		// Token: 0x06006F05 RID: 28421
		internal abstract void PrivateConnectChild(int index, TItem item);

		// Token: 0x06006F06 RID: 28422
		internal abstract void PrivateDisconnectChild(TItem item);

		// Token: 0x06006F07 RID: 28423 RVA: 0x001FE484 File Offset: 0x001FC684
		internal void PrivateRemove(TItem item)
		{
			int index = item.Index;
			this.PrivateDisconnectChild(item);
			int size = this.Size - 1;
			this.Size = size;
			for (int i = index; i < this.Size; i++)
			{
				this.Items[i] = this.Items[i + 1];
				this.Items[i].Index = i;
			}
			this.Items[this.Size] = default(TItem);
		}

		// Token: 0x06006F08 RID: 28424 RVA: 0x001FE510 File Offset: 0x001FC710
		internal bool BelongsToOwner(TItem item)
		{
			if (item == null)
			{
				return false;
			}
			DependencyObject parent = item.Parent;
			if (parent is ContentElementCollection<TParent, TItem>.DummyProxy)
			{
				parent = LogicalTreeHelper.GetParent(parent);
			}
			return parent == this.Owner;
		}

		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x06006F09 RID: 28425 RVA: 0x001FE550 File Offset: 0x001FC750
		// (set) Token: 0x06006F0A RID: 28426 RVA: 0x001FE55C File Offset: 0x001FC75C
		internal int PrivateCapacity
		{
			get
			{
				return this.Items.Length;
			}
			set
			{
				if (value != this.Items.Length)
				{
					if (value < this.Size)
					{
						throw new ArgumentOutOfRangeException(SR.Get("TableCollectionNotEnoughCapacity"));
					}
					if (value > 0)
					{
						TItem[] array = new TItem[value];
						if (this.Size > 0)
						{
							Array.Copy(this.Items, 0, array, 0, this.Size);
						}
						this.Items = array;
						return;
					}
					this.Items = new TItem[this.DefaultCapacity];
				}
			}
		}

		// Token: 0x0400368F RID: 13967
		private readonly TParent _owner;

		// Token: 0x04003690 RID: 13968
		private TItem[] _items;

		// Token: 0x04003691 RID: 13969
		private int _size;

		// Token: 0x04003692 RID: 13970
		private int _version;

		// Token: 0x04003693 RID: 13971
		protected const int c_defaultCapacity = 8;

		// Token: 0x02000B2D RID: 2861
		protected class ContentElementCollectionEnumeratorSimple : IEnumerator<!1>, IDisposable, IEnumerator
		{
			// Token: 0x06008D54 RID: 36180 RVA: 0x0025953C File Offset: 0x0025773C
			internal ContentElementCollectionEnumeratorSimple(ContentElementCollection<TParent, TItem> collection)
			{
				this._collection = collection;
				this._index = -1;
				this.Version = this._collection.Version;
				this._currentElement = collection;
			}

			// Token: 0x06008D55 RID: 36181 RVA: 0x0025956C File Offset: 0x0025776C
			public bool MoveNext()
			{
				if (this.Version != this._collection.Version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				if (this._index < this._collection.Size - 1)
				{
					this._index++;
					this._currentElement = this._collection[this._index];
					return true;
				}
				this._currentElement = this._collection;
				this._index = this._collection.Size;
				return false;
			}

			// Token: 0x17001F72 RID: 8050
			// (get) Token: 0x06008D56 RID: 36182 RVA: 0x002595FC File Offset: 0x002577FC
			public TItem Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return (TItem)((object)this._currentElement);
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x17001F73 RID: 8051
			// (get) Token: 0x06008D57 RID: 36183 RVA: 0x0025964B File Offset: 0x0025784B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06008D58 RID: 36184 RVA: 0x00259658 File Offset: 0x00257858
			public void Reset()
			{
				if (this.Version != this._collection.Version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				this._currentElement = this._collection;
				this._index = -1;
			}

			// Token: 0x06008D59 RID: 36185 RVA: 0x000B1479 File Offset: 0x000AF679
			public void Dispose()
			{
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004A85 RID: 19077
			private ContentElementCollection<TParent, TItem> _collection;

			// Token: 0x04004A86 RID: 19078
			private int _index;

			// Token: 0x04004A87 RID: 19079
			protected int Version;

			// Token: 0x04004A88 RID: 19080
			private object _currentElement;
		}

		// Token: 0x02000B2E RID: 2862
		protected class DummyProxy : DependencyObject
		{
		}
	}
}
