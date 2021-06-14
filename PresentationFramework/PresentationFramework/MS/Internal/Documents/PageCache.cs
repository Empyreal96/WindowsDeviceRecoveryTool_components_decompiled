using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020006D7 RID: 1751
	internal class PageCache
	{
		// Token: 0x0600713A RID: 28986 RVA: 0x00206568 File Offset: 0x00204768
		public PageCache()
		{
			this._cache = new List<PageCacheEntry>(this._defaultCacheSize);
			this._pageDestroyedWatcher = new PageDestroyedWatcher();
		}

		// Token: 0x17001AE1 RID: 6881
		// (get) Token: 0x0600713C RID: 28988 RVA: 0x002067A3 File Offset: 0x002049A3
		// (set) Token: 0x0600713B RID: 28987 RVA: 0x002065BC File Offset: 0x002047BC
		public DynamicDocumentPaginator Content
		{
			get
			{
				return this._documentPaginator;
			}
			set
			{
				if (this._documentPaginator != value)
				{
					this._dynamicPageSizes = false;
					this._defaultPageSize = this._initialDefaultPageSize;
					this._isDefaultSizeKnown = false;
					this._isPaginationCompleted = false;
					if (this._documentPaginator != null)
					{
						this._documentPaginator.PagesChanged -= this.OnPagesChanged;
						this._documentPaginator.GetPageCompleted -= this.OnGetPageCompleted;
						this._documentPaginator.PaginationProgress -= this.OnPaginationProgress;
						this._documentPaginator.PaginationCompleted -= this.OnPaginationCompleted;
						this._documentPaginator.IsBackgroundPaginationEnabled = this._originalIsBackgroundPaginationEnabled;
					}
					this._documentPaginator = value;
					this.ClearCache();
					if (this._documentPaginator != null)
					{
						this._documentPaginator.PagesChanged += this.OnPagesChanged;
						this._documentPaginator.GetPageCompleted += this.OnGetPageCompleted;
						this._documentPaginator.PaginationProgress += this.OnPaginationProgress;
						this._documentPaginator.PaginationCompleted += this.OnPaginationCompleted;
						this._documentPaginator.PageSize = this._defaultPageSize;
						this._originalIsBackgroundPaginationEnabled = this._documentPaginator.IsBackgroundPaginationEnabled;
						this._documentPaginator.IsBackgroundPaginationEnabled = true;
						if (this._documentPaginator.Source is DependencyObject)
						{
							if ((FlowDirection)((DependencyObject)this._documentPaginator.Source).GetValue(FrameworkElement.FlowDirectionProperty) == FlowDirection.LeftToRight)
							{
								this._isContentRightToLeft = false;
							}
							else
							{
								this._isContentRightToLeft = true;
							}
						}
					}
					if (this._documentPaginator != null)
					{
						if (this._documentPaginator.PageCount > 0)
						{
							this.OnPaginationProgress(this._documentPaginator, new PaginationProgressEventArgs(0, this._documentPaginator.PageCount));
						}
						if (this._documentPaginator.IsPageCountValid)
						{
							this.OnPaginationCompleted(this._documentPaginator, EventArgs.Empty);
						}
					}
				}
			}
		}

		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x0600713D RID: 28989 RVA: 0x002067AB File Offset: 0x002049AB
		public int PageCount
		{
			get
			{
				return this._cache.Count;
			}
		}

		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x0600713E RID: 28990 RVA: 0x002067B8 File Offset: 0x002049B8
		public bool DynamicPageSizes
		{
			get
			{
				return this._dynamicPageSizes;
			}
		}

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x0600713F RID: 28991 RVA: 0x002067C0 File Offset: 0x002049C0
		public bool IsContentRightToLeft
		{
			get
			{
				return this._isContentRightToLeft;
			}
		}

		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x06007140 RID: 28992 RVA: 0x002067C8 File Offset: 0x002049C8
		public bool IsPaginationCompleted
		{
			get
			{
				return this._isPaginationCompleted;
			}
		}

		// Token: 0x14000148 RID: 328
		// (add) Token: 0x06007141 RID: 28993 RVA: 0x002067D0 File Offset: 0x002049D0
		// (remove) Token: 0x06007142 RID: 28994 RVA: 0x00206808 File Offset: 0x00204A08
		public event PaginationProgressEventHandler PaginationProgress;

		// Token: 0x14000149 RID: 329
		// (add) Token: 0x06007143 RID: 28995 RVA: 0x00206840 File Offset: 0x00204A40
		// (remove) Token: 0x06007144 RID: 28996 RVA: 0x00206878 File Offset: 0x00204A78
		public event EventHandler PaginationCompleted;

		// Token: 0x1400014A RID: 330
		// (add) Token: 0x06007145 RID: 28997 RVA: 0x002068B0 File Offset: 0x00204AB0
		// (remove) Token: 0x06007146 RID: 28998 RVA: 0x002068E8 File Offset: 0x00204AE8
		public event PagesChangedEventHandler PagesChanged;

		// Token: 0x1400014B RID: 331
		// (add) Token: 0x06007147 RID: 28999 RVA: 0x00206920 File Offset: 0x00204B20
		// (remove) Token: 0x06007148 RID: 29000 RVA: 0x00206958 File Offset: 0x00204B58
		public event GetPageCompletedEventHandler GetPageCompleted;

		// Token: 0x1400014C RID: 332
		// (add) Token: 0x06007149 RID: 29001 RVA: 0x00206990 File Offset: 0x00204B90
		// (remove) Token: 0x0600714A RID: 29002 RVA: 0x002069C8 File Offset: 0x00204BC8
		public event PageCacheChangedEventHandler PageCacheChanged;

		// Token: 0x0600714B RID: 29003 RVA: 0x00206A00 File Offset: 0x00204C00
		public Size GetPageSize(int pageNumber)
		{
			if (pageNumber >= 0 && pageNumber < this._cache.Count)
			{
				Size pageSize = this._cache[pageNumber].PageSize;
				Invariant.Assert(pageSize != Size.Empty, "PageCache entry's PageSize is Empty.");
				return pageSize;
			}
			return new Size(0.0, 0.0);
		}

		// Token: 0x0600714C RID: 29004 RVA: 0x00206A5F File Offset: 0x00204C5F
		public bool IsPageDirty(int pageNumber)
		{
			return pageNumber < 0 || pageNumber >= this._cache.Count || this._cache[pageNumber].Dirty;
		}

		// Token: 0x0600714D RID: 29005 RVA: 0x00206A86 File Offset: 0x00204C86
		private void OnPaginationProgress(object sender, PaginationProgressEventArgs args)
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.PaginationProgressDelegate), args);
		}

		// Token: 0x0600714E RID: 29006 RVA: 0x00206AA4 File Offset: 0x00204CA4
		private object PaginationProgressDelegate(object parameter)
		{
			PaginationProgressEventArgs paginationProgressEventArgs = parameter as PaginationProgressEventArgs;
			if (paginationProgressEventArgs == null)
			{
				throw new InvalidOperationException("parameter");
			}
			this.ValidatePaginationArgs(paginationProgressEventArgs.Start, paginationProgressEventArgs.Count);
			if (this._isPaginationCompleted)
			{
				if (paginationProgressEventArgs.Start == 0)
				{
					this._isDefaultSizeKnown = false;
					this._dynamicPageSizes = false;
				}
				this._isPaginationCompleted = false;
			}
			if (paginationProgressEventArgs.Start + paginationProgressEventArgs.Count < 0)
			{
				throw new ArgumentOutOfRangeException("args");
			}
			List<PageCacheChange> list = new List<PageCacheChange>(2);
			if (paginationProgressEventArgs.Count > 0)
			{
				if (paginationProgressEventArgs.Start >= this._cache.Count)
				{
					PageCacheChange pageCacheChange = this.AddRange(paginationProgressEventArgs.Start, paginationProgressEventArgs.Count);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
				else if (paginationProgressEventArgs.Start + paginationProgressEventArgs.Count < this._cache.Count)
				{
					PageCacheChange pageCacheChange = this.DirtyRange(paginationProgressEventArgs.Start, paginationProgressEventArgs.Count);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
				else
				{
					PageCacheChange pageCacheChange = this.DirtyRange(paginationProgressEventArgs.Start, this._cache.Count - paginationProgressEventArgs.Start);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
					pageCacheChange = this.AddRange(this._cache.Count, paginationProgressEventArgs.Count - (this._cache.Count - paginationProgressEventArgs.Start) + 1);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
			}
			int num = (this._documentPaginator != null) ? this._documentPaginator.PageCount : 0;
			if (num < this._cache.Count)
			{
				PageCacheChange pageCacheChange = new PageCacheChange(num, this._cache.Count - num, PageCacheChangeType.Remove);
				list.Add(pageCacheChange);
				this._cache.RemoveRange(num, this._cache.Count - num);
			}
			this.FirePageCacheChangedEvent(list);
			if (this.PaginationProgress != null)
			{
				this.PaginationProgress(this, paginationProgressEventArgs);
			}
			return null;
		}

		// Token: 0x0600714F RID: 29007 RVA: 0x00206C73 File Offset: 0x00204E73
		private void OnPaginationCompleted(object sender, EventArgs args)
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.PaginationCompletedDelegate), args);
		}

		// Token: 0x06007150 RID: 29008 RVA: 0x00206C90 File Offset: 0x00204E90
		private object PaginationCompletedDelegate(object parameter)
		{
			EventArgs eventArgs = parameter as EventArgs;
			if (eventArgs == null)
			{
				throw new ArgumentOutOfRangeException("parameter");
			}
			this._isPaginationCompleted = true;
			if (this.PaginationCompleted != null)
			{
				this.PaginationCompleted(this, eventArgs);
			}
			return null;
		}

		// Token: 0x06007151 RID: 29009 RVA: 0x00206CCF File Offset: 0x00204ECF
		private void OnPagesChanged(object sender, PagesChangedEventArgs args)
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.PagesChangedDelegate), args);
		}

		// Token: 0x06007152 RID: 29010 RVA: 0x00206CEC File Offset: 0x00204EEC
		private object PagesChangedDelegate(object parameter)
		{
			PagesChangedEventArgs pagesChangedEventArgs = parameter as PagesChangedEventArgs;
			if (pagesChangedEventArgs == null)
			{
				throw new ArgumentOutOfRangeException("parameter");
			}
			this.ValidatePaginationArgs(pagesChangedEventArgs.Start, pagesChangedEventArgs.Count);
			int num = pagesChangedEventArgs.Count;
			if (pagesChangedEventArgs.Start + pagesChangedEventArgs.Count >= this._cache.Count || pagesChangedEventArgs.Start + pagesChangedEventArgs.Count < 0)
			{
				num = this._cache.Count - pagesChangedEventArgs.Start;
			}
			List<PageCacheChange> list = new List<PageCacheChange>(1);
			if (num > 0)
			{
				PageCacheChange pageCacheChange = this.DirtyRange(pagesChangedEventArgs.Start, num);
				if (pageCacheChange != null)
				{
					list.Add(pageCacheChange);
				}
				this.FirePageCacheChangedEvent(list);
			}
			if (this.PagesChanged != null)
			{
				this.PagesChanged(this, pagesChangedEventArgs);
			}
			return null;
		}

		// Token: 0x06007153 RID: 29011 RVA: 0x00206DA4 File Offset: 0x00204FA4
		private void OnGetPageCompleted(object sender, GetPageCompletedEventArgs args)
		{
			if (!args.Cancelled && args.Error == null && args.DocumentPage != DocumentPage.Missing)
			{
				this._pageDestroyedWatcher.AddPage(args.DocumentPage);
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.GetPageCompletedDelegate), args);
			}
		}

		// Token: 0x06007154 RID: 29012 RVA: 0x00206DFC File Offset: 0x00204FFC
		private object GetPageCompletedDelegate(object parameter)
		{
			GetPageCompletedEventArgs getPageCompletedEventArgs = parameter as GetPageCompletedEventArgs;
			if (getPageCompletedEventArgs == null)
			{
				throw new ArgumentOutOfRangeException("parameter");
			}
			bool flag = this._pageDestroyedWatcher.IsDestroyed(getPageCompletedEventArgs.DocumentPage);
			this._pageDestroyedWatcher.RemovePage(getPageCompletedEventArgs.DocumentPage);
			if (flag)
			{
				return null;
			}
			if (!getPageCompletedEventArgs.Cancelled && getPageCompletedEventArgs.Error == null && getPageCompletedEventArgs.DocumentPage != DocumentPage.Missing)
			{
				if (getPageCompletedEventArgs.DocumentPage.Size == Size.Empty)
				{
					throw new ArgumentOutOfRangeException("args");
				}
				PageCacheEntry pageCacheEntry;
				pageCacheEntry.PageSize = getPageCompletedEventArgs.DocumentPage.Size;
				pageCacheEntry.Dirty = false;
				List<PageCacheChange> list = new List<PageCacheChange>(2);
				if (getPageCompletedEventArgs.PageNumber > this._cache.Count - 1)
				{
					PageCacheChange pageCacheChange = this.AddRange(getPageCompletedEventArgs.PageNumber, 1);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
					pageCacheChange = this.UpdateEntry(getPageCompletedEventArgs.PageNumber, pageCacheEntry);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
				else
				{
					PageCacheChange pageCacheChange = this.UpdateEntry(getPageCompletedEventArgs.PageNumber, pageCacheEntry);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
				if (this._isDefaultSizeKnown && pageCacheEntry.PageSize != this._lastPageSize)
				{
					this._dynamicPageSizes = true;
				}
				this._lastPageSize = pageCacheEntry.PageSize;
				if (!this._isDefaultSizeKnown)
				{
					this._defaultPageSize = pageCacheEntry.PageSize;
					this._isDefaultSizeKnown = true;
					this.SetDefaultPageSize(true);
				}
				this.FirePageCacheChangedEvent(list);
			}
			if (this.GetPageCompleted != null)
			{
				this.GetPageCompleted(this, getPageCompletedEventArgs);
			}
			return null;
		}

		// Token: 0x06007155 RID: 29013 RVA: 0x00206F84 File Offset: 0x00205184
		private void ValidatePaginationArgs(int start, int count)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
		}

		// Token: 0x06007156 RID: 29014 RVA: 0x00206FA4 File Offset: 0x002051A4
		private void SetDefaultPageSize(bool dirtyOnly)
		{
			List<PageCacheChange> list = new List<PageCacheChange>(this.PageCount);
			Invariant.Assert(this._defaultPageSize != Size.Empty, "Default Page Size is Empty.");
			for (int i = 0; i < this._cache.Count; i++)
			{
				if (this._cache[i].Dirty || !dirtyOnly)
				{
					PageCacheEntry newEntry;
					newEntry.PageSize = this._defaultPageSize;
					newEntry.Dirty = true;
					PageCacheChange pageCacheChange = this.UpdateEntry(i, newEntry);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
			}
			this.FirePageCacheChangedEvent(list);
		}

		// Token: 0x06007157 RID: 29015 RVA: 0x00207034 File Offset: 0x00205234
		private void FirePageCacheChangedEvent(List<PageCacheChange> changes)
		{
			if (this.PageCacheChanged != null && changes != null && changes.Count > 0)
			{
				PageCacheChangedEventArgs e = new PageCacheChangedEventArgs(changes);
				this.PageCacheChanged(this, e);
			}
		}

		// Token: 0x06007158 RID: 29016 RVA: 0x0020706C File Offset: 0x0020526C
		private PageCacheChange AddRange(int start, int count)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count < 1)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			Invariant.Assert(this._defaultPageSize != Size.Empty, "Default Page Size is Empty.");
			if (start >= this._cache.Count)
			{
				count += start - this._cache.Count;
				start = this._cache.Count;
			}
			for (int i = start; i < start + count; i++)
			{
				PageCacheEntry item;
				item.PageSize = this._defaultPageSize;
				item.Dirty = true;
				this._cache.Add(item);
			}
			return new PageCacheChange(start, count, PageCacheChangeType.Add);
		}

		// Token: 0x06007159 RID: 29017 RVA: 0x00207114 File Offset: 0x00205314
		private PageCacheChange UpdateEntry(int index, PageCacheEntry newEntry)
		{
			if (index >= this._cache.Count || index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			Invariant.Assert(newEntry.PageSize != Size.Empty, "Updated entry newEntry has Empty PageSize.");
			if (newEntry.PageSize != this._cache[index].PageSize || newEntry.Dirty != this._cache[index].Dirty)
			{
				this._cache[index] = newEntry;
				return new PageCacheChange(index, 1, PageCacheChangeType.Update);
			}
			return null;
		}

		// Token: 0x0600715A RID: 29018 RVA: 0x002071A8 File Offset: 0x002053A8
		private PageCacheChange DirtyRange(int start, int count)
		{
			if (start >= this._cache.Count)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (start + count > this._cache.Count || count < 1)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			Invariant.Assert(this._defaultPageSize != Size.Empty, "Default Page Size is Empty.");
			for (int i = start; i < start + count; i++)
			{
				PageCacheEntry value;
				value.Dirty = true;
				value.PageSize = this._defaultPageSize;
				this._cache[i] = value;
			}
			return new PageCacheChange(start, count, PageCacheChangeType.Update);
		}

		// Token: 0x0600715B RID: 29019 RVA: 0x00207240 File Offset: 0x00205440
		private void ClearCache()
		{
			if (this._cache.Count > 0)
			{
				List<PageCacheChange> list = new List<PageCacheChange>(1);
				PageCacheChange item = new PageCacheChange(0, this._cache.Count, PageCacheChangeType.Remove);
				list.Add(item);
				this._cache.Clear();
				this.FirePageCacheChangedEvent(list);
			}
		}

		// Token: 0x0400370A RID: 14090
		private List<PageCacheEntry> _cache;

		// Token: 0x0400370B RID: 14091
		private PageDestroyedWatcher _pageDestroyedWatcher;

		// Token: 0x0400370C RID: 14092
		private DynamicDocumentPaginator _documentPaginator;

		// Token: 0x0400370D RID: 14093
		private bool _originalIsBackgroundPaginationEnabled;

		// Token: 0x0400370E RID: 14094
		private bool _dynamicPageSizes;

		// Token: 0x0400370F RID: 14095
		private bool _isContentRightToLeft;

		// Token: 0x04003710 RID: 14096
		private bool _isPaginationCompleted;

		// Token: 0x04003711 RID: 14097
		private bool _isDefaultSizeKnown;

		// Token: 0x04003712 RID: 14098
		private Size _defaultPageSize;

		// Token: 0x04003713 RID: 14099
		private Size _lastPageSize;

		// Token: 0x04003714 RID: 14100
		private readonly Size _initialDefaultPageSize = new Size(816.0, 1056.0);

		// Token: 0x04003715 RID: 14101
		private readonly int _defaultCacheSize = 64;
	}
}
