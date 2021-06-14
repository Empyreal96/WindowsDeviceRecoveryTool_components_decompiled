using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Wraps and creates an object that you can use as a binding source.</summary>
	// Token: 0x020001B6 RID: 438
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class ObjectDataProvider : DataSourceProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.ObjectDataProvider" /> class.</summary>
		// Token: 0x06001C47 RID: 7239 RVA: 0x000853A4 File Offset: 0x000835A4
		public ObjectDataProvider()
		{
			this._constructorParameters = new ParameterCollection(new ParameterCollectionChanged(this.OnParametersChanged));
			this._methodParameters = new ParameterCollection(new ParameterCollectionChanged(this.OnParametersChanged));
			this._sourceDataChangedHandler = new EventHandler(this.OnSourceDataChanged);
		}

		/// <summary>Gets or sets the type of object to create an instance of.</summary>
		/// <returns>This property is <see langword="null" /> when the <see cref="T:System.Windows.Data.ObjectDataProvider" /> is uninitialized or explicitly set to null. If <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectInstance" /> is assigned, <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectType" /> returns the type of the object or null if the object is null. The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="T:System.Windows.Data.ObjectDataProvider" /> is assigned both an <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectType" /> and an <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectInstance" />; only one is allowed.</exception>
		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000853FE File Offset: 0x000835FE
		// (set) Token: 0x06001C49 RID: 7241 RVA: 0x00085408 File Offset: 0x00083608
		public Type ObjectType
		{
			get
			{
				return this._objectType;
			}
			set
			{
				if (this._mode == ObjectDataProvider.SourceMode.FromInstance)
				{
					throw new InvalidOperationException(SR.Get("ObjectDataProviderCanHaveOnlyOneSource"));
				}
				this._mode = ((value == null) ? ObjectDataProvider.SourceMode.NoSource : ObjectDataProvider.SourceMode.FromType);
				this._constructorParameters.SetReadOnly(false);
				if ((this._needNewInstance = this.SetObjectType(value)) && !base.IsRefreshDeferred)
				{
					base.Refresh();
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectType" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001C4A RID: 7242 RVA: 0x0008546D File Offset: 0x0008366D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeObjectType()
		{
			return this._mode == ObjectDataProvider.SourceMode.FromType && this.ObjectType != null;
		}

		/// <summary>Gets or sets the object used as the binding source.</summary>
		/// <returns>The instance of the object constructed from <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectType" /> and <see cref="P:System.Windows.Data.ObjectDataProvider.ConstructorParameters" />, or the <see cref="T:System.Windows.Data.DataSourceProvider" /> of which the <see cref="P:System.Windows.Data.DataSourceProvider.Data" /> is used as the <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectInstance" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="T:System.Windows.Data.ObjectDataProvider" /> is assigned both an <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectType" /> and an <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectInstance" />; only one is allowed.</exception>
		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001C4B RID: 7243 RVA: 0x00085486 File Offset: 0x00083686
		// (set) Token: 0x06001C4C RID: 7244 RVA: 0x000854A0 File Offset: 0x000836A0
		public object ObjectInstance
		{
			get
			{
				if (this._instanceProvider == null)
				{
					return this._objectInstance;
				}
				return this._instanceProvider;
			}
			set
			{
				if (this._mode == ObjectDataProvider.SourceMode.FromType)
				{
					throw new InvalidOperationException(SR.Get("ObjectDataProviderCanHaveOnlyOneSource"));
				}
				this._mode = ((value == null) ? ObjectDataProvider.SourceMode.NoSource : ObjectDataProvider.SourceMode.FromInstance);
				if (this.ObjectInstance == value)
				{
					return;
				}
				if (value != null)
				{
					this._constructorParameters.SetReadOnly(true);
					this._constructorParameters.ClearInternal();
				}
				else
				{
					this._constructorParameters.SetReadOnly(false);
				}
				value = this.TryInstanceProvider(value);
				if (this.SetObjectInstance(value) && !base.IsRefreshDeferred)
				{
					base.Refresh();
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.ObjectDataProvider.ObjectInstance" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001C4D RID: 7245 RVA: 0x00085525 File Offset: 0x00083725
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeObjectInstance()
		{
			return this._mode == ObjectDataProvider.SourceMode.FromInstance && this.ObjectInstance != null;
		}

		/// <summary>Gets or sets the name of the method to call.</summary>
		/// <returns>The name of the method to call. The default value is <see langword="null" />.</returns>
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x0008553B File Offset: 0x0008373B
		// (set) Token: 0x06001C4F RID: 7247 RVA: 0x00085543 File Offset: 0x00083743
		[DefaultValue(null)]
		public string MethodName
		{
			get
			{
				return this._methodName;
			}
			set
			{
				this._methodName = value;
				this.OnPropertyChanged("MethodName");
				if (!base.IsRefreshDeferred)
				{
					base.Refresh();
				}
			}
		}

		/// <summary>Gets the list of parameters to pass to the constructor.</summary>
		/// <returns>The list of parameters to pass to the constructor. The default value is <see langword="null" />.</returns>
		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x00085565 File Offset: 0x00083765
		public IList ConstructorParameters
		{
			get
			{
				return this._constructorParameters;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.ObjectDataProvider.ConstructorParameters" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001C51 RID: 7249 RVA: 0x0008556D File Offset: 0x0008376D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeConstructorParameters()
		{
			return this._mode == ObjectDataProvider.SourceMode.FromType && this._constructorParameters.Count > 0;
		}

		/// <summary>Gets the list of parameters to pass to the method.</summary>
		/// <returns>The list of parameters to pass to the method. The default is an empty list.</returns>
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x00085588 File Offset: 0x00083788
		public IList MethodParameters
		{
			get
			{
				return this._methodParameters;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.ObjectDataProvider.MethodParameters" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001C53 RID: 7251 RVA: 0x00085590 File Offset: 0x00083790
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeMethodParameters()
		{
			return this._methodParameters.Count > 0;
		}

		/// <summary>Gets or sets a value that indicates whether to perform object creation in a worker thread or in the active context.</summary>
		/// <returns>
		///     <see langword="true" /> to perform object creation in a worker thread; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x000855A0 File Offset: 0x000837A0
		// (set) Token: 0x06001C55 RID: 7253 RVA: 0x000855A8 File Offset: 0x000837A8
		[DefaultValue(false)]
		public bool IsAsynchronous
		{
			get
			{
				return this._isAsynchronous;
			}
			set
			{
				this._isAsynchronous = value;
				this.OnPropertyChanged("IsAsynchronous");
			}
		}

		/// <summary>Starts to create the requested object, either immediately or on a background thread, based on the value of the <see cref="P:System.Windows.Data.ObjectDataProvider.IsAsynchronous" /> property.</summary>
		// Token: 0x06001C56 RID: 7254 RVA: 0x000855BC File Offset: 0x000837BC
		protected override void BeginQuery()
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.BeginQuery(new object[]
				{
					TraceData.Identify(this),
					this.IsAsynchronous ? "asynchronous" : "synchronous"
				}));
			}
			if (this.IsAsynchronous)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.QueryWorker), null);
				return;
			}
			this.QueryWorker(null);
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00085628 File Offset: 0x00083828
		private object TryInstanceProvider(object value)
		{
			if (this._instanceProvider != null)
			{
				this._instanceProvider.DataChanged -= this._sourceDataChangedHandler;
			}
			this._instanceProvider = (value as DataSourceProvider);
			if (this._instanceProvider != null)
			{
				this._instanceProvider.DataChanged += this._sourceDataChangedHandler;
				value = this._instanceProvider.Data;
			}
			return value;
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00085681 File Offset: 0x00083881
		private bool SetObjectInstance(object value)
		{
			if (this._objectInstance == value)
			{
				return false;
			}
			this._objectInstance = value;
			this.SetObjectType((value != null) ? value.GetType() : null);
			this.OnPropertyChanged("ObjectInstance");
			return true;
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x000856B4 File Offset: 0x000838B4
		private bool SetObjectType(Type newType)
		{
			if (this._objectType != newType)
			{
				this._objectType = newType;
				this.OnPropertyChanged("ObjectType");
				return true;
			}
			return false;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000856DC File Offset: 0x000838DC
		private void QueryWorker(object obj)
		{
			object obj2 = null;
			Exception ex = null;
			if (this._mode == ObjectDataProvider.SourceMode.NoSource || this._objectType == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.ObjectDataProviderHasNoSource);
				}
				ex = new InvalidOperationException(SR.Get("ObjectDataProviderHasNoSource"));
			}
			else
			{
				Exception ex2 = null;
				if (this._needNewInstance && this._mode == ObjectDataProvider.SourceMode.FromType)
				{
					ConstructorInfo[] constructors = this._objectType.GetConstructors();
					if (constructors.Length != 0)
					{
						this._objectInstance = this.CreateObjectInstance(out ex2);
					}
					this._needNewInstance = false;
				}
				if (string.IsNullOrEmpty(this.MethodName))
				{
					obj2 = this._objectInstance;
				}
				else
				{
					obj2 = this.InvokeMethodOnInstance(out ex);
					if (ex != null && ex2 != null)
					{
						ex = ex2;
					}
				}
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.QueryFinished(new object[]
				{
					TraceData.Identify(this),
					base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
					TraceData.Identify(obj2),
					TraceData.IdentifyException(ex)
				}));
			}
			this.OnQueryFinished(obj2, ex, null, null);
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000857E4 File Offset: 0x000839E4
		private object CreateObjectInstance(out Exception e)
		{
			object result = null;
			string text = null;
			e = null;
			try
			{
				object[] array = new object[this._constructorParameters.Count];
				this._constructorParameters.CopyTo(array, 0);
				result = Activator.CreateInstance(this._objectType, BindingFlags.Default, null, array, CultureInfo.InvariantCulture);
				this.OnPropertyChanged("ObjectInstance");
			}
			catch (ArgumentException ex)
			{
				text = "Cannot create Context Affinity object.";
				e = ex;
			}
			catch (COMException ex2)
			{
				text = "Marshaling issue detected.";
				e = ex2;
			}
			catch (MissingMethodException ex3)
			{
				text = "Wrong parameters for constructor.";
				e = ex3;
			}
			catch (Exception ex4)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex4))
				{
					throw;
				}
				text = null;
				e = ex4;
			}
			catch
			{
				text = null;
				e = new InvalidOperationException(SR.Get("ObjectDataProviderNonCLSException", new object[]
				{
					this._objectType.Name
				}));
			}
			if (e != null || text != null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.ObjDPCreateFailed, this._objectType.Name, text, e);
				}
				if (!this.IsAsynchronous && text == null)
				{
					throw e;
				}
			}
			return result;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00085914 File Offset: 0x00083B14
		private object InvokeMethodOnInstance(out Exception e)
		{
			object result = null;
			string text = null;
			e = null;
			object[] array = new object[this._methodParameters.Count];
			this._methodParameters.CopyTo(array, 0);
			try
			{
				result = this._objectType.InvokeMember(this.MethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null, this._objectInstance, array, CultureInfo.InvariantCulture);
			}
			catch (ArgumentException ex)
			{
				text = "Parameter array contains a string that is a null reference.";
				e = ex;
			}
			catch (MethodAccessException ex2)
			{
				text = "The specified member is a class initializer.";
				e = ex2;
			}
			catch (MissingMethodException ex3)
			{
				text = "No method was found with matching parameter signature.";
				e = ex3;
			}
			catch (TargetException ex4)
			{
				text = "The specified member cannot be invoked on target.";
				e = ex4;
			}
			catch (AmbiguousMatchException ex5)
			{
				text = "More than one method matches the binding criteria.";
				e = ex5;
			}
			catch (Exception ex6)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex6))
				{
					throw;
				}
				text = null;
				e = ex6;
			}
			catch
			{
				text = null;
				e = new InvalidOperationException(SR.Get("ObjectDataProviderNonCLSExceptionInvoke", new object[]
				{
					this.MethodName,
					this._objectType.Name
				}));
			}
			if (e != null || text != null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.ObjDPInvokeFailed, new object[]
					{
						this.MethodName,
						this._objectType.Name,
						text,
						e
					});
				}
				if (!this.IsAsynchronous && text == null)
				{
					throw e;
				}
			}
			return result;
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00085AA4 File Offset: 0x00083CA4
		private void OnParametersChanged(ParameterCollection sender)
		{
			if (sender == this._constructorParameters)
			{
				Invariant.Assert(this._mode != ObjectDataProvider.SourceMode.FromInstance);
				this._needNewInstance = true;
			}
			if (!base.IsRefreshDeferred)
			{
				base.Refresh();
			}
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x00085AD5 File Offset: 0x00083CD5
		private void OnSourceDataChanged(object sender, EventArgs args)
		{
			Invariant.Assert(sender == this._instanceProvider);
			if (this.SetObjectInstance(this._instanceProvider.Data) && !base.IsRefreshDeferred)
			{
				base.Refresh();
			}
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x00085B06 File Offset: 0x00083D06
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x040013C4 RID: 5060
		private Type _objectType;

		// Token: 0x040013C5 RID: 5061
		private object _objectInstance;

		// Token: 0x040013C6 RID: 5062
		private string _methodName;

		// Token: 0x040013C7 RID: 5063
		private DataSourceProvider _instanceProvider;

		// Token: 0x040013C8 RID: 5064
		private ParameterCollection _constructorParameters;

		// Token: 0x040013C9 RID: 5065
		private ParameterCollection _methodParameters;

		// Token: 0x040013CA RID: 5066
		private bool _isAsynchronous;

		// Token: 0x040013CB RID: 5067
		private ObjectDataProvider.SourceMode _mode;

		// Token: 0x040013CC RID: 5068
		private bool _needNewInstance = true;

		// Token: 0x040013CD RID: 5069
		private EventHandler _sourceDataChangedHandler;

		// Token: 0x040013CE RID: 5070
		private const string s_instance = "ObjectInstance";

		// Token: 0x040013CF RID: 5071
		private const string s_type = "ObjectType";

		// Token: 0x040013D0 RID: 5072
		private const string s_method = "MethodName";

		// Token: 0x040013D1 RID: 5073
		private const string s_async = "IsAsynchronous";

		// Token: 0x040013D2 RID: 5074
		private const BindingFlags s_invokeMethodFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding;

		// Token: 0x02000883 RID: 2179
		private enum SourceMode
		{
			// Token: 0x04004159 RID: 16729
			NoSource,
			// Token: 0x0400415A RID: 16730
			FromType,
			// Token: 0x0400415B RID: 16731
			FromInstance
		}
	}
}
