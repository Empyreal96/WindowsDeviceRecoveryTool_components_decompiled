using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Evaluation;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200019A RID: 410
	internal sealed class ODataJsonLightReader : ODataReaderCoreAsync
	{
		// Token: 0x06000C61 RID: 3169 RVA: 0x0002A761 File Offset: 0x00028961
		internal ODataJsonLightReader(ODataJsonLightInputContext jsonLightInputContext, IEdmEntitySet entitySet, IEdmEntityType expectedEntityType, bool readingFeed, IODataReaderWriterListener listener) : base(jsonLightInputContext, readingFeed, listener)
		{
			this.jsonLightInputContext = jsonLightInputContext;
			this.jsonLightEntryAndFeedDeserializer = new ODataJsonLightEntryAndFeedDeserializer(jsonLightInputContext);
			this.topLevelScope = new ODataJsonLightReader.JsonLightTopLevelScope(entitySet, expectedEntityType);
			base.EnterScope(this.topLevelScope);
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0002A79A File Offset: 0x0002899A
		private IODataJsonLightReaderEntryState CurrentEntryState
		{
			get
			{
				return (IODataJsonLightReaderEntryState)base.CurrentScope;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x0002A7A7 File Offset: 0x000289A7
		private ODataJsonLightReader.JsonLightFeedScope CurrentJsonLightFeedScope
		{
			get
			{
				return (ODataJsonLightReader.JsonLightFeedScope)base.CurrentScope;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x0002A7B4 File Offset: 0x000289B4
		private ODataJsonLightReader.JsonLightNavigationLinkScope CurrentJsonLightNavigationLinkScope
		{
			get
			{
				return (ODataJsonLightReader.JsonLightNavigationLinkScope)base.CurrentScope;
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0002A7C4 File Offset: 0x000289C4
		protected override bool ReadAtStartImplementation()
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = this.jsonLightInputContext.CreateDuplicatePropertyNamesChecker();
			ODataPayloadKind payloadKind = base.ReadingFeed ? ODataPayloadKind.Feed : ODataPayloadKind.Entry;
			this.jsonLightEntryAndFeedDeserializer.ReadPayloadStart(payloadKind, duplicatePropertyNamesChecker, base.IsReadingNestedPayload, false);
			return this.ReadAtStartImplementationSynchronously(duplicatePropertyNamesChecker);
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0002A820 File Offset: 0x00028A20
		protected override Task<bool> ReadAtStartImplementationAsync()
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = this.jsonLightInputContext.CreateDuplicatePropertyNamesChecker();
			ODataPayloadKind payloadKind = base.ReadingFeed ? ODataPayloadKind.Feed : ODataPayloadKind.Entry;
			return this.jsonLightEntryAndFeedDeserializer.ReadPayloadStartAsync(payloadKind, duplicatePropertyNamesChecker, base.IsReadingNestedPayload, false).FollowOnSuccessWith((Task t) => this.ReadAtStartImplementationSynchronously(duplicatePropertyNamesChecker));
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0002A882 File Offset: 0x00028A82
		protected override bool ReadAtFeedStartImplementation()
		{
			return this.ReadAtFeedStartImplementationSynchronously();
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0002A88A File Offset: 0x00028A8A
		protected override Task<bool> ReadAtFeedStartImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtFeedStartImplementationSynchronously));
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0002A89D File Offset: 0x00028A9D
		protected override bool ReadAtFeedEndImplementation()
		{
			return this.ReadAtFeedEndImplementationSynchronously();
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0002A8A5 File Offset: 0x00028AA5
		protected override Task<bool> ReadAtFeedEndImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtFeedEndImplementationSynchronously));
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0002A8B8 File Offset: 0x00028AB8
		protected override bool ReadAtEntryStartImplementation()
		{
			return this.ReadAtEntryStartImplementationSynchronously();
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0002A8C0 File Offset: 0x00028AC0
		protected override Task<bool> ReadAtEntryStartImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtEntryStartImplementationSynchronously));
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0002A8D3 File Offset: 0x00028AD3
		protected override bool ReadAtEntryEndImplementation()
		{
			return this.ReadAtEntryEndImplementationSynchronously();
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0002A8DB File Offset: 0x00028ADB
		protected override Task<bool> ReadAtEntryEndImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtEntryEndImplementationSynchronously));
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x0002A8EE File Offset: 0x00028AEE
		protected override bool ReadAtNavigationLinkStartImplementation()
		{
			return this.ReadAtNavigationLinkStartImplementationSynchronously();
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0002A8F6 File Offset: 0x00028AF6
		protected override Task<bool> ReadAtNavigationLinkStartImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtNavigationLinkStartImplementationSynchronously));
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0002A909 File Offset: 0x00028B09
		protected override bool ReadAtNavigationLinkEndImplementation()
		{
			return this.ReadAtNavigationLinkEndImplementationSynchronously();
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0002A911 File Offset: 0x00028B11
		protected override Task<bool> ReadAtNavigationLinkEndImplementationAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtNavigationLinkEndImplementationSynchronously));
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0002A924 File Offset: 0x00028B24
		protected override bool ReadAtEntityReferenceLink()
		{
			return this.ReadAtEntityReferenceLinkSynchronously();
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0002A92C File Offset: 0x00028B2C
		protected override Task<bool> ReadAtEntityReferenceLinkAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadAtEntityReferenceLinkSynchronously));
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0002A940 File Offset: 0x00028B40
		private bool ReadAtStartImplementationSynchronously(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			if (this.jsonLightInputContext.ReadingResponse)
			{
				ReaderValidationUtils.ValidateFeedOrEntryMetadataUri(this.jsonLightEntryAndFeedDeserializer.MetadataUriParseResult, base.CurrentScope);
			}
			string selectQueryOption = (this.jsonLightEntryAndFeedDeserializer.MetadataUriParseResult == null) ? null : this.jsonLightEntryAndFeedDeserializer.MetadataUriParseResult.SelectQueryOption;
			SelectedPropertiesNode selectedProperties = SelectedPropertiesNode.Create(selectQueryOption);
			if (base.ReadingFeed)
			{
				ODataFeed feed = new ODataFeed();
				this.topLevelScope.DuplicatePropertyNamesChecker = duplicatePropertyNamesChecker;
				bool readAllFeedProperties = this.jsonLightInputContext.JsonReader is ReorderingJsonReader;
				this.jsonLightEntryAndFeedDeserializer.ReadTopLevelFeedAnnotations(feed, duplicatePropertyNamesChecker, true, readAllFeedProperties);
				this.ReadFeedStart(feed, selectedProperties);
				return true;
			}
			this.ReadEntryStart(duplicatePropertyNamesChecker, selectedProperties);
			return true;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0002A9E8 File Offset: 0x00028BE8
		private bool ReadAtFeedStartImplementationSynchronously()
		{
			JsonNodeType nodeType = this.jsonLightEntryAndFeedDeserializer.JsonReader.NodeType;
			if (nodeType != JsonNodeType.StartObject)
			{
				if (nodeType != JsonNodeType.EndArray)
				{
					throw new ODataException(Strings.ODataJsonReader_CannotReadEntriesOfFeed(this.jsonLightEntryAndFeedDeserializer.JsonReader.NodeType));
				}
				this.ReadFeedEnd();
			}
			else
			{
				this.ReadEntryStart(null, this.CurrentJsonLightFeedScope.SelectedProperties);
			}
			return true;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0002AA50 File Offset: 0x00028C50
		private bool ReadAtFeedEndImplementationSynchronously()
		{
			bool isTopLevel = base.IsTopLevel;
			base.PopScope(ODataReaderState.FeedEnd);
			if (isTopLevel)
			{
				this.jsonLightEntryAndFeedDeserializer.JsonReader.Read();
				this.jsonLightEntryAndFeedDeserializer.ReadPayloadEnd(base.IsReadingNestedPayload);
				this.ReplaceScope(ODataReaderState.Completed);
				return false;
			}
			this.ReadExpandedNavigationLinkEnd(true);
			return true;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0002AAA4 File Offset: 0x00028CA4
		private bool ReadAtEntryStartImplementationSynchronously()
		{
			if (base.CurrentEntry == null)
			{
				this.EndEntry();
			}
			else if (this.jsonLightInputContext.UseServerApiBehavior)
			{
				ODataJsonLightReaderNavigationLinkInfo odataJsonLightReaderNavigationLinkInfo = this.jsonLightEntryAndFeedDeserializer.ReadEntryContent(this.CurrentEntryState);
				if (odataJsonLightReaderNavigationLinkInfo != null)
				{
					this.StartNavigationLink(odataJsonLightReaderNavigationLinkInfo);
				}
				else
				{
					this.EndEntry();
				}
			}
			else if (this.CurrentEntryState.FirstNavigationLinkInfo != null)
			{
				this.StartNavigationLink(this.CurrentEntryState.FirstNavigationLinkInfo);
			}
			else
			{
				this.EndEntry();
			}
			return true;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0002AB1C File Offset: 0x00028D1C
		private bool ReadAtEntryEndImplementationSynchronously()
		{
			bool isTopLevel = base.IsTopLevel;
			bool isExpandedLinkContent = base.IsExpandedLinkContent;
			base.PopScope(ODataReaderState.EntryEnd);
			this.jsonLightEntryAndFeedDeserializer.JsonReader.Read();
			JsonNodeType nodeType = this.jsonLightEntryAndFeedDeserializer.JsonReader.NodeType;
			bool result = true;
			if (isTopLevel)
			{
				this.jsonLightEntryAndFeedDeserializer.ReadPayloadEnd(base.IsReadingNestedPayload);
				this.ReplaceScope(ODataReaderState.Completed);
				result = false;
			}
			else if (isExpandedLinkContent)
			{
				this.ReadExpandedNavigationLinkEnd(false);
			}
			else
			{
				JsonNodeType jsonNodeType = nodeType;
				if (jsonNodeType != JsonNodeType.StartObject)
				{
					if (jsonNodeType != JsonNodeType.EndArray)
					{
						throw new ODataException(Strings.ODataJsonReader_CannotReadEntriesOfFeed(this.jsonLightEntryAndFeedDeserializer.JsonReader.NodeType));
					}
					this.ReadFeedEnd();
				}
				else
				{
					this.ReadEntryStart(null, this.CurrentJsonLightFeedScope.SelectedProperties);
				}
			}
			return result;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0002ABDC File Offset: 0x00028DDC
		private bool ReadAtNavigationLinkStartImplementationSynchronously()
		{
			ODataNavigationLink currentNavigationLink = base.CurrentNavigationLink;
			IODataJsonLightReaderEntryState iodataJsonLightReaderEntryState = (IODataJsonLightReaderEntryState)base.LinkParentEntityScope;
			if (this.jsonLightInputContext.ReadingResponse)
			{
				if (iodataJsonLightReaderEntryState.ProcessingMissingProjectedNavigationLinks)
				{
					this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
				}
				else if (!this.jsonLightEntryAndFeedDeserializer.JsonReader.IsOnValueNode())
				{
					ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataJsonLightReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, false, currentNavigationLink.IsCollection);
					iodataJsonLightReaderEntryState.NavigationPropertiesRead.Add(currentNavigationLink.Name);
					this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
				}
				else if (!currentNavigationLink.IsCollection.Value)
				{
					ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataJsonLightReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, true, new bool?(false));
					this.ReadExpandedEntryStart(currentNavigationLink);
				}
				else
				{
					ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataJsonLightReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, true, new bool?(true));
					ODataJsonLightReaderNavigationLinkInfo navigationLinkInfo = this.CurrentJsonLightNavigationLinkScope.NavigationLinkInfo;
					ODataJsonLightReader.JsonLightEntryScope jsonLightEntryScope = (ODataJsonLightReader.JsonLightEntryScope)base.LinkParentEntityScope;
					SelectedPropertiesNode selectedProperties = jsonLightEntryScope.SelectedProperties;
					this.ReadFeedStart(navigationLinkInfo.ExpandedFeed, selectedProperties.GetSelectedPropertiesForNavigationProperty(jsonLightEntryScope.EntityType, currentNavigationLink.Name));
				}
			}
			else
			{
				ODataJsonLightReaderNavigationLinkInfo navigationLinkInfo2 = this.CurrentJsonLightNavigationLinkScope.NavigationLinkInfo;
				ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataJsonLightReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, navigationLinkInfo2.IsExpanded, currentNavigationLink.IsCollection);
				this.ReadNextNavigationLinkContentItemInRequest();
			}
			return true;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0002AD14 File Offset: 0x00028F14
		private bool ReadAtNavigationLinkEndImplementationSynchronously()
		{
			base.PopScope(ODataReaderState.NavigationLinkEnd);
			IODataJsonLightReaderEntryState currentEntryState = this.CurrentEntryState;
			ODataJsonLightReaderNavigationLinkInfo odataJsonLightReaderNavigationLinkInfo;
			if (this.jsonLightInputContext.ReadingResponse && currentEntryState.ProcessingMissingProjectedNavigationLinks)
			{
				odataJsonLightReaderNavigationLinkInfo = currentEntryState.Entry.MetadataBuilder.GetNextUnprocessedNavigationLink();
			}
			else
			{
				odataJsonLightReaderNavigationLinkInfo = this.jsonLightEntryAndFeedDeserializer.ReadEntryContent(currentEntryState);
			}
			if (odataJsonLightReaderNavigationLinkInfo == null)
			{
				this.EndEntry();
			}
			else
			{
				this.StartNavigationLink(odataJsonLightReaderNavigationLinkInfo);
			}
			return true;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0002AD79 File Offset: 0x00028F79
		private bool ReadAtEntityReferenceLinkSynchronously()
		{
			base.PopScope(ODataReaderState.EntityReferenceLink);
			this.ReadNextNavigationLinkContentItemInRequest();
			return true;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0002AD89 File Offset: 0x00028F89
		private void ReadFeedStart(ODataFeed feed, SelectedPropertiesNode selectedProperties)
		{
			this.jsonLightEntryAndFeedDeserializer.ReadFeedContentStart();
			base.EnterScope(new ODataJsonLightReader.JsonLightFeedScope(feed, base.CurrentEntitySet, base.CurrentEntityType, selectedProperties));
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0002ADB0 File Offset: 0x00028FB0
		private void ReadFeedEnd()
		{
			this.jsonLightEntryAndFeedDeserializer.ReadFeedContentEnd();
			ODataJsonLightReaderNavigationLinkInfo expandedNavigationLinkInfo = null;
			ODataJsonLightReader.JsonLightNavigationLinkScope jsonLightNavigationLinkScope = (ODataJsonLightReader.JsonLightNavigationLinkScope)base.ExpandedLinkContentParentScope;
			if (jsonLightNavigationLinkScope != null)
			{
				expandedNavigationLinkInfo = jsonLightNavigationLinkScope.NavigationLinkInfo;
			}
			this.jsonLightEntryAndFeedDeserializer.ReadNextLinkAnnotationAtFeedEnd(base.CurrentFeed, expandedNavigationLinkInfo, this.topLevelScope.DuplicatePropertyNamesChecker);
			this.ReplaceScope(ODataReaderState.FeedEnd);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0002AE04 File Offset: 0x00029004
		private void ReadExpandedEntryStart(ODataNavigationLink navigationLink)
		{
			if (this.jsonLightEntryAndFeedDeserializer.JsonReader.NodeType == JsonNodeType.PrimitiveValue)
			{
				base.EnterScope(new ODataJsonLightReader.JsonLightEntryScope(ODataReaderState.EntryStart, null, base.CurrentEntitySet, base.CurrentEntityType, null, null));
				return;
			}
			ODataJsonLightReader.JsonLightEntryScope jsonLightEntryScope = (ODataJsonLightReader.JsonLightEntryScope)base.LinkParentEntityScope;
			SelectedPropertiesNode selectedProperties = jsonLightEntryScope.SelectedProperties;
			this.ReadEntryStart(null, selectedProperties.GetSelectedPropertiesForNavigationProperty(jsonLightEntryScope.EntityType, navigationLink.Name));
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0002AE6C File Offset: 0x0002906C
		private void ReadEntryStart(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, SelectedPropertiesNode selectedProperties)
		{
			if (this.jsonLightEntryAndFeedDeserializer.JsonReader.NodeType == JsonNodeType.StartObject)
			{
				this.jsonLightEntryAndFeedDeserializer.JsonReader.Read();
			}
			this.StartEntry(duplicatePropertyNamesChecker, selectedProperties);
			if (this.jsonLightInputContext.JsonReader.NodeType == JsonNodeType.Property)
			{
				this.jsonLightEntryAndFeedDeserializer.ApplyAnnotationGroupIfPresent(this.CurrentEntryState);
			}
			this.jsonLightEntryAndFeedDeserializer.ReadEntryTypeName(this.CurrentEntryState);
			base.ApplyEntityTypeNameFromPayload(base.CurrentEntry.TypeName);
			if (base.CurrentFeedValidator != null)
			{
				base.CurrentFeedValidator.ValidateEntry(base.CurrentEntityType);
			}
			if (base.CurrentEntityType != null)
			{
				base.CurrentEntry.SetAnnotation<ODataTypeAnnotation>(new ODataTypeAnnotation(base.CurrentEntitySet, base.CurrentEntityType));
			}
			if (this.jsonLightInputContext.UseServerApiBehavior)
			{
				this.CurrentEntryState.FirstNavigationLinkInfo = null;
				return;
			}
			this.CurrentEntryState.FirstNavigationLinkInfo = this.jsonLightEntryAndFeedDeserializer.ReadEntryContent(this.CurrentEntryState);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0002AF60 File Offset: 0x00029160
		private void ReadExpandedNavigationLinkEnd(bool isCollection)
		{
			base.CurrentNavigationLink.IsCollection = new bool?(isCollection);
			IODataJsonLightReaderEntryState iodataJsonLightReaderEntryState = (IODataJsonLightReaderEntryState)base.LinkParentEntityScope;
			iodataJsonLightReaderEntryState.NavigationPropertiesRead.Add(base.CurrentNavigationLink.Name);
			this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0002AFA8 File Offset: 0x000291A8
		private void ReadNextNavigationLinkContentItemInRequest()
		{
			ODataJsonLightReaderNavigationLinkInfo navigationLinkInfo = this.CurrentJsonLightNavigationLinkScope.NavigationLinkInfo;
			if (navigationLinkInfo.HasEntityReferenceLink)
			{
				base.EnterScope(new ODataReaderCore.Scope(ODataReaderState.EntityReferenceLink, navigationLinkInfo.ReportEntityReferenceLink(), null, null));
				return;
			}
			if (!navigationLinkInfo.IsExpanded)
			{
				this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
				return;
			}
			if (navigationLinkInfo.NavigationLink.IsCollection == true)
			{
				SelectedPropertiesNode selectedProperties = SelectedPropertiesNode.Create(null);
				this.ReadFeedStart(new ODataFeed(), selectedProperties);
				return;
			}
			this.ReadExpandedEntryStart(navigationLinkInfo.NavigationLink);
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0002B02F File Offset: 0x0002922F
		private void StartEntry(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, SelectedPropertiesNode selectedProperties)
		{
			base.EnterScope(new ODataJsonLightReader.JsonLightEntryScope(ODataReaderState.EntryStart, ReaderUtils.CreateNewEntry(), base.CurrentEntitySet, base.CurrentEntityType, duplicatePropertyNamesChecker ?? this.jsonLightInputContext.CreateDuplicatePropertyNamesChecker(), selectedProperties));
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0002B060 File Offset: 0x00029260
		private void StartNavigationLink(ODataJsonLightReaderNavigationLinkInfo navigationLinkInfo)
		{
			ODataNavigationLink navigationLink = navigationLinkInfo.NavigationLink;
			IEdmNavigationProperty navigationProperty = navigationLinkInfo.NavigationProperty;
			IEdmEntityType expectedEntityType = null;
			if (navigationProperty != null)
			{
				IEdmTypeReference type = navigationProperty.Type;
				expectedEntityType = (type.IsCollection() ? type.AsCollection().ElementType().AsEntity().EntityDefinition() : type.AsEntity().EntityDefinition());
			}
			if (this.jsonLightInputContext.ReadingResponse)
			{
				ODataAssociationLink odataAssociationLink = new ODataAssociationLink
				{
					Name = navigationLink.Name
				};
				if (navigationLink.AssociationLinkUrl != null)
				{
					odataAssociationLink.Url = navigationLink.AssociationLinkUrl;
				}
				base.CurrentEntry.AddAssociationLink(odataAssociationLink);
				ODataEntityMetadataBuilder entityMetadataBuilderForReader = this.jsonLightEntryAndFeedDeserializer.MetadataContext.GetEntityMetadataBuilderForReader(this.CurrentEntryState);
				navigationLink.SetMetadataBuilder(entityMetadataBuilderForReader);
				odataAssociationLink.SetMetadataBuilder(entityMetadataBuilderForReader);
			}
			IEdmEntitySet entitySet = (navigationProperty == null) ? null : base.CurrentEntitySet.FindNavigationTarget(navigationProperty);
			base.EnterScope(new ODataJsonLightReader.JsonLightNavigationLinkScope(navigationLinkInfo, entitySet, expectedEntityType));
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0002B14B File Offset: 0x0002934B
		private void ReplaceScope(ODataReaderState state)
		{
			base.ReplaceScope(new ODataReaderCore.Scope(state, this.Item, base.CurrentEntitySet, base.CurrentEntityType));
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0002B16C File Offset: 0x0002936C
		private void EndEntry()
		{
			IODataJsonLightReaderEntryState currentEntryState = this.CurrentEntryState;
			if (currentEntryState.DuplicatePropertyNamesChecker != null)
			{
				foreach (string propertyName in currentEntryState.DuplicatePropertyNamesChecker.GetAllUnprocessedProperties())
				{
					currentEntryState.AnyPropertyFound = true;
					ODataJsonLightReaderNavigationLinkInfo odataJsonLightReaderNavigationLinkInfo = this.jsonLightEntryAndFeedDeserializer.ReadEntryPropertyWithoutValue(currentEntryState, propertyName);
					currentEntryState.DuplicatePropertyNamesChecker.MarkPropertyAsProcessed(propertyName);
					if (odataJsonLightReaderNavigationLinkInfo != null)
					{
						this.StartNavigationLink(odataJsonLightReaderNavigationLinkInfo);
						return;
					}
				}
			}
			if (base.CurrentEntry != null)
			{
				ODataEntityMetadataBuilder entityMetadataBuilderForReader = this.jsonLightEntryAndFeedDeserializer.MetadataContext.GetEntityMetadataBuilderForReader(this.CurrentEntryState);
				if (entityMetadataBuilderForReader != base.CurrentEntry.MetadataBuilder)
				{
					foreach (string navigationPropertyName in this.CurrentEntryState.NavigationPropertiesRead)
					{
						entityMetadataBuilderForReader.MarkNavigationLinkProcessed(navigationPropertyName);
					}
					base.CurrentEntry.MetadataBuilder = entityMetadataBuilderForReader;
				}
			}
			this.jsonLightEntryAndFeedDeserializer.ValidateEntryMetadata(currentEntryState);
			if (this.jsonLightInputContext.ReadingResponse && base.CurrentEntry != null)
			{
				ODataJsonLightReaderNavigationLinkInfo nextUnprocessedNavigationLink = base.CurrentEntry.MetadataBuilder.GetNextUnprocessedNavigationLink();
				if (nextUnprocessedNavigationLink != null)
				{
					this.CurrentEntryState.ProcessingMissingProjectedNavigationLinks = true;
					this.StartNavigationLink(nextUnprocessedNavigationLink);
					return;
				}
			}
			base.EndEntry(new ODataJsonLightReader.JsonLightEntryScope(ODataReaderState.EntryEnd, (ODataEntry)this.Item, base.CurrentEntitySet, base.CurrentEntityType, this.CurrentEntryState.DuplicatePropertyNamesChecker, this.CurrentEntryState.SelectedProperties));
		}

		// Token: 0x0400043D RID: 1085
		private readonly ODataJsonLightInputContext jsonLightInputContext;

		// Token: 0x0400043E RID: 1086
		private readonly ODataJsonLightEntryAndFeedDeserializer jsonLightEntryAndFeedDeserializer;

		// Token: 0x0400043F RID: 1087
		private readonly ODataJsonLightReader.JsonLightTopLevelScope topLevelScope;

		// Token: 0x0200019B RID: 411
		private sealed class JsonLightTopLevelScope : ODataReaderCore.Scope
		{
			// Token: 0x06000C87 RID: 3207 RVA: 0x0002B304 File Offset: 0x00029504
			internal JsonLightTopLevelScope(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType) : base(ODataReaderState.Start, null, entitySet, expectedEntityType)
			{
			}

			// Token: 0x170002AF RID: 687
			// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0002B310 File Offset: 0x00029510
			// (set) Token: 0x06000C89 RID: 3209 RVA: 0x0002B318 File Offset: 0x00029518
			public DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker { get; set; }
		}

		// Token: 0x0200019C RID: 412
		private sealed class JsonLightEntryScope : ODataReaderCore.Scope, IODataJsonLightReaderEntryState
		{
			// Token: 0x06000C8A RID: 3210 RVA: 0x0002B321 File Offset: 0x00029521
			internal JsonLightEntryScope(ODataReaderState readerState, ODataEntry entry, IEdmEntitySet entitySet, IEdmEntityType expectedEntityType, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, SelectedPropertiesNode selectedProperties) : base(readerState, entry, entitySet, expectedEntityType)
			{
				this.DuplicatePropertyNamesChecker = duplicatePropertyNamesChecker;
				this.SelectedProperties = selectedProperties;
			}

			// Token: 0x170002B0 RID: 688
			// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0002B33E File Offset: 0x0002953E
			// (set) Token: 0x06000C8C RID: 3212 RVA: 0x0002B346 File Offset: 0x00029546
			public ODataEntityMetadataBuilder MetadataBuilder { get; set; }

			// Token: 0x170002B1 RID: 689
			// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0002B34F File Offset: 0x0002954F
			// (set) Token: 0x06000C8E RID: 3214 RVA: 0x0002B357 File Offset: 0x00029557
			public bool AnyPropertyFound { get; set; }

			// Token: 0x170002B2 RID: 690
			// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0002B360 File Offset: 0x00029560
			// (set) Token: 0x06000C90 RID: 3216 RVA: 0x0002B368 File Offset: 0x00029568
			public ODataJsonLightReaderNavigationLinkInfo FirstNavigationLinkInfo { get; set; }

			// Token: 0x170002B3 RID: 691
			// (get) Token: 0x06000C91 RID: 3217 RVA: 0x0002B371 File Offset: 0x00029571
			// (set) Token: 0x06000C92 RID: 3218 RVA: 0x0002B379 File Offset: 0x00029579
			public DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker { get; private set; }

			// Token: 0x170002B4 RID: 692
			// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0002B382 File Offset: 0x00029582
			// (set) Token: 0x06000C94 RID: 3220 RVA: 0x0002B38A File Offset: 0x0002958A
			public SelectedPropertiesNode SelectedProperties { get; private set; }

			// Token: 0x170002B5 RID: 693
			// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0002B394 File Offset: 0x00029594
			public List<string> NavigationPropertiesRead
			{
				get
				{
					List<string> result;
					if ((result = this.navigationPropertiesRead) == null)
					{
						result = (this.navigationPropertiesRead = new List<string>());
					}
					return result;
				}
			}

			// Token: 0x170002B6 RID: 694
			// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0002B3B9 File Offset: 0x000295B9
			// (set) Token: 0x06000C97 RID: 3223 RVA: 0x0002B3C1 File Offset: 0x000295C1
			public bool ProcessingMissingProjectedNavigationLinks { get; set; }

			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0002B3CA File Offset: 0x000295CA
			ODataEntry IODataJsonLightReaderEntryState.Entry
			{
				get
				{
					return (ODataEntry)base.Item;
				}
			}

			// Token: 0x170002B8 RID: 696
			// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0002B3D7 File Offset: 0x000295D7
			IEdmEntityType IODataJsonLightReaderEntryState.EntityType
			{
				get
				{
					return base.EntityType;
				}
			}

			// Token: 0x04000441 RID: 1089
			private List<string> navigationPropertiesRead;
		}

		// Token: 0x0200019D RID: 413
		private sealed class JsonLightFeedScope : ODataReaderCore.Scope
		{
			// Token: 0x06000C9A RID: 3226 RVA: 0x0002B3DF File Offset: 0x000295DF
			internal JsonLightFeedScope(ODataFeed feed, IEdmEntitySet entitySet, IEdmEntityType expectedEntityType, SelectedPropertiesNode selectedProperties) : base(ODataReaderState.FeedStart, feed, entitySet, expectedEntityType)
			{
				this.SelectedProperties = selectedProperties;
			}

			// Token: 0x170002B9 RID: 697
			// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0002B3F3 File Offset: 0x000295F3
			// (set) Token: 0x06000C9C RID: 3228 RVA: 0x0002B3FB File Offset: 0x000295FB
			public SelectedPropertiesNode SelectedProperties { get; private set; }
		}

		// Token: 0x0200019E RID: 414
		private sealed class JsonLightNavigationLinkScope : ODataReaderCore.Scope
		{
			// Token: 0x06000C9D RID: 3229 RVA: 0x0002B404 File Offset: 0x00029604
			internal JsonLightNavigationLinkScope(ODataJsonLightReaderNavigationLinkInfo navigationLinkInfo, IEdmEntitySet entitySet, IEdmEntityType expectedEntityType) : base(ODataReaderState.NavigationLinkStart, navigationLinkInfo.NavigationLink, entitySet, expectedEntityType)
			{
				this.NavigationLinkInfo = navigationLinkInfo;
			}

			// Token: 0x170002BA RID: 698
			// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0002B41C File Offset: 0x0002961C
			// (set) Token: 0x06000C9F RID: 3231 RVA: 0x0002B424 File Offset: 0x00029624
			public ODataJsonLightReaderNavigationLinkInfo NavigationLinkInfo { get; private set; }
		}
	}
}
