using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Utility;

namespace System.Windows.Data
{
	/// <summary>Holds an existing collection structure, such as an <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> or a <see cref="T:System.Data.DataSet" />, to be used inside a <see cref="T:System.Windows.Data.CompositeCollection" />.</summary>
	// Token: 0x020001A4 RID: 420
	public class CollectionContainer : DependencyObject, INotifyCollectionChanged, IWeakEventListener
	{
		/// <summary>Gets or sets the collection to add. </summary>
		/// <returns>The collection to add. The default is an empty collection.</returns>
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x0007D68F File Offset: 0x0007B88F
		// (set) Token: 0x06001A4E RID: 6734 RVA: 0x0007D6A1 File Offset: 0x0007B8A1
		public IEnumerable Collection
		{
			get
			{
				return (IEnumerable)base.GetValue(CollectionContainer.CollectionProperty);
			}
			set
			{
				base.SetValue(CollectionContainer.CollectionProperty, value);
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.CollectionContainer.Collection" /> property should be persisted. </summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A4F RID: 6735 RVA: 0x0007D6B0 File Offset: 0x0007B8B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCollection()
		{
			if (this.Collection == null)
			{
				return false;
			}
			ICollection collection = this.Collection as ICollection;
			if (collection != null && collection.Count == 0)
			{
				return false;
			}
			IEnumerator enumerator = this.Collection.GetEnumerator();
			bool result = enumerator.MoveNext();
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			return result;
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x0007D704 File Offset: 0x0007B904
		internal ICollectionView View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x0007D70C File Offset: 0x0007B90C
		internal int ViewCount
		{
			get
			{
				if (this.View == null)
				{
					return 0;
				}
				CollectionView collectionView = this.View as CollectionView;
				if (collectionView != null)
				{
					return collectionView.Count;
				}
				ICollection collection = this.View as ICollection;
				if (collection != null)
				{
					return collection.Count;
				}
				if (this.ViewList != null)
				{
					return this.ViewList.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x0007D764 File Offset: 0x0007B964
		internal bool ViewIsEmpty
		{
			get
			{
				if (this.View == null)
				{
					return true;
				}
				ICollectionView view = this.View;
				if (view != null)
				{
					return view.IsEmpty;
				}
				ICollection collection = this.View as ICollection;
				if (collection != null)
				{
					return collection.Count == 0;
				}
				if (this.ViewList == null)
				{
					return true;
				}
				IndexedEnumerable viewList = this.ViewList;
				if (viewList != null)
				{
					return viewList.IsEmpty;
				}
				return this.ViewList.Count == 0;
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0007D7D0 File Offset: 0x0007B9D0
		internal object ViewItem(int index)
		{
			Invariant.Assert(index >= 0 && this.View != null);
			CollectionView collectionView = this.View as CollectionView;
			if (collectionView != null)
			{
				return collectionView.GetItemAt(index);
			}
			if (this.ViewList != null)
			{
				return this.ViewList[index];
			}
			return null;
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0007D820 File Offset: 0x0007BA20
		internal int ViewIndexOf(object item)
		{
			if (this.View == null)
			{
				return -1;
			}
			CollectionView collectionView = this.View as CollectionView;
			if (collectionView != null)
			{
				return collectionView.IndexOf(item);
			}
			if (this.ViewList != null)
			{
				return this.ViewList.IndexOf(item);
			}
			return -1;
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0007D864 File Offset: 0x0007BA64
		internal void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			if (this._view != null)
			{
				CollectionView collectionView = this._view as CollectionView;
				if (collectionView != null)
				{
					collectionView.GetCollectionChangedSources(level + 1, format, sources);
					return;
				}
				format(level + 1, this._view, new bool?(true), sources);
			}
		}

		/// <summary>Occurs when the continaed collection has changed.</summary>
		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06001A56 RID: 6742 RVA: 0x0007D8B9 File Offset: 0x0007BAB9
		// (remove) Token: 0x06001A57 RID: 6743 RVA: 0x0007D8C2 File Offset: 0x0007BAC2
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

		/// <summary>Occurs when the contained collection changes.</summary>
		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06001A58 RID: 6744 RVA: 0x0007D8CC File Offset: 0x0007BACC
		// (remove) Token: 0x06001A59 RID: 6745 RVA: 0x0007D904 File Offset: 0x0007BB04
		protected virtual event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>Raises the <see cref="E:System.Windows.Data.CollectionContainer.CollectionChanged" /> event.</summary>
		/// <param name="args">The event data.</param>
		// Token: 0x06001A5A RID: 6746 RVA: 0x0007D939 File Offset: 0x0007BB39
		protected virtual void OnContainedCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method. This only recognizes manager objects of type <see cref="T:System.Collections.Specialized.CollectionChangedEventManager" />.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="e">Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A5B RID: 6747 RVA: 0x0007D950 File Offset: 0x0007BB50
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		/// <summary>Handles events from the centralized event table.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method. This only recognizes manager objects of type <see cref="T:System.Collections.Specialized.CollectionChangedEventManager" />.</param>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A5C RID: 6748 RVA: 0x0000B02A File Offset: 0x0000922A
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x0007D95B File Offset: 0x0007BB5B
		private IndexedEnumerable ViewList
		{
			get
			{
				if (this._viewList == null && this.View != null)
				{
					this._viewList = new IndexedEnumerable(this.View);
				}
				return this._viewList;
			}
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0007D984 File Offset: 0x0007BB84
		private static object OnGetCollection(DependencyObject d)
		{
			return ((CollectionContainer)d).Collection;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0007D994 File Offset: 0x0007BB94
		private static void OnCollectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionContainer collectionContainer = (CollectionContainer)d;
			collectionContainer.HookUpToCollection((IEnumerable)e.NewValue, true);
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0007D9BC File Offset: 0x0007BBBC
		private void HookUpToCollection(IEnumerable newCollection, bool shouldRaiseChangeEvent)
		{
			this._viewList = null;
			if (this.View != null)
			{
				CollectionChangedEventManager.RemoveHandler(this.View, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
				if (this._traceLog != null)
				{
					this._traceLog.Add("Unsubscribe to CollectionChange from {0}", new object[]
					{
						TraceLog.IdFor(this.View)
					});
				}
			}
			if (newCollection != null)
			{
				this._view = CollectionViewSource.GetDefaultCollectionView(newCollection, this, null);
			}
			else
			{
				this._view = null;
			}
			if (this.View != null)
			{
				CollectionChangedEventManager.AddHandler(this.View, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
				if (this._traceLog != null)
				{
					this._traceLog.Add("Subscribe to CollectionChange from {0}", new object[]
					{
						TraceLog.IdFor(this.View)
					});
				}
			}
			if (shouldRaiseChangeEvent)
			{
				this.OnContainedCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x0007DA8F File Offset: 0x0007BC8F
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnContainedCollectionChanged(e);
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x0007DA98 File Offset: 0x0007BC98
		private void InitializeTraceLog()
		{
			this._traceLog = new TraceLog(20);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionContainer.Collection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionContainer.Collection" /> dependency property.</returns>
		// Token: 0x0400134C RID: 4940
		public static readonly DependencyProperty CollectionProperty = DependencyProperty.Register("Collection", typeof(IEnumerable), typeof(CollectionContainer), new FrameworkPropertyMetadata(new PropertyChangedCallback(CollectionContainer.OnCollectionPropertyChanged)));

		// Token: 0x0400134E RID: 4942
		private TraceLog _traceLog;

		// Token: 0x0400134F RID: 4943
		private ICollectionView _view;

		// Token: 0x04001350 RID: 4944
		private IndexedEnumerable _viewList;
	}
}
