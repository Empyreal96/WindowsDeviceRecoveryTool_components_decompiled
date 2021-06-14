using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000F6 RID: 246
	internal sealed class ODataAtomEntryMetadataSerializer : ODataAtomMetadataSerializer
	{
		// Token: 0x0600064A RID: 1610 RVA: 0x00016A39 File Offset: 0x00014C39
		internal ODataAtomEntryMetadataSerializer(ODataAtomOutputContext atomOutputContext) : base(atomOutputContext)
		{
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00016A44 File Offset: 0x00014C44
		private ODataAtomFeedMetadataSerializer SourceMetadataSerializer
		{
			get
			{
				ODataAtomFeedMetadataSerializer result;
				if ((result = this.sourceMetadataSerializer) == null)
				{
					result = (this.sourceMetadataSerializer = new ODataAtomFeedMetadataSerializer(base.AtomOutputContext));
				}
				return result;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00016A70 File Offset: 0x00014C70
		internal void WriteEntryMetadata(AtomEntryMetadata entryMetadata, AtomEntryMetadata epmEntryMetadata, string updatedTime)
		{
			AtomEntryMetadata atomEntryMetadata = ODataAtomWriterMetadataEpmMergeUtils.MergeCustomAndEpmEntryMetadata(entryMetadata, epmEntryMetadata, base.MessageWriterSettings.WriterBehavior);
			if (atomEntryMetadata == null)
			{
				base.WriteEmptyElement("", "title", "http://www.w3.org/2005/Atom");
				base.WriteElementWithTextContent("", "updated", "http://www.w3.org/2005/Atom", updatedTime);
				base.WriteEmptyAuthor();
				return;
			}
			base.WriteTextConstruct("", "title", "http://www.w3.org/2005/Atom", atomEntryMetadata.Title);
			AtomTextConstruct summary = atomEntryMetadata.Summary;
			if (summary != null)
			{
				base.WriteTextConstruct("", "summary", "http://www.w3.org/2005/Atom", summary);
			}
			string text = base.UseClientFormatBehavior ? atomEntryMetadata.PublishedString : ((atomEntryMetadata.Published != null) ? ODataAtomConvert.ToAtomString(atomEntryMetadata.Published.Value) : null);
			if (text != null)
			{
				base.WriteElementWithTextContent("", "published", "http://www.w3.org/2005/Atom", text);
			}
			string text2 = base.UseClientFormatBehavior ? atomEntryMetadata.UpdatedString : ((atomEntryMetadata.Updated != null) ? ODataAtomConvert.ToAtomString(atomEntryMetadata.Updated.Value) : null);
			text2 = (text2 ?? updatedTime);
			base.WriteElementWithTextContent("", "updated", "http://www.w3.org/2005/Atom", text2);
			bool flag = false;
			IEnumerable<AtomPersonMetadata> authors = atomEntryMetadata.Authors;
			if (authors != null)
			{
				foreach (AtomPersonMetadata atomPersonMetadata in authors)
				{
					if (atomPersonMetadata == null)
					{
						throw new ODataException(Strings.ODataAtomWriterMetadataUtils_AuthorMetadataMustNotContainNull);
					}
					base.XmlWriter.WriteStartElement("", "author", "http://www.w3.org/2005/Atom");
					base.WritePersonMetadata(atomPersonMetadata);
					base.XmlWriter.WriteEndElement();
					flag = true;
				}
			}
			if (!flag)
			{
				base.WriteEmptyAuthor();
			}
			IEnumerable<AtomPersonMetadata> contributors = atomEntryMetadata.Contributors;
			if (contributors != null)
			{
				foreach (AtomPersonMetadata atomPersonMetadata2 in contributors)
				{
					if (atomPersonMetadata2 == null)
					{
						throw new ODataException(Strings.ODataAtomWriterMetadataUtils_ContributorMetadataMustNotContainNull);
					}
					base.XmlWriter.WriteStartElement("", "contributor", "http://www.w3.org/2005/Atom");
					base.WritePersonMetadata(atomPersonMetadata2);
					base.XmlWriter.WriteEndElement();
				}
			}
			IEnumerable<AtomLinkMetadata> links = atomEntryMetadata.Links;
			if (links != null)
			{
				foreach (AtomLinkMetadata atomLinkMetadata in links)
				{
					if (atomLinkMetadata == null)
					{
						throw new ODataException(Strings.ODataAtomWriterMetadataUtils_LinkMetadataMustNotContainNull);
					}
					base.WriteAtomLink(atomLinkMetadata, null);
				}
			}
			IEnumerable<AtomCategoryMetadata> categories = atomEntryMetadata.Categories;
			if (categories != null)
			{
				foreach (AtomCategoryMetadata atomCategoryMetadata in categories)
				{
					if (atomCategoryMetadata == null)
					{
						throw new ODataException(Strings.ODataAtomWriterMetadataUtils_CategoryMetadataMustNotContainNull);
					}
					base.WriteCategory(atomCategoryMetadata);
				}
			}
			if (atomEntryMetadata.Rights != null)
			{
				base.WriteTextConstruct("", "rights", "http://www.w3.org/2005/Atom", atomEntryMetadata.Rights);
			}
			AtomFeedMetadata source = atomEntryMetadata.Source;
			if (source != null)
			{
				base.XmlWriter.WriteStartElement("", "source", "http://www.w3.org/2005/Atom");
				bool flag2;
				this.SourceMetadataSerializer.WriteFeedMetadata(source, null, updatedTime, out flag2);
				base.XmlWriter.WriteEndElement();
			}
		}

		// Token: 0x04000282 RID: 642
		private ODataAtomFeedMetadataSerializer sourceMetadataSerializer;
	}
}
