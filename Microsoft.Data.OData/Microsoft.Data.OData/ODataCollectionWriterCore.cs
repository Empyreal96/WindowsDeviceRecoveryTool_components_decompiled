using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x02000185 RID: 389
	internal abstract class ODataCollectionWriterCore : ODataCollectionWriter, IODataOutputInStreamErrorListener
	{
		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002445F File Offset: 0x0002265F
		protected ODataCollectionWriterCore(ODataOutputContext outputContext, IEdmTypeReference itemTypeReference) : this(outputContext, itemTypeReference, null)
		{
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002446A File Offset: 0x0002266A
		protected ODataCollectionWriterCore(ODataOutputContext outputContext, IEdmTypeReference expectedItemType, IODataReaderWriterListener listener)
		{
			this.outputContext = outputContext;
			this.expectedItemType = expectedItemType;
			this.listener = listener;
			this.scopes.Push(new ODataCollectionWriterCore.Scope(ODataCollectionWriterCore.CollectionWriterState.Start, null));
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x000244A4 File Offset: 0x000226A4
		protected ODataCollectionWriterCore.CollectionWriterState State
		{
			get
			{
				return this.scopes.Peek().State;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x000244B6 File Offset: 0x000226B6
		protected DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker
		{
			get
			{
				if (this.duplicatePropertyNamesChecker == null)
				{
					this.duplicatePropertyNamesChecker = new DuplicatePropertyNamesChecker(this.outputContext.MessageWriterSettings.WriterBehavior.AllowDuplicatePropertyNames, this.outputContext.WritingResponse);
				}
				return this.duplicatePropertyNamesChecker;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x000244F1 File Offset: 0x000226F1
		protected CollectionWithoutExpectedTypeValidator CollectionValidator
		{
			get
			{
				return this.collectionValidator;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x000244F9 File Offset: 0x000226F9
		protected IEdmTypeReference ItemTypeReference
		{
			get
			{
				return this.expectedItemType;
			}
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x00024504 File Offset: 0x00022704
		public sealed override void Flush()
		{
			this.VerifyCanFlush(true);
			try
			{
				this.FlushSynchronously();
			}
			catch
			{
				this.ReplaceScope(ODataCollectionWriterCore.CollectionWriterState.Error, null);
				throw;
			}
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00024546 File Offset: 0x00022746
		public sealed override Task FlushAsync()
		{
			this.VerifyCanFlush(false);
			return this.FlushAsynchronously().FollowOnFaultWith(delegate(Task t)
			{
				this.ReplaceScope(ODataCollectionWriterCore.CollectionWriterState.Error, null);
			});
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00024566 File Offset: 0x00022766
		public sealed override void WriteStart(ODataCollectionStart collectionStart)
		{
			this.VerifyCanWriteStart(true, collectionStart);
			this.WriteStartImplementation(collectionStart);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00024594 File Offset: 0x00022794
		public sealed override Task WriteStartAsync(ODataCollectionStart collection)
		{
			this.VerifyCanWriteStart(false, collection);
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				this.WriteStartImplementation(collection);
			});
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x000245D3 File Offset: 0x000227D3
		public sealed override void WriteItem(object item)
		{
			this.VerifyCanWriteItem(true);
			this.WriteItemImplementation(item);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00024600 File Offset: 0x00022800
		public sealed override Task WriteItemAsync(object item)
		{
			this.VerifyCanWriteItem(false);
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				this.WriteItemImplementation(item);
			});
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00024639 File Offset: 0x00022839
		public sealed override void WriteEnd()
		{
			this.VerifyCanWriteEnd(true);
			this.WriteEndImplementation();
			if (this.scopes.Peek().State == ODataCollectionWriterCore.CollectionWriterState.Completed)
			{
				this.Flush();
			}
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00024682 File Offset: 0x00022882
		public sealed override Task WriteEndAsync()
		{
			this.VerifyCanWriteEnd(false);
			return TaskUtils.GetTaskForSynchronousOperation(new Action(this.WriteEndImplementation)).FollowOnSuccessWithTask(delegate(Task task)
			{
				if (this.scopes.Peek().State == ODataCollectionWriterCore.CollectionWriterState.Completed)
				{
					return this.FlushAsync();
				}
				return TaskUtils.CompletedTask;
			});
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x000246B0 File Offset: 0x000228B0
		void IODataOutputInStreamErrorListener.OnInStreamError()
		{
			this.VerifyNotDisposed();
			if (this.State == ODataCollectionWriterCore.CollectionWriterState.Completed)
			{
				throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromCompleted(this.State.ToString(), ODataCollectionWriterCore.CollectionWriterState.Error.ToString()));
			}
			this.StartPayloadInStartState();
			this.EnterScope(ODataCollectionWriterCore.CollectionWriterState.Error, this.scopes.Peek().Item);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002470F File Offset: 0x0002290F
		protected static bool IsErrorState(ODataCollectionWriterCore.CollectionWriterState state)
		{
			return state == ODataCollectionWriterCore.CollectionWriterState.Error;
		}

		// Token: 0x06000AF8 RID: 2808
		protected abstract void VerifyNotDisposed();

		// Token: 0x06000AF9 RID: 2809
		protected abstract void FlushSynchronously();

		// Token: 0x06000AFA RID: 2810
		protected abstract Task FlushAsynchronously();

		// Token: 0x06000AFB RID: 2811
		protected abstract void StartPayload();

		// Token: 0x06000AFC RID: 2812
		protected abstract void EndPayload();

		// Token: 0x06000AFD RID: 2813
		protected abstract void StartCollection(ODataCollectionStart collectionStart);

		// Token: 0x06000AFE RID: 2814
		protected abstract void EndCollection();

		// Token: 0x06000AFF RID: 2815
		protected abstract void WriteCollectionItem(object item, IEdmTypeReference expectedItemTypeReference);

		// Token: 0x06000B00 RID: 2816 RVA: 0x00024718 File Offset: 0x00022918
		private void VerifyCanWriteStart(bool synchronousCall, ODataCollectionStart collectionStart)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataCollectionStart>(collectionStart, "collection");
			string name = collectionStart.Name;
			if (name != null && name.Length == 0)
			{
				throw new ODataException(Strings.ODataCollectionWriterCore_CollectionsMustNotHaveEmptyName);
			}
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00024794 File Offset: 0x00022994
		private void WriteStartImplementation(ODataCollectionStart collectionStart)
		{
			this.StartPayloadInStartState();
			this.EnterScope(ODataCollectionWriterCore.CollectionWriterState.Collection, collectionStart);
			this.InterceptException(delegate
			{
				if (this.expectedItemType == null)
				{
					this.collectionValidator = new CollectionWithoutExpectedTypeValidator(null);
				}
				this.StartCollection(collectionStart);
			});
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000247DA File Offset: 0x000229DA
		private void VerifyCanWriteItem(bool synchronousCall)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002481C File Offset: 0x00022A1C
		private void WriteItemImplementation(object item)
		{
			if (this.scopes.Peek().State != ODataCollectionWriterCore.CollectionWriterState.Item)
			{
				this.EnterScope(ODataCollectionWriterCore.CollectionWriterState.Item, item);
			}
			this.InterceptException(delegate
			{
				ValidationUtils.ValidateCollectionItem(item, true);
				this.WriteCollectionItem(item, this.expectedItemType);
			});
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0002486F File Offset: 0x00022A6F
		private void VerifyCanWriteEnd(bool synchronousCall)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00024905 File Offset: 0x00022B05
		private void WriteEndImplementation()
		{
			this.InterceptException(delegate
			{
				ODataCollectionWriterCore.Scope scope = this.scopes.Peek();
				switch (scope.State)
				{
				case ODataCollectionWriterCore.CollectionWriterState.Start:
				case ODataCollectionWriterCore.CollectionWriterState.Completed:
				case ODataCollectionWriterCore.CollectionWriterState.Error:
					throw new ODataException(Strings.ODataCollectionWriterCore_WriteEndCalledInInvalidState(scope.State.ToString()));
				case ODataCollectionWriterCore.CollectionWriterState.Collection:
					this.EndCollection();
					break;
				case ODataCollectionWriterCore.CollectionWriterState.Item:
					this.LeaveScope();
					this.EndCollection();
					break;
				default:
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataCollectionWriterCore_WriteEnd_UnreachableCodePath));
				}
				this.LeaveScope();
			});
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00024919 File Offset: 0x00022B19
		private void VerifyCanFlush(bool synchronousCall)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00024928 File Offset: 0x00022B28
		private void VerifyCallAllowed(bool synchronousCall)
		{
			if (synchronousCall)
			{
				if (!this.outputContext.Synchronous)
				{
					throw new ODataException(Strings.ODataCollectionWriterCore_SyncCallOnAsyncWriter);
				}
			}
			else if (this.outputContext.Synchronous)
			{
				throw new ODataException(Strings.ODataCollectionWriterCore_AsyncCallOnSyncWriter);
			}
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00024960 File Offset: 0x00022B60
		private void StartPayloadInStartState()
		{
			ODataCollectionWriterCore.Scope scope = this.scopes.Peek();
			if (scope.State == ODataCollectionWriterCore.CollectionWriterState.Start)
			{
				this.InterceptException(new Action(this.StartPayload));
			}
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00024994 File Offset: 0x00022B94
		private void InterceptException(Action action)
		{
			try
			{
				action();
			}
			catch
			{
				if (!ODataCollectionWriterCore.IsErrorState(this.State))
				{
					this.EnterScope(ODataCollectionWriterCore.CollectionWriterState.Error, this.scopes.Peek().Item);
				}
				throw;
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x000249E0 File Offset: 0x00022BE0
		private void NotifyListener(ODataCollectionWriterCore.CollectionWriterState newState)
		{
			if (this.listener != null)
			{
				if (ODataCollectionWriterCore.IsErrorState(newState))
				{
					this.listener.OnException();
					return;
				}
				if (newState == ODataCollectionWriterCore.CollectionWriterState.Completed)
				{
					this.listener.OnCompleted();
				}
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00024A28 File Offset: 0x00022C28
		private void EnterScope(ODataCollectionWriterCore.CollectionWriterState newState, object item)
		{
			this.InterceptException(delegate
			{
				this.ValidateTransition(newState);
			});
			this.scopes.Push(new ODataCollectionWriterCore.Scope(newState, item));
			this.NotifyListener(newState);
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00024A80 File Offset: 0x00022C80
		private void LeaveScope()
		{
			this.scopes.Pop();
			if (this.scopes.Count == 1)
			{
				this.scopes.Pop();
				this.scopes.Push(new ODataCollectionWriterCore.Scope(ODataCollectionWriterCore.CollectionWriterState.Completed, null));
				this.InterceptException(new Action(this.EndPayload));
				this.NotifyListener(ODataCollectionWriterCore.CollectionWriterState.Completed);
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00024ADF File Offset: 0x00022CDF
		private void ReplaceScope(ODataCollectionWriterCore.CollectionWriterState newState, ODataItem item)
		{
			this.ValidateTransition(newState);
			this.scopes.Pop();
			this.scopes.Push(new ODataCollectionWriterCore.Scope(newState, item));
			this.NotifyListener(newState);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00024B10 File Offset: 0x00022D10
		private void ValidateTransition(ODataCollectionWriterCore.CollectionWriterState newState)
		{
			if (!ODataCollectionWriterCore.IsErrorState(this.State) && ODataCollectionWriterCore.IsErrorState(newState))
			{
				return;
			}
			switch (this.State)
			{
			case ODataCollectionWriterCore.CollectionWriterState.Start:
				if (newState != ODataCollectionWriterCore.CollectionWriterState.Collection && newState != ODataCollectionWriterCore.CollectionWriterState.Completed)
				{
					throw new ODataException(Strings.ODataCollectionWriterCore_InvalidTransitionFromStart(this.State.ToString(), newState.ToString()));
				}
				break;
			case ODataCollectionWriterCore.CollectionWriterState.Collection:
				if (newState != ODataCollectionWriterCore.CollectionWriterState.Item && newState != ODataCollectionWriterCore.CollectionWriterState.Completed)
				{
					throw new ODataException(Strings.ODataCollectionWriterCore_InvalidTransitionFromCollection(this.State.ToString(), newState.ToString()));
				}
				break;
			case ODataCollectionWriterCore.CollectionWriterState.Item:
				if (newState != ODataCollectionWriterCore.CollectionWriterState.Completed)
				{
					throw new ODataException(Strings.ODataCollectionWriterCore_InvalidTransitionFromItem(this.State.ToString(), newState.ToString()));
				}
				break;
			case ODataCollectionWriterCore.CollectionWriterState.Completed:
				throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromCompleted(this.State.ToString(), newState.ToString()));
			case ODataCollectionWriterCore.CollectionWriterState.Error:
				if (newState != ODataCollectionWriterCore.CollectionWriterState.Error)
				{
					throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromError(this.State.ToString(), newState.ToString()));
				}
				break;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataCollectionWriterCore_ValidateTransition_UnreachableCodePath));
			}
		}

		// Token: 0x04000402 RID: 1026
		private readonly ODataOutputContext outputContext;

		// Token: 0x04000403 RID: 1027
		private readonly IODataReaderWriterListener listener;

		// Token: 0x04000404 RID: 1028
		private readonly Stack<ODataCollectionWriterCore.Scope> scopes = new Stack<ODataCollectionWriterCore.Scope>();

		// Token: 0x04000405 RID: 1029
		private readonly IEdmTypeReference expectedItemType;

		// Token: 0x04000406 RID: 1030
		private DuplicatePropertyNamesChecker duplicatePropertyNamesChecker;

		// Token: 0x04000407 RID: 1031
		private CollectionWithoutExpectedTypeValidator collectionValidator;

		// Token: 0x02000186 RID: 390
		internal enum CollectionWriterState
		{
			// Token: 0x04000409 RID: 1033
			Start,
			// Token: 0x0400040A RID: 1034
			Collection,
			// Token: 0x0400040B RID: 1035
			Item,
			// Token: 0x0400040C RID: 1036
			Completed,
			// Token: 0x0400040D RID: 1037
			Error
		}

		// Token: 0x02000187 RID: 391
		private sealed class Scope
		{
			// Token: 0x06000B12 RID: 2834 RVA: 0x00024C50 File Offset: 0x00022E50
			public Scope(ODataCollectionWriterCore.CollectionWriterState state, object item)
			{
				this.state = state;
				this.item = item;
			}

			// Token: 0x17000294 RID: 660
			// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00024C66 File Offset: 0x00022E66
			public ODataCollectionWriterCore.CollectionWriterState State
			{
				get
				{
					return this.state;
				}
			}

			// Token: 0x17000295 RID: 661
			// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00024C6E File Offset: 0x00022E6E
			public object Item
			{
				get
				{
					return this.item;
				}
			}

			// Token: 0x0400040E RID: 1038
			private readonly ODataCollectionWriterCore.CollectionWriterState state;

			// Token: 0x0400040F RID: 1039
			private readonly object item;
		}
	}
}
