﻿using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Evaluation;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x020001AA RID: 426
	internal sealed class ODataJsonLightWriter : ODataWriterCore
	{
		// Token: 0x06000D29 RID: 3369 RVA: 0x0002D504 File Offset: 0x0002B704
		internal ODataJsonLightWriter(ODataJsonLightOutputContext jsonLightOutputContext, IEdmEntitySet entitySet, IEdmEntityType entityType, bool writingFeed) : base(jsonLightOutputContext, entitySet, entityType, writingFeed)
		{
			this.jsonLightOutputContext = jsonLightOutputContext;
			this.jsonLightEntryAndFeedSerializer = new ODataJsonLightEntryAndFeedSerializer(this.jsonLightOutputContext);
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0002D52C File Offset: 0x0002B72C
		private ODataJsonLightWriter.JsonLightEntryScope CurrentEntryScope
		{
			get
			{
				return base.CurrentScope as ODataJsonLightWriter.JsonLightEntryScope;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x0002D548 File Offset: 0x0002B748
		private ODataJsonLightWriter.JsonLightFeedScope CurrentFeedScope
		{
			get
			{
				return base.CurrentScope as ODataJsonLightWriter.JsonLightFeedScope;
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0002D562 File Offset: 0x0002B762
		protected override void VerifyNotDisposed()
		{
			this.jsonLightOutputContext.VerifyNotDisposed();
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0002D56F File Offset: 0x0002B76F
		protected override void FlushSynchronously()
		{
			this.jsonLightOutputContext.Flush();
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0002D57C File Offset: 0x0002B77C
		protected override Task FlushAsynchronously()
		{
			return this.jsonLightOutputContext.FlushAsync();
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0002D589 File Offset: 0x0002B789
		protected override void StartPayload()
		{
			this.jsonLightEntryAndFeedSerializer.WritePayloadStart();
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0002D596 File Offset: 0x0002B796
		protected override void EndPayload()
		{
			this.jsonLightEntryAndFeedSerializer.WritePayloadEnd();
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0002D5A4 File Offset: 0x0002B7A4
		protected override void PrepareEntryForWriteStart(ODataEntry entry, ODataFeedAndEntryTypeContext typeContext, SelectedPropertiesNode selectedProperties)
		{
			if (this.jsonLightOutputContext.MessageWriterSettings.AutoComputePayloadMetadataInJson)
			{
				ODataWriterCore.EntryScope entryScope = (ODataWriterCore.EntryScope)base.CurrentScope;
				ODataEntityMetadataBuilder builder = this.jsonLightOutputContext.MetadataLevel.CreateEntityMetadataBuilder(entry, typeContext, entryScope.SerializationInfo, entryScope.EntityType, selectedProperties, this.jsonLightOutputContext.WritingResponse, this.jsonLightOutputContext.MessageWriterSettings.AutoGeneratedUrlsShouldPutKeyValueInDedicatedSegment);
				this.jsonLightOutputContext.MetadataLevel.InjectMetadataBuilder(entry, builder);
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0002D61C File Offset: 0x0002B81C
		protected override void ValidateEntryMediaResource(ODataEntry entry, IEdmEntityType entityType)
		{
			if (this.jsonLightOutputContext.MessageWriterSettings.AutoComputePayloadMetadataInJson && this.jsonLightOutputContext.MetadataLevel is JsonNoMetadataLevel)
			{
				return;
			}
			base.ValidateEntryMediaResource(entry, entityType);
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0002D64C File Offset: 0x0002B84C
		protected override void StartEntry(ODataEntry entry)
		{
			ODataNavigationLink parentNavigationLink = base.ParentNavigationLink;
			if (parentNavigationLink != null)
			{
				this.jsonLightOutputContext.JsonWriter.WriteName(parentNavigationLink.Name);
			}
			if (entry == null)
			{
				this.jsonLightOutputContext.JsonWriter.WriteValue(null);
				return;
			}
			this.jsonLightOutputContext.JsonWriter.StartObjectScope();
			ODataJsonLightWriter.JsonLightEntryScope currentEntryScope = this.CurrentEntryScope;
			if (base.IsTopLevel)
			{
				this.jsonLightEntryAndFeedSerializer.TryWriteEntryMetadataUri(currentEntryScope.GetOrCreateTypeContext(this.jsonLightOutputContext.Model, this.jsonLightOutputContext.WritingResponse));
			}
			this.jsonLightEntryAndFeedSerializer.WriteAnnotationGroup(entry);
			this.jsonLightEntryAndFeedSerializer.WriteEntryStartMetadataProperties(currentEntryScope);
			this.jsonLightEntryAndFeedSerializer.WriteEntryMetadataProperties(currentEntryScope);
			this.jsonLightEntryAndFeedSerializer.InstanceAnnotationWriter.WriteInstanceAnnotations(entry.InstanceAnnotations, currentEntryScope.InstanceAnnotationWriteTracker);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0002D714 File Offset: 0x0002B914
		protected override void EndEntry(ODataEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			ODataJsonLightWriter.JsonLightEntryScope currentEntryScope = this.CurrentEntryScope;
			ProjectedPropertiesAnnotation projectedPropertiesAnnotation = ODataWriterCore.GetProjectedPropertiesAnnotation(currentEntryScope);
			this.jsonLightEntryAndFeedSerializer.WriteEntryMetadataProperties(currentEntryScope);
			this.jsonLightEntryAndFeedSerializer.InstanceAnnotationWriter.WriteInstanceAnnotations(entry.InstanceAnnotations, currentEntryScope.InstanceAnnotationWriteTracker);
			this.jsonLightEntryAndFeedSerializer.WriteEntryEndMetadataProperties(currentEntryScope, currentEntryScope.DuplicatePropertyNamesChecker);
			this.jsonLightEntryAndFeedSerializer.WriteProperties(base.EntryEntityType, entry.Properties, false, base.DuplicatePropertyNamesChecker, projectedPropertiesAnnotation);
			this.jsonLightOutputContext.JsonWriter.EndObjectScope();
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0002D79C File Offset: 0x0002B99C
		protected override void StartFeed(ODataFeed feed)
		{
			IJsonWriter jsonWriter = this.jsonLightOutputContext.JsonWriter;
			if (base.ParentNavigationLink == null)
			{
				jsonWriter.StartObjectScope();
				this.jsonLightEntryAndFeedSerializer.TryWriteFeedMetadataUri(this.CurrentFeedScope.GetOrCreateTypeContext(this.jsonLightOutputContext.Model, this.jsonLightOutputContext.WritingResponse));
				if (this.jsonLightOutputContext.WritingResponse)
				{
					this.WriteFeedCount(feed, null);
					this.WriteFeedNextLink(feed, null);
					this.WriteFeedDeltaLink(feed);
				}
				this.jsonLightEntryAndFeedSerializer.InstanceAnnotationWriter.WriteInstanceAnnotations(feed.InstanceAnnotations, this.CurrentFeedScope.InstanceAnnotationWriteTracker);
				jsonWriter.WriteValuePropertyName();
				jsonWriter.StartArrayScope();
				return;
			}
			string name = base.ParentNavigationLink.Name;
			base.ValidateNoDeltaLinkForExpandedFeed(feed);
			this.ValidateNoCustomInstanceAnnotationsForExpandedFeed(feed);
			if (this.jsonLightOutputContext.WritingResponse)
			{
				this.WriteFeedCount(feed, name);
				this.WriteFeedNextLink(feed, name);
				jsonWriter.WriteName(name);
				jsonWriter.StartArrayScope();
				return;
			}
			ODataJsonLightWriter.JsonLightNavigationLinkScope jsonLightNavigationLinkScope = (ODataJsonLightWriter.JsonLightNavigationLinkScope)base.ParentNavigationLinkScope;
			if (!jsonLightNavigationLinkScope.FeedWritten)
			{
				if (jsonLightNavigationLinkScope.EntityReferenceLinkWritten)
				{
					jsonWriter.EndArrayScope();
				}
				jsonWriter.WriteName(name);
				jsonWriter.StartArrayScope();
				jsonLightNavigationLinkScope.FeedWritten = true;
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0002D8C0 File Offset: 0x0002BAC0
		protected override void EndFeed(ODataFeed feed)
		{
			bool flag = base.ParentNavigationLink == null;
			if (flag)
			{
				this.jsonLightOutputContext.JsonWriter.EndArrayScope();
				this.jsonLightEntryAndFeedSerializer.InstanceAnnotationWriter.WriteInstanceAnnotations(feed.InstanceAnnotations, this.CurrentFeedScope.InstanceAnnotationWriteTracker);
				if (this.jsonLightOutputContext.WritingResponse)
				{
					this.WriteFeedCount(feed, null);
					this.WriteFeedNextLink(feed, null);
					this.WriteFeedDeltaLink(feed);
				}
				this.jsonLightOutputContext.JsonWriter.EndObjectScope();
				return;
			}
			string name = base.ParentNavigationLink.Name;
			base.ValidateNoDeltaLinkForExpandedFeed(feed);
			this.ValidateNoCustomInstanceAnnotationsForExpandedFeed(feed);
			if (this.jsonLightOutputContext.WritingResponse)
			{
				this.jsonLightOutputContext.JsonWriter.EndArrayScope();
				this.WriteFeedCount(feed, name);
				this.WriteFeedNextLink(feed, name);
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0002D987 File Offset: 0x0002BB87
		protected override void WriteDeferredNavigationLink(ODataNavigationLink navigationLink)
		{
			this.jsonLightEntryAndFeedSerializer.WriteNavigationLinkMetadata(navigationLink, base.DuplicatePropertyNamesChecker);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0002D99B File Offset: 0x0002BB9B
		protected override void StartNavigationLinkWithContent(ODataNavigationLink navigationLink)
		{
			if (this.jsonLightOutputContext.WritingResponse)
			{
				this.jsonLightEntryAndFeedSerializer.WriteNavigationLinkMetadata(navigationLink, base.DuplicatePropertyNamesChecker);
				return;
			}
			WriterValidationUtils.ValidateNavigationLinkHasCardinality(navigationLink);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0002D9C4 File Offset: 0x0002BBC4
		protected override void EndNavigationLinkWithContent(ODataNavigationLink navigationLink)
		{
			if (!this.jsonLightOutputContext.WritingResponse)
			{
				ODataJsonLightWriter.JsonLightNavigationLinkScope jsonLightNavigationLinkScope = (ODataJsonLightWriter.JsonLightNavigationLinkScope)base.CurrentScope;
				if (jsonLightNavigationLinkScope.EntityReferenceLinkWritten && !jsonLightNavigationLinkScope.FeedWritten && navigationLink.IsCollection.Value)
				{
					this.jsonLightOutputContext.JsonWriter.EndArrayScope();
				}
				if (jsonLightNavigationLinkScope.FeedWritten)
				{
					this.jsonLightOutputContext.JsonWriter.EndArrayScope();
				}
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0002DA34 File Offset: 0x0002BC34
		protected override void WriteEntityReferenceInNavigationLinkContent(ODataNavigationLink parentNavigationLink, ODataEntityReferenceLink entityReferenceLink)
		{
			ODataJsonLightWriter.JsonLightNavigationLinkScope jsonLightNavigationLinkScope = (ODataJsonLightWriter.JsonLightNavigationLinkScope)base.CurrentScope;
			if (jsonLightNavigationLinkScope.FeedWritten)
			{
				throw new ODataException(Strings.ODataJsonLightWriter_EntityReferenceLinkAfterFeedInRequest);
			}
			if (!jsonLightNavigationLinkScope.EntityReferenceLinkWritten)
			{
				this.jsonLightOutputContext.JsonWriter.WritePropertyAnnotationName(parentNavigationLink.Name, "odata.bind");
				if (parentNavigationLink.IsCollection.Value)
				{
					this.jsonLightOutputContext.JsonWriter.StartArrayScope();
				}
				jsonLightNavigationLinkScope.EntityReferenceLinkWritten = true;
			}
			this.jsonLightOutputContext.JsonWriter.WriteValue(this.jsonLightEntryAndFeedSerializer.UriToString(entityReferenceLink.Url));
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0002DACB File Offset: 0x0002BCCB
		protected override ODataWriterCore.FeedScope CreateFeedScope(ODataFeed feed, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
		{
			return new ODataJsonLightWriter.JsonLightFeedScope(feed, entitySet, entityType, skipWriting, selectedProperties);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0002DAD9 File Offset: 0x0002BCD9
		protected override ODataWriterCore.EntryScope CreateEntryScope(ODataEntry entry, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
		{
			return new ODataJsonLightWriter.JsonLightEntryScope(entry, base.GetEntrySerializationInfo(entry), entitySet, entityType, skipWriting, this.jsonLightOutputContext.WritingResponse, this.jsonLightOutputContext.MessageWriterSettings.WriterBehavior, selectedProperties);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0002DB09 File Offset: 0x0002BD09
		protected override ODataWriterCore.NavigationLinkScope CreateNavigationLinkScope(ODataWriterCore.WriterState writerState, ODataNavigationLink navLink, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
		{
			return new ODataJsonLightWriter.JsonLightNavigationLinkScope(writerState, navLink, entitySet, entityType, skipWriting, selectedProperties);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0002DB1C File Offset: 0x0002BD1C
		private void WriteFeedCount(ODataFeed feed, string propertyName)
		{
			long? count = feed.Count;
			if (count != null && !this.CurrentFeedScope.CountWritten)
			{
				if (propertyName == null)
				{
					this.jsonLightOutputContext.JsonWriter.WriteName("odata.count");
				}
				else
				{
					this.jsonLightOutputContext.JsonWriter.WritePropertyAnnotationName(propertyName, "odata.count");
				}
				this.jsonLightOutputContext.JsonWriter.WriteValue(count.Value);
				this.CurrentFeedScope.CountWritten = true;
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0002DB9C File Offset: 0x0002BD9C
		private void WriteFeedNextLink(ODataFeed feed, string propertyName)
		{
			Uri nextPageLink = feed.NextPageLink;
			if (nextPageLink != null && !this.CurrentFeedScope.NextPageLinkWritten)
			{
				if (propertyName == null)
				{
					this.jsonLightOutputContext.JsonWriter.WriteName("odata.nextLink");
				}
				else
				{
					this.jsonLightOutputContext.JsonWriter.WritePropertyAnnotationName(propertyName, "odata.nextLink");
				}
				this.jsonLightOutputContext.JsonWriter.WriteValue(this.jsonLightEntryAndFeedSerializer.UriToString(nextPageLink));
				this.CurrentFeedScope.NextPageLinkWritten = true;
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0002DC20 File Offset: 0x0002BE20
		private void WriteFeedDeltaLink(ODataFeed feed)
		{
			Uri deltaLink = feed.DeltaLink;
			if (deltaLink != null && !this.CurrentFeedScope.DeltaLinkWritten)
			{
				this.jsonLightOutputContext.JsonWriter.WriteName("odata.deltaLink");
				this.jsonLightOutputContext.JsonWriter.WriteValue(this.jsonLightEntryAndFeedSerializer.UriToString(deltaLink));
				this.CurrentFeedScope.DeltaLinkWritten = true;
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0002DC87 File Offset: 0x0002BE87
		private void ValidateNoCustomInstanceAnnotationsForExpandedFeed(ODataFeed feed)
		{
			if (feed.InstanceAnnotations.Count > 0)
			{
				throw new ODataException(Strings.ODataJsonLightWriter_InstanceAnnotationNotSupportedOnExpandedFeed);
			}
		}

		// Token: 0x0400046B RID: 1131
		private readonly ODataJsonLightOutputContext jsonLightOutputContext;

		// Token: 0x0400046C RID: 1132
		private readonly ODataJsonLightEntryAndFeedSerializer jsonLightEntryAndFeedSerializer;

		// Token: 0x020001AB RID: 427
		private sealed class JsonLightFeedScope : ODataWriterCore.FeedScope
		{
			// Token: 0x06000D42 RID: 3394 RVA: 0x0002DCA2 File Offset: 0x0002BEA2
			internal JsonLightFeedScope(ODataFeed feed, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties) : base(feed, entitySet, entityType, skipWriting, selectedProperties)
			{
			}

			// Token: 0x170002D9 RID: 729
			// (get) Token: 0x06000D43 RID: 3395 RVA: 0x0002DCB1 File Offset: 0x0002BEB1
			// (set) Token: 0x06000D44 RID: 3396 RVA: 0x0002DCB9 File Offset: 0x0002BEB9
			internal bool CountWritten
			{
				get
				{
					return this.countWritten;
				}
				set
				{
					this.countWritten = value;
				}
			}

			// Token: 0x170002DA RID: 730
			// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0002DCC2 File Offset: 0x0002BEC2
			// (set) Token: 0x06000D46 RID: 3398 RVA: 0x0002DCCA File Offset: 0x0002BECA
			internal bool NextPageLinkWritten
			{
				get
				{
					return this.nextLinkWritten;
				}
				set
				{
					this.nextLinkWritten = value;
				}
			}

			// Token: 0x170002DB RID: 731
			// (get) Token: 0x06000D47 RID: 3399 RVA: 0x0002DCD3 File Offset: 0x0002BED3
			// (set) Token: 0x06000D48 RID: 3400 RVA: 0x0002DCDB File Offset: 0x0002BEDB
			internal bool DeltaLinkWritten
			{
				get
				{
					return this.deltaLinkWritten;
				}
				set
				{
					this.deltaLinkWritten = value;
				}
			}

			// Token: 0x0400046D RID: 1133
			private bool countWritten;

			// Token: 0x0400046E RID: 1134
			private bool nextLinkWritten;

			// Token: 0x0400046F RID: 1135
			private bool deltaLinkWritten;
		}

		// Token: 0x020001AC RID: 428
		private sealed class JsonLightEntryScope : ODataWriterCore.EntryScope, IODataJsonLightWriterEntryState
		{
			// Token: 0x06000D49 RID: 3401 RVA: 0x0002DCE4 File Offset: 0x0002BEE4
			internal JsonLightEntryScope(ODataEntry entry, ODataFeedAndEntrySerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, bool writingResponse, ODataWriterBehavior writerBehavior, SelectedPropertiesNode selectedProperties) : base(entry, serializationInfo, entitySet, entityType, skipWriting, writingResponse, writerBehavior, selectedProperties)
			{
			}

			// Token: 0x170002DC RID: 732
			// (get) Token: 0x06000D4A RID: 3402 RVA: 0x0002DD04 File Offset: 0x0002BF04
			public ODataEntry Entry
			{
				get
				{
					return (ODataEntry)base.Item;
				}
			}

			// Token: 0x170002DD RID: 733
			// (get) Token: 0x06000D4B RID: 3403 RVA: 0x0002DD11 File Offset: 0x0002BF11
			// (set) Token: 0x06000D4C RID: 3404 RVA: 0x0002DD1A File Offset: 0x0002BF1A
			public bool EditLinkWritten
			{
				get
				{
					return this.IsMetadataPropertyWritten(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.EditLink);
				}
				set
				{
					this.SetWrittenMetadataProperty(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.EditLink);
				}
			}

			// Token: 0x170002DE RID: 734
			// (get) Token: 0x06000D4D RID: 3405 RVA: 0x0002DD23 File Offset: 0x0002BF23
			// (set) Token: 0x06000D4E RID: 3406 RVA: 0x0002DD2C File Offset: 0x0002BF2C
			public bool ReadLinkWritten
			{
				get
				{
					return this.IsMetadataPropertyWritten(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.ReadLink);
				}
				set
				{
					this.SetWrittenMetadataProperty(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.ReadLink);
				}
			}

			// Token: 0x170002DF RID: 735
			// (get) Token: 0x06000D4F RID: 3407 RVA: 0x0002DD35 File Offset: 0x0002BF35
			// (set) Token: 0x06000D50 RID: 3408 RVA: 0x0002DD3E File Offset: 0x0002BF3E
			public bool MediaEditLinkWritten
			{
				get
				{
					return this.IsMetadataPropertyWritten(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.MediaEditLink);
				}
				set
				{
					this.SetWrittenMetadataProperty(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.MediaEditLink);
				}
			}

			// Token: 0x170002E0 RID: 736
			// (get) Token: 0x06000D51 RID: 3409 RVA: 0x0002DD47 File Offset: 0x0002BF47
			// (set) Token: 0x06000D52 RID: 3410 RVA: 0x0002DD50 File Offset: 0x0002BF50
			public bool MediaReadLinkWritten
			{
				get
				{
					return this.IsMetadataPropertyWritten(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.MediaReadLink);
				}
				set
				{
					this.SetWrittenMetadataProperty(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.MediaReadLink);
				}
			}

			// Token: 0x170002E1 RID: 737
			// (get) Token: 0x06000D53 RID: 3411 RVA: 0x0002DD59 File Offset: 0x0002BF59
			// (set) Token: 0x06000D54 RID: 3412 RVA: 0x0002DD63 File Offset: 0x0002BF63
			public bool MediaContentTypeWritten
			{
				get
				{
					return this.IsMetadataPropertyWritten(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.MediaContentType);
				}
				set
				{
					this.SetWrittenMetadataProperty(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.MediaContentType);
				}
			}

			// Token: 0x170002E2 RID: 738
			// (get) Token: 0x06000D55 RID: 3413 RVA: 0x0002DD6D File Offset: 0x0002BF6D
			// (set) Token: 0x06000D56 RID: 3414 RVA: 0x0002DD77 File Offset: 0x0002BF77
			public bool MediaETagWritten
			{
				get
				{
					return this.IsMetadataPropertyWritten(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.MediaETag);
				}
				set
				{
					this.SetWrittenMetadataProperty(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty.MediaETag);
				}
			}

			// Token: 0x06000D57 RID: 3415 RVA: 0x0002DD81 File Offset: 0x0002BF81
			private void SetWrittenMetadataProperty(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty jsonLightMetadataProperty)
			{
				this.alreadyWrittenMetadataProperties |= (int)jsonLightMetadataProperty;
			}

			// Token: 0x06000D58 RID: 3416 RVA: 0x0002DD91 File Offset: 0x0002BF91
			private bool IsMetadataPropertyWritten(ODataJsonLightWriter.JsonLightEntryScope.JsonLightEntryMetadataProperty jsonLightMetadataProperty)
			{
				return (this.alreadyWrittenMetadataProperties & (int)jsonLightMetadataProperty) == (int)jsonLightMetadataProperty;
			}

			// Token: 0x04000470 RID: 1136
			private int alreadyWrittenMetadataProperties;

			// Token: 0x020001AD RID: 429
			[Flags]
			private enum JsonLightEntryMetadataProperty
			{
				// Token: 0x04000472 RID: 1138
				EditLink = 1,
				// Token: 0x04000473 RID: 1139
				ReadLink = 2,
				// Token: 0x04000474 RID: 1140
				MediaEditLink = 4,
				// Token: 0x04000475 RID: 1141
				MediaReadLink = 8,
				// Token: 0x04000476 RID: 1142
				MediaContentType = 16,
				// Token: 0x04000477 RID: 1143
				MediaETag = 32
			}
		}

		// Token: 0x020001AE RID: 430
		private sealed class JsonLightNavigationLinkScope : ODataWriterCore.NavigationLinkScope
		{
			// Token: 0x06000D59 RID: 3417 RVA: 0x0002DD9E File Offset: 0x0002BF9E
			internal JsonLightNavigationLinkScope(ODataWriterCore.WriterState writerState, ODataNavigationLink navLink, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties) : base(writerState, navLink, entitySet, entityType, skipWriting, selectedProperties)
			{
			}

			// Token: 0x170002E3 RID: 739
			// (get) Token: 0x06000D5A RID: 3418 RVA: 0x0002DDAF File Offset: 0x0002BFAF
			// (set) Token: 0x06000D5B RID: 3419 RVA: 0x0002DDB7 File Offset: 0x0002BFB7
			internal bool EntityReferenceLinkWritten
			{
				get
				{
					return this.entityReferenceLinkWritten;
				}
				set
				{
					this.entityReferenceLinkWritten = value;
				}
			}

			// Token: 0x170002E4 RID: 740
			// (get) Token: 0x06000D5C RID: 3420 RVA: 0x0002DDC0 File Offset: 0x0002BFC0
			// (set) Token: 0x06000D5D RID: 3421 RVA: 0x0002DDC8 File Offset: 0x0002BFC8
			internal bool FeedWritten
			{
				get
				{
					return this.feedWritten;
				}
				set
				{
					this.feedWritten = value;
				}
			}

			// Token: 0x06000D5E RID: 3422 RVA: 0x0002DDD4 File Offset: 0x0002BFD4
			internal override ODataWriterCore.NavigationLinkScope Clone(ODataWriterCore.WriterState newWriterState)
			{
				return new ODataJsonLightWriter.JsonLightNavigationLinkScope(newWriterState, (ODataNavigationLink)base.Item, base.EntitySet, base.EntityType, base.SkipWriting, base.SelectedProperties)
				{
					EntityReferenceLinkWritten = this.entityReferenceLinkWritten,
					FeedWritten = this.feedWritten
				};
			}

			// Token: 0x04000478 RID: 1144
			private bool entityReferenceLinkWritten;

			// Token: 0x04000479 RID: 1145
			private bool feedWritten;
		}
	}
}