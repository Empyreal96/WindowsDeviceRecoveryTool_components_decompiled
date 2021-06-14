using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x0200014A RID: 330
	internal abstract class ODataCollectionReaderCore : ODataCollectionReader
	{
		// Token: 0x060008E4 RID: 2276 RVA: 0x0001C7DC File Offset: 0x0001A9DC
		protected ODataCollectionReaderCore(ODataInputContext inputContext, IEdmTypeReference expectedItemTypeReference, IODataReaderWriterListener listener)
		{
			this.inputContext = inputContext;
			this.expectedItemTypeReference = expectedItemTypeReference;
			if (this.expectedItemTypeReference == null)
			{
				this.collectionValidator = new CollectionWithoutExpectedTypeValidator(null);
			}
			this.listener = listener;
			this.EnterScope(ODataCollectionReaderState.Start, null);
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0001C82B File Offset: 0x0001AA2B
		public sealed override ODataCollectionReaderState State
		{
			get
			{
				this.inputContext.VerifyNotDisposed();
				return this.scopes.Peek().State;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0001C848 File Offset: 0x0001AA48
		public sealed override object Item
		{
			get
			{
				this.inputContext.VerifyNotDisposed();
				return this.scopes.Peek().Item;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0001C865 File Offset: 0x0001AA65
		protected bool IsCollectionElementEmpty
		{
			get
			{
				return this.scopes.Peek().IsCollectionElementEmpty;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0001C877 File Offset: 0x0001AA77
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x0001C880 File Offset: 0x0001AA80
		protected IEdmTypeReference ExpectedItemTypeReference
		{
			get
			{
				return this.expectedItemTypeReference;
			}
			set
			{
				ExceptionUtils.CheckArgumentNotNull<IEdmTypeReference>(value, "value");
				if (this.State != ODataCollectionReaderState.Start)
				{
					throw new ODataException(Strings.ODataCollectionReaderCore_ExpectedItemTypeSetInInvalidState(this.State.ToString(), ODataCollectionReaderState.Start.ToString()));
				}
				if (this.expectedItemTypeReference != value)
				{
					this.expectedItemTypeReference = value;
					this.collectionValidator = null;
				}
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0001C8DD File Offset: 0x0001AADD
		protected CollectionWithoutExpectedTypeValidator CollectionValidator
		{
			get
			{
				return this.collectionValidator;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0001C8E5 File Offset: 0x0001AAE5
		protected bool IsReadingNestedPayload
		{
			get
			{
				return this.listener != null;
			}
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001C8F3 File Offset: 0x0001AAF3
		public sealed override bool Read()
		{
			this.VerifyCanRead(true);
			return this.InterceptException<bool>(new Func<bool>(this.ReadSynchronously));
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001C918 File Offset: 0x0001AB18
		public sealed override Task<bool> ReadAsync()
		{
			this.VerifyCanRead(false);
			return this.ReadAsynchronously().FollowOnFaultWith(delegate(Task<bool> t)
			{
				this.EnterScope(ODataCollectionReaderState.Exception, null);
			});
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001C938 File Offset: 0x0001AB38
		protected bool ReadImplementation()
		{
			bool result;
			switch (this.State)
			{
			case ODataCollectionReaderState.Start:
				result = this.ReadAtStartImplementation();
				break;
			case ODataCollectionReaderState.CollectionStart:
				result = this.ReadAtCollectionStartImplementation();
				break;
			case ODataCollectionReaderState.Value:
				result = this.ReadAtValueImplementation();
				break;
			case ODataCollectionReaderState.CollectionEnd:
				result = this.ReadAtCollectionEndImplementation();
				break;
			case ODataCollectionReaderState.Exception:
			case ODataCollectionReaderState.Completed:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataCollectionReaderCore_ReadImplementation));
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataCollectionReaderCore_ReadImplementation));
			}
			return result;
		}

		// Token: 0x060008EF RID: 2287
		protected abstract bool ReadAtStartImplementation();

		// Token: 0x060008F0 RID: 2288
		protected abstract bool ReadAtCollectionStartImplementation();

		// Token: 0x060008F1 RID: 2289
		protected abstract bool ReadAtValueImplementation();

		// Token: 0x060008F2 RID: 2290
		protected abstract bool ReadAtCollectionEndImplementation();

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001C9B5 File Offset: 0x0001ABB5
		protected bool ReadSynchronously()
		{
			return this.ReadImplementation();
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001C9BD File Offset: 0x0001ABBD
		protected virtual Task<bool> ReadAsynchronously()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadImplementation));
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001C9D0 File Offset: 0x0001ABD0
		protected void EnterScope(ODataCollectionReaderState state, object item)
		{
			this.EnterScope(state, item, false);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001C9DC File Offset: 0x0001ABDC
		protected void EnterScope(ODataCollectionReaderState state, object item, bool isCollectionElementEmpty)
		{
			if (state == ODataCollectionReaderState.Value)
			{
				ValidationUtils.ValidateCollectionItem(item, true);
			}
			this.scopes.Push(new ODataCollectionReaderCore.Scope(state, item, isCollectionElementEmpty));
			if (this.listener != null)
			{
				if (state == ODataCollectionReaderState.Exception)
				{
					this.listener.OnException();
					return;
				}
				if (state == ODataCollectionReaderState.Completed)
				{
					this.listener.OnCompleted();
				}
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001CA2E File Offset: 0x0001AC2E
		protected void ReplaceScope(ODataCollectionReaderState state, object item)
		{
			if (state == ODataCollectionReaderState.Value)
			{
				ValidationUtils.ValidateCollectionItem(item, true);
			}
			this.scopes.Pop();
			this.EnterScope(state, item);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001CA4F File Offset: 0x0001AC4F
		protected void PopScope(ODataCollectionReaderState state)
		{
			this.scopes.Pop();
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001CA60 File Offset: 0x0001AC60
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
					this.EnterScope(ODataCollectionReaderState.Exception, null);
				}
				throw;
			}
			return result;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001CA9C File Offset: 0x0001AC9C
		private void VerifyCanRead(bool synchronousCall)
		{
			this.inputContext.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
			if (this.State == ODataCollectionReaderState.Exception || this.State == ODataCollectionReaderState.Completed)
			{
				throw new ODataException(Strings.ODataCollectionReaderCore_ReadOrReadAsyncCalledInInvalidState(this.State));
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001CAD8 File Offset: 0x0001ACD8
		private void VerifyCallAllowed(bool synchronousCall)
		{
			if (synchronousCall)
			{
				this.VerifySynchronousCallAllowed();
				return;
			}
			this.VerifyAsynchronousCallAllowed();
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001CAEA File Offset: 0x0001ACEA
		private void VerifySynchronousCallAllowed()
		{
			if (!this.inputContext.Synchronous)
			{
				throw new ODataException(Strings.ODataCollectionReaderCore_SyncCallOnAsyncReader);
			}
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001CB04 File Offset: 0x0001AD04
		private void VerifyAsynchronousCallAllowed()
		{
			if (this.inputContext.Synchronous)
			{
				throw new ODataException(Strings.ODataCollectionReaderCore_AsyncCallOnSyncReader);
			}
		}

		// Token: 0x04000356 RID: 854
		private readonly ODataInputContext inputContext;

		// Token: 0x04000357 RID: 855
		private readonly Stack<ODataCollectionReaderCore.Scope> scopes = new Stack<ODataCollectionReaderCore.Scope>();

		// Token: 0x04000358 RID: 856
		private readonly IODataReaderWriterListener listener;

		// Token: 0x04000359 RID: 857
		private CollectionWithoutExpectedTypeValidator collectionValidator;

		// Token: 0x0400035A RID: 858
		private IEdmTypeReference expectedItemTypeReference;

		// Token: 0x0200014B RID: 331
		protected sealed class Scope
		{
			// Token: 0x060008FF RID: 2303 RVA: 0x0001CB1E File Offset: 0x0001AD1E
			public Scope(ODataCollectionReaderState state, object item) : this(state, item, false)
			{
			}

			// Token: 0x06000900 RID: 2304 RVA: 0x0001CB29 File Offset: 0x0001AD29
			public Scope(ODataCollectionReaderState state, object item, bool isCollectionElementEmpty)
			{
				this.state = state;
				this.item = item;
				this.isCollectionElementEmpty = isCollectionElementEmpty;
				bool flag = this.isCollectionElementEmpty;
			}

			// Token: 0x17000224 RID: 548
			// (get) Token: 0x06000901 RID: 2305 RVA: 0x0001CB4D File Offset: 0x0001AD4D
			public ODataCollectionReaderState State
			{
				get
				{
					return this.state;
				}
			}

			// Token: 0x17000225 RID: 549
			// (get) Token: 0x06000902 RID: 2306 RVA: 0x0001CB55 File Offset: 0x0001AD55
			public object Item
			{
				get
				{
					return this.item;
				}
			}

			// Token: 0x17000226 RID: 550
			// (get) Token: 0x06000903 RID: 2307 RVA: 0x0001CB5D File Offset: 0x0001AD5D
			public bool IsCollectionElementEmpty
			{
				get
				{
					return this.isCollectionElementEmpty;
				}
			}

			// Token: 0x0400035B RID: 859
			private readonly ODataCollectionReaderState state;

			// Token: 0x0400035C RID: 860
			private readonly object item;

			// Token: 0x0400035D RID: 861
			private readonly bool isCollectionElementEmpty;
		}
	}
}
