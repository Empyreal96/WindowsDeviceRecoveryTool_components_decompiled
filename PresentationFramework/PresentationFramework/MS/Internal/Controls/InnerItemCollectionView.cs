using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MS.Internal.Data;

namespace MS.Internal.Controls
{
	// Token: 0x0200075E RID: 1886
	internal sealed class InnerItemCollectionView : CollectionView, IList, ICollection, IEnumerable
	{
		// Token: 0x06007802 RID: 30722 RVA: 0x002237EC File Offset: 0x002219EC
		public InnerItemCollectionView(int capacity, ItemCollection itemCollection) : base(EmptyEnumerable.Instance, false)
		{
			this._rawList = (this._viewList = new ArrayList(capacity));
			this._itemCollection = itemCollection;
		}

		// Token: 0x17001C67 RID: 7271
		// (get) Token: 0x06007803 RID: 30723 RVA: 0x00223821 File Offset: 0x00221A21
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

		// Token: 0x17001C68 RID: 7272
		// (get) Token: 0x06007804 RID: 30724 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanSort
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007805 RID: 30725 RVA: 0x0022383C File Offset: 0x00221A3C
		public override bool Contains(object item)
		{
			return this._viewList.Contains(item);
		}

		// Token: 0x17001C69 RID: 7273
		public object this[int index]
		{
			get
			{
				return this.GetItemAt(index);
			}
			set
			{
				DependencyObject dependencyObject = this.AssertPristineModelChild(value);
				bool flag = this.CurrentPosition == index;
				object obj = this._viewList[index];
				this._viewList[index] = value;
				int num = -1;
				if (this.IsCachedMode)
				{
					num = this._rawList.IndexOf(obj);
					this._rawList[num] = value;
				}
				bool flag2 = true;
				if (dependencyObject != null)
				{
					flag2 = false;
					try
					{
						this.SetModelParent(value);
						flag2 = true;
					}
					finally
					{
						if (!flag2)
						{
							this._viewList[index] = obj;
							if (num > 0)
							{
								this._rawList[num] = obj;
							}
						}
						else
						{
							this.ClearModelParent(obj);
						}
					}
				}
				if (!flag2)
				{
					return;
				}
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, obj, index));
				this.SetIsModified();
			}
		}

		// Token: 0x17001C6A RID: 7274
		// (get) Token: 0x06007808 RID: 30728 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C6B RID: 7275
		// (get) Token: 0x06007809 RID: 30729 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600780A RID: 30730 RVA: 0x00223914 File Offset: 0x00221B14
		public int Add(object item)
		{
			DependencyObject dependencyObject = this.AssertPristineModelChild(item);
			int num = this._viewList.Add(item);
			int num2 = -1;
			if (this.IsCachedMode)
			{
				num2 = this._rawList.Add(item);
			}
			bool flag = true;
			if (dependencyObject != null)
			{
				flag = false;
				try
				{
					this.SetModelParent(item);
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this._viewList.RemoveAt(num);
						if (num2 >= 0)
						{
							this._rawList.RemoveAt(num2);
						}
						this.ClearModelParent(item);
						num = -1;
					}
				}
			}
			if (!flag)
			{
				return -1;
			}
			this.AdjustCurrencyForAdd(num);
			this.SetIsModified();
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, num));
			return num;
		}

		// Token: 0x0600780B RID: 30731 RVA: 0x002239BC File Offset: 0x00221BBC
		public void Clear()
		{
			try
			{
				for (int i = this._rawList.Count - 1; i >= 0; i--)
				{
					this.ClearModelParent(this._rawList[i]);
				}
			}
			finally
			{
				this._rawList.Clear();
				base.RefreshOrDefer();
			}
		}

		// Token: 0x0600780C RID: 30732 RVA: 0x00223A18 File Offset: 0x00221C18
		public void Insert(int index, object item)
		{
			DependencyObject dependencyObject = this.AssertPristineModelChild(item);
			this._viewList.Insert(index, item);
			int num = -1;
			if (this.IsCachedMode)
			{
				num = this._rawList.Add(item);
			}
			bool flag = true;
			if (dependencyObject != null)
			{
				flag = false;
				try
				{
					this.SetModelParent(item);
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this._viewList.RemoveAt(index);
						if (num >= 0)
						{
							this._rawList.RemoveAt(num);
						}
						this.ClearModelParent(item);
					}
				}
			}
			if (!flag)
			{
				return;
			}
			this.AdjustCurrencyForAdd(index);
			this.SetIsModified();
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
		}

		// Token: 0x0600780D RID: 30733 RVA: 0x00223ABC File Offset: 0x00221CBC
		public void Remove(object item)
		{
			int index = this._viewList.IndexOf(item);
			int indexR = -1;
			if (this.IsCachedMode)
			{
				indexR = this._rawList.IndexOf(item);
			}
			this._RemoveAt(index, indexR, item);
		}

		// Token: 0x0600780E RID: 30734 RVA: 0x00223AF8 File Offset: 0x00221CF8
		public void RemoveAt(int index)
		{
			if (0 <= index && index < this.ViewCount)
			{
				object obj = this[index];
				int indexR = -1;
				if (this.IsCachedMode)
				{
					indexR = this._rawList.IndexOf(obj);
				}
				this._RemoveAt(index, indexR, obj);
				return;
			}
			throw new ArgumentOutOfRangeException("index", SR.Get("ItemCollectionRemoveArgumentOutOfRange"));
		}

		// Token: 0x17001C6C RID: 7276
		// (get) Token: 0x0600780F RID: 30735 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C6D RID: 7277
		// (get) Token: 0x06007810 RID: 30736 RVA: 0x00223B4F File Offset: 0x00221D4F
		object ICollection.SyncRoot
		{
			get
			{
				return this._rawList.SyncRoot;
			}
		}

		// Token: 0x06007811 RID: 30737 RVA: 0x00223B5C File Offset: 0x00221D5C
		void ICollection.CopyTo(Array array, int index)
		{
			this._viewList.CopyTo(array, index);
		}

		// Token: 0x17001C6E RID: 7278
		// (get) Token: 0x06007812 RID: 30738 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public override IEnumerable SourceCollection
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001C6F RID: 7279
		// (get) Token: 0x06007813 RID: 30739 RVA: 0x00223B6B File Offset: 0x00221D6B
		public override int Count
		{
			get
			{
				return this.ViewCount;
			}
		}

		// Token: 0x17001C70 RID: 7280
		// (get) Token: 0x06007814 RID: 30740 RVA: 0x00223B73 File Offset: 0x00221D73
		public override bool IsEmpty
		{
			get
			{
				return this.ViewCount == 0;
			}
		}

		// Token: 0x17001C71 RID: 7281
		// (get) Token: 0x06007815 RID: 30741 RVA: 0x00223B7E File Offset: 0x00221D7E
		public override bool NeedsRefresh
		{
			get
			{
				return base.NeedsRefresh || this._isModified;
			}
		}

		// Token: 0x06007816 RID: 30742 RVA: 0x00223B90 File Offset: 0x00221D90
		public override int IndexOf(object item)
		{
			return this._viewList.IndexOf(item);
		}

		// Token: 0x06007817 RID: 30743 RVA: 0x00223B9E File Offset: 0x00221D9E
		public override object GetItemAt(int index)
		{
			return this._viewList[index];
		}

		// Token: 0x06007818 RID: 30744 RVA: 0x00223BAC File Offset: 0x00221DAC
		public override bool MoveCurrentTo(object item)
		{
			if (ItemsControl.EqualsEx(this.CurrentItem, item) && (item != null || this.IsCurrentInView))
			{
				return this.IsCurrentInView;
			}
			return this.MoveCurrentToPosition(this.IndexOf(item));
		}

		// Token: 0x06007819 RID: 30745 RVA: 0x00223BDC File Offset: 0x00221DDC
		public override bool MoveCurrentToPosition(int position)
		{
			if (position < -1 || position > this.ViewCount)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			if (position != this.CurrentPosition && base.OKToChangeCurrent())
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

		// Token: 0x0600781A RID: 30746 RVA: 0x00223C74 File Offset: 0x00221E74
		protected override void RefreshOverride()
		{
			bool isEmpty = this.IsEmpty;
			object currentItem = this.CurrentItem;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			int currentPosition = this.CurrentPosition;
			base.OnCurrentChanging();
			if (this.SortDescriptions.Count > 0 || this.Filter != null)
			{
				if (this.Filter == null)
				{
					this._viewList = new ArrayList(this._rawList);
				}
				else
				{
					this._viewList = new ArrayList();
					for (int i = 0; i < this._rawList.Count; i++)
					{
						if (this.Filter(this._rawList[i]))
						{
							this._viewList.Add(this._rawList[i]);
						}
					}
				}
				if (this._sort != null && this._sort.Count > 0 && this.ViewCount > 0)
				{
					SortFieldComparer.SortHelper(this._viewList, new SortFieldComparer(this._sort, this.Culture));
				}
			}
			else
			{
				this._viewList = this._rawList;
			}
			if (this.IsEmpty || isCurrentBeforeFirst)
			{
				this._MoveCurrentToPosition(-1);
			}
			else if (isCurrentAfterLast)
			{
				this._MoveCurrentToPosition(this.ViewCount);
			}
			else if (currentItem != null)
			{
				int num = this._viewList.IndexOf(currentItem);
				if (num < 0)
				{
					num = 0;
				}
				this._MoveCurrentToPosition(num);
			}
			this.ClearIsModified();
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
			if (currentPosition != this.CurrentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (currentItem != this.CurrentItem)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		// Token: 0x0600781B RID: 30747 RVA: 0x00223E2E File Offset: 0x0022202E
		protected override IEnumerator GetEnumerator()
		{
			return this._viewList.GetEnumerator();
		}

		// Token: 0x17001C72 RID: 7282
		// (get) Token: 0x0600781C RID: 30748 RVA: 0x00223E3B File Offset: 0x0022203B
		internal ItemCollection ItemCollection
		{
			get
			{
				return this._itemCollection;
			}
		}

		// Token: 0x17001C73 RID: 7283
		// (get) Token: 0x0600781D RID: 30749 RVA: 0x00223E43 File Offset: 0x00222043
		internal IEnumerator LogicalChildren
		{
			get
			{
				return this._rawList.GetEnumerator();
			}
		}

		// Token: 0x17001C74 RID: 7284
		// (get) Token: 0x0600781E RID: 30750 RVA: 0x00223E50 File Offset: 0x00222050
		internal int RawCount
		{
			get
			{
				return this._rawList.Count;
			}
		}

		// Token: 0x17001C75 RID: 7285
		// (get) Token: 0x0600781F RID: 30751 RVA: 0x00223E5D File Offset: 0x0022205D
		private int ViewCount
		{
			get
			{
				return this._viewList.Count;
			}
		}

		// Token: 0x17001C76 RID: 7286
		// (get) Token: 0x06007820 RID: 30752 RVA: 0x00223E6A File Offset: 0x0022206A
		private bool IsCachedMode
		{
			get
			{
				return this._viewList != this._rawList;
			}
		}

		// Token: 0x17001C77 RID: 7287
		// (get) Token: 0x06007821 RID: 30753 RVA: 0x00223E7D File Offset: 0x0022207D
		private FrameworkElement ModelParentFE
		{
			get
			{
				return this.ItemCollection.ModelParentFE;
			}
		}

		// Token: 0x17001C78 RID: 7288
		// (get) Token: 0x06007822 RID: 30754 RVA: 0x00223E8A File Offset: 0x0022208A
		private bool IsCurrentInView
		{
			get
			{
				return 0 <= this.CurrentPosition && this.CurrentPosition < this.ViewCount;
			}
		}

		// Token: 0x06007823 RID: 30755 RVA: 0x00223EA5 File Offset: 0x002220A5
		private void SetIsModified()
		{
			if (this.IsCachedMode)
			{
				this._isModified = true;
			}
		}

		// Token: 0x06007824 RID: 30756 RVA: 0x00223EB6 File Offset: 0x002220B6
		private void ClearIsModified()
		{
			this._isModified = false;
		}

		// Token: 0x06007825 RID: 30757 RVA: 0x00223EC0 File Offset: 0x002220C0
		private void _RemoveAt(int index, int indexR, object item)
		{
			if (index >= 0)
			{
				this._viewList.RemoveAt(index);
			}
			if (indexR >= 0)
			{
				this._rawList.RemoveAt(indexR);
			}
			try
			{
				this.ClearModelParent(item);
			}
			finally
			{
				if (index >= 0)
				{
					this.AdjustCurrencyForRemove(index);
					this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
					if (this._currentElementWasRemoved)
					{
						this.MoveCurrencyOffDeletedElement();
					}
				}
			}
		}

		// Token: 0x06007826 RID: 30758 RVA: 0x00223F30 File Offset: 0x00222130
		private DependencyObject AssertPristineModelChild(object item)
		{
			DependencyObject dependencyObject = item as DependencyObject;
			if (dependencyObject == null)
			{
				return null;
			}
			if (LogicalTreeHelper.GetParent(dependencyObject) != null)
			{
				throw new InvalidOperationException(SR.Get("ReparentModelChildIllegal"));
			}
			return dependencyObject;
		}

		// Token: 0x06007827 RID: 30759 RVA: 0x00223F62 File Offset: 0x00222162
		private void SetModelParent(object item)
		{
			if (this.ModelParentFE != null && item is DependencyObject)
			{
				LogicalTreeHelper.AddLogicalChild(this.ModelParentFE, null, item);
			}
		}

		// Token: 0x06007828 RID: 30760 RVA: 0x00223F81 File Offset: 0x00222181
		private void ClearModelParent(object item)
		{
			if (this.ModelParentFE != null && item is DependencyObject)
			{
				LogicalTreeHelper.RemoveLogicalChild(this.ModelParentFE, null, item);
			}
		}

		// Token: 0x06007829 RID: 30761 RVA: 0x00223FA0 File Offset: 0x002221A0
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

		// Token: 0x0600782A RID: 30762 RVA: 0x0022400A File Offset: 0x0022220A
		private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.RefreshOrDefer();
		}

		// Token: 0x0600782B RID: 30763 RVA: 0x00224012 File Offset: 0x00222212
		private void _MoveCurrentToPosition(int position)
		{
			if (position < 0)
			{
				base.SetCurrent(null, -1);
				return;
			}
			if (position >= this.ViewCount)
			{
				base.SetCurrent(null, this.ViewCount);
				return;
			}
			base.SetCurrent(this._viewList[position], position);
		}

		// Token: 0x0600782C RID: 30764 RVA: 0x0022404C File Offset: 0x0022224C
		private void AdjustCurrencyForAdd(int index)
		{
			if (index < 0)
			{
				return;
			}
			if (this.ViewCount == 1)
			{
				base.SetCurrent(null, -1);
				return;
			}
			if (index <= this.CurrentPosition)
			{
				int num = this.CurrentPosition + 1;
				if (num < this.ViewCount)
				{
					base.SetCurrent(this._viewList[num], num);
					return;
				}
				base.SetCurrent(null, this.ViewCount);
			}
		}

		// Token: 0x0600782D RID: 30765 RVA: 0x002240AC File Offset: 0x002222AC
		private void AdjustCurrencyForRemove(int index)
		{
			if (index < 0)
			{
				return;
			}
			if (index < this.CurrentPosition)
			{
				int num = this.CurrentPosition - 1;
				base.SetCurrent(this._viewList[num], num);
				return;
			}
			if (index == this.CurrentPosition)
			{
				this._currentElementWasRemoved = true;
			}
		}

		// Token: 0x0600782E RID: 30766 RVA: 0x002240F4 File Offset: 0x002222F4
		private void MoveCurrencyOffDeletedElement()
		{
			int num = this.ViewCount - 1;
			int position = (this.CurrentPosition < num) ? this.CurrentPosition : num;
			this._currentElementWasRemoved = false;
			base.OnCurrentChanging();
			this._MoveCurrentToPosition(position);
			this.OnCurrentChanged();
		}

		// Token: 0x0600782F RID: 30767 RVA: 0x0007D0A8 File Offset: 0x0007B2A8
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x040038E3 RID: 14563
		private SortDescriptionCollection _sort;

		// Token: 0x040038E4 RID: 14564
		private ArrayList _viewList;

		// Token: 0x040038E5 RID: 14565
		private ArrayList _rawList;

		// Token: 0x040038E6 RID: 14566
		private ItemCollection _itemCollection;

		// Token: 0x040038E7 RID: 14567
		private bool _isModified;

		// Token: 0x040038E8 RID: 14568
		private bool _currentElementWasRemoved;
	}
}
