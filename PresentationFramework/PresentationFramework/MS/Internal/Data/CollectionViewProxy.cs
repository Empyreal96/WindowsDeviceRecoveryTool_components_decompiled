using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200070E RID: 1806
	internal class CollectionViewProxy : CollectionView, IEditableCollectionViewAddNewItem, IEditableCollectionView, ICollectionViewLiveShaping, IItemProperties
	{
		// Token: 0x060073E6 RID: 29670 RVA: 0x00212CA4 File Offset: 0x00210EA4
		internal CollectionViewProxy(ICollectionView view) : base(view.SourceCollection, false)
		{
			this._view = view;
			view.CollectionChanged += this._OnViewChanged;
			view.CurrentChanging += this._OnCurrentChanging;
			view.CurrentChanged += this._OnCurrentChanged;
			INotifyPropertyChanged notifyPropertyChanged = view as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged += this._OnPropertyChanged;
			}
		}

		// Token: 0x17001B89 RID: 7049
		// (get) Token: 0x060073E7 RID: 29671 RVA: 0x00212D17 File Offset: 0x00210F17
		// (set) Token: 0x060073E8 RID: 29672 RVA: 0x00212D24 File Offset: 0x00210F24
		public override CultureInfo Culture
		{
			get
			{
				return this.ProxiedView.Culture;
			}
			set
			{
				this.ProxiedView.Culture = value;
			}
		}

		// Token: 0x060073E9 RID: 29673 RVA: 0x00212D32 File Offset: 0x00210F32
		public override bool Contains(object item)
		{
			return this.ProxiedView.Contains(item);
		}

		// Token: 0x17001B8A RID: 7050
		// (get) Token: 0x060073EA RID: 29674 RVA: 0x00212D40 File Offset: 0x00210F40
		public override IEnumerable SourceCollection
		{
			get
			{
				return base.SourceCollection;
			}
		}

		// Token: 0x17001B8B RID: 7051
		// (get) Token: 0x060073EB RID: 29675 RVA: 0x00212D48 File Offset: 0x00210F48
		// (set) Token: 0x060073EC RID: 29676 RVA: 0x00212D55 File Offset: 0x00210F55
		public override Predicate<object> Filter
		{
			get
			{
				return this.ProxiedView.Filter;
			}
			set
			{
				this.ProxiedView.Filter = value;
			}
		}

		// Token: 0x17001B8C RID: 7052
		// (get) Token: 0x060073ED RID: 29677 RVA: 0x00212D63 File Offset: 0x00210F63
		public override bool CanFilter
		{
			get
			{
				return this.ProxiedView.CanFilter;
			}
		}

		// Token: 0x17001B8D RID: 7053
		// (get) Token: 0x060073EE RID: 29678 RVA: 0x00212D70 File Offset: 0x00210F70
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this.ProxiedView.SortDescriptions;
			}
		}

		// Token: 0x17001B8E RID: 7054
		// (get) Token: 0x060073EF RID: 29679 RVA: 0x00212D7D File Offset: 0x00210F7D
		public override bool CanSort
		{
			get
			{
				return this.ProxiedView.CanSort;
			}
		}

		// Token: 0x17001B8F RID: 7055
		// (get) Token: 0x060073F0 RID: 29680 RVA: 0x00212D8A File Offset: 0x00210F8A
		public override bool CanGroup
		{
			get
			{
				return this.ProxiedView.CanGroup;
			}
		}

		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x060073F1 RID: 29681 RVA: 0x00212D97 File Offset: 0x00210F97
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this.ProxiedView.GroupDescriptions;
			}
		}

		// Token: 0x17001B91 RID: 7057
		// (get) Token: 0x060073F2 RID: 29682 RVA: 0x00212DA4 File Offset: 0x00210FA4
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				return this.ProxiedView.Groups;
			}
		}

		// Token: 0x060073F3 RID: 29683 RVA: 0x00212DB4 File Offset: 0x00210FB4
		public override void Refresh()
		{
			IndexedEnumerable indexedEnumerable = Interlocked.Exchange<IndexedEnumerable>(ref this._indexer, null);
			if (indexedEnumerable != null)
			{
				indexedEnumerable.Invalidate();
			}
			this.ProxiedView.Refresh();
		}

		// Token: 0x060073F4 RID: 29684 RVA: 0x00212DE2 File Offset: 0x00210FE2
		public override IDisposable DeferRefresh()
		{
			return this.ProxiedView.DeferRefresh();
		}

		// Token: 0x17001B92 RID: 7058
		// (get) Token: 0x060073F5 RID: 29685 RVA: 0x00212DEF File Offset: 0x00210FEF
		public override object CurrentItem
		{
			get
			{
				return this.ProxiedView.CurrentItem;
			}
		}

		// Token: 0x17001B93 RID: 7059
		// (get) Token: 0x060073F6 RID: 29686 RVA: 0x00212DFC File Offset: 0x00210FFC
		public override int CurrentPosition
		{
			get
			{
				return this.ProxiedView.CurrentPosition;
			}
		}

		// Token: 0x17001B94 RID: 7060
		// (get) Token: 0x060073F7 RID: 29687 RVA: 0x00212E09 File Offset: 0x00211009
		public override bool IsCurrentAfterLast
		{
			get
			{
				return this.ProxiedView.IsCurrentAfterLast;
			}
		}

		// Token: 0x17001B95 RID: 7061
		// (get) Token: 0x060073F8 RID: 29688 RVA: 0x00212E16 File Offset: 0x00211016
		public override bool IsCurrentBeforeFirst
		{
			get
			{
				return this.ProxiedView.IsCurrentBeforeFirst;
			}
		}

		// Token: 0x060073F9 RID: 29689 RVA: 0x00212E23 File Offset: 0x00211023
		public override bool MoveCurrentToFirst()
		{
			return this.ProxiedView.MoveCurrentToFirst();
		}

		// Token: 0x060073FA RID: 29690 RVA: 0x00212E30 File Offset: 0x00211030
		public override bool MoveCurrentToPrevious()
		{
			return this.ProxiedView.MoveCurrentToPrevious();
		}

		// Token: 0x060073FB RID: 29691 RVA: 0x00212E3D File Offset: 0x0021103D
		public override bool MoveCurrentToNext()
		{
			return this.ProxiedView.MoveCurrentToNext();
		}

		// Token: 0x060073FC RID: 29692 RVA: 0x00212E4A File Offset: 0x0021104A
		public override bool MoveCurrentToLast()
		{
			return this.ProxiedView.MoveCurrentToLast();
		}

		// Token: 0x060073FD RID: 29693 RVA: 0x00212E57 File Offset: 0x00211057
		public override bool MoveCurrentTo(object item)
		{
			return this.ProxiedView.MoveCurrentTo(item);
		}

		// Token: 0x060073FE RID: 29694 RVA: 0x00212E65 File Offset: 0x00211065
		public override bool MoveCurrentToPosition(int position)
		{
			return this.ProxiedView.MoveCurrentToPosition(position);
		}

		// Token: 0x14000157 RID: 343
		// (add) Token: 0x060073FF RID: 29695 RVA: 0x00212E73 File Offset: 0x00211073
		// (remove) Token: 0x06007400 RID: 29696 RVA: 0x00212E7C File Offset: 0x0021107C
		public override event CurrentChangingEventHandler CurrentChanging
		{
			add
			{
				this.PrivateCurrentChanging += value;
			}
			remove
			{
				this.PrivateCurrentChanging -= value;
			}
		}

		// Token: 0x14000158 RID: 344
		// (add) Token: 0x06007401 RID: 29697 RVA: 0x00212E85 File Offset: 0x00211085
		// (remove) Token: 0x06007402 RID: 29698 RVA: 0x00212E8E File Offset: 0x0021108E
		public override event EventHandler CurrentChanged
		{
			add
			{
				this.PrivateCurrentChanged += value;
			}
			remove
			{
				this.PrivateCurrentChanged -= value;
			}
		}

		// Token: 0x17001B96 RID: 7062
		// (get) Token: 0x06007403 RID: 29699 RVA: 0x00212E97 File Offset: 0x00211097
		public override int Count
		{
			get
			{
				return this.EnumerableWrapper.Count;
			}
		}

		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x06007404 RID: 29700 RVA: 0x00212EA4 File Offset: 0x002110A4
		public override bool IsEmpty
		{
			get
			{
				return this.ProxiedView.IsEmpty;
			}
		}

		// Token: 0x17001B98 RID: 7064
		// (get) Token: 0x06007405 RID: 29701 RVA: 0x00212EB1 File Offset: 0x002110B1
		public ICollectionView ProxiedView
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x06007406 RID: 29702 RVA: 0x00212EB9 File Offset: 0x002110B9
		public override int IndexOf(object item)
		{
			return this.EnumerableWrapper.IndexOf(item);
		}

		// Token: 0x06007407 RID: 29703 RVA: 0x00212EC7 File Offset: 0x002110C7
		public override bool PassesFilter(object item)
		{
			return !this.ProxiedView.CanFilter || this.ProxiedView.Filter == null || item == CollectionView.NewItemPlaceholder || item == ((IEditableCollectionView)this).CurrentAddItem || this.ProxiedView.Filter(item);
		}

		// Token: 0x06007408 RID: 29704 RVA: 0x00212F07 File Offset: 0x00211107
		public override object GetItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.EnumerableWrapper[index];
		}

		// Token: 0x06007409 RID: 29705 RVA: 0x00212F24 File Offset: 0x00211124
		public override void DetachFromSourceCollection()
		{
			if (this._view != null)
			{
				this._view.CollectionChanged -= this._OnViewChanged;
				this._view.CurrentChanging -= this._OnCurrentChanging;
				this._view.CurrentChanged -= this._OnCurrentChanged;
				INotifyPropertyChanged notifyPropertyChanged = this._view as INotifyPropertyChanged;
				if (notifyPropertyChanged != null)
				{
					notifyPropertyChanged.PropertyChanged -= this._OnPropertyChanged;
				}
				this._view = null;
			}
			base.DetachFromSourceCollection();
		}

		// Token: 0x17001B99 RID: 7065
		// (get) Token: 0x0600740A RID: 29706 RVA: 0x00212FAC File Offset: 0x002111AC
		// (set) Token: 0x0600740B RID: 29707 RVA: 0x00212FD0 File Offset: 0x002111D0
		NewItemPlaceholderPosition IEditableCollectionView.NewItemPlaceholderPosition
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.NewItemPlaceholderPosition;
				}
				return NewItemPlaceholderPosition.None;
			}
			set
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					editableCollectionView.NewItemPlaceholderPosition = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
				{
					"NewItemPlaceholderPosition"
				}));
			}
		}

		// Token: 0x17001B9A RID: 7066
		// (get) Token: 0x0600740C RID: 29708 RVA: 0x00213014 File Offset: 0x00211214
		bool IEditableCollectionView.CanAddNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanAddNew;
			}
		}

		// Token: 0x0600740D RID: 29709 RVA: 0x00213038 File Offset: 0x00211238
		object IEditableCollectionView.AddNew()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				return editableCollectionView.AddNew();
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNew"
			}));
		}

		// Token: 0x0600740E RID: 29710 RVA: 0x00213078 File Offset: 0x00211278
		void IEditableCollectionView.CommitNew()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CommitNew();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CommitNew"
			}));
		}

		// Token: 0x0600740F RID: 29711 RVA: 0x002130B8 File Offset: 0x002112B8
		void IEditableCollectionView.CancelNew()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CancelNew();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CancelNew"
			}));
		}

		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x06007410 RID: 29712 RVA: 0x002130F8 File Offset: 0x002112F8
		bool IEditableCollectionView.IsAddingNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsAddingNew;
			}
		}

		// Token: 0x17001B9C RID: 7068
		// (get) Token: 0x06007411 RID: 29713 RVA: 0x0021311C File Offset: 0x0021131C
		object IEditableCollectionView.CurrentAddItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentAddItem;
				}
				return null;
			}
		}

		// Token: 0x17001B9D RID: 7069
		// (get) Token: 0x06007412 RID: 29714 RVA: 0x00213140 File Offset: 0x00211340
		bool IEditableCollectionView.CanRemove
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanRemove;
			}
		}

		// Token: 0x06007413 RID: 29715 RVA: 0x00213164 File Offset: 0x00211364
		void IEditableCollectionView.RemoveAt(int index)
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.RemoveAt(index);
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"RemoveAt"
			}));
		}

		// Token: 0x06007414 RID: 29716 RVA: 0x002131A8 File Offset: 0x002113A8
		void IEditableCollectionView.Remove(object item)
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.Remove(item);
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"Remove"
			}));
		}

		// Token: 0x06007415 RID: 29717 RVA: 0x002131EC File Offset: 0x002113EC
		void IEditableCollectionView.EditItem(object item)
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.EditItem(item);
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"EditItem"
			}));
		}

		// Token: 0x06007416 RID: 29718 RVA: 0x00213230 File Offset: 0x00211430
		void IEditableCollectionView.CommitEdit()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CommitEdit();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CommitEdit"
			}));
		}

		// Token: 0x06007417 RID: 29719 RVA: 0x00213270 File Offset: 0x00211470
		void IEditableCollectionView.CancelEdit()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CancelEdit();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CancelEdit"
			}));
		}

		// Token: 0x17001B9E RID: 7070
		// (get) Token: 0x06007418 RID: 29720 RVA: 0x002132B0 File Offset: 0x002114B0
		bool IEditableCollectionView.CanCancelEdit
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanCancelEdit;
			}
		}

		// Token: 0x17001B9F RID: 7071
		// (get) Token: 0x06007419 RID: 29721 RVA: 0x002132D4 File Offset: 0x002114D4
		bool IEditableCollectionView.IsEditingItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsEditingItem;
			}
		}

		// Token: 0x17001BA0 RID: 7072
		// (get) Token: 0x0600741A RID: 29722 RVA: 0x002132F8 File Offset: 0x002114F8
		object IEditableCollectionView.CurrentEditItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentEditItem;
				}
				return null;
			}
		}

		// Token: 0x17001BA1 RID: 7073
		// (get) Token: 0x0600741B RID: 29723 RVA: 0x0021331C File Offset: 0x0021151C
		bool IEditableCollectionViewAddNewItem.CanAddNewItem
		{
			get
			{
				IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this.ProxiedView as IEditableCollectionViewAddNewItem;
				return editableCollectionViewAddNewItem != null && editableCollectionViewAddNewItem.CanAddNewItem;
			}
		}

		// Token: 0x0600741C RID: 29724 RVA: 0x00213340 File Offset: 0x00211540
		object IEditableCollectionViewAddNewItem.AddNewItem(object newItem)
		{
			IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this.ProxiedView as IEditableCollectionViewAddNewItem;
			if (editableCollectionViewAddNewItem != null)
			{
				return editableCollectionViewAddNewItem.AddNewItem(newItem);
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNewItem"
			}));
		}

		// Token: 0x17001BA2 RID: 7074
		// (get) Token: 0x0600741D RID: 29725 RVA: 0x00213384 File Offset: 0x00211584
		bool ICollectionViewLiveShaping.CanChangeLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveSorting;
			}
		}

		// Token: 0x17001BA3 RID: 7075
		// (get) Token: 0x0600741E RID: 29726 RVA: 0x002133A8 File Offset: 0x002115A8
		bool ICollectionViewLiveShaping.CanChangeLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveFiltering;
			}
		}

		// Token: 0x17001BA4 RID: 7076
		// (get) Token: 0x0600741F RID: 29727 RVA: 0x002133CC File Offset: 0x002115CC
		bool ICollectionViewLiveShaping.CanChangeLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveGrouping;
			}
		}

		// Token: 0x17001BA5 RID: 7077
		// (get) Token: 0x06007420 RID: 29728 RVA: 0x002133F0 File Offset: 0x002115F0
		// (set) Token: 0x06007421 RID: 29729 RVA: 0x0021341C File Offset: 0x0021161C
		bool? ICollectionViewLiveShaping.IsLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveSorting;
			}
			set
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					collectionViewLiveShaping.IsLiveSorting = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveSorting",
					"CanChangeLiveSorting"
				}));
			}
		}

		// Token: 0x17001BA6 RID: 7078
		// (get) Token: 0x06007422 RID: 29730 RVA: 0x00213468 File Offset: 0x00211668
		// (set) Token: 0x06007423 RID: 29731 RVA: 0x00213494 File Offset: 0x00211694
		bool? ICollectionViewLiveShaping.IsLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveFiltering;
			}
			set
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					collectionViewLiveShaping.IsLiveFiltering = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveFiltering",
					"CanChangeLiveFiltering"
				}));
			}
		}

		// Token: 0x17001BA7 RID: 7079
		// (get) Token: 0x06007424 RID: 29732 RVA: 0x002134E0 File Offset: 0x002116E0
		// (set) Token: 0x06007425 RID: 29733 RVA: 0x0021350C File Offset: 0x0021170C
		bool? ICollectionViewLiveShaping.IsLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveGrouping;
			}
			set
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					collectionViewLiveShaping.IsLiveGrouping = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveGrouping",
					"CanChangeLiveGrouping"
				}));
			}
		}

		// Token: 0x17001BA8 RID: 7080
		// (get) Token: 0x06007426 RID: 29734 RVA: 0x00213558 File Offset: 0x00211758
		ObservableCollection<string> ICollectionViewLiveShaping.LiveSortingProperties
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					return collectionViewLiveShaping.LiveSortingProperties;
				}
				if (this._liveSortingProperties == null)
				{
					this._liveSortingProperties = new ObservableCollection<string>();
				}
				return this._liveSortingProperties;
			}
		}

		// Token: 0x17001BA9 RID: 7081
		// (get) Token: 0x06007427 RID: 29735 RVA: 0x00213594 File Offset: 0x00211794
		ObservableCollection<string> ICollectionViewLiveShaping.LiveFilteringProperties
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					return collectionViewLiveShaping.LiveFilteringProperties;
				}
				if (this._liveFilteringProperties == null)
				{
					this._liveFilteringProperties = new ObservableCollection<string>();
				}
				return this._liveFilteringProperties;
			}
		}

		// Token: 0x17001BAA RID: 7082
		// (get) Token: 0x06007428 RID: 29736 RVA: 0x002135D0 File Offset: 0x002117D0
		ObservableCollection<string> ICollectionViewLiveShaping.LiveGroupingProperties
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					return collectionViewLiveShaping.LiveGroupingProperties;
				}
				if (this._liveGroupingProperties == null)
				{
					this._liveGroupingProperties = new ObservableCollection<string>();
				}
				return this._liveGroupingProperties;
			}
		}

		// Token: 0x17001BAB RID: 7083
		// (get) Token: 0x06007429 RID: 29737 RVA: 0x0021360C File Offset: 0x0021180C
		ReadOnlyCollection<ItemPropertyInfo> IItemProperties.ItemProperties
		{
			get
			{
				IItemProperties itemProperties = this.ProxiedView as IItemProperties;
				if (itemProperties != null)
				{
					return itemProperties.ItemProperties;
				}
				return null;
			}
		}

		// Token: 0x0600742A RID: 29738 RVA: 0x00213630 File Offset: 0x00211830
		protected override IEnumerator GetEnumerator()
		{
			return this.ProxiedView.GetEnumerator();
		}

		// Token: 0x0600742B RID: 29739 RVA: 0x00213640 File Offset: 0x00211840
		internal override void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			if (this._view != null)
			{
				format(level + 1, this._view, new bool?(true), sources);
				object sourceCollection = this._view.SourceCollection;
				if (sourceCollection != null)
				{
					format(level + 2, sourceCollection, null, sources);
				}
			}
		}

		// Token: 0x0600742C RID: 29740 RVA: 0x001653AB File Offset: 0x001635AB
		private void _OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			this.OnPropertyChanged(args);
		}

		// Token: 0x0600742D RID: 29741 RVA: 0x0021369D File Offset: 0x0021189D
		private void _OnViewChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			this.OnCollectionChanged(args);
		}

		// Token: 0x0600742E RID: 29742 RVA: 0x002136A6 File Offset: 0x002118A6
		private void _OnCurrentChanging(object sender, CurrentChangingEventArgs args)
		{
			if (this.PrivateCurrentChanging != null)
			{
				this.PrivateCurrentChanging(this, args);
			}
		}

		// Token: 0x0600742F RID: 29743 RVA: 0x002136BD File Offset: 0x002118BD
		private void _OnCurrentChanged(object sender, EventArgs args)
		{
			if (this.PrivateCurrentChanged != null)
			{
				this.PrivateCurrentChanged(this, args);
			}
		}

		// Token: 0x17001BAC RID: 7084
		// (get) Token: 0x06007430 RID: 29744 RVA: 0x002136D4 File Offset: 0x002118D4
		private IndexedEnumerable EnumerableWrapper
		{
			get
			{
				if (this._indexer == null)
				{
					IndexedEnumerable value = new IndexedEnumerable(this.ProxiedView, new Predicate<object>(this.PassesFilter));
					Interlocked.CompareExchange<IndexedEnumerable>(ref this._indexer, value, null);
				}
				return this._indexer;
			}
		}

		// Token: 0x14000159 RID: 345
		// (add) Token: 0x06007431 RID: 29745 RVA: 0x00213718 File Offset: 0x00211918
		// (remove) Token: 0x06007432 RID: 29746 RVA: 0x00213750 File Offset: 0x00211950
		private event CurrentChangingEventHandler PrivateCurrentChanging;

		// Token: 0x1400015A RID: 346
		// (add) Token: 0x06007433 RID: 29747 RVA: 0x00213788 File Offset: 0x00211988
		// (remove) Token: 0x06007434 RID: 29748 RVA: 0x002137C0 File Offset: 0x002119C0
		private event EventHandler PrivateCurrentChanged;

		// Token: 0x040037C3 RID: 14275
		private ICollectionView _view;

		// Token: 0x040037C4 RID: 14276
		private IndexedEnumerable _indexer;

		// Token: 0x040037C7 RID: 14279
		private ObservableCollection<string> _liveSortingProperties;

		// Token: 0x040037C8 RID: 14280
		private ObservableCollection<string> _liveFilteringProperties;

		// Token: 0x040037C9 RID: 14281
		private ObservableCollection<string> _liveGroupingProperties;
	}
}
