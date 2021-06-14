using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000728 RID: 1832
	internal class EnumerableCollectionView : CollectionView, IItemProperties
	{
		// Token: 0x0600751D RID: 29981 RVA: 0x00217CC8 File Offset: 0x00215EC8
		internal EnumerableCollectionView(IEnumerable source) : base(source, -1)
		{
			this._snapshot = new ObservableCollection<object>();
			this._pollForChanges = !(source is INotifyCollectionChanged);
			this.LoadSnapshotCore(source);
			if (this._snapshot.Count > 0)
			{
				base.SetCurrent(this._snapshot[0], 0, 1);
			}
			else
			{
				base.SetCurrent(null, -1, 0);
			}
			this._view = new ListCollectionView(this._snapshot);
			INotifyCollectionChanged view = this._view;
			view.CollectionChanged += this._OnViewChanged;
			INotifyPropertyChanged view2 = this._view;
			view2.PropertyChanged += this._OnPropertyChanged;
			this._view.CurrentChanging += this._OnCurrentChanging;
			this._view.CurrentChanged += this._OnCurrentChanged;
		}

		// Token: 0x17001BD5 RID: 7125
		// (get) Token: 0x0600751E RID: 29982 RVA: 0x00217D9F File Offset: 0x00215F9F
		// (set) Token: 0x0600751F RID: 29983 RVA: 0x00217DAC File Offset: 0x00215FAC
		public override CultureInfo Culture
		{
			get
			{
				return this._view.Culture;
			}
			set
			{
				this._view.Culture = value;
			}
		}

		// Token: 0x06007520 RID: 29984 RVA: 0x00217DBA File Offset: 0x00215FBA
		public override bool Contains(object item)
		{
			this.EnsureSnapshot();
			return this._view.Contains(item);
		}

		// Token: 0x17001BD6 RID: 7126
		// (get) Token: 0x06007521 RID: 29985 RVA: 0x00217DCE File Offset: 0x00215FCE
		// (set) Token: 0x06007522 RID: 29986 RVA: 0x00217DDB File Offset: 0x00215FDB
		public override Predicate<object> Filter
		{
			get
			{
				return this._view.Filter;
			}
			set
			{
				this._view.Filter = value;
			}
		}

		// Token: 0x17001BD7 RID: 7127
		// (get) Token: 0x06007523 RID: 29987 RVA: 0x00217DE9 File Offset: 0x00215FE9
		public override bool CanFilter
		{
			get
			{
				return this._view.CanFilter;
			}
		}

		// Token: 0x17001BD8 RID: 7128
		// (get) Token: 0x06007524 RID: 29988 RVA: 0x00217DF6 File Offset: 0x00215FF6
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this._view.SortDescriptions;
			}
		}

		// Token: 0x17001BD9 RID: 7129
		// (get) Token: 0x06007525 RID: 29989 RVA: 0x00217E03 File Offset: 0x00216003
		public override bool CanSort
		{
			get
			{
				return this._view.CanSort;
			}
		}

		// Token: 0x17001BDA RID: 7130
		// (get) Token: 0x06007526 RID: 29990 RVA: 0x00217E10 File Offset: 0x00216010
		public override bool CanGroup
		{
			get
			{
				return this._view.CanGroup;
			}
		}

		// Token: 0x17001BDB RID: 7131
		// (get) Token: 0x06007527 RID: 29991 RVA: 0x00217E1D File Offset: 0x0021601D
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._view.GroupDescriptions;
			}
		}

		// Token: 0x17001BDC RID: 7132
		// (get) Token: 0x06007528 RID: 29992 RVA: 0x00217E2A File Offset: 0x0021602A
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				return this._view.Groups;
			}
		}

		// Token: 0x06007529 RID: 29993 RVA: 0x00217E37 File Offset: 0x00216037
		public override IDisposable DeferRefresh()
		{
			return this._view.DeferRefresh();
		}

		// Token: 0x17001BDD RID: 7133
		// (get) Token: 0x0600752A RID: 29994 RVA: 0x00217E44 File Offset: 0x00216044
		public override object CurrentItem
		{
			get
			{
				return this._view.CurrentItem;
			}
		}

		// Token: 0x17001BDE RID: 7134
		// (get) Token: 0x0600752B RID: 29995 RVA: 0x00217E51 File Offset: 0x00216051
		public override int CurrentPosition
		{
			get
			{
				return this._view.CurrentPosition;
			}
		}

		// Token: 0x17001BDF RID: 7135
		// (get) Token: 0x0600752C RID: 29996 RVA: 0x00217E5E File Offset: 0x0021605E
		public override bool IsCurrentAfterLast
		{
			get
			{
				return this._view.IsCurrentAfterLast;
			}
		}

		// Token: 0x17001BE0 RID: 7136
		// (get) Token: 0x0600752D RID: 29997 RVA: 0x00217E6B File Offset: 0x0021606B
		public override bool IsCurrentBeforeFirst
		{
			get
			{
				return this._view.IsCurrentBeforeFirst;
			}
		}

		// Token: 0x0600752E RID: 29998 RVA: 0x00217E78 File Offset: 0x00216078
		public override bool MoveCurrentToFirst()
		{
			return this._view.MoveCurrentToFirst();
		}

		// Token: 0x0600752F RID: 29999 RVA: 0x00217E85 File Offset: 0x00216085
		public override bool MoveCurrentToPrevious()
		{
			return this._view.MoveCurrentToPrevious();
		}

		// Token: 0x06007530 RID: 30000 RVA: 0x00217E92 File Offset: 0x00216092
		public override bool MoveCurrentToNext()
		{
			return this._view.MoveCurrentToNext();
		}

		// Token: 0x06007531 RID: 30001 RVA: 0x00217E9F File Offset: 0x0021609F
		public override bool MoveCurrentToLast()
		{
			return this._view.MoveCurrentToLast();
		}

		// Token: 0x06007532 RID: 30002 RVA: 0x00217EAC File Offset: 0x002160AC
		public override bool MoveCurrentTo(object item)
		{
			return this._view.MoveCurrentTo(item);
		}

		// Token: 0x06007533 RID: 30003 RVA: 0x00217EBA File Offset: 0x002160BA
		public override bool MoveCurrentToPosition(int position)
		{
			return this._view.MoveCurrentToPosition(position);
		}

		// Token: 0x17001BE1 RID: 7137
		// (get) Token: 0x06007534 RID: 30004 RVA: 0x00217EC8 File Offset: 0x002160C8
		public ReadOnlyCollection<ItemPropertyInfo> ItemProperties
		{
			get
			{
				return ((IItemProperties)this._view).ItemProperties;
			}
		}

		// Token: 0x17001BE2 RID: 7138
		// (get) Token: 0x06007535 RID: 30005 RVA: 0x00217ED5 File Offset: 0x002160D5
		public override int Count
		{
			get
			{
				this.EnsureSnapshot();
				return this._view.Count;
			}
		}

		// Token: 0x17001BE3 RID: 7139
		// (get) Token: 0x06007536 RID: 30006 RVA: 0x00217EE8 File Offset: 0x002160E8
		public override bool IsEmpty
		{
			get
			{
				this.EnsureSnapshot();
				return this._view == null || this._view.IsEmpty;
			}
		}

		// Token: 0x17001BE4 RID: 7140
		// (get) Token: 0x06007537 RID: 30007 RVA: 0x00217F05 File Offset: 0x00216105
		public override bool NeedsRefresh
		{
			get
			{
				return this._view.NeedsRefresh;
			}
		}

		// Token: 0x06007538 RID: 30008 RVA: 0x00217F12 File Offset: 0x00216112
		public override int IndexOf(object item)
		{
			this.EnsureSnapshot();
			return this._view.IndexOf(item);
		}

		// Token: 0x06007539 RID: 30009 RVA: 0x00217F26 File Offset: 0x00216126
		public override bool PassesFilter(object item)
		{
			return !this._view.CanFilter || this._view.Filter == null || this._view.Filter(item);
		}

		// Token: 0x0600753A RID: 30010 RVA: 0x00217F55 File Offset: 0x00216155
		public override object GetItemAt(int index)
		{
			this.EnsureSnapshot();
			return this._view.GetItemAt(index);
		}

		// Token: 0x0600753B RID: 30011 RVA: 0x00217F69 File Offset: 0x00216169
		protected override IEnumerator GetEnumerator()
		{
			this.EnsureSnapshot();
			return ((IEnumerable)this._view).GetEnumerator();
		}

		// Token: 0x0600753C RID: 30012 RVA: 0x00217F7C File Offset: 0x0021617C
		protected override void RefreshOverride()
		{
			this.LoadSnapshot(this.SourceCollection);
		}

		// Token: 0x0600753D RID: 30013 RVA: 0x00217F8C File Offset: 0x0021618C
		protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this._view == null)
			{
				return;
			}
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (args.NewStartingIndex < 0 || this._snapshot.Count <= args.NewStartingIndex)
				{
					for (int i = 0; i < args.NewItems.Count; i++)
					{
						this._snapshot.Add(args.NewItems[i]);
					}
					return;
				}
				for (int j = args.NewItems.Count - 1; j >= 0; j--)
				{
					this._snapshot.Insert(args.NewStartingIndex, args.NewItems[j]);
				}
				return;
			case NotifyCollectionChangedAction.Remove:
			{
				if (args.OldStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("RemovedItemNotFound"));
				}
				int k = args.OldItems.Count - 1;
				int num = args.OldStartingIndex + k;
				while (k >= 0)
				{
					if (!ItemsControl.EqualsEx(args.OldItems[k], this._snapshot[num]))
					{
						throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
						{
							num
						}));
					}
					this._snapshot.RemoveAt(num);
					k--;
					num--;
				}
				return;
			}
			case NotifyCollectionChangedAction.Replace:
			{
				int l = args.NewItems.Count - 1;
				int num2 = args.NewStartingIndex + l;
				while (l >= 0)
				{
					if (!ItemsControl.EqualsEx(args.OldItems[l], this._snapshot[num2]))
					{
						throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
						{
							num2
						}));
					}
					this._snapshot[num2] = args.NewItems[l];
					l--;
					num2--;
				}
				return;
			}
			case NotifyCollectionChangedAction.Move:
			{
				if (args.NewStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("CannotMoveToUnknownPosition"));
				}
				if (args.OldStartingIndex < args.NewStartingIndex)
				{
					int m = args.OldItems.Count - 1;
					int num3 = args.OldStartingIndex + m;
					int num4 = args.NewStartingIndex + m;
					while (m >= 0)
					{
						if (!ItemsControl.EqualsEx(args.OldItems[m], this._snapshot[num3]))
						{
							throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
							{
								num3
							}));
						}
						this._snapshot.Move(num3, num4);
						m--;
						num3--;
						num4--;
					}
					return;
				}
				int n = 0;
				int num5 = args.OldStartingIndex + n;
				int num6 = args.NewStartingIndex + n;
				while (n < args.OldItems.Count)
				{
					if (!ItemsControl.EqualsEx(args.OldItems[n], this._snapshot[num5]))
					{
						throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
						{
							num5
						}));
					}
					this._snapshot.Move(num5, num6);
					n++;
					num5++;
					num6++;
				}
				return;
			}
			case NotifyCollectionChangedAction.Reset:
				this.LoadSnapshot(this.SourceCollection);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600753E RID: 30014 RVA: 0x002182B0 File Offset: 0x002164B0
		private void LoadSnapshot(IEnumerable source)
		{
			base.OnCurrentChanging();
			object currentItem = this.CurrentItem;
			int currentPosition = this.CurrentPosition;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			this.LoadSnapshotCore(source);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			this.OnCurrentChanged();
			if (this.IsCurrentAfterLast != isCurrentAfterLast)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("IsCurrentAfterLast"));
			}
			if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("IsCurrentBeforeFirst"));
			}
			if (currentPosition != this.CurrentPosition)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentPosition"));
			}
			if (currentItem != this.CurrentItem)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentItem"));
			}
		}

		// Token: 0x0600753F RID: 30015 RVA: 0x0021835C File Offset: 0x0021655C
		private void LoadSnapshotCore(IEnumerable source)
		{
			IEnumerator enumerator = source.GetEnumerator();
			using (this.IgnoreViewEvents())
			{
				this._snapshot.Clear();
				while (enumerator.MoveNext())
				{
					object item = enumerator.Current;
					this._snapshot.Add(item);
				}
			}
			if (this._pollForChanges)
			{
				IEnumerator trackingEnumerator = this._trackingEnumerator;
				this._trackingEnumerator = enumerator;
				enumerator = trackingEnumerator;
			}
			IDisposable disposable2 = enumerator as IDisposable;
			if (disposable2 != null)
			{
				disposable2.Dispose();
			}
		}

		// Token: 0x06007540 RID: 30016 RVA: 0x002183E4 File Offset: 0x002165E4
		private void EnsureSnapshot()
		{
			if (this._pollForChanges)
			{
				try
				{
					this._trackingEnumerator.MoveNext();
				}
				catch (InvalidOperationException)
				{
					if (TraceData.IsEnabled && !this._warningHasBeenRaised)
					{
						this._warningHasBeenRaised = true;
						TraceData.Trace(TraceEventType.Warning, TraceData.CollectionChangedWithoutNotification(new object[]
						{
							this.SourceCollection.GetType().FullName
						}));
					}
					this.LoadSnapshotCore(this.SourceCollection);
				}
			}
		}

		// Token: 0x06007541 RID: 30017 RVA: 0x00218460 File Offset: 0x00216660
		private IDisposable IgnoreViewEvents()
		{
			return new EnumerableCollectionView.IgnoreViewEventsHelper(this);
		}

		// Token: 0x06007542 RID: 30018 RVA: 0x00218468 File Offset: 0x00216668
		private void BeginIgnoreEvents()
		{
			this._ignoreEventsLevel++;
		}

		// Token: 0x06007543 RID: 30019 RVA: 0x00218478 File Offset: 0x00216678
		private void EndIgnoreEvents()
		{
			this._ignoreEventsLevel--;
		}

		// Token: 0x06007544 RID: 30020 RVA: 0x00218488 File Offset: 0x00216688
		private void _OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (this._ignoreEventsLevel != 0)
			{
				return;
			}
			this.OnPropertyChanged(args);
		}

		// Token: 0x06007545 RID: 30021 RVA: 0x0021849A File Offset: 0x0021669A
		private void _OnViewChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (this._ignoreEventsLevel != 0)
			{
				return;
			}
			this.OnCollectionChanged(args);
		}

		// Token: 0x06007546 RID: 30022 RVA: 0x002184AC File Offset: 0x002166AC
		private void _OnCurrentChanging(object sender, CurrentChangingEventArgs args)
		{
			if (this._ignoreEventsLevel != 0)
			{
				return;
			}
			base.OnCurrentChanging();
		}

		// Token: 0x06007547 RID: 30023 RVA: 0x002184BD File Offset: 0x002166BD
		private void _OnCurrentChanged(object sender, EventArgs args)
		{
			if (this._ignoreEventsLevel != 0)
			{
				return;
			}
			this.OnCurrentChanged();
		}

		// Token: 0x0400381B RID: 14363
		private ListCollectionView _view;

		// Token: 0x0400381C RID: 14364
		private ObservableCollection<object> _snapshot;

		// Token: 0x0400381D RID: 14365
		private IEnumerator _trackingEnumerator;

		// Token: 0x0400381E RID: 14366
		private int _ignoreEventsLevel;

		// Token: 0x0400381F RID: 14367
		private bool _pollForChanges;

		// Token: 0x04003820 RID: 14368
		private bool _warningHasBeenRaised;

		// Token: 0x02000B4F RID: 2895
		private class IgnoreViewEventsHelper : IDisposable
		{
			// Token: 0x06008DB7 RID: 36279 RVA: 0x0025A0DE File Offset: 0x002582DE
			public IgnoreViewEventsHelper(EnumerableCollectionView parent)
			{
				this._parent = parent;
				this._parent.BeginIgnoreEvents();
			}

			// Token: 0x06008DB8 RID: 36280 RVA: 0x0025A0F8 File Offset: 0x002582F8
			public void Dispose()
			{
				if (this._parent != null)
				{
					this._parent.EndIgnoreEvents();
					this._parent = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004AE9 RID: 19177
			private EnumerableCollectionView _parent;
		}
	}
}
