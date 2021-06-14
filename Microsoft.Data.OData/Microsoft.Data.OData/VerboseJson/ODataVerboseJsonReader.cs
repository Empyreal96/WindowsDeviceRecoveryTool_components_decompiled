using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x0200020A RID: 522
	internal sealed class ODataVerboseJsonReader : ODataReaderCore
	{
		// Token: 0x06000FE4 RID: 4068 RVA: 0x00039C7C File Offset: 0x00037E7C
		internal ODataVerboseJsonReader(ODataVerboseJsonInputContext verboseJsonInputContext, IEdmEntitySet entitySet, IEdmEntityType expectedEntityType, bool readingFeed, IODataReaderWriterListener listener) : base(verboseJsonInputContext, readingFeed, listener)
		{
			this.verboseJsonInputContext = verboseJsonInputContext;
			this.verboseJsonEntryAndFeedDeserializer = new ODataVerboseJsonEntryAndFeedDeserializer(verboseJsonInputContext);
			if (!this.verboseJsonInputContext.Model.IsUserModel())
			{
				throw new ODataException(Strings.ODataJsonReader_ParsingWithoutMetadata);
			}
			base.EnterScope(new ODataReaderCore.Scope(ODataReaderState.Start, null, entitySet, expectedEntityType));
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x00039CD3 File Offset: 0x00037ED3
		private IODataVerboseJsonReaderEntryState CurrentEntryState
		{
			get
			{
				return (IODataVerboseJsonReaderEntryState)base.CurrentScope;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x00039CE0 File Offset: 0x00037EE0
		private ODataVerboseJsonReader.JsonScope CurrentJsonScope
		{
			get
			{
				return (ODataVerboseJsonReader.JsonScope)base.CurrentScope;
			}
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00039CED File Offset: 0x00037EED
		protected override bool ReadAtStartImplementation()
		{
			this.verboseJsonEntryAndFeedDeserializer.ReadPayloadStart(base.IsReadingNestedPayload);
			if (base.ReadingFeed)
			{
				this.ReadFeedStart(false);
				return true;
			}
			this.ReadEntryStart();
			return true;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00039D18 File Offset: 0x00037F18
		protected override bool ReadAtFeedStartImplementation()
		{
			JsonNodeType nodeType = this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType;
			if (nodeType != JsonNodeType.StartObject)
			{
				if (nodeType != JsonNodeType.EndArray)
				{
					throw new ODataException(Strings.ODataJsonReader_CannotReadEntriesOfFeed(this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType));
				}
				this.verboseJsonEntryAndFeedDeserializer.ReadFeedEnd(base.CurrentFeed, this.CurrentJsonScope.FeedHasResultsWrapper, base.IsExpandedLinkContent);
				this.ReplaceScope(ODataReaderState.FeedEnd);
			}
			else
			{
				this.ReadEntryStart();
			}
			return true;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00039D94 File Offset: 0x00037F94
		protected override bool ReadAtFeedEndImplementation()
		{
			bool isTopLevel = base.IsTopLevel;
			base.PopScope(ODataReaderState.FeedEnd);
			bool result;
			if (isTopLevel)
			{
				this.verboseJsonEntryAndFeedDeserializer.JsonReader.Read();
				this.verboseJsonEntryAndFeedDeserializer.ReadPayloadEnd(base.IsReadingNestedPayload);
				this.ReplaceScope(ODataReaderState.Completed);
				result = false;
			}
			else
			{
				if (this.verboseJsonInputContext.ReadingResponse)
				{
					this.verboseJsonEntryAndFeedDeserializer.JsonReader.Read();
					this.ReadExpandedNavigationLinkEnd(true);
				}
				else
				{
					this.ReadExpandedCollectionNavigationLinkContentInRequest();
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00039E10 File Offset: 0x00038010
		protected override bool ReadAtEntryStartImplementation()
		{
			if (base.CurrentEntry == null)
			{
				this.EndEntry();
			}
			else if (this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType == JsonNodeType.EndObject)
			{
				this.EndEntry();
			}
			else if (this.verboseJsonInputContext.UseServerApiBehavior)
			{
				IEdmNavigationProperty navigationProperty;
				ODataNavigationLink odataNavigationLink = this.verboseJsonEntryAndFeedDeserializer.ReadEntryContent(this.CurrentEntryState, out navigationProperty);
				if (odataNavigationLink != null)
				{
					this.StartNavigationLink(odataNavigationLink, navigationProperty);
				}
				else
				{
					this.EndEntry();
				}
			}
			else
			{
				this.StartNavigationLink(this.CurrentEntryState.FirstNavigationLink, this.CurrentEntryState.FirstNavigationProperty);
			}
			return true;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00039E9C File Offset: 0x0003809C
		protected override bool ReadAtEntryEndImplementation()
		{
			bool isTopLevel = base.IsTopLevel;
			bool isExpandedLinkContent = base.IsExpandedLinkContent;
			base.PopScope(ODataReaderState.EntryEnd);
			this.verboseJsonEntryAndFeedDeserializer.JsonReader.Read();
			JsonNodeType nodeType = this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType;
			bool result = true;
			if (isTopLevel)
			{
				this.verboseJsonEntryAndFeedDeserializer.ReadPayloadEnd(base.IsReadingNestedPayload);
				this.ReplaceScope(ODataReaderState.Completed);
				result = false;
			}
			else if (isExpandedLinkContent)
			{
				this.ReadExpandedNavigationLinkEnd(false);
			}
			else if (this.CurrentJsonScope.FeedInExpandedNavigationLinkInRequest)
			{
				this.ReadExpandedCollectionNavigationLinkContentInRequest();
			}
			else
			{
				JsonNodeType jsonNodeType = nodeType;
				if (jsonNodeType != JsonNodeType.StartObject)
				{
					if (jsonNodeType != JsonNodeType.EndArray)
					{
						throw new ODataException(Strings.ODataJsonReader_CannotReadEntriesOfFeed(this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType));
					}
					this.verboseJsonEntryAndFeedDeserializer.ReadFeedEnd(base.CurrentFeed, this.CurrentJsonScope.FeedHasResultsWrapper, base.IsExpandedLinkContent);
					this.ReplaceScope(ODataReaderState.FeedEnd);
				}
				else
				{
					this.ReadEntryStart();
				}
			}
			return result;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00039F8C File Offset: 0x0003818C
		protected override bool ReadAtNavigationLinkStartImplementation()
		{
			ODataNavigationLink currentNavigationLink = base.CurrentNavigationLink;
			IODataVerboseJsonReaderEntryState iodataVerboseJsonReaderEntryState = (IODataVerboseJsonReaderEntryState)base.LinkParentEntityScope;
			if (this.verboseJsonInputContext.ReadingResponse && this.verboseJsonEntryAndFeedDeserializer.IsDeferredLink(true))
			{
				ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataVerboseJsonReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, false, currentNavigationLink.IsCollection);
				this.verboseJsonEntryAndFeedDeserializer.ReadDeferredNavigationLink(currentNavigationLink);
				this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
			}
			else if (!currentNavigationLink.IsCollection.Value)
			{
				if (!this.verboseJsonInputContext.ReadingResponse && this.verboseJsonEntryAndFeedDeserializer.IsEntityReferenceLink())
				{
					ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataVerboseJsonReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, false, new bool?(false));
					ODataEntityReferenceLink item = this.verboseJsonEntryAndFeedDeserializer.ReadEntityReferenceLink();
					this.EnterScope(ODataReaderState.EntityReferenceLink, item, null);
				}
				else
				{
					ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataVerboseJsonReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, true, new bool?(false));
					if (this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType == JsonNodeType.PrimitiveValue)
					{
						this.EnterScope(ODataReaderState.EntryStart, null, base.CurrentEntityType);
					}
					else
					{
						this.ReadEntryStart();
					}
				}
			}
			else
			{
				ReaderUtils.CheckForDuplicateNavigationLinkNameAndSetAssociationLink(iodataVerboseJsonReaderEntryState.DuplicatePropertyNamesChecker, currentNavigationLink, true, new bool?(true));
				if (this.verboseJsonInputContext.ReadingResponse)
				{
					this.ReadFeedStart(true);
				}
				else
				{
					if (this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType != JsonNodeType.StartObject && this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType != JsonNodeType.StartArray)
					{
						throw new ODataException(Strings.ODataJsonReader_CannotReadFeedStart(this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType));
					}
					bool flag = this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType == JsonNodeType.StartObject;
					this.verboseJsonEntryAndFeedDeserializer.ReadFeedStart(new ODataFeed(), flag, true);
					this.CurrentJsonScope.FeedHasResultsWrapper = flag;
					this.ReadExpandedCollectionNavigationLinkContentInRequest();
				}
			}
			return true;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0003A13C File Offset: 0x0003833C
		protected override bool ReadAtNavigationLinkEndImplementation()
		{
			base.PopScope(ODataReaderState.NavigationLinkEnd);
			IEdmNavigationProperty navigationProperty;
			ODataNavigationLink odataNavigationLink = this.verboseJsonEntryAndFeedDeserializer.ReadEntryContent(this.CurrentEntryState, out navigationProperty);
			if (odataNavigationLink == null)
			{
				this.EndEntry();
			}
			else
			{
				this.StartNavigationLink(odataNavigationLink, navigationProperty);
			}
			return true;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0003A178 File Offset: 0x00038378
		protected override bool ReadAtEntityReferenceLink()
		{
			base.PopScope(ODataReaderState.EntityReferenceLink);
			if (base.CurrentNavigationLink.IsCollection == true)
			{
				this.ReadExpandedCollectionNavigationLinkContentInRequest();
			}
			else
			{
				this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
			}
			return true;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0003A1C0 File Offset: 0x000383C0
		private void ReadFeedStart(bool isExpandedLinkContent)
		{
			ODataFeed odataFeed = new ODataFeed();
			if (this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType != JsonNodeType.StartObject && this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType != JsonNodeType.StartArray)
			{
				throw new ODataException(Strings.ODataJsonReader_CannotReadFeedStart(this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType));
			}
			bool flag = this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType == JsonNodeType.StartObject;
			this.verboseJsonEntryAndFeedDeserializer.ReadFeedStart(odataFeed, flag, isExpandedLinkContent);
			this.EnterScope(ODataReaderState.FeedStart, odataFeed, base.CurrentEntityType);
			this.CurrentJsonScope.FeedHasResultsWrapper = flag;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0003A258 File Offset: 0x00038458
		private void ReadExpandedCollectionNavigationLinkContentInRequest()
		{
			if (this.verboseJsonEntryAndFeedDeserializer.IsEntityReferenceLink())
			{
				if (this.State == ODataReaderState.FeedStart)
				{
					this.ReplaceScope(ODataReaderState.FeedEnd);
					return;
				}
				this.CurrentJsonScope.ExpandedNavigationLinkInRequestHasContent = true;
				ODataEntityReferenceLink item = this.verboseJsonEntryAndFeedDeserializer.ReadEntityReferenceLink();
				this.EnterScope(ODataReaderState.EntityReferenceLink, item, null);
				return;
			}
			else if (this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType == JsonNodeType.EndArray || this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType == JsonNodeType.EndObject)
			{
				if (this.State == ODataReaderState.FeedStart)
				{
					this.verboseJsonEntryAndFeedDeserializer.ReadFeedEnd(base.CurrentFeed, this.CurrentJsonScope.FeedHasResultsWrapper, true);
					this.ReplaceScope(ODataReaderState.FeedEnd);
					return;
				}
				if (!this.CurrentJsonScope.ExpandedNavigationLinkInRequestHasContent)
				{
					this.CurrentJsonScope.ExpandedNavigationLinkInRequestHasContent = true;
					this.EnterScope(ODataReaderState.FeedStart, new ODataFeed(), base.CurrentEntityType);
					this.CurrentJsonScope.FeedInExpandedNavigationLinkInRequest = true;
					return;
				}
				if (this.CurrentJsonScope.FeedHasResultsWrapper)
				{
					ODataFeed feed = new ODataFeed();
					this.verboseJsonEntryAndFeedDeserializer.ReadFeedEnd(feed, true, true);
				}
				this.verboseJsonEntryAndFeedDeserializer.JsonReader.Read();
				this.ReadExpandedNavigationLinkEnd(true);
				return;
			}
			else
			{
				if (this.State != ODataReaderState.FeedStart)
				{
					this.CurrentJsonScope.ExpandedNavigationLinkInRequestHasContent = true;
					this.EnterScope(ODataReaderState.FeedStart, new ODataFeed(), base.CurrentEntityType);
					this.CurrentJsonScope.FeedInExpandedNavigationLinkInRequest = true;
					return;
				}
				if (this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType != JsonNodeType.StartObject)
				{
					throw new ODataException(Strings.ODataJsonReader_CannotReadEntriesOfFeed(this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType));
				}
				this.ReadEntryStart();
				return;
			}
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0003A3DC File Offset: 0x000385DC
		private void ReadEntryStart()
		{
			this.verboseJsonEntryAndFeedDeserializer.ReadEntryStart();
			this.StartEntry();
			this.ReadEntryMetadata();
			if (this.verboseJsonInputContext.UseServerApiBehavior)
			{
				this.CurrentEntryState.FirstNavigationLink = null;
				this.CurrentEntryState.FirstNavigationProperty = null;
				return;
			}
			IEdmNavigationProperty firstNavigationProperty;
			this.CurrentEntryState.FirstNavigationLink = this.verboseJsonEntryAndFeedDeserializer.ReadEntryContent(this.CurrentEntryState, out firstNavigationProperty);
			this.CurrentEntryState.FirstNavigationProperty = firstNavigationProperty;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003A450 File Offset: 0x00038650
		private void ReadEntryMetadata()
		{
			this.verboseJsonEntryAndFeedDeserializer.JsonReader.StartBuffering();
			bool flag = false;
			while (this.verboseJsonEntryAndFeedDeserializer.JsonReader.NodeType == JsonNodeType.Property)
			{
				string strA = this.verboseJsonEntryAndFeedDeserializer.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal(strA, "__metadata") == 0)
				{
					flag = true;
					break;
				}
				this.verboseJsonEntryAndFeedDeserializer.JsonReader.SkipValue();
			}
			string entityTypeNameFromPayload = null;
			object bookmark = null;
			if (flag)
			{
				bookmark = this.verboseJsonEntryAndFeedDeserializer.JsonReader.BookmarkCurrentPosition();
				entityTypeNameFromPayload = this.verboseJsonEntryAndFeedDeserializer.ReadTypeNameFromMetadataPropertyValue();
			}
			base.ApplyEntityTypeNameFromPayload(entityTypeNameFromPayload);
			if (base.CurrentFeedValidator != null)
			{
				base.CurrentFeedValidator.ValidateEntry(base.CurrentEntityType);
			}
			if (flag)
			{
				this.verboseJsonEntryAndFeedDeserializer.JsonReader.MoveToBookmark(bookmark);
				this.verboseJsonEntryAndFeedDeserializer.ReadEntryMetadataPropertyValue(this.CurrentEntryState);
			}
			this.verboseJsonEntryAndFeedDeserializer.JsonReader.StopBuffering();
			this.verboseJsonEntryAndFeedDeserializer.ValidateEntryMetadata(this.CurrentEntryState);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0003A540 File Offset: 0x00038740
		private void ReadExpandedNavigationLinkEnd(bool isCollection)
		{
			base.CurrentNavigationLink.IsCollection = new bool?(isCollection);
			this.ReplaceScope(ODataReaderState.NavigationLinkEnd);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003A55A File Offset: 0x0003875A
		private void StartEntry()
		{
			this.EnterScope(ODataReaderState.EntryStart, ReaderUtils.CreateNewEntry(), base.CurrentEntityType);
			this.CurrentJsonScope.DuplicatePropertyNamesChecker = this.verboseJsonInputContext.CreateDuplicatePropertyNamesChecker();
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003A584 File Offset: 0x00038784
		private void StartNavigationLink(ODataNavigationLink navigationLink, IEdmNavigationProperty navigationProperty)
		{
			IEdmEntityType expectedEntityType = null;
			if (navigationProperty != null)
			{
				IEdmTypeReference type = navigationProperty.Type;
				expectedEntityType = (type.IsCollection() ? type.AsCollection().ElementType().AsEntity().EntityDefinition() : type.AsEntity().EntityDefinition());
			}
			this.EnterScope(ODataReaderState.NavigationLinkStart, navigationLink, expectedEntityType);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0003A5D1 File Offset: 0x000387D1
		private void EnterScope(ODataReaderState state, ODataItem item, IEdmEntityType expectedEntityType)
		{
			base.EnterScope(new ODataVerboseJsonReader.JsonScope(state, item, expectedEntityType));
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0003A5E1 File Offset: 0x000387E1
		private void ReplaceScope(ODataReaderState state)
		{
			base.ReplaceScope(new ODataVerboseJsonReader.JsonScope(state, this.Item, base.CurrentEntityType));
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0003A5FB File Offset: 0x000387FB
		private void EndEntry()
		{
			base.EndEntry(new ODataVerboseJsonReader.JsonScope(ODataReaderState.EntryEnd, this.Item, base.CurrentEntityType));
		}

		// Token: 0x040005E0 RID: 1504
		private readonly ODataVerboseJsonInputContext verboseJsonInputContext;

		// Token: 0x040005E1 RID: 1505
		private readonly ODataVerboseJsonEntryAndFeedDeserializer verboseJsonEntryAndFeedDeserializer;

		// Token: 0x0200020C RID: 524
		private sealed class JsonScope : ODataReaderCore.Scope, IODataVerboseJsonReaderEntryState
		{
			// Token: 0x06001002 RID: 4098 RVA: 0x0003A615 File Offset: 0x00038815
			internal JsonScope(ODataReaderState state, ODataItem item, IEdmEntityType expectedEntityType) : base(state, item, null, expectedEntityType)
			{
			}

			// Token: 0x17000356 RID: 854
			// (get) Token: 0x06001003 RID: 4099 RVA: 0x0003A621 File Offset: 0x00038821
			// (set) Token: 0x06001004 RID: 4100 RVA: 0x0003A629 File Offset: 0x00038829
			public bool MetadataPropertyFound { get; set; }

			// Token: 0x17000357 RID: 855
			// (get) Token: 0x06001005 RID: 4101 RVA: 0x0003A632 File Offset: 0x00038832
			// (set) Token: 0x06001006 RID: 4102 RVA: 0x0003A63A File Offset: 0x0003883A
			public ODataNavigationLink FirstNavigationLink { get; set; }

			// Token: 0x17000358 RID: 856
			// (get) Token: 0x06001007 RID: 4103 RVA: 0x0003A643 File Offset: 0x00038843
			// (set) Token: 0x06001008 RID: 4104 RVA: 0x0003A64B File Offset: 0x0003884B
			public IEdmNavigationProperty FirstNavigationProperty { get; set; }

			// Token: 0x17000359 RID: 857
			// (get) Token: 0x06001009 RID: 4105 RVA: 0x0003A654 File Offset: 0x00038854
			// (set) Token: 0x0600100A RID: 4106 RVA: 0x0003A65C File Offset: 0x0003885C
			public DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker { get; set; }

			// Token: 0x1700035A RID: 858
			// (get) Token: 0x0600100B RID: 4107 RVA: 0x0003A665 File Offset: 0x00038865
			// (set) Token: 0x0600100C RID: 4108 RVA: 0x0003A66D File Offset: 0x0003886D
			public bool FeedInExpandedNavigationLinkInRequest { get; set; }

			// Token: 0x1700035B RID: 859
			// (get) Token: 0x0600100D RID: 4109 RVA: 0x0003A676 File Offset: 0x00038876
			// (set) Token: 0x0600100E RID: 4110 RVA: 0x0003A67E File Offset: 0x0003887E
			public bool FeedHasResultsWrapper { get; set; }

			// Token: 0x1700035C RID: 860
			// (get) Token: 0x0600100F RID: 4111 RVA: 0x0003A687 File Offset: 0x00038887
			// (set) Token: 0x06001010 RID: 4112 RVA: 0x0003A68F File Offset: 0x0003888F
			public bool ExpandedNavigationLinkInRequestHasContent { get; set; }

			// Token: 0x1700035D RID: 861
			// (get) Token: 0x06001011 RID: 4113 RVA: 0x0003A698 File Offset: 0x00038898
			ODataEntry IODataVerboseJsonReaderEntryState.Entry
			{
				get
				{
					return (ODataEntry)base.Item;
				}
			}

			// Token: 0x1700035E RID: 862
			// (get) Token: 0x06001012 RID: 4114 RVA: 0x0003A6A5 File Offset: 0x000388A5
			IEdmEntityType IODataVerboseJsonReaderEntryState.EntityType
			{
				get
				{
					return base.EntityType;
				}
			}
		}
	}
}
