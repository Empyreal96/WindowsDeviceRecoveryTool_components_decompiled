using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Represents the <see cref="T:System.Windows.Data.CollectionView" /> for collections that implement <see cref="T:System.ComponentModel.IBindingList" />, such as Microsoft ActiveX Data Objects (ADO) data views.</summary>
	// Token: 0x020001A2 RID: 418
	public sealed class BindingListCollectionView : CollectionView, IComparer, IEditableCollectionView, ICollectionViewLiveShaping, IItemProperties
	{
		/// <summary>Initializes an instance of <see cref="T:System.Windows.Data.BindingListCollectionView" /> over the given list.</summary>
		/// <param name="list">The underlying <see cref="T:System.ComponentModel.IBindingList" />.</param>
		// Token: 0x060019B7 RID: 6583 RVA: 0x0007AC24 File Offset: 0x00078E24
		public BindingListCollectionView(IBindingList list) : base(list)
		{
			this.InternalList = list;
			this._blv = (list as IBindingListView);
			this._isDataView = SystemDataHelper.IsDataView(list);
			this.SubscribeToChanges();
			this._group = new CollectionViewGroupRoot(this);
			this._group.GroupDescriptionChanged += this.OnGroupDescriptionChanged;
			((INotifyCollectionChanged)this._group).CollectionChanged += this.OnGroupChanged;
			((INotifyCollectionChanged)this._group.GroupDescriptions).CollectionChanged += this.OnGroupByChanged;
		}

		/// <summary>Returns a value that indicates whether the specified item in the underlying collection belongs to the view.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		///     <see langword="true" /> if the specified item belongs to the view or if there is not filter set on the collection view; otherwise, <see langword="false" />.</returns>
		// Token: 0x060019B8 RID: 6584 RVA: 0x0007ACCA File Offset: 0x00078ECA
		public override bool PassesFilter(object item)
		{
			return !this.IsCustomFilterSet || this.Contains(item);
		}

		/// <summary>Returns a value that indicates whether a given item belongs to the collection view.</summary>
		/// <param name="item">The object to check.</param>
		/// <returns>
		///     <see langword="true" /> if the item belongs to the collection view; otherwise, <see langword="false" />.</returns>
		// Token: 0x060019B9 RID: 6585 RVA: 0x0007ACDD File Offset: 0x00078EDD
		public override bool Contains(object item)
		{
			base.VerifyRefreshNotDeferred();
			if (item != CollectionView.NewItemPlaceholder)
			{
				return this.CollectionProxy.Contains(item);
			}
			return this.NewItemPlaceholderPosition > NewItemPlaceholderPosition.None;
		}

		/// <summary>Sets the item at the specified index to be the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> in the view.</summary>
		/// <param name="position">The index to set the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> to.</param>
		/// <returns>
		///     <see langword="true" /> if the resulting <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Thrown if the index is out of range. </exception>
		// Token: 0x060019BA RID: 6586 RVA: 0x0007AD03 File Offset: 0x00078F03
		public override bool MoveCurrentToPosition(int position)
		{
			base.VerifyRefreshNotDeferred();
			if (position < -1 || position > this.InternalCount)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this._MoveTo(position);
			return this.IsCurrentInView;
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="o1">  First object to compare.</param>
		/// <param name="o2">  Second object to compare.</param>
		/// <returns>Less than zero means <paramref name="o1" /> is less than <paramref name="o2" />, a value of zero means they are equal, and over zero means <paramref name="o1" /> is greater than <paramref name="o2" />.</returns>
		// Token: 0x060019BB RID: 6587 RVA: 0x0007AD30 File Offset: 0x00078F30
		int IComparer.Compare(object o1, object o2)
		{
			int num = this.InternalIndexOf(o1);
			int num2 = this.InternalIndexOf(o2);
			return num - num2;
		}

		/// <summary>Returns the index at which the given item belongs in the collection view.</summary>
		/// <param name="item">The object to look for in the collection.</param>
		/// <returns>The index of the item in the collection, or -1 if the item does not exist in the collection view.</returns>
		// Token: 0x060019BC RID: 6588 RVA: 0x0007AD50 File Offset: 0x00078F50
		public override int IndexOf(object item)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalIndexOf(item);
		}

		/// <summary>Retrieves the item at the specified position in the view.</summary>
		/// <param name="index">The zero-based index at which the item is located.</param>
		/// <returns>The item at the specified position in the view.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">If <paramref name="index" /> is out of range.</exception>
		// Token: 0x060019BD RID: 6589 RVA: 0x0007AD5F File Offset: 0x00078F5F
		public override object GetItemAt(int index)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalItemAt(index);
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0007AD6E File Offset: 0x00078F6E
		protected override IEnumerator GetEnumerator()
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalGetEnumerator();
		}

		/// <summary>Detaches the underlying collection from this collection view to enable the collection view to be garbage collected.</summary>
		// Token: 0x060019BF RID: 6591 RVA: 0x0007AD7C File Offset: 0x00078F7C
		public override void DetachFromSourceCollection()
		{
			if (this.InternalList != null && this.InternalList.SupportsChangeNotification)
			{
				this.InternalList.ListChanged -= this.OnListChanged;
			}
			this.InternalList = null;
			base.DetachFromSourceCollection();
		}

		/// <summary>Gets a collection of <see cref="T:System.ComponentModel.SortDescription" /> objects that describes how the items in the collection are sorted in the view.</summary>
		/// <returns>A collection of <see cref="T:System.ComponentModel.SortDescription" /> objects that describe how the items in the collection are sorted in the view.</returns>
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x060019C0 RID: 6592 RVA: 0x0007ADB8 File Offset: 0x00078FB8
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				if (this.InternalList.SupportsSorting)
				{
					if (this._sort == null)
					{
						bool allowMultipleDescriptions = this._blv != null && this._blv.SupportsAdvancedSorting;
						this._sort = new BindingListCollectionView.BindingListSortDescriptionCollection(allowMultipleDescriptions);
						((INotifyCollectionChanged)this._sort).CollectionChanged += this.SortDescriptionsChanged;
					}
					return this._sort;
				}
				return SortDescriptionCollection.Empty;
			}
		}

		/// <summary>Gets a value that indicates whether the collection supports sorting.</summary>
		/// <returns>For a default instance of <see cref="T:System.Windows.Data.BindingListCollectionView" /> this property always returns <see langword="true" />.</returns>
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x0007AE20 File Offset: 0x00079020
		public override bool CanSort
		{
			get
			{
				return this.InternalList.SupportsSorting;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x0007AE2D File Offset: 0x0007902D
		// (set) Token: 0x060019C3 RID: 6595 RVA: 0x0007AE35 File Offset: 0x00079035
		private IComparer ActiveComparer
		{
			get
			{
				return this._comparer;
			}
			set
			{
				this._comparer = value;
			}
		}

		/// <summary>Gets a value that indicates whether the view supports callback-based filtering.</summary>
		/// <returns>This property always returns <see langword="false" />.</returns>
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060019C4 RID: 6596 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanFilter
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets a custom filter.</summary>
		/// <returns>A string that specifies how the items are filtered.</returns>
		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x0007AE3E File Offset: 0x0007903E
		// (set) Token: 0x060019C6 RID: 6598 RVA: 0x0007AE48 File Offset: 0x00079048
		public string CustomFilter
		{
			get
			{
				return this._customFilter;
			}
			set
			{
				if (!this.CanCustomFilter)
				{
					throw new NotSupportedException(SR.Get("BindingListCannotCustomFilter"));
				}
				if (this.IsAddingNew || this.IsEditingItem)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
					{
						"CustomFilter"
					}));
				}
				if (base.AllowsCrossThreadChanges)
				{
					base.VerifyAccess();
				}
				this._customFilter = value;
				base.RefreshOrDefer();
			}
		}

		/// <summary>Gets a value that indicates whether the view supports custom filtering.</summary>
		/// <returns>
		///     <see langword="true" /> if the view supports custom filtering; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x0007AEB6 File Offset: 0x000790B6
		public bool CanCustomFilter
		{
			get
			{
				return this._blv != null && this._blv.SupportsFiltering;
			}
		}

		/// <summary>Gets a value that indicates whether the view supports grouping.</summary>
		/// <returns>For a default instance of <see cref="T:System.Windows.Data.BindingListCollectionView" /> this property always returns <see langword="true" />.</returns>
		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060019C8 RID: 6600 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanGroup
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.ComponentModel.GroupDescription" /> objects that describe how the items in the collection are grouped in the view.</summary>
		/// <returns>A collection of <see cref="T:System.ComponentModel.GroupDescription" /> objects that describe how the items in the collection are grouped in the view.</returns>
		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x0007AECD File Offset: 0x000790CD
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._group.GroupDescriptions;
			}
		}

		/// <summary>Gets the top-level groups.</summary>
		/// <returns>A read-only collection of the top-level groups, or <see langword="null" /> if there are no groups.</returns>
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060019CA RID: 6602 RVA: 0x0007AEDA File Offset: 0x000790DA
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				if (!this._isGrouping)
				{
					return null;
				}
				return this._group.Items;
			}
		}

		/// <summary>Gets or sets a delegate to select the <see cref="T:System.ComponentModel.GroupDescription" /> as a function of the parent group and its level. </summary>
		/// <returns>A method that provides the logic for the selection of the <see cref="T:System.ComponentModel.GroupDescription" /> as a function of the parent group and its level. The default is <see langword="null" />.</returns>
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x0007AEF1 File Offset: 0x000790F1
		// (set) Token: 0x060019CC RID: 6604 RVA: 0x0007AF00 File Offset: 0x00079100
		[DefaultValue(null)]
		public GroupDescriptionSelectorCallback GroupBySelector
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
						"GroupBySelector"
					}));
				}
				this._group.GroupBySelector = value;
				base.RefreshOrDefer();
			}
		}

		/// <summary>Gets the estimated number of records in the collection. </summary>
		/// <returns>One of the following:ValueMeaning-1Could not determine the count of the collection. This might be returned by a "virtualizing" view, where the view deliberately does not account for all items in the underlying collection because the view is attempting to increase efficiency and minimize dependence on always having the entire collection available.any other integerThe count of the collection.</returns>
		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x0007AF5B File Offset: 0x0007915B
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
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x0007AF69 File Offset: 0x00079169
		public override bool IsEmpty
		{
			get
			{
				return this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.None && this.CollectionProxy.Count == 0;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the list of items (after applying the sort and filters, if any) is already in the correct order for grouping.</summary>
		/// <returns>
		///     <see langword="true" /> if the list of items is already in the correct order for grouping; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x0007AF83 File Offset: 0x00079183
		// (set) Token: 0x060019D0 RID: 6608 RVA: 0x0007AF90 File Offset: 0x00079190
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

		/// <summary>Gets or sets the position of the new item placeholder in the <see cref="T:System.Windows.Data.BindingListCollectionView" />.</summary>
		/// <returns>One of the enumeration values that specifies the position of the new item placeholder in the <see cref="T:System.Windows.Data.BindingListCollectionView" />.</returns>
		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x0007AF9E File Offset: 0x0007919E
		// (set) Token: 0x060019D2 RID: 6610 RVA: 0x0007AFA8 File Offset: 0x000791A8
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
					if (!this._isGrouping)
					{
						base.OnCollectionChanged(null, notifyCollectionChangedEventArgs);
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
		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x0007B1CD File Offset: 0x000793CD
		public bool CanAddNew
		{
			get
			{
				return !this.IsEditingItem && this.InternalList.AllowNew;
			}
		}

		/// <summary>Starts an add transaction and returns the pending new item.</summary>
		/// <returns>The pending new item.</returns>
		// Token: 0x060019D4 RID: 6612 RVA: 0x0007B1E4 File Offset: 0x000793E4
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
			object newItem = null;
			BindingOperations.AccessCollection(this.InternalList, delegate
			{
				this.ProcessPendingChanges();
				this._newItemIndex = -2;
				newItem = this.InternalList.AddNew();
			}, true);
			this.MoveCurrentTo(newItem);
			ISupportInitialize supportInitialize = newItem as ISupportInitialize;
			if (supportInitialize != null)
			{
				supportInitialize.BeginInit();
			}
			if (!this.IsDataView)
			{
				IEditableObject editableObject = newItem as IEditableObject;
				if (editableObject != null)
				{
					editableObject.BeginEdit();
				}
			}
			return newItem;
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0007B2A4 File Offset: 0x000794A4
		private void BeginAddNew(object newItem, int index)
		{
			this.SetNewItem(newItem);
			this._newItemIndex = index;
			int index2 = index;
			if (!this._isGrouping)
			{
				switch (this.NewItemPlaceholderPosition)
				{
				case NewItemPlaceholderPosition.AtBeginning:
					this._newItemIndex--;
					index2 = 1;
					break;
				case NewItemPlaceholderPosition.AtEnd:
					index2 = this.InternalCount - 2;
					break;
				}
			}
			this.ProcessCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, index2));
		}

		/// <summary>Ends the add transaction and saves the pending new item.</summary>
		// Token: 0x060019D6 RID: 6614 RVA: 0x0007B310 File Offset: 0x00079510
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
			ICancelAddNew ican = this.InternalList as ICancelAddNew;
			IEditableObject ieo;
			BindingOperations.AccessCollection(this.InternalList, delegate
			{
				this.ProcessPendingChanges();
				if (ican != null)
				{
					ican.EndNew(this._newItemIndex);
					return;
				}
				if ((ieo = (this._newItem as IEditableObject)) != null)
				{
					ieo.EndEdit();
				}
			}, true);
			if (this._newItem != CollectionView.NoNewItem)
			{
				int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
				NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = this.ProcessCommitNew(this._newItemIndex, this._newItemIndex + num);
				if (notifyCollectionChangedEventArgs != null)
				{
					base.OnCollectionChanged(this.InternalList, notifyCollectionChangedEventArgs);
				}
			}
		}

		/// <summary>Ends the add transaction and discards the pending new item.</summary>
		// Token: 0x060019D7 RID: 6615 RVA: 0x0007B3D8 File Offset: 0x000795D8
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
			ICancelAddNew ican = this.InternalList as ICancelAddNew;
			IEditableObject ieo;
			BindingOperations.AccessCollection(this.InternalList, delegate
			{
				this.ProcessPendingChanges();
				if (ican != null)
				{
					ican.CancelNew(this._newItemIndex);
					return;
				}
				if ((ieo = (this._newItem as IEditableObject)) != null)
				{
					ieo.CancelEdit();
				}
			}, true);
			object newItem = this._newItem;
			object noNewItem = CollectionView.NoNewItem;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0007B46C File Offset: 0x0007966C
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

		// Token: 0x060019D9 RID: 6617 RVA: 0x0007B4B8 File Offset: 0x000796B8
		private NotifyCollectionChangedEventArgs ProcessCommitNew(int fromIndex, int toIndex)
		{
			if (this._isGrouping)
			{
				this.CommitNewForGrouping();
				return null;
			}
			switch (this.NewItemPlaceholderPosition)
			{
			case NewItemPlaceholderPosition.AtBeginning:
				fromIndex = 1;
				break;
			case NewItemPlaceholderPosition.AtEnd:
				fromIndex = this.InternalCount - 2;
				break;
			}
			object changedItem = this.EndAddNew(false);
			NotifyCollectionChangedEventArgs result = null;
			if (fromIndex != toIndex)
			{
				result = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, changedItem, toIndex, fromIndex);
			}
			return result;
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0007B518 File Offset: 0x00079718
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
			object item = this.EndAddNew(false);
			this._group.RemoveSpecialItem(index, item, false);
			this.AddItemToGroups(item);
		}

		/// <summary>Gets a value that indicates whether an add transaction is in progress.</summary>
		/// <returns>
		///     <see langword="true" /> if an add transaction is in progress; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x0007B587 File Offset: 0x00079787
		public bool IsAddingNew
		{
			get
			{
				return this._newItem != CollectionView.NoNewItem;
			}
		}

		/// <summary>Gets the item that is being added during the current add transaction.</summary>
		/// <returns>The item that is being added if <see cref="P:System.Windows.Data.BindingListCollectionView.IsAddingNew" /> is <see langword="true" />; otherwise, <see langword="null" />.</returns>
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060019DC RID: 6620 RVA: 0x0007B599 File Offset: 0x00079799
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

		// Token: 0x060019DD RID: 6621 RVA: 0x0007B5AB File Offset: 0x000797AB
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
		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060019DE RID: 6622 RVA: 0x0007B5E3 File Offset: 0x000797E3
		public bool CanRemove
		{
			get
			{
				return !this.IsEditingItem && !this.IsAddingNew && this.InternalList.AllowRemove;
			}
		}

		/// <summary>Removes the item at the specified position from the collection.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0 or greater than the number of items in the collection view.</exception>
		// Token: 0x060019DF RID: 6623 RVA: 0x0007B604 File Offset: 0x00079804
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
		// Token: 0x060019E0 RID: 6624 RVA: 0x0007B654 File Offset: 0x00079854
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

		// Token: 0x060019E1 RID: 6625 RVA: 0x0007B6AC File Offset: 0x000798AC
		private void RemoveImpl(object item, int index)
		{
			if (item == CollectionView.NewItemPlaceholder)
			{
				throw new InvalidOperationException(SR.Get("RemovingPlaceholder"));
			}
			BindingOperations.AccessCollection(this.InternalList, delegate
			{
				this.ProcessPendingChanges();
				if (index >= this.InternalList.Count || !ItemsControl.EqualsEx(item, this.GetItemAt(index)))
				{
					index = this.InternalList.IndexOf(item);
					if (index < 0)
					{
						return;
					}
				}
				if (this._isGrouping)
				{
					index = this.InternalList.IndexOf(item);
				}
				else
				{
					int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
					index -= num;
				}
				try
				{
					this._isRemoving = true;
					this.InternalList.RemoveAt(index);
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
		// Token: 0x060019E2 RID: 6626 RVA: 0x0007B70C File Offset: 0x0007990C
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
		// Token: 0x060019E3 RID: 6627 RVA: 0x0007B778 File Offset: 0x00079978
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
			IEditableObject ieo = this._editItem as IEditableObject;
			object editItem = this._editItem;
			this.SetEditItem(null);
			if (ieo != null)
			{
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					this.ProcessPendingChanges();
					ieo.EndEdit();
				}, true);
			}
			if (this._isGrouping)
			{
				this.RemoveItemFromGroups(editItem);
				this.AddItemToGroups(editItem);
				return;
			}
		}

		/// <summary>Ends the edit transaction and, if possible, restores the original value to the item.</summary>
		// Token: 0x060019E4 RID: 6628 RVA: 0x0007B828 File Offset: 0x00079A28
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

		// Token: 0x060019E5 RID: 6629 RVA: 0x0007B8A0 File Offset: 0x00079AA0
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
		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060019E6 RID: 6630 RVA: 0x0007B8C9 File Offset: 0x00079AC9
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
		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x0007B8D9 File Offset: 0x00079AD9
		public bool IsEditingItem
		{
			get
			{
				return this._editItem != null;
			}
		}

		/// <summary>Gets the item in the collection that is being edited.</summary>
		/// <returns>The item in the collection that is being edited if <see cref="P:System.Windows.Data.ListCollectionView.IsEditingItem" /> is <see langword="true" />; otherwise, <see langword="null" />.</returns>
		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060019E8 RID: 6632 RVA: 0x0007B8E4 File Offset: 0x00079AE4
		public object CurrentEditItem
		{
			get
			{
				return this._editItem;
			}
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0007B8EC File Offset: 0x00079AEC
		private void SetEditItem(object item)
		{
			if (!ItemsControl.EqualsEx(item, this._editItem))
			{
				this._editItem = item;
				this.OnPropertyChanged("CurrentEditItem");
				this.OnPropertyChanged("IsEditingItem");
				this.OnPropertyChanged("CanCancelEdit");
				this.OnPropertyChanged("CanAddNew");
				this.OnPropertyChanged("CanRemove");
			}
		}

		/// <summary>Gets a value that indicates whether this view supports turning sorting data in real time on or off.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060019EA RID: 6634 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool CanChangeLiveSorting
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value that indicates whether this view supports turning filtering data in real time on or off.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool CanChangeLiveFiltering
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value that indicates whether this view supports turning grouping data in real time on or off.</summary>
		/// <returns>
		///     <see langword="True" /> in all cases.</returns>
		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x00016748 File Offset: 0x00014948
		public bool CanChangeLiveGrouping
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a value that indicates whether sorting data in real time is enabled.</summary>
		/// <returns>
		///
		///     <see langword="true" /> if sorting data in real time is enabled; <see langword="false" /> if live sorting is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live sorting.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Data.BindingListCollectionView.IsLiveSorting" /> cannot be set.</exception>
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x0007B948 File Offset: 0x00079B48
		// (set) Token: 0x060019EE RID: 6638 RVA: 0x0007B96D File Offset: 0x00079B6D
		public bool? IsLiveSorting
		{
			get
			{
				if (!this.IsDataView)
				{
					return null;
				}
				return new bool?(true);
			}
			set
			{
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveSorting",
					"CanChangeLiveSorting"
				}));
			}
		}

		/// <summary>Gets or sets a value that indicates whether filtering data in real time is enabled.</summary>
		/// <returns>
		///
		///     <see langword="true" /> if filtering data in real time is enabled; <see langword="false" /> if live filtering is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live filtering.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Data.BindingListCollectionView.IsLiveFiltering" /> cannot be set.</exception>
		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x0007B994 File Offset: 0x00079B94
		// (set) Token: 0x060019F0 RID: 6640 RVA: 0x0007B9B9 File Offset: 0x00079BB9
		public bool? IsLiveFiltering
		{
			get
			{
				if (!this.IsDataView)
				{
					return null;
				}
				return new bool?(true);
			}
			set
			{
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveFiltering",
					"CanChangeLiveFiltering"
				}));
			}
		}

		/// <summary>Gets or sets a value that indicates whether grouping data in real time is enabled.</summary>
		/// <returns>
		///
		///     <see langword="true" /> if grouping data in real time is enabled; <see langword="false" /> if live grouping is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live grouping.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <see cref="P:System.Windows.Data.BindingListCollectionView.IsLiveGrouping" /> cannot be set to <see langword="null" />.</exception>
		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060019F1 RID: 6641 RVA: 0x0007B9E0 File Offset: 0x00079BE0
		// (set) Token: 0x060019F2 RID: 6642 RVA: 0x0007B9E8 File Offset: 0x00079BE8
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
					base.RefreshOrDefer();
					this.OnPropertyChanged("IsLiveGrouping");
				}
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in sorting data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in sorting data in real time.</returns>
		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x0007BA52 File Offset: 0x00079C52
		public ObservableCollection<string> LiveSortingProperties
		{
			get
			{
				if (this._liveSortingProperties == null)
				{
					this._liveSortingProperties = new ObservableCollection<string>();
				}
				return this._liveSortingProperties;
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in filtering data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in filtering data in real time.</returns>
		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x0007BA6D File Offset: 0x00079C6D
		public ObservableCollection<string> LiveFilteringProperties
		{
			get
			{
				if (this._liveFilteringProperties == null)
				{
					this._liveFilteringProperties = new ObservableCollection<string>();
				}
				return this._liveFilteringProperties;
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in grouping data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in grouping data in real time.</returns>
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060019F5 RID: 6645 RVA: 0x0007BA88 File Offset: 0x00079C88
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

		// Token: 0x060019F6 RID: 6646 RVA: 0x0007BABC File Offset: 0x00079CBC
		private void OnLivePropertyListChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.IsLiveGrouping == true)
			{
				base.RefreshOrDefer();
			}
		}

		/// <summary>Gets a collection of objects that describes the properties of the items in the collection.</summary>
		/// <returns>A collection of objects that describes the properties of the items in the collection.</returns>
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x0007BAEE File Offset: 0x00079CEE
		public ReadOnlyCollection<ItemPropertyInfo> ItemProperties
		{
			get
			{
				return base.GetItemProperties();
			}
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0007BAF8 File Offset: 0x00079CF8
		protected override void RefreshOverride()
		{
			object currentItem = this.CurrentItem;
			int num = this.IsEmpty ? 0 : this.CurrentPosition;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			base.OnCurrentChanging();
			this._ignoreInnerRefresh = true;
			if (this.IsCustomFilterSet || this._isFiltered)
			{
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					if (this.IsCustomFilterSet)
					{
						this._isFiltered = true;
						this._blv.Filter = this._customFilter;
						return;
					}
					if (this._isFiltered)
					{
						this._isFiltered = false;
						this._blv.RemoveFilter();
					}
				}, true);
			}
			if (this._sort != null && this._sort.Count > 0 && this.CollectionProxy != null && this.CollectionProxy.Count > 0)
			{
				ListSortDescriptionCollection sorts = this.ConvertSortDescriptionCollection(this._sort);
				if (sorts.Count > 0)
				{
					this._isSorted = true;
					BindingOperations.AccessCollection(this.InternalList, delegate
					{
						if (this._blv == null)
						{
							this.InternalList.ApplySort(sorts[0].PropertyDescriptor, sorts[0].SortDirection);
							return;
						}
						this._blv.ApplySort(sorts);
					}, true);
				}
				this.ActiveComparer = new SortFieldComparer(this._sort, this.Culture);
			}
			else if (this._isSorted)
			{
				this._isSorted = false;
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					this.InternalList.RemoveSort();
				}, true);
				this.ActiveComparer = null;
			}
			this.InitializeGrouping();
			this.PrepareCachedList();
			this.PrepareGroups();
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
			this._ignoreInnerRefresh = false;
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

		// Token: 0x060019F9 RID: 6649 RVA: 0x0007BD25 File Offset: 0x00079F25
		protected override void OnAllowsCrossThreadChangesChanged()
		{
			this.PrepareCachedList();
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0007BD2D File Offset: 0x00079F2D
		private void PrepareCachedList()
		{
			if (base.AllowsCrossThreadChanges)
			{
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					this.RebuildLists();
				}, false);
				return;
			}
			this.RebuildListsCore();
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0007BD58 File Offset: 0x00079F58
		private void RebuildLists()
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				base.ClearPendingChanges();
				this.RebuildListsCore();
			}
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x0007BDA0 File Offset: 0x00079FA0
		private void RebuildListsCore()
		{
			this._cachedList = new ArrayList(this.InternalList);
			LiveShapingList liveShapingList = this._shadowList as LiveShapingList;
			if (liveShapingList != null)
			{
				liveShapingList.LiveShapingDirty -= this.OnLiveShapingDirty;
			}
			if (this._isGrouping && this.IsLiveGrouping == true)
			{
				liveShapingList = (this._shadowList = new LiveShapingList(this, this.GetLiveShapingFlags(), this.ActiveComparer));
				foreach (object value in this.InternalList)
				{
					liveShapingList.Add(value);
				}
				liveShapingList.LiveShapingDirty += this.OnLiveShapingDirty;
				return;
			}
			if (base.AllowsCrossThreadChanges)
			{
				this._shadowList = new ArrayList(this.InternalList);
				return;
			}
			this._shadowList = null;
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00002137 File Offset: 0x00000337
		[Obsolete("Replaced by OnAllowsCrossThreadChangesChanged")]
		protected override void OnBeginChangeLogging(NotifyCollectionChangedEventArgs args)
		{
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0007BEA4 File Offset: 0x0007A0A4
		protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			bool flag = false;
			this.ValidateCollectionChangedEventArgs(args);
			int currentPosition = this.CurrentPosition;
			int currentPosition2 = this.CurrentPosition;
			object currentItem = this.CurrentItem;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			bool flag2 = false;
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (this._newItemIndex == -2)
				{
					this.BeginAddNew(args.NewItems[0], args.NewStartingIndex);
					return;
				}
				if (this._isGrouping)
				{
					this.AddItemToGroups(args.NewItems[0]);
				}
				else
				{
					this.AdjustCurrencyForAdd(args.NewStartingIndex);
					flag = true;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (this._isGrouping)
				{
					this.RemoveItemFromGroups(args.OldItems[0]);
				}
				else
				{
					flag2 = this.AdjustCurrencyForRemove(args.OldStartingIndex);
					flag = true;
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (this._isGrouping)
				{
					this.RemoveItemFromGroups(args.OldItems[0]);
					this.AddItemToGroups(args.NewItems[0]);
				}
				else
				{
					flag2 = this.AdjustCurrencyForReplace(args.NewStartingIndex);
					flag = true;
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (!this._isGrouping)
				{
					this.AdjustCurrencyForMove(args.OldStartingIndex, args.NewStartingIndex);
					flag = true;
				}
				else
				{
					this._group.MoveWithinSubgroups(args.OldItems[0], null, this.InternalList, args.OldStartingIndex, args.NewStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				if (this._isGrouping)
				{
					base.RefreshOrDefer();
				}
				else
				{
					flag = true;
				}
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			if (base.AllowsCrossThreadChanges)
			{
				this.AdjustShadowCopy(args);
			}
			bool flag3 = this.IsCurrentAfterLast != isCurrentAfterLast;
			bool flag4 = this.IsCurrentBeforeFirst != isCurrentBeforeFirst;
			bool flag5 = this.CurrentPosition != currentPosition2;
			bool flag6 = this.CurrentItem != currentItem;
			isCurrentAfterLast = this.IsCurrentAfterLast;
			isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			currentPosition2 = this.CurrentPosition;
			currentItem = this.CurrentItem;
			if (flag)
			{
				this.OnCollectionChanged(args);
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
			if (flag2)
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

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x0007C1B9 File Offset: 0x0007A3B9
		private int InternalCount
		{
			get
			{
				if (this._isGrouping)
				{
					return this._group.ItemCount;
				}
				return ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.None) ? 0 : 1) + this.CollectionProxy.Count;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x0007C1E7 File Offset: 0x0007A3E7
		private bool IsDataView
		{
			get
			{
				return this._isDataView;
			}
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0007C1F0 File Offset: 0x0007A3F0
		private int InternalIndexOf(object item)
		{
			if (this._isGrouping)
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
				case NewItemPlaceholderPosition.AtBeginning:
					return 1;
				case NewItemPlaceholderPosition.AtEnd:
					return this.InternalCount - 2;
				}
			}
			int num = this.CollectionProxy.IndexOf(item);
			if (num >= this.CollectionProxy.Count)
			{
				num = -1;
			}
			if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning && num >= 0)
			{
				num += (this.IsAddingNew ? 2 : 1);
			}
			return num;
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0007C2B8 File Offset: 0x0007A4B8
		private object InternalItemAt(int index)
		{
			if (this._isGrouping)
			{
				return this._group.LeafAt(index);
			}
			switch (this.NewItemPlaceholderPosition)
			{
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
					if (index <= this._newItemIndex + 1)
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
				if (this.IsAddingNew && index == this.InternalCount - 2)
				{
					return this._newItem;
				}
				break;
			}
			return this.CollectionProxy[index];
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x0007C35E File Offset: 0x0007A55E
		private bool InternalContains(object item)
		{
			if (item == CollectionView.NewItemPlaceholder)
			{
				return this.NewItemPlaceholderPosition > NewItemPlaceholderPosition.None;
			}
			if (this._isGrouping)
			{
				return this._group.LeafIndexOf(item) >= 0;
			}
			return this.CollectionProxy.Contains(item);
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0007C399 File Offset: 0x0007A599
		private IEnumerator InternalGetEnumerator()
		{
			if (!this._isGrouping)
			{
				return new CollectionView.PlaceholderAwareEnumerator(this, this.CollectionProxy.GetEnumerator(), this.NewItemPlaceholderPosition, this._newItem);
			}
			return this._group.GetLeafEnumerator();
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0007C3CC File Offset: 0x0007A5CC
		private void AdjustShadowCopy(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this._shadowList.Insert(e.NewStartingIndex, e.NewItems[0]);
				return;
			case NotifyCollectionChangedAction.Remove:
				this._shadowList.RemoveAt(e.OldStartingIndex);
				return;
			case NotifyCollectionChangedAction.Replace:
				this._shadowList[e.OldStartingIndex] = e.NewItems[0];
				return;
			case NotifyCollectionChangedAction.Move:
				this._shadowList.Move(e.OldStartingIndex, e.NewStartingIndex);
				return;
			default:
				return;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001A06 RID: 6662 RVA: 0x0007C45C File Offset: 0x0007A65C
		private bool IsCurrentInView
		{
			get
			{
				return 0 <= this.CurrentPosition && this.CurrentPosition < this.InternalCount;
			}
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0007C478 File Offset: 0x0007A678
		private void _MoveTo(int proposed)
		{
			if (proposed == this.CurrentPosition || this.IsEmpty)
			{
				return;
			}
			object obj = (0 <= proposed && proposed < this.InternalCount) ? this.GetItemAt(proposed) : null;
			if (obj == CollectionView.NewItemPlaceholder)
			{
				return;
			}
			if (base.OKToChangeCurrent())
			{
				bool isCurrentAfterLast = this.IsCurrentAfterLast;
				bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
				base.SetCurrent(obj, proposed);
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

		// Token: 0x06001A08 RID: 6664 RVA: 0x0007C51A File Offset: 0x0007A71A
		private void SubscribeToChanges()
		{
			if (this.InternalList.SupportsChangeNotification)
			{
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					this.InternalList.ListChanged += this.OnListChanged;
					this.RebuildLists();
				}, false);
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0007C544 File Offset: 0x0007A744
		private void OnListChanged(object sender, ListChangedEventArgs args)
		{
			if (this._ignoreInnerRefresh && args.ListChangedType == ListChangedType.Reset)
			{
				return;
			}
			NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = null;
			int num = this._isGrouping ? 0 : ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0);
			int num2 = args.NewIndex;
			switch (args.ListChangedType)
			{
			case ListChangedType.Reset:
			case ListChangedType.PropertyDescriptorAdded:
			case ListChangedType.PropertyDescriptorDeleted:
			case ListChangedType.PropertyDescriptorChanged:
				break;
			case ListChangedType.ItemAdded:
				if (this.InternalList.Count == this._cachedList.Count)
				{
					if (this.IsAddingNew && num2 == this._newItemIndex)
					{
						notifyCollectionChangedEventArgs = this.ProcessCommitNew(num2 + num, num2 + num);
						goto IL_38D;
					}
					goto IL_38D;
				}
				else
				{
					object obj = this.InternalList[num2];
					notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, obj, num2 + num);
					this._cachedList.Insert(num2, obj);
					if (this.InternalList.Count != this._cachedList.Count)
					{
						throw new InvalidOperationException(SR.Get("InconsistentBindingList", new object[]
						{
							this.InternalList,
							args.ListChangedType
						}));
					}
					if (num2 <= this._newItemIndex)
					{
						this._newItemIndex++;
						goto IL_38D;
					}
					goto IL_38D;
				}
				break;
			case ListChangedType.ItemDeleted:
			{
				object obj = this._cachedList[num2];
				this._cachedList.RemoveAt(num2);
				if (this.InternalList.Count != this._cachedList.Count)
				{
					throw new InvalidOperationException(SR.Get("InconsistentBindingList", new object[]
					{
						this.InternalList,
						args.ListChangedType
					}));
				}
				if (num2 < this._newItemIndex)
				{
					this._newItemIndex--;
				}
				if (obj == this.CurrentEditItem)
				{
					this.ImplicitlyCancelEdit();
				}
				if (obj == this.CurrentAddItem)
				{
					this.EndAddNew(true);
					NewItemPlaceholderPosition newItemPlaceholderPosition = this.NewItemPlaceholderPosition;
					if (newItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)
					{
						if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
						{
							num2 = this.InternalCount - 1;
						}
					}
					else
					{
						num2 = 0;
					}
				}
				notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, obj, num2 + num);
				goto IL_38D;
			}
			case ListChangedType.ItemMoved:
			{
				object obj;
				if (this.IsAddingNew && args.OldIndex == this._newItemIndex)
				{
					obj = this._newItem;
					notifyCollectionChangedEventArgs = this.ProcessCommitNew(args.OldIndex, num2 + num);
				}
				else
				{
					obj = this.InternalList[num2];
					notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, obj, num2 + num, args.OldIndex + num);
					if (args.OldIndex < this._newItemIndex && this._newItemIndex < args.NewIndex)
					{
						this._newItemIndex--;
					}
					else if (args.NewIndex <= this._newItemIndex && this._newItemIndex < args.OldIndex)
					{
						this._newItemIndex++;
					}
				}
				this._cachedList.RemoveAt(args.OldIndex);
				this._cachedList.Insert(args.NewIndex, obj);
				if (this.InternalList.Count != this._cachedList.Count)
				{
					throw new InvalidOperationException(SR.Get("InconsistentBindingList", new object[]
					{
						this.InternalList,
						args.ListChangedType
					}));
				}
				goto IL_38D;
			}
			case ListChangedType.ItemChanged:
				if (this._itemsRaisePropertyChanged == null)
				{
					object obj = this.InternalList[args.NewIndex];
					this._itemsRaisePropertyChanged = new bool?(obj is INotifyPropertyChanged);
				}
				if (this._itemsRaisePropertyChanged.Value)
				{
					goto IL_38D;
				}
				break;
			default:
				goto IL_38D;
			}
			if (this.IsEditingItem)
			{
				this.ImplicitlyCancelEdit();
			}
			if (this.IsAddingNew)
			{
				this._newItemIndex = this.InternalList.IndexOf(this._newItem);
				if (this._newItemIndex < 0)
				{
					this.EndAddNew(true);
				}
			}
			base.RefreshOrDefer();
			IL_38D:
			if (notifyCollectionChangedEventArgs != null)
			{
				base.OnCollectionChanged(sender, notifyCollectionChangedEventArgs);
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x0007C8EC File Offset: 0x0007AAEC
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

		// Token: 0x06001A0B RID: 6667 RVA: 0x0007C944 File Offset: 0x0007AB44
		private bool AdjustCurrencyForRemove(int index)
		{
			bool result = index == this.CurrentPosition;
			if (index < this.CurrentPosition)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
			}
			return result;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0007C97C File Offset: 0x0007AB7C
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

		// Token: 0x06001A0D RID: 6669 RVA: 0x0007C9F0 File Offset: 0x0007ABF0
		private bool AdjustCurrencyForReplace(int index)
		{
			bool flag = index == this.CurrentPosition;
			if (flag)
			{
				base.SetCurrent(this.GetItemAt(index), index);
			}
			return flag;
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0007CA1C File Offset: 0x0007AC1C
		private void MoveCurrencyOffDeletedElement(int oldCurrentPosition)
		{
			int num = this.InternalCount - 1;
			int num2 = (oldCurrentPosition < num) ? oldCurrentPosition : num;
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

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001A0F RID: 6671 RVA: 0x0007CA63 File Offset: 0x0007AC63
		private IList CollectionProxy
		{
			get
			{
				if (this._shadowList != null)
				{
					return this._shadowList;
				}
				return this.InternalList;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001A10 RID: 6672 RVA: 0x0007CA7A File Offset: 0x0007AC7A
		// (set) Token: 0x06001A11 RID: 6673 RVA: 0x0007CA82 File Offset: 0x0007AC82
		private IBindingList InternalList
		{
			get
			{
				return this._internalList;
			}
			set
			{
				this._internalList = value;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x0007CA8B File Offset: 0x0007AC8B
		private bool IsCustomFilterSet
		{
			get
			{
				return this._blv != null && !string.IsNullOrEmpty(this._customFilter);
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001A13 RID: 6675 RVA: 0x00016748 File Offset: 0x00014948
		private bool CanGroupNamesChange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0007CAA5 File Offset: 0x0007ACA5
		private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.IsAddingNew || this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Sorting"
				}));
			}
			base.RefreshOrDefer();
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0007CADC File Offset: 0x0007ACDC
		private ListSortDescriptionCollection ConvertSortDescriptionCollection(SortDescriptionCollection sorts)
		{
			ITypedList typedList;
			PropertyDescriptorCollection propertyDescriptorCollection;
			Type itemType;
			if ((typedList = (this.InternalList as ITypedList)) != null)
			{
				propertyDescriptorCollection = typedList.GetItemProperties(null);
			}
			else if ((itemType = base.GetItemType(true)) != null)
			{
				propertyDescriptorCollection = TypeDescriptor.GetProperties(itemType);
			}
			else
			{
				propertyDescriptorCollection = null;
			}
			if (propertyDescriptorCollection == null || propertyDescriptorCollection.Count == 0)
			{
				throw new ArgumentException(SR.Get("CannotDetermineSortByPropertiesForCollection"));
			}
			ListSortDescription[] array = new ListSortDescription[sorts.Count];
			for (int i = 0; i < sorts.Count; i++)
			{
				PropertyDescriptor propertyDescriptor = propertyDescriptorCollection.Find(sorts[i].PropertyName, true);
				if (propertyDescriptor == null)
				{
					string listName = typedList.GetListName(null);
					throw new ArgumentException(SR.Get("PropertyToSortByNotFoundOnType", new object[]
					{
						listName,
						sorts[i].PropertyName
					}));
				}
				ListSortDescription listSortDescription = new ListSortDescription(propertyDescriptor, sorts[i].Direction);
				array[i] = listSortDescription;
			}
			return new ListSortDescriptionCollection(array);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0007CBD9 File Offset: 0x0007ADD9
		private void InitializeGrouping()
		{
			this._group.Clear();
			this._group.Initialize();
			this._isGrouping = (this._group.GroupBy != null);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0007CC08 File Offset: 0x0007AE08
		private void PrepareGroups()
		{
			if (!this._isGrouping)
			{
				return;
			}
			IList collectionProxy = this.CollectionProxy;
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
					listComparer.ResetList(collectionProxy);
				}
				else
				{
					this._group.ActiveComparer = new CollectionViewGroupInternal.IListComparer(collectionProxy);
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
			LiveShapingList liveShapingList = collectionProxy as LiveShapingList;
			int i = 0;
			int count = collectionProxy.Count;
			while (i < count)
			{
				object obj = collectionProxy[i];
				LiveShapingItem lsi = flag ? liveShapingList.ItemAt(i) : null;
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

		// Token: 0x06001A18 RID: 6680 RVA: 0x0007CD83 File Offset: 0x0007AF83
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

		// Token: 0x06001A19 RID: 6681 RVA: 0x0007CDB8 File Offset: 0x0007AFB8
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

		// Token: 0x06001A1A RID: 6682 RVA: 0x0007CDB8 File Offset: 0x0007AFB8
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

		// Token: 0x06001A1B RID: 6683 RVA: 0x0007CDF0 File Offset: 0x0007AFF0
		private void AddItemToGroups(object item)
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
			this._group.AddToSubgroups(item, null, false);
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0007CE6E File Offset: 0x0007B06E
		private void RemoveItemFromGroups(object item)
		{
			if (this.CanGroupNamesChange || this._group.RemoveFromSubgroups(item))
			{
				this._group.RemoveItemFromSubgroupsByExhaustiveSearch(item);
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0007CE94 File Offset: 0x0007B094
		private LiveShapingFlags GetLiveShapingFlags()
		{
			LiveShapingFlags liveShapingFlags = (LiveShapingFlags)0;
			if (this.IsLiveGrouping == true)
			{
				liveShapingFlags |= LiveShapingFlags.Grouping;
			}
			return liveShapingFlags;
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x0007CEC8 File Offset: 0x0007B0C8
		internal void RestoreLiveShaping()
		{
			LiveShapingList liveShapingList = this.CollectionProxy as LiveShapingList;
			if (liveShapingList == null)
			{
				return;
			}
			if (this._isGrouping)
			{
				List<AbandonedGroupItem> deleteList = new List<AbandonedGroupItem>();
				foreach (LiveShapingItem liveShapingItem in liveShapingList.GroupDirtyItems)
				{
					if (!liveShapingItem.IsDeleted)
					{
						this._group.RestoreGrouping(liveShapingItem, deleteList);
						liveShapingItem.IsGroupDirty = false;
					}
				}
				this._group.DeleteAbandonedGroupItems(deleteList);
			}
			liveShapingList.GroupDirtyItems.Clear();
			this.IsLiveShapingDirty = false;
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001A1F RID: 6687 RVA: 0x0007CF6C File Offset: 0x0007B16C
		// (set) Token: 0x06001A20 RID: 6688 RVA: 0x0007CF74 File Offset: 0x0007B174
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

		// Token: 0x06001A21 RID: 6689 RVA: 0x0007CFA3 File Offset: 0x0007B1A3
		private void OnLiveShapingDirty(object sender, EventArgs e)
		{
			this.IsLiveShapingDirty = true;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0007CFAC File Offset: 0x0007B1AC
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

		// Token: 0x06001A23 RID: 6691 RVA: 0x0007D0A8 File Offset: 0x0007B2A8
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0007D0B6 File Offset: 0x0007B2B6
		private void DeferAction(Action action)
		{
			if (this._deferredActions == null)
			{
				this._deferredActions = new List<Action>();
			}
			this._deferredActions.Add(action);
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0007D0D8 File Offset: 0x0007B2D8
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

		// Token: 0x04001330 RID: 4912
		private IBindingList _internalList;

		// Token: 0x04001331 RID: 4913
		private CollectionViewGroupRoot _group;

		// Token: 0x04001332 RID: 4914
		private bool _isGrouping;

		// Token: 0x04001333 RID: 4915
		private IBindingListView _blv;

		// Token: 0x04001334 RID: 4916
		private BindingListCollectionView.BindingListSortDescriptionCollection _sort;

		// Token: 0x04001335 RID: 4917
		private IList _shadowList;

		// Token: 0x04001336 RID: 4918
		private bool _isSorted;

		// Token: 0x04001337 RID: 4919
		private IComparer _comparer;

		// Token: 0x04001338 RID: 4920
		private string _customFilter;

		// Token: 0x04001339 RID: 4921
		private bool _isFiltered;

		// Token: 0x0400133A RID: 4922
		private bool _ignoreInnerRefresh;

		// Token: 0x0400133B RID: 4923
		private bool? _itemsRaisePropertyChanged;

		// Token: 0x0400133C RID: 4924
		private bool _isDataView;

		// Token: 0x0400133D RID: 4925
		private object _newItem = CollectionView.NoNewItem;

		// Token: 0x0400133E RID: 4926
		private object _editItem;

		// Token: 0x0400133F RID: 4927
		private int _newItemIndex;

		// Token: 0x04001340 RID: 4928
		private NewItemPlaceholderPosition _newItemPlaceholderPosition;

		// Token: 0x04001341 RID: 4929
		private List<Action> _deferredActions;

		// Token: 0x04001342 RID: 4930
		private bool _isRemoving;

		// Token: 0x04001343 RID: 4931
		private bool? _isLiveGrouping = new bool?(false);

		// Token: 0x04001344 RID: 4932
		private bool _isLiveShapingDirty;

		// Token: 0x04001345 RID: 4933
		private ObservableCollection<string> _liveSortingProperties;

		// Token: 0x04001346 RID: 4934
		private ObservableCollection<string> _liveFilteringProperties;

		// Token: 0x04001347 RID: 4935
		private ObservableCollection<string> _liveGroupingProperties;

		// Token: 0x04001348 RID: 4936
		private IList _cachedList;

		// Token: 0x0200086F RID: 2159
		private class BindingListSortDescriptionCollection : SortDescriptionCollection
		{
			// Token: 0x06008308 RID: 33544 RVA: 0x00244641 File Offset: 0x00242841
			internal BindingListSortDescriptionCollection(bool allowMultipleDescriptions)
			{
				this._allowMultipleDescriptions = allowMultipleDescriptions;
			}

			// Token: 0x06008309 RID: 33545 RVA: 0x00244650 File Offset: 0x00242850
			protected override void InsertItem(int index, SortDescription item)
			{
				if (!this._allowMultipleDescriptions && base.Count > 0)
				{
					throw new InvalidOperationException(SR.Get("BindingListCanOnlySortByOneProperty"));
				}
				base.InsertItem(index, item);
			}

			// Token: 0x04004125 RID: 16677
			private bool _allowMultipleDescriptions;
		}
	}
}
