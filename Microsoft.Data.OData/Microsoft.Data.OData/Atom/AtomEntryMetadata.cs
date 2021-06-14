using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200027B RID: 635
	public sealed class AtomEntryMetadata : ODataAnnotatable
	{
		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060014FF RID: 5375 RVA: 0x0004D7F8 File Offset: 0x0004B9F8
		// (set) Token: 0x06001500 RID: 5376 RVA: 0x0004D800 File Offset: 0x0004BA00
		public IEnumerable<AtomPersonMetadata> Authors { get; set; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001501 RID: 5377 RVA: 0x0004D809 File Offset: 0x0004BA09
		// (set) Token: 0x06001502 RID: 5378 RVA: 0x0004D811 File Offset: 0x0004BA11
		public AtomCategoryMetadata CategoryWithTypeName { get; set; }

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x0004D81A File Offset: 0x0004BA1A
		// (set) Token: 0x06001504 RID: 5380 RVA: 0x0004D822 File Offset: 0x0004BA22
		public IEnumerable<AtomCategoryMetadata> Categories { get; set; }

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x0004D82B File Offset: 0x0004BA2B
		// (set) Token: 0x06001506 RID: 5382 RVA: 0x0004D833 File Offset: 0x0004BA33
		public IEnumerable<AtomPersonMetadata> Contributors { get; set; }

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x0004D83C File Offset: 0x0004BA3C
		// (set) Token: 0x06001508 RID: 5384 RVA: 0x0004D844 File Offset: 0x0004BA44
		public AtomLinkMetadata SelfLink { get; set; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x0004D84D File Offset: 0x0004BA4D
		// (set) Token: 0x0600150A RID: 5386 RVA: 0x0004D855 File Offset: 0x0004BA55
		public AtomLinkMetadata EditLink { get; set; }

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x0004D85E File Offset: 0x0004BA5E
		// (set) Token: 0x0600150C RID: 5388 RVA: 0x0004D866 File Offset: 0x0004BA66
		public IEnumerable<AtomLinkMetadata> Links { get; set; }

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x0600150D RID: 5389 RVA: 0x0004D86F File Offset: 0x0004BA6F
		// (set) Token: 0x0600150E RID: 5390 RVA: 0x0004D877 File Offset: 0x0004BA77
		public DateTimeOffset? Published { get; set; }

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x0600150F RID: 5391 RVA: 0x0004D880 File Offset: 0x0004BA80
		// (set) Token: 0x06001510 RID: 5392 RVA: 0x0004D888 File Offset: 0x0004BA88
		public AtomTextConstruct Rights { get; set; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x0004D891 File Offset: 0x0004BA91
		// (set) Token: 0x06001512 RID: 5394 RVA: 0x0004D899 File Offset: 0x0004BA99
		public AtomFeedMetadata Source { get; set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001513 RID: 5395 RVA: 0x0004D8A2 File Offset: 0x0004BAA2
		// (set) Token: 0x06001514 RID: 5396 RVA: 0x0004D8AA File Offset: 0x0004BAAA
		public AtomTextConstruct Summary { get; set; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x0004D8B3 File Offset: 0x0004BAB3
		// (set) Token: 0x06001516 RID: 5398 RVA: 0x0004D8BB File Offset: 0x0004BABB
		public AtomTextConstruct Title { get; set; }

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001517 RID: 5399 RVA: 0x0004D8C4 File Offset: 0x0004BAC4
		// (set) Token: 0x06001518 RID: 5400 RVA: 0x0004D8CC File Offset: 0x0004BACC
		public DateTimeOffset? Updated { get; set; }

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x0004D8D5 File Offset: 0x0004BAD5
		// (set) Token: 0x0600151A RID: 5402 RVA: 0x0004D8DD File Offset: 0x0004BADD
		internal string PublishedString
		{
			get
			{
				return this.publishedString;
			}
			set
			{
				this.publishedString = value;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x0004D8E6 File Offset: 0x0004BAE6
		// (set) Token: 0x0600151C RID: 5404 RVA: 0x0004D8EE File Offset: 0x0004BAEE
		internal string UpdatedString
		{
			get
			{
				return this.updatedString;
			}
			set
			{
				this.updatedString = value;
			}
		}

		// Token: 0x040007A4 RID: 1956
		private string publishedString;

		// Token: 0x040007A5 RID: 1957
		private string updatedString;
	}
}
