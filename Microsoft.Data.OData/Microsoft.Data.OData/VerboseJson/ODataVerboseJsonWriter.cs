using System;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000291 RID: 657
	internal sealed class ODataVerboseJsonWriter : ODataWriterCore
	{
		// Token: 0x06001622 RID: 5666 RVA: 0x000509B3 File Offset: 0x0004EBB3
		internal ODataVerboseJsonWriter(ODataVerboseJsonOutputContext jsonOutputContext, IEdmEntitySet entitySet, IEdmEntityType entityType, bool writingFeed) : base(jsonOutputContext, entitySet, entityType, writingFeed)
		{
			this.verboseJsonOutputContext = jsonOutputContext;
			this.verboseJsonEntryAndFeedSerializer = new ODataVerboseJsonEntryAndFeedSerializer(this.verboseJsonOutputContext);
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001623 RID: 5667 RVA: 0x000509D8 File Offset: 0x0004EBD8
		private ODataVerboseJsonWriter.VerboseJsonFeedScope CurrentFeedScope
		{
			get
			{
				return base.CurrentScope as ODataVerboseJsonWriter.VerboseJsonFeedScope;
			}
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x000509F2 File Offset: 0x0004EBF2
		protected override void VerifyNotDisposed()
		{
			this.verboseJsonOutputContext.VerifyNotDisposed();
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x000509FF File Offset: 0x0004EBFF
		protected override void FlushSynchronously()
		{
			this.verboseJsonOutputContext.Flush();
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00050A0C File Offset: 0x0004EC0C
		protected override Task FlushAsynchronously()
		{
			return this.verboseJsonOutputContext.FlushAsync();
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00050A19 File Offset: 0x0004EC19
		protected override void StartPayload()
		{
			this.verboseJsonEntryAndFeedSerializer.WritePayloadStart();
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00050A26 File Offset: 0x0004EC26
		protected override void EndPayload()
		{
			this.verboseJsonEntryAndFeedSerializer.WritePayloadEnd();
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00050A34 File Offset: 0x0004EC34
		protected override void StartEntry(ODataEntry entry)
		{
			if (entry == null)
			{
				this.verboseJsonOutputContext.JsonWriter.WriteValue(null);
				return;
			}
			this.verboseJsonOutputContext.JsonWriter.StartObjectScope();
			ProjectedPropertiesAnnotation projectedPropertiesAnnotation = ODataWriterCore.GetProjectedPropertiesAnnotation(base.CurrentScope);
			this.verboseJsonEntryAndFeedSerializer.WriteEntryMetadata(entry, projectedPropertiesAnnotation, base.EntryEntityType, base.DuplicatePropertyNamesChecker);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00050A8C File Offset: 0x0004EC8C
		protected override void EndEntry(ODataEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			ProjectedPropertiesAnnotation projectedPropertiesAnnotation = ODataWriterCore.GetProjectedPropertiesAnnotation(base.CurrentScope);
			this.verboseJsonEntryAndFeedSerializer.WriteProperties(base.EntryEntityType, entry.Properties, false, base.DuplicatePropertyNamesChecker, projectedPropertiesAnnotation);
			this.verboseJsonOutputContext.JsonWriter.EndObjectScope();
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00050AD8 File Offset: 0x0004ECD8
		protected override void StartFeed(ODataFeed feed)
		{
			if (base.ParentNavigationLink == null || this.verboseJsonOutputContext.WritingResponse)
			{
				if (this.verboseJsonOutputContext.Version >= ODataVersion.V2 && this.verboseJsonOutputContext.WritingResponse)
				{
					this.verboseJsonOutputContext.JsonWriter.StartObjectScope();
					this.WriteFeedCount(feed);
					this.verboseJsonOutputContext.JsonWriter.WriteDataArrayName();
				}
				this.verboseJsonOutputContext.JsonWriter.StartArrayScope();
			}
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00050B4C File Offset: 0x0004ED4C
		protected override void EndFeed(ODataFeed feed)
		{
			if (base.ParentNavigationLink == null || this.verboseJsonOutputContext.WritingResponse)
			{
				this.verboseJsonOutputContext.JsonWriter.EndArrayScope();
				Uri nextPageLink = feed.NextPageLink;
				if (this.verboseJsonOutputContext.Version >= ODataVersion.V2 && this.verboseJsonOutputContext.WritingResponse)
				{
					this.WriteFeedCount(feed);
					if (nextPageLink != null)
					{
						this.verboseJsonOutputContext.JsonWriter.WriteName("__next");
						this.verboseJsonOutputContext.JsonWriter.WriteValue(this.verboseJsonEntryAndFeedSerializer.UriToAbsoluteUriString(nextPageLink));
					}
					this.verboseJsonOutputContext.JsonWriter.EndObjectScope();
				}
			}
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00050BF4 File Offset: 0x0004EDF4
		protected override void WriteDeferredNavigationLink(ODataNavigationLink navigationLink)
		{
			WriterValidationUtils.ValidateNavigationLinkUrlPresent(navigationLink);
			this.verboseJsonOutputContext.JsonWriter.WriteName(navigationLink.Name);
			this.verboseJsonOutputContext.JsonWriter.StartObjectScope();
			this.verboseJsonOutputContext.JsonWriter.WriteName("__deferred");
			this.verboseJsonOutputContext.JsonWriter.StartObjectScope();
			this.verboseJsonOutputContext.JsonWriter.WriteName("uri");
			this.verboseJsonOutputContext.JsonWriter.WriteValue(this.verboseJsonEntryAndFeedSerializer.UriToAbsoluteUriString(navigationLink.Url));
			this.verboseJsonOutputContext.JsonWriter.EndObjectScope();
			this.verboseJsonOutputContext.JsonWriter.EndObjectScope();
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x00050CA8 File Offset: 0x0004EEA8
		protected override void StartNavigationLinkWithContent(ODataNavigationLink navigationLink)
		{
			this.verboseJsonOutputContext.JsonWriter.WriteName(navigationLink.Name);
			if (this.verboseJsonOutputContext.WritingResponse)
			{
				return;
			}
			WriterValidationUtils.ValidateNavigationLinkHasCardinality(navigationLink);
			if (navigationLink.IsCollection.Value)
			{
				this.verboseJsonOutputContext.JsonWriter.StartArrayScope();
			}
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x00050D00 File Offset: 0x0004EF00
		protected override void EndNavigationLinkWithContent(ODataNavigationLink navigationLink)
		{
			if (this.verboseJsonOutputContext.WritingResponse)
			{
				return;
			}
			if (navigationLink.IsCollection.Value)
			{
				this.verboseJsonOutputContext.JsonWriter.EndArrayScope();
			}
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x00050D3C File Offset: 0x0004EF3C
		protected override void WriteEntityReferenceInNavigationLinkContent(ODataNavigationLink parentNavigationLink, ODataEntityReferenceLink entityReferenceLink)
		{
			this.verboseJsonOutputContext.JsonWriter.StartObjectScope();
			this.verboseJsonOutputContext.JsonWriter.WriteName("__metadata");
			this.verboseJsonOutputContext.JsonWriter.StartObjectScope();
			this.verboseJsonOutputContext.JsonWriter.WriteName("uri");
			this.verboseJsonOutputContext.JsonWriter.WriteValue(this.verboseJsonEntryAndFeedSerializer.UriToAbsoluteUriString(entityReferenceLink.Url));
			this.verboseJsonOutputContext.JsonWriter.EndObjectScope();
			this.verboseJsonOutputContext.JsonWriter.EndObjectScope();
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00050DD4 File Offset: 0x0004EFD4
		protected override ODataWriterCore.FeedScope CreateFeedScope(ODataFeed feed, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
		{
			return new ODataVerboseJsonWriter.VerboseJsonFeedScope(feed, entitySet, entityType, skipWriting, selectedProperties);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00050DE2 File Offset: 0x0004EFE2
		protected override ODataWriterCore.EntryScope CreateEntryScope(ODataEntry entry, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
		{
			return new ODataWriterCore.EntryScope(entry, base.GetEntrySerializationInfo(entry), entitySet, entityType, skipWriting, this.verboseJsonOutputContext.WritingResponse, this.verboseJsonOutputContext.MessageWriterSettings.WriterBehavior, selectedProperties);
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00050E14 File Offset: 0x0004F014
		private void WriteFeedCount(ODataFeed feed)
		{
			long? count = feed.Count;
			if (count != null && !this.CurrentFeedScope.CountWritten)
			{
				this.verboseJsonOutputContext.JsonWriter.WriteName("__count");
				this.verboseJsonOutputContext.JsonWriter.WriteValue(count.Value);
				this.CurrentFeedScope.CountWritten = true;
			}
		}

		// Token: 0x040008C2 RID: 2242
		private readonly ODataVerboseJsonOutputContext verboseJsonOutputContext;

		// Token: 0x040008C3 RID: 2243
		private readonly ODataVerboseJsonEntryAndFeedSerializer verboseJsonEntryAndFeedSerializer;

		// Token: 0x02000292 RID: 658
		private sealed class VerboseJsonFeedScope : ODataWriterCore.FeedScope
		{
			// Token: 0x06001634 RID: 5684 RVA: 0x00050E76 File Offset: 0x0004F076
			internal VerboseJsonFeedScope(ODataFeed feed, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties) : base(feed, entitySet, entityType, skipWriting, selectedProperties)
			{
			}

			// Token: 0x17000474 RID: 1140
			// (get) Token: 0x06001635 RID: 5685 RVA: 0x00050E85 File Offset: 0x0004F085
			// (set) Token: 0x06001636 RID: 5686 RVA: 0x00050E8D File Offset: 0x0004F08D
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

			// Token: 0x040008C4 RID: 2244
			private bool countWritten;
		}
	}
}
