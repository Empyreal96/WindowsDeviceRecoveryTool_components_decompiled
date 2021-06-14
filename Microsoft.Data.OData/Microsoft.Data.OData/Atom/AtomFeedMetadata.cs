using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200027A RID: 634
	public sealed class AtomFeedMetadata : ODataAnnotatable
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0004D702 File Offset: 0x0004B902
		// (set) Token: 0x060014E3 RID: 5347 RVA: 0x0004D70A File Offset: 0x0004B90A
		public IEnumerable<AtomPersonMetadata> Authors { get; set; }

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0004D713 File Offset: 0x0004B913
		// (set) Token: 0x060014E5 RID: 5349 RVA: 0x0004D71B File Offset: 0x0004B91B
		public IEnumerable<AtomCategoryMetadata> Categories { get; set; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x0004D724 File Offset: 0x0004B924
		// (set) Token: 0x060014E7 RID: 5351 RVA: 0x0004D72C File Offset: 0x0004B92C
		public IEnumerable<AtomPersonMetadata> Contributors { get; set; }

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x0004D735 File Offset: 0x0004B935
		// (set) Token: 0x060014E9 RID: 5353 RVA: 0x0004D73D File Offset: 0x0004B93D
		public AtomGeneratorMetadata Generator { get; set; }

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x0004D746 File Offset: 0x0004B946
		// (set) Token: 0x060014EB RID: 5355 RVA: 0x0004D74E File Offset: 0x0004B94E
		public Uri Icon { get; set; }

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0004D757 File Offset: 0x0004B957
		// (set) Token: 0x060014ED RID: 5357 RVA: 0x0004D75F File Offset: 0x0004B95F
		public IEnumerable<AtomLinkMetadata> Links { get; set; }

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x0004D768 File Offset: 0x0004B968
		// (set) Token: 0x060014EF RID: 5359 RVA: 0x0004D770 File Offset: 0x0004B970
		public Uri Logo { get; set; }

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0004D779 File Offset: 0x0004B979
		// (set) Token: 0x060014F1 RID: 5361 RVA: 0x0004D781 File Offset: 0x0004B981
		public AtomTextConstruct Rights { get; set; }

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x0004D78A File Offset: 0x0004B98A
		// (set) Token: 0x060014F3 RID: 5363 RVA: 0x0004D792 File Offset: 0x0004B992
		public AtomLinkMetadata SelfLink { get; set; }

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x0004D79B File Offset: 0x0004B99B
		// (set) Token: 0x060014F5 RID: 5365 RVA: 0x0004D7A3 File Offset: 0x0004B9A3
		public AtomLinkMetadata NextPageLink { get; set; }

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x0004D7AC File Offset: 0x0004B9AC
		// (set) Token: 0x060014F7 RID: 5367 RVA: 0x0004D7B4 File Offset: 0x0004B9B4
		public string SourceId { get; set; }

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x0004D7BD File Offset: 0x0004B9BD
		// (set) Token: 0x060014F9 RID: 5369 RVA: 0x0004D7C5 File Offset: 0x0004B9C5
		public AtomTextConstruct Subtitle { get; set; }

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060014FA RID: 5370 RVA: 0x0004D7CE File Offset: 0x0004B9CE
		// (set) Token: 0x060014FB RID: 5371 RVA: 0x0004D7D6 File Offset: 0x0004B9D6
		public AtomTextConstruct Title { get; set; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060014FC RID: 5372 RVA: 0x0004D7DF File Offset: 0x0004B9DF
		// (set) Token: 0x060014FD RID: 5373 RVA: 0x0004D7E7 File Offset: 0x0004B9E7
		public DateTimeOffset? Updated { get; set; }
	}
}
