using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000158 RID: 344
	internal abstract class ODataReaderCore : ODataReader
	{
		// Token: 0x0600094A RID: 2378 RVA: 0x0001D31C File Offset: 0x0001B51C
		protected ODataReaderCore(ODataInputContext inputContext, bool readingFeed, IODataReaderWriterListener listener)
		{
			this.inputContext = inputContext;
			this.readingFeed = readingFeed;
			this.listener = listener;
			this.currentEntryDepth = 0;
			if (this.readingFeed && this.inputContext.Model.IsUserModel())
			{
				this.feedValidator = new FeedWithoutExpectedTypeValidator();
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0001D37B File Offset: 0x0001B57B
		public sealed override ODataReaderState State
		{
			get
			{
				this.inputContext.VerifyNotDisposed();
				return this.scopes.Peek().State;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0001D398 File Offset: 0x0001B598
		public sealed override ODataItem Item
		{
			get
			{
				this.inputContext.VerifyNotDisposed();
				return this.scopes.Peek().Item;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0001D3B5 File Offset: 0x0001B5B5
		protected ODataEntry CurrentEntry
		{
			get
			{
				return (ODataEntry)this.Item;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x0001D3C2 File Offset: 0x0001B5C2
		protected ODataFeed CurrentFeed
		{
			get
			{
				return (ODataFeed)this.Item;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x0001D3CF File Offset: 0x0001B5CF
		protected ODataNavigationLink CurrentNavigationLink
		{
			get
			{
				return (ODataNavigationLink)this.Item;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x0001D3DC File Offset: 0x0001B5DC
		protected ODataEntityReferenceLink CurrentEntityReferenceLink
		{
			get
			{
				return (ODataEntityReferenceLink)this.Item;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0001D3EC File Offset: 0x0001B5EC
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x0001D40B File Offset: 0x0001B60B
		protected IEdmEntityType CurrentEntityType
		{
			get
			{
				return this.scopes.Peek().EntityType;
			}
			set
			{
				this.scopes.Peek().EntityType = value;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0001D420 File Offset: 0x0001B620
		protected IEdmEntitySet CurrentEntitySet
		{
			get
			{
				return this.scopes.Peek().EntitySet;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x0001D43F File Offset: 0x0001B63F
		protected ODataReaderCore.Scope CurrentScope
		{
			get
			{
				return this.scopes.Peek();
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x0001D44C File Offset: 0x0001B64C
		protected ODataReaderCore.Scope LinkParentEntityScope
		{
			get
			{
				return this.scopes.Skip(1).First<ODataReaderCore.Scope>();
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x0001D45F File Offset: 0x0001B65F
		protected bool IsTopLevel
		{
			get
			{
				return this.scopes.Count <= 2;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x0001D474 File Offset: 0x0001B674
		protected ODataReaderCore.Scope ExpandedLinkContentParentScope
		{
			get
			{
				ODataReaderCore.Scope scope = this.scopes.Skip(1).First<ODataReaderCore.Scope>();
				if (scope.State == ODataReaderState.NavigationLinkStart)
				{
					return scope;
				}
				return null;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x0001D49F File Offset: 0x0001B69F
		protected bool IsExpandedLinkContent
		{
			get
			{
				return this.ExpandedLinkContentParentScope != null;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0001D4AD File Offset: 0x0001B6AD
		protected bool ReadingFeed
		{
			get
			{
				return this.readingFeed;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0001D4B5 File Offset: 0x0001B6B5
		protected bool IsReadingNestedPayload
		{
			get
			{
				return this.listener != null;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0001D4C3 File Offset: 0x0001B6C3
		protected FeedWithoutExpectedTypeValidator CurrentFeedValidator
		{
			get
			{
				if (this.scopes.Count != 3)
				{
					return null;
				}
				return this.feedValidator;
			}
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0001D4DB File Offset: 0x0001B6DB
		public sealed override bool Read()
		{
			this.VerifyCanRead(true);
			return this.InterceptException<bool>(new Func<bool>(this.ReadSynchronously));
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001D507 File Offset: 0x0001B707
		public sealed override Task<bool> ReadAsync()
		{
			this.VerifyCanRead(false);
			return this.ReadAsynchronously().FollowOnFaultWith(delegate(Task<bool> t)
			{
				this.EnterScope(new ODataReaderCore.Scope(ODataReaderState.Exception, null, null, null));
			});
		}

		// Token: 0x0600095E RID: 2398
		protected abstract bool ReadAtStartImplementation();

		// Token: 0x0600095F RID: 2399
		protected abstract bool ReadAtFeedStartImplementation();

		// Token: 0x06000960 RID: 2400
		protected abstract bool ReadAtFeedEndImplementation();

		// Token: 0x06000961 RID: 2401
		protected abstract bool ReadAtEntryStartImplementation();

		// Token: 0x06000962 RID: 2402
		protected abstract bool ReadAtEntryEndImplementation();

		// Token: 0x06000963 RID: 2403
		protected abstract bool ReadAtNavigationLinkStartImplementation();

		// Token: 0x06000964 RID: 2404
		protected abstract bool ReadAtNavigationLinkEndImplementation();

		// Token: 0x06000965 RID: 2405
		protected abstract bool ReadAtEntityReferenceLink();

		// Token: 0x06000966 RID: 2406 RVA: 0x0001D527 File Offset: 0x0001B727
		protected void EnterScope(ODataReaderCore.Scope scope)
		{
			this.scopes.Push(scope);
			if (this.listener != null)
			{
				if (scope.State == ODataReaderState.Exception)
				{
					this.listener.OnException();
					return;
				}
				if (scope.State == ODataReaderState.Completed)
				{
					this.listener.OnCompleted();
				}
			}
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0001D567 File Offset: 0x0001B767
		protected void ReplaceScope(ODataReaderCore.Scope scope)
		{
			this.scopes.Pop();
			this.EnterScope(scope);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0001D57C File Offset: 0x0001B77C
		protected void PopScope(ODataReaderState state)
		{
			this.scopes.Pop();
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0001D58A File Offset: 0x0001B78A
		protected void EndEntry(ODataReaderCore.Scope scope)
		{
			this.scopes.Pop();
			this.EnterScope(scope);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001D5A4 File Offset: 0x0001B7A4
		protected void ApplyEntityTypeNameFromPayload(string entityTypeNameFromPayload)
		{
			EdmTypeKind edmTypeKind;
			SerializationTypeNameAnnotation serializationTypeNameAnnotation;
			IEdmEntityTypeReference edmEntityTypeReference = (IEdmEntityTypeReference)ReaderValidationUtils.ResolvePayloadTypeNameAndComputeTargetType(EdmTypeKind.Entity, null, this.CurrentEntityType.ToTypeReference(), entityTypeNameFromPayload, this.inputContext.Model, this.inputContext.MessageReaderSettings, this.inputContext.Version, () => EdmTypeKind.Entity, out edmTypeKind, out serializationTypeNameAnnotation);
			IEdmEntityType edmEntityType = null;
			ODataEntry currentEntry = this.CurrentEntry;
			if (edmEntityTypeReference != null)
			{
				edmEntityType = edmEntityTypeReference.EntityDefinition();
				currentEntry.TypeName = edmEntityType.ODataFullName();
				if (serializationTypeNameAnnotation != null)
				{
					currentEntry.SetAnnotation<SerializationTypeNameAnnotation>(serializationTypeNameAnnotation);
				}
			}
			else if (entityTypeNameFromPayload != null)
			{
				currentEntry.TypeName = entityTypeNameFromPayload;
			}
			this.CurrentEntityType = edmEntityType;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001D64C File Offset: 0x0001B84C
		protected bool ReadSynchronously()
		{
			return this.ReadImplementation();
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001D654 File Offset: 0x0001B854
		protected virtual Task<bool> ReadAsynchronously()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadImplementation));
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001D668 File Offset: 0x0001B868
		protected void IncreaseEntryDepth()
		{
			this.currentEntryDepth++;
			if (this.currentEntryDepth > this.inputContext.MessageReaderSettings.MessageQuotas.MaxNestingDepth)
			{
				throw new ODataException(Strings.ValidationUtils_MaxDepthOfNestedEntriesExceeded(this.inputContext.MessageReaderSettings.MessageQuotas.MaxNestingDepth));
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0001D6C5 File Offset: 0x0001B8C5
		protected void DecreaseEntryDepth()
		{
			this.currentEntryDepth--;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0001D6D8 File Offset: 0x0001B8D8
		private bool ReadImplementation()
		{
			bool result;
			switch (this.State)
			{
			case ODataReaderState.Start:
				result = this.ReadAtStartImplementation();
				break;
			case ODataReaderState.FeedStart:
				result = this.ReadAtFeedStartImplementation();
				break;
			case ODataReaderState.FeedEnd:
				result = this.ReadAtFeedEndImplementation();
				break;
			case ODataReaderState.EntryStart:
				this.IncreaseEntryDepth();
				result = this.ReadAtEntryStartImplementation();
				break;
			case ODataReaderState.EntryEnd:
				this.DecreaseEntryDepth();
				result = this.ReadAtEntryEndImplementation();
				break;
			case ODataReaderState.NavigationLinkStart:
				result = this.ReadAtNavigationLinkStartImplementation();
				break;
			case ODataReaderState.NavigationLinkEnd:
				result = this.ReadAtNavigationLinkEndImplementation();
				break;
			case ODataReaderState.EntityReferenceLink:
				result = this.ReadAtEntityReferenceLink();
				break;
			case ODataReaderState.Exception:
			case ODataReaderState.Completed:
				throw new ODataException(Strings.ODataReaderCore_NoReadCallsAllowed(this.State));
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataReaderCore_ReadImplementation));
			}
			if ((this.State == ODataReaderState.EntryStart || this.State == ODataReaderState.EntryEnd) && this.Item != null)
			{
				ReaderValidationUtils.ValidateEntry(this.CurrentEntry);
			}
			return result;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0001D7C0 File Offset: 0x0001B9C0
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
					this.EnterScope(new ODataReaderCore.Scope(ODataReaderState.Exception, null, null, null));
				}
				throw;
			}
			return result;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0001D804 File Offset: 0x0001BA04
		private void VerifyCanRead(bool synchronousCall)
		{
			this.inputContext.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
			if (this.State == ODataReaderState.Exception || this.State == ODataReaderState.Completed)
			{
				throw new ODataException(Strings.ODataReaderCore_ReadOrReadAsyncCalledInInvalidState(this.State));
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0001D841 File Offset: 0x0001BA41
		private void VerifyCallAllowed(bool synchronousCall)
		{
			if (synchronousCall)
			{
				if (!this.inputContext.Synchronous)
				{
					throw new ODataException(Strings.ODataReaderCore_SyncCallOnAsyncReader);
				}
			}
			else if (this.inputContext.Synchronous)
			{
				throw new ODataException(Strings.ODataReaderCore_AsyncCallOnSyncReader);
			}
		}

		// Token: 0x04000371 RID: 881
		private readonly ODataInputContext inputContext;

		// Token: 0x04000372 RID: 882
		private readonly bool readingFeed;

		// Token: 0x04000373 RID: 883
		private readonly Stack<ODataReaderCore.Scope> scopes = new Stack<ODataReaderCore.Scope>();

		// Token: 0x04000374 RID: 884
		private readonly IODataReaderWriterListener listener;

		// Token: 0x04000375 RID: 885
		private readonly FeedWithoutExpectedTypeValidator feedValidator;

		// Token: 0x04000376 RID: 886
		private int currentEntryDepth;

		// Token: 0x02000159 RID: 345
		protected internal class Scope
		{
			// Token: 0x06000975 RID: 2421 RVA: 0x0001D876 File Offset: 0x0001BA76
			internal Scope(ODataReaderState state, ODataItem item, IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
			{
				this.state = state;
				this.item = item;
				this.EntityType = expectedEntityType;
				this.EntitySet = entitySet;
			}

			// Token: 0x17000248 RID: 584
			// (get) Token: 0x06000976 RID: 2422 RVA: 0x0001D89B File Offset: 0x0001BA9B
			internal ODataReaderState State
			{
				get
				{
					return this.state;
				}
			}

			// Token: 0x17000249 RID: 585
			// (get) Token: 0x06000977 RID: 2423 RVA: 0x0001D8A3 File Offset: 0x0001BAA3
			internal ODataItem Item
			{
				get
				{
					return this.item;
				}
			}

			// Token: 0x1700024A RID: 586
			// (get) Token: 0x06000978 RID: 2424 RVA: 0x0001D8AB File Offset: 0x0001BAAB
			// (set) Token: 0x06000979 RID: 2425 RVA: 0x0001D8B3 File Offset: 0x0001BAB3
			internal IEdmEntitySet EntitySet { get; set; }

			// Token: 0x1700024B RID: 587
			// (get) Token: 0x0600097A RID: 2426 RVA: 0x0001D8BC File Offset: 0x0001BABC
			// (set) Token: 0x0600097B RID: 2427 RVA: 0x0001D8C4 File Offset: 0x0001BAC4
			internal IEdmEntityType EntityType { get; set; }

			// Token: 0x04000378 RID: 888
			private readonly ODataReaderState state;

			// Token: 0x04000379 RID: 889
			private readonly ODataItem item;
		}
	}
}
