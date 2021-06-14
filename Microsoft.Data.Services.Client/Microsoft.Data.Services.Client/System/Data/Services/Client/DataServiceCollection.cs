using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Services.Client.Materialization;
using System.Linq;
using System.Threading;

namespace System.Data.Services.Client
{
	// Token: 0x020000F4 RID: 244
	public class DataServiceCollection<T> : ObservableCollection<T>
	{
		// Token: 0x06000805 RID: 2053 RVA: 0x00022408 File Offset: 0x00020608
		public DataServiceCollection() : this(null, null, TrackingMode.AutoChangeTracking, null, null, null)
		{
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00022416 File Offset: 0x00020616
		public DataServiceCollection(IEnumerable<T> items) : this(null, items, TrackingMode.AutoChangeTracking, null, null, null)
		{
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00022424 File Offset: 0x00020624
		public DataServiceCollection(IEnumerable<T> items, TrackingMode trackingMode) : this(null, items, trackingMode, null, null, null)
		{
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00022432 File Offset: 0x00020632
		public DataServiceCollection(DataServiceContext context) : this(context, null, TrackingMode.AutoChangeTracking, null, null, null)
		{
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00022440 File Offset: 0x00020640
		public DataServiceCollection(DataServiceContext context, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback) : this(context, null, TrackingMode.AutoChangeTracking, entitySetName, entityChangedCallback, collectionChangedCallback)
		{
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002244F File Offset: 0x0002064F
		public DataServiceCollection(IEnumerable<T> items, TrackingMode trackingMode, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback) : this(null, items, trackingMode, entitySetName, entityChangedCallback, collectionChangedCallback)
		{
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00022460 File Offset: 0x00020660
		public DataServiceCollection(DataServiceContext context, IEnumerable<T> items, TrackingMode trackingMode, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback)
		{
			if (trackingMode == TrackingMode.AutoChangeTracking)
			{
				if (context == null)
				{
					if (items == null)
					{
						this.trackingOnLoad = true;
						this.entitySetName = entitySetName;
						this.entityChangedCallback = entityChangedCallback;
						this.collectionChangedCallback = collectionChangedCallback;
					}
					else
					{
						context = DataServiceCollection<T>.GetContextFromItems(items);
					}
				}
				if (!this.trackingOnLoad)
				{
					if (items != null)
					{
						DataServiceCollection<T>.ValidateIteratorParameter(items);
					}
					this.StartTracking(context, items, entitySetName, entityChangedCallback, collectionChangedCallback);
					return;
				}
			}
			else if (items != null)
			{
				this.Load(items);
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000224D0 File Offset: 0x000206D0
		internal DataServiceCollection(object entityMaterializer, DataServiceContext context, IEnumerable<T> items, TrackingMode trackingMode, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback) : this((context != null) ? context : ((ODataEntityMaterializer)entityMaterializer).EntityTrackingAdapter.Context, items, trackingMode, entitySetName, entityChangedCallback, collectionChangedCallback)
		{
			if (items != null)
			{
				((ODataEntityMaterializer)entityMaterializer).PropagateContinuation<T>(items, this);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600080D RID: 2061 RVA: 0x00022508 File Offset: 0x00020708
		// (remove) Token: 0x0600080E RID: 2062 RVA: 0x00022540 File Offset: 0x00020740
		public event EventHandler<LoadCompletedEventArgs> LoadCompleted;

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x00022575 File Offset: 0x00020775
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x0002257D File Offset: 0x0002077D
		public DataServiceQueryContinuation<T> Continuation
		{
			get
			{
				return this.continuation;
			}
			set
			{
				this.continuation = value;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x00022586 File Offset: 0x00020786
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x0002258E File Offset: 0x0002078E
		internal BindingObserver Observer
		{
			get
			{
				return this.observer;
			}
			set
			{
				this.observer = value;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x00022597 File Offset: 0x00020797
		internal bool IsTracking
		{
			get
			{
				return this.observer != null;
			}
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x000225A8 File Offset: 0x000207A8
		public void Load(IEnumerable<T> items)
		{
			DataServiceCollection<T>.ValidateIteratorParameter(items);
			if (this.trackingOnLoad)
			{
				DataServiceContext contextFromItems = DataServiceCollection<T>.GetContextFromItems(items);
				this.trackingOnLoad = false;
				this.StartTracking(contextFromItems, items, this.entitySetName, this.entityChangedCallback, this.collectionChangedCallback);
				return;
			}
			this.StartLoading();
			try
			{
				this.InternalLoadCollection(items);
			}
			finally
			{
				this.FinishLoading();
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00022658 File Offset: 0x00020858
		public void LoadAsync(IQueryable<T> query)
		{
			Util.CheckArgumentNull<IQueryable<T>>(query, "query");
			DataServiceQuery<T> dsq = query as DataServiceQuery<T>;
			if (dsq == null)
			{
				throw new ArgumentException(Strings.DataServiceCollection_LoadAsyncRequiresDataServiceQuery, "query");
			}
			if (this.ongoingAsyncOperation != null)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_MultipleLoadAsyncOperationsAtTheSameTime);
			}
			if (this.trackingOnLoad)
			{
				this.StartTracking(((DataServiceQueryProvider)dsq.Provider).Context, null, this.entitySetName, this.entityChangedCallback, this.collectionChangedCallback);
				this.trackingOnLoad = false;
			}
			this.BeginLoadAsyncOperation((AsyncCallback asyncCallback) => dsq.BeginExecute(asyncCallback, null), delegate(IAsyncResult asyncResult)
			{
				QueryOperationResponse<T> queryOperationResponse = (QueryOperationResponse<T>)dsq.EndExecute(asyncResult);
				this.Load(queryOperationResponse);
				return queryOperationResponse;
			});
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0002275C File Offset: 0x0002095C
		public void LoadAsync(Uri requestUri)
		{
			Util.CheckArgumentNull<Uri>(requestUri, "requestUri");
			if (!this.IsTracking)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_OperationForTrackedOnly);
			}
			if (this.ongoingAsyncOperation != null)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_MultipleLoadAsyncOperationsAtTheSameTime);
			}
			DataServiceContext context = this.observer.Context;
			requestUri = UriUtil.CreateUri(context.BaseUri, requestUri);
			this.BeginLoadAsyncOperation((AsyncCallback asyncCallback) => context.BeginExecute<T>(requestUri, asyncCallback, null), delegate(IAsyncResult asyncResult)
			{
				QueryOperationResponse<T> queryOperationResponse = (QueryOperationResponse<T>)context.EndExecute<T>(asyncResult);
				this.Load(queryOperationResponse);
				return queryOperationResponse;
			});
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00022840 File Offset: 0x00020A40
		public void LoadAsync()
		{
			if (!this.IsTracking)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_OperationForTrackedOnly);
			}
			object parent;
			string property;
			if (!this.observer.LookupParent<T>(this, out parent, out property))
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_LoadAsyncNoParamsWithoutParentEntity);
			}
			if (this.ongoingAsyncOperation != null)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_MultipleLoadAsyncOperationsAtTheSameTime);
			}
			this.BeginLoadAsyncOperation((AsyncCallback asyncCallback) => this.observer.Context.BeginLoadProperty(parent, property, asyncCallback, null), (IAsyncResult asyncResult) => this.observer.Context.EndLoadProperty(asyncResult));
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0002290C File Offset: 0x00020B0C
		public bool LoadNextPartialSetAsync()
		{
			if (!this.IsTracking)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_OperationForTrackedOnly);
			}
			if (this.ongoingAsyncOperation != null)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_MultipleLoadAsyncOperationsAtTheSameTime);
			}
			if (this.Continuation == null)
			{
				if (this.LoadCompleted != null)
				{
					this.LoadCompleted(this, new LoadCompletedEventArgs(null, null));
				}
				return false;
			}
			this.BeginLoadAsyncOperation((AsyncCallback asyncCallback) => this.observer.Context.BeginExecute<T>(this.Continuation, asyncCallback, null), delegate(IAsyncResult asyncResult)
			{
				QueryOperationResponse<T> queryOperationResponse = (QueryOperationResponse<T>)this.observer.Context.EndExecute<T>(asyncResult);
				this.Load(queryOperationResponse);
				return queryOperationResponse;
			});
			return true;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00022983 File Offset: 0x00020B83
		public void CancelAsyncLoad()
		{
			if (this.ongoingAsyncOperation != null)
			{
				this.observer.Context.CancelRequest(this.ongoingAsyncOperation);
			}
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x000229A4 File Offset: 0x00020BA4
		public void Load(T item)
		{
			if (item == null)
			{
				throw Error.ArgumentNull("item");
			}
			this.StartLoading();
			try
			{
				if (!base.Contains(item))
				{
					base.Add(item);
				}
			}
			finally
			{
				this.FinishLoading();
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x000229F4 File Offset: 0x00020BF4
		public void Clear(bool stopTracking)
		{
			if (!this.IsTracking)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_OperationForTrackedOnly);
			}
			if (!stopTracking)
			{
				base.Clear();
				return;
			}
			try
			{
				this.observer.DetachBehavior = true;
				base.Clear();
			}
			finally
			{
				this.observer.DetachBehavior = false;
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00022A50 File Offset: 0x00020C50
		public void Detach()
		{
			if (!this.IsTracking)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_OperationForTrackedOnly);
			}
			if (!this.rootCollection)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_CannotStopTrackingChildCollection);
			}
			this.observer.StopTracking();
			this.observer = null;
			this.rootCollection = false;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00022A9C File Offset: 0x00020C9C
		protected override void InsertItem(int index, T item)
		{
			if (this.trackingOnLoad)
			{
				throw new InvalidOperationException(Strings.DataServiceCollection_InsertIntoTrackedButNotLoadedCollection);
			}
			if (this.IsTracking && item != null && !(item is INotifyPropertyChanged))
			{
				throw new InvalidOperationException(Strings.DataBinding_NotifyPropertyChangedNotImpl(item.GetType()));
			}
			base.InsertItem(index, item);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00022AFB File Offset: 0x00020CFB
		private static void ValidateIteratorParameter(IEnumerable<T> items)
		{
			Util.CheckArgumentNull<IEnumerable<T>>(items, "items");
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00022B0C File Offset: 0x00020D0C
		private static DataServiceContext GetContextFromItems(IEnumerable<T> items)
		{
			DataServiceQuery<T> dataServiceQuery = items as DataServiceQuery<T>;
			if (dataServiceQuery != null)
			{
				DataServiceQueryProvider dataServiceQueryProvider = dataServiceQuery.Provider as DataServiceQueryProvider;
				return dataServiceQueryProvider.Context;
			}
			QueryOperationResponse queryOperationResponse = items as QueryOperationResponse;
			if (queryOperationResponse != null)
			{
				return queryOperationResponse.Results.Context;
			}
			throw new ArgumentException(Strings.DataServiceCollection_CannotDetermineContextFromItems);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00022B5C File Offset: 0x00020D5C
		private void InternalLoadCollection(IEnumerable<T> items)
		{
			DataServiceQuery<T> dataServiceQuery = items as DataServiceQuery<T>;
			if (dataServiceQuery != null)
			{
				items = (dataServiceQuery.Execute() as QueryOperationResponse<T>);
			}
			foreach (T item in items)
			{
				if (!base.Contains(item))
				{
					base.Add(item);
				}
			}
			QueryOperationResponse<T> queryOperationResponse = items as QueryOperationResponse<T>;
			if (queryOperationResponse != null)
			{
				this.continuation = queryOperationResponse.GetContinuation();
				return;
			}
			this.continuation = null;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00022BE4 File Offset: 0x00020DE4
		private void StartLoading()
		{
			if (this.IsTracking)
			{
				if (this.observer.Context == null)
				{
					throw new InvalidOperationException(Strings.DataServiceCollection_LoadRequiresTargetCollectionObserved);
				}
				this.observer.AttachBehavior = true;
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00022C12 File Offset: 0x00020E12
		private void FinishLoading()
		{
			if (this.IsTracking)
			{
				this.observer.AttachBehavior = false;
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00022C28 File Offset: 0x00020E28
		private void StartTracking(DataServiceContext context, IEnumerable<T> items, string entitySet, Func<EntityChangedParams, bool> entityChanged, Func<EntityCollectionChangedParams, bool> collectionChanged)
		{
			if (!BindingEntityInfo.IsEntityType(typeof(T), context.Model))
			{
				throw new ArgumentException(Strings.DataBinding_DataServiceCollectionArgumentMustHaveEntityType(typeof(T)));
			}
			this.observer = new BindingObserver(context, entityChanged, collectionChanged);
			if (items != null)
			{
				try
				{
					this.InternalLoadCollection(items);
				}
				catch
				{
					this.observer = null;
					throw;
				}
			}
			this.observer.StartTracking<T>(this, entitySet);
			this.rootCollection = true;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00022D44 File Offset: 0x00020F44
		private void BeginLoadAsyncOperation(Func<AsyncCallback, IAsyncResult> beginCall, Func<IAsyncResult, QueryOperationResponse> endCall)
		{
			DataServiceCollection<T>.<>c__DisplayClass12 CS$<>8__locals1 = new DataServiceCollection<T>.<>c__DisplayClass12();
			CS$<>8__locals1.endCall = endCall;
			CS$<>8__locals1.<>4__this = this;
			this.ongoingAsyncOperation = null;
			try
			{
				SynchronizationContext syncContext = SynchronizationContext.Current;
				AsyncCallback arg;
				if (syncContext == null)
				{
					arg = delegate(IAsyncResult ar)
					{
						CS$<>8__locals1.<>4__this.EndLoadAsyncOperation(CS$<>8__locals1.endCall, ar);
					};
				}
				else
				{
					arg = delegate(IAsyncResult ar)
					{
						DataServiceCollection<T>.<>c__DisplayClass12 CS$<>8__locals13 = CS$<>8__locals1;
						IAsyncResult ar = ar;
						syncContext.Post(delegate(object unused)
						{
							CS$<>8__locals13.<>4__this.EndLoadAsyncOperation(CS$<>8__locals13.endCall, ar);
						}, null);
					};
				}
				this.ongoingAsyncOperation = beginCall(arg);
			}
			catch (Exception)
			{
				this.ongoingAsyncOperation = null;
				throw;
			}
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00022DE4 File Offset: 0x00020FE4
		private void EndLoadAsyncOperation(Func<IAsyncResult, QueryOperationResponse> endCall, IAsyncResult asyncResult)
		{
			try
			{
				QueryOperationResponse queryOperationResponse = endCall(asyncResult);
				this.ongoingAsyncOperation = null;
				if (this.LoadCompleted != null)
				{
					this.LoadCompleted(this, new LoadCompletedEventArgs(queryOperationResponse, null));
				}
			}
			catch (Exception ex)
			{
				if (!CommonUtil.IsCatchableExceptionType(ex))
				{
					throw;
				}
				this.ongoingAsyncOperation = null;
				if (this.LoadCompleted != null)
				{
					this.LoadCompleted(this, new LoadCompletedEventArgs(null, ex));
				}
			}
		}

		// Token: 0x040004D6 RID: 1238
		private BindingObserver observer;

		// Token: 0x040004D7 RID: 1239
		private bool rootCollection;

		// Token: 0x040004D8 RID: 1240
		private DataServiceQueryContinuation<T> continuation;

		// Token: 0x040004D9 RID: 1241
		private bool trackingOnLoad;

		// Token: 0x040004DA RID: 1242
		private Func<EntityChangedParams, bool> entityChangedCallback;

		// Token: 0x040004DB RID: 1243
		private Func<EntityCollectionChangedParams, bool> collectionChangedCallback;

		// Token: 0x040004DC RID: 1244
		private string entitySetName;

		// Token: 0x040004DD RID: 1245
		private IAsyncResult ongoingAsyncOperation;
	}
}
