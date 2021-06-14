using System;
using System.Linq;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000F4 RID: 244
	internal sealed class ODataAtomEntryAndFeedSerializer : ODataAtomPropertyAndValueSerializer
	{
		// Token: 0x0600062B RID: 1579 RVA: 0x0001615E File Offset: 0x0001435E
		internal ODataAtomEntryAndFeedSerializer(ODataAtomOutputContext atomOutputContext) : base(atomOutputContext)
		{
			this.atomEntryMetadataSerializer = new ODataAtomEntryMetadataSerializer(atomOutputContext);
			this.atomFeedMetadataSerializer = new ODataAtomFeedMetadataSerializer(atomOutputContext);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001617F File Offset: 0x0001437F
		internal void WriteEntryPropertiesStart()
		{
			base.XmlWriter.WriteStartElement("m", "properties", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001619B File Offset: 0x0001439B
		internal void WriteEntryPropertiesEnd()
		{
			base.XmlWriter.WriteEndElement();
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000161A8 File Offset: 0x000143A8
		internal void WriteEntryTypeName(string typeName, AtomEntryMetadata entryMetadata)
		{
			if (typeName != null)
			{
				AtomCategoryMetadata category = ODataAtomWriterMetadataUtils.MergeCategoryMetadata((entryMetadata == null) ? null : entryMetadata.CategoryWithTypeName, typeName, base.MessageWriterSettings.WriterBehavior.ODataTypeScheme);
				this.atomEntryMetadataSerializer.WriteCategory(category);
			}
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000161E7 File Offset: 0x000143E7
		internal void WriteEntryMetadata(AtomEntryMetadata entryMetadata, AtomEntryMetadata epmEntryMetadata, string updatedTime)
		{
			this.atomEntryMetadataSerializer.WriteEntryMetadata(entryMetadata, epmEntryMetadata, updatedTime);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000161F7 File Offset: 0x000143F7
		internal void WriteEntryId(string entryId)
		{
			base.WriteElementWithTextContent("", "id", "http://www.w3.org/2005/Atom", entryId);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00016210 File Offset: 0x00014410
		internal void WriteEntryReadLink(Uri readLink, AtomEntryMetadata entryMetadata)
		{
			AtomLinkMetadata linkMetadata = (entryMetadata == null) ? null : entryMetadata.SelfLink;
			this.WriteReadOrEditLink(readLink, linkMetadata, "self");
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00016238 File Offset: 0x00014438
		internal void WriteEntryEditLink(Uri editLink, AtomEntryMetadata entryMetadata)
		{
			AtomLinkMetadata linkMetadata = (entryMetadata == null) ? null : entryMetadata.EditLink;
			this.WriteReadOrEditLink(editLink, linkMetadata, "edit");
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00016260 File Offset: 0x00014460
		internal void WriteEntryMediaEditLink(ODataStreamReferenceValue mediaResource)
		{
			Uri editLink = mediaResource.EditLink;
			if (editLink != null)
			{
				AtomStreamReferenceMetadata annotation = mediaResource.GetAnnotation<AtomStreamReferenceMetadata>();
				AtomLinkMetadata metadata = (annotation == null) ? null : annotation.EditLink;
				AtomLinkMetadata linkMetadata = ODataAtomWriterMetadataUtils.MergeLinkMetadata(metadata, "edit-media", editLink, null, null);
				this.atomEntryMetadataSerializer.WriteAtomLink(linkMetadata, mediaResource.ETag);
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000162B4 File Offset: 0x000144B4
		internal void WriteAssociationLink(ODataAssociationLink associationLink, IEdmEntityType owningType, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, ProjectedPropertiesAnnotation projectedProperties)
		{
			ValidationUtils.ValidateAssociationLinkNotNull(associationLink);
			string name = associationLink.Name;
			if (projectedProperties.ShouldSkipProperty(name))
			{
				return;
			}
			base.ValidateAssociationLink(associationLink, owningType);
			duplicatePropertyNamesChecker.CheckForDuplicateAssociationLinkNames(associationLink);
			AtomLinkMetadata annotation = associationLink.GetAnnotation<AtomLinkMetadata>();
			string relation = AtomUtils.ComputeODataAssociationLinkRelation(associationLink);
			AtomLinkMetadata linkMetadata = ODataAtomWriterMetadataUtils.MergeLinkMetadata(annotation, relation, associationLink.Url, name, "application/xml");
			this.atomEntryMetadataSerializer.WriteAtomLink(linkMetadata, null);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00016318 File Offset: 0x00014518
		internal void WriteNavigationLinkStart(ODataNavigationLink navigationLink, Uri navigationLinkUrlOverride)
		{
			base.XmlWriter.WriteStartElement("", "link", "http://www.w3.org/2005/Atom");
			string relation = AtomUtils.ComputeODataNavigationLinkRelation(navigationLink);
			string mediaType = AtomUtils.ComputeODataNavigationLinkType(navigationLink);
			string name = navigationLink.Name;
			Uri href = navigationLinkUrlOverride ?? navigationLink.Url;
			AtomLinkMetadata annotation = navigationLink.GetAnnotation<AtomLinkMetadata>();
			AtomLinkMetadata linkMetadata = ODataAtomWriterMetadataUtils.MergeLinkMetadata(annotation, relation, href, name, mediaType);
			this.atomEntryMetadataSerializer.WriteAtomLinkAttributes(linkMetadata, null);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00016384 File Offset: 0x00014584
		internal void WriteFeedMetadata(ODataFeed feed, string updatedTime, out bool authorWritten)
		{
			AtomFeedMetadata annotation = feed.GetAnnotation<AtomFeedMetadata>();
			if (annotation == null)
			{
				base.WriteElementWithTextContent("", "id", "http://www.w3.org/2005/Atom", feed.Id);
				base.WriteEmptyElement("", "title", "http://www.w3.org/2005/Atom");
				base.WriteElementWithTextContent("", "updated", "http://www.w3.org/2005/Atom", updatedTime);
				authorWritten = false;
				return;
			}
			this.atomFeedMetadataSerializer.WriteFeedMetadata(annotation, feed, updatedTime, out authorWritten);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000163F4 File Offset: 0x000145F4
		internal void WriteFeedDefaultAuthor()
		{
			this.atomFeedMetadataSerializer.WriteEmptyAuthor();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00016410 File Offset: 0x00014610
		internal void WriteFeedNextPageLink(ODataFeed feed)
		{
			Uri nextPageLink = feed.NextPageLink;
			if (nextPageLink != null)
			{
				this.WriteFeedLink(feed, "next", nextPageLink, delegate(AtomFeedMetadata feedMetadata)
				{
					if (feedMetadata != null)
					{
						return feedMetadata.NextPageLink;
					}
					return null;
				});
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00016498 File Offset: 0x00014698
		internal void WriteFeedDeltaLink(ODataFeed feed)
		{
			Uri deltaLink = feed.DeltaLink;
			if (deltaLink != null)
			{
				this.WriteFeedLink(feed, "http://docs.oasis-open.org/odata/ns/delta", deltaLink, delegate(AtomFeedMetadata feedMetadata)
				{
					if (feedMetadata != null)
					{
						return feedMetadata.Links.FirstOrDefault((AtomLinkMetadata link) => link.Relation == "http://docs.oasis-open.org/odata/ns/delta");
					}
					return null;
				});
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x000164E0 File Offset: 0x000146E0
		internal void WriteFeedLink(ODataFeed feed, string relation, Uri href, Func<AtomFeedMetadata, AtomLinkMetadata> getLinkMetadata)
		{
			AtomFeedMetadata annotation = feed.GetAnnotation<AtomFeedMetadata>();
			AtomLinkMetadata linkMetadata = ODataAtomWriterMetadataUtils.MergeLinkMetadata(getLinkMetadata(annotation), relation, href, null, null);
			this.atomFeedMetadataSerializer.WriteAtomLink(linkMetadata, null);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00016514 File Offset: 0x00014714
		internal void WriteStreamProperty(ODataProperty streamProperty, IEdmEntityType owningType, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, ProjectedPropertiesAnnotation projectedProperties)
		{
			WriterValidationUtils.ValidatePropertyNotNull(streamProperty);
			string name = streamProperty.Name;
			if (projectedProperties.ShouldSkipProperty(name))
			{
				return;
			}
			WriterValidationUtils.ValidatePropertyName(name);
			duplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(streamProperty);
			IEdmProperty edmProperty = WriterValidationUtils.ValidatePropertyDefined(streamProperty.Name, owningType);
			WriterValidationUtils.ValidateStreamReferenceProperty(streamProperty, edmProperty, base.Version, base.WritingResponse);
			ODataStreamReferenceValue odataStreamReferenceValue = (ODataStreamReferenceValue)streamProperty.Value;
			WriterValidationUtils.ValidateStreamReferenceValue(odataStreamReferenceValue, false);
			if (owningType != null && owningType.IsOpen && edmProperty == null)
			{
				ValidationUtils.ValidateOpenPropertyValue(streamProperty.Name, odataStreamReferenceValue);
			}
			AtomStreamReferenceMetadata annotation = odataStreamReferenceValue.GetAnnotation<AtomStreamReferenceMetadata>();
			string contentType = odataStreamReferenceValue.ContentType;
			string name2 = streamProperty.Name;
			Uri readLink = odataStreamReferenceValue.ReadLink;
			if (readLink != null)
			{
				string relation = AtomUtils.ComputeStreamPropertyRelation(streamProperty, false);
				AtomLinkMetadata metadata = (annotation == null) ? null : annotation.SelfLink;
				AtomLinkMetadata linkMetadata = ODataAtomWriterMetadataUtils.MergeLinkMetadata(metadata, relation, readLink, name2, contentType);
				this.atomEntryMetadataSerializer.WriteAtomLink(linkMetadata, null);
			}
			Uri editLink = odataStreamReferenceValue.EditLink;
			if (editLink != null)
			{
				string relation2 = AtomUtils.ComputeStreamPropertyRelation(streamProperty, true);
				AtomLinkMetadata metadata2 = (annotation == null) ? null : annotation.EditLink;
				AtomLinkMetadata linkMetadata2 = ODataAtomWriterMetadataUtils.MergeLinkMetadata(metadata2, relation2, editLink, name2, contentType);
				this.atomEntryMetadataSerializer.WriteAtomLink(linkMetadata2, odataStreamReferenceValue.ETag);
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00016640 File Offset: 0x00014840
		internal void WriteOperation(ODataOperation operation)
		{
			WriterValidationUtils.ValidateCanWriteOperation(operation, base.WritingResponse);
			ValidationUtils.ValidateOperationMetadataNotNull(operation);
			ValidationUtils.ValidateOperationTargetNotNull(operation);
			string localName;
			if (operation is ODataAction)
			{
				localName = "action";
			}
			else
			{
				localName = "function";
			}
			base.XmlWriter.WriteStartElement("m", localName, "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			string value = base.UriToUrlAttributeValue(operation.Metadata, false);
			base.XmlWriter.WriteAttributeString("metadata", value);
			if (operation.Title != null)
			{
				base.XmlWriter.WriteAttributeString("title", operation.Title);
			}
			string value2 = base.UriToUrlAttributeValue(operation.Target);
			base.XmlWriter.WriteAttributeString("target", value2);
			base.XmlWriter.WriteEndElement();
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000166F8 File Offset: 0x000148F8
		private void WriteReadOrEditLink(Uri link, AtomLinkMetadata linkMetadata, string linkRelation)
		{
			if (link != null)
			{
				AtomLinkMetadata linkMetadata2 = ODataAtomWriterMetadataUtils.MergeLinkMetadata(linkMetadata, linkRelation, link, null, null);
				this.atomEntryMetadataSerializer.WriteAtomLink(linkMetadata2, null);
			}
		}

		// Token: 0x0400027D RID: 637
		private readonly ODataAtomEntryMetadataSerializer atomEntryMetadataSerializer;

		// Token: 0x0400027E RID: 638
		private readonly ODataAtomFeedMetadataSerializer atomFeedMetadataSerializer;
	}
}
