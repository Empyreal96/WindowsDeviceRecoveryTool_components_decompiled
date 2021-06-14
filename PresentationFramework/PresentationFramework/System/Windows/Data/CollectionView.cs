using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Hashing.PresentationFramework;

namespace System.Windows.Data
{
	/// <summary>Represents a view for grouping, sorting, filtering, and navigating a data collection.</summary>
	// Token: 0x020001A7 RID: 423
	public class CollectionView : DispatcherObject, ICollectionView, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.CollectionView" /> class that represents a view of the specified collection. </summary>
		/// <param name="collection">The underlying collection.</param>
		// Token: 0x06001A6B RID: 6763 RVA: 0x0007DACD File Offset: 0x0007BCCD
		public CollectionView(IEnumerable collection) : this(collection, 0)
		{
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0007DAD8 File Offset: 0x0007BCD8
		internal CollectionView(IEnumerable collection, int moveToFirst)
		{
			CollectionView.<>c__DisplayClass1_0 CS$<>8__locals1 = new CollectionView.<>c__DisplayClass1_0();
			CS$<>8__locals1.collection = collection;
			base..ctor();
			if (CS$<>8__locals1.collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (base.GetType() == typeof(CollectionView) && TraceData.IsEnabled)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.CollectionViewIsUnsupported);
			}
			this._engine = DataBindEngine.CurrentDataBindEngine;
			if (!this._engine.IsShutDown)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.AllowsCrossThreadChanges, this._engine.ViewManager.GetSynchronizationInfo(CS$<>8__locals1.collection).IsSynchronized);
			}
			else
			{
				moveToFirst = -1;
			}
			this._sourceCollection = CS$<>8__locals1.collection;
			INotifyCollectionChanged notifyCollectionChanged = CS$<>8__locals1.collection as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				IBindingList bindingList;
				if (!(this is BindingListCollectionView) || ((bindingList = (CS$<>8__locals1.collection as IBindingList)) != null && !bindingList.SupportsChangeNotification))
				{
					notifyCollectionChanged.CollectionChanged += this.OnCollectionChanged;
				}
				this.SetFlag(CollectionView.CollectionViewFlags.IsDynamic, true);
			}
			object currentItem = null;
			int currentPosition = -1;
			if (moveToFirst >= 0)
			{
				BindingOperations.AccessCollection(CS$<>8__locals1.collection, delegate
				{
					IEnumerator enumerator = CS$<>8__locals1.collection.GetEnumerator();
					if (enumerator.MoveNext())
					{
						currentItem = enumerator.Current;
						currentPosition = 0;
					}
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}, false);
			}
			this._currentItem = currentItem;
			this._currentPosition = currentPosition;
			this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, this._currentPosition < 0);
			this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, this._currentPosition < 0);
			this.SetFlag(CollectionView.CollectionViewFlags.CachedIsEmpty, this._currentPosition < 0);
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0007DCAA File Offset: 0x0007BEAA
		internal CollectionView(IEnumerable collection, bool shouldProcessCollectionChanged) : this(collection)
		{
			this.SetFlag(CollectionView.CollectionViewFlags.ShouldProcessCollectionChanged, shouldProcessCollectionChanged);
		}

		/// <summary>Gets or sets the culture information to use during sorting.</summary>
		/// <returns>The culture information to use during sorting.</returns>
		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001A6E RID: 6766 RVA: 0x0007DCBB File Offset: 0x0007BEBB
		// (set) Token: 0x06001A6F RID: 6767 RVA: 0x0007DCC3 File Offset: 0x0007BEC3
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public virtual CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._culture != value)
				{
					this._culture = value;
					this.OnPropertyChanged("Culture");
				}
			}
		}

		/// <summary>Returns a value that indicates whether the specified item belongs to the view.</summary>
		/// <param name="item">The object to check.</param>
		/// <returns>
		///     <see langword="true" /> if the item belongs to the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A70 RID: 6768 RVA: 0x0007DCEE File Offset: 0x0007BEEE
		public virtual bool Contains(object item)
		{
			this.VerifyRefreshNotDeferred();
			return this.IndexOf(item) >= 0;
		}

		/// <summary>Returns the underlying unfiltered collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerable" /> object that is the underlying collection.</returns>
		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x0007DD03 File Offset: 0x0007BF03
		public virtual IEnumerable SourceCollection
		{
			get
			{
				return this._sourceCollection;
			}
		}

		/// <summary>Gets or sets a method used to determine if an item is suitable for inclusion in the view.</summary>
		/// <returns>A delegate that represents the method used to determine if an item is suitable for inclusion in the view.</returns>
		/// <exception cref="T:System.NotSupportedException">The current implementation does not support filtering. </exception>
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x0007DD0B File Offset: 0x0007BF0B
		// (set) Token: 0x06001A73 RID: 6771 RVA: 0x0007DD13 File Offset: 0x0007BF13
		public virtual Predicate<object> Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				if (!this.CanFilter)
				{
					throw new NotSupportedException();
				}
				this._filter = value;
				this.RefreshOrDefer();
			}
		}

		/// <summary>Gets a value that indicates whether the view supports filtering.</summary>
		/// <returns>
		///     <see langword="true" /> if the view supports filtering; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001A74 RID: 6772 RVA: 0x00016748 File Offset: 0x00014948
		public virtual bool CanFilter
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.ComponentModel.SortDescription" /> structures that describes how the items in the collection are sorted in the view.</summary>
		/// <returns>An empty <see cref="T:System.ComponentModel.SortDescriptionCollection" /> in all cases.</returns>
		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001A75 RID: 6773 RVA: 0x0007DD30 File Offset: 0x0007BF30
		public virtual SortDescriptionCollection SortDescriptions
		{
			get
			{
				return SortDescriptionCollection.Empty;
			}
		}

		/// <summary>Gets a value that indicates whether the view supports sorting.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001A76 RID: 6774 RVA: 0x0000B02A File Offset: 0x0000922A
		public virtual bool CanSort
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value that indicates whether the view supports grouping.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x0000B02A File Offset: 0x0000922A
		public virtual bool CanGroup
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.ComponentModel.GroupDescription" /> objects that describes how the items in the collection are grouped in the view.</summary>
		/// <returns>
		///     <see langword="null" /> in all cases.</returns>
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001A78 RID: 6776 RVA: 0x0000C238 File Offset: 0x0000A438
		public virtual ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets a collection of the top-level groups that is constructed based on the <see cref="P:System.Windows.Data.CollectionView.GroupDescriptions" /> property.</summary>
		/// <returns>
		///     <see langword="null" /> in all cases.</returns>
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x0000C238 File Offset: 0x0000A438
		public virtual ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				return null;
			}
		}

		/// <summary>Re-creates the view.</summary>
		// Token: 0x06001A7A RID: 6778 RVA: 0x0007DD38 File Offset: 0x0007BF38
		public virtual void Refresh()
		{
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && (editableCollectionView.IsAddingNew || editableCollectionView.IsEditingItem))
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Refresh"
				}));
			}
			this.RefreshInternal();
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0007DD83 File Offset: 0x0007BF83
		internal void RefreshInternal()
		{
			if (this.AllowsCrossThreadChanges)
			{
				base.VerifyAccess();
			}
			this.RefreshOverride();
			this.SetFlag(CollectionView.CollectionViewFlags.NeedsRefresh, false);
		}

		/// <summary>Enters a defer cycle that you can use to merge changes to the view and delay automatic refresh.</summary>
		/// <returns>An <see cref="T:System.IDisposable" /> object that you can use to dispose of the calling object.</returns>
		// Token: 0x06001A7C RID: 6780 RVA: 0x0007DDA8 File Offset: 0x0007BFA8
		public virtual IDisposable DeferRefresh()
		{
			if (this.AllowsCrossThreadChanges)
			{
				base.VerifyAccess();
			}
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && (editableCollectionView.IsAddingNew || editableCollectionView.IsEditingItem))
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"DeferRefresh"
				}));
			}
			this._deferLevel++;
			return new CollectionView.DeferHelper(this);
		}

		/// <summary>Gets the current item in the view.</summary>
		/// <returns>The current item of the view. By default, the first item of the collection starts as the current item.</returns>
		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x0007DE0F File Offset: 0x0007C00F
		public virtual object CurrentItem
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this._currentItem;
			}
		}

		/// <summary>Gets the ordinal position of the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> within the (optionally sorted and filtered) view.</summary>
		/// <returns>The ordinal position of the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> within the (optionally sorted and filtered) view.</returns>
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001A7E RID: 6782 RVA: 0x0007DE1D File Offset: 0x0007C01D
		public virtual int CurrentPosition
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this._currentPosition;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> of the view is beyond the end of the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> of the view is beyond the end of the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001A7F RID: 6783 RVA: 0x0007DE2B File Offset: 0x0007C02B
		public virtual bool IsCurrentAfterLast
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> of the view is before the beginning of the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> of the view is before the beginning of the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x0007DE3B File Offset: 0x0007C03B
		public virtual bool IsCurrentBeforeFirst
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst);
			}
		}

		/// <summary>Sets the first item in the view as the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the resulting <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A81 RID: 6785 RVA: 0x0007DE4C File Offset: 0x0007C04C
		public virtual bool MoveCurrentToFirst()
		{
			this.VerifyRefreshNotDeferred();
			int position = 0;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				position = 1;
			}
			return this.MoveCurrentToPosition(position);
		}

		/// <summary>Sets the last item in the view as the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the resulting <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A82 RID: 6786 RVA: 0x0007DE80 File Offset: 0x0007C080
		public virtual bool MoveCurrentToLast()
		{
			this.VerifyRefreshNotDeferred();
			int num = this.Count - 1;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				num--;
			}
			return this.MoveCurrentToPosition(num);
		}

		/// <summary>Sets the item after the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> in the view as the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the resulting <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A83 RID: 6787 RVA: 0x0007DEBC File Offset: 0x0007C0BC
		public virtual bool MoveCurrentToNext()
		{
			this.VerifyRefreshNotDeferred();
			int num = this.CurrentPosition + 1;
			int count = this.Count;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && num == 0 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				num = 1;
			}
			if (editableCollectionView != null && num == count - 1 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				num = count;
			}
			return num <= count && this.MoveCurrentToPosition(num);
		}

		/// <summary>Sets the item before the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> in the view as the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the resulting <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A84 RID: 6788 RVA: 0x0007DF18 File Offset: 0x0007C118
		public virtual bool MoveCurrentToPrevious()
		{
			this.VerifyRefreshNotDeferred();
			int num = this.CurrentPosition - 1;
			int count = this.Count;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && num == count - 1 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				num = count - 2;
			}
			if (editableCollectionView != null && num == 0 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				num = -1;
			}
			return num >= -1 && this.MoveCurrentToPosition(num);
		}

		/// <summary>Sets the specified item to be the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> in the view.</summary>
		/// <param name="item">The item to set as the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" />.</param>
		/// <returns>
		///     <see langword="true" /> if the resulting <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A85 RID: 6789 RVA: 0x0007DF78 File Offset: 0x0007C178
		public virtual bool MoveCurrentTo(object item)
		{
			this.VerifyRefreshNotDeferred();
			if ((ItemsControl.EqualsEx(this.CurrentItem, item) || ItemsControl.EqualsEx(CollectionView.NewItemPlaceholder, item)) && (item != null || this.IsCurrentInView))
			{
				return this.IsCurrentInView;
			}
			int position = -1;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if ((editableCollectionView != null && editableCollectionView.IsAddingNew && ItemsControl.EqualsEx(item, editableCollectionView.CurrentAddItem)) || this.PassesFilter(item))
			{
				position = this.IndexOf(item);
			}
			return this.MoveCurrentToPosition(position);
		}

		/// <summary>Sets the item at the specified index to be the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> in the view.</summary>
		/// <param name="position">The index to set the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> to.</param>
		/// <returns>
		///     <see langword="true" /> if the resulting <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A86 RID: 6790 RVA: 0x0007DFF8 File Offset: 0x0007C1F8
		public virtual bool MoveCurrentToPosition(int position)
		{
			this.VerifyRefreshNotDeferred();
			if (position < -1 || position > this.Count)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && ((position == 0 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) || (position == this.Count - 1 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)))
			{
				return this.IsCurrentInView;
			}
			if ((position != this.CurrentPosition || !this.IsCurrentInSync) && this.OKToChangeCurrent())
			{
				bool isCurrentAfterLast = this.IsCurrentAfterLast;
				bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
				this._MoveCurrentToPosition(position);
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
			return this.IsCurrentInView;
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is changing.</summary>
		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06001A87 RID: 6791 RVA: 0x0007E0CC File Offset: 0x0007C2CC
		// (remove) Token: 0x06001A88 RID: 6792 RVA: 0x0007E104 File Offset: 0x0007C304
		public virtual event CurrentChangingEventHandler CurrentChanging;

		/// <summary>Occurs after the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> has changed.</summary>
		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06001A89 RID: 6793 RVA: 0x0007E13C File Offset: 0x0007C33C
		// (remove) Token: 0x06001A8A RID: 6794 RVA: 0x0007E174 File Offset: 0x0007C374
		public virtual event EventHandler CurrentChanged;

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> object that you can use to enumerate the items in the view.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that you can use to enumerate the items in the view.</returns>
		// Token: 0x06001A8B RID: 6795 RVA: 0x0007E1A9 File Offset: 0x0007C3A9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Returns a value that indicates whether the specified item in the underlying collection belongs to the view.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		///     <see langword="true" /> if the specified item belongs to the view or if there is not filter set on the collection view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A8C RID: 6796 RVA: 0x0007E1B1 File Offset: 0x0007C3B1
		public virtual bool PassesFilter(object item)
		{
			return !this.CanFilter || this.Filter == null || this.Filter(item);
		}

		/// <summary>Returns the index at which the specified item is located.</summary>
		/// <param name="item">The item to locate.</param>
		/// <returns>The index at which the specified item is located, or –1 if the item is unknown.</returns>
		// Token: 0x06001A8D RID: 6797 RVA: 0x0007E1D1 File Offset: 0x0007C3D1
		public virtual int IndexOf(object item)
		{
			this.VerifyRefreshNotDeferred();
			return this.EnumerableWrapper.IndexOf(item);
		}

		/// <summary>Retrieves the item at the specified zero-based index in the view.</summary>
		/// <param name="index">The zero-based index of the item to retrieve.</param>
		/// <returns>The item at the specified zero-based index in the view.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0. </exception>
		// Token: 0x06001A8E RID: 6798 RVA: 0x0007E1E5 File Offset: 0x0007C3E5
		public virtual object GetItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.EnumerableWrapper[index];
		}

		/// <summary>Removes the reference to the underlying collection from the <see cref="T:System.Windows.Data.CollectionView" />.</summary>
		// Token: 0x06001A8F RID: 6799 RVA: 0x0007E204 File Offset: 0x0007C404
		public virtual void DetachFromSourceCollection()
		{
			INotifyCollectionChanged notifyCollectionChanged = this._sourceCollection as INotifyCollectionChanged;
			IBindingList bindingList;
			if (notifyCollectionChanged != null && (!(this is BindingListCollectionView) || ((bindingList = (this._sourceCollection as IBindingList)) != null && !bindingList.SupportsChangeNotification)))
			{
				notifyCollectionChanged.CollectionChanged -= this.OnCollectionChanged;
			}
			this._sourceCollection = null;
		}

		/// <summary>Gets the number of records in the view.</summary>
		/// <returns>The number of records in the view, or –1 if the number of records is unknown.</returns>
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x0007E258 File Offset: 0x0007C458
		public virtual int Count
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this.EnumerableWrapper.Count;
			}
		}

		/// <summary>Gets a value that indicates whether the resulting (filtered) view is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if the resulting view is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x0007E26B File Offset: 0x0007C46B
		public virtual bool IsEmpty
		{
			get
			{
				return this.EnumerableWrapper.IsEmpty;
			}
		}

		/// <summary>Returns an object that you can use to compare items in the view.</summary>
		/// <returns>An <see cref="T:System.Collections.IComparer" /> object that you can use to compare items in the view.</returns>
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x0007E278 File Offset: 0x0007C478
		public virtual IComparer Comparer
		{
			get
			{
				return this as IComparer;
			}
		}

		/// <summary>Gets a value that indicates whether the view needs to be refreshed.</summary>
		/// <returns>
		///     <see langword="true" /> if the view needs to be refreshed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x0007E280 File Offset: 0x0007C480
		public virtual bool NeedsRefresh
		{
			get
			{
				return this.CheckFlag(CollectionView.CollectionViewFlags.NeedsRefresh);
			}
		}

		/// <summary>Gets a value that indicates whether any object is subscribing to the events of this <see cref="T:System.Windows.Data.CollectionView" />.</summary>
		/// <returns>
		///     <see langword="true" /> if any object is subscribing to the events of this <see cref="T:System.Windows.Data.CollectionView" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001A94 RID: 6804 RVA: 0x0007E28D File Offset: 0x0007C48D
		public virtual bool IsInUse
		{
			get
			{
				return this.CollectionChanged != null || this.PropertyChanged != null || this.CurrentChanged != null || this.CurrentChanging != null;
			}
		}

		/// <summary>Gets the object that is in the collection to represent a new item.</summary>
		/// <returns>The object that is in the collection to represent a new item.</returns>
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x0007E2B2 File Offset: 0x0007C4B2
		public static object NewItemPlaceholder
		{
			get
			{
				return CollectionView._newItemPlaceholder;
			}
		}

		/// <summary>Occurs when the view has changed.</summary>
		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06001A96 RID: 6806 RVA: 0x0007E2BC File Offset: 0x0007C4BC
		// (remove) Token: 0x06001A97 RID: 6807 RVA: 0x0007E2F4 File Offset: 0x0007C4F4
		protected virtual event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>Occurs when the view has changed.</summary>
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06001A98 RID: 6808 RVA: 0x0007E329 File Offset: 0x0007C529
		// (remove) Token: 0x06001A99 RID: 6809 RVA: 0x0007E332 File Offset: 0x0007C532
		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			add
			{
				this.CollectionChanged += value;
			}
			remove
			{
				this.CollectionChanged -= value;
			}
		}

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06001A9A RID: 6810 RVA: 0x0007E33B File Offset: 0x0007C53B
		// (remove) Token: 0x06001A9B RID: 6811 RVA: 0x0007E344 File Offset: 0x0007C544
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this.PropertyChanged += value;
			}
			remove
			{
				this.PropertyChanged -= value;
			}
		}

		/// <summary>Raises the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event using the specified arguments.</summary>
		/// <param name="e">Arguments of the event being raised.</param>
		// Token: 0x06001A9C RID: 6812 RVA: 0x0007E34D File Offset: 0x0007C54D
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		/// <summary>Occurs when a property value has changed.</summary>
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06001A9D RID: 6813 RVA: 0x0007E364 File Offset: 0x0007C564
		// (remove) Token: 0x06001A9E RID: 6814 RVA: 0x0007E39C File Offset: 0x0007C59C
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Re-creates the view.</summary>
		// Token: 0x06001A9F RID: 6815 RVA: 0x0007E3D4 File Offset: 0x0007C5D4
		protected virtual void RefreshOverride()
		{
			if (this.SortDescriptions.Count > 0)
			{
				throw new InvalidOperationException(SR.Get("ImplementOtherMembersWithSort", new object[]
				{
					"Refresh()"
				}));
			}
			object currentItem = this._currentItem;
			bool flag = this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast);
			bool flag2 = this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst);
			int currentPosition = this._currentPosition;
			this.OnCurrentChanging();
			this.InvalidateEnumerableWrapper();
			if (this.IsEmpty || flag2)
			{
				this._MoveCurrentToPosition(-1);
			}
			else if (flag)
			{
				this._MoveCurrentToPosition(this.Count);
			}
			else if (currentItem != null)
			{
				int num = this.EnumerableWrapper.IndexOf(currentItem);
				if (num < 0)
				{
					num = 0;
				}
				this._MoveCurrentToPosition(num);
			}
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			this.OnCurrentChanged();
			if (this.IsCurrentAfterLast != flag)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst != flag2)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (currentPosition != this.CurrentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (currentItem != this.CurrentItem)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		/// <summary>Returns an object that you can use to enumerate the items in the view.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that you can use to enumerate the items in the view.</returns>
		// Token: 0x06001AA0 RID: 6816 RVA: 0x0007E4DF File Offset: 0x0007C6DF
		protected virtual IEnumerator GetEnumerator()
		{
			this.VerifyRefreshNotDeferred();
			if (this.SortDescriptions.Count > 0)
			{
				throw new InvalidOperationException(SR.Get("ImplementOtherMembersWithSort", new object[]
				{
					"GetEnumerator()"
				}));
			}
			return this.EnumerableWrapper.GetEnumerator();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Data.CollectionView.CollectionChanged" /> event. </summary>
		/// <param name="args">The <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> object to pass to the event handler.</param>
		// Token: 0x06001AA1 RID: 6817 RVA: 0x0007E520 File Offset: 0x0007C720
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			this._timestamp++;
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
			if (args.Action != NotifyCollectionChangedAction.Replace && args.Action != NotifyCollectionChangedAction.Move)
			{
				this.OnPropertyChanged("Count");
			}
			bool isEmpty = this.IsEmpty;
			if (isEmpty != this.CheckFlag(CollectionView.CollectionViewFlags.CachedIsEmpty))
			{
				this.SetFlag(CollectionView.CollectionViewFlags.CachedIsEmpty, isEmpty);
				this.OnPropertyChanged("IsEmpty");
			}
		}

		/// <summary>Sets the specified item and index as the values of the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> and <see cref="P:System.Windows.Data.CollectionView.CurrentPosition" /> properties.</summary>
		/// <param name="newItem">The item to set as the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" />.</param>
		/// <param name="newPosition">The value to set as the <see cref="P:System.Windows.Data.CollectionView.CurrentPosition" /> property value.</param>
		// Token: 0x06001AA2 RID: 6818 RVA: 0x0007E5A8 File Offset: 0x0007C7A8
		protected void SetCurrent(object newItem, int newPosition)
		{
			int count = (newItem != null) ? 0 : (this.IsEmpty ? 0 : this.Count);
			this.SetCurrent(newItem, newPosition, count);
		}

		/// <summary>Sets the specified item and index as the values of the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> and <see cref="P:System.Windows.Data.CollectionView.CurrentPosition" /> properties. This method can be called from a constructor of a derived class.</summary>
		/// <param name="newItem">The item to set as the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" />.</param>
		/// <param name="newPosition">The value to set as the <see cref="P:System.Windows.Data.CollectionView.CurrentPosition" /> property value.</param>
		/// <param name="count">The number of items in the <see cref="T:System.Windows.Data.CollectionView" />. </param>
		// Token: 0x06001AA3 RID: 6819 RVA: 0x0007E5D8 File Offset: 0x0007C7D8
		protected void SetCurrent(object newItem, int newPosition, int count)
		{
			if (newItem != null)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, false);
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, false);
			}
			else if (count == 0)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, true);
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, true);
				newPosition = -1;
			}
			else
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, newPosition < 0);
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, newPosition >= count);
			}
			this._currentItem = newItem;
			this._currentPosition = newPosition;
		}

		/// <summary>Returns a value that indicates whether the view can change which item is the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" />.</summary>
		/// <returns>
		///     <see langword="false" /> if a listener cancels the change; otherwise, <see langword="true" />.</returns>
		// Token: 0x06001AA4 RID: 6820 RVA: 0x0007E63C File Offset: 0x0007C83C
		protected bool OKToChangeCurrent()
		{
			CurrentChangingEventArgs currentChangingEventArgs = new CurrentChangingEventArgs();
			this.OnCurrentChanging(currentChangingEventArgs);
			return !currentChangingEventArgs.Cancel;
		}

		/// <summary>Raises a <see cref="E:System.Windows.Data.CollectionView.CurrentChanging" /> event that is not cancelable.</summary>
		// Token: 0x06001AA5 RID: 6821 RVA: 0x0007E65F File Offset: 0x0007C85F
		protected void OnCurrentChanging()
		{
			this._currentPosition = -1;
			this.OnCurrentChanging(CollectionView.uncancelableCurrentChangingEventArgs);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Data.CollectionView.CurrentChanging" /> event with the specified arguments.</summary>
		/// <param name="args">Information about the event.</param>
		// Token: 0x06001AA6 RID: 6822 RVA: 0x0007E674 File Offset: 0x0007C874
		protected virtual void OnCurrentChanging(CurrentChangingEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (this._currentChangedMonitor.Busy)
			{
				if (args.IsCancelable)
				{
					args.Cancel = true;
				}
				return;
			}
			if (this.CurrentChanging != null)
			{
				this.CurrentChanging(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Data.CollectionView.CurrentChanged" /> event.</summary>
		// Token: 0x06001AA7 RID: 6823 RVA: 0x0007E6C4 File Offset: 0x0007C8C4
		protected virtual void OnCurrentChanged()
		{
			if (this.CurrentChanged != null && this._currentChangedMonitor.Enter())
			{
				using (this._currentChangedMonitor)
				{
					this.CurrentChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>When overridden in a derived class, processes a single change on the UI thread.</summary>
		/// <param name="args">The <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> object to process.</param>
		// Token: 0x06001AA8 RID: 6824 RVA: 0x0007E71C File Offset: 0x0007C91C
		protected virtual void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			this.ValidateCollectionChangedEventArgs(args);
			object currentItem = this._currentItem;
			bool flag = this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast);
			bool flag2 = this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst);
			int currentPosition = this._currentPosition;
			bool flag3 = false;
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (this.PassesFilter(args.NewItems[0]))
				{
					flag3 = true;
					this.AdjustCurrencyForAdd(args.NewStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (this.PassesFilter(args.OldItems[0]))
				{
					flag3 = true;
					this.AdjustCurrencyForRemove(args.OldStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (this.PassesFilter(args.OldItems[0]) || this.PassesFilter(args.NewItems[0]))
				{
					flag3 = true;
					this.AdjustCurrencyForReplace(args.OldStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (this.PassesFilter(args.NewItems[0]))
				{
					flag3 = true;
					this.AdjustCurrencyForMove(args.OldStartingIndex, args.NewStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				this.RefreshOrDefer();
				return;
			}
			if (flag3)
			{
				this.OnCollectionChanged(args);
			}
			if (this._currentElementWasRemovedOrReplaced)
			{
				this.MoveCurrencyOffDeletedElement();
				this._currentElementWasRemovedOrReplaced = false;
			}
			if (this.IsCurrentAfterLast != flag)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst != flag2)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (this._currentPosition != currentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (this._currentItem != currentItem)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Data.CollectionView.CollectionChanged" /> event.</summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="args">The <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> object to pass to the event handler.</param>
		// Token: 0x06001AA9 RID: 6825 RVA: 0x0007E8A5 File Offset: 0x0007CAA5
		protected void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (this.CheckFlag(CollectionView.CollectionViewFlags.ShouldProcessCollectionChanged))
			{
				if (!this.AllowsCrossThreadChanges)
				{
					if (!base.CheckAccess())
					{
						throw new NotSupportedException(SR.Get("MultiThreadedCollectionChangeNotSupported"));
					}
					this.ProcessCollectionChanged(args);
					return;
				}
				else
				{
					this.PostChange(args);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Data.CollectionView.AllowsCrossThreadChanges" /> property changes.</summary>
		// Token: 0x06001AAA RID: 6826 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnAllowsCrossThreadChangesChanged()
		{
		}

		/// <summary>Clears unprocessed changed to the collection.</summary>
		// Token: 0x06001AAB RID: 6827 RVA: 0x0007E8E0 File Offset: 0x0007CAE0
		protected void ClearPendingChanges()
		{
			object syncRoot = this._changeLog.SyncRoot;
			lock (syncRoot)
			{
				this._changeLog.Clear();
				this._tempChangeLog.Clear();
			}
		}

		/// <summary>Ensures that all pending changes to the collection have been committed.</summary>
		// Token: 0x06001AAC RID: 6828 RVA: 0x0007E938 File Offset: 0x0007CB38
		protected void ProcessPendingChanges()
		{
			object syncRoot = this._changeLog.SyncRoot;
			lock (syncRoot)
			{
				this.ProcessChangeLog(this._changeLog, true);
				this._changeLog.Clear();
			}
		}

		/// <summary>Called by the base class to notify the derived class that an <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event has been posted to the message queue.</summary>
		/// <param name="args">The <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> object that is added to the change log.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="args" /> is <see langword="null" />. </exception>
		// Token: 0x06001AAD RID: 6829 RVA: 0x00002137 File Offset: 0x00000337
		[Obsolete("Replaced by OnAllowsCrossThreadChangesChanged")]
		protected virtual void OnBeginChangeLogging(NotifyCollectionChangedEventArgs args)
		{
		}

		/// <summary>Clears any pending changes from the change log.</summary>
		// Token: 0x06001AAE RID: 6830 RVA: 0x0007E990 File Offset: 0x0007CB90
		[Obsolete("Replaced by ClearPendingChanges")]
		protected void ClearChangeLog()
		{
			this.ClearPendingChanges();
		}

		/// <summary>Refreshes the view or specifies that the view needs to be refreshed when the defer cycle completes.</summary>
		// Token: 0x06001AAF RID: 6831 RVA: 0x0007E998 File Offset: 0x0007CB98
		protected void RefreshOrDefer()
		{
			if (this.IsRefreshDeferred)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.NeedsRefresh, true);
				return;
			}
			this.RefreshInternal();
		}

		/// <summary>Gets a value that indicates whether the underlying collection provides change notifications.</summary>
		/// <returns>
		///     <see langword="true" /> if the underlying collection provides change notifications; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x0007E9B5 File Offset: 0x0007CBB5
		protected bool IsDynamic
		{
			get
			{
				return this.CheckFlag(CollectionView.CollectionViewFlags.IsDynamic);
			}
		}

		/// <summary>Gets a value that indicates whether a thread other than the one that created the <see cref="T:System.Windows.Data.CollectionView" /> can change the <see cref="P:System.Windows.Data.CollectionView.SourceCollection" />. </summary>
		/// <returns>
		///     <see langword="true" /> if a thread other than the one that created the <see cref="T:System.Windows.Data.CollectionView" /> can change the <see cref="P:System.Windows.Data.CollectionView.SourceCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x0007E9BF File Offset: 0x0007CBBF
		protected bool AllowsCrossThreadChanges
		{
			get
			{
				return this.CheckFlag(CollectionView.CollectionViewFlags.AllowsCrossThreadChanges);
			}
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0007E9CC File Offset: 0x0007CBCC
		internal void SetAllowsCrossThreadChanges(bool value)
		{
			bool flag = this.CheckFlag(CollectionView.CollectionViewFlags.AllowsCrossThreadChanges);
			if (flag == value)
			{
				return;
			}
			this.SetFlag(CollectionView.CollectionViewFlags.AllowsCrossThreadChanges, value);
			this.OnAllowsCrossThreadChangesChanged();
		}

		/// <summary>Gets a value that indicates whether it has been necessary to update the change log because a <see cref="E:System.Windows.Data.CollectionView.CollectionChanged" /> notification has been received on a different thread without first entering the user interface (UI) thread dispatcher.</summary>
		/// <returns>
		///     <see langword="true" /> if it has been necessary to update the change log because a <see cref="E:System.Windows.Data.CollectionView.CollectionChanged" /> notification has been received on a different thread without first entering the user interface (UI) thread dispatcher; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001AB3 RID: 6835 RVA: 0x0007E9FC File Offset: 0x0007CBFC
		protected bool UpdatedOutsideDispatcher
		{
			get
			{
				return this.AllowsCrossThreadChanges;
			}
		}

		/// <summary>Gets a value that indicates whether there is an outstanding <see cref="M:System.Windows.Data.CollectionView.DeferRefresh" /> in use.</summary>
		/// <returns>
		///     <see langword="true" /> if there is an outstanding <see cref="M:System.Windows.Data.CollectionView.DeferRefresh" /> in use; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x0007EA04 File Offset: 0x0007CC04
		protected bool IsRefreshDeferred
		{
			get
			{
				return this._deferLevel > 0;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is at the <see cref="P:System.Windows.Data.CollectionView.CurrentPosition" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Data.CollectionView.CurrentItem" /> is in the view and at the <see cref="P:System.Windows.Data.CollectionView.CurrentPosition" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x0007EA0F File Offset: 0x0007CC0F
		protected bool IsCurrentInSync
		{
			get
			{
				if (this.IsCurrentInView)
				{
					return this.GetItemAt(this.CurrentPosition) == this.CurrentItem;
				}
				return this.CurrentItem == null;
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x0007EA38 File Offset: 0x0007CC38
		internal void SetViewManagerData(object value)
		{
			if (this._vmData == null)
			{
				this._vmData = value;
				return;
			}
			object[] array;
			if ((array = (this._vmData as object[])) == null)
			{
				this._vmData = new object[]
				{
					this._vmData,
					value
				};
				return;
			}
			object[] array2 = new object[array.Length + 1];
			array.CopyTo(array2, 0);
			array2[array.Length] = value;
			this._vmData = array2;
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x0007EA9E File Offset: 0x0007CC9E
		internal virtual bool HasReliableHashCodes()
		{
			return this.IsEmpty || HashHelper.HasReliableHashCode(this.GetItemAt(0));
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x0007EAB6 File Offset: 0x0007CCB6
		internal void VerifyRefreshNotDeferred()
		{
			if (this.AllowsCrossThreadChanges)
			{
				base.VerifyAccess();
			}
			if (this.IsRefreshDeferred)
			{
				throw new InvalidOperationException(SR.Get("NoCheckOrChangeWhenDeferred"));
			}
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x0007EAE0 File Offset: 0x0007CCE0
		internal void InvalidateEnumerableWrapper()
		{
			IndexedEnumerable indexedEnumerable = Interlocked.Exchange<IndexedEnumerable>(ref this._enumerableWrapper, null);
			if (indexedEnumerable != null)
			{
				indexedEnumerable.Invalidate();
			}
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x0007EB04 File Offset: 0x0007CD04
		internal ReadOnlyCollection<ItemPropertyInfo> GetItemProperties()
		{
			IEnumerable sourceCollection = this.SourceCollection;
			if (sourceCollection == null)
			{
				return null;
			}
			IEnumerable enumerable = null;
			ITypedList typedList = sourceCollection as ITypedList;
			Type itemType;
			object representativeItem;
			if (typedList != null)
			{
				enumerable = typedList.GetItemProperties(null);
			}
			else if ((itemType = this.GetItemType(false)) != null)
			{
				enumerable = TypeDescriptor.GetProperties(itemType);
			}
			else if ((representativeItem = this.GetRepresentativeItem()) != null)
			{
				ICustomTypeProvider customTypeProvider = representativeItem as ICustomTypeProvider;
				if (customTypeProvider == null)
				{
					enumerable = TypeDescriptor.GetProperties(representativeItem);
				}
				else
				{
					enumerable = customTypeProvider.GetCustomType().GetProperties();
				}
			}
			if (enumerable == null)
			{
				return null;
			}
			List<ItemPropertyInfo> list = new List<ItemPropertyInfo>();
			foreach (object obj in enumerable)
			{
				PropertyDescriptor propertyDescriptor;
				PropertyInfo propertyInfo;
				if ((propertyDescriptor = (obj as PropertyDescriptor)) != null)
				{
					list.Add(new ItemPropertyInfo(propertyDescriptor.Name, propertyDescriptor.PropertyType, propertyDescriptor));
				}
				else if ((propertyInfo = (obj as PropertyInfo)) != null)
				{
					list.Add(new ItemPropertyInfo(propertyInfo.Name, propertyInfo.PropertyType, propertyInfo));
				}
			}
			return new ReadOnlyCollection<ItemPropertyInfo>(list);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x0007EC2C File Offset: 0x0007CE2C
		internal Type GetItemType(bool useRepresentativeItem)
		{
			Type type = this.SourceCollection.GetType();
			foreach (Type type2 in type.GetInterfaces())
			{
				if (type2.Name == CollectionView.IEnumerableT)
				{
					Type[] genericArguments = type2.GetGenericArguments();
					if (genericArguments.Length == 1)
					{
						Type type3 = genericArguments[0];
						if (typeof(ICustomTypeProvider).IsAssignableFrom(type3))
						{
							break;
						}
						if (!(type3 == typeof(object)))
						{
							return type3;
						}
					}
				}
			}
			if (useRepresentativeItem)
			{
				object representativeItem = this.GetRepresentativeItem();
				return ReflectionHelper.GetReflectionType(representativeItem);
			}
			return null;
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x0007ECC4 File Offset: 0x0007CEC4
		internal object GetRepresentativeItem()
		{
			if (this.IsEmpty)
			{
				return null;
			}
			object result = null;
			IEnumerator enumerator = this.GetEnumerator();
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (obj != null && obj != CollectionView.NewItemPlaceholder)
				{
					result = obj;
					break;
				}
			}
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			return result;
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x0007ED18 File Offset: 0x0007CF18
		internal virtual void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, null, sources);
			if (this._sourceCollection != null)
			{
				format(level + 1, this._sourceCollection, null, sources);
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x0007ED58 File Offset: 0x0007CF58
		internal object SyncRoot
		{
			get
			{
				return this._syncObject;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x0007ED60 File Offset: 0x0007CF60
		internal int Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x0007ED68 File Offset: 0x0007CF68
		private bool IsCurrentInView
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return 0 <= this.CurrentPosition && this.CurrentPosition < this.Count;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x0007ED8C File Offset: 0x0007CF8C
		private IndexedEnumerable EnumerableWrapper
		{
			get
			{
				if (this._enumerableWrapper == null)
				{
					IndexedEnumerable value = new IndexedEnumerable(this.SourceCollection, new Predicate<object>(this.PassesFilter));
					Interlocked.CompareExchange<IndexedEnumerable>(ref this._enumerableWrapper, value, null);
				}
				return this._enumerableWrapper;
			}
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0007EDD0 File Offset: 0x0007CFD0
		private void _MoveCurrentToPosition(int position)
		{
			if (position < 0)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, true);
				this.SetCurrent(null, -1);
				return;
			}
			if (position >= this.Count)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, true);
				this.SetCurrent(null, this.Count);
				return;
			}
			this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst | CollectionView.CollectionViewFlags.IsCurrentAfterLast, false);
			this.SetCurrent(this.EnumerableWrapper[position], position);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x0007EE30 File Offset: 0x0007D030
		private void MoveCurrencyOffDeletedElement()
		{
			int num = this.Count - 1;
			int position = (this._currentPosition < num) ? this._currentPosition : num;
			this.OnCurrentChanging();
			this._MoveCurrentToPosition(position);
			this.OnCurrentChanged();
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x0007EE6C File Offset: 0x0007D06C
		private void EndDefer()
		{
			this._deferLevel--;
			if (this._deferLevel == 0 && this.CheckFlag(CollectionView.CollectionViewFlags.NeedsRefresh))
			{
				this.Refresh();
			}
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x0007EE98 File Offset: 0x0007D098
		private void DeferProcessing(ICollection changeLog)
		{
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				object syncRoot2 = this._changeLog.SyncRoot;
				lock (syncRoot2)
				{
					if (this._changeLog == null)
					{
						this._changeLog = new ArrayList(changeLog);
					}
					else
					{
						this._changeLog.InsertRange(0, changeLog);
					}
					if (this._databindOperation != null)
					{
						this._engine.ChangeCost(this._databindOperation, changeLog.Count);
					}
					else
					{
						this._databindOperation = this._engine.Marshal(new DispatcherOperationCallback(this.ProcessInvoke), null, changeLog.Count);
					}
				}
			}
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x0007EF68 File Offset: 0x0007D168
		private ICollection ProcessChangeLog(ArrayList changeLog, bool processAll = false)
		{
			int num = 0;
			bool flag = false;
			long ticks = DateTime.Now.Ticks;
			int count = changeLog.Count;
			while (num < changeLog.Count && !flag)
			{
				NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = changeLog[num] as NotifyCollectionChangedEventArgs;
				if (notifyCollectionChangedEventArgs != null)
				{
					this.ProcessCollectionChanged(notifyCollectionChangedEventArgs);
				}
				if (!processAll)
				{
					flag = (DateTime.Now.Ticks - ticks > 50000L);
				}
				num++;
			}
			if (flag && num < changeLog.Count)
			{
				changeLog.RemoveRange(0, num);
				return changeLog;
			}
			return null;
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x0007EFEE File Offset: 0x0007D1EE
		private bool CheckFlag(CollectionView.CollectionViewFlags flags)
		{
			return (this._flags & flags) > (CollectionView.CollectionViewFlags)0;
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x0007EFFB File Offset: 0x0007D1FB
		private void SetFlag(CollectionView.CollectionViewFlags flags, bool value)
		{
			if (value)
			{
				this._flags |= flags;
				return;
			}
			this._flags &= ~flags;
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0007F020 File Offset: 0x0007D220
		private void PostChange(NotifyCollectionChangedEventArgs args)
		{
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				object syncRoot2 = this._changeLog.SyncRoot;
				lock (syncRoot2)
				{
					if (args.Action == NotifyCollectionChangedAction.Reset)
					{
						this._changeLog.Clear();
					}
					if (this._changeLog.Count == 0 && base.CheckAccess())
					{
						this.ProcessCollectionChanged(args);
					}
					else
					{
						this._changeLog.Add(args);
						if (this._databindOperation == null)
						{
							this._databindOperation = this._engine.Marshal(new DispatcherOperationCallback(this.ProcessInvoke), null, this._changeLog.Count);
						}
					}
				}
			}
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x0007F0F8 File Offset: 0x0007D2F8
		private object ProcessInvoke(object arg)
		{
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				object syncRoot2 = this._changeLog.SyncRoot;
				lock (syncRoot2)
				{
					this._databindOperation = null;
					this._tempChangeLog = this._changeLog;
					this._changeLog = new ArrayList();
				}
			}
			ICollection collection = this.ProcessChangeLog(this._tempChangeLog, false);
			if (collection != null && collection.Count > 0)
			{
				this.DeferProcessing(collection);
			}
			this._tempChangeLog = CollectionView.EmptyArrayList;
			return null;
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x0007F1AC File Offset: 0x0007D3AC
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
				if (e.OldStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("RemovedItemNotFound"));
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

		// Token: 0x06001ACC RID: 6860 RVA: 0x0007F2C4 File Offset: 0x0007D4C4
		private void AdjustCurrencyForAdd(int index)
		{
			if (this.Count == 1)
			{
				this._currentPosition = -1;
				return;
			}
			if (index <= this._currentPosition)
			{
				this._currentPosition++;
				if (this._currentPosition < this.Count)
				{
					this._currentItem = this.EnumerableWrapper[this._currentPosition];
				}
			}
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0007F31E File Offset: 0x0007D51E
		private void AdjustCurrencyForRemove(int index)
		{
			if (index < this._currentPosition)
			{
				this._currentPosition--;
				return;
			}
			if (index == this._currentPosition)
			{
				this._currentElementWasRemovedOrReplaced = true;
			}
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x0007F348 File Offset: 0x0007D548
		private void AdjustCurrencyForMove(int oldIndex, int newIndex)
		{
			if ((oldIndex < this.CurrentPosition && newIndex < this.CurrentPosition) || (oldIndex > this.CurrentPosition && newIndex > this.CurrentPosition))
			{
				return;
			}
			if (oldIndex <= this.CurrentPosition)
			{
				this.AdjustCurrencyForRemove(oldIndex);
				return;
			}
			if (newIndex <= this.CurrentPosition)
			{
				this.AdjustCurrencyForAdd(newIndex);
			}
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x0007F39B File Offset: 0x0007D59B
		private void AdjustCurrencyForReplace(int index)
		{
			if (index == this._currentPosition)
			{
				this._currentElementWasRemovedOrReplaced = true;
			}
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x0007D0A8 File Offset: 0x0007B2A8
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x04001357 RID: 4951
		private ArrayList _changeLog = new ArrayList();

		// Token: 0x04001358 RID: 4952
		private ArrayList _tempChangeLog = CollectionView.EmptyArrayList;

		// Token: 0x04001359 RID: 4953
		private DataBindOperation _databindOperation;

		// Token: 0x0400135A RID: 4954
		private object _vmData;

		// Token: 0x0400135B RID: 4955
		private IEnumerable _sourceCollection;

		// Token: 0x0400135C RID: 4956
		private CultureInfo _culture;

		// Token: 0x0400135D RID: 4957
		private CollectionView.SimpleMonitor _currentChangedMonitor = new CollectionView.SimpleMonitor();

		// Token: 0x0400135E RID: 4958
		private int _deferLevel;

		// Token: 0x0400135F RID: 4959
		private IndexedEnumerable _enumerableWrapper;

		// Token: 0x04001360 RID: 4960
		private Predicate<object> _filter;

		// Token: 0x04001361 RID: 4961
		private object _currentItem;

		// Token: 0x04001362 RID: 4962
		private int _currentPosition;

		// Token: 0x04001363 RID: 4963
		private CollectionView.CollectionViewFlags _flags = CollectionView.CollectionViewFlags.ShouldProcessCollectionChanged | CollectionView.CollectionViewFlags.NeedsRefresh;

		// Token: 0x04001364 RID: 4964
		private bool _currentElementWasRemovedOrReplaced;

		// Token: 0x04001365 RID: 4965
		private static object _newItemPlaceholder = new NamedObject("NewItemPlaceholder");

		// Token: 0x04001366 RID: 4966
		private object _syncObject = new object();

		// Token: 0x04001367 RID: 4967
		private DataBindEngine _engine;

		// Token: 0x04001368 RID: 4968
		private int _timestamp;

		// Token: 0x04001369 RID: 4969
		private static readonly ArrayList EmptyArrayList = new ArrayList();

		// Token: 0x0400136A RID: 4970
		private static readonly string IEnumerableT = typeof(IEnumerable<>).Name;

		// Token: 0x0400136B RID: 4971
		internal static readonly object NoNewItem = new NamedObject("NoNewItem");

		// Token: 0x0400136C RID: 4972
		private static readonly CurrentChangingEventArgs uncancelableCurrentChangingEventArgs = new CurrentChangingEventArgs(false);

		// Token: 0x0400136D RID: 4973
		internal const string CountPropertyName = "Count";

		// Token: 0x0400136E RID: 4974
		internal const string IsEmptyPropertyName = "IsEmpty";

		// Token: 0x0400136F RID: 4975
		internal const string CulturePropertyName = "Culture";

		// Token: 0x04001370 RID: 4976
		internal const string CurrentPositionPropertyName = "CurrentPosition";

		// Token: 0x04001371 RID: 4977
		internal const string CurrentItemPropertyName = "CurrentItem";

		// Token: 0x04001372 RID: 4978
		internal const string IsCurrentBeforeFirstPropertyName = "IsCurrentBeforeFirst";

		// Token: 0x04001373 RID: 4979
		internal const string IsCurrentAfterLastPropertyName = "IsCurrentAfterLast";

		// Token: 0x02000878 RID: 2168
		internal class PlaceholderAwareEnumerator : IEnumerator
		{
			// Token: 0x0600831C RID: 33564 RVA: 0x00244943 File Offset: 0x00242B43
			public PlaceholderAwareEnumerator(CollectionView collectionView, IEnumerator baseEnumerator, NewItemPlaceholderPosition placeholderPosition, object newItem)
			{
				this._collectionView = collectionView;
				this._timestamp = collectionView.Timestamp;
				this._baseEnumerator = baseEnumerator;
				this._placeholderPosition = placeholderPosition;
				this._newItem = newItem;
			}

			// Token: 0x0600831D RID: 33565 RVA: 0x00244974 File Offset: 0x00242B74
			public bool MoveNext()
			{
				if (this._timestamp != this._collectionView.Timestamp)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				switch (this._position)
				{
				case CollectionView.PlaceholderAwareEnumerator.Position.BeforePlaceholder:
					if (this._placeholderPosition == NewItemPlaceholderPosition.AtBeginning)
					{
						this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder;
					}
					else if (!this._baseEnumerator.MoveNext() || (this._newItem != CollectionView.NoNewItem && this._baseEnumerator.Current == this._newItem && !this._baseEnumerator.MoveNext()))
					{
						if (this._newItem != CollectionView.NoNewItem)
						{
							this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnNewItem;
						}
						else
						{
							if (this._placeholderPosition == NewItemPlaceholderPosition.None)
							{
								return false;
							}
							this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder;
						}
					}
					return true;
				case CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder:
					if (this._newItem != CollectionView.NoNewItem && this._placeholderPosition == NewItemPlaceholderPosition.AtBeginning)
					{
						this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnNewItem;
						return true;
					}
					break;
				case CollectionView.PlaceholderAwareEnumerator.Position.OnNewItem:
					if (this._placeholderPosition == NewItemPlaceholderPosition.AtEnd)
					{
						this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder;
						return true;
					}
					break;
				}
				this._position = CollectionView.PlaceholderAwareEnumerator.Position.AfterPlaceholder;
				return this._baseEnumerator.MoveNext() && (this._newItem == CollectionView.NoNewItem || this._baseEnumerator.Current != this._newItem || this._baseEnumerator.MoveNext());
			}

			// Token: 0x17001DB3 RID: 7603
			// (get) Token: 0x0600831E RID: 33566 RVA: 0x00244AAB File Offset: 0x00242CAB
			public object Current
			{
				get
				{
					if (this._position == CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder)
					{
						return CollectionView.NewItemPlaceholder;
					}
					if (this._position != CollectionView.PlaceholderAwareEnumerator.Position.OnNewItem)
					{
						return this._baseEnumerator.Current;
					}
					return this._newItem;
				}
			}

			// Token: 0x0600831F RID: 33567 RVA: 0x00244AD7 File Offset: 0x00242CD7
			public void Reset()
			{
				this._position = CollectionView.PlaceholderAwareEnumerator.Position.BeforePlaceholder;
				this._baseEnumerator.Reset();
			}

			// Token: 0x04004138 RID: 16696
			private CollectionView _collectionView;

			// Token: 0x04004139 RID: 16697
			private IEnumerator _baseEnumerator;

			// Token: 0x0400413A RID: 16698
			private NewItemPlaceholderPosition _placeholderPosition;

			// Token: 0x0400413B RID: 16699
			private CollectionView.PlaceholderAwareEnumerator.Position _position;

			// Token: 0x0400413C RID: 16700
			private object _newItem;

			// Token: 0x0400413D RID: 16701
			private int _timestamp;

			// Token: 0x02000B9F RID: 2975
			private enum Position
			{
				// Token: 0x04004EA0 RID: 20128
				BeforePlaceholder,
				// Token: 0x04004EA1 RID: 20129
				OnPlaceholder,
				// Token: 0x04004EA2 RID: 20130
				OnNewItem,
				// Token: 0x04004EA3 RID: 20131
				AfterPlaceholder
			}
		}

		// Token: 0x02000879 RID: 2169
		private class DeferHelper : IDisposable
		{
			// Token: 0x06008320 RID: 33568 RVA: 0x00244AEB File Offset: 0x00242CEB
			public DeferHelper(CollectionView collectionView)
			{
				this._collectionView = collectionView;
			}

			// Token: 0x06008321 RID: 33569 RVA: 0x00244AFA File Offset: 0x00242CFA
			public void Dispose()
			{
				if (this._collectionView != null)
				{
					this._collectionView.EndDefer();
					this._collectionView = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x0400413E RID: 16702
			private CollectionView _collectionView;
		}

		// Token: 0x0200087A RID: 2170
		private class SimpleMonitor : IDisposable
		{
			// Token: 0x06008322 RID: 33570 RVA: 0x00244B1C File Offset: 0x00242D1C
			public bool Enter()
			{
				if (this._entered)
				{
					return false;
				}
				this._entered = true;
				return true;
			}

			// Token: 0x06008323 RID: 33571 RVA: 0x00244B30 File Offset: 0x00242D30
			public void Dispose()
			{
				this._entered = false;
				GC.SuppressFinalize(this);
			}

			// Token: 0x17001DB4 RID: 7604
			// (get) Token: 0x06008324 RID: 33572 RVA: 0x00244B3F File Offset: 0x00242D3F
			public bool Busy
			{
				get
				{
					return this._entered;
				}
			}

			// Token: 0x0400413F RID: 16703
			private bool _entered;
		}

		// Token: 0x0200087B RID: 2171
		[Flags]
		private enum CollectionViewFlags
		{
			// Token: 0x04004141 RID: 16705
			UpdatedOutsideDispatcher = 2,
			// Token: 0x04004142 RID: 16706
			ShouldProcessCollectionChanged = 4,
			// Token: 0x04004143 RID: 16707
			IsCurrentBeforeFirst = 8,
			// Token: 0x04004144 RID: 16708
			IsCurrentAfterLast = 16,
			// Token: 0x04004145 RID: 16709
			IsDynamic = 32,
			// Token: 0x04004146 RID: 16710
			IsDataInGroupOrder = 64,
			// Token: 0x04004147 RID: 16711
			NeedsRefresh = 128,
			// Token: 0x04004148 RID: 16712
			AllowsCrossThreadChanges = 256,
			// Token: 0x04004149 RID: 16713
			CachedIsEmpty = 512
		}
	}
}
