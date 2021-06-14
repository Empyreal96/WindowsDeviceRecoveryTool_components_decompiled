using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x02000505 RID: 1285
	internal class MultipleCopiesCollection : IList, ICollection, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		// Token: 0x06005267 RID: 21095 RVA: 0x001707A6 File Offset: 0x0016E9A6
		internal MultipleCopiesCollection(object item, int count)
		{
			this.CopiedItem = item;
			this._count = count;
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x001707BC File Offset: 0x0016E9BC
		internal void MirrorCollectionChange(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.Insert(e.NewStartingIndex);
				return;
			case NotifyCollectionChangedAction.Remove:
				this.RemoveAt(e.OldStartingIndex);
				return;
			case NotifyCollectionChangedAction.Replace:
				this.OnReplace(this.CopiedItem, this.CopiedItem, e.NewStartingIndex);
				return;
			case NotifyCollectionChangedAction.Move:
				this.Move(e.OldStartingIndex, e.NewStartingIndex);
				return;
			case NotifyCollectionChangedAction.Reset:
				this.Reset();
				return;
			default:
				return;
			}
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x00170838 File Offset: 0x0016EA38
		internal void SyncToCount(int newCount)
		{
			int repeatCount = this.RepeatCount;
			if (newCount != repeatCount)
			{
				if (newCount > repeatCount)
				{
					this.InsertRange(repeatCount, newCount - repeatCount);
					return;
				}
				int num = repeatCount - newCount;
				this.RemoveRange(repeatCount - num, num);
			}
		}

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x0600526A RID: 21098 RVA: 0x0017086D File Offset: 0x0016EA6D
		// (set) Token: 0x0600526B RID: 21099 RVA: 0x00170878 File Offset: 0x0016EA78
		internal object CopiedItem
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value == CollectionView.NewItemPlaceholder)
				{
					value = DataGrid.NewItemPlaceholder;
				}
				if (this._item != value)
				{
					object item = this._item;
					this._item = value;
					this.OnPropertyChanged("Item[]");
					for (int i = 0; i < this._count; i++)
					{
						this.OnReplace(item, this._item, i);
					}
				}
			}
		}

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x0600526C RID: 21100 RVA: 0x001708D5 File Offset: 0x0016EAD5
		// (set) Token: 0x0600526D RID: 21101 RVA: 0x001708DD File Offset: 0x0016EADD
		private int RepeatCount
		{
			get
			{
				return this._count;
			}
			set
			{
				if (this._count != value)
				{
					this._count = value;
					this.OnPropertyChanged("Count");
					this.OnPropertyChanged("Item[]");
				}
			}
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x00170908 File Offset: 0x0016EB08
		private void Insert(int index)
		{
			int repeatCount = this.RepeatCount;
			this.RepeatCount = repeatCount + 1;
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, this.CopiedItem, index);
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x00170934 File Offset: 0x0016EB34
		private void InsertRange(int index, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Insert(index);
				index++;
			}
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x00170959 File Offset: 0x0016EB59
		private void Move(int oldIndex, int newIndex)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, this.CopiedItem, newIndex, oldIndex));
		}

		// Token: 0x06005271 RID: 21105 RVA: 0x00170970 File Offset: 0x0016EB70
		private void RemoveAt(int index)
		{
			int repeatCount = this.RepeatCount;
			this.RepeatCount = repeatCount - 1;
			this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, this.CopiedItem, index);
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x0017099C File Offset: 0x0016EB9C
		private void RemoveRange(int index, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.RemoveAt(index);
			}
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x001709BC File Offset: 0x0016EBBC
		private void OnReplace(object oldItem, object newItem, int index)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x001709CD File Offset: 0x0016EBCD
		private void Reset()
		{
			this.RepeatCount = 0;
			this.OnCollectionReset();
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x001709DC File Offset: 0x0016EBDC
		public int Add(object value)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x001709DC File Offset: 0x0016EBDC
		public void Clear()
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x001709ED File Offset: 0x0016EBED
		public bool Contains(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this._item == value;
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x00170A06 File Offset: 0x0016EC06
		public int IndexOf(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this._item != value)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x001709DC File Offset: 0x0016EBDC
		public void Insert(int index, object value)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x0600527A RID: 21114 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x0600527B RID: 21115 RVA: 0x00016748 File Offset: 0x00014948
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600527C RID: 21116 RVA: 0x001709DC File Offset: 0x0016EBDC
		public void Remove(object value)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x001709DC File Offset: 0x0016EBDC
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x170013FD RID: 5117
		public object this[int index]
		{
			get
			{
				if (index >= 0 && index < this.RepeatCount)
				{
					return this._item;
				}
				throw new ArgumentOutOfRangeException("index");
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x00041D30 File Offset: 0x0003FF30
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x06005281 RID: 21121 RVA: 0x00170A49 File Offset: 0x0016EC49
		public int Count
		{
			get
			{
				return this.RepeatCount;
			}
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x06005282 RID: 21122 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x06005283 RID: 21123 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x00170A51 File Offset: 0x0016EC51
		public IEnumerator GetEnumerator()
		{
			return new MultipleCopiesCollection.MultipleCopiesCollectionEnumerator(this);
		}

		// Token: 0x14000101 RID: 257
		// (add) Token: 0x06005285 RID: 21125 RVA: 0x00170A5C File Offset: 0x0016EC5C
		// (remove) Token: 0x06005286 RID: 21126 RVA: 0x00170A94 File Offset: 0x0016EC94
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x06005287 RID: 21127 RVA: 0x00170AC9 File Offset: 0x0016ECC9
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x00170AD9 File Offset: 0x0016ECD9
		private void OnCollectionReset()
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x00170AE7 File Offset: 0x0016ECE7
		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, e);
			}
		}

		// Token: 0x14000102 RID: 258
		// (add) Token: 0x0600528A RID: 21130 RVA: 0x00170B00 File Offset: 0x0016ED00
		// (remove) Token: 0x0600528B RID: 21131 RVA: 0x00170B38 File Offset: 0x0016ED38
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600528C RID: 21132 RVA: 0x00170B6D File Offset: 0x0016ED6D
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x00170B7B File Offset: 0x0016ED7B
		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		// Token: 0x04002CB1 RID: 11441
		private object _item;

		// Token: 0x04002CB2 RID: 11442
		private int _count;

		// Token: 0x04002CB3 RID: 11443
		private const string CountName = "Count";

		// Token: 0x04002CB4 RID: 11444
		private const string IndexerName = "Item[]";

		// Token: 0x020009AD RID: 2477
		private class MultipleCopiesCollectionEnumerator : IEnumerator
		{
			// Token: 0x06008846 RID: 34886 RVA: 0x00251F00 File Offset: 0x00250100
			public MultipleCopiesCollectionEnumerator(MultipleCopiesCollection collection)
			{
				this._collection = collection;
				this._item = this._collection.CopiedItem;
				this._count = this._collection.RepeatCount;
				this._current = -1;
			}

			// Token: 0x17001EC0 RID: 7872
			// (get) Token: 0x06008847 RID: 34887 RVA: 0x00251F38 File Offset: 0x00250138
			object IEnumerator.Current
			{
				get
				{
					if (this._current < 0)
					{
						return null;
					}
					if (this._current < this._count)
					{
						return this._item;
					}
					throw new InvalidOperationException();
				}
			}

			// Token: 0x06008848 RID: 34888 RVA: 0x00251F60 File Offset: 0x00250160
			bool IEnumerator.MoveNext()
			{
				if (!this.IsCollectionUnchanged)
				{
					throw new InvalidOperationException();
				}
				int num = this._current + 1;
				if (num < this._count)
				{
					this._current = num;
					return true;
				}
				return false;
			}

			// Token: 0x06008849 RID: 34889 RVA: 0x00251F97 File Offset: 0x00250197
			void IEnumerator.Reset()
			{
				if (this.IsCollectionUnchanged)
				{
					this._current = -1;
					return;
				}
				throw new InvalidOperationException();
			}

			// Token: 0x17001EC1 RID: 7873
			// (get) Token: 0x0600884A RID: 34890 RVA: 0x00251FAE File Offset: 0x002501AE
			private bool IsCollectionUnchanged
			{
				get
				{
					return this._collection.RepeatCount == this._count && this._collection.CopiedItem == this._item;
				}
			}

			// Token: 0x0400450D RID: 17677
			private object _item;

			// Token: 0x0400450E RID: 17678
			private int _count;

			// Token: 0x0400450F RID: 17679
			private int _current;

			// Token: 0x04004510 RID: 17680
			private MultipleCopiesCollection _collection;
		}
	}
}
