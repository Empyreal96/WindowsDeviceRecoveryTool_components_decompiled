using System;
using Microsoft.Data.OData.Atom;

namespace Microsoft.Data.OData
{
	// Token: 0x02000213 RID: 531
	internal static class AtomMetadataReaderUtils
	{
		// Token: 0x0600105F RID: 4191 RVA: 0x0003BDA0 File Offset: 0x00039FA0
		internal static AtomEntryMetadata CreateNewAtomEntryMetadata()
		{
			return new AtomEntryMetadata
			{
				Authors = ReadOnlyEnumerable<AtomPersonMetadata>.Empty(),
				Categories = ReadOnlyEnumerable<AtomCategoryMetadata>.Empty(),
				Contributors = ReadOnlyEnumerable<AtomPersonMetadata>.Empty(),
				Links = ReadOnlyEnumerable<AtomLinkMetadata>.Empty()
			};
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0003BDE0 File Offset: 0x00039FE0
		internal static AtomFeedMetadata CreateNewAtomFeedMetadata()
		{
			return new AtomFeedMetadata
			{
				Authors = ReadOnlyEnumerable<AtomPersonMetadata>.Empty(),
				Categories = ReadOnlyEnumerable<AtomCategoryMetadata>.Empty(),
				Contributors = ReadOnlyEnumerable<AtomPersonMetadata>.Empty(),
				Links = ReadOnlyEnumerable<AtomLinkMetadata>.Empty()
			};
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0003BE20 File Offset: 0x0003A020
		internal static void AddAuthor(this AtomEntryMetadata entryMetadata, AtomPersonMetadata authorMetadata)
		{
			entryMetadata.Authors = entryMetadata.Authors.ConcatToReadOnlyEnumerable("Authors", authorMetadata);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0003BE39 File Offset: 0x0003A039
		internal static void AddContributor(this AtomEntryMetadata entryMetadata, AtomPersonMetadata contributorMetadata)
		{
			entryMetadata.Contributors = entryMetadata.Contributors.ConcatToReadOnlyEnumerable("Contributors", contributorMetadata);
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0003BE52 File Offset: 0x0003A052
		internal static void AddLink(this AtomEntryMetadata entryMetadata, AtomLinkMetadata linkMetadata)
		{
			entryMetadata.Links = entryMetadata.Links.ConcatToReadOnlyEnumerable("Links", linkMetadata);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0003BE6B File Offset: 0x0003A06B
		internal static void AddLink(this AtomFeedMetadata feedMetadata, AtomLinkMetadata linkMetadata)
		{
			feedMetadata.Links = feedMetadata.Links.ConcatToReadOnlyEnumerable("Links", linkMetadata);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0003BE84 File Offset: 0x0003A084
		internal static void AddCategory(this AtomEntryMetadata entryMetadata, AtomCategoryMetadata categoryMetadata)
		{
			entryMetadata.Categories = entryMetadata.Categories.ConcatToReadOnlyEnumerable("Categories", categoryMetadata);
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0003BE9D File Offset: 0x0003A09D
		internal static void AddCategory(this AtomFeedMetadata feedMetadata, AtomCategoryMetadata categoryMetadata)
		{
			feedMetadata.Categories = feedMetadata.Categories.ConcatToReadOnlyEnumerable("Categories", categoryMetadata);
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0003BEB6 File Offset: 0x0003A0B6
		internal static void AddAuthor(this AtomFeedMetadata feedMetadata, AtomPersonMetadata authorMetadata)
		{
			feedMetadata.Authors = feedMetadata.Authors.ConcatToReadOnlyEnumerable("Authors", authorMetadata);
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0003BECF File Offset: 0x0003A0CF
		internal static void AddContributor(this AtomFeedMetadata feedMetadata, AtomPersonMetadata contributorMetadata)
		{
			feedMetadata.Contributors = feedMetadata.Contributors.ConcatToReadOnlyEnumerable("Contributors", contributorMetadata);
		}
	}
}
