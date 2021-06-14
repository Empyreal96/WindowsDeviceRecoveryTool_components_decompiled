using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000F5 RID: 245
	internal abstract class ODataAtomMetadataSerializer : ODataAtomSerializer
	{
		// Token: 0x06000641 RID: 1601 RVA: 0x00016726 File Offset: 0x00014926
		internal ODataAtomMetadataSerializer(ODataAtomOutputContext atomOutputContext) : base(atomOutputContext)
		{
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00016730 File Offset: 0x00014930
		internal void WriteTextConstruct(string prefix, string localName, string ns, AtomTextConstruct textConstruct)
		{
			base.XmlWriter.WriteStartElement(prefix, localName, ns);
			if (textConstruct != null)
			{
				AtomTextConstructKind kind = textConstruct.Kind;
				base.XmlWriter.WriteAttributeString("type", AtomValueUtils.ToString(textConstruct.Kind));
				string text = textConstruct.Text;
				if (text == null)
				{
					text = string.Empty;
				}
				if (kind == AtomTextConstructKind.Xhtml)
				{
					ODataAtomWriterUtils.WriteRaw(base.XmlWriter, text);
				}
				else
				{
					ODataAtomWriterUtils.WriteString(base.XmlWriter, text);
				}
			}
			base.XmlWriter.WriteEndElement();
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000167AD File Offset: 0x000149AD
		internal void WriteCategory(AtomCategoryMetadata category)
		{
			this.WriteCategory("", category.Term, category.Scheme, category.Label);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000167CC File Offset: 0x000149CC
		internal void WriteCategory(string atomPrefix, string term, string scheme, string label)
		{
			base.XmlWriter.WriteStartElement(atomPrefix, "category", "http://www.w3.org/2005/Atom");
			if (term == null)
			{
				throw new ODataException(Strings.ODataAtomWriterMetadataUtils_CategoryMustSpecifyTerm);
			}
			base.XmlWriter.WriteAttributeString("term", term);
			if (scheme != null)
			{
				base.XmlWriter.WriteAttributeString("scheme", scheme);
			}
			if (label != null)
			{
				base.XmlWriter.WriteAttributeString("label", label);
			}
			base.XmlWriter.WriteEndElement();
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00016843 File Offset: 0x00014A43
		internal void WriteEmptyAuthor()
		{
			base.XmlWriter.WriteStartElement("", "author", "http://www.w3.org/2005/Atom");
			base.WriteEmptyElement("", "name", "http://www.w3.org/2005/Atom");
			base.XmlWriter.WriteEndElement();
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00016880 File Offset: 0x00014A80
		internal void WritePersonMetadata(AtomPersonMetadata personMetadata)
		{
			base.WriteElementWithTextContent("", "name", "http://www.w3.org/2005/Atom", personMetadata.Name);
			string text = personMetadata.UriFromEpm;
			if (text == null)
			{
				Uri uri = personMetadata.Uri;
				if (uri != null)
				{
					text = base.UriToUrlAttributeValue(uri);
				}
			}
			if (text != null)
			{
				base.WriteElementWithTextContent("", "uri", "http://www.w3.org/2005/Atom", text);
			}
			string email = personMetadata.Email;
			if (email != null)
			{
				base.WriteElementWithTextContent("", "email", "http://www.w3.org/2005/Atom", email);
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00016903 File Offset: 0x00014B03
		internal void WriteAtomLink(AtomLinkMetadata linkMetadata, string etag)
		{
			base.XmlWriter.WriteStartElement("", "link", "http://www.w3.org/2005/Atom");
			this.WriteAtomLinkAttributes(linkMetadata, etag);
			base.XmlWriter.WriteEndElement();
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00016934 File Offset: 0x00014B34
		internal void WriteAtomLinkAttributes(AtomLinkMetadata linkMetadata, string etag)
		{
			string href = (linkMetadata.Href == null) ? null : base.UriToUrlAttributeValue(linkMetadata.Href);
			this.WriteAtomLinkMetadataAttributes(linkMetadata.Relation, href, linkMetadata.HrefLang, linkMetadata.Title, linkMetadata.MediaType, linkMetadata.Length);
			if (etag != null)
			{
				ODataAtomWriterUtils.WriteETag(base.XmlWriter, etag);
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00016994 File Offset: 0x00014B94
		private void WriteAtomLinkMetadataAttributes(string relation, string href, string hrefLang, string title, string mediaType, int? length)
		{
			if (relation != null)
			{
				base.XmlWriter.WriteAttributeString("rel", relation);
			}
			if (mediaType != null)
			{
				base.XmlWriter.WriteAttributeString("type", mediaType);
			}
			if (title != null)
			{
				base.XmlWriter.WriteAttributeString("title", title);
			}
			if (href == null)
			{
				throw new ODataException(Strings.ODataAtomWriterMetadataUtils_LinkMustSpecifyHref);
			}
			base.XmlWriter.WriteAttributeString("href", href);
			if (hrefLang != null)
			{
				base.XmlWriter.WriteAttributeString("hreflang", hrefLang);
			}
			if (length != null)
			{
				base.XmlWriter.WriteAttributeString("length", length.Value.ToString());
			}
		}
	}
}
