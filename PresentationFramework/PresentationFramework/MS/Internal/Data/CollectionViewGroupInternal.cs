using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x0200070B RID: 1803
	internal class CollectionViewGroupInternal : CollectionViewGroup
	{
		// Token: 0x0600739A RID: 29594 RVA: 0x00211614 File Offset: 0x0020F814
		internal CollectionViewGroupInternal(object name, CollectionViewGroupInternal parent, bool isExplicit = false) : base(name)
		{
			this._parentGroup = parent;
			this._isExplicit = isExplicit;
		}

		// Token: 0x17001B7A RID: 7034
		// (get) Token: 0x0600739B RID: 29595 RVA: 0x00211632 File Offset: 0x0020F832
		public override bool IsBottomLevel
		{
			get
			{
				return this._groupBy == null;
			}
		}

		// Token: 0x17001B7B RID: 7035
		// (get) Token: 0x0600739C RID: 29596 RVA: 0x0021163D File Offset: 0x0020F83D
		// (set) Token: 0x0600739D RID: 29597 RVA: 0x00211648 File Offset: 0x0020F848
		internal GroupDescription GroupBy
		{
			get
			{
				return this._groupBy;
			}
			set
			{
				bool isBottomLevel = this.IsBottomLevel;
				if (this._groupBy != null)
				{
					PropertyChangedEventManager.RemoveHandler(this._groupBy, new EventHandler<PropertyChangedEventArgs>(this.OnGroupByChanged), string.Empty);
				}
				this._groupBy = value;
				if (this._groupBy != null)
				{
					PropertyChangedEventManager.AddHandler(this._groupBy, new EventHandler<PropertyChangedEventArgs>(this.OnGroupByChanged), string.Empty);
				}
				this._groupComparer = ((this._groupBy == null) ? null : ListCollectionView.PrepareComparer(this._groupBy.CustomSort, this._groupBy.SortDescriptionsInternal, delegate
				{
					for (CollectionViewGroupInternal collectionViewGroupInternal = this; collectionViewGroupInternal != null; collectionViewGroupInternal = collectionViewGroupInternal.Parent)
					{
						CollectionViewGroupRoot collectionViewGroupRoot = collectionViewGroupInternal as CollectionViewGroupRoot;
						if (collectionViewGroupRoot != null)
						{
							return collectionViewGroupRoot.View;
						}
					}
					return null;
				}));
				if (isBottomLevel != this.IsBottomLevel)
				{
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsBottomLevel"));
				}
			}
		}

		// Token: 0x17001B7C RID: 7036
		// (get) Token: 0x0600739E RID: 29598 RVA: 0x002116FC File Offset: 0x0020F8FC
		// (set) Token: 0x0600739F RID: 29599 RVA: 0x00211704 File Offset: 0x0020F904
		internal int FullCount
		{
			get
			{
				return this._fullCount;
			}
			set
			{
				this._fullCount = value;
			}
		}

		// Token: 0x17001B7D RID: 7037
		// (get) Token: 0x060073A0 RID: 29600 RVA: 0x0021170D File Offset: 0x0020F90D
		// (set) Token: 0x060073A1 RID: 29601 RVA: 0x00211715 File Offset: 0x0020F915
		internal int LastIndex
		{
			get
			{
				return this._lastIndex;
			}
			set
			{
				this._lastIndex = value;
			}
		}

		// Token: 0x17001B7E RID: 7038
		// (get) Token: 0x060073A2 RID: 29602 RVA: 0x00211720 File Offset: 0x0020F920
		internal object SeedItem
		{
			get
			{
				if (base.ItemCount > 0 && (this.GroupBy == null || this.GroupBy.GroupNames.Count == 0))
				{
					int i = 0;
					int count = base.Items.Count;
					while (i < count)
					{
						CollectionViewGroupInternal collectionViewGroupInternal = base.Items[i] as CollectionViewGroupInternal;
						if (collectionViewGroupInternal == null)
						{
							return base.Items[i];
						}
						if (collectionViewGroupInternal.ItemCount > 0)
						{
							return collectionViewGroupInternal.SeedItem;
						}
						i++;
					}
					return DependencyProperty.UnsetValue;
				}
				return DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x060073A3 RID: 29603 RVA: 0x002117A5 File Offset: 0x0020F9A5
		internal void Add(object item)
		{
			if (this._groupComparer == null)
			{
				this.ChangeCounts(item, 1);
				base.ProtectedItems.Add(item);
				return;
			}
			this.Insert(item, null, null);
		}

		// Token: 0x060073A4 RID: 29604 RVA: 0x002117D0 File Offset: 0x0020F9D0
		internal int Remove(object item, bool returnLeafIndex)
		{
			int result = -1;
			int num = base.ProtectedItems.IndexOf(item);
			if (num >= 0)
			{
				if (returnLeafIndex)
				{
					result = this.LeafIndexFromItem(null, num);
				}
				CollectionViewGroupInternal collectionViewGroupInternal = item as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					collectionViewGroupInternal.Clear();
					this.RemoveSubgroupFromMap(collectionViewGroupInternal);
				}
				this.ChangeCounts(item, -1);
				if (base.ProtectedItems.Count > 0)
				{
					base.ProtectedItems.RemoveAt(num);
				}
			}
			return result;
		}

		// Token: 0x060073A5 RID: 29605 RVA: 0x00211838 File Offset: 0x0020FA38
		internal void Clear()
		{
			this.FullCount = 1;
			base.ProtectedItemCount = 0;
			if (this._groupBy != null)
			{
				PropertyChangedEventManager.RemoveHandler(this._groupBy, new EventHandler<PropertyChangedEventArgs>(this.OnGroupByChanged), string.Empty);
				this._groupBy = null;
				int i = 0;
				int count = base.ProtectedItems.Count;
				while (i < count)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = base.ProtectedItems[i] as CollectionViewGroupInternal;
					if (collectionViewGroupInternal != null)
					{
						collectionViewGroupInternal.Clear();
					}
					i++;
				}
			}
			base.ProtectedItems.Clear();
			if (this._nameToGroupMap != null)
			{
				this._nameToGroupMap.Clear();
			}
		}

		// Token: 0x060073A6 RID: 29606 RVA: 0x002118D0 File Offset: 0x0020FAD0
		internal int LeafIndexOf(object item)
		{
			int num = 0;
			int i = 0;
			int count = base.Items.Count;
			while (i < count)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = base.Items[i] as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					int num2 = collectionViewGroupInternal.LeafIndexOf(item);
					if (num2 >= 0)
					{
						return num + num2;
					}
					num += collectionViewGroupInternal.ItemCount;
				}
				else
				{
					if (ItemsControl.EqualsEx(item, base.Items[i]))
					{
						return num;
					}
					num++;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x060073A7 RID: 29607 RVA: 0x00211948 File Offset: 0x0020FB48
		internal int LeafIndexFromItem(object item, int index)
		{
			int num = 0;
			CollectionViewGroupInternal collectionViewGroupInternal = this;
			while (collectionViewGroupInternal != null)
			{
				int num2 = 0;
				int count = collectionViewGroupInternal.Items.Count;
				while (num2 < count && (index >= 0 || !ItemsControl.EqualsEx(item, collectionViewGroupInternal.Items[num2])) && index != num2)
				{
					CollectionViewGroupInternal collectionViewGroupInternal2 = collectionViewGroupInternal.Items[num2] as CollectionViewGroupInternal;
					num += ((collectionViewGroupInternal2 == null) ? 1 : collectionViewGroupInternal2.ItemCount);
					num2++;
				}
				item = collectionViewGroupInternal;
				collectionViewGroupInternal = collectionViewGroupInternal.Parent;
				index = -1;
			}
			return num;
		}

		// Token: 0x060073A8 RID: 29608 RVA: 0x002119C4 File Offset: 0x0020FBC4
		internal object LeafAt(int index)
		{
			int i = 0;
			int count = base.Items.Count;
			while (i < count)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = base.Items[i] as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					if (index < collectionViewGroupInternal.ItemCount)
					{
						return collectionViewGroupInternal.LeafAt(index);
					}
					index -= collectionViewGroupInternal.ItemCount;
				}
				else
				{
					if (index == 0)
					{
						return base.Items[i];
					}
					index--;
				}
				i++;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x060073A9 RID: 29609 RVA: 0x00211A3A File Offset: 0x0020FC3A
		internal IEnumerator GetLeafEnumerator()
		{
			return new CollectionViewGroupInternal.LeafEnumerator(this);
		}

		// Token: 0x060073AA RID: 29610 RVA: 0x00211A44 File Offset: 0x0020FC44
		internal int Insert(object item, object seed, IComparer comparer)
		{
			int low = 0;
			if (this._groupComparer == null && this.GroupBy != null)
			{
				low = this.GroupBy.GroupNames.Count;
			}
			int num = this.FindIndex(item, seed, comparer, low, base.ProtectedItems.Count);
			this.ChangeCounts(item, 1);
			base.ProtectedItems.Insert(num, item);
			return num;
		}

		// Token: 0x060073AB RID: 29611 RVA: 0x00211AA0 File Offset: 0x0020FCA0
		protected virtual int FindIndex(object item, object seed, IComparer comparer, int low, int high)
		{
			int i;
			if (this._groupComparer == null)
			{
				if (comparer != null)
				{
					CollectionViewGroupInternal.IListComparer listComparer = comparer as CollectionViewGroupInternal.IListComparer;
					if (listComparer != null)
					{
						listComparer.Reset();
					}
					for (i = low; i < high; i++)
					{
						CollectionViewGroupInternal collectionViewGroupInternal = base.ProtectedItems[i] as CollectionViewGroupInternal;
						object obj = (collectionViewGroupInternal != null) ? collectionViewGroupInternal.SeedItem : base.ProtectedItems[i];
						if (obj != DependencyProperty.UnsetValue && comparer.Compare(seed, obj) < 0)
						{
							break;
						}
					}
				}
				else
				{
					i = high;
				}
			}
			else
			{
				i = low;
				while (i < high && this._groupComparer.Compare(item, base.ProtectedItems[i]) >= 0)
				{
					i++;
				}
			}
			return i;
		}

		// Token: 0x060073AC RID: 29612 RVA: 0x00211B44 File Offset: 0x0020FD44
		internal bool Move(object item, IList list, ref int oldIndex, ref int newIndex)
		{
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			int count = base.ProtectedItems.Count;
			int num4 = 0;
			for (;;)
			{
				if (num4 == oldIndex)
				{
					num = num3;
					if (num2 >= 0)
					{
						goto IL_6F;
					}
					num3++;
				}
				if (num4 == newIndex)
				{
					num2 = num3;
					if (num >= 0)
					{
						break;
					}
					num4++;
					oldIndex++;
				}
				if (num3 < count && ItemsControl.EqualsEx(base.ProtectedItems[num3], list[num4]))
				{
					num3++;
				}
				num4++;
			}
			num2--;
			IL_6F:
			if (num == num2)
			{
				return false;
			}
			int num5 = 0;
			int num6;
			int num7;
			int num8;
			if (num < num2)
			{
				num6 = num + 1;
				num7 = num2 + 1;
				num8 = this.LeafIndexFromItem(null, num);
			}
			else
			{
				num6 = num2;
				num7 = num;
				num8 = this.LeafIndexFromItem(null, num2);
			}
			for (int i = num6; i < num7; i++)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = base.Items[i] as CollectionViewGroupInternal;
				num5 += ((collectionViewGroupInternal == null) ? 1 : collectionViewGroupInternal.ItemCount);
			}
			if (num < num2)
			{
				oldIndex = num8;
				newIndex = oldIndex + num5;
			}
			else
			{
				newIndex = num8;
				oldIndex = newIndex + num5;
			}
			base.ProtectedItems.Move(num, num2);
			return true;
		}

		// Token: 0x060073AD RID: 29613 RVA: 0x00211C59 File Offset: 0x0020FE59
		protected virtual void OnGroupByChanged()
		{
			if (this.Parent != null)
			{
				this.Parent.OnGroupByChanged();
			}
		}

		// Token: 0x060073AE RID: 29614 RVA: 0x00211C6E File Offset: 0x0020FE6E
		internal void AddSubgroupToMap(object nameKey, CollectionViewGroupInternal subgroup)
		{
			if (nameKey == null)
			{
				nameKey = CollectionViewGroupInternal._nullGroupNameKey;
			}
			if (this._nameToGroupMap == null)
			{
				this._nameToGroupMap = new Hashtable();
			}
			this._nameToGroupMap[nameKey] = new WeakReference(subgroup);
			this.ScheduleMapCleanup();
		}

		// Token: 0x060073AF RID: 29615 RVA: 0x00211CA8 File Offset: 0x0020FEA8
		private void RemoveSubgroupFromMap(CollectionViewGroupInternal subgroup)
		{
			if (this._nameToGroupMap == null)
			{
				return;
			}
			object obj = null;
			foreach (object obj2 in this._nameToGroupMap.Keys)
			{
				WeakReference weakReference = this._nameToGroupMap[obj2] as WeakReference;
				if (weakReference != null && weakReference.Target == subgroup)
				{
					obj = obj2;
					break;
				}
			}
			if (obj != null)
			{
				this._nameToGroupMap.Remove(obj);
			}
			this.ScheduleMapCleanup();
		}

		// Token: 0x060073B0 RID: 29616 RVA: 0x00211D40 File Offset: 0x0020FF40
		internal CollectionViewGroupInternal GetSubgroupFromMap(object nameKey)
		{
			if (this._nameToGroupMap != null)
			{
				if (nameKey == null)
				{
					nameKey = CollectionViewGroupInternal._nullGroupNameKey;
				}
				WeakReference weakReference = this._nameToGroupMap[nameKey] as WeakReference;
				if (weakReference != null)
				{
					return weakReference.Target as CollectionViewGroupInternal;
				}
			}
			return null;
		}

		// Token: 0x060073B1 RID: 29617 RVA: 0x00211D81 File Offset: 0x0020FF81
		private void ScheduleMapCleanup()
		{
			if (!this._mapCleanupScheduled)
			{
				this._mapCleanupScheduled = true;
				Dispatcher.CurrentDispatcher.BeginInvoke(new Action(delegate()
				{
					this._mapCleanupScheduled = false;
					if (this._nameToGroupMap != null)
					{
						ArrayList arrayList = new ArrayList();
						foreach (object obj in this._nameToGroupMap.Keys)
						{
							WeakReference weakReference = this._nameToGroupMap[obj] as WeakReference;
							if (weakReference == null || !weakReference.IsAlive)
							{
								arrayList.Add(obj);
							}
						}
						foreach (object key in arrayList)
						{
							this._nameToGroupMap.Remove(key);
						}
					}
				}), DispatcherPriority.ContextIdle, new object[0]);
			}
		}

		// Token: 0x17001B7F RID: 7039
		// (get) Token: 0x060073B2 RID: 29618 RVA: 0x00211DB0 File Offset: 0x0020FFB0
		internal CollectionViewGroupInternal Parent
		{
			get
			{
				return this._parentGroup;
			}
		}

		// Token: 0x17001B80 RID: 7040
		// (get) Token: 0x060073B3 RID: 29619 RVA: 0x00211DB8 File Offset: 0x0020FFB8
		private bool IsExplicit
		{
			get
			{
				return this._isExplicit;
			}
		}

		// Token: 0x060073B4 RID: 29620 RVA: 0x00211DC0 File Offset: 0x0020FFC0
		protected void ChangeCounts(object item, int delta)
		{
			bool flag = !(item is CollectionViewGroup);
			using (CollectionViewGroupInternal.EmptyGroupRemover emptyGroupRemover = CollectionViewGroupInternal.EmptyGroupRemover.Create(flag && delta < 0))
			{
				for (CollectionViewGroupInternal collectionViewGroupInternal = this; collectionViewGroupInternal != null; collectionViewGroupInternal = collectionViewGroupInternal._parentGroup)
				{
					collectionViewGroupInternal.FullCount += delta;
					if (flag)
					{
						collectionViewGroupInternal.ProtectedItemCount += delta;
						if (collectionViewGroupInternal.ProtectedItemCount == 0)
						{
							emptyGroupRemover.RemoveEmptyGroup(collectionViewGroupInternal);
						}
					}
				}
			}
			this._version++;
		}

		// Token: 0x060073B5 RID: 29621 RVA: 0x00211E50 File Offset: 0x00210050
		private void OnGroupByChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnGroupByChanged();
		}

		// Token: 0x040037AE RID: 14254
		private GroupDescription _groupBy;

		// Token: 0x040037AF RID: 14255
		private CollectionViewGroupInternal _parentGroup;

		// Token: 0x040037B0 RID: 14256
		private IComparer _groupComparer;

		// Token: 0x040037B1 RID: 14257
		private int _fullCount = 1;

		// Token: 0x040037B2 RID: 14258
		private int _lastIndex;

		// Token: 0x040037B3 RID: 14259
		private int _version;

		// Token: 0x040037B4 RID: 14260
		private Hashtable _nameToGroupMap;

		// Token: 0x040037B5 RID: 14261
		private bool _mapCleanupScheduled;

		// Token: 0x040037B6 RID: 14262
		private bool _isExplicit;

		// Token: 0x040037B7 RID: 14263
		private static NamedObject _nullGroupNameKey = new NamedObject("NullGroupNameKey");

		// Token: 0x02000B44 RID: 2884
		internal class IListComparer : IComparer
		{
			// Token: 0x06008D89 RID: 36233 RVA: 0x00259AEB File Offset: 0x00257CEB
			internal IListComparer(IList list)
			{
				this.ResetList(list);
			}

			// Token: 0x06008D8A RID: 36234 RVA: 0x00259AFA File Offset: 0x00257CFA
			internal void Reset()
			{
				this._index = 0;
			}

			// Token: 0x06008D8B RID: 36235 RVA: 0x00259B03 File Offset: 0x00257D03
			internal void ResetList(IList list)
			{
				this._list = list;
				this._index = 0;
			}

			// Token: 0x06008D8C RID: 36236 RVA: 0x00259B14 File Offset: 0x00257D14
			public int Compare(object x, object y)
			{
				if (ItemsControl.EqualsEx(x, y))
				{
					return 0;
				}
				int num = (this._list != null) ? this._list.Count : 0;
				while (this._index < num)
				{
					object o = this._list[this._index];
					if (ItemsControl.EqualsEx(x, o))
					{
						return -1;
					}
					if (ItemsControl.EqualsEx(y, o))
					{
						return 1;
					}
					this._index++;
				}
				return 1;
			}

			// Token: 0x04004AC8 RID: 19144
			private int _index;

			// Token: 0x04004AC9 RID: 19145
			private IList _list;
		}

		// Token: 0x02000B45 RID: 2885
		private class LeafEnumerator : IEnumerator
		{
			// Token: 0x06008D8D RID: 36237 RVA: 0x00259B85 File Offset: 0x00257D85
			public LeafEnumerator(CollectionViewGroupInternal group)
			{
				this._group = group;
				this.DoReset();
			}

			// Token: 0x06008D8E RID: 36238 RVA: 0x00259B9A File Offset: 0x00257D9A
			void IEnumerator.Reset()
			{
				this.DoReset();
			}

			// Token: 0x06008D8F RID: 36239 RVA: 0x00259BA2 File Offset: 0x00257DA2
			private void DoReset()
			{
				this._version = this._group._version;
				this._index = -1;
				this._subEnum = null;
			}

			// Token: 0x06008D90 RID: 36240 RVA: 0x00259BC4 File Offset: 0x00257DC4
			bool IEnumerator.MoveNext()
			{
				if (this._group._version != this._version)
				{
					throw new InvalidOperationException();
				}
				while (this._subEnum == null || !this._subEnum.MoveNext())
				{
					this._index++;
					if (this._index >= this._group.Items.Count)
					{
						return false;
					}
					CollectionViewGroupInternal collectionViewGroupInternal = this._group.Items[this._index] as CollectionViewGroupInternal;
					if (collectionViewGroupInternal == null)
					{
						this._current = this._group.Items[this._index];
						this._subEnum = null;
						return true;
					}
					this._subEnum = collectionViewGroupInternal.GetLeafEnumerator();
				}
				this._current = this._subEnum.Current;
				return true;
			}

			// Token: 0x17001F7A RID: 8058
			// (get) Token: 0x06008D91 RID: 36241 RVA: 0x00259C8C File Offset: 0x00257E8C
			object IEnumerator.Current
			{
				get
				{
					if (this._index < 0 || this._index >= this._group.Items.Count)
					{
						throw new InvalidOperationException();
					}
					return this._current;
				}
			}

			// Token: 0x04004ACA RID: 19146
			private CollectionViewGroupInternal _group;

			// Token: 0x04004ACB RID: 19147
			private int _version;

			// Token: 0x04004ACC RID: 19148
			private int _index;

			// Token: 0x04004ACD RID: 19149
			private IEnumerator _subEnum;

			// Token: 0x04004ACE RID: 19150
			private object _current;
		}

		// Token: 0x02000B46 RID: 2886
		private class EmptyGroupRemover : IDisposable
		{
			// Token: 0x06008D92 RID: 36242 RVA: 0x00259CBB File Offset: 0x00257EBB
			public static CollectionViewGroupInternal.EmptyGroupRemover Create(bool isNeeded)
			{
				if (!isNeeded)
				{
					return null;
				}
				return new CollectionViewGroupInternal.EmptyGroupRemover();
			}

			// Token: 0x06008D93 RID: 36243 RVA: 0x00259CC7 File Offset: 0x00257EC7
			public void RemoveEmptyGroup(CollectionViewGroupInternal group)
			{
				if (this._toRemove == null)
				{
					this._toRemove = new List<CollectionViewGroupInternal>();
				}
				this._toRemove.Add(group);
			}

			// Token: 0x06008D94 RID: 36244 RVA: 0x00259CE8 File Offset: 0x00257EE8
			public void Dispose()
			{
				if (this._toRemove != null)
				{
					foreach (CollectionViewGroupInternal collectionViewGroupInternal in this._toRemove)
					{
						CollectionViewGroupInternal parent = collectionViewGroupInternal.Parent;
						if (parent != null && !collectionViewGroupInternal.IsExplicit)
						{
							parent.Remove(collectionViewGroupInternal, false);
						}
					}
				}
			}

			// Token: 0x04004ACF RID: 19151
			private List<CollectionViewGroupInternal> _toRemove;
		}
	}
}
