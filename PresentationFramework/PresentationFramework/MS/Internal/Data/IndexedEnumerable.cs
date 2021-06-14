using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200072A RID: 1834
	internal class IndexedEnumerable : IEnumerable, IWeakEventListener
	{
		// Token: 0x0600754A RID: 30026 RVA: 0x002184CE File Offset: 0x002166CE
		internal IndexedEnumerable(IEnumerable collection) : this(collection, null)
		{
		}

		// Token: 0x0600754B RID: 30027 RVA: 0x002184D8 File Offset: 0x002166D8
		internal IndexedEnumerable(IEnumerable collection, Predicate<object> filterCallback)
		{
			this._filterCallback = filterCallback;
			this.SetCollection(collection);
			if (this.List == null)
			{
				INotifyCollectionChanged notifyCollectionChanged = collection as INotifyCollectionChanged;
				if (notifyCollectionChanged != null)
				{
					CollectionChangedEventManager.AddHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
				}
			}
		}

		// Token: 0x0600754C RID: 30028 RVA: 0x00218534 File Offset: 0x00216734
		internal int IndexOf(object item)
		{
			int num;
			if (this.GetNativeIndexOf(item, out num))
			{
				return num;
			}
			if (this.EnsureCacheCurrent() && item == this._cachedItem)
			{
				return this._cachedIndex;
			}
			num = -1;
			if (this._cachedIndex >= 0)
			{
				this.UseNewEnumerator();
			}
			int num2 = 0;
			while (this._enumerator.MoveNext())
			{
				if (object.Equals(this._enumerator.Current, item))
				{
					num = num2;
					break;
				}
				num2++;
			}
			if (num >= 0)
			{
				this.CacheCurrentItem(num, this._enumerator.Current);
			}
			else
			{
				this.ClearAllCaches();
				this.DisposeEnumerator(ref this._enumerator);
			}
			return num;
		}

		// Token: 0x17001BE5 RID: 7141
		// (get) Token: 0x0600754D RID: 30029 RVA: 0x002185D0 File Offset: 0x002167D0
		internal int Count
		{
			get
			{
				this.EnsureCacheCurrent();
				int num = 0;
				if (this.GetNativeCount(out num))
				{
					return num;
				}
				if (this._cachedCount >= 0)
				{
					return this._cachedCount;
				}
				num = 0;
				foreach (object obj in this)
				{
					num++;
				}
				this._cachedCount = num;
				this._cachedIsEmpty = new bool?(this._cachedCount == 0);
				return num;
			}
		}

		// Token: 0x17001BE6 RID: 7142
		// (get) Token: 0x0600754E RID: 30030 RVA: 0x00218660 File Offset: 0x00216860
		internal bool IsEmpty
		{
			get
			{
				bool result;
				if (this.GetNativeIsEmpty(out result))
				{
					return result;
				}
				if (this._cachedIsEmpty != null)
				{
					return this._cachedIsEmpty.Value;
				}
				IEnumerator enumerator = this.GetEnumerator();
				this._cachedIsEmpty = new bool?(!enumerator.MoveNext());
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				if (this._cachedIsEmpty.Value)
				{
					this._cachedCount = 0;
				}
				return this._cachedIsEmpty.Value;
			}
		}

		// Token: 0x17001BE7 RID: 7143
		internal object this[int index]
		{
			get
			{
				object result;
				if (this.GetNativeItemAt(index, out result))
				{
					return result;
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				int num = index - this._cachedIndex;
				if (num < 0)
				{
					this.UseNewEnumerator();
					num = index + 1;
				}
				if (this.EnsureCacheCurrent())
				{
					if (index == this._cachedIndex)
					{
						return this._cachedItem;
					}
				}
				else
				{
					num = index + 1;
				}
				while (num > 0 && this._enumerator.MoveNext())
				{
					num--;
				}
				if (num != 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				this.CacheCurrentItem(index, this._enumerator.Current);
				return this._cachedItem;
			}
		}

		// Token: 0x17001BE8 RID: 7144
		// (get) Token: 0x06007550 RID: 30032 RVA: 0x00218775 File Offset: 0x00216975
		internal IEnumerable Enumerable
		{
			get
			{
				return this._enumerable;
			}
		}

		// Token: 0x17001BE9 RID: 7145
		// (get) Token: 0x06007551 RID: 30033 RVA: 0x0021877D File Offset: 0x0021697D
		internal ICollection Collection
		{
			get
			{
				return this._collection;
			}
		}

		// Token: 0x17001BEA RID: 7146
		// (get) Token: 0x06007552 RID: 30034 RVA: 0x00218785 File Offset: 0x00216985
		internal IList List
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x17001BEB RID: 7147
		// (get) Token: 0x06007553 RID: 30035 RVA: 0x0021878D File Offset: 0x0021698D
		internal CollectionView CollectionView
		{
			get
			{
				return this._collectionView;
			}
		}

		// Token: 0x06007554 RID: 30036 RVA: 0x00218795 File Offset: 0x00216995
		public IEnumerator GetEnumerator()
		{
			return new IndexedEnumerable.FilteredEnumerator(this, this.Enumerable, this.FilterCallback);
		}

		// Token: 0x06007555 RID: 30037 RVA: 0x002187AC File Offset: 0x002169AC
		internal static void CopyTo(IEnumerable collection, Array array, int index)
		{
			Invariant.Assert(collection != null, "collection is null");
			Invariant.Assert(array != null, "target array is null");
			Invariant.Assert(array.Rank == 1, "expected array of rank=1");
			Invariant.Assert(index >= 0, "index must be positive");
			ICollection collection2 = collection as ICollection;
			if (collection2 != null)
			{
				collection2.CopyTo(array, index);
				return;
			}
			foreach (object value in collection)
			{
				if (index >= array.Length)
				{
					throw new ArgumentException(SR.Get("CopyToNotEnoughSpace"), "index");
				}
				((IList)array)[index] = value;
				index++;
			}
		}

		// Token: 0x06007556 RID: 30038 RVA: 0x0021887C File Offset: 0x00216A7C
		internal void Invalidate()
		{
			this.ClearAllCaches();
			if (this.List == null)
			{
				INotifyCollectionChanged notifyCollectionChanged = this.Enumerable as INotifyCollectionChanged;
				if (notifyCollectionChanged != null)
				{
					CollectionChangedEventManager.RemoveHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
				}
			}
			this._enumerable = null;
			this.DisposeEnumerator(ref this._enumerator);
			this.DisposeEnumerator(ref this._changeTracker);
			this._collection = null;
			this._list = null;
			this._filterCallback = null;
		}

		// Token: 0x17001BEC RID: 7148
		// (get) Token: 0x06007557 RID: 30039 RVA: 0x002188EC File Offset: 0x00216AEC
		private Predicate<object> FilterCallback
		{
			get
			{
				return this._filterCallback;
			}
		}

		// Token: 0x06007558 RID: 30040 RVA: 0x002188F4 File Offset: 0x00216AF4
		private void CacheCurrentItem(int index, object item)
		{
			this._cachedIndex = index;
			this._cachedItem = item;
			this._cachedVersion = this._enumeratorVersion;
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x00218910 File Offset: 0x00216B10
		private bool EnsureCacheCurrent()
		{
			int num = this.EnsureEnumerator();
			if (num != this._cachedVersion)
			{
				this.ClearAllCaches();
				this._cachedVersion = num;
			}
			return num == this._cachedVersion && this._cachedIndex >= 0;
		}

		// Token: 0x0600755A RID: 30042 RVA: 0x00218954 File Offset: 0x00216B54
		private int EnsureEnumerator()
		{
			if (this._enumerator == null)
			{
				this.UseNewEnumerator();
			}
			else
			{
				try
				{
					this._changeTracker.MoveNext();
				}
				catch (InvalidOperationException)
				{
					this.UseNewEnumerator();
				}
			}
			return this._enumeratorVersion;
		}

		// Token: 0x0600755B RID: 30043 RVA: 0x002189A0 File Offset: 0x00216BA0
		private void UseNewEnumerator()
		{
			this._enumeratorVersion++;
			this.DisposeEnumerator(ref this._changeTracker);
			this._changeTracker = this._enumerable.GetEnumerator();
			this.DisposeEnumerator(ref this._enumerator);
			this._enumerator = this.GetEnumerator();
			this._cachedIndex = -1;
			this._cachedItem = null;
		}

		// Token: 0x0600755C RID: 30044 RVA: 0x002189FE File Offset: 0x00216BFE
		private void InvalidateEnumerator()
		{
			this._enumeratorVersion++;
			this.DisposeEnumerator(ref this._enumerator);
			this.ClearAllCaches();
		}

		// Token: 0x0600755D RID: 30045 RVA: 0x00218A20 File Offset: 0x00216C20
		private void DisposeEnumerator(ref IEnumerator ie)
		{
			IDisposable disposable = ie as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			ie = null;
		}

		// Token: 0x0600755E RID: 30046 RVA: 0x00218A41 File Offset: 0x00216C41
		private void ClearAllCaches()
		{
			this._cachedItem = null;
			this._cachedIndex = -1;
			this._cachedCount = -1;
		}

		// Token: 0x0600755F RID: 30047 RVA: 0x00218A58 File Offset: 0x00216C58
		private void SetCollection(IEnumerable collection)
		{
			Invariant.Assert(collection != null);
			this._enumerable = collection;
			this._collection = (collection as ICollection);
			this._list = (collection as IList);
			this._collectionView = (collection as CollectionView);
			if (this.List == null && this.CollectionView == null)
			{
				Type type = collection.GetType();
				MethodInfo method = type.GetMethod("IndexOf", new Type[]
				{
					typeof(object)
				});
				if (method != null && method.ReturnType == typeof(int))
				{
					this._reflectedIndexOf = method;
				}
				MemberInfo[] defaultMembers = type.GetDefaultMembers();
				for (int i = 0; i <= defaultMembers.Length - 1; i++)
				{
					PropertyInfo propertyInfo = defaultMembers[i] as PropertyInfo;
					if (propertyInfo != null)
					{
						ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
						if (indexParameters.Length == 1 && indexParameters[0].ParameterType.IsAssignableFrom(typeof(int)))
						{
							this._reflectedItemAt = propertyInfo;
							break;
						}
					}
				}
				if (this.Collection == null)
				{
					PropertyInfo property = type.GetProperty("Count", typeof(int));
					if (property != null)
					{
						this._reflectedCount = property;
					}
				}
			}
		}

		// Token: 0x06007560 RID: 30048 RVA: 0x00218B8C File Offset: 0x00216D8C
		private bool GetNativeCount(out int value)
		{
			bool result = false;
			value = -1;
			if (this.Collection != null)
			{
				value = this.Collection.Count;
				result = true;
			}
			else if (this.CollectionView != null)
			{
				value = this.CollectionView.Count;
				result = true;
			}
			else if (this._reflectedCount != null)
			{
				try
				{
					value = (int)this._reflectedCount.GetValue(this.Enumerable, null);
					result = true;
				}
				catch (MethodAccessException)
				{
					this._reflectedCount = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06007561 RID: 30049 RVA: 0x00218C18 File Offset: 0x00216E18
		private bool GetNativeIsEmpty(out bool isEmpty)
		{
			bool result = false;
			isEmpty = true;
			if (this.Collection != null)
			{
				isEmpty = (this.Collection.Count == 0);
				result = true;
			}
			else if (this.CollectionView != null)
			{
				isEmpty = this.CollectionView.IsEmpty;
				result = true;
			}
			else if (this._reflectedCount != null)
			{
				try
				{
					isEmpty = ((int)this._reflectedCount.GetValue(this.Enumerable, null) == 0);
					result = true;
				}
				catch (MethodAccessException)
				{
					this._reflectedCount = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06007562 RID: 30050 RVA: 0x00218CAC File Offset: 0x00216EAC
		private bool GetNativeIndexOf(object item, out int value)
		{
			bool result = false;
			value = -1;
			if (this.List != null && this.FilterCallback == null)
			{
				value = this.List.IndexOf(item);
				result = true;
			}
			else if (this.CollectionView != null)
			{
				value = this.CollectionView.IndexOf(item);
				result = true;
			}
			else if (this._reflectedIndexOf != null)
			{
				try
				{
					value = (int)this._reflectedIndexOf.Invoke(this.Enumerable, new object[]
					{
						item
					});
					result = true;
				}
				catch (MethodAccessException)
				{
					this._reflectedIndexOf = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06007563 RID: 30051 RVA: 0x00218D4C File Offset: 0x00216F4C
		private bool GetNativeItemAt(int index, out object value)
		{
			bool result = false;
			value = null;
			if (this.List != null)
			{
				value = this.List[index];
				result = true;
			}
			else if (this.CollectionView != null)
			{
				value = this.CollectionView.GetItemAt(index);
				result = true;
			}
			else if (this._reflectedItemAt != null)
			{
				try
				{
					value = this._reflectedItemAt.GetValue(this.Enumerable, new object[]
					{
						index
					});
					result = true;
				}
				catch (MethodAccessException)
				{
					this._reflectedItemAt = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06007564 RID: 30052 RVA: 0x00218DE4 File Offset: 0x00216FE4
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		// Token: 0x06007565 RID: 30053 RVA: 0x0000B02A File Offset: 0x0000922A
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x06007566 RID: 30054 RVA: 0x00218DEF File Offset: 0x00216FEF
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.InvalidateEnumerator();
		}

		// Token: 0x04003821 RID: 14369
		private IEnumerable _enumerable;

		// Token: 0x04003822 RID: 14370
		private IEnumerator _enumerator;

		// Token: 0x04003823 RID: 14371
		private IEnumerator _changeTracker;

		// Token: 0x04003824 RID: 14372
		private ICollection _collection;

		// Token: 0x04003825 RID: 14373
		private IList _list;

		// Token: 0x04003826 RID: 14374
		private CollectionView _collectionView;

		// Token: 0x04003827 RID: 14375
		private int _enumeratorVersion;

		// Token: 0x04003828 RID: 14376
		private object _cachedItem;

		// Token: 0x04003829 RID: 14377
		private int _cachedIndex = -1;

		// Token: 0x0400382A RID: 14378
		private int _cachedVersion = -1;

		// Token: 0x0400382B RID: 14379
		private int _cachedCount = -1;

		// Token: 0x0400382C RID: 14380
		private bool? _cachedIsEmpty;

		// Token: 0x0400382D RID: 14381
		private PropertyInfo _reflectedCount;

		// Token: 0x0400382E RID: 14382
		private PropertyInfo _reflectedItemAt;

		// Token: 0x0400382F RID: 14383
		private MethodInfo _reflectedIndexOf;

		// Token: 0x04003830 RID: 14384
		private Predicate<object> _filterCallback;

		// Token: 0x02000B50 RID: 2896
		private class FilteredEnumerator : IEnumerator, IDisposable
		{
			// Token: 0x06008DB9 RID: 36281 RVA: 0x0025A11A File Offset: 0x0025831A
			public FilteredEnumerator(IndexedEnumerable indexedEnumerable, IEnumerable enumerable, Predicate<object> filterCallback)
			{
				this._enumerable = enumerable;
				this._enumerator = this._enumerable.GetEnumerator();
				this._filterCallback = filterCallback;
				this._indexedEnumerable = indexedEnumerable;
			}

			// Token: 0x06008DBA RID: 36282 RVA: 0x0025A148 File Offset: 0x00258348
			void IEnumerator.Reset()
			{
				if (this._indexedEnumerable._enumerable == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				this.Dispose();
				this._enumerator = this._enumerable.GetEnumerator();
			}

			// Token: 0x06008DBB RID: 36283 RVA: 0x0025A180 File Offset: 0x00258380
			bool IEnumerator.MoveNext()
			{
				if (this._indexedEnumerable._enumerable == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				bool result;
				if (this._filterCallback == null)
				{
					result = this._enumerator.MoveNext();
				}
				else
				{
					while ((result = this._enumerator.MoveNext()) && !this._filterCallback(this._enumerator.Current))
					{
					}
				}
				return result;
			}

			// Token: 0x17001F82 RID: 8066
			// (get) Token: 0x06008DBC RID: 36284 RVA: 0x0025A1E8 File Offset: 0x002583E8
			object IEnumerator.Current
			{
				get
				{
					return this._enumerator.Current;
				}
			}

			// Token: 0x06008DBD RID: 36285 RVA: 0x0025A1F8 File Offset: 0x002583F8
			public void Dispose()
			{
				IDisposable disposable = this._enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				this._enumerator = null;
			}

			// Token: 0x04004AEA RID: 19178
			private IEnumerable _enumerable;

			// Token: 0x04004AEB RID: 19179
			private IEnumerator _enumerator;

			// Token: 0x04004AEC RID: 19180
			private IndexedEnumerable _indexedEnumerable;

			// Token: 0x04004AED RID: 19181
			private Predicate<object> _filterCallback;
		}
	}
}
