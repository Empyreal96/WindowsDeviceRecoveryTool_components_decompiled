using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x020001A3 RID: 419
	internal abstract class ODataWriterCore : ODataWriter, IODataOutputInStreamErrorListener
	{
		// Token: 0x06000CBE RID: 3262 RVA: 0x0002BEC4 File Offset: 0x0002A0C4
		protected ODataWriterCore(ODataOutputContext outputContext, IEdmEntitySet entitySet, IEdmEntityType entityType, bool writingFeed)
		{
			this.outputContext = outputContext;
			this.writingFeed = writingFeed;
			if (this.writingFeed && this.outputContext.Model.IsUserModel())
			{
				this.feedValidator = new FeedWithoutExpectedTypeValidator();
			}
			if (entitySet != null && entityType == null)
			{
				entityType = this.outputContext.EdmTypeResolver.GetElementType(entitySet);
			}
			this.scopes.Push(new ODataWriterCore.Scope(ODataWriterCore.WriterState.Start, null, entitySet, entityType, false, outputContext.MessageWriterSettings.MetadataDocumentUri.SelectedProperties()));
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0002BF54 File Offset: 0x0002A154
		protected ODataWriterCore.Scope CurrentScope
		{
			get
			{
				return this.scopes.Peek();
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0002BF61 File Offset: 0x0002A161
		protected ODataWriterCore.WriterState State
		{
			get
			{
				return this.CurrentScope.State;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0002BF6E File Offset: 0x0002A16E
		protected bool SkipWriting
		{
			get
			{
				return this.CurrentScope.SkipWriting;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0002BF7B File Offset: 0x0002A17B
		protected bool IsTopLevel
		{
			get
			{
				return this.scopes.Count == 2;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0002BF8C File Offset: 0x0002A18C
		protected ODataNavigationLink ParentNavigationLink
		{
			get
			{
				ODataWriterCore.Scope parentOrNull = this.scopes.ParentOrNull;
				if (parentOrNull != null)
				{
					return parentOrNull.Item as ODataNavigationLink;
				}
				return null;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0002BFB8 File Offset: 0x0002A1B8
		protected IEdmEntityType ParentEntryEntityType
		{
			get
			{
				ODataWriterCore.Scope parent = this.scopes.Parent;
				return parent.EntityType;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0002BFD8 File Offset: 0x0002A1D8
		protected IEdmEntitySet ParentEntryEntitySet
		{
			get
			{
				ODataWriterCore.Scope parent = this.scopes.Parent;
				return parent.EntitySet;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0002BFF7 File Offset: 0x0002A1F7
		protected int FeedScopeEntryCount
		{
			get
			{
				return ((ODataWriterCore.FeedScope)this.CurrentScope).EntryCount;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0002C00C File Offset: 0x0002A20C
		protected DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker
		{
			get
			{
				ODataWriterCore.EntryScope entryScope;
				switch (this.State)
				{
				case ODataWriterCore.WriterState.Entry:
					entryScope = (ODataWriterCore.EntryScope)this.CurrentScope;
					goto IL_53;
				case ODataWriterCore.WriterState.NavigationLink:
				case ODataWriterCore.WriterState.NavigationLinkWithContent:
					entryScope = (ODataWriterCore.EntryScope)this.scopes.Parent;
					goto IL_53;
				}
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataWriterCore_DuplicatePropertyNamesChecker));
				IL_53:
				return entryScope.DuplicatePropertyNamesChecker;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x0002C072 File Offset: 0x0002A272
		protected IEdmEntityType EntryEntityType
		{
			get
			{
				return this.CurrentScope.EntityType;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0002C080 File Offset: 0x0002A280
		protected ODataWriterCore.NavigationLinkScope ParentNavigationLinkScope
		{
			get
			{
				ODataWriterCore.Scope scope = this.scopes.Parent;
				if (scope.State == ODataWriterCore.WriterState.Start)
				{
					return null;
				}
				if (scope.State == ODataWriterCore.WriterState.Feed)
				{
					scope = this.scopes.ParentOfParent;
					if (scope.State == ODataWriterCore.WriterState.Start)
					{
						return null;
					}
				}
				if (scope.State == ODataWriterCore.WriterState.NavigationLinkWithContent)
				{
					return (ODataWriterCore.NavigationLinkScope)scope;
				}
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataWriterCore_ParentNavigationLinkScope));
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x0002C0E2 File Offset: 0x0002A2E2
		private FeedWithoutExpectedTypeValidator CurrentFeedValidator
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

		// Token: 0x06000CCB RID: 3275 RVA: 0x0002C0FC File Offset: 0x0002A2FC
		public sealed override void Flush()
		{
			this.VerifyCanFlush(true);
			try
			{
				this.FlushSynchronously();
			}
			catch
			{
				this.EnterScope(ODataWriterCore.WriterState.Error, null);
				throw;
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0002C13E File Offset: 0x0002A33E
		public sealed override Task FlushAsync()
		{
			this.VerifyCanFlush(false);
			return this.FlushAsynchronously().FollowOnFaultWith(delegate(Task t)
			{
				this.EnterScope(ODataWriterCore.WriterState.Error, null);
			});
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0002C15E File Offset: 0x0002A35E
		public sealed override void WriteStart(ODataFeed feed)
		{
			this.VerifyCanWriteStartFeed(true, feed);
			this.WriteStartFeedImplementation(feed);
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0002C18C File Offset: 0x0002A38C
		public sealed override Task WriteStartAsync(ODataFeed feed)
		{
			this.VerifyCanWriteStartFeed(false, feed);
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				this.WriteStartFeedImplementation(feed);
			});
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0002C1CB File Offset: 0x0002A3CB
		public sealed override void WriteStart(ODataEntry entry)
		{
			this.VerifyCanWriteStartEntry(true, entry);
			this.WriteStartEntryImplementation(entry);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0002C1F8 File Offset: 0x0002A3F8
		public sealed override Task WriteStartAsync(ODataEntry entry)
		{
			this.VerifyCanWriteStartEntry(false, entry);
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				this.WriteStartEntryImplementation(entry);
			});
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0002C237 File Offset: 0x0002A437
		public sealed override void WriteStart(ODataNavigationLink navigationLink)
		{
			this.VerifyCanWriteStartNavigationLink(true, navigationLink);
			this.WriteStartNavigationLinkImplementation(navigationLink);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0002C264 File Offset: 0x0002A464
		public sealed override Task WriteStartAsync(ODataNavigationLink navigationLink)
		{
			this.VerifyCanWriteStartNavigationLink(false, navigationLink);
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				this.WriteStartNavigationLinkImplementation(navigationLink);
			});
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0002C2A3 File Offset: 0x0002A4A3
		public sealed override void WriteEnd()
		{
			this.VerifyCanWriteEnd(true);
			this.WriteEndImplementation();
			if (this.CurrentScope.State == ODataWriterCore.WriterState.Completed)
			{
				this.Flush();
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0002C2E2 File Offset: 0x0002A4E2
		public sealed override Task WriteEndAsync()
		{
			this.VerifyCanWriteEnd(false);
			return TaskUtils.GetTaskForSynchronousOperation(new Action(this.WriteEndImplementation)).FollowOnSuccessWithTask(delegate(Task task)
			{
				if (this.CurrentScope.State == ODataWriterCore.WriterState.Completed)
				{
					return this.FlushAsync();
				}
				return TaskUtils.CompletedTask;
			});
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0002C30D File Offset: 0x0002A50D
		public sealed override void WriteEntityReferenceLink(ODataEntityReferenceLink entityReferenceLink)
		{
			this.VerifyCanWriteEntityReferenceLink(entityReferenceLink, true);
			this.WriteEntityReferenceLinkImplementation(entityReferenceLink);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0002C33C File Offset: 0x0002A53C
		public sealed override Task WriteEntityReferenceLinkAsync(ODataEntityReferenceLink entityReferenceLink)
		{
			this.VerifyCanWriteEntityReferenceLink(entityReferenceLink, false);
			return TaskUtils.GetTaskForSynchronousOperation(delegate()
			{
				this.WriteEntityReferenceLinkImplementation(entityReferenceLink);
			});
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0002C37C File Offset: 0x0002A57C
		void IODataOutputInStreamErrorListener.OnInStreamError()
		{
			this.VerifyNotDisposed();
			if (this.State == ODataWriterCore.WriterState.Completed)
			{
				throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromCompleted(this.State.ToString(), ODataWriterCore.WriterState.Error.ToString()));
			}
			this.StartPayloadInStartState();
			this.EnterScope(ODataWriterCore.WriterState.Error, this.CurrentScope.Item);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0002C3D6 File Offset: 0x0002A5D6
		protected static bool IsErrorState(ODataWriterCore.WriterState state)
		{
			return state == ODataWriterCore.WriterState.Error;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0002C3DC File Offset: 0x0002A5DC
		protected static ProjectedPropertiesAnnotation GetProjectedPropertiesAnnotation(ODataWriterCore.Scope currentScope)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataWriterCore.Scope>(currentScope, "currentScope");
			ODataItem item = currentScope.Item;
			if (item != null)
			{
				return item.GetAnnotation<ProjectedPropertiesAnnotation>();
			}
			return null;
		}

		// Token: 0x06000CDA RID: 3290
		protected abstract void VerifyNotDisposed();

		// Token: 0x06000CDB RID: 3291
		protected abstract void FlushSynchronously();

		// Token: 0x06000CDC RID: 3292
		protected abstract Task FlushAsynchronously();

		// Token: 0x06000CDD RID: 3293
		protected abstract void StartPayload();

		// Token: 0x06000CDE RID: 3294
		protected abstract void StartEntry(ODataEntry entry);

		// Token: 0x06000CDF RID: 3295
		protected abstract void EndEntry(ODataEntry entry);

		// Token: 0x06000CE0 RID: 3296
		protected abstract void StartFeed(ODataFeed feed);

		// Token: 0x06000CE1 RID: 3297
		protected abstract void EndPayload();

		// Token: 0x06000CE2 RID: 3298
		protected abstract void EndFeed(ODataFeed feed);

		// Token: 0x06000CE3 RID: 3299
		protected abstract void WriteDeferredNavigationLink(ODataNavigationLink navigationLink);

		// Token: 0x06000CE4 RID: 3300
		protected abstract void StartNavigationLinkWithContent(ODataNavigationLink navigationLink);

		// Token: 0x06000CE5 RID: 3301
		protected abstract void EndNavigationLinkWithContent(ODataNavigationLink navigationLink);

		// Token: 0x06000CE6 RID: 3302
		protected abstract void WriteEntityReferenceInNavigationLinkContent(ODataNavigationLink parentNavigationLink, ODataEntityReferenceLink entityReferenceLink);

		// Token: 0x06000CE7 RID: 3303
		protected abstract ODataWriterCore.FeedScope CreateFeedScope(ODataFeed feed, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties);

		// Token: 0x06000CE8 RID: 3304
		protected abstract ODataWriterCore.EntryScope CreateEntryScope(ODataEntry entry, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties);

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0002C408 File Offset: 0x0002A608
		protected ODataFeedAndEntrySerializationInfo GetEntrySerializationInfo(ODataEntry entry)
		{
			ODataFeedAndEntrySerializationInfo odataFeedAndEntrySerializationInfo = (entry == null) ? null : entry.SerializationInfo;
			if (odataFeedAndEntrySerializationInfo != null)
			{
				return odataFeedAndEntrySerializationInfo;
			}
			ODataWriterCore.FeedScope feedScope = this.CurrentScope as ODataWriterCore.FeedScope;
			if (feedScope != null)
			{
				ODataFeed odataFeed = (ODataFeed)feedScope.Item;
				return odataFeed.SerializationInfo;
			}
			return null;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0002C44A File Offset: 0x0002A64A
		protected virtual ODataWriterCore.NavigationLinkScope CreateNavigationLinkScope(ODataWriterCore.WriterState writerState, ODataNavigationLink navLink, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
		{
			return new ODataWriterCore.NavigationLinkScope(writerState, navLink, entitySet, entityType, skipWriting, selectedProperties);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0002C45A File Offset: 0x0002A65A
		protected virtual void PrepareEntryForWriteStart(ODataEntry entry, ODataFeedAndEntryTypeContext typeContext, SelectedPropertiesNode selectedProperties)
		{
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0002C45C File Offset: 0x0002A65C
		protected virtual void ValidateEntryMediaResource(ODataEntry entry, IEdmEntityType entityType)
		{
			bool validateMediaResource = this.outputContext.UseDefaultFormatBehavior || this.outputContext.UseServerFormatBehavior;
			ValidationUtils.ValidateEntryMetadataResource(entry, entityType, this.outputContext.Model, validateMediaResource);
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0002C498 File Offset: 0x0002A698
		protected IEdmEntityType ValidateEntryType(ODataEntry entry)
		{
			if (entry.TypeName == null && this.CurrentScope.EntityType != null)
			{
				return this.CurrentScope.EntityType;
			}
			return (IEdmEntityType)TypeNameOracle.ResolveAndValidateTypeName(this.outputContext.Model, entry.TypeName, EdmTypeKind.Entity);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0002C4D7 File Offset: 0x0002A6D7
		protected void ValidateNoDeltaLinkForExpandedFeed(ODataFeed feed)
		{
			if (feed.DeltaLink != null)
			{
				throw new ODataException(Strings.ODataWriterCore_DeltaLinkNotSupportedOnExpandedFeed);
			}
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0002C4F2 File Offset: 0x0002A6F2
		private void VerifyCanWriteStartFeed(bool synchronousCall, ODataFeed feed)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataFeed>(feed, "feed");
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
			this.StartPayloadInStartState();
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0002C5A4 File Offset: 0x0002A7A4
		private void WriteStartFeedImplementation(ODataFeed feed)
		{
			this.CheckForNavigationLinkWithContent(ODataPayloadKind.Feed);
			this.EnterScope(ODataWriterCore.WriterState.Feed, feed);
			if (!this.SkipWriting)
			{
				this.InterceptException(delegate
				{
					if (feed.Count != null)
					{
						if (!this.IsTopLevel)
						{
							throw new ODataException(Strings.ODataWriterCore_OnlyTopLevelFeedsSupportInlineCount);
						}
						if (!this.outputContext.WritingResponse)
						{
							this.ThrowODataException(Strings.ODataWriterCore_InlineCountInRequest, feed);
						}
						ODataVersionChecker.CheckCount(this.outputContext.Version);
					}
					this.StartFeed(feed);
				});
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0002C5FA File Offset: 0x0002A7FA
		private void VerifyCanWriteStartEntry(bool synchronousCall, ODataEntry entry)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
			if (this.State != ODataWriterCore.WriterState.NavigationLink)
			{
				ExceptionUtils.CheckArgumentNotNull<ODataEntry>(entry, "entry");
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002C728 File Offset: 0x0002A928
		private void WriteStartEntryImplementation(ODataEntry entry)
		{
			this.StartPayloadInStartState();
			this.CheckForNavigationLinkWithContent(ODataPayloadKind.Entry);
			this.EnterScope(ODataWriterCore.WriterState.Entry, entry);
			if (!this.SkipWriting)
			{
				this.IncreaseEntryDepth();
				this.InterceptException(delegate
				{
					if (entry != null)
					{
						ODataWriterCore.EntryScope entryScope = (ODataWriterCore.EntryScope)this.CurrentScope;
						IEdmEntityType edmEntityType = this.ValidateEntryType(entry);
						entryScope.EntityTypeFromMetadata = entryScope.EntityType;
						ODataWriterCore.NavigationLinkScope parentNavigationLinkScope = this.ParentNavigationLinkScope;
						if (parentNavigationLinkScope != null)
						{
							WriterValidationUtils.ValidateEntryInExpandedLink(edmEntityType, parentNavigationLinkScope.EntityType);
							entryScope.EntityTypeFromMetadata = parentNavigationLinkScope.EntityType;
						}
						else if (this.CurrentFeedValidator != null)
						{
							this.CurrentFeedValidator.ValidateEntry(edmEntityType);
						}
						entryScope.EntityType = edmEntityType;
						this.PrepareEntryForWriteStart(entry, entryScope.GetOrCreateTypeContext(this.outputContext.Model, this.outputContext.WritingResponse), entryScope.SelectedProperties);
						this.ValidateEntryMediaResource(entry, edmEntityType);
						WriterValidationUtils.ValidateEntryAtStart(entry);
					}
					this.StartEntry(entry);
				});
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002C78A File Offset: 0x0002A98A
		private void VerifyCanWriteStartNavigationLink(bool synchronousCall, ODataNavigationLink navigationLink)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataNavigationLink>(navigationLink, "navigationLink");
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0002C7A4 File Offset: 0x0002A9A4
		private void WriteStartNavigationLinkImplementation(ODataNavigationLink navigationLink)
		{
			this.EnterScope(ODataWriterCore.WriterState.NavigationLink, navigationLink);
			ODataEntry odataEntry = (ODataEntry)this.scopes.Parent.Item;
			if (odataEntry.MetadataBuilder != null)
			{
				navigationLink.SetMetadataBuilder(odataEntry.MetadataBuilder);
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0002C7E3 File Offset: 0x0002A9E3
		private void VerifyCanWriteEnd(bool synchronousCall)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002C958 File Offset: 0x0002AB58
		private void WriteEndImplementation()
		{
			this.InterceptException(delegate
			{
				ODataWriterCore.Scope currentScope = this.CurrentScope;
				switch (currentScope.State)
				{
				case ODataWriterCore.WriterState.Start:
				case ODataWriterCore.WriterState.Completed:
				case ODataWriterCore.WriterState.Error:
					throw new ODataException(Strings.ODataWriterCore_WriteEndCalledInInvalidState(currentScope.State.ToString()));
				case ODataWriterCore.WriterState.Entry:
					if (!this.SkipWriting)
					{
						ODataEntry odataEntry = (ODataEntry)currentScope.Item;
						if (odataEntry != null)
						{
							WriterValidationUtils.ValidateEntryAtEnd(odataEntry);
						}
						this.EndEntry(odataEntry);
						this.DecreaseEntryDepth();
					}
					break;
				case ODataWriterCore.WriterState.Feed:
					if (!this.SkipWriting)
					{
						ODataFeed feed = (ODataFeed)currentScope.Item;
						WriterValidationUtils.ValidateFeedAtEnd(feed, !this.outputContext.WritingResponse, this.outputContext.Version);
						this.EndFeed(feed);
					}
					break;
				case ODataWriterCore.WriterState.NavigationLink:
					if (!this.outputContext.WritingResponse)
					{
						throw new ODataException(Strings.ODataWriterCore_DeferredLinkInRequest);
					}
					if (!this.SkipWriting)
					{
						ODataNavigationLink odataNavigationLink = (ODataNavigationLink)currentScope.Item;
						this.DuplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(odataNavigationLink, false, odataNavigationLink.IsCollection);
						this.WriteDeferredNavigationLink(odataNavigationLink);
						this.MarkNavigationLinkAsProcessed(odataNavigationLink);
					}
					break;
				case ODataWriterCore.WriterState.NavigationLinkWithContent:
					if (!this.SkipWriting)
					{
						ODataNavigationLink odataNavigationLink2 = (ODataNavigationLink)currentScope.Item;
						this.EndNavigationLinkWithContent(odataNavigationLink2);
						this.MarkNavigationLinkAsProcessed(odataNavigationLink2);
					}
					break;
				default:
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataWriterCore_WriteEnd_UnreachableCodePath));
				}
				this.LeaveScope();
			});
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0002C96C File Offset: 0x0002AB6C
		private void MarkNavigationLinkAsProcessed(ODataNavigationLink link)
		{
			ODataEntry odataEntry = (ODataEntry)this.scopes.Parent.Item;
			odataEntry.MetadataBuilder.MarkNavigationLinkProcessed(link.Name);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0002C9A0 File Offset: 0x0002ABA0
		private void VerifyCanWriteEntityReferenceLink(ODataEntityReferenceLink entityReferenceLink, bool synchronousCall)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataEntityReferenceLink>(entityReferenceLink, "entityReferenceLink");
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0002C9F8 File Offset: 0x0002ABF8
		private void WriteEntityReferenceLinkImplementation(ODataEntityReferenceLink entityReferenceLink)
		{
			if (this.outputContext.WritingResponse)
			{
				this.ThrowODataException(Strings.ODataWriterCore_EntityReferenceLinkInResponse, null);
			}
			this.CheckForNavigationLinkWithContent(ODataPayloadKind.EntityReferenceLink);
			if (!this.SkipWriting)
			{
				this.InterceptException(delegate
				{
					WriterValidationUtils.ValidateEntityReferenceLink(entityReferenceLink);
					this.WriteEntityReferenceInNavigationLinkContent((ODataNavigationLink)this.CurrentScope.Item, entityReferenceLink);
				});
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0002CA5A File Offset: 0x0002AC5A
		private void VerifyCanFlush(bool synchronousCall)
		{
			this.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0002CA69 File Offset: 0x0002AC69
		private void VerifyCallAllowed(bool synchronousCall)
		{
			if (synchronousCall)
			{
				if (!this.outputContext.Synchronous)
				{
					throw new ODataException(Strings.ODataWriterCore_SyncCallOnAsyncWriter);
				}
			}
			else if (this.outputContext.Synchronous)
			{
				throw new ODataException(Strings.ODataWriterCore_AsyncCallOnSyncWriter);
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0002CA9E File Offset: 0x0002AC9E
		private void ThrowODataException(string errorMessage, ODataItem item)
		{
			this.EnterScope(ODataWriterCore.WriterState.Error, item);
			throw new ODataException(errorMessage);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0002CAAE File Offset: 0x0002ACAE
		private void StartPayloadInStartState()
		{
			if (this.State == ODataWriterCore.WriterState.Start)
			{
				this.InterceptException(new Action(this.StartPayload));
			}
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0002CBCC File Offset: 0x0002ADCC
		private void CheckForNavigationLinkWithContent(ODataPayloadKind contentPayloadKind)
		{
			ODataWriterCore.Scope currentScope = this.CurrentScope;
			if (currentScope.State == ODataWriterCore.WriterState.NavigationLink || currentScope.State == ODataWriterCore.WriterState.NavigationLinkWithContent)
			{
				ODataNavigationLink currentNavigationLink = (ODataNavigationLink)currentScope.Item;
				this.InterceptException(delegate
				{
					IEdmNavigationProperty edmNavigationProperty = WriterValidationUtils.ValidateNavigationLink(currentNavigationLink, this.ParentEntryEntityType, new ODataPayloadKind?(contentPayloadKind));
					if (edmNavigationProperty != null)
					{
						this.CurrentScope.EntityType = edmNavigationProperty.ToEntityType();
						IEdmEntitySet parentEntryEntitySet = this.ParentEntryEntitySet;
						this.CurrentScope.EntitySet = ((parentEntryEntitySet == null) ? null : parentEntryEntitySet.FindNavigationTarget(edmNavigationProperty));
					}
				});
				if (currentScope.State == ODataWriterCore.WriterState.NavigationLinkWithContent)
				{
					if (this.outputContext.WritingResponse || currentNavigationLink.IsCollection != true)
					{
						this.ThrowODataException(Strings.ODataWriterCore_MultipleItemsInNavigationLinkContent, currentNavigationLink);
						return;
					}
				}
				else
				{
					this.PromoteNavigationLinkScope();
					if (!this.SkipWriting)
					{
						this.InterceptException(delegate
						{
							this.DuplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(currentNavigationLink, contentPayloadKind != ODataPayloadKind.EntityReferenceLink, new bool?(contentPayloadKind == ODataPayloadKind.Feed));
							this.StartNavigationLinkWithContent(currentNavigationLink);
						});
						return;
					}
				}
			}
			else if (contentPayloadKind == ODataPayloadKind.EntityReferenceLink)
			{
				this.ThrowODataException(Strings.ODataWriterCore_EntityReferenceLinkWithoutNavigationLink, null);
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0002CCC4 File Offset: 0x0002AEC4
		private void InterceptException(Action action)
		{
			try
			{
				action();
			}
			catch
			{
				if (!ODataWriterCore.IsErrorState(this.State))
				{
					this.EnterScope(ODataWriterCore.WriterState.Error, this.CurrentScope.Item);
				}
				throw;
			}
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0002CD0C File Offset: 0x0002AF0C
		private void IncreaseEntryDepth()
		{
			this.currentEntryDepth++;
			if (this.currentEntryDepth > this.outputContext.MessageWriterSettings.MessageQuotas.MaxNestingDepth)
			{
				this.ThrowODataException(Strings.ValidationUtils_MaxDepthOfNestedEntriesExceeded(this.outputContext.MessageWriterSettings.MessageQuotas.MaxNestingDepth), null);
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0002CD6A File Offset: 0x0002AF6A
		private void DecreaseEntryDepth()
		{
			this.currentEntryDepth--;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0002CD98 File Offset: 0x0002AF98
		private void EnterScope(ODataWriterCore.WriterState newState, ODataItem item)
		{
			this.InterceptException(delegate
			{
				this.ValidateTransition(newState);
			});
			bool flag = this.SkipWriting;
			ODataWriterCore.Scope currentScope = this.CurrentScope;
			IEdmEntitySet entitySet = null;
			IEdmEntityType entityType = null;
			SelectedPropertiesNode selectedProperties = currentScope.SelectedProperties;
			if (newState == ODataWriterCore.WriterState.Entry || newState == ODataWriterCore.WriterState.Feed)
			{
				entitySet = currentScope.EntitySet;
				entityType = currentScope.EntityType;
			}
			ODataWriterCore.WriterState state = currentScope.State;
			if (state == ODataWriterCore.WriterState.Entry && newState == ODataWriterCore.WriterState.NavigationLink)
			{
				ODataNavigationLink odataNavigationLink = (ODataNavigationLink)item;
				if (!flag)
				{
					ProjectedPropertiesAnnotation projectedPropertiesAnnotation = ODataWriterCore.GetProjectedPropertiesAnnotation(currentScope);
					flag = projectedPropertiesAnnotation.ShouldSkipProperty(odataNavigationLink.Name);
					selectedProperties = currentScope.SelectedProperties.GetSelectedPropertiesForNavigationProperty(currentScope.EntityType, odataNavigationLink.Name);
					if (this.outputContext.WritingResponse)
					{
						IEdmEntityType entityType2 = currentScope.EntityType;
						IEdmNavigationProperty edmNavigationProperty = WriterValidationUtils.ValidateNavigationLink(odataNavigationLink, entityType2, null);
						if (edmNavigationProperty != null)
						{
							entityType = edmNavigationProperty.ToEntityType();
							IEdmEntitySet entitySet2 = currentScope.EntitySet;
							entitySet = ((entitySet2 == null) ? null : entitySet2.FindNavigationTarget(edmNavigationProperty));
						}
					}
				}
			}
			else if (newState == ODataWriterCore.WriterState.Entry && state == ODataWriterCore.WriterState.Feed)
			{
				((ODataWriterCore.FeedScope)currentScope).EntryCount++;
			}
			this.PushScope(newState, item, entitySet, entityType, flag, selectedProperties);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0002CEF0 File Offset: 0x0002B0F0
		private void LeaveScope()
		{
			this.scopes.Pop();
			if (this.scopes.Count == 1)
			{
				ODataWriterCore.Scope scope = this.scopes.Pop();
				this.PushScope(ODataWriterCore.WriterState.Completed, null, scope.EntitySet, scope.EntityType, false, scope.SelectedProperties);
				this.InterceptException(new Action(this.EndPayload));
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0002CF54 File Offset: 0x0002B154
		private void PromoteNavigationLinkScope()
		{
			this.ValidateTransition(ODataWriterCore.WriterState.NavigationLinkWithContent);
			ODataWriterCore.NavigationLinkScope navigationLinkScope = (ODataWriterCore.NavigationLinkScope)this.scopes.Pop();
			ODataWriterCore.NavigationLinkScope scope = navigationLinkScope.Clone(ODataWriterCore.WriterState.NavigationLinkWithContent);
			this.scopes.Push(scope);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0002CF90 File Offset: 0x0002B190
		private void ValidateTransition(ODataWriterCore.WriterState newState)
		{
			if (!ODataWriterCore.IsErrorState(this.State) && ODataWriterCore.IsErrorState(newState))
			{
				return;
			}
			switch (this.State)
			{
			case ODataWriterCore.WriterState.Start:
				if (newState != ODataWriterCore.WriterState.Feed && newState != ODataWriterCore.WriterState.Entry)
				{
					throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromStart(this.State.ToString(), newState.ToString()));
				}
				if (newState == ODataWriterCore.WriterState.Feed && !this.writingFeed)
				{
					throw new ODataException(Strings.ODataWriterCore_CannotWriteTopLevelFeedWithEntryWriter);
				}
				if (newState == ODataWriterCore.WriterState.Entry && this.writingFeed)
				{
					throw new ODataException(Strings.ODataWriterCore_CannotWriteTopLevelEntryWithFeedWriter);
				}
				break;
			case ODataWriterCore.WriterState.Entry:
				if (this.CurrentScope.Item == null)
				{
					throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromNullEntry(this.State.ToString(), newState.ToString()));
				}
				if (newState != ODataWriterCore.WriterState.NavigationLink)
				{
					throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromEntry(this.State.ToString(), newState.ToString()));
				}
				break;
			case ODataWriterCore.WriterState.Feed:
				if (newState != ODataWriterCore.WriterState.Entry)
				{
					throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromFeed(this.State.ToString(), newState.ToString()));
				}
				break;
			case ODataWriterCore.WriterState.NavigationLink:
				if (newState != ODataWriterCore.WriterState.NavigationLinkWithContent)
				{
					throw new ODataException(Strings.ODataWriterCore_InvalidStateTransition(this.State.ToString(), newState.ToString()));
				}
				break;
			case ODataWriterCore.WriterState.NavigationLinkWithContent:
				if (newState != ODataWriterCore.WriterState.Feed && newState != ODataWriterCore.WriterState.Entry)
				{
					throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromExpandedLink(this.State.ToString(), newState.ToString()));
				}
				break;
			case ODataWriterCore.WriterState.Completed:
				throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromCompleted(this.State.ToString(), newState.ToString()));
			case ODataWriterCore.WriterState.Error:
				if (newState != ODataWriterCore.WriterState.Error)
				{
					throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromError(this.State.ToString(), newState.ToString()));
				}
				break;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataWriterCore_ValidateTransition_UnreachableCodePath));
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0002D194 File Offset: 0x0002B394
		private void PushScope(ODataWriterCore.WriterState state, ODataItem item, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
		{
			ODataWriterCore.Scope scope;
			switch (state)
			{
			case ODataWriterCore.WriterState.Start:
			case ODataWriterCore.WriterState.Completed:
			case ODataWriterCore.WriterState.Error:
				scope = new ODataWriterCore.Scope(state, item, entitySet, entityType, skipWriting, selectedProperties);
				break;
			case ODataWriterCore.WriterState.Entry:
				scope = this.CreateEntryScope((ODataEntry)item, entitySet, entityType, skipWriting, selectedProperties);
				break;
			case ODataWriterCore.WriterState.Feed:
				scope = this.CreateFeedScope((ODataFeed)item, entitySet, entityType, skipWriting, selectedProperties);
				break;
			case ODataWriterCore.WriterState.NavigationLink:
			case ODataWriterCore.WriterState.NavigationLinkWithContent:
				scope = this.CreateNavigationLinkScope(state, (ODataNavigationLink)item, entitySet, entityType, skipWriting, selectedProperties);
				break;
			default:
			{
				string message = Strings.General_InternalError(InternalErrorCodes.ODataWriterCore_Scope_Create_UnreachableCodePath);
				throw new ODataException(message);
			}
			}
			this.scopes.Push(scope);
		}

		// Token: 0x0400044D RID: 1101
		private readonly ODataOutputContext outputContext;

		// Token: 0x0400044E RID: 1102
		private readonly bool writingFeed;

		// Token: 0x0400044F RID: 1103
		private readonly ODataWriterCore.ScopeStack scopes = new ODataWriterCore.ScopeStack();

		// Token: 0x04000450 RID: 1104
		private readonly FeedWithoutExpectedTypeValidator feedValidator;

		// Token: 0x04000451 RID: 1105
		private int currentEntryDepth;

		// Token: 0x020001A4 RID: 420
		internal enum WriterState
		{
			// Token: 0x04000453 RID: 1107
			Start,
			// Token: 0x04000454 RID: 1108
			Entry,
			// Token: 0x04000455 RID: 1109
			Feed,
			// Token: 0x04000456 RID: 1110
			NavigationLink,
			// Token: 0x04000457 RID: 1111
			NavigationLinkWithContent,
			// Token: 0x04000458 RID: 1112
			Completed,
			// Token: 0x04000459 RID: 1113
			Error
		}

		// Token: 0x020001A5 RID: 421
		internal sealed class ScopeStack
		{
			// Token: 0x06000D0A RID: 3338 RVA: 0x0002D23A File Offset: 0x0002B43A
			internal ScopeStack()
			{
			}

			// Token: 0x170002C7 RID: 711
			// (get) Token: 0x06000D0B RID: 3339 RVA: 0x0002D24D File Offset: 0x0002B44D
			internal int Count
			{
				get
				{
					return this.scopes.Count;
				}
			}

			// Token: 0x170002C8 RID: 712
			// (get) Token: 0x06000D0C RID: 3340 RVA: 0x0002D25C File Offset: 0x0002B45C
			internal ODataWriterCore.Scope Parent
			{
				get
				{
					ODataWriterCore.Scope item = this.scopes.Pop();
					ODataWriterCore.Scope result = this.scopes.Peek();
					this.scopes.Push(item);
					return result;
				}
			}

			// Token: 0x170002C9 RID: 713
			// (get) Token: 0x06000D0D RID: 3341 RVA: 0x0002D290 File Offset: 0x0002B490
			internal ODataWriterCore.Scope ParentOfParent
			{
				get
				{
					ODataWriterCore.Scope item = this.scopes.Pop();
					ODataWriterCore.Scope item2 = this.scopes.Pop();
					ODataWriterCore.Scope result = this.scopes.Peek();
					this.scopes.Push(item2);
					this.scopes.Push(item);
					return result;
				}
			}

			// Token: 0x170002CA RID: 714
			// (get) Token: 0x06000D0E RID: 3342 RVA: 0x0002D2DA File Offset: 0x0002B4DA
			internal ODataWriterCore.Scope ParentOrNull
			{
				get
				{
					if (this.Count != 0)
					{
						return this.Parent;
					}
					return null;
				}
			}

			// Token: 0x06000D0F RID: 3343 RVA: 0x0002D2EC File Offset: 0x0002B4EC
			internal void Push(ODataWriterCore.Scope scope)
			{
				this.scopes.Push(scope);
			}

			// Token: 0x06000D10 RID: 3344 RVA: 0x0002D2FA File Offset: 0x0002B4FA
			internal ODataWriterCore.Scope Pop()
			{
				return this.scopes.Pop();
			}

			// Token: 0x06000D11 RID: 3345 RVA: 0x0002D307 File Offset: 0x0002B507
			internal ODataWriterCore.Scope Peek()
			{
				return this.scopes.Peek();
			}

			// Token: 0x0400045A RID: 1114
			private readonly Stack<ODataWriterCore.Scope> scopes = new Stack<ODataWriterCore.Scope>();
		}

		// Token: 0x020001A6 RID: 422
		internal class Scope
		{
			// Token: 0x06000D12 RID: 3346 RVA: 0x0002D314 File Offset: 0x0002B514
			internal Scope(ODataWriterCore.WriterState state, ODataItem item, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
			{
				this.state = state;
				this.item = item;
				this.entityType = entityType;
				this.entitySet = entitySet;
				this.skipWriting = skipWriting;
				this.selectedProperties = selectedProperties;
			}

			// Token: 0x170002CB RID: 715
			// (get) Token: 0x06000D13 RID: 3347 RVA: 0x0002D349 File Offset: 0x0002B549
			// (set) Token: 0x06000D14 RID: 3348 RVA: 0x0002D351 File Offset: 0x0002B551
			public IEdmEntityType EntityType
			{
				get
				{
					return this.entityType;
				}
				set
				{
					this.entityType = value;
				}
			}

			// Token: 0x170002CC RID: 716
			// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0002D35A File Offset: 0x0002B55A
			internal ODataWriterCore.WriterState State
			{
				get
				{
					return this.state;
				}
			}

			// Token: 0x170002CD RID: 717
			// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0002D362 File Offset: 0x0002B562
			internal ODataItem Item
			{
				get
				{
					return this.item;
				}
			}

			// Token: 0x170002CE RID: 718
			// (get) Token: 0x06000D17 RID: 3351 RVA: 0x0002D36A File Offset: 0x0002B56A
			// (set) Token: 0x06000D18 RID: 3352 RVA: 0x0002D372 File Offset: 0x0002B572
			internal IEdmEntitySet EntitySet
			{
				get
				{
					return this.entitySet;
				}
				set
				{
					this.entitySet = value;
				}
			}

			// Token: 0x170002CF RID: 719
			// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0002D37B File Offset: 0x0002B57B
			internal SelectedPropertiesNode SelectedProperties
			{
				get
				{
					return this.selectedProperties;
				}
			}

			// Token: 0x170002D0 RID: 720
			// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0002D383 File Offset: 0x0002B583
			internal bool SkipWriting
			{
				get
				{
					return this.skipWriting;
				}
			}

			// Token: 0x0400045B RID: 1115
			private readonly ODataWriterCore.WriterState state;

			// Token: 0x0400045C RID: 1116
			private readonly ODataItem item;

			// Token: 0x0400045D RID: 1117
			private readonly bool skipWriting;

			// Token: 0x0400045E RID: 1118
			private readonly SelectedPropertiesNode selectedProperties;

			// Token: 0x0400045F RID: 1119
			private IEdmEntitySet entitySet;

			// Token: 0x04000460 RID: 1120
			private IEdmEntityType entityType;
		}

		// Token: 0x020001A7 RID: 423
		internal abstract class FeedScope : ODataWriterCore.Scope
		{
			// Token: 0x06000D1B RID: 3355 RVA: 0x0002D38B File Offset: 0x0002B58B
			internal FeedScope(ODataFeed feed, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties) : base(ODataWriterCore.WriterState.Feed, feed, entitySet, entityType, skipWriting, selectedProperties)
			{
				this.serializationInfo = feed.SerializationInfo;
			}

			// Token: 0x170002D1 RID: 721
			// (get) Token: 0x06000D1C RID: 3356 RVA: 0x0002D3A7 File Offset: 0x0002B5A7
			// (set) Token: 0x06000D1D RID: 3357 RVA: 0x0002D3AF File Offset: 0x0002B5AF
			internal int EntryCount
			{
				get
				{
					return this.entryCount;
				}
				set
				{
					this.entryCount = value;
				}
			}

			// Token: 0x170002D2 RID: 722
			// (get) Token: 0x06000D1E RID: 3358 RVA: 0x0002D3B8 File Offset: 0x0002B5B8
			internal InstanceAnnotationWriteTracker InstanceAnnotationWriteTracker
			{
				get
				{
					if (this.instanceAnnotationWriteTracker == null)
					{
						this.instanceAnnotationWriteTracker = new InstanceAnnotationWriteTracker();
					}
					return this.instanceAnnotationWriteTracker;
				}
			}

			// Token: 0x06000D1F RID: 3359 RVA: 0x0002D3D3 File Offset: 0x0002B5D3
			internal ODataFeedAndEntryTypeContext GetOrCreateTypeContext(IEdmModel model, bool writingResponse)
			{
				if (this.typeContext == null)
				{
					this.typeContext = ODataFeedAndEntryTypeContext.Create(this.serializationInfo, base.EntitySet, EdmTypeWriterResolver.Instance.GetElementType(base.EntitySet), base.EntityType, model, writingResponse);
				}
				return this.typeContext;
			}

			// Token: 0x04000461 RID: 1121
			private readonly ODataFeedAndEntrySerializationInfo serializationInfo;

			// Token: 0x04000462 RID: 1122
			private int entryCount;

			// Token: 0x04000463 RID: 1123
			private InstanceAnnotationWriteTracker instanceAnnotationWriteTracker;

			// Token: 0x04000464 RID: 1124
			private ODataFeedAndEntryTypeContext typeContext;
		}

		// Token: 0x020001A8 RID: 424
		internal class EntryScope : ODataWriterCore.Scope
		{
			// Token: 0x06000D20 RID: 3360 RVA: 0x0002D412 File Offset: 0x0002B612
			internal EntryScope(ODataEntry entry, ODataFeedAndEntrySerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, bool writingResponse, ODataWriterBehavior writerBehavior, SelectedPropertiesNode selectedProperties) : base(ODataWriterCore.WriterState.Entry, entry, entitySet, entityType, skipWriting, selectedProperties)
			{
				if (entry != null)
				{
					this.duplicatePropertyNamesChecker = new DuplicatePropertyNamesChecker(writerBehavior.AllowDuplicatePropertyNames, writingResponse);
					this.odataEntryTypeName = entry.TypeName;
				}
				this.serializationInfo = serializationInfo;
			}

			// Token: 0x170002D3 RID: 723
			// (get) Token: 0x06000D21 RID: 3361 RVA: 0x0002D44D File Offset: 0x0002B64D
			// (set) Token: 0x06000D22 RID: 3362 RVA: 0x0002D455 File Offset: 0x0002B655
			public IEdmEntityType EntityTypeFromMetadata
			{
				get
				{
					return this.entityTypeFromMetadata;
				}
				internal set
				{
					this.entityTypeFromMetadata = value;
				}
			}

			// Token: 0x170002D4 RID: 724
			// (get) Token: 0x06000D23 RID: 3363 RVA: 0x0002D45E File Offset: 0x0002B65E
			public ODataFeedAndEntrySerializationInfo SerializationInfo
			{
				get
				{
					return this.serializationInfo;
				}
			}

			// Token: 0x170002D5 RID: 725
			// (get) Token: 0x06000D24 RID: 3364 RVA: 0x0002D466 File Offset: 0x0002B666
			internal DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker
			{
				get
				{
					return this.duplicatePropertyNamesChecker;
				}
			}

			// Token: 0x170002D6 RID: 726
			// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0002D46E File Offset: 0x0002B66E
			internal InstanceAnnotationWriteTracker InstanceAnnotationWriteTracker
			{
				get
				{
					if (this.instanceAnnotationWriteTracker == null)
					{
						this.instanceAnnotationWriteTracker = new InstanceAnnotationWriteTracker();
					}
					return this.instanceAnnotationWriteTracker;
				}
			}

			// Token: 0x06000D26 RID: 3366 RVA: 0x0002D489 File Offset: 0x0002B689
			public ODataFeedAndEntryTypeContext GetOrCreateTypeContext(IEdmModel model, bool writingResponse)
			{
				if (this.typeContext == null)
				{
					this.typeContext = ODataFeedAndEntryTypeContext.Create(this.serializationInfo, base.EntitySet, EdmTypeWriterResolver.Instance.GetElementType(base.EntitySet), this.EntityTypeFromMetadata, model, writingResponse);
				}
				return this.typeContext;
			}

			// Token: 0x04000465 RID: 1125
			private readonly DuplicatePropertyNamesChecker duplicatePropertyNamesChecker;

			// Token: 0x04000466 RID: 1126
			private readonly ODataFeedAndEntrySerializationInfo serializationInfo;

			// Token: 0x04000467 RID: 1127
			private readonly string odataEntryTypeName;

			// Token: 0x04000468 RID: 1128
			private IEdmEntityType entityTypeFromMetadata;

			// Token: 0x04000469 RID: 1129
			private ODataFeedAndEntryTypeContext typeContext;

			// Token: 0x0400046A RID: 1130
			private InstanceAnnotationWriteTracker instanceAnnotationWriteTracker;
		}

		// Token: 0x020001A9 RID: 425
		internal class NavigationLinkScope : ODataWriterCore.Scope
		{
			// Token: 0x06000D27 RID: 3367 RVA: 0x0002D4C8 File Offset: 0x0002B6C8
			internal NavigationLinkScope(ODataWriterCore.WriterState writerState, ODataNavigationLink navLink, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties) : base(writerState, navLink, entitySet, entityType, skipWriting, selectedProperties)
			{
			}

			// Token: 0x06000D28 RID: 3368 RVA: 0x0002D4D9 File Offset: 0x0002B6D9
			internal virtual ODataWriterCore.NavigationLinkScope Clone(ODataWriterCore.WriterState newWriterState)
			{
				return new ODataWriterCore.NavigationLinkScope(newWriterState, (ODataNavigationLink)base.Item, base.EntitySet, base.EntityType, base.SkipWriting, base.SelectedProperties);
			}
		}
	}
}
