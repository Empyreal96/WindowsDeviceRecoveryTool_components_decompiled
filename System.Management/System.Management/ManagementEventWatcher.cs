using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Management
{
	/// <summary>Subscribes to temporary event notifications based on a specified event query.          </summary>
	// Token: 0x02000017 RID: 23
	[ToolboxItem(false)]
	public class ManagementEventWatcher : Component
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003F0A File Offset: 0x0000210A
		private void HandleIdentifierChange(object sender, IdentifierChangedEventArgs e)
		{
			this.Stop();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementEventWatcher" /> class. For further initialization, set the properties on the object. This is the default constructor.          </summary>
		// Token: 0x0600006A RID: 106 RVA: 0x00003F12 File Offset: 0x00002112
		public ManagementEventWatcher() : this(null, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementEventWatcher" /> class when given a WMI event query.          </summary>
		/// <param name="query">An <see cref="T:System.Management.EventQuery" /> representing a WMI event query, which determines the events for which the watcher will listen.</param>
		// Token: 0x0600006B RID: 107 RVA: 0x00003F1D File Offset: 0x0000211D
		public ManagementEventWatcher(EventQuery query) : this(null, query, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementEventWatcher" /> class when given a WMI event query in the form of a string.          </summary>
		/// <param name="query"> A WMI event query, which defines the events for which the watcher will listen.</param>
		// Token: 0x0600006C RID: 108 RVA: 0x00003F28 File Offset: 0x00002128
		public ManagementEventWatcher(string query) : this(null, new EventQuery(query), null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementEventWatcher" />              class that listens for events conforming to the given WMI event query.          </summary>
		/// <param name="scope">A <see cref="T:System.Management.ManagementScope" /> representing the scope (namespace) in which the watcher will listen for events.</param>
		/// <param name="query">An <see cref="T:System.Management.EventQuery" /> representing a WMI event query, which determines the events for which the watcher will listen.  </param>
		// Token: 0x0600006D RID: 109 RVA: 0x00003F38 File Offset: 0x00002138
		public ManagementEventWatcher(ManagementScope scope, EventQuery query) : this(scope, query, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementEventWatcher" /> class that listens for events conforming to the given WMI event query. For this variant, the query and the scope are specified as strings.          </summary>
		/// <param name="scope">The management scope (namespace) in which the watcher will listen for events.</param>
		/// <param name="query">The query that defines the events for which the watcher will listen. </param>
		// Token: 0x0600006E RID: 110 RVA: 0x00003F43 File Offset: 0x00002143
		public ManagementEventWatcher(string scope, string query) : this(new ManagementScope(scope), new EventQuery(query), null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementEventWatcher" /> class that listens for events conforming to the given WMI event query, according to the specified options. For this variant, the query and the scope are specified as strings. The options object can specify options such as a time-out and context information.          </summary>
		/// <param name="scope">The management scope (namespace) in which the watcher will listen for events.</param>
		/// <param name="query">The query that defines the events for which the watcher will listen.  </param>
		/// <param name="options">An <see cref="T:System.Management.EventWatcherOptions" /> representing additional options used to watch for events. </param>
		// Token: 0x0600006F RID: 111 RVA: 0x00003F58 File Offset: 0x00002158
		public ManagementEventWatcher(string scope, string query, EventWatcherOptions options) : this(new ManagementScope(scope), new EventQuery(query), options)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementEventWatcher" /> class that listens for events conforming to the given WMI event query, according to the specified options. For this variant, the query and the scope are specified objects. The options object can specify options such as time-out and context information.          </summary>
		/// <param name="scope">A <see cref="T:System.Management.ManagementScope" /> representing the scope (namespace) in which the watcher will listen for events.</param>
		/// <param name="query">An <see cref="T:System.Management.EventQuery" /> representing a WMI event query, which determines the events for which the watcher will listen. </param>
		/// <param name="options">An <see cref="T:System.Management.EventWatcherOptions" /> representing additional options used to watch for events. </param>
		// Token: 0x06000070 RID: 112 RVA: 0x00003F70 File Offset: 0x00002170
		public ManagementEventWatcher(ManagementScope scope, EventQuery query, EventWatcherOptions options)
		{
			if (scope != null)
			{
				this.scope = ManagementScope._Clone(scope, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
			}
			else
			{
				this.scope = ManagementScope._Clone(null, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
			}
			if (query != null)
			{
				this.query = (EventQuery)query.Clone();
			}
			else
			{
				this.query = new EventQuery();
			}
			this.query.IdentifierChanged += this.HandleIdentifierChange;
			if (options != null)
			{
				this.options = (EventWatcherOptions)options.Clone();
			}
			else
			{
				this.options = new EventWatcherOptions();
			}
			this.options.IdentifierChanged += this.HandleIdentifierChange;
			this.enumWbem = null;
			this.cachedCount = 0U;
			this.cacheIndex = 0U;
			this.sink = null;
			this.delegateInvoker = new WmiDelegateInvoker(this);
		}

		/// <summary>Ensures that outstanding calls are cleared. This is the destructor for the object. In C#, finalizers are expressed using destructor syntax. </summary>
		// Token: 0x06000071 RID: 113 RVA: 0x00004050 File Offset: 0x00002250
		~ManagementEventWatcher()
		{
			this.Stop();
			if (this.scope != null)
			{
				this.scope.IdentifierChanged -= this.HandleIdentifierChange;
			}
			if (this.options != null)
			{
				this.options.IdentifierChanged -= this.HandleIdentifierChange;
			}
			if (this.query != null)
			{
				this.query.IdentifierChanged -= this.HandleIdentifierChange;
			}
		}

		/// <summary>Occurs when a new event arrives.</summary>
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000072 RID: 114 RVA: 0x000040DC File Offset: 0x000022DC
		// (remove) Token: 0x06000073 RID: 115 RVA: 0x00004114 File Offset: 0x00002314
		public event EventArrivedEventHandler EventArrived;

		/// <summary>Occurs when a subscription is canceled.</summary>
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000074 RID: 116 RVA: 0x0000414C File Offset: 0x0000234C
		// (remove) Token: 0x06000075 RID: 117 RVA: 0x00004184 File Offset: 0x00002384
		public event StoppedEventHandler Stopped;

		/// <summary>Gets or sets the scope in which to watch for events (namespace or scope).      </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementScope" /> that contains the scope the in which to watch for events.</returns>
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000041B9 File Offset: 0x000023B9
		// (set) Token: 0x06000077 RID: 119 RVA: 0x000041C4 File Offset: 0x000023C4
		public ManagementScope Scope
		{
			get
			{
				return this.scope;
			}
			set
			{
				if (value != null)
				{
					ManagementScope managementScope = this.scope;
					this.scope = value.Clone();
					if (managementScope != null)
					{
						managementScope.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.scope.IdentifierChanged += this.HandleIdentifierChange;
					this.HandleIdentifierChange(this, null);
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		/// <summary>Gets or sets the criteria to apply to events.      </summary>
		/// <returns>Returns an <see cref="T:System.Management.EventQuery" /> that contains the query to apply to events.</returns>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00004226 File Offset: 0x00002426
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00004230 File Offset: 0x00002430
		public EventQuery Query
		{
			get
			{
				return this.query;
			}
			set
			{
				if (value != null)
				{
					ManagementQuery managementQuery = this.query;
					this.query = (EventQuery)value.Clone();
					if (managementQuery != null)
					{
						managementQuery.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.query.IdentifierChanged += this.HandleIdentifierChange;
					this.HandleIdentifierChange(this, null);
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		/// <summary>Gets or sets the options used to watch for events. </summary>
		/// <returns>Returns an <see cref="T:System.Management.EventWatcherOptions" /> that contains the event options used to watch for events.</returns>
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00004297 File Offset: 0x00002497
		// (set) Token: 0x0600007B RID: 123 RVA: 0x000042A0 File Offset: 0x000024A0
		public EventWatcherOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				if (value != null)
				{
					EventWatcherOptions eventWatcherOptions = this.options;
					this.options = (EventWatcherOptions)value.Clone();
					if (eventWatcherOptions != null)
					{
						eventWatcherOptions.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.cachedObjects = new IWbemClassObjectFreeThreaded[this.options.BlockSize];
					this.options.IdentifierChanged += this.HandleIdentifierChange;
					this.HandleIdentifierChange(this, null);
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		/// <summary>Waits for the next event that matches the specified query to arrive, and then returns it.          </summary>
		/// <returns>A <see cref="T:System.Management.ManagementBaseObject" /> representing the newly arrived event.</returns>
		// Token: 0x0600007C RID: 124 RVA: 0x00004320 File Offset: 0x00002520
		public ManagementBaseObject WaitForNextEvent()
		{
			ManagementBaseObject result = null;
			this.Initialize();
			lock (this)
			{
				SecurityHandler securityHandler = this.Scope.GetSecurityHandler();
				int num = 0;
				try
				{
					if (this.enumWbem == null)
					{
						num = this.scope.GetSecuredIWbemServicesHandler(this.Scope.GetIWbemServices()).ExecNotificationQuery_(this.query.QueryLanguage, this.query.QueryString, this.options.Flags, this.options.GetContext(), ref this.enumWbem);
					}
					if (num >= 0)
					{
						if (this.cachedCount - this.cacheIndex == 0U)
						{
							IWbemClassObject_DoNotMarshal[] array = new IWbemClassObject_DoNotMarshal[this.options.BlockSize];
							int lTimeout = (ManagementOptions.InfiniteTimeout == this.options.Timeout) ? -1 : ((int)this.options.Timeout.TotalMilliseconds);
							num = this.scope.GetSecuredIEnumWbemClassObjectHandler(this.enumWbem).Next_(lTimeout, (uint)this.options.BlockSize, array, ref this.cachedCount);
							this.cacheIndex = 0U;
							if (num >= 0)
							{
								if (this.cachedCount == 0U)
								{
									ManagementException.ThrowWithExtendedInfo(ManagementStatus.Timedout);
								}
								int num2 = 0;
								while ((long)num2 < (long)((ulong)this.cachedCount))
								{
									this.cachedObjects[num2] = new IWbemClassObjectFreeThreaded(Marshal.GetIUnknownForObject(array[num2]));
									num2++;
								}
							}
						}
						if (num >= 0)
						{
							result = new ManagementBaseObject(this.cachedObjects[(int)this.cacheIndex]);
							this.cacheIndex += 1U;
						}
					}
				}
				finally
				{
					securityHandler.Reset();
				}
				if (num < 0)
				{
					if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			return result;
		}

		/// <summary>Subscribes to events with the given query and delivers them, asynchronously, through the <see cref="E:System.Management.ManagementEventWatcher.EventArrived" /> event.          </summary>
		// Token: 0x0600007D RID: 125 RVA: 0x00004520 File Offset: 0x00002720
		public void Start()
		{
			this.Initialize();
			this.Stop();
			SecurityHandler securityHandler = this.Scope.GetSecurityHandler();
			IWbemServices iwbemServices = this.scope.GetIWbemServices();
			try
			{
				this.sink = new SinkForEventQuery(this, this.options.Context, iwbemServices);
				if (this.sink.Status < 0)
				{
					Marshal.ThrowExceptionForHR(this.sink.Status, WmiNetUtilsHelper.GetErrorInfo_f());
				}
				int num = this.scope.GetSecuredIWbemServicesHandler(iwbemServices).ExecNotificationQueryAsync_(this.query.QueryLanguage, this.query.QueryString, 0, this.options.GetContext(), this.sink.Stub);
				if (num < 0)
				{
					if (this.sink != null)
					{
						this.sink.ReleaseStub();
						this.sink = null;
					}
					if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			finally
			{
				securityHandler.Reset();
			}
		}

		/// <summary>Cancels the subscription whether it is synchronous or asynchronous.          </summary>
		// Token: 0x0600007E RID: 126 RVA: 0x00004630 File Offset: 0x00002830
		public void Stop()
		{
			if (this.enumWbem != null)
			{
				Marshal.ReleaseComObject(this.enumWbem);
				this.enumWbem = null;
				this.FireStopped(new StoppedEventArgs(this.options.Context, 262150));
			}
			if (this.sink != null)
			{
				this.sink.Cancel();
				this.sink = null;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004690 File Offset: 0x00002890
		private void Initialize()
		{
			if (this.query == null)
			{
				throw new InvalidOperationException();
			}
			if (this.options == null)
			{
				this.Options = new EventWatcherOptions();
			}
			lock (this)
			{
				if (this.scope == null)
				{
					this.Scope = new ManagementScope();
				}
				if (this.cachedObjects == null)
				{
					this.cachedObjects = new IWbemClassObjectFreeThreaded[this.options.BlockSize];
				}
			}
			ManagementScope obj = this.scope;
			lock (obj)
			{
				this.scope.Initialize();
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000474C File Offset: 0x0000294C
		internal void FireStopped(StoppedEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.Stopped, args);
			}
			catch
			{
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004780 File Offset: 0x00002980
		internal void FireEventArrived(EventArrivedEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.EventArrived, args);
			}
			catch
			{
			}
		}

		// Token: 0x04000086 RID: 134
		private ManagementScope scope;

		// Token: 0x04000087 RID: 135
		private EventQuery query;

		// Token: 0x04000088 RID: 136
		private EventWatcherOptions options;

		// Token: 0x04000089 RID: 137
		private IEnumWbemClassObject enumWbem;

		// Token: 0x0400008A RID: 138
		private IWbemClassObjectFreeThreaded[] cachedObjects;

		// Token: 0x0400008B RID: 139
		private uint cachedCount;

		// Token: 0x0400008C RID: 140
		private uint cacheIndex;

		// Token: 0x0400008D RID: 141
		private SinkForEventQuery sink;

		// Token: 0x0400008E RID: 142
		private WmiDelegateInvoker delegateInvoker;
	}
}
