using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Management
{
	/// <summary>Retrieves a collection of management objects based on a specified query. This class is one of the more commonly used entry points to retrieving management information. For example, it can be used to enumerate all disk drives, network adapters, processes and many more management objects on a system, or to query for all network connections that are up, services that are paused, and so on.  When instantiated, an instance of this class takes as input a WMI query represented in an <see cref="T:System.Management.ObjectQuery" /> or its derivatives, and optionally a <see cref="T:System.Management.ManagementScope" /> representing the WMI namespace to execute the query in. It can also take additional advanced options in an <see cref="T:System.Management.EnumerationOptions" />. When the <see cref="M:System.Management.ManagementObjectSearcher.Get" /> method on this object                   is invoked, the <see cref="T:System.Management.ManagementObjectSearcher" /> executes the given query in the specified scope and returns a collection of management objects that match the query in a <see cref="T:System.Management.ManagementObjectCollection" />. </summary>
	// Token: 0x02000020 RID: 32
	[ToolboxItem(false)]
	public class ManagementObjectSearcher : Component
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementObjectSearcher" /> class. After some properties on this object are set, the object can be used to invoke a query for management information. This is the default constructor.          </summary>
		// Token: 0x060000FE RID: 254 RVA: 0x00007316 File Offset: 0x00005516
		public ManagementObjectSearcher() : this(null, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementObjectSearcher" /> class used to invoke the specified query for management information.          </summary>
		/// <param name="queryString">The WMI query to be invoked by the object.</param>
		// Token: 0x060000FF RID: 255 RVA: 0x00007321 File Offset: 0x00005521
		public ManagementObjectSearcher(string queryString) : this(null, new ObjectQuery(queryString), null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementObjectSearcher" /> class used to invoke the specified query for management information.          </summary>
		/// <param name="query">An <see cref="T:System.Management.ObjectQuery" /> representing the query to be invoked by the searcher.</param>
		// Token: 0x06000100 RID: 256 RVA: 0x00007331 File Offset: 0x00005531
		public ManagementObjectSearcher(ObjectQuery query) : this(null, query, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementObjectSearcher" /> class used to invoke the specified query in the specified scope.          </summary>
		/// <param name="scope">The scope in which to query.</param>
		/// <param name="queryString">The query to be invoked.  </param>
		// Token: 0x06000101 RID: 257 RVA: 0x0000733C File Offset: 0x0000553C
		public ManagementObjectSearcher(string scope, string queryString) : this(new ManagementScope(scope), new ObjectQuery(queryString), null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementObjectSearcher" /> class used to invoke the specified query in the specified scope.          </summary>
		/// <param name="scope">A <see cref="T:System.Management.ManagementScope" /> representing the scope in which to invoke the query.</param>
		/// <param name="query">An <see cref="T:System.Management.ObjectQuery" /> representing the query to be invoked. </param>
		// Token: 0x06000102 RID: 258 RVA: 0x00007351 File Offset: 0x00005551
		public ManagementObjectSearcher(ManagementScope scope, ObjectQuery query) : this(scope, query, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementObjectSearcher" /> class used to invoke the specified query, in the specified scope, and with the specified options.          </summary>
		/// <param name="scope">The scope in which the query should be invoked.</param>
		/// <param name="queryString">The query to be invoked. </param>
		/// <param name="options">An <see cref="T:System.Management.EnumerationOptions" /> specifying additional options for the query.  </param>
		// Token: 0x06000103 RID: 259 RVA: 0x0000735C File Offset: 0x0000555C
		public ManagementObjectSearcher(string scope, string queryString, EnumerationOptions options) : this(new ManagementScope(scope), new ObjectQuery(queryString), options)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.ManagementObjectSearcher" /> class to be used to invoke the specified query in the specified scope, with the specified options.          </summary>
		/// <param name="scope">A <see cref="T:System.Management.ManagementScope" /> specifying the scope of the query.</param>
		/// <param name="query">An <see cref="T:System.Management.ObjectQuery" /> specifying the query to be invoked. </param>
		/// <param name="options">An <see cref="T:System.Management.EnumerationOptions" /> specifying additional options to be used for the query. </param>
		// Token: 0x06000104 RID: 260 RVA: 0x00007374 File Offset: 0x00005574
		public ManagementObjectSearcher(ManagementScope scope, ObjectQuery query, EnumerationOptions options)
		{
			this.scope = ManagementScope._Clone(scope);
			if (query != null)
			{
				this.query = (ObjectQuery)query.Clone();
			}
			else
			{
				this.query = new ObjectQuery();
			}
			if (options != null)
			{
				this.options = (EnumerationOptions)options.Clone();
				return;
			}
			this.options = new EnumerationOptions();
		}

		/// <summary>Gets or sets the scope in which to look for objects (the scope represents a WMI namespace).          </summary>
		/// <returns>Returns a <see cref="T:System.Management.ManagementScope" /> that contains the scope (namespace) in which to look for the WMI objects.</returns>
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000073D4 File Offset: 0x000055D4
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000073DC File Offset: 0x000055DC
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
					this.scope = value.Clone();
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		/// <summary>Gets or sets the query to be invoked in the searcher (that is, the criteria to be applied to the search for management objects).          </summary>
		/// <returns>Returns an <see cref="T:System.Management.ObjectQuery" /> that contains the query to be invoked in the searcher.</returns>
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000073F8 File Offset: 0x000055F8
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00007400 File Offset: 0x00005600
		public ObjectQuery Query
		{
			get
			{
				return this.query;
			}
			set
			{
				if (value != null)
				{
					this.query = (ObjectQuery)value.Clone();
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		/// <summary>Gets or sets the options for how to search for objects.          </summary>
		/// <returns>Returns an <see cref="T:System.Management.EnumerationOptions" /> that contains the options for searching for WMI objects.</returns>
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00007421 File Offset: 0x00005621
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00007429 File Offset: 0x00005629
		public EnumerationOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				if (value != null)
				{
					this.options = (EnumerationOptions)value.Clone();
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		/// <summary>Invokes the specified WMI query and returns the resulting collection.          </summary>
		/// <returns>A <see cref="T:System.Management.ManagementObjectCollection" /> containing the objects that match the specified query.</returns>
		// Token: 0x0600010B RID: 267 RVA: 0x0000744C File Offset: 0x0000564C
		public ManagementObjectCollection Get()
		{
			this.Initialize();
			IEnumWbemClassObject enumWbem = null;
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			EnumerationOptions enumerationOptions = (EnumerationOptions)this.options.Clone();
			int num = 0;
			try
			{
				if (this.query.GetType() == typeof(SelectQuery) && ((SelectQuery)this.query).Condition == null && ((SelectQuery)this.query).SelectedProperties == null && this.options.EnumerateDeep)
				{
					enumerationOptions.EnsureLocatable = false;
					enumerationOptions.PrototypeOnly = false;
					if (!((SelectQuery)this.query).IsSchemaQuery)
					{
						num = this.scope.GetSecuredIWbemServicesHandler(this.scope.GetIWbemServices()).CreateInstanceEnum_(((SelectQuery)this.query).ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbem);
					}
					else
					{
						num = this.scope.GetSecuredIWbemServicesHandler(this.scope.GetIWbemServices()).CreateClassEnum_(((SelectQuery)this.query).ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbem);
					}
				}
				else
				{
					enumerationOptions.EnumerateDeep = true;
					num = this.scope.GetSecuredIWbemServicesHandler(this.scope.GetIWbemServices()).ExecQuery_(this.query.QueryLanguage, this.query.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbem);
				}
			}
			catch (COMException e)
			{
				ManagementException.ThrowWithExtendedInfo(e);
			}
			finally
			{
				securityHandler.Reset();
			}
			if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
			}
			else if (((long)num & (long)((ulong)-2147483648)) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
			return new ManagementObjectCollection(this.scope, this.options, enumWbem);
		}

		/// <summary>Invokes the WMI query asynchronously, and binds to a watcher to deliver the results.          </summary>
		/// <param name="watcher">The watcher that raises events triggered by the operation. </param>
		// Token: 0x0600010C RID: 268 RVA: 0x0000764C File Offset: 0x0000584C
		public void Get(ManagementOperationObserver watcher)
		{
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			this.Initialize();
			IWbemServices iwbemServices = this.scope.GetIWbemServices();
			EnumerationOptions enumerationOptions = (EnumerationOptions)this.options.Clone();
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(this.scope, enumerationOptions.Context);
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			int num = 0;
			try
			{
				if (this.query.GetType() == typeof(SelectQuery) && ((SelectQuery)this.query).Condition == null && ((SelectQuery)this.query).SelectedProperties == null && this.options.EnumerateDeep)
				{
					enumerationOptions.EnsureLocatable = false;
					enumerationOptions.PrototypeOnly = false;
					if (!((SelectQuery)this.query).IsSchemaQuery)
					{
						num = this.scope.GetSecuredIWbemServicesHandler(iwbemServices).CreateInstanceEnumAsync_(((SelectQuery)this.query).ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
					}
					else
					{
						num = this.scope.GetSecuredIWbemServicesHandler(iwbemServices).CreateClassEnumAsync_(((SelectQuery)this.query).ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
					}
				}
				else
				{
					enumerationOptions.EnumerateDeep = true;
					num = this.scope.GetSecuredIWbemServicesHandler(iwbemServices).ExecQueryAsync_(this.query.QueryLanguage, this.query.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
				}
			}
			catch (COMException e)
			{
				watcher.RemoveSink(newSink);
				ManagementException.ThrowWithExtendedInfo(e);
			}
			finally
			{
				securityHandler.Reset();
			}
			if (((long)num & (long)((ulong)-4096)) == (long)((ulong)-2147217408))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)-2147483648)) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007874 File Offset: 0x00005A74
		private void Initialize()
		{
			if (this.query == null)
			{
				throw new InvalidOperationException();
			}
			lock (this)
			{
				if (this.scope == null)
				{
					this.scope = ManagementScope._Clone(null);
				}
			}
			ManagementScope obj = this.scope;
			lock (obj)
			{
				if (!this.scope.IsConnected)
				{
					this.scope.Initialize();
				}
			}
		}

		// Token: 0x04000114 RID: 276
		private ManagementScope scope;

		// Token: 0x04000115 RID: 277
		private ObjectQuery query;

		// Token: 0x04000116 RID: 278
		private EnumerationOptions options;
	}
}
