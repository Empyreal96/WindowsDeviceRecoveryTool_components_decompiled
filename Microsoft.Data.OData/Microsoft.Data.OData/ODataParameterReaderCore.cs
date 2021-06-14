using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000152 RID: 338
	internal abstract class ODataParameterReaderCore : ODataParameterReader, IODataReaderWriterListener
	{
		// Token: 0x0600091D RID: 2333 RVA: 0x0001CCF9 File Offset: 0x0001AEF9
		protected ODataParameterReaderCore(ODataInputContext inputContext, IEdmFunctionImport functionImport)
		{
			this.inputContext = inputContext;
			this.functionImport = functionImport;
			this.EnterScope(ODataParameterReaderState.Start, null, null);
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x0001CD33 File Offset: 0x0001AF33
		public sealed override ODataParameterReaderState State
		{
			get
			{
				this.inputContext.VerifyNotDisposed();
				return this.scopes.Peek().State;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x0001CD50 File Offset: 0x0001AF50
		public override string Name
		{
			get
			{
				this.inputContext.VerifyNotDisposed();
				return this.scopes.Peek().Name;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0001CD6D File Offset: 0x0001AF6D
		public override object Value
		{
			get
			{
				this.inputContext.VerifyNotDisposed();
				return this.scopes.Peek().Value;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x0001CD8A File Offset: 0x0001AF8A
		protected IEdmFunctionImport FunctionImport
		{
			get
			{
				return this.functionImport;
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001CD94 File Offset: 0x0001AF94
		public override ODataCollectionReader CreateCollectionReader()
		{
			this.VerifyCanCreateSubReader(ODataParameterReaderState.Collection);
			this.subReaderState = ODataParameterReaderCore.SubReaderState.Active;
			IEdmTypeReference elementType = ((IEdmCollectionType)this.GetParameterTypeReference(this.Name).Definition).ElementType;
			return this.CreateCollectionReader(elementType);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001CDD2 File Offset: 0x0001AFD2
		public sealed override bool Read()
		{
			this.VerifyCanRead(true);
			return this.InterceptException<bool>(new Func<bool>(this.ReadSynchronously));
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001CDF8 File Offset: 0x0001AFF8
		public sealed override Task<bool> ReadAsync()
		{
			this.VerifyCanRead(false);
			return this.ReadAsynchronously().FollowOnFaultWith(delegate(Task<bool> t)
			{
				this.EnterScope(ODataParameterReaderState.Exception, null, null);
			});
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001CE18 File Offset: 0x0001B018
		void IODataReaderWriterListener.OnException()
		{
			this.EnterScope(ODataParameterReaderState.Exception, null, null);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001CE23 File Offset: 0x0001B023
		void IODataReaderWriterListener.OnCompleted()
		{
			this.subReaderState = ODataParameterReaderCore.SubReaderState.Completed;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001CE2C File Offset: 0x0001B02C
		protected internal IEdmTypeReference GetParameterTypeReference(string parameterName)
		{
			IEdmFunctionParameter edmFunctionParameter = this.FunctionImport.FindParameter(parameterName);
			if (edmFunctionParameter == null)
			{
				throw new ODataException(Strings.ODataParameterReaderCore_ParameterNameNotInMetadata(parameterName, this.FunctionImport.Name));
			}
			return this.inputContext.EdmTypeResolver.GetParameterType(edmFunctionParameter);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001CE74 File Offset: 0x0001B074
		protected internal void EnterScope(ODataParameterReaderState state, string name, object value)
		{
			if (state == ODataParameterReaderState.Value && value != null && !(value is ODataComplexValue) && !EdmLibraryExtensions.IsPrimitiveType(value.GetType()))
			{
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterReaderCore_ValueMustBePrimitiveOrComplexOrNull));
			}
			if (this.scopes.Count == 0 || this.State != ODataParameterReaderState.Exception)
			{
				if (state == ODataParameterReaderState.Completed)
				{
					List<string> list = new List<string>();
					foreach (IEdmFunctionParameter edmFunctionParameter in this.FunctionImport.Parameters.Skip(this.FunctionImport.IsBindable ? 1 : 0))
					{
						if (!this.parametersRead.Contains(edmFunctionParameter.Name) && !this.inputContext.EdmTypeResolver.GetParameterType(edmFunctionParameter).IsNullable)
						{
							list.Add(edmFunctionParameter.Name);
						}
					}
					if (list.Count > 0)
					{
						this.scopes.Push(new ODataParameterReaderCore.Scope(ODataParameterReaderState.Exception, null, null));
						throw new ODataException(Strings.ODataParameterReaderCore_ParametersMissingInPayload(this.FunctionImport.Name, string.Join(",", list.ToArray())));
					}
				}
				else if (name != null)
				{
					if (this.parametersRead.Contains(name))
					{
						throw new ODataException(Strings.ODataParameterReaderCore_DuplicateParametersInPayload(name));
					}
					this.parametersRead.Add(name);
				}
				this.scopes.Push(new ODataParameterReaderCore.Scope(state, name, value));
			}
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001CFE0 File Offset: 0x0001B1E0
		protected internal void PopScope(ODataParameterReaderState state)
		{
			this.scopes.Pop();
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001CFEE File Offset: 0x0001B1EE
		protected void OnParameterCompleted()
		{
			this.subReaderState = ODataParameterReaderCore.SubReaderState.None;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001CFF8 File Offset: 0x0001B1F8
		protected bool ReadImplementation()
		{
			bool result;
			switch (this.State)
			{
			case ODataParameterReaderState.Start:
				result = this.ReadAtStartImplementation();
				break;
			case ODataParameterReaderState.Value:
			case ODataParameterReaderState.Collection:
				this.OnParameterCompleted();
				result = this.ReadNextParameterImplementation();
				break;
			case ODataParameterReaderState.Exception:
			case ODataParameterReaderState.Completed:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterReaderCore_ReadImplementation));
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataParameterReaderCore_ReadImplementation));
			}
			return result;
		}

		// Token: 0x0600092C RID: 2348
		protected abstract bool ReadAtStartImplementation();

		// Token: 0x0600092D RID: 2349
		protected abstract bool ReadNextParameterImplementation();

		// Token: 0x0600092E RID: 2350
		protected abstract ODataCollectionReader CreateCollectionReader(IEdmTypeReference expectedItemTypeReference);

		// Token: 0x0600092F RID: 2351 RVA: 0x0001D067 File Offset: 0x0001B267
		protected bool ReadSynchronously()
		{
			return this.ReadImplementation();
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001D06F File Offset: 0x0001B26F
		protected virtual Task<bool> ReadAsynchronously()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadImplementation));
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001D082 File Offset: 0x0001B282
		private static string GetCreateReaderMethodName(ODataParameterReaderState state)
		{
			return "Create" + state.ToString() + "Reader";
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001D0A0 File Offset: 0x0001B2A0
		private void VerifyCanCreateSubReader(ODataParameterReaderState expectedState)
		{
			this.inputContext.VerifyNotDisposed();
			if (this.State != expectedState)
			{
				throw new ODataException(Strings.ODataParameterReaderCore_InvalidCreateReaderMethodCalledForState(ODataParameterReaderCore.GetCreateReaderMethodName(expectedState), this.State));
			}
			if (this.subReaderState != ODataParameterReaderCore.SubReaderState.None)
			{
				throw new ODataException(Strings.ODataParameterReaderCore_CreateReaderAlreadyCalled(ODataParameterReaderCore.GetCreateReaderMethodName(expectedState), this.Name));
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001D0FC File Offset: 0x0001B2FC
		private T InterceptException<T>(Func<T> action)
		{
			T result;
			try
			{
				result = action();
			}
			catch (Exception e)
			{
				if (ExceptionUtils.IsCatchableExceptionType(e))
				{
					this.EnterScope(ODataParameterReaderState.Exception, null, null);
				}
				throw;
			}
			return result;
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001D138 File Offset: 0x0001B338
		private void VerifyCanRead(bool synchronousCall)
		{
			this.inputContext.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
			if (this.State == ODataParameterReaderState.Exception || this.State == ODataParameterReaderState.Completed)
			{
				throw new ODataException(Strings.ODataParameterReaderCore_ReadOrReadAsyncCalledInInvalidState(this.State));
			}
			if (this.State == ODataParameterReaderState.Collection)
			{
				if (this.subReaderState == ODataParameterReaderCore.SubReaderState.None)
				{
					throw new ODataException(Strings.ODataParameterReaderCore_SubReaderMustBeCreatedAndReadToCompletionBeforeTheNextReadOrReadAsyncCall(this.State, ODataParameterReaderCore.GetCreateReaderMethodName(this.State)));
				}
				if (this.subReaderState == ODataParameterReaderCore.SubReaderState.Active)
				{
					throw new ODataException(Strings.ODataParameterReaderCore_SubReaderMustBeInCompletedStateBeforeTheNextReadOrReadAsyncCall(this.State, ODataParameterReaderCore.GetCreateReaderMethodName(this.State)));
				}
			}
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001D1DB File Offset: 0x0001B3DB
		private void VerifyCallAllowed(bool synchronousCall)
		{
			if (synchronousCall)
			{
				this.VerifySynchronousCallAllowed();
				return;
			}
			this.VerifyAsynchronousCallAllowed();
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001D1ED File Offset: 0x0001B3ED
		private void VerifySynchronousCallAllowed()
		{
			if (!this.inputContext.Synchronous)
			{
				throw new ODataException(Strings.ODataParameterReaderCore_SyncCallOnAsyncReader);
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001D207 File Offset: 0x0001B407
		private void VerifyAsynchronousCallAllowed()
		{
			if (this.inputContext.Synchronous)
			{
				throw new ODataException(Strings.ODataParameterReaderCore_AsyncCallOnSyncReader);
			}
		}

		// Token: 0x04000364 RID: 868
		private readonly ODataInputContext inputContext;

		// Token: 0x04000365 RID: 869
		private readonly IEdmFunctionImport functionImport;

		// Token: 0x04000366 RID: 870
		private readonly Stack<ODataParameterReaderCore.Scope> scopes = new Stack<ODataParameterReaderCore.Scope>();

		// Token: 0x04000367 RID: 871
		private readonly HashSet<string> parametersRead = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x04000368 RID: 872
		private ODataParameterReaderCore.SubReaderState subReaderState;

		// Token: 0x02000153 RID: 339
		private enum SubReaderState
		{
			// Token: 0x0400036A RID: 874
			None,
			// Token: 0x0400036B RID: 875
			Active,
			// Token: 0x0400036C RID: 876
			Completed
		}

		// Token: 0x02000154 RID: 340
		protected sealed class Scope
		{
			// Token: 0x06000939 RID: 2361 RVA: 0x0001D221 File Offset: 0x0001B421
			public Scope(ODataParameterReaderState state, string name, object value)
			{
				this.state = state;
				this.name = name;
				this.value = value;
			}

			// Token: 0x17000232 RID: 562
			// (get) Token: 0x0600093A RID: 2362 RVA: 0x0001D23E File Offset: 0x0001B43E
			public ODataParameterReaderState State
			{
				get
				{
					return this.state;
				}
			}

			// Token: 0x17000233 RID: 563
			// (get) Token: 0x0600093B RID: 2363 RVA: 0x0001D246 File Offset: 0x0001B446
			public string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x17000234 RID: 564
			// (get) Token: 0x0600093C RID: 2364 RVA: 0x0001D24E File Offset: 0x0001B44E
			public object Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x0400036D RID: 877
			private readonly ODataParameterReaderState state;

			// Token: 0x0400036E RID: 878
			private readonly string name;

			// Token: 0x0400036F RID: 879
			private readonly object value;
		}
	}
}
