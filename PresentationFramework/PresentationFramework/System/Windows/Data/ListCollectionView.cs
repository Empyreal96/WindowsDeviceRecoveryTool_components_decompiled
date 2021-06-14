using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Represents the collection view for collections that implement <see cref="T:System.Collections.IList" />. </summary>
	// Token: 0x020001B2 RID: 434
	public class ListCollectionView : CollectionView, IComparer, IEditableCollectionViewAddNewItem, IEditableCollectionView, ICollectionViewLiveShaping, IItemProperties
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.ListCollectionView" /> class, using a supplied collection that implements <see cref="T:System.Collections.IList" />.</summary>
		/// <param name="list">The underlying collection, which must implement <see cref="T:System.Collections.IList" />.</param>
		// Token: 0x06001B69 RID: 7017 RVA: 0x00080A0C File Offset: 0x0007EC0C
		public ListCollectionView(IList list) : base(list)
		{
			if (base.AllowsCrossThreadChanges)
			{
				BindingOperations.AccessCollection(list, delegate
				{
					base.ClearPendingChanges();
					this.ShadowCollection = new ArrayList((ICollection)this.SourceCollection);
					this._internalList = this.ShadowCollection;
				}, false);
			}
			else
			{
				this._internalList = list;
			}
			if (this.InternalList.Count == 0)
			{
				base.SetCurrent(null, -1, 0);
			}
			else
			{
				base.SetCurrent(this.InternalList[0], 0, 1);
			}
			this._group = new CollectionViewGroupRoot(this);
			this._group.GroupDescriptionChanged += this.OnGroupDescriptionChanged;
			((INotifyCollectionChanged)this._group).CollectionChanged += this.OnGroupChanged;
			((INotifyCollectionChanged)this._group.GroupDescriptions).CollectionChanged += this.OnGroupByChanged;
		}

		/// <summary>Recreates the view.</summary>
		// Token: 0x06001B6A RID: 7018 RVA: 0x00080AF8 File Offset: 0x0007ECF8
		protected override void RefreshOverride()
		{
			if (base.AllowsCrossThreadChanges)
			{
				BindingOperations.AccessCollection(this.SourceCollection, delegate
				{
					object syncRoot = base.SyncRoot;
					lock (syncRoot)
					{
						base.ClearPendingChanges();
						this.ShadowCollection = new ArrayList((ICollection)this.SourceCollection);
					}
				}, false);
			}
			object currentItem = this.CurrentItem;
			int num = this.IsEmpty ? -1 : this.CurrentPosition;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			base.OnCurrentChanging();
			this.PrepareLocalArray();
			if (isCurrentBeforeFirst || this.IsEmpty)
			{
				base.SetCurrent(null, -1);
			}
			else if (isCurrentAfterLast)
			{
				base.SetCurrent(null, this.InternalCount);
			}
			else
			{
				int num2 = this.InternalIndexOf(currentItem);
				if (num2 < 0)
				{
					num2 = ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0);
					object newItem;
					if (num2 < this.InternalCount && (newItem = this.InternalItemAt(num2)) != CollectionView.NewItemPlaceholder)
					{
						base.SetCurrent(newItem, num2);
					}
					else
					{
						base.SetCurrent(null, -1);
					}
				}
				else
				{
					base.SetCurrent(currentItem, num2);
				}
			}
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			this.OnCurrentChanged();
			if (this.IsCurrentAfterLast != isCurrentAfterLast)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (num != this.CurrentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (currentItem != this.CurrentItem)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		/// <summary>Returns a value that indicates whether a given item belongs to the collection view.</summary>
		/// <param name="item">The object to check.</param>
		/// <returns>
		///     <see langword="true" /> if the item belongs to the collection view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B6B RID: 7019 RVA: 0x00080C39 File Offset: 0x0007EE39
		public override bool Contains(object item)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalContains(item);
		}

		/// <summary>Sets the item at the specified index to be the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> in the view.</summary>
		/// <param name="position">The index to set the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> to.</param>
		/// <returns>
		///     <see langword="true" /> if the resulting <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if the index is out of range. </exception>
		// Token: 0x06001B6C RID: 7020 RVA: 0x00080C48 File Offset: 0x0007EE48
		public override bool MoveCurrentToPosition(int position)
		{
			base.VerifyRefreshNotDeferred();
			if (position < -1 || position > this.InternalCount)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			if (position != this.CurrentPosition || !base.IsCurrentInSync)
			{
				object obj = (0 <= position && position < this.InternalCount) ? this.InternalItemAt(position) : null;
				if (obj != CollectionView.NewItemPlaceholder && base.OKToChangeCurrent())
				{
					bool isCurrentAfterLast = this.IsCurrentAfterLast;
					bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
					base.SetCurrent(obj, position);
					this.OnCurrentChanged();
					if (this.IsCurrentAfterLast != isCurrentAfterLast)
					{
						this.OnPropertyChanged("IsCurrentAfterLast");
					}
					if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
					{
						this.OnPropertyChanged("IsCurrentBeforeFirst");
					}
					this.OnPropertyChanged("CurrentPosition");
					this.OnPropertyChanged("CurrentItem");
				}
			}
			return this.IsCurrentInView;
		}

		/// <summary>Gets a value that indicates whether the collection view supports grouping.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection view supports grouping; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001B6D RID: 7021 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanGroup
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.ComponentModel.GroupDescription" /> objects that describe how the items in the collection are grouped in the view.</summary>
		/// <returns>A collection of <see cref="T:System.ComponentModel.GroupDescription" /> objects that describe how the items in the collection are grouped in the view.</returns>
		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001B6E RID: 7022 RVA: 0x00080D0F File Offset: 0x0007EF0F
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._group.GroupDescriptions;
			}
		}

		/// <summary>Gets the top-level groups.</summary>
		/// <returns>A read-only collection of the top-level groups, or <see langword="null" /> if there are no groups.</returns>
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001B6F RID: 7023 RVA: 0x00080D1C File Offset: 0x0007EF1C
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				if (!this.IsGrouping)
				{
					return null;
				}
				return this._group.Items;
			}
		}

		/// <summary>Returns a value that indicates whether the specified item in the underlying collection belongs to the view.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		///     <see langword="true" /> if the specified item belongs to the view or if there is not filter set on the collection view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B70 RID: 7024 RVA: 0x00080D33 File Offset: 0x0007EF33
		public override bool PassesFilter(object item)
		{
			return this.ActiveFilter == null || this.ActiveFilter(item);
		}

		/// <summary>Returns the index where the given data item belongs in the collection, or -1 if the index of that item is unknown. </summary>
		/// <param name="item">The object to check for in the collection.</param>
		/// <returns>The index of the item in the collection, or -1 if the item does not exist in the collection.</returns>
		// Token: 0x06001B71 RID: 7025 RVA: 0x00080D4B File Offset: 0x0007EF4B
		public override int IndexOf(object item)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalIndexOf(item);
		}

		/// <summary>Retrieves the item at the specified position in the view.</summary>
		/// <param name="index">The zero-based index at which the item is located.</param>
		/// <returns>The item at the specified position in the view.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">If <paramref name="index" /> is out of range.</exception>
		// Token: 0x06001B72 RID: 7026 RVA: 0x00080D5A File Offset: 0x0007EF5A
		public override object GetItemAt(int index)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalItemAt(index);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="o1">The first object to compare.</param>
		/// <param name="o2">The second object to compare.</param>
		/// <returns>A value that is less than zero means <paramref name="o1" /> is less than <paramref name="o2;" /> a value of zero means the objects are equal; and a value that is over zero means <paramref name="o1" /> is greater than <paramref name="o2" />.</returns>
		// Token: 0x06001B73 RID: 7027 RVA: 0x00080D69 File Offset: 0x0007EF69
		int IComparer.Compare(object o1, object o2)
		{
			return this.Compare(o1, o2);
		}

		/// <summary>Compares two objects and returns a value that indicates whether one is less than, equal to, or greater than the other.</summary>
		/// <param name="o1">The first object to compare.</param>
		/// <param name="o2">The second object to compare.</param>
		/// <returns>Less than zero if <paramref name="o1" /> is less than <paramref name="o2" />, zero if <paramref name="o1" /> and <paramref name="o2" /> are equal, or greater than zero if <paramref name="o1" /> is greater than <paramref name="o2" />.</returns>
		// Token: 0x06001B74 RID: 7028 RVA: 0x00080D74 File Offset: 0x0007EF74
		protected virtual int Compare(object o1, object o2)
		{
			if (this.IsGrouping)
			{
				int num = this.InternalIndexOf(o1);
				int num2 = this.InternalIndexOf(o2);
				return num - num2;
			}
			if (this.ActiveComparer != null)
			{
				return this.ActiveComparer.Compare(o1, o2);
			}
			int num3 = this.InternalList.IndexOf(o1);
			int num4 = this.InternalList.IndexOf(o2);
			return num3 - num4;
		}

		/// <summary>Returns an object that you can use to enumerate the items in the view.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that you can use to enumerate the items in the view.</returns>
		// Token: 0x06001B75 RID: 7029 RVA: 0x00080DD0 File Offset: 0x0007EFD0
		protected override IEnumerator GetEnumerator()
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalGetEnumerator();
		}

		/// <summary>Gets a collection of <see cref="T:System.ComponentModel.SortDescription" /> objects that describes how the items in the collection are sorted in the view.</summary>
		/// <returns>A collection of <see cref="T:System.ComponentModel.SortDescription" /> objects that describe how the items in the collection are sorted in the view.</returns>
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001B76 RID: 7030 RVA: 0x00080DDE File Offset: 0x0007EFDE
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				if (this._sort == null)
				{
					this.SetSortDescriptions(new SortDescriptionCollection());
				}
				return this._sort;
			}
		}

		/// <summary>Gets a value that indicates whether the collection view supports sorting.</summary>
		/// <returns>For a default instance of <see cref="T:System.Windows.Data.ListCollectionView" />, this property always returns <see langword="true" />.</returns>
		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001B77 RID: 7031 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanSort
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value that indicates whether the view supports callback-based filtering.</summary>
		/// <returns>For a default instance of <see cref="T:System.Windows.Data.ListCollectionView" />, this property always returns <see langword="true" />.</returns>
		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanFilter
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a method that is used to determine whether an item is suitable for inclusion in the view.</summary>
		/// <returns>A delegate that represents the method that is used to determine whether an item is suitable for inclusion in the view.</returns>
		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001B79 RID: 7033 RVA: 0x00080DF9 File Offset: 0x0007EFF9
		// (set) Token: 0x06001B7A RID: 7034 RVA: 0x00080E04 File Offset: 0x0007F004
		public override Predicate<object> Filter
		{
			get
			{
				return base.Filter;
			}
			set
			{
				if (base.AllowsCrossThreadChanges)
				{
					base.VerifyAccess();
				}
				if (this.IsAddingNew || this.IsEditingItem)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
					{
						"Filter"
					}));
				}
				base.Filter = value;
			}
		}

		/// <summary>Gets or sets a custom object that implements <see cref="T:System.Collections.IComparer" /> to sort items in the view.</summary>
		/// <returns>The sort criteria as an implementation of <see cref="T:System.Collections.IComparer" />.</returns>
		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001B7B RID: 7035 RVA: 0x00080E54 File Offset: 0x0007F054
		// (set) Token: 0x06001B7C RID: 7036 RVA: 0x00080E5C File Offset: 0x0007F05C
		public IComparer CustomSort
		{
			get
			{
				return this._customSort;
			}
			set
			{
				if (base.AllowsCrossThreadChanges)
				{
					base.VerifyAccess();
				}
				if (this.IsAddingNew || this.IsEditingItem)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
					{
						"CustomSort"
					}));
				}
				this._customSort = value;
				this.SetSortDescriptions(null);
				base.RefreshOrDefer();
			}
		}

		/// <summary>Gets or sets a delegate to select the <see cref="T:System.ComponentModel.GroupDescription" /> as a function of the parent group and its level. </summary>
		/// <returns>A method that provides the logic for the selection of the <see cref="T:System.ComponentModel.GroupDescription" /> as a function of the parent group and its level. The default value is <see langword="null" />.</returns>
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001B7D RID: 7037 RVA: 0x00080EB9 File Offset: 0x0007F0B9
		// (set) Token: 0x06001B7E RID: 7038 RVA: 0x00080EC8 File Offset: 0x0007F0C8
		[DefaultValue(null)]
		public virtual GroupDescriptionSelectorCallback GroupBySelector
		{
			get
			{
				return this._group.GroupBySelector;
			}
			set
			{
				if (!this.CanGroup)
				{
					throw new NotSupportedException();
				}
				if (this.IsAddingNew || this.IsEditingItem)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
					{
						"Grouping"
					}));
				}
				this._group.GroupBySelector = value;
				base.RefreshOrDefer();
			}
		}

		/// <summary>Gets the estimated number of records.</summary>
		/// <returns>One of the following:ValueMeaning-1Could not determine the count of the collection. This might be returned by a "virtualizing" view, where the view deliberately does not account for all items in the underlying collection because the view is trying to increase efficiency and minimize dependence on always having the whole collection available.any other integerThe count of the collection.</returns>
		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001B7F RID: 7039 RVA: 0x00080F23 File Offset: 0x0007F123
		public override int Count
		{
			get
			{
				base.VerifyRefreshNotDeferred();
				return this.InternalCount;
			}
		}

		/// <summary>Returns a value that indicates whether the resulting (filtered) view is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if the resulting view is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x00080F31 File Offset: 0x0007F131
		public override bool IsEmpty
		{
			get
			{
				return this.InternalCount == 0;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the list of items (after applying the sort and filters, if any) is already in the correct order for grouping.</summary>
		/// <returns>
		///     <see langword="true" /> if the list of items is already in the correct order for grouping; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001B81 RID: 7041 RVA: 0x00080F3C File Offset: 0x0007F13C
		// (set) Token: 0x06001B82 RID: 7042 RVA: 0x00080F49 File Offset: 0x0007F149
		public bool IsDataInGroupOrder
		{
			get
			{
				return this._group.IsDataInGroupOrder;
			}
			set
			{
				this._group.IsDataInGroupOrder = value;
			}
		}

		/// <summary>Gets or sets the position of the new item placeholder in the <see cref="T:System.Windows.Data.ListCollectionView" />.</summary>
		/// <returns>One of the enumeration values that specifies the position of the new item placeholder in the <see cref="T:System.Windows.Data.ListCollectionView" />.</returns>
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001B83 RID: 7043 RVA: 0x00080F57 File Offset: 0x0007F157
		// (set) Token: 0x06001B84 RID: 7044 RVA: 0x00080F60 File Offset: 0x0007F160
		public NewItemPlaceholderPosition NewItemPlaceholderPosition
		{
			get
			{
				return this._newItemPlaceholderPosition;
			}
			set
			{
				base.VerifyRefreshNotDeferred();
				if (value != this._newItemPlaceholderPosition && this.IsAddingNew)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
					{
						"NewItemPlaceholderPosition",
						"AddNew"
					}));
				}
				if (value != this._newItemPlaceholderPosition && this._isRemoving)
				{
					this.DeferAction(delegate
					{
						this.NewItemPlaceholderPosition = value;
					});
					return;
				}
				NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = null;
				int num = -1;
				int num2 = -1;
				switch (value)
				{
				case NewItemPlaceholderPosition.None:
					switch (this._newItemPlaceholderPosition)
					{
					case NewItemPlaceholderPosition.AtBeginning:
						num = 0;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, CollectionView.NewItemPlaceholder, num);
						break;
					case NewItemPlaceholderPosition.AtEnd:
						num = this.InternalCount - 1;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, CollectionView.NewItemPlaceholder, num);
						break;
					}
					break;
				case NewItemPlaceholderPosition.AtBeginning:
					switch (this._newItemPlaceholderPosition)
					{
					case NewItemPlaceholderPosition.None:
						num2 = 0;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, CollectionView.NewItemPlaceholder, num2);
						break;
					case NewItemPlaceholderPosition.AtEnd:
						num = this.InternalCount - 1;
						num2 = 0;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, CollectionView.NewItemPlaceholder, num2, num);
						break;
					}
					break;
				case NewItemPlaceholderPosition.AtEnd:
					switch (this._newItemPlaceholderPosition)
					{
					case NewItemPlaceholderPosition.None:
						num2 = this.InternalCount;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, CollectionView.NewItemPlaceholder, num2);
						break;
					case NewItemPlaceholderPosition.AtBeginning:
						num = 0;
						num2 = this.InternalCount - 1;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, CollectionView.NewItemPlaceholder, num2, num);
						break;
					}
					break;
				}
				if (notifyCollectionChangedEventArgs != null)
				{
					this._newItemPlaceholderPosition = value;
					if (!this.IsGrouping)
					{
						this.ProcessCollectionChangedWithAdjustedIndex(notifyCollectionChangedEventArgs, num, num2);
					}
					else
					{
						if (num >= 0)
						{
							int index = (num == 0) ? 0 : (this._group.Items.Count - 1);
							this._group.RemoveSpecialItem(index, CollectionView.NewItemPlaceholder, false);
						}
						if (num2 >= 0)
						{
							int index2 = (num2 == 0) ? 0 : this._group.Items.Count;
							this._group.InsertSpecialItem(index2, CollectionView.NewItemPlaceholder, false);
						}
					}
					this.OnPropertyChanged("NewItemPlaceholderPosition");
				}
			}
		}

		/// <summary>Gets a value that indicates whether a new item can be added to the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if a new item can be added to the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001B85 RID: 7045 RVA: 0x00081186 File Offset: 0x0007F386
		public bool CanAddNew
		{
			get
			{
				return !this.IsEditingItem && !this.SourceList.IsFixedSize && this.CanConstructItem;
			}
		}

		/// <summary>Gets a value that indicates whether a specified object can be added to the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if a specified object can be added to the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001B86 RID: 7046 RVA: 0x000811A5 File Offset: 0x0007F3A5
		public bool CanAddNewItem
		{
			get
			{
				return !this.IsEditingItem && !this.SourceList.IsFixedSize;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001B87 RID: 7047 RVA: 0x000811BF File Offset: 0x0007F3BF
		private bool CanConstructItem
		{
			get
			{
				if (!this._isItemConstructorValid)
				{
					this.EnsureItemConstructor();
				}
				return this._itemConstructor != null;
			}
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000811DC File Offset: 0x0007F3DC
		private void EnsureItemConstructor()
		{
			if (!this._isItemConstructorValid)
			{
				Type itemType = base.GetItemType(true);
				if (itemType != null)
				{
					this._itemConstructor = itemType.GetConstructor(Type.EmptyTypes);
					this._isItemConstructorValid = true;
				}
			}
		}

		/// <summary>Starts an add transaction and returns the pending new item.</summary>
		/// <returns>The pending new item.</returns>
		// Token: 0x06001B89 RID: 7049 RVA: 0x0008121C File Offset: 0x0007F41C
		public object AddNew()
		{
			base.VerifyRefreshNotDeferred();
			if (this.IsEditingItem)
			{
				this.CommitEdit();
			}
			this.CommitNew();
			if (!this.CanAddNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
				{
					"AddNew"
				}));
			}
			return this.AddNewCommon(this._itemConstructor.Invoke(null));
		}

		/// <summary>Adds the specified object to the collection.</summary>
		/// <param name="newItem">The object to add to the collection.</param>
		/// <returns>The object that was added to the collection.</returns>
		/// <exception cref="T:System.InvalidOperationException">An object cannot be added to the <see cref="T:System.Windows.Data.ListCollectionView" /> by using the <see cref="M:System.Windows.Data.ListCollectionView.AddNewItem(System.Object)" /> method.</exception>
		// Token: 0x06001B8A RID: 7050 RVA: 0x0008127C File Offset: 0x0007F47C
		public object AddNewItem(object newItem)
		{
			base.VerifyRefreshNotDeferred();
			if (this.IsEditingItem)
			{
				this.CommitEdit();
			}
			this.CommitNew();
			if (!this.CanAddNewItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
				{
					"AddNewItem"
				}));
			}
			return this.AddNewCommon(newItem);
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000812D0 File Offset: 0x0007F4D0
		private object AddNewCommon(object newItem)
		{
			BindingOperations.AccessCollection(this.SourceList, delegate
			{
				this.ProcessPendingChanges();
				this._newItemIndex = -2;
				int index = this.SourceList.Add(newItem);
				if (!(this.SourceList is INotifyCollectionChanged))
				{
					if (!ItemsControl.EqualsEx(newItem, this.SourceList[index]))
					{
						index = this.SourceList.IndexOf(newItem);
					}
					this.BeginAddNew(newItem, index);
				}
			}, true);
			this.MoveCurrentTo(newItem);
			ISupportInitialize supportInitialize = newItem as ISupportInitialize;
			if (supportInitialize != null)
			{
				supportInitialize.BeginInit();
			}
			IEditableObject editableObject = newItem as IEditableObject;
			if (editableObject != null)
			{
				editableObject.BeginEdit();
			}
			return newItem;
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x00081348 File Offset: 0x0007F548
		private void BeginAddNew(object newItem, int index)
		{
			this.SetNewItem(newItem);
			this._newItemIndex = index;
			int num = -1;
			switch (this.NewItemPlaceholderPosition)
			{
			case NewItemPlaceholderPosition.None:
				num = (this.UsesLocalArray ? (this.InternalCount - 1) : this._newItemIndex);
				break;
			case NewItemPlaceholderPosition.AtBeginning:
				num = 1;
				break;
			case NewItemPlaceholderPosition.AtEnd:
				num = this.InternalCount - 2;
				break;
			}
			this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, num), -1, num);
		}

		/// <summary>Ends the add transaction and saves the pending new item.</summary>
		// Token: 0x06001B8D RID: 7053 RVA: 0x000813B8 File Offset: 0x0007F5B8
		public void CommitNew()
		{
			if (this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
				{
					"CommitNew",
					"EditItem"
				}));
			}
			base.VerifyRefreshNotDeferred();
			if (this._newItem == CollectionView.NoNewItem)
			{
				return;
			}
			if (this.IsGrouping)
			{
				this.CommitNewForGrouping();
				return;
			}
			int num = 0;
			switch (this.NewItemPlaceholderPosition)
			{
			case NewItemPlaceholderPosition.None:
				num = (this.UsesLocalArray ? (this.InternalCount - 1) : this._newItemIndex);
				break;
			case NewItemPlaceholderPosition.AtBeginning:
				num = 1;
				break;
			case NewItemPlaceholderPosition.AtEnd:
				num = this.InternalCount - 2;
				break;
			}
			object obj = this.EndAddNew(false);
			int num2 = this.AdjustBefore(NotifyCollectionChangedAction.Add, obj, this._newItemIndex);
			if (num2 < 0)
			{
				this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, obj, num), num, -1);
				return;
			}
			if (num == num2)
			{
				if (this.UsesLocalArray)
				{
					if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
					{
						num2--;
					}
					this.InternalList.Insert(num2, obj);
					return;
				}
			}
			else
			{
				this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, obj, num2, num), num, num2);
			}
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000814C0 File Offset: 0x0007F6C0
		private void CommitNewForGrouping()
		{
			int index;
			switch (this.NewItemPlaceholderPosition)
			{
			default:
				index = this._group.Items.Count - 1;
				break;
			case NewItemPlaceholderPosition.AtBeginning:
				index = 1;
				break;
			case NewItemPlaceholderPosition.AtEnd:
				index = this._group.Items.Count - 2;
				break;
			}
			int newItemIndex = this._newItemIndex;
			object obj = this.EndAddNew(false);
			this._group.RemoveSpecialItem(index, obj, false);
			this.ProcessCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, obj, newItemIndex));
		}

		/// <summary>Ends the add transaction and discards the pending new item.</summary>
		// Token: 0x06001B8F RID: 7055 RVA: 0x00081540 File Offset: 0x0007F740
		public void CancelNew()
		{
			if (this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
				{
					"CancelNew",
					"EditItem"
				}));
			}
			base.VerifyRefreshNotDeferred();
			if (this._newItem == CollectionView.NoNewItem)
			{
				return;
			}
			BindingOperations.AccessCollection(this.SourceList, delegate
			{
				base.ProcessPendingChanges();
				this.SourceList.RemoveAt(this._newItemIndex);
				if (this._newItem != CollectionView.NoNewItem)
				{
					int num = this.AdjustBefore(NotifyCollectionChangedAction.Remove, this._newItem, this._newItemIndex);
					object changedItem = this.EndAddNew(true);
					this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItem, num), num, -1);
				}
			}, true);
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000815A8 File Offset: 0x0007F7A8
		private object EndAddNew(bool cancel)
		{
			object newItem = this._newItem;
			this.SetNewItem(CollectionView.NoNewItem);
			IEditableObject editableObject = newItem as IEditableObject;
			if (editableObject != null)
			{
				if (cancel)
				{
					editableObject.CancelEdit();
				}
				else
				{
					editableObject.EndEdit();
				}
			}
			ISupportInitialize supportInitialize = newItem as ISupportInitialize;
			if (supportInitialize != null)
			{
				supportInitialize.EndInit();
			}
			return newItem;
		}

		/// <summary>Gets a value that indicates whether an add transaction is in progress.</summary>
		/// <returns>
		///     <see langword="true" /> if an add transaction is in progress; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x000815F3 File Offset: 0x0007F7F3
		public bool IsAddingNew
		{
			get
			{
				return this._newItem != CollectionView.NoNewItem;
			}
		}

		/// <summary>Gets the item that is being added during the current add transaction.</summary>
		/// <returns>The item that is being added if <see cref="P:System.Windows.Data.ListCollectionView.IsAddingNew" /> is <see langword="true" />; otherwise, <see langword="null" />.</returns>
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x00081605 File Offset: 0x0007F805
		public object CurrentAddItem
		{
			get
			{
				if (!this.IsAddingNew)
				{
					return null;
				}
				return this._newItem;
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x00081617 File Offset: 0x0007F817
		private void SetNewItem(object item)
		{
			if (!ItemsControl.EqualsEx(item, this._newItem))
			{
				this._newItem = item;
				this.OnPropertyChanged("CurrentAddItem");
				this.OnPropertyChanged("IsAddingNew");
				this.OnPropertyChanged("CanRemove");
			}
		}

		/// <summary>Gets a value that indicates whether an item can be removed from the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if an item can be removed from the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001B94 RID: 7060 RVA: 0x0008164F File Offset: 0x0007F84F
		public bool CanRemove
		{
			get
			{
				return !this.IsEditingItem && !this.IsAddingNew && !this.SourceList.IsFixedSize;
			}
		}

		/// <summary>Removes the item at the specified position from the collection.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0 or greater than the number of items in the collection view.</exception>
		// Token: 0x06001B95 RID: 7061 RVA: 0x00081674 File Offset: 0x0007F874
		public void RemoveAt(int index)
		{
			if (this.IsEditingItem || this.IsAddingNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"RemoveAt"
				}));
			}
			base.VerifyRefreshNotDeferred();
			this.RemoveImpl(this.GetItemAt(index), index);
		}

		/// <summary>Removes the specified item from the collection.</summary>
		/// <param name="item">The item to remove.</param>
		// Token: 0x06001B96 RID: 7062 RVA: 0x000816C4 File Offset: 0x0007F8C4
		public void Remove(object item)
		{
			if (this.IsEditingItem || this.IsAddingNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Remove"
				}));
			}
			base.VerifyRefreshNotDeferred();
			int num = this.InternalIndexOf(item);
			if (num >= 0)
			{
				this.RemoveImpl(item, num);
			}
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x0008171C File Offset: 0x0007F91C
		private void RemoveImpl(object item, int index)
		{
			if (item == CollectionView.NewItemPlaceholder)
			{
				throw new InvalidOperationException(SR.Get("RemovingPlaceholder"));
			}
			BindingOperations.AccessCollection(this.SourceList, delegate
			{
				this.ProcessPendingChanges();
				if (index >= this.InternalCount || !ItemsControl.EqualsEx(item, this.GetItemAt(index)))
				{
					index = this.InternalIndexOf(item);
					if (index < 0)
					{
						return;
					}
				}
				int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
				int index2 = index - num;
				bool flag = !(this.SourceList is INotifyCollectionChanged);
				try
				{
					this._isRemoving = true;
					if (this.UsesLocalArray || this.IsGrouping)
					{
						if (flag)
						{
							index2 = this.SourceList.IndexOf(item);
							this.SourceList.RemoveAt(index2);
						}
						else
						{
							this.SourceList.Remove(item);
						}
					}
					else
					{
						this.SourceList.RemoveAt(index2);
					}
					if (flag)
					{
						this.ProcessCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index2));
					}
				}
				finally
				{
					this._isRemoving = false;
					this.DoDeferredActions();
				}
			}, true);
		}

		/// <summary>Begins an edit transaction of the specified item.</summary>
		/// <param name="item">The item to edit.</param>
		// Token: 0x06001B98 RID: 7064 RVA: 0x0008177C File Offset: 0x0007F97C
		public void EditItem(object item)
		{
			base.VerifyRefreshNotDeferred();
			if (item == CollectionView.NewItemPlaceholder)
			{
				throw new ArgumentException(SR.Get("CannotEditPlaceholder"), "item");
			}
			if (this.IsAddingNew)
			{
				if (ItemsControl.EqualsEx(item, this._newItem))
				{
					return;
				}
				this.CommitNew();
			}
			this.CommitEdit();
			this.SetEditItem(item);
			IEditableObject editableObject = item as IEditableObject;
			if (editableObject != null)
			{
				editableObject.BeginEdit();
			}
		}

		/// <summary>Ends the edit transaction and saves the pending changes.</summary>
		// Token: 0x06001B99 RID: 7065 RVA: 0x000817E8 File Offset: 0x0007F9E8
		public void CommitEdit()
		{
			if (this.IsAddingNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
				{
					"CommitEdit",
					"AddNew"
				}));
			}
			base.VerifyRefreshNotDeferred();
			if (this._editItem == null)
			{
				return;
			}
			object editItem = this._editItem;
			IEditableObject editableObject = this._editItem as IEditableObject;
			this.SetEditItem(null);
			if (editableObject != null)
			{
				editableObject.EndEdit();
			}
			int num = this.InternalIndexOf(editItem);
			bool flag = num >= 0;
			bool flag2 = flag ? this.PassesFilter(editItem) : (this.SourceList.Contains(editItem) && this.PassesFilter(editItem));
			if (this.IsGrouping)
			{
				if (flag)
				{
					this.RemoveItemFromGroups(editItem);
				}
				if (flag2)
				{
					LiveShapingList liveShapingList = this.InternalList as LiveShapingList;
					LiveShapingItem lsi = (liveShapingList == null) ? null : liveShapingList.ItemAt(liveShapingList.IndexOf(editItem));
					this.AddItemToGroups(editItem, lsi);
				}
				return;
			}
			if (this.UsesLocalArray)
			{
				IList internalList = this.InternalList;
				int num2 = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
				int num3 = -1;
				if (flag)
				{
					if (!flag2)
					{
						this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, editItem, num), num, -1);
						return;
					}
					if (this.ActiveComparer != null)
					{
						int num4 = num - num2;
						if (num4 > 0 && this.ActiveComparer.Compare(internalList[num4 - 1], editItem) > 0)
						{
							num3 = internalList.Search(0, num4, editItem, this.ActiveComparer);
							if (num3 < 0)
							{
								num3 = ~num3;
							}
						}
						else if (num4 < internalList.Count - 1 && this.ActiveComparer.Compare(editItem, internalList[num4 + 1]) > 0)
						{
							num3 = internalList.Search(num4 + 1, internalList.Count - num4 - 1, editItem, this.ActiveComparer);
							if (num3 < 0)
							{
								num3 = ~num3;
							}
							num3--;
						}
						if (num3 >= 0)
						{
							this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, editItem, num3 + num2, num), num, num3 + num2);
							return;
						}
					}
				}
				else if (flag2)
				{
					num3 = this.AdjustBefore(NotifyCollectionChangedAction.Add, editItem, this.SourceList.IndexOf(editItem));
					this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, editItem, num3 + num2), -1, num3 + num2);
				}
			}
		}

		/// <summary>Ends the edit transaction, and if possible, restores the original value to the item.</summary>
		// Token: 0x06001B9A RID: 7066 RVA: 0x00081A08 File Offset: 0x0007FC08
		public void CancelEdit()
		{
			if (this.IsAddingNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
				{
					"CancelEdit",
					"AddNew"
				}));
			}
			base.VerifyRefreshNotDeferred();
			if (this._editItem == null)
			{
				return;
			}
			IEditableObject editableObject = this._editItem as IEditableObject;
			this.SetEditItem(null);
			if (editableObject != null)
			{
				editableObject.CancelEdit();
				return;
			}
			throw new InvalidOperationException(SR.Get("CancelEditNotSupported"));
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x00081A80 File Offset: 0x0007FC80
		private void ImplicitlyCancelEdit()
		{
			IEditableObject editableObject = this._editItem as IEditableObject;
			this.SetEditItem(null);
			if (editableObject != null)
			{
				editableObject.CancelEdit();
			}
		}

		/// <summary>Gets a value that indicates whether the collection view can discard pending changes and restore the original values of an edited object.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection view can discard pending changes and restore the original values of an edited object; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x00081AA9 File Offset: 0x0007FCA9
		public bool CanCancelEdit
		{
			get
			{
				return this._editItem is IEditableObject;
			}
		}

		/// <summary>Gets a value that indicates whether an edit transaction is in progress.</summary>
		/// <returns>
		///     <see langword="true" /> if an edit transaction is in progress; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001B9D RID: 7069 RVA: 0x00081AB9 File Offset: 0x0007FCB9
		public bool IsEditingItem
		{
			get
			{
				return this._editItem != null;
			}
		}

		/// <summary>Gets the item in the collection that is being edited.</summary>
		/// <returns>The item in the collection that is being edited if <see cref="P:System.Windows.Data.ListCollectionView.IsEditingItem" /> is <see langword="true" />; otherwise, <see langword="null" />.</returns>
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001B9E RID: 7070 RVA: 0x00081AC4 File Offset: 0x0007FCC4
		public object CurrentEditItem
		{
			get
			{
				return this._editItem;
			}
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x00081ACC File Offset: 0x0007FCCC
		private void SetEditItem(object item)
		{
			if (!ItemsControl.EqualsEx(item, this._editItem))
			{
				this._editItem = item;
				this.OnPropertyChanged("CurrentEditItem");
				this.OnPropertyChanged("IsEditingItem");
				this.OnPropertyChanged("CanCancelEdit");
				this.OnPropertyChanged("CanAddNew");
				this.OnPropertyChanged("CanAddNewItem");
				this.OnPropertyChanged("CanRemove");
			}
		}

		/// <summary>Gets a value that indicates whether the collection view supports turning sorting data in real time on or off.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001BA0 RID: 7072 RVA: 0x00016748 File Offset: 0x00014948
		public bool CanChangeLiveSorting
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value that indicates whether the collection view supports turning filtering data in real time on or off.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x00016748 File Offset: 0x00014948
		public bool CanChangeLiveFiltering
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value that indicates whether the collection view supports turning grouping data in real time on or off.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001BA2 RID: 7074 RVA: 0x00016748 File Offset: 0x00014948
		public bool CanChangeLiveGrouping
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a value that indicates whether sorting in real time is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if sorting data in real time is enabled; <see langword="false" /> if live sorting is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live sorting.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <see cref="P:System.Windows.Data.ListCollectionView.IsLiveFiltering" /> cannot be set to <see langword="null" />.</exception>
		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001BA3 RID: 7075 RVA: 0x00081B30 File Offset: 0x0007FD30
		// (set) Token: 0x06001BA4 RID: 7076 RVA: 0x00081B38 File Offset: 0x0007FD38
		public bool? IsLiveSorting
		{
			get
			{
				return this._isLiveSorting;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value != this._isLiveSorting)
				{
					this._isLiveSorting = value;
					this.RebuildLocalArray();
					this.OnPropertyChanged("IsLiveSorting");
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether filtering data in real time is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if filtering data in real time is enabled; <see langword="false" /> if live filtering is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live filtering.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <see cref="P:System.Windows.Data.ListCollectionView.IsLiveFiltering" /> cannot be set to <see langword="null" />.</exception>
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001BA5 RID: 7077 RVA: 0x00081BA2 File Offset: 0x0007FDA2
		// (set) Token: 0x06001BA6 RID: 7078 RVA: 0x00081BAC File Offset: 0x0007FDAC
		public bool? IsLiveFiltering
		{
			get
			{
				return this._isLiveFiltering;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value != this._isLiveFiltering)
				{
					this._isLiveFiltering = value;
					this.RebuildLocalArray();
					this.OnPropertyChanged("IsLiveFiltering");
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether grouping data in real time is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if grouping data in real time is enabled; <see langword="false" /> if live grouping is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live grouping.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <see cref="P:System.Windows.Data.ListCollectionView.IsLiveGrouping" /> cannot be set to <see langword="null" />.</exception>
		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x00081C16 File Offset: 0x0007FE16
		// (set) Token: 0x06001BA8 RID: 7080 RVA: 0x00081C20 File Offset: 0x0007FE20
		public bool? IsLiveGrouping
		{
			get
			{
				return this._isLiveGrouping;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value != this._isLiveGrouping)
				{
					this._isLiveGrouping = value;
					this.RebuildLocalArray();
					this.OnPropertyChanged("IsLiveGrouping");
				}
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x00081C8C File Offset: 0x0007FE8C
		private bool IsLiveShaping
		{
			get
			{
				return this.IsLiveSorting == true || this.IsLiveFiltering == true || this.IsLiveGrouping == true;
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in sorting data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in sorting data in real time.</returns>
		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001BAA RID: 7082 RVA: 0x00081CF5 File Offset: 0x0007FEF5
		public ObservableCollection<string> LiveSortingProperties
		{
			get
			{
				if (this._liveSortingProperties == null)
				{
					this._liveSortingProperties = new ObservableCollection<string>();
					this._liveSortingProperties.CollectionChanged += this.OnLivePropertyListChanged;
				}
				return this._liveSortingProperties;
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in filtering data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in filtering data in real time.</returns>
		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001BAB RID: 7083 RVA: 0x00081D27 File Offset: 0x0007FF27
		public ObservableCollection<string> LiveFilteringProperties
		{
			get
			{
				if (this._liveFilteringProperties == null)
				{
					this._liveFilteringProperties = new ObservableCollection<string>();
					this._liveFilteringProperties.CollectionChanged += this.OnLivePropertyListChanged;
				}
				return this._liveFilteringProperties;
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in grouping data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in grouping data in real time.</returns>
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001BAC RID: 7084 RVA: 0x00081D59 File Offset: 0x0007FF59
		public ObservableCollection<string> LiveGroupingProperties
		{
			get
			{
				if (this._liveGroupingProperties == null)
				{
					this._liveGroupingProperties = new ObservableCollection<string>();
					this._liveGroupingProperties.CollectionChanged += this.OnLivePropertyListChanged;
				}
				return this._liveGroupingProperties;
			}
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x00081D8B File Offset: 0x0007FF8B
		private void OnLivePropertyListChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.IsLiveShaping)
			{
				this.RebuildLocalArray();
			}
		}

		/// <summary>Gets a collection of objects that describes the properties of the items in the collection.</summary>
		/// <returns>A collection of objects that describes the properties of the items in the collection.</returns>
		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001BAE RID: 7086 RVA: 0x0007BAEE File Offset: 0x00079CEE
		public ReadOnlyCollection<ItemPropertyInfo> ItemProperties
		{
			get
			{
				return base.GetItemProperties();
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Data.CollectionView.AllowsCrossThreadChanges" /> property changes.</summary>
		// Token: 0x06001BAF RID: 7087 RVA: 0x00081D9B File Offset: 0x0007FF9B
		protected override void OnAllowsCrossThreadChangesChanged()
		{
			if (base.AllowsCrossThreadChanges)
			{
				BindingOperations.AccessCollection(this.SourceCollection, delegate
				{
					object syncRoot = base.SyncRoot;
					lock (syncRoot)
					{
						base.ClearPendingChanges();
						this.ShadowCollection = new ArrayList((ICollection)this.SourceCollection);
						if (!this.UsesLocalArray)
						{
							this._internalList = this.ShadowCollection;
						}
					}
				}, false);
				return;
			}
			this.ShadowCollection = null;
			if (!this.UsesLocalArray)
			{
				this._internalList = this.SourceList;
			}
		}

		/// <summary>Called by the base class to notify the derived class that a <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event has been posted to the message queue.</summary>
		/// <param name="args">The <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> object that is added to the change log.</param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="args" /> is <see langword="null" />. </exception>
		// Token: 0x06001BB0 RID: 7088 RVA: 0x00002137 File Offset: 0x00000337
		[Obsolete("Replaced by OnAllowsCrossThreadChangesChanged")]
		protected override void OnBeginChangeLogging(NotifyCollectionChangedEventArgs args)
		{
		}

		/// <summary>Handles <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> events.</summary>
		/// <param name="args">The <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> object to process.</param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="args" /> is <see langword="null" />. </exception>
		// Token: 0x06001BB1 RID: 7089 RVA: 0x00081DDC File Offset: 0x0007FFDC
		protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			this.ValidateCollectionChangedEventArgs(args);
			if (!this._isItemConstructorValid)
			{
				switch (args.Action)
				{
				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Reset:
					this.OnPropertyChanged("CanAddNew");
					break;
				}
			}
			int num = -1;
			int num2 = -1;
			if (base.AllowsCrossThreadChanges && args.Action != NotifyCollectionChangedAction.Reset)
			{
				if ((args.Action != NotifyCollectionChangedAction.Remove && args.NewStartingIndex < 0) || (args.Action != NotifyCollectionChangedAction.Add && args.OldStartingIndex < 0))
				{
					return;
				}
				this.AdjustShadowCopy(args);
			}
			if (args.Action == NotifyCollectionChangedAction.Reset)
			{
				if (this.IsEditingItem)
				{
					this.ImplicitlyCancelEdit();
				}
				if (this.IsAddingNew)
				{
					this._newItemIndex = this.SourceList.IndexOf(this._newItem);
					if (this._newItemIndex < 0)
					{
						this.EndAddNew(true);
					}
				}
				base.RefreshOrDefer();
				return;
			}
			if (args.Action == NotifyCollectionChangedAction.Add && this._newItemIndex == -2)
			{
				this.BeginAddNew(args.NewItems[0], args.NewStartingIndex);
				return;
			}
			if (args.Action != NotifyCollectionChangedAction.Remove)
			{
				num2 = this.AdjustBefore(NotifyCollectionChangedAction.Add, args.NewItems[0], args.NewStartingIndex);
			}
			if (args.Action != NotifyCollectionChangedAction.Add)
			{
				num = this.AdjustBefore(NotifyCollectionChangedAction.Remove, args.OldItems[0], args.OldStartingIndex);
				if (this.UsesLocalArray && num >= 0 && num < num2)
				{
					num2--;
				}
			}
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (this.IsAddingNew && args.NewStartingIndex <= this._newItemIndex)
				{
					this._newItemIndex++;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
			{
				if (this.IsAddingNew && args.OldStartingIndex < this._newItemIndex)
				{
					this._newItemIndex--;
				}
				object obj = args.OldItems[0];
				if (obj == this.CurrentEditItem)
				{
					this.ImplicitlyCancelEdit();
				}
				else if (obj == this.CurrentAddItem)
				{
					this.EndAddNew(true);
				}
				break;
			}
			case NotifyCollectionChangedAction.Move:
				if (this.IsAddingNew)
				{
					if (args.OldStartingIndex == this._newItemIndex)
					{
						this._newItemIndex = args.NewStartingIndex;
					}
					else if (args.OldStartingIndex < this._newItemIndex && this._newItemIndex <= args.NewStartingIndex)
					{
						this._newItemIndex--;
					}
					else if (args.NewStartingIndex <= this._newItemIndex && this._newItemIndex < args.OldStartingIndex)
					{
						this._newItemIndex++;
					}
				}
				if (this.ActiveComparer != null && num == num2)
				{
					return;
				}
				break;
			}
			this.ProcessCollectionChangedWithAdjustedIndex(args, num, num2);
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x00082088 File Offset: 0x00080288
		private void ProcessCollectionChangedWithAdjustedIndex(NotifyCollectionChangedEventArgs args, int adjustedOldIndex, int adjustedNewIndex)
		{
			NotifyCollectionChangedAction notifyCollectionChangedAction = args.Action;
			if (adjustedOldIndex == adjustedNewIndex && adjustedOldIndex >= 0)
			{
				notifyCollectionChangedAction = NotifyCollectionChangedAction.Replace;
			}
			else if (adjustedOldIndex == -1)
			{
				if (adjustedNewIndex < 0 && args.Action != NotifyCollectionChangedAction.Add)
				{
					notifyCollectionChangedAction = NotifyCollectionChangedAction.Remove;
				}
			}
			else if (adjustedOldIndex < -1)
			{
				if (adjustedNewIndex >= 0)
				{
					notifyCollectionChangedAction = NotifyCollectionChangedAction.Add;
				}
				else if (notifyCollectionChangedAction == NotifyCollectionChangedAction.Move)
				{
					return;
				}
			}
			else if (adjustedNewIndex < 0)
			{
				notifyCollectionChangedAction = NotifyCollectionChangedAction.Remove;
			}
			else
			{
				notifyCollectionChangedAction = NotifyCollectionChangedAction.Move;
			}
			int num = this.IsGrouping ? 0 : ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? (this.IsAddingNew ? 2 : 1) : 0);
			int currentPosition = this.CurrentPosition;
			int currentPosition2 = this.CurrentPosition;
			object currentItem = this.CurrentItem;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			object obj = (args.OldItems != null && args.OldItems.Count > 0) ? args.OldItems[0] : null;
			object obj2 = (args.NewItems != null && args.NewItems.Count > 0) ? args.NewItems[0] : null;
			LiveShapingList liveShapingList = this.InternalList as LiveShapingList;
			NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = null;
			switch (notifyCollectionChangedAction)
			{
			case NotifyCollectionChangedAction.Add:
			{
				if (adjustedNewIndex == -2)
				{
					if (liveShapingList != null && this.IsLiveFiltering == true)
					{
						liveShapingList.AddFilteredItem(obj2);
					}
					return;
				}
				bool flag = obj2 == CollectionView.NewItemPlaceholder || (this.IsAddingNew && ItemsControl.EqualsEx(this._newItem, obj2));
				if (this.UsesLocalArray && !flag)
				{
					this.InternalList.Insert(adjustedNewIndex - num, obj2);
				}
				if (!this.IsGrouping)
				{
					this.AdjustCurrencyForAdd(adjustedNewIndex);
					args = new NotifyCollectionChangedEventArgs(notifyCollectionChangedAction, obj2, adjustedNewIndex);
				}
				else
				{
					LiveShapingItem lsi = (liveShapingList == null || flag) ? null : liveShapingList.ItemAt(adjustedNewIndex - num);
					this.AddItemToGroups(obj2, lsi);
				}
				break;
			}
			case NotifyCollectionChangedAction.Remove:
				if (adjustedOldIndex == -2)
				{
					if (liveShapingList != null && this.IsLiveFiltering == true)
					{
						liveShapingList.RemoveFilteredItem(obj);
					}
					return;
				}
				if (this.UsesLocalArray)
				{
					int num2 = adjustedOldIndex - num;
					if (num2 < this.InternalList.Count && ItemsControl.EqualsEx(this.ItemFrom(this.InternalList[num2]), obj))
					{
						this.InternalList.RemoveAt(num2);
					}
				}
				if (!this.IsGrouping)
				{
					this.AdjustCurrencyForRemove(adjustedOldIndex);
					args = new NotifyCollectionChangedEventArgs(notifyCollectionChangedAction, args.OldItems[0], adjustedOldIndex);
				}
				else
				{
					this.RemoveItemFromGroups(obj);
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (adjustedOldIndex == -2)
				{
					if (liveShapingList != null && this.IsLiveFiltering == true)
					{
						liveShapingList.ReplaceFilteredItem(obj, obj2);
					}
					return;
				}
				if (this.UsesLocalArray)
				{
					this.InternalList[adjustedOldIndex - num] = obj2;
				}
				if (!this.IsGrouping)
				{
					this.AdjustCurrencyForReplace(adjustedOldIndex);
					args = new NotifyCollectionChangedEventArgs(notifyCollectionChangedAction, args.NewItems[0], args.OldItems[0], adjustedOldIndex);
				}
				else
				{
					LiveShapingItem lsi = (liveShapingList == null) ? null : liveShapingList.ItemAt(adjustedNewIndex - num);
					this.RemoveItemFromGroups(obj);
					this.AddItemToGroups(obj2, lsi);
				}
				break;
			case NotifyCollectionChangedAction.Move:
			{
				bool flag2 = ItemsControl.EqualsEx(obj, obj2);
				if (this.UsesLocalArray && (liveShapingList == null || !liveShapingList.IsRestoringLiveSorting))
				{
					int num3 = adjustedOldIndex - num;
					int num4 = adjustedNewIndex - num;
					if (num3 < this.InternalList.Count && ItemsControl.EqualsEx(this.InternalList[num3], obj))
					{
						if (CollectionView.NewItemPlaceholder != obj2)
						{
							this.InternalList.Move(num3, num4);
							if (!flag2)
							{
								this.InternalList[num4] = obj2;
							}
						}
						else
						{
							this.InternalList.RemoveAt(num3);
						}
					}
					else if (CollectionView.NewItemPlaceholder != obj2)
					{
						this.InternalList.Insert(num4, obj2);
					}
				}
				if (!this.IsGrouping)
				{
					this.AdjustCurrencyForMove(adjustedOldIndex, adjustedNewIndex);
					if (flag2)
					{
						args = new NotifyCollectionChangedEventArgs(notifyCollectionChangedAction, args.OldItems[0], adjustedNewIndex, adjustedOldIndex);
					}
					else
					{
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, args.NewItems, adjustedNewIndex);
						args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, args.OldItems, adjustedOldIndex);
					}
				}
				else
				{
					LiveShapingItem lsi = (liveShapingList == null) ? null : liveShapingList.ItemAt(adjustedNewIndex);
					if (flag2)
					{
						this.MoveItemWithinGroups(obj, lsi, adjustedOldIndex, adjustedNewIndex);
					}
					else
					{
						this.RemoveItemFromGroups(obj);
						this.AddItemToGroups(obj2, lsi);
					}
				}
				break;
			}
			default:
				Invariant.Assert(false, SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					notifyCollectionChangedAction
				}));
				break;
			}
			bool flag3 = this.IsCurrentAfterLast != isCurrentAfterLast;
			bool flag4 = this.IsCurrentBeforeFirst != isCurrentBeforeFirst;
			bool flag5 = this.CurrentPosition != currentPosition2;
			bool flag6 = this.CurrentItem != currentItem;
			isCurrentAfterLast = this.IsCurrentAfterLast;
			isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			currentPosition2 = this.CurrentPosition;
			currentItem = this.CurrentItem;
			if (!this.IsGrouping)
			{
				this.OnCollectionChanged(args);
				if (notifyCollectionChangedEventArgs != null)
				{
					this.OnCollectionChanged(notifyCollectionChangedEventArgs);
				}
				if (this.IsCurrentAfterLast != isCurrentAfterLast)
				{
					flag3 = false;
					isCurrentAfterLast = this.IsCurrentAfterLast;
				}
				if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
				{
					flag4 = false;
					isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
				}
				if (this.CurrentPosition != currentPosition2)
				{
					flag5 = false;
					currentPosition2 = this.CurrentPosition;
				}
				if (this.CurrentItem != currentItem)
				{
					flag6 = false;
					currentItem = this.CurrentItem;
				}
			}
			if (this._currentElementWasRemoved)
			{
				this.MoveCurrencyOffDeletedElement(currentPosition);
				flag3 = (flag3 || this.IsCurrentAfterLast != isCurrentAfterLast);
				flag4 = (flag4 || this.IsCurrentBeforeFirst != isCurrentBeforeFirst);
				flag5 = (flag5 || this.CurrentPosition != currentPosition2);
				flag6 = (flag6 || this.CurrentItem != currentItem);
			}
			if (flag3)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (flag4)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (flag5)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (flag6)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		/// <summary>Returns the index of the specified item in the <see cref="P:System.Windows.Data.ListCollectionView.InternalList" />.</summary>
		/// <param name="item">The item to return an index for.</param>
		/// <returns>The index of the specified item in the <see cref="P:System.Windows.Data.ListCollectionView.InternalList" />.</returns>
		// Token: 0x06001BB3 RID: 7091 RVA: 0x0008266C File Offset: 0x0008086C
		protected int InternalIndexOf(object item)
		{
			if (this.IsGrouping)
			{
				return this._group.LeafIndexOf(item);
			}
			if (item == CollectionView.NewItemPlaceholder)
			{
				switch (this.NewItemPlaceholderPosition)
				{
				case NewItemPlaceholderPosition.None:
					return -1;
				case NewItemPlaceholderPosition.AtBeginning:
					return 0;
				case NewItemPlaceholderPosition.AtEnd:
					return this.InternalCount - 1;
				}
			}
			else if (this.IsAddingNew && ItemsControl.EqualsEx(item, this._newItem))
			{
				switch (this.NewItemPlaceholderPosition)
				{
				case NewItemPlaceholderPosition.None:
					if (this.UsesLocalArray)
					{
						return this.InternalCount - 1;
					}
					break;
				case NewItemPlaceholderPosition.AtBeginning:
					return 1;
				case NewItemPlaceholderPosition.AtEnd:
					return this.InternalCount - 2;
				}
			}
			int num = this.InternalList.IndexOf(item);
			if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning && num >= 0)
			{
				num += (this.IsAddingNew ? 2 : 1);
			}
			return num;
		}

		/// <summary>Returns the item at the given index in the <see cref="P:System.Windows.Data.ListCollectionView.InternalList" />.</summary>
		/// <param name="index">The index at which the item is located.</param>
		/// <returns>The item at the specified zero-based index in the view.</returns>
		// Token: 0x06001BB4 RID: 7092 RVA: 0x00082738 File Offset: 0x00080938
		protected object InternalItemAt(int index)
		{
			if (this.IsGrouping)
			{
				return this._group.LeafAt(index);
			}
			switch (this.NewItemPlaceholderPosition)
			{
			case NewItemPlaceholderPosition.None:
				if (this.IsAddingNew && this.UsesLocalArray && index == this.InternalCount - 1)
				{
					return this._newItem;
				}
				break;
			case NewItemPlaceholderPosition.AtBeginning:
				if (index == 0)
				{
					return CollectionView.NewItemPlaceholder;
				}
				index--;
				if (this.IsAddingNew)
				{
					if (index == 0)
					{
						return this._newItem;
					}
					if (this.UsesLocalArray || index <= this._newItemIndex)
					{
						index--;
					}
				}
				break;
			case NewItemPlaceholderPosition.AtEnd:
				if (index == this.InternalCount - 1)
				{
					return CollectionView.NewItemPlaceholder;
				}
				if (this.IsAddingNew)
				{
					if (index == this.InternalCount - 2)
					{
						return this._newItem;
					}
					if (!this.UsesLocalArray && index >= this._newItemIndex)
					{
						index++;
					}
				}
				break;
			}
			return this.InternalList[index];
		}

		/// <summary>Return a value that indicates whether the <see cref="P:System.Windows.Data.ListCollectionView.InternalList" /> contains the item.</summary>
		/// <param name="item">The item to locate.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Data.ListCollectionView.InternalList" /> contains the item; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001BB5 RID: 7093 RVA: 0x00082828 File Offset: 0x00080A28
		protected bool InternalContains(object item)
		{
			if (item == CollectionView.NewItemPlaceholder)
			{
				return this.NewItemPlaceholderPosition > NewItemPlaceholderPosition.None;
			}
			if (this.IsGrouping)
			{
				return this._group.LeafIndexOf(item) >= 0;
			}
			return this.InternalList.Contains(item);
		}

		/// <summary>Returns an enumerator for the <see cref="P:System.Windows.Data.ListCollectionView.InternalList" />. </summary>
		/// <returns>An enumerator for the <see cref="P:System.Windows.Data.ListCollectionView.InternalList" />.</returns>
		// Token: 0x06001BB6 RID: 7094 RVA: 0x00082863 File Offset: 0x00080A63
		protected IEnumerator InternalGetEnumerator()
		{
			if (!this.IsGrouping)
			{
				return new CollectionView.PlaceholderAwareEnumerator(this, this.InternalList.GetEnumerator(), this.NewItemPlaceholderPosition, this._newItem);
			}
			return this._group.GetLeafEnumerator();
		}

		/// <summary>Gets a value that indicates whether a private copy of the data is needed for sorting and filtering.</summary>
		/// <returns>
		///     <see langword="true" /> if a private copy of the data is needed for sorting and filtering; otherwise, <see langword="false" />. The default implementation returns <see langword="true" /> if there is an <see cref="P:System.Windows.Data.ListCollectionView.ActiveFilter" /> or <see cref="P:System.Windows.Data.ListCollectionView.ActiveComparer" />, or both.</returns>
		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x00082898 File Offset: 0x00080A98
		protected bool UsesLocalArray
		{
			get
			{
				return this.ActiveComparer != null || this.ActiveFilter != null || (this.IsGrouping && this.IsLiveGrouping == true);
			}
		}

		/// <summary>Gets the complete and unfiltered underlying collection.</summary>
		/// <returns>The underlying collection, which must implement <see cref="T:System.Collections.IList" />.</returns>
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x000828DD File Offset: 0x00080ADD
		protected IList InternalList
		{
			get
			{
				return this._internalList;
			}
		}

		/// <summary>Gets or sets the current active comparer that is used in sorting.</summary>
		/// <returns>An <see cref="T:System.Collections.IComparer" /> object that is the active comparer.</returns>
		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x000828E5 File Offset: 0x00080AE5
		// (set) Token: 0x06001BBA RID: 7098 RVA: 0x000828ED File Offset: 0x00080AED
		protected IComparer ActiveComparer
		{
			get
			{
				return this._activeComparer;
			}
			set
			{
				this._activeComparer = value;
			}
		}

		/// <summary>Gets or sets the current active <see cref="P:System.Windows.Data.CollectionView.Filter" /> callback.</summary>
		/// <returns>The active <see cref="P:System.Windows.Data.CollectionView.Filter" /> callback.</returns>
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001BBB RID: 7099 RVA: 0x000828F6 File Offset: 0x00080AF6
		// (set) Token: 0x06001BBC RID: 7100 RVA: 0x000828FE File Offset: 0x00080AFE
		protected Predicate<object> ActiveFilter
		{
			get
			{
				return this._activeFilter;
			}
			set
			{
				this._activeFilter = value;
			}
		}

		/// <summary>Gets a value that indicates whether there are groups in the view.</summary>
		/// <returns>
		///     <see langword="true" /> if there are groups in the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001BBD RID: 7101 RVA: 0x00082907 File Offset: 0x00080B07
		protected bool IsGrouping
		{
			get
			{
				return this._isGrouping;
			}
		}

		/// <summary>Gets the number of records in the <see cref="P:System.Windows.Data.ListCollectionView.InternalList" />.</summary>
		/// <returns>The number of records in the <see cref="P:System.Windows.Data.ListCollectionView.InternalList" />.</returns>
		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x00082910 File Offset: 0x00080B10
		protected int InternalCount
		{
			get
			{
				if (this.IsGrouping)
				{
					return this._group.ItemCount;
				}
				int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.None) ? 0 : 1;
				if (this.UsesLocalArray && this.IsAddingNew)
				{
					num++;
				}
				return num + this.InternalList.Count;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x0008295F File Offset: 0x00080B5F
		// (set) Token: 0x06001BC0 RID: 7104 RVA: 0x00082967 File Offset: 0x00080B67
		internal ArrayList ShadowCollection
		{
			get
			{
				return this._shadowCollection;
			}
			set
			{
				this._shadowCollection = value;
			}
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x00082970 File Offset: 0x00080B70
		internal void AdjustShadowCopy(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (e.NewStartingIndex > -1)
				{
					this.ShadowCollection.Insert(e.NewStartingIndex, e.NewItems[0]);
					return;
				}
				this.ShadowCollection.Add(e.NewItems[0]);
				return;
			case NotifyCollectionChangedAction.Remove:
				if (e.OldStartingIndex > -1)
				{
					this.ShadowCollection.RemoveAt(e.OldStartingIndex);
					return;
				}
				this.ShadowCollection.Remove(e.OldItems[0]);
				return;
			case NotifyCollectionChangedAction.Replace:
			{
				if (e.OldStartingIndex > -1)
				{
					this.ShadowCollection[e.OldStartingIndex] = e.NewItems[0];
					return;
				}
				int num = this.ShadowCollection.IndexOf(e.OldItems[0]);
				this.ShadowCollection[num] = e.NewItems[0];
				return;
			}
			case NotifyCollectionChangedAction.Move:
			{
				int num = e.OldStartingIndex;
				if (num < 0)
				{
					num = this.ShadowCollection.IndexOf(e.NewItems[0]);
				}
				this.ShadowCollection.RemoveAt(num);
				this.ShadowCollection.Insert(e.NewStartingIndex, e.NewItems[0]);
				return;
			}
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x00082ADA File Offset: 0x00080CDA
		internal bool HasSortDescriptions
		{
			get
			{
				return this._sort != null && this._sort.Count > 0;
			}
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x00082AF4 File Offset: 0x00080CF4
		internal static IComparer PrepareComparer(IComparer customSort, SortDescriptionCollection sort, Func<CollectionView> lazyGetCollectionView)
		{
			if (customSort != null)
			{
				return customSort;
			}
			if (sort != null && sort.Count > 0)
			{
				CollectionView collectionView = lazyGetCollectionView();
				if (collectionView.SourceCollection != null)
				{
					IComparer comparer = SystemXmlHelper.PrepareXmlComparer(collectionView.SourceCollection, sort, collectionView.Culture);
					if (comparer != null)
					{
						return comparer;
					}
				}
				return new SortFieldComparer(sort, collectionView.Culture);
			}
			return null;
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x00082B47 File Offset: 0x00080D47
		private bool IsCurrentInView
		{
			get
			{
				return 0 <= this.CurrentPosition && this.CurrentPosition < this.InternalCount;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x00016748 File Offset: 0x00014948
		private bool CanGroupNamesChange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x00082B62 File Offset: 0x00080D62
		private IList SourceList
		{
			get
			{
				return this.SourceCollection as IList;
			}
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x00082B70 File Offset: 0x00080D70
		private void ValidateCollectionChangedEventArgs(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (e.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (e.NewItems.Count != 1 || e.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				if (e.NewStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("CannotMoveToUnknownPosition"));
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x00082C6C File Offset: 0x00080E6C
		private void PrepareLocalArray()
		{
			this.PrepareShaping();
			LiveShapingList liveShapingList = this._internalList as LiveShapingList;
			if (liveShapingList != null)
			{
				liveShapingList.LiveShapingDirty -= this.OnLiveShapingDirty;
				liveShapingList.Clear();
			}
			IList list;
			if (!base.AllowsCrossThreadChanges)
			{
				list = (this.SourceCollection as IList);
			}
			else
			{
				IList list2 = this.ShadowCollection;
				list = list2;
			}
			IList list3 = list;
			if (!this.UsesLocalArray)
			{
				this._internalList = list3;
			}
			else
			{
				int count = list3.Count;
				IList list4;
				if (!this.IsLiveShaping)
				{
					IList list2 = new ArrayList(count);
					list4 = list2;
				}
				else
				{
					IList list2 = new LiveShapingList(this, this.GetLiveShapingFlags(), this.ActiveComparer);
					list4 = list2;
				}
				IList list5 = list4;
				liveShapingList = (list5 as LiveShapingList);
				for (int i = 0; i < count; i++)
				{
					if (!this.IsAddingNew || i != this._newItemIndex)
					{
						object obj = list3[i];
						if (this.ActiveFilter == null || this.ActiveFilter(obj))
						{
							list5.Add(obj);
						}
						else if (this.IsLiveFiltering == true)
						{
							liveShapingList.AddFilteredItem(obj);
						}
					}
				}
				if (this.ActiveComparer != null)
				{
					list5.Sort(this.ActiveComparer);
				}
				if (liveShapingList != null)
				{
					liveShapingList.LiveShapingDirty += this.OnLiveShapingDirty;
				}
				this._internalList = list5;
			}
			this.PrepareGroups();
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x00082DC0 File Offset: 0x00080FC0
		private void OnLiveShapingDirty(object sender, EventArgs e)
		{
			this.IsLiveShapingDirty = true;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x00082DC9 File Offset: 0x00080FC9
		private void RebuildLocalArray()
		{
			if (base.IsRefreshDeferred)
			{
				base.RefreshOrDefer();
				return;
			}
			this.PrepareLocalArray();
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x00082DE0 File Offset: 0x00080FE0
		private void MoveCurrencyOffDeletedElement(int oldCurrentPosition)
		{
			int num = this.InternalCount - 1;
			int num2 = (oldCurrentPosition < num) ? oldCurrentPosition : num;
			this._currentElementWasRemoved = false;
			base.OnCurrentChanging();
			if (num2 < 0)
			{
				base.SetCurrent(null, num2);
			}
			else
			{
				base.SetCurrent(this.InternalItemAt(num2), num2);
			}
			this.OnCurrentChanged();
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x00082E30 File Offset: 0x00081030
		private int AdjustBefore(NotifyCollectionChangedAction action, object item, int index)
		{
			if (action == NotifyCollectionChangedAction.Reset)
			{
				return -1;
			}
			if (item == CollectionView.NewItemPlaceholder)
			{
				if (this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)
				{
					return this.InternalCount - 1;
				}
				return 0;
			}
			else if (this.IsAddingNew && this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.None && ItemsControl.EqualsEx(item, this._newItem))
			{
				if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
				{
					return 1;
				}
				if (!this.UsesLocalArray)
				{
					return index;
				}
				return this.InternalCount - 2;
			}
			else
			{
				int num = this.IsGrouping ? 0 : ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? (this.IsAddingNew ? 2 : 1) : 0);
				IEnumerable enumerable;
				if (!base.AllowsCrossThreadChanges)
				{
					enumerable = this.SourceCollection;
				}
				else
				{
					IEnumerable shadowCollection = this.ShadowCollection;
					enumerable = shadowCollection;
				}
				IList list = enumerable as IList;
				if (index < -1 || index > list.Count)
				{
					throw new InvalidOperationException(SR.Get("CollectionChangeIndexOutOfRange", new object[]
					{
						index,
						list.Count
					}));
				}
				if (action == NotifyCollectionChangedAction.Add)
				{
					if (index >= 0)
					{
						if (!ItemsControl.EqualsEx(item, list[index]))
						{
							throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
							{
								index
							}));
						}
					}
					else
					{
						index = list.IndexOf(item);
						if (index < 0)
						{
							throw new InvalidOperationException(SR.Get("AddedItemNotInCollection"));
						}
					}
				}
				if (!this.UsesLocalArray)
				{
					if (this.IsAddingNew && this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.None && index > this._newItemIndex)
					{
						index--;
					}
					if (index >= 0)
					{
						return index + num;
					}
					return index;
				}
				else
				{
					if (action == NotifyCollectionChangedAction.Add)
					{
						if (!this.PassesFilter(item))
						{
							return -2;
						}
						if (!this.UsesLocalArray)
						{
							index = -1;
						}
						else if (this.ActiveComparer != null)
						{
							index = this.InternalList.Search(item, this.ActiveComparer);
							if (index < 0)
							{
								index = ~index;
							}
						}
						else
						{
							index = this.MatchingSearch(item, index, list, this.InternalList);
						}
					}
					else if (action == NotifyCollectionChangedAction.Remove)
					{
						if (!this.IsAddingNew || item != this._newItem)
						{
							index = this.InternalList.IndexOf(item);
							if (index < 0)
							{
								return -2;
							}
						}
						else
						{
							switch (this.NewItemPlaceholderPosition)
							{
							case NewItemPlaceholderPosition.None:
								return this.InternalCount - 1;
							case NewItemPlaceholderPosition.AtBeginning:
								return 1;
							case NewItemPlaceholderPosition.AtEnd:
								return this.InternalCount - 2;
							}
						}
					}
					else
					{
						index = -1;
					}
					if (index >= 0)
					{
						return index + num;
					}
					return index;
				}
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x00083058 File Offset: 0x00081258
		private int MatchingSearch(object item, int index, IList ilFull, IList ilPartial)
		{
			int num = 0;
			int num2 = 0;
			while (num < index && num2 < this.InternalList.Count)
			{
				if (ItemsControl.EqualsEx(ilFull[num], ilPartial[num2]))
				{
					num++;
					num2++;
				}
				else if (ItemsControl.EqualsEx(item, ilPartial[num2]))
				{
					num2++;
				}
				else
				{
					num++;
				}
			}
			return num2;
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000830B8 File Offset: 0x000812B8
		private void AdjustCurrencyForAdd(int index)
		{
			if (this.InternalCount == 1)
			{
				base.SetCurrent(null, -1);
				return;
			}
			if (index <= this.CurrentPosition)
			{
				int num = this.CurrentPosition + 1;
				if (num < this.InternalCount)
				{
					base.SetCurrent(this.GetItemAt(num), num);
					return;
				}
				base.SetCurrent(null, this.InternalCount);
			}
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0008310E File Offset: 0x0008130E
		private void AdjustCurrencyForRemove(int index)
		{
			if (index < this.CurrentPosition)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
				return;
			}
			if (index == this.CurrentPosition)
			{
				this._currentElementWasRemoved = true;
			}
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x00083140 File Offset: 0x00081340
		private void AdjustCurrencyForMove(int oldIndex, int newIndex)
		{
			if (oldIndex == this.CurrentPosition)
			{
				base.SetCurrent(this.GetItemAt(newIndex), newIndex);
				return;
			}
			if (oldIndex < this.CurrentPosition && this.CurrentPosition <= newIndex)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
				return;
			}
			if (newIndex <= this.CurrentPosition && this.CurrentPosition < oldIndex)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition + 1);
			}
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x000831B2 File Offset: 0x000813B2
		private void AdjustCurrencyForReplace(int index)
		{
			if (index == this.CurrentPosition)
			{
				this._currentElementWasRemoved = true;
			}
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000831C4 File Offset: 0x000813C4
		private void PrepareShaping()
		{
			this.ActiveComparer = ListCollectionView.PrepareComparer(this._customSort, this._sort, () => this);
			this.ActiveFilter = this.Filter;
			this._group.Clear();
			this._group.Initialize();
			this._isGrouping = (this._group.GroupBy != null);
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0008322C File Offset: 0x0008142C
		private void SetSortDescriptions(SortDescriptionCollection descriptions)
		{
			if (this._sort != null)
			{
				((INotifyCollectionChanged)this._sort).CollectionChanged -= this.SortDescriptionsChanged;
			}
			this._sort = descriptions;
			if (this._sort != null)
			{
				Invariant.Assert(this._sort.Count == 0, "must be empty SortDescription collection");
				((INotifyCollectionChanged)this._sort).CollectionChanged += this.SortDescriptionsChanged;
			}
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x00083298 File Offset: 0x00081498
		private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.IsAddingNew || this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Sorting"
				}));
			}
			if (this._sort.Count > 0)
			{
				this._customSort = null;
			}
			base.RefreshOrDefer();
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x000832F0 File Offset: 0x000814F0
		private void PrepareGroups()
		{
			if (!this._isGrouping)
			{
				return;
			}
			IComparer activeComparer = this.ActiveComparer;
			if (activeComparer != null)
			{
				this._group.ActiveComparer = activeComparer;
			}
			else
			{
				CollectionViewGroupInternal.IListComparer listComparer = this._group.ActiveComparer as CollectionViewGroupInternal.IListComparer;
				if (listComparer != null)
				{
					listComparer.ResetList(this.InternalList);
				}
				else
				{
					this._group.ActiveComparer = new CollectionViewGroupInternal.IListComparer(this.InternalList);
				}
			}
			if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				this._group.InsertSpecialItem(0, CollectionView.NewItemPlaceholder, true);
				if (this.IsAddingNew)
				{
					this._group.InsertSpecialItem(1, this._newItem, true);
				}
			}
			bool flag = this.IsLiveGrouping == true;
			LiveShapingList liveShapingList = this.InternalList as LiveShapingList;
			int i = 0;
			int count = this.InternalList.Count;
			while (i < count)
			{
				object obj = this.InternalList[i];
				LiveShapingItem lsi = (liveShapingList != null) ? liveShapingList.ItemAt(i) : null;
				if (!this.IsAddingNew || !ItemsControl.EqualsEx(this._newItem, obj))
				{
					this._group.AddToSubgroups(obj, lsi, true);
				}
				i++;
			}
			if (this.IsAddingNew && this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)
			{
				this._group.InsertSpecialItem(this._group.Items.Count, this._newItem, true);
			}
			if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				this._group.InsertSpecialItem(this._group.Items.Count, CollectionView.NewItemPlaceholder, true);
			}
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x0008347A File Offset: 0x0008167A
		private void OnGroupChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				this.AdjustCurrencyForAdd(e.NewStartingIndex);
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				this.AdjustCurrencyForRemove(e.OldStartingIndex);
			}
			this.OnCollectionChanged(e);
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x000834AE File Offset: 0x000816AE
		private void OnGroupByChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.IsAddingNew || this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Grouping"
				}));
			}
			base.RefreshOrDefer();
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x000834AE File Offset: 0x000816AE
		private void OnGroupDescriptionChanged(object sender, EventArgs e)
		{
			if (this.IsAddingNew || this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Grouping"
				}));
			}
			base.RefreshOrDefer();
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000834E4 File Offset: 0x000816E4
		private void AddItemToGroups(object item, LiveShapingItem lsi)
		{
			if (this.IsAddingNew && item == this._newItem)
			{
				int index;
				switch (this.NewItemPlaceholderPosition)
				{
				default:
					index = this._group.Items.Count;
					break;
				case NewItemPlaceholderPosition.AtBeginning:
					index = 1;
					break;
				case NewItemPlaceholderPosition.AtEnd:
					index = this._group.Items.Count - 1;
					break;
				}
				this._group.InsertSpecialItem(index, item, false);
				return;
			}
			this._group.AddToSubgroups(item, lsi, false);
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x00083562 File Offset: 0x00081762
		private void RemoveItemFromGroups(object item)
		{
			if (this.CanGroupNamesChange || this._group.RemoveFromSubgroups(item))
			{
				this._group.RemoveItemFromSubgroupsByExhaustiveSearch(item);
			}
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x00083586 File Offset: 0x00081786
		private void MoveItemWithinGroups(object item, LiveShapingItem lsi, int oldIndex, int newIndex)
		{
			this._group.MoveWithinSubgroups(item, lsi, this.InternalList, oldIndex, newIndex);
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000835A0 File Offset: 0x000817A0
		private LiveShapingFlags GetLiveShapingFlags()
		{
			LiveShapingFlags liveShapingFlags = (LiveShapingFlags)0;
			if (this.IsLiveSorting == true)
			{
				liveShapingFlags |= LiveShapingFlags.Sorting;
			}
			if (this.IsLiveFiltering == true)
			{
				liveShapingFlags |= LiveShapingFlags.Filtering;
			}
			if (this.IsLiveGrouping == true)
			{
				liveShapingFlags |= LiveShapingFlags.Grouping;
			}
			return liveShapingFlags;
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x0008361C File Offset: 0x0008181C
		internal void RestoreLiveShaping()
		{
			LiveShapingList liveShapingList = this.InternalList as LiveShapingList;
			if (liveShapingList == null)
			{
				return;
			}
			if (this.ActiveComparer != null)
			{
				double num = (double)liveShapingList.SortDirtyItems.Count / (double)(liveShapingList.Count + 1);
				if (num < 0.8)
				{
					using (List<LiveShapingItem>.Enumerator enumerator = liveShapingList.SortDirtyItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							LiveShapingItem liveShapingItem = enumerator.Current;
							if (liveShapingItem.IsSortDirty && !liveShapingItem.IsDeleted && liveShapingItem.ForwardChanges)
							{
								liveShapingItem.IsSortDirty = false;
								liveShapingItem.IsSortPendingClean = false;
								int num2;
								int num3;
								liveShapingList.FindPosition(liveShapingItem, out num2, out num3);
								if (num2 != num3)
								{
									if (num2 < num3)
									{
										num3--;
									}
									this.ProcessLiveShapingCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, liveShapingItem.Item, num2, num3), num2, num3);
								}
							}
						}
						goto IL_DE;
					}
				}
				liveShapingList.RestoreLiveSortingByInsertionSort(new Action<NotifyCollectionChangedEventArgs, int, int>(this.ProcessLiveShapingCollectionChange));
			}
			IL_DE:
			liveShapingList.SortDirtyItems.Clear();
			if (this.ActiveFilter != null)
			{
				foreach (LiveShapingItem liveShapingItem2 in liveShapingList.FilterDirtyItems)
				{
					if (liveShapingItem2.IsFilterDirty && liveShapingItem2.ForwardChanges)
					{
						object item = liveShapingItem2.Item;
						bool failsFilter = liveShapingItem2.FailsFilter;
						bool flag = !this.PassesFilter(item);
						if (failsFilter != flag)
						{
							if (flag)
							{
								int num4 = liveShapingList.IndexOf(liveShapingItem2);
								this.ProcessLiveShapingCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, num4), num4, -1);
								liveShapingList.AddFilteredItem(liveShapingItem2);
							}
							else
							{
								liveShapingList.RemoveFilteredItem(liveShapingItem2);
								int num4;
								if (this.ActiveComparer != null)
								{
									num4 = liveShapingList.Search(0, liveShapingList.Count, item);
									if (num4 < 0)
									{
										num4 = ~num4;
									}
								}
								else
								{
									IEnumerable enumerable;
									if (!base.AllowsCrossThreadChanges)
									{
										enumerable = this.SourceCollection;
									}
									else
									{
										IEnumerable shadowCollection = this.ShadowCollection;
										enumerable = shadowCollection;
									}
									IList list = enumerable as IList;
									num4 = liveShapingItem2.GetAndClearStartingIndex();
									while (num4 < list.Count && !ItemsControl.EqualsEx(item, list[num4]))
									{
										num4++;
									}
									liveShapingList.SetStartingIndexForFilteredItem(item, num4 + 1);
									num4 = this.MatchingSearch(item, num4, list, liveShapingList);
								}
								this.ProcessLiveShapingCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, num4), -1, num4);
							}
						}
						liveShapingItem2.IsFilterDirty = false;
					}
				}
			}
			liveShapingList.FilterDirtyItems.Clear();
			if (this.IsGrouping)
			{
				List<AbandonedGroupItem> deleteList = new List<AbandonedGroupItem>();
				foreach (LiveShapingItem liveShapingItem3 in liveShapingList.GroupDirtyItems)
				{
					if (liveShapingItem3.IsGroupDirty && !liveShapingItem3.IsDeleted && liveShapingItem3.ForwardChanges)
					{
						this._group.RestoreGrouping(liveShapingItem3, deleteList);
						liveShapingItem3.IsGroupDirty = false;
					}
				}
				this._group.DeleteAbandonedGroupItems(deleteList);
			}
			liveShapingList.GroupDirtyItems.Clear();
			this.IsLiveShapingDirty = false;
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x0008395C File Offset: 0x00081B5C
		private void ProcessLiveShapingCollectionChange(NotifyCollectionChangedEventArgs args, int oldIndex, int newIndex)
		{
			if (!this.IsGrouping && this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				if (oldIndex >= 0)
				{
					oldIndex++;
				}
				if (newIndex >= 0)
				{
					newIndex++;
				}
			}
			this.ProcessCollectionChangedWithAdjustedIndex(args, oldIndex, newIndex);
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x0008398A File Offset: 0x00081B8A
		// (set) Token: 0x06001BE0 RID: 7136 RVA: 0x00083992 File Offset: 0x00081B92
		internal bool IsLiveShapingDirty
		{
			get
			{
				return this._isLiveShapingDirty;
			}
			set
			{
				if (value == this._isLiveShapingDirty)
				{
					return;
				}
				this._isLiveShapingDirty = value;
				if (value)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(this.RestoreLiveShaping));
				}
			}
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000839C4 File Offset: 0x00081BC4
		private object ItemFrom(object o)
		{
			LiveShapingItem liveShapingItem = o as LiveShapingItem;
			if (liveShapingItem != null)
			{
				return liveShapingItem.Item;
			}
			return o;
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0007D0A8 File Offset: 0x0007B2A8
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000839E3 File Offset: 0x00081BE3
		private void DeferAction(Action action)
		{
			if (this._deferredActions == null)
			{
				this._deferredActions = new List<Action>();
			}
			this._deferredActions.Add(action);
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x00083A04 File Offset: 0x00081C04
		private void DoDeferredActions()
		{
			if (this._deferredActions != null)
			{
				List<Action> deferredActions = this._deferredActions;
				this._deferredActions = null;
				foreach (Action action in deferredActions)
				{
					action();
				}
			}
		}

		// Token: 0x040013A5 RID: 5029
		private const double LiveSortingDensityThreshold = 0.8;

		// Token: 0x040013A6 RID: 5030
		private IList _internalList;

		// Token: 0x040013A7 RID: 5031
		private CollectionViewGroupRoot _group;

		// Token: 0x040013A8 RID: 5032
		private bool _isGrouping;

		// Token: 0x040013A9 RID: 5033
		private IComparer _activeComparer;

		// Token: 0x040013AA RID: 5034
		private Predicate<object> _activeFilter;

		// Token: 0x040013AB RID: 5035
		private SortDescriptionCollection _sort;

		// Token: 0x040013AC RID: 5036
		private IComparer _customSort;

		// Token: 0x040013AD RID: 5037
		private ArrayList _shadowCollection;

		// Token: 0x040013AE RID: 5038
		private bool _currentElementWasRemoved;

		// Token: 0x040013AF RID: 5039
		private object _newItem = CollectionView.NoNewItem;

		// Token: 0x040013B0 RID: 5040
		private object _editItem;

		// Token: 0x040013B1 RID: 5041
		private int _newItemIndex;

		// Token: 0x040013B2 RID: 5042
		private NewItemPlaceholderPosition _newItemPlaceholderPosition;

		// Token: 0x040013B3 RID: 5043
		private bool _isItemConstructorValid;

		// Token: 0x040013B4 RID: 5044
		private ConstructorInfo _itemConstructor;

		// Token: 0x040013B5 RID: 5045
		private List<Action> _deferredActions;

		// Token: 0x040013B6 RID: 5046
		private ObservableCollection<string> _liveSortingProperties;

		// Token: 0x040013B7 RID: 5047
		private ObservableCollection<string> _liveFilteringProperties;

		// Token: 0x040013B8 RID: 5048
		private ObservableCollection<string> _liveGroupingProperties;

		// Token: 0x040013B9 RID: 5049
		private bool? _isLiveSorting = new bool?(false);

		// Token: 0x040013BA RID: 5050
		private bool? _isLiveFiltering = new bool?(false);

		// Token: 0x040013BB RID: 5051
		private bool? _isLiveGrouping = new bool?(false);

		// Token: 0x040013BC RID: 5052
		private bool _isLiveShapingDirty;

		// Token: 0x040013BD RID: 5053
		private bool _isRemoving;

		// Token: 0x040013BE RID: 5054
		private const int _unknownIndex = -1;
	}
}
