using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000F7 RID: 247
	internal sealed class ODataAtomFeedMetadataSerializer : ODataAtomMetadataSerializer
	{
		// Token: 0x0600064D RID: 1613 RVA: 0x00016DD4 File Offset: 0x00014FD4
		internal ODataAtomFeedMetadataSerializer(ODataAtomOutputContext atomOutputContext) : base(atomOutputContext)
		{
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00016DE0 File Offset: 0x00014FE0
		internal void WriteFeedMetadata(AtomFeedMetadata feedMetadata, ODataFeed feed, string updatedTime, out bool authorWritten)
		{
			string textContent = (feed == null) ? feedMetadata.SourceId : feed.Id;
			base.WriteElementWithTextContent("", "id", "http://www.w3.org/2005/Atom", textContent);
			base.WriteTextConstruct("", "title", "http://www.w3.org/2005/Atom", feedMetadata.Title);
			if (feedMetadata.Subtitle != null)
			{
				base.WriteTextConstruct("", "subtitle", "http://www.w3.org/2005/Atom", feedMetadata.Subtitle);
			}
			string textContent2 = (feedMetadata.Updated != null) ? ODataAtomConvert.ToAtomString(feedMetadata.Updated.Value) : updatedTime;
			base.WriteElementWithTextContent("", "updated", "http://www.w3.org/2005/Atom", textContent2);
			AtomLinkMetadata selfLink = feedMetadata.SelfLink;
			if (selfLink != null)
			{
				AtomLinkMetadata linkMetadata = ODataAtomWriterMetadataUtils.MergeLinkMetadata(selfLink, "self", null, null, null);
				base.WriteAtomLink(linkMetadata, null);
			}
			IEnumerable<AtomLinkMetadata> links = feedMetadata.Links;
			if (links != null)
			{
				foreach (AtomLinkMetadata atomLinkMetadata in links)
				{
					if (atomLinkMetadata.Relation != "http://docs.oasis-open.org/odata/ns/delta")
					{
						base.WriteAtomLink(atomLinkMetadata, null);
					}
				}
			}
			IEnumerable<AtomCategoryMetadata> categories = feedMetadata.Categories;
			if (categories != null)
			{
				foreach (AtomCategoryMetadata category in categories)
				{
					base.WriteCategory(category);
				}
			}
			Uri logo = feedMetadata.Logo;
			if (logo != null)
			{
				base.WriteElementWithTextContent("", "logo", "http://www.w3.org/2005/Atom", base.UriToUrlAttributeValue(logo));
			}
			if (feedMetadata.Rights != null)
			{
				base.WriteTextConstruct("", "rights", "http://www.w3.org/2005/Atom", feedMetadata.Rights);
			}
			IEnumerable<AtomPersonMetadata> contributors = feedMetadata.Contributors;
			if (contributors != null)
			{
				foreach (AtomPersonMetadata personMetadata in contributors)
				{
					base.XmlWriter.WriteStartElement("", "contributor", "http://www.w3.org/2005/Atom");
					base.WritePersonMetadata(personMetadata);
					base.XmlWriter.WriteEndElement();
				}
			}
			AtomGeneratorMetadata generator = feedMetadata.Generator;
			if (generator != null)
			{
				base.XmlWriter.WriteStartElement("", "generator", "http://www.w3.org/2005/Atom");
				if (generator.Uri != null)
				{
					base.XmlWriter.WriteAttributeString("uri", base.UriToUrlAttributeValue(generator.Uri));
				}
				if (!string.IsNullOrEmpty(generator.Version))
				{
					base.XmlWriter.WriteAttributeString("version", generator.Version);
				}
				ODataAtomWriterUtils.WriteString(base.XmlWriter, generator.Name);
				base.XmlWriter.WriteEndElement();
			}
			Uri icon = feedMetadata.Icon;
			if (icon != null)
			{
				base.WriteElementWithTextContent("", "icon", "http://www.w3.org/2005/Atom", base.UriToUrlAttributeValue(icon));
			}
			IEnumerable<AtomPersonMetadata> authors = feedMetadata.Authors;
			authorWritten = false;
			if (authors != null)
			{
				foreach (AtomPersonMetadata personMetadata2 in authors)
				{
					authorWritten = true;
					base.XmlWriter.WriteStartElement("", "author", "http://www.w3.org/2005/Atom");
					base.WritePersonMetadata(personMetadata2);
					base.XmlWriter.WriteEndElement();
				}
			}
		}
	}
}
