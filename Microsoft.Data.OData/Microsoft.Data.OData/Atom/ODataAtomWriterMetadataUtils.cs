using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000281 RID: 641
	internal static class ODataAtomWriterMetadataUtils
	{
		// Token: 0x0600154D RID: 5453 RVA: 0x0004DCC0 File Offset: 0x0004BEC0
		internal static AtomLinkMetadata MergeLinkMetadata(AtomLinkMetadata metadata, string relation, Uri href, string title, string mediaType)
		{
			AtomLinkMetadata atomLinkMetadata = new AtomLinkMetadata(metadata);
			string relation2 = atomLinkMetadata.Relation;
			if (relation2 != null)
			{
				if (string.CompareOrdinal(relation, relation2) != 0)
				{
					throw new ODataException(Strings.ODataAtomWriterMetadataUtils_LinkRelationsMustMatch(relation, relation2));
				}
			}
			else
			{
				atomLinkMetadata.Relation = relation;
			}
			if (href != null)
			{
				Uri href2 = atomLinkMetadata.Href;
				if (href2 != null)
				{
					if (!href.Equals(href2))
					{
						throw new ODataException(Strings.ODataAtomWriterMetadataUtils_LinkHrefsMustMatch(href, href2));
					}
				}
				else
				{
					atomLinkMetadata.Href = href;
				}
			}
			if (title != null)
			{
				string title2 = atomLinkMetadata.Title;
				if (title2 != null)
				{
					if (string.CompareOrdinal(title, title2) != 0)
					{
						throw new ODataException(Strings.ODataAtomWriterMetadataUtils_LinkTitlesMustMatch(title, title2));
					}
				}
				else
				{
					atomLinkMetadata.Title = title;
				}
			}
			if (mediaType != null)
			{
				string mediaType2 = atomLinkMetadata.MediaType;
				if (mediaType2 != null)
				{
					if (!HttpUtils.CompareMediaTypeNames(mediaType, mediaType2))
					{
						throw new ODataException(Strings.ODataAtomWriterMetadataUtils_LinkMediaTypesMustMatch(mediaType, mediaType2));
					}
				}
				else
				{
					atomLinkMetadata.MediaType = mediaType;
				}
			}
			return atomLinkMetadata;
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0004DD90 File Offset: 0x0004BF90
		internal static AtomCategoryMetadata MergeCategoryMetadata(AtomCategoryMetadata categoryMetadata, string term, string scheme)
		{
			AtomCategoryMetadata atomCategoryMetadata = new AtomCategoryMetadata(categoryMetadata);
			string term2 = atomCategoryMetadata.Term;
			if (term2 != null)
			{
				if (string.CompareOrdinal(term, term2) != 0)
				{
					throw new ODataException(Strings.ODataAtomWriterMetadataUtils_CategoryTermsMustMatch(term, term2));
				}
			}
			else
			{
				atomCategoryMetadata.Term = term;
			}
			string scheme2 = atomCategoryMetadata.Scheme;
			if (scheme2 != null)
			{
				if (string.CompareOrdinal(scheme, scheme2) != 0)
				{
					throw new ODataException(Strings.ODataAtomWriterMetadataUtils_CategorySchemesMustMatch(scheme, scheme2));
				}
			}
			else
			{
				atomCategoryMetadata.Scheme = scheme;
			}
			return atomCategoryMetadata;
		}
	}
}
