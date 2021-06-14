using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000715 RID: 1813
	internal class DataBindEngine : DispatcherObject
	{
		// Token: 0x060074AA RID: 29866 RVA: 0x0021612C File Offset: 0x0021432C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private DataBindEngine()
		{
			DataBindEngine.DataBindEngineShutDownListener dataBindEngineShutDownListener = new DataBindEngine.DataBindEngineShutDownListener(this);
			this._head = new DataBindEngine.Task(null, TaskOps.TransferValue, null);
			this._tail = this._head;
			this._mostRecentTask = new HybridDictionary();
			this._cleanupHelper = new CleanupHelper(new Func<bool, bool>(this.DoCleanup), 400, 10000, 5000);
		}

		// Token: 0x17001BC1 RID: 7105
		// (get) Token: 0x060074AB RID: 29867 RVA: 0x002161FB File Offset: 0x002143FB
		internal PathParser PathParser
		{
			get
			{
				return this._pathParser;
			}
		}

		// Token: 0x17001BC2 RID: 7106
		// (get) Token: 0x060074AC RID: 29868 RVA: 0x00216203 File Offset: 0x00214403
		internal ValueConverterContext ValueConverterContext
		{
			get
			{
				return this._valueConverterContext;
			}
		}

		// Token: 0x17001BC3 RID: 7107
		// (get) Token: 0x060074AD RID: 29869 RVA: 0x0021620B File Offset: 0x0021440B
		internal AccessorTable AccessorTable
		{
			get
			{
				return this._accessorTable;
			}
		}

		// Token: 0x17001BC4 RID: 7108
		// (get) Token: 0x060074AE RID: 29870 RVA: 0x00216213 File Offset: 0x00214413
		internal bool IsShutDown
		{
			get
			{
				return this._viewManager == null;
			}
		}

		// Token: 0x17001BC5 RID: 7109
		// (get) Token: 0x060074AF RID: 29871 RVA: 0x0021621E File Offset: 0x0021441E
		// (set) Token: 0x060074B0 RID: 29872 RVA: 0x00216226 File Offset: 0x00214426
		internal bool CleanupEnabled
		{
			get
			{
				return this._cleanupEnabled;
			}
			set
			{
				this._cleanupEnabled = value;
				WeakEventManager.SetCleanupEnabled(value);
			}
		}

		// Token: 0x17001BC6 RID: 7110
		// (get) Token: 0x060074B1 RID: 29873 RVA: 0x00216235 File Offset: 0x00214435
		internal IAsyncDataDispatcher AsyncDataDispatcher
		{
			get
			{
				if (this._defaultAsyncDataDispatcher == null)
				{
					this._defaultAsyncDataDispatcher = new DefaultAsyncDataDispatcher();
				}
				return this._defaultAsyncDataDispatcher;
			}
		}

		// Token: 0x17001BC7 RID: 7111
		// (get) Token: 0x060074B2 RID: 29874 RVA: 0x00216250 File Offset: 0x00214450
		internal static DataBindEngine CurrentDataBindEngine
		{
			get
			{
				if (DataBindEngine._currentEngine == null)
				{
					DataBindEngine._currentEngine = new DataBindEngine();
				}
				return DataBindEngine._currentEngine;
			}
		}

		// Token: 0x17001BC8 RID: 7112
		// (get) Token: 0x060074B3 RID: 29875 RVA: 0x00216268 File Offset: 0x00214468
		internal ViewManager ViewManager
		{
			get
			{
				return this._viewManager;
			}
		}

		// Token: 0x17001BC9 RID: 7113
		// (get) Token: 0x060074B4 RID: 29876 RVA: 0x00216270 File Offset: 0x00214470
		internal CommitManager CommitManager
		{
			get
			{
				if (!this._commitManager.IsEmpty)
				{
					this.ScheduleCleanup();
				}
				return this._commitManager;
			}
		}

		// Token: 0x060074B5 RID: 29877 RVA: 0x0021628C File Offset: 0x0021448C
		internal void AddTask(IDataBindEngineClient c, TaskOps op)
		{
			if (this._mostRecentTask == null)
			{
				return;
			}
			if (this._head == this._tail)
			{
				this.RequestRun();
			}
			DataBindEngine.Task previousForClient = (DataBindEngine.Task)this._mostRecentTask[c];
			DataBindEngine.Task task = new DataBindEngine.Task(c, op, previousForClient);
			this._tail.Next = task;
			this._tail = task;
			this._mostRecentTask[c] = task;
			if (op == TaskOps.AttachToContext && this._layoutElement == null && (this._layoutElement = (c.TargetElement as UIElement)) != null)
			{
				this._layoutElement.LayoutUpdated += this.OnLayoutUpdated;
			}
		}

		// Token: 0x060074B6 RID: 29878 RVA: 0x0021632C File Offset: 0x0021452C
		internal void CancelTask(IDataBindEngineClient c, TaskOps op)
		{
			if (this._mostRecentTask == null)
			{
				return;
			}
			for (DataBindEngine.Task task = (DataBindEngine.Task)this._mostRecentTask[c]; task != null; task = task.PreviousForClient)
			{
				if (task.op == op && task.status == DataBindEngine.Task.Status.Pending)
				{
					task.status = DataBindEngine.Task.Status.Cancelled;
					return;
				}
			}
		}

		// Token: 0x060074B7 RID: 29879 RVA: 0x0021637C File Offset: 0x0021457C
		internal void CancelTasks(IDataBindEngineClient c)
		{
			if (this._mostRecentTask == null)
			{
				return;
			}
			for (DataBindEngine.Task task = (DataBindEngine.Task)this._mostRecentTask[c]; task != null; task = task.PreviousForClient)
			{
				Invariant.Assert(task.client == c, "task list is corrupt");
				if (task.status == DataBindEngine.Task.Status.Pending)
				{
					task.status = DataBindEngine.Task.Status.Cancelled;
				}
			}
			this._mostRecentTask.Remove(c);
		}

		// Token: 0x060074B8 RID: 29880 RVA: 0x002163E0 File Offset: 0x002145E0
		internal object Run(object arg)
		{
			bool flag = (bool)arg;
			DataBindEngine.Task task = flag ? null : new DataBindEngine.Task(null, TaskOps.TransferValue, null);
			DataBindEngine.Task task2 = task;
			if (this._layoutElement != null)
			{
				this._layoutElement.LayoutUpdated -= this.OnLayoutUpdated;
				this._layoutElement = null;
			}
			if (this.IsShutDown)
			{
				return null;
			}
			DataBindEngine.Task next;
			for (DataBindEngine.Task task3 = this._head.Next; task3 != null; task3 = next)
			{
				task3.PreviousForClient = null;
				if (task3.status == DataBindEngine.Task.Status.Pending)
				{
					task3.Run(flag);
					next = task3.Next;
					if (task3.status == DataBindEngine.Task.Status.Retry && !flag)
					{
						task3.status = DataBindEngine.Task.Status.Pending;
						task2.Next = task3;
						task2 = task3;
						task2.Next = null;
					}
				}
				else
				{
					next = task3.Next;
				}
			}
			this._head.Next = null;
			this._tail = this._head;
			this._mostRecentTask.Clear();
			if (!flag)
			{
				DataBindEngine.Task head = this._head;
				this._head = null;
				for (DataBindEngine.Task next2 = task.Next; next2 != null; next2 = next2.Next)
				{
					this.AddTask(next2.client, next2.op);
				}
				this._head = head;
			}
			return null;
		}

		// Token: 0x060074B9 RID: 29881 RVA: 0x00216508 File Offset: 0x00214708
		internal ViewRecord GetViewRecord(object collection, CollectionViewSource key, Type collectionViewType, bool createView, Func<object, object> GetSourceItem)
		{
			if (this.IsShutDown)
			{
				return null;
			}
			ViewRecord viewRecord = this._viewManager.GetViewRecord(collection, key, collectionViewType, createView, GetSourceItem);
			if (viewRecord != null && !viewRecord.IsInitialized)
			{
				this.ScheduleCleanup();
			}
			return viewRecord;
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x00216544 File Offset: 0x00214744
		internal void RegisterCollectionSynchronizationCallback(IEnumerable collection, object context, CollectionSynchronizationCallback synchronizationCallback)
		{
			this._viewManager.RegisterCollectionSynchronizationCallback(collection, context, synchronizationCallback);
		}

		// Token: 0x060074BB RID: 29883 RVA: 0x00216554 File Offset: 0x00214754
		internal IValueConverter GetDefaultValueConverter(Type sourceType, Type targetType, bool targetToSource)
		{
			IValueConverter valueConverter = this._valueConverterTable[sourceType, targetType, targetToSource];
			if (valueConverter == null)
			{
				valueConverter = DefaultValueConverter.Create(sourceType, targetType, targetToSource, this);
				if (valueConverter != null)
				{
					this._valueConverterTable.Add(sourceType, targetType, targetToSource, valueConverter);
				}
			}
			return valueConverter;
		}

		// Token: 0x060074BC RID: 29884 RVA: 0x00216590 File Offset: 0x00214790
		internal void AddAsyncRequest(DependencyObject target, AsyncDataRequest request)
		{
			if (target == null)
			{
				return;
			}
			IAsyncDataDispatcher asyncDataDispatcher = this.AsyncDataDispatcher;
			if (this._asyncDispatchers == null)
			{
				this._asyncDispatchers = new HybridDictionary(1);
			}
			this._asyncDispatchers[asyncDataDispatcher] = null;
			asyncDataDispatcher.AddRequest(request);
		}

		// Token: 0x060074BD RID: 29885 RVA: 0x002165D0 File Offset: 0x002147D0
		internal object GetValue(object item, PropertyDescriptor pd, bool indexerIsNext)
		{
			return this._valueTable.GetValue(item, pd, indexerIsNext);
		}

		// Token: 0x060074BE RID: 29886 RVA: 0x002165E0 File Offset: 0x002147E0
		internal void RegisterForCacheChanges(object item, object descriptor)
		{
			PropertyDescriptor propertyDescriptor = descriptor as PropertyDescriptor;
			if (item != null && propertyDescriptor != null && ValueTable.ShouldCache(item, propertyDescriptor))
			{
				this._valueTable.RegisterForChanges(item, propertyDescriptor, this);
			}
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x00216611 File Offset: 0x00214811
		internal void ScheduleCleanup()
		{
			if (!BaseAppContextSwitches.EnableCleanupSchedulingImprovements)
			{
				if (Interlocked.Increment(ref this._cleanupRequests) == 1)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new DispatcherOperationCallback(this.CleanupOperation), null);
					return;
				}
			}
			else
			{
				this._cleanupHelper.ScheduleCleanup();
			}
		}

		// Token: 0x060074C0 RID: 29888 RVA: 0x0021664E File Offset: 0x0021484E
		private bool DoCleanup(bool forceCleanup)
		{
			return (this.CleanupEnabled || forceCleanup) && this.DoCleanup();
		}

		// Token: 0x060074C1 RID: 29889 RVA: 0x00216662 File Offset: 0x00214862
		internal bool Cleanup()
		{
			if (!BaseAppContextSwitches.EnableCleanupSchedulingImprovements)
			{
				return this.DoCleanup();
			}
			return this._cleanupHelper.DoCleanup(true);
		}

		// Token: 0x060074C2 RID: 29890 RVA: 0x00216680 File Offset: 0x00214880
		private bool DoCleanup()
		{
			bool flag = false;
			if (!this.IsShutDown)
			{
				flag = (this._viewManager.Purge() || flag);
				flag = (WeakEventManager.Cleanup() || flag);
				flag = (this._valueTable.Purge() || flag);
				flag = (this._commitManager.Purge() || flag);
			}
			return flag;
		}

		// Token: 0x060074C3 RID: 29891 RVA: 0x002166CC File Offset: 0x002148CC
		internal DataBindOperation Marshal(DispatcherOperationCallback method, object arg, int cost = 1)
		{
			DataBindOperation dataBindOperation = new DataBindOperation(method, arg, cost);
			object crossThreadQueueLock = this._crossThreadQueueLock;
			lock (crossThreadQueueLock)
			{
				this._crossThreadQueue.Enqueue(dataBindOperation);
				this._crossThreadCost += cost;
				if (this._crossThreadDispatcherOperation == null)
				{
					this._crossThreadDispatcherOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(this.ProcessCrossThreadRequests));
				}
			}
			return dataBindOperation;
		}

		// Token: 0x060074C4 RID: 29892 RVA: 0x00216750 File Offset: 0x00214950
		internal void ChangeCost(DataBindOperation op, int delta)
		{
			object crossThreadQueueLock = this._crossThreadQueueLock;
			lock (crossThreadQueueLock)
			{
				op.Cost += delta;
				this._crossThreadCost += delta;
			}
		}

		// Token: 0x060074C5 RID: 29893 RVA: 0x002167A8 File Offset: 0x002149A8
		private void ProcessCrossThreadRequests()
		{
			if (this.IsShutDown)
			{
				return;
			}
			try
			{
				long ticks = DateTime.Now.Ticks;
				do
				{
					object crossThreadQueueLock = this._crossThreadQueueLock;
					DataBindOperation dataBindOperation;
					lock (crossThreadQueueLock)
					{
						if (this._crossThreadQueue.Count > 0)
						{
							dataBindOperation = this._crossThreadQueue.Dequeue();
							this._crossThreadCost -= dataBindOperation.Cost;
						}
						else
						{
							dataBindOperation = null;
						}
					}
					if (dataBindOperation == null)
					{
						break;
					}
					dataBindOperation.Invoke();
				}
				while (DateTime.Now.Ticks - ticks <= 50000L);
			}
			finally
			{
				object crossThreadQueueLock2 = this._crossThreadQueueLock;
				lock (crossThreadQueueLock2)
				{
					if (this._crossThreadQueue.Count > 0)
					{
						this._crossThreadDispatcherOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(this.ProcessCrossThreadRequests));
					}
					else
					{
						this._crossThreadDispatcherOperation = null;
						this._crossThreadCost = 0;
					}
				}
			}
		}

		// Token: 0x060074C6 RID: 29894 RVA: 0x002168C8 File Offset: 0x00214AC8
		private void RequestRun()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new DispatcherOperationCallback(this.Run), false);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.Run), true);
		}

		// Token: 0x060074C7 RID: 29895 RVA: 0x00216908 File Offset: 0x00214B08
		private object CleanupOperation(object arg)
		{
			Interlocked.Exchange(ref this._cleanupRequests, 0);
			if (!this._cleanupEnabled)
			{
				return null;
			}
			this.Cleanup();
			return null;
		}

		// Token: 0x060074C8 RID: 29896 RVA: 0x0021692C File Offset: 0x00214B2C
		private void OnShutDown()
		{
			this._viewManager = null;
			this._commitManager = null;
			this._valueConverterTable = null;
			this._mostRecentTask = null;
			this._head = (this._tail = null);
			this._crossThreadQueue.Clear();
			HybridDictionary hybridDictionary = Interlocked.Exchange<HybridDictionary>(ref this._asyncDispatchers, null);
			if (hybridDictionary != null)
			{
				foreach (object obj in hybridDictionary.Keys)
				{
					IAsyncDataDispatcher asyncDataDispatcher = obj as IAsyncDataDispatcher;
					if (asyncDataDispatcher != null)
					{
						asyncDataDispatcher.CancelAllRequests();
					}
				}
			}
			this._defaultAsyncDataDispatcher = null;
		}

		// Token: 0x060074C9 RID: 29897 RVA: 0x002169E0 File Offset: 0x00214BE0
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.Run(false);
		}

		// Token: 0x040037E4 RID: 14308
		private HybridDictionary _mostRecentTask;

		// Token: 0x040037E5 RID: 14309
		private DataBindEngine.Task _head;

		// Token: 0x040037E6 RID: 14310
		private DataBindEngine.Task _tail;

		// Token: 0x040037E7 RID: 14311
		private UIElement _layoutElement;

		// Token: 0x040037E8 RID: 14312
		private ViewManager _viewManager = new ViewManager();

		// Token: 0x040037E9 RID: 14313
		private CommitManager _commitManager = new CommitManager();

		// Token: 0x040037EA RID: 14314
		private DataBindEngine.ValueConverterTable _valueConverterTable = new DataBindEngine.ValueConverterTable();

		// Token: 0x040037EB RID: 14315
		private PathParser _pathParser = new PathParser();

		// Token: 0x040037EC RID: 14316
		private IAsyncDataDispatcher _defaultAsyncDataDispatcher;

		// Token: 0x040037ED RID: 14317
		private HybridDictionary _asyncDispatchers;

		// Token: 0x040037EE RID: 14318
		private ValueConverterContext _valueConverterContext = new ValueConverterContext();

		// Token: 0x040037EF RID: 14319
		private bool _cleanupEnabled = true;

		// Token: 0x040037F0 RID: 14320
		private ValueTable _valueTable = new ValueTable();

		// Token: 0x040037F1 RID: 14321
		private AccessorTable _accessorTable = new AccessorTable();

		// Token: 0x040037F2 RID: 14322
		private int _cleanupRequests;

		// Token: 0x040037F3 RID: 14323
		private CleanupHelper _cleanupHelper;

		// Token: 0x040037F4 RID: 14324
		private Queue<DataBindOperation> _crossThreadQueue = new Queue<DataBindOperation>();

		// Token: 0x040037F5 RID: 14325
		private object _crossThreadQueueLock = new object();

		// Token: 0x040037F6 RID: 14326
		private int _crossThreadCost;

		// Token: 0x040037F7 RID: 14327
		private DispatcherOperation _crossThreadDispatcherOperation;

		// Token: 0x040037F8 RID: 14328
		internal const int CrossThreadThreshold = 50000;

		// Token: 0x040037F9 RID: 14329
		[ThreadStatic]
		private static DataBindEngine _currentEngine;

		// Token: 0x02000B4B RID: 2891
		private class Task
		{
			// Token: 0x06008DB0 RID: 36272 RVA: 0x00259FC6 File Offset: 0x002581C6
			public Task(IDataBindEngineClient c, TaskOps o, DataBindEngine.Task previousForClient)
			{
				this.client = c;
				this.op = o;
				this.PreviousForClient = previousForClient;
				this.status = DataBindEngine.Task.Status.Pending;
			}

			// Token: 0x06008DB1 RID: 36273 RVA: 0x00259FEC File Offset: 0x002581EC
			public void Run(bool lastChance)
			{
				this.status = DataBindEngine.Task.Status.Running;
				DataBindEngine.Task.Status status = DataBindEngine.Task.Status.Completed;
				switch (this.op)
				{
				case TaskOps.TransferValue:
					this.client.TransferValue();
					break;
				case TaskOps.UpdateValue:
					this.client.UpdateValue();
					break;
				case TaskOps.AttachToContext:
					if (!this.client.AttachToContext(lastChance) && !lastChance)
					{
						status = DataBindEngine.Task.Status.Retry;
					}
					break;
				case TaskOps.VerifySourceReference:
					this.client.VerifySourceReference(lastChance);
					break;
				case TaskOps.RaiseTargetUpdatedEvent:
					this.client.OnTargetUpdated();
					break;
				}
				this.status = status;
			}

			// Token: 0x04004ADD RID: 19165
			public IDataBindEngineClient client;

			// Token: 0x04004ADE RID: 19166
			public TaskOps op;

			// Token: 0x04004ADF RID: 19167
			public DataBindEngine.Task.Status status;

			// Token: 0x04004AE0 RID: 19168
			public DataBindEngine.Task Next;

			// Token: 0x04004AE1 RID: 19169
			public DataBindEngine.Task PreviousForClient;

			// Token: 0x02000BBD RID: 3005
			public enum Status
			{
				// Token: 0x04004F05 RID: 20229
				Pending,
				// Token: 0x04004F06 RID: 20230
				Running,
				// Token: 0x04004F07 RID: 20231
				Completed,
				// Token: 0x04004F08 RID: 20232
				Retry,
				// Token: 0x04004F09 RID: 20233
				Cancelled
			}
		}

		// Token: 0x02000B4C RID: 2892
		private class ValueConverterTable : Hashtable
		{
			// Token: 0x17001F81 RID: 8065
			public IValueConverter this[Type sourceType, Type targetType, bool targetToSource]
			{
				get
				{
					DataBindEngine.ValueConverterTable.Key key = new DataBindEngine.ValueConverterTable.Key(sourceType, targetType, targetToSource);
					object obj = base[key];
					return (IValueConverter)obj;
				}
			}

			// Token: 0x06008DB3 RID: 36275 RVA: 0x0025A0A2 File Offset: 0x002582A2
			public void Add(Type sourceType, Type targetType, bool targetToSource, IValueConverter value)
			{
				base.Add(new DataBindEngine.ValueConverterTable.Key(sourceType, targetType, targetToSource), value);
			}

			// Token: 0x02000BBE RID: 3006
			private struct Key
			{
				// Token: 0x06009209 RID: 37385 RVA: 0x0025F9B2 File Offset: 0x0025DBB2
				public Key(Type sourceType, Type targetType, bool targetToSource)
				{
					this._sourceType = sourceType;
					this._targetType = targetType;
					this._targetToSource = targetToSource;
				}

				// Token: 0x0600920A RID: 37386 RVA: 0x0025F9C9 File Offset: 0x0025DBC9
				public override int GetHashCode()
				{
					return this._sourceType.GetHashCode() + this._targetType.GetHashCode();
				}

				// Token: 0x0600920B RID: 37387 RVA: 0x0025F9E2 File Offset: 0x0025DBE2
				public override bool Equals(object o)
				{
					return o is DataBindEngine.ValueConverterTable.Key && this == (DataBindEngine.ValueConverterTable.Key)o;
				}

				// Token: 0x0600920C RID: 37388 RVA: 0x0025F9FF File Offset: 0x0025DBFF
				public static bool operator ==(DataBindEngine.ValueConverterTable.Key k1, DataBindEngine.ValueConverterTable.Key k2)
				{
					return k1._sourceType == k2._sourceType && k1._targetType == k2._targetType && k1._targetToSource == k2._targetToSource;
				}

				// Token: 0x0600920D RID: 37389 RVA: 0x0025FA37 File Offset: 0x0025DC37
				public static bool operator !=(DataBindEngine.ValueConverterTable.Key k1, DataBindEngine.ValueConverterTable.Key k2)
				{
					return !(k1 == k2);
				}

				// Token: 0x04004F0A RID: 20234
				private Type _sourceType;

				// Token: 0x04004F0B RID: 20235
				private Type _targetType;

				// Token: 0x04004F0C RID: 20236
				private bool _targetToSource;
			}
		}

		// Token: 0x02000B4D RID: 2893
		private sealed class DataBindEngineShutDownListener : ShutDownListener
		{
			// Token: 0x06008DB5 RID: 36277 RVA: 0x002579C2 File Offset: 0x00255BC2
			[SecurityCritical]
			[SecurityTreatAsSafe]
			public DataBindEngineShutDownListener(DataBindEngine target) : base(target)
			{
			}

			// Token: 0x06008DB6 RID: 36278 RVA: 0x0025A0C4 File Offset: 0x002582C4
			internal override void OnShutDown(object target, object sender, EventArgs e)
			{
				DataBindEngine dataBindEngine = (DataBindEngine)target;
				dataBindEngine.OnShutDown();
			}
		}
	}
}
