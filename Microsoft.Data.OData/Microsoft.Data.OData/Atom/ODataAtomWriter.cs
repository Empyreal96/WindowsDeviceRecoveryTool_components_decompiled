using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200029B RID: 667
	internal sealed class ODataAtomWriter : ODataWriterCore
	{
		// Token: 0x0600167A RID: 5754 RVA: 0x000519A4 File Offset: 0x0004FBA4
		internal ODataAtomWriter(ODataAtomOutputContext atomOutputContext, IEdmEntitySet entitySet, IEdmEntityType entityType, bool writingFeed) : base(atomOutputContext, entitySet, entityType, writingFeed)
		{
			this.atomOutputContext = atomOutputContext;
			if (this.atomOutputContext.MessageWriterSettings.AtomStartEntryXmlCustomizationCallback != null)
			{
				this.atomOutputContext.InitializeWriterCustomization();
			}
			this.atomEntryAndFeedSerializer = new ODataAtomEntryAndFeedSerializer(this.atomOutputContext);
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x00051A04 File Offset: 0x0004FC04
		private ODataAtomWriter.AtomEntryScope CurrentEntryScope
		{
			get
			{
				return base.CurrentScope as ODataAtomWriter.AtomEntryScope;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x00051A20 File Offset: 0x0004FC20
		private ODataAtomWriter.AtomFeedScope CurrentFeedScope
		{
			get
			{
				return base.CurrentScope as ODataAtomWriter.AtomFeedScope;
			}
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00051A3A File Offset: 0x0004FC3A
		protected override void VerifyNotDisposed()
		{
			this.atomOutputContext.VerifyNotDisposed();
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00051A47 File Offset: 0x0004FC47
		protected override void FlushSynchronously()
		{
			this.atomOutputContext.Flush();
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00051A54 File Offset: 0x0004FC54
		protected override Task FlushAsynchronously()
		{
			return this.atomOutputContext.FlushAsync();
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x00051A61 File Offset: 0x0004FC61
		protected override void StartPayload()
		{
			this.atomEntryAndFeedSerializer.WritePayloadStart();
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x00051A6E File Offset: 0x0004FC6E
		protected override void EndPayload()
		{
			this.atomEntryAndFeedSerializer.WritePayloadEnd();
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00051A7C File Offset: 0x0004FC7C
		protected override void StartEntry(ODataEntry entry)
		{
			this.CheckAndWriteParentNavigationLinkStartForInlineElement();
			if (entry == null)
			{
				return;
			}
			this.StartEntryXmlCustomization(entry);
			this.atomOutputContext.XmlWriter.WriteStartElement("", "entry", "http://www.w3.org/2005/Atom");
			if (base.IsTopLevel)
			{
				this.atomEntryAndFeedSerializer.WriteBaseUriAndDefaultNamespaceAttributes();
			}
			string etag = entry.ETag;
			if (etag != null)
			{
				ODataAtomWriterUtils.WriteETag(this.atomOutputContext.XmlWriter, etag);
			}
			ODataAtomWriter.AtomEntryScope currentEntryScope = this.CurrentEntryScope;
			AtomEntryMetadata entryMetadata = entry.Atom();
			string id = entry.Id;
			if (id != null)
			{
				this.atomEntryAndFeedSerializer.WriteEntryId(id);
				currentEntryScope.SetWrittenElement(ODataAtomWriter.AtomElement.Id);
			}
			string entryTypeNameForWriting = this.atomOutputContext.TypeNameOracle.GetEntryTypeNameForWriting(entry);
			this.atomEntryAndFeedSerializer.WriteEntryTypeName(entryTypeNameForWriting, entryMetadata);
			Uri editLink = entry.EditLink;
			if (editLink != null)
			{
				this.atomEntryAndFeedSerializer.WriteEntryEditLink(editLink, entryMetadata);
				currentEntryScope.SetWrittenElement(ODataAtomWriter.AtomElement.EditLink);
			}
			Uri readLink = entry.ReadLink;
			if (readLink != null)
			{
				this.atomEntryAndFeedSerializer.WriteEntryReadLink(readLink, entryMetadata);
				currentEntryScope.SetWrittenElement(ODataAtomWriter.AtomElement.ReadLink);
			}
			this.WriteInstanceAnnotations(entry.InstanceAnnotations, currentEntryScope.InstanceAnnotationWriteTracker);
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00051B94 File Offset: 0x0004FD94
		protected override void EndEntry(ODataEntry entry)
		{
			if (entry == null)
			{
				this.CheckAndWriteParentNavigationLinkEndForInlineElement();
				return;
			}
			IEdmEntityType entryEntityType = base.EntryEntityType;
			EntryPropertiesValueCache entryPropertiesValueCache = new EntryPropertiesValueCache(entry);
			ODataEntityPropertyMappingCache odataEntityPropertyMappingCache = this.atomOutputContext.Model.EnsureEpmCache(entryEntityType, int.MaxValue);
			if (odataEntityPropertyMappingCache != null)
			{
				EpmWriterUtils.CacheEpmProperties(entryPropertiesValueCache, odataEntityPropertyMappingCache.EpmSourceTree);
			}
			ODataAtomWriter.AtomEntryScope currentEntryScope = this.CurrentEntryScope;
			ProjectedPropertiesAnnotation projectedPropertiesAnnotation = ODataWriterCore.GetProjectedPropertiesAnnotation(currentEntryScope);
			AtomEntryMetadata entryMetadata = entry.Atom();
			if (!currentEntryScope.IsElementWritten(ODataAtomWriter.AtomElement.Id))
			{
				this.atomEntryAndFeedSerializer.WriteEntryId(entry.Id);
			}
			Uri editLink = entry.EditLink;
			if (editLink != null && !currentEntryScope.IsElementWritten(ODataAtomWriter.AtomElement.EditLink))
			{
				this.atomEntryAndFeedSerializer.WriteEntryEditLink(editLink, entryMetadata);
			}
			Uri readLink = entry.ReadLink;
			if (readLink != null && !currentEntryScope.IsElementWritten(ODataAtomWriter.AtomElement.ReadLink))
			{
				this.atomEntryAndFeedSerializer.WriteEntryReadLink(readLink, entryMetadata);
			}
			AtomEntryMetadata epmEntryMetadata = null;
			if (odataEntityPropertyMappingCache != null)
			{
				ODataVersionChecker.CheckEntityPropertyMapping(this.atomOutputContext.Version, entryEntityType, this.atomOutputContext.Model);
				epmEntryMetadata = EpmSyndicationWriter.WriteEntryEpm(odataEntityPropertyMappingCache.EpmTargetTree, entryPropertiesValueCache, entryEntityType.ToTypeReference().AsEntity(), this.atomOutputContext);
			}
			this.atomEntryAndFeedSerializer.WriteEntryMetadata(entryMetadata, epmEntryMetadata, this.updatedTime);
			IEnumerable<ODataProperty> entryStreamProperties = entryPropertiesValueCache.EntryStreamProperties;
			if (entryStreamProperties != null)
			{
				foreach (ODataProperty streamProperty in entryStreamProperties)
				{
					this.atomEntryAndFeedSerializer.WriteStreamProperty(streamProperty, entryEntityType, base.DuplicatePropertyNamesChecker, projectedPropertiesAnnotation);
				}
			}
			IEnumerable<ODataAssociationLink> associationLinks = entry.AssociationLinks;
			if (associationLinks != null)
			{
				foreach (ODataAssociationLink associationLink in associationLinks)
				{
					this.atomEntryAndFeedSerializer.WriteAssociationLink(associationLink, entryEntityType, base.DuplicatePropertyNamesChecker, projectedPropertiesAnnotation);
				}
			}
			IEnumerable<ODataAction> actions = entry.Actions;
			if (actions != null)
			{
				foreach (ODataAction operation in actions)
				{
					ValidationUtils.ValidateOperationNotNull(operation, true);
					this.atomEntryAndFeedSerializer.WriteOperation(operation);
				}
			}
			IEnumerable<ODataFunction> functions = entry.Functions;
			if (functions != null)
			{
				foreach (ODataFunction operation2 in functions)
				{
					ValidationUtils.ValidateOperationNotNull(operation2, false);
					this.atomEntryAndFeedSerializer.WriteOperation(operation2);
				}
			}
			this.WriteEntryContent(entry, entryEntityType, entryPropertiesValueCache, (odataEntityPropertyMappingCache == null) ? null : odataEntityPropertyMappingCache.EpmSourceTree.Root, projectedPropertiesAnnotation);
			if (odataEntityPropertyMappingCache != null)
			{
				EpmCustomWriter.WriteEntryEpm(this.atomOutputContext.XmlWriter, odataEntityPropertyMappingCache.EpmTargetTree, entryPropertiesValueCache, entryEntityType.ToTypeReference().AsEntity(), this.atomOutputContext);
			}
			this.WriteInstanceAnnotations(entry.InstanceAnnotations, currentEntryScope.InstanceAnnotationWriteTracker);
			this.atomOutputContext.XmlWriter.WriteEndElement();
			this.EndEntryXmlCustomization(entry);
			this.CheckAndWriteParentNavigationLinkEndForInlineElement();
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x00051E98 File Offset: 0x00050098
		protected override void StartFeed(ODataFeed feed)
		{
			if (string.IsNullOrEmpty(feed.Id))
			{
				throw new ODataException(Strings.ODataAtomWriter_FeedsMustHaveNonEmptyId);
			}
			this.CheckAndWriteParentNavigationLinkStartForInlineElement();
			this.atomOutputContext.XmlWriter.WriteStartElement("", "feed", "http://www.w3.org/2005/Atom");
			if (base.IsTopLevel)
			{
				this.atomEntryAndFeedSerializer.WriteBaseUriAndDefaultNamespaceAttributes();
				if (feed.Count != null)
				{
					this.atomEntryAndFeedSerializer.WriteCount(feed.Count.Value, false);
				}
			}
			bool authorWritten;
			this.atomEntryAndFeedSerializer.WriteFeedMetadata(feed, this.updatedTime, out authorWritten);
			this.CurrentFeedScope.AuthorWritten = authorWritten;
			this.WriteFeedInstanceAnnotations(feed, this.CurrentFeedScope);
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00051F4C File Offset: 0x0005014C
		protected override void EndFeed(ODataFeed feed)
		{
			ODataAtomWriter.AtomFeedScope currentFeedScope = this.CurrentFeedScope;
			if (!currentFeedScope.AuthorWritten && currentFeedScope.EntryCount == 0)
			{
				this.atomEntryAndFeedSerializer.WriteFeedDefaultAuthor();
			}
			this.WriteFeedInstanceAnnotations(feed, currentFeedScope);
			this.atomEntryAndFeedSerializer.WriteFeedNextPageLink(feed);
			if (base.IsTopLevel)
			{
				if (this.atomOutputContext.WritingResponse)
				{
					this.atomEntryAndFeedSerializer.WriteFeedDeltaLink(feed);
				}
			}
			else
			{
				base.ValidateNoDeltaLinkForExpandedFeed(feed);
			}
			this.atomOutputContext.XmlWriter.WriteEndElement();
			this.CheckAndWriteParentNavigationLinkEndForInlineElement();
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x00051FCF File Offset: 0x000501CF
		protected override void WriteDeferredNavigationLink(ODataNavigationLink navigationLink)
		{
			this.WriteNavigationLinkStart(navigationLink, null);
			this.WriteNavigationLinkEnd();
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x00051FDF File Offset: 0x000501DF
		protected override void StartNavigationLinkWithContent(ODataNavigationLink navigationLink)
		{
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x00051FE1 File Offset: 0x000501E1
		protected override void EndNavigationLinkWithContent(ODataNavigationLink navigationLink)
		{
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x00051FE3 File Offset: 0x000501E3
		protected override void WriteEntityReferenceInNavigationLinkContent(ODataNavigationLink parentNavigationLink, ODataEntityReferenceLink entityReferenceLink)
		{
			this.WriteNavigationLinkStart(parentNavigationLink, entityReferenceLink.Url);
			this.WriteNavigationLinkEnd();
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00051FF8 File Offset: 0x000501F8
		protected override ODataWriterCore.FeedScope CreateFeedScope(ODataFeed feed, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
		{
			return new ODataAtomWriter.AtomFeedScope(feed, entitySet, entityType, skipWriting, selectedProperties);
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x00052006 File Offset: 0x00050206
		protected override ODataWriterCore.EntryScope CreateEntryScope(ODataEntry entry, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties)
		{
			return new ODataAtomWriter.AtomEntryScope(entry, base.GetEntrySerializationInfo(entry), entitySet, entityType, skipWriting, this.atomOutputContext.WritingResponse, this.atomOutputContext.MessageWriterSettings.WriterBehavior, selectedProperties);
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00052040 File Offset: 0x00050240
		private void WriteInstanceAnnotations(IEnumerable<ODataInstanceAnnotation> instanceAnnotations, InstanceAnnotationWriteTracker tracker)
		{
			IEnumerable<AtomInstanceAnnotation> instanceAnnotations2 = from instanceAnnotation in instanceAnnotations
			select AtomInstanceAnnotation.CreateFrom(instanceAnnotation, null);
			this.atomEntryAndFeedSerializer.WriteInstanceAnnotations(instanceAnnotations2, tracker);
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0005207E File Offset: 0x0005027E
		private void WriteFeedInstanceAnnotations(ODataFeed feed, ODataAtomWriter.AtomFeedScope currentFeedScope)
		{
			if (base.IsTopLevel)
			{
				this.WriteInstanceAnnotations(feed.InstanceAnnotations, currentFeedScope.InstanceAnnotationWriteTracker);
				return;
			}
			if (feed.InstanceAnnotations.Count > 0)
			{
				throw new ODataException(Strings.ODataJsonLightWriter_InstanceAnnotationNotSupportedOnExpandedFeed);
			}
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x000520B4 File Offset: 0x000502B4
		private void WriteEntryContent(ODataEntry entry, IEdmEntityType entryType, EntryPropertiesValueCache propertiesValueCache, EpmSourcePathSegment rootSourcePathSegment, ProjectedPropertiesAnnotation projectedProperties)
		{
			ODataStreamReferenceValue mediaResource = entry.MediaResource;
			if (mediaResource == null)
			{
				this.atomOutputContext.XmlWriter.WriteStartElement("", "content", "http://www.w3.org/2005/Atom");
				this.atomOutputContext.XmlWriter.WriteAttributeString("type", "application/xml");
				this.atomEntryAndFeedSerializer.WriteProperties(entryType, propertiesValueCache.EntryProperties, false, new Action(this.atomEntryAndFeedSerializer.WriteEntryPropertiesStart), new Action(this.atomEntryAndFeedSerializer.WriteEntryPropertiesEnd), base.DuplicatePropertyNamesChecker, propertiesValueCache, rootSourcePathSegment, projectedProperties);
				this.atomOutputContext.XmlWriter.WriteEndElement();
				return;
			}
			WriterValidationUtils.ValidateStreamReferenceValue(mediaResource, true);
			this.atomEntryAndFeedSerializer.WriteEntryMediaEditLink(mediaResource);
			if (mediaResource.ReadLink != null)
			{
				this.atomOutputContext.XmlWriter.WriteStartElement("", "content", "http://www.w3.org/2005/Atom");
				this.atomOutputContext.XmlWriter.WriteAttributeString("type", mediaResource.ContentType);
				this.atomOutputContext.XmlWriter.WriteAttributeString("src", this.atomEntryAndFeedSerializer.UriToUrlAttributeValue(mediaResource.ReadLink));
				this.atomOutputContext.XmlWriter.WriteEndElement();
			}
			this.atomEntryAndFeedSerializer.WriteProperties(entryType, propertiesValueCache.EntryProperties, false, new Action(this.atomEntryAndFeedSerializer.WriteEntryPropertiesStart), new Action(this.atomEntryAndFeedSerializer.WriteEntryPropertiesEnd), base.DuplicatePropertyNamesChecker, propertiesValueCache, rootSourcePathSegment, projectedProperties);
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x0005222C File Offset: 0x0005042C
		private void CheckAndWriteParentNavigationLinkStartForInlineElement()
		{
			ODataNavigationLink parentNavigationLink = base.ParentNavigationLink;
			if (parentNavigationLink != null)
			{
				this.WriteNavigationLinkStart(parentNavigationLink, null);
				this.atomOutputContext.XmlWriter.WriteStartElement("m", "inline", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0005226C File Offset: 0x0005046C
		private void CheckAndWriteParentNavigationLinkEndForInlineElement()
		{
			ODataNavigationLink parentNavigationLink = base.ParentNavigationLink;
			if (parentNavigationLink != null)
			{
				this.atomOutputContext.XmlWriter.WriteEndElement();
				this.WriteNavigationLinkEnd();
			}
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00052299 File Offset: 0x00050499
		private void WriteNavigationLinkStart(ODataNavigationLink navigationLink, Uri navigationLinkUrlOverride)
		{
			WriterValidationUtils.ValidateNavigationLinkHasCardinality(navigationLink);
			WriterValidationUtils.ValidateNavigationLinkUrlPresent(navigationLink);
			this.atomEntryAndFeedSerializer.WriteNavigationLinkStart(navigationLink, navigationLinkUrlOverride);
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x000522B4 File Offset: 0x000504B4
		private void WriteNavigationLinkEnd()
		{
			this.atomOutputContext.XmlWriter.WriteEndElement();
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x000522C8 File Offset: 0x000504C8
		private void StartEntryXmlCustomization(ODataEntry entry)
		{
			if (this.atomOutputContext.MessageWriterSettings.AtomStartEntryXmlCustomizationCallback != null)
			{
				XmlWriter xmlWriter = this.atomOutputContext.MessageWriterSettings.AtomStartEntryXmlCustomizationCallback(entry, this.atomOutputContext.XmlWriter);
				if (xmlWriter != null)
				{
					if (object.ReferenceEquals(this.atomOutputContext.XmlWriter, xmlWriter))
					{
						throw new ODataException(Strings.ODataAtomWriter_StartEntryXmlCustomizationCallbackReturnedSameInstance);
					}
				}
				else
				{
					xmlWriter = this.atomOutputContext.XmlWriter;
				}
				this.atomOutputContext.PushCustomWriter(xmlWriter);
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x00052344 File Offset: 0x00050544
		private void EndEntryXmlCustomization(ODataEntry entry)
		{
			if (this.atomOutputContext.MessageWriterSettings.AtomStartEntryXmlCustomizationCallback != null)
			{
				XmlWriter xmlWriter = this.atomOutputContext.PopCustomWriter();
				if (!object.ReferenceEquals(this.atomOutputContext.XmlWriter, xmlWriter))
				{
					this.atomOutputContext.MessageWriterSettings.AtomEndEntryXmlCustomizationCallback(entry, xmlWriter, this.atomOutputContext.XmlWriter);
				}
			}
		}

		// Token: 0x0400091F RID: 2335
		private readonly string updatedTime = ODataAtomConvert.ToAtomString(DateTimeOffset.UtcNow);

		// Token: 0x04000920 RID: 2336
		private readonly ODataAtomOutputContext atomOutputContext;

		// Token: 0x04000921 RID: 2337
		private readonly ODataAtomEntryAndFeedSerializer atomEntryAndFeedSerializer;

		// Token: 0x0200029C RID: 668
		private enum AtomElement
		{
			// Token: 0x04000924 RID: 2340
			Id = 1,
			// Token: 0x04000925 RID: 2341
			ReadLink,
			// Token: 0x04000926 RID: 2342
			EditLink = 4
		}

		// Token: 0x0200029D RID: 669
		private sealed class AtomFeedScope : ODataWriterCore.FeedScope
		{
			// Token: 0x06001696 RID: 5782 RVA: 0x000523A4 File Offset: 0x000505A4
			internal AtomFeedScope(ODataFeed feed, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, SelectedPropertiesNode selectedProperties) : base(feed, entitySet, entityType, skipWriting, selectedProperties)
			{
			}

			// Token: 0x17000489 RID: 1161
			// (get) Token: 0x06001697 RID: 5783 RVA: 0x000523B3 File Offset: 0x000505B3
			// (set) Token: 0x06001698 RID: 5784 RVA: 0x000523BB File Offset: 0x000505BB
			internal bool AuthorWritten
			{
				get
				{
					return this.authorWritten;
				}
				set
				{
					this.authorWritten = value;
				}
			}

			// Token: 0x04000927 RID: 2343
			private bool authorWritten;
		}

		// Token: 0x0200029E RID: 670
		private sealed class AtomEntryScope : ODataWriterCore.EntryScope
		{
			// Token: 0x06001699 RID: 5785 RVA: 0x000523C4 File Offset: 0x000505C4
			internal AtomEntryScope(ODataEntry entry, ODataFeedAndEntrySerializationInfo serializationInfo, IEdmEntitySet entitySet, IEdmEntityType entityType, bool skipWriting, bool writingResponse, ODataWriterBehavior writerBehavior, SelectedPropertiesNode selectedProperties) : base(entry, serializationInfo, entitySet, entityType, skipWriting, writingResponse, writerBehavior, selectedProperties)
			{
			}

			// Token: 0x0600169A RID: 5786 RVA: 0x000523E4 File Offset: 0x000505E4
			internal void SetWrittenElement(ODataAtomWriter.AtomElement atomElement)
			{
				this.alreadyWrittenElements |= (int)atomElement;
			}

			// Token: 0x0600169B RID: 5787 RVA: 0x000523F4 File Offset: 0x000505F4
			internal bool IsElementWritten(ODataAtomWriter.AtomElement atomElement)
			{
				return (this.alreadyWrittenElements & (int)atomElement) == (int)atomElement;
			}

			// Token: 0x04000928 RID: 2344
			private int alreadyWrittenElements;
		}
	}
}
