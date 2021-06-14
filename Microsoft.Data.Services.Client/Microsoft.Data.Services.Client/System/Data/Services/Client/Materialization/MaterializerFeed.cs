using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000116 RID: 278
	internal struct MaterializerFeed
	{
		// Token: 0x0600091A RID: 2330 RVA: 0x0002539F File Offset: 0x0002359F
		private MaterializerFeed(ODataFeed feed, IEnumerable<ODataEntry> entries)
		{
			this.feed = feed;
			this.entries = entries;
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x000253AF File Offset: 0x000235AF
		public ODataFeed Feed
		{
			get
			{
				return this.feed;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x000253B7 File Offset: 0x000235B7
		public IEnumerable<ODataEntry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x000253BF File Offset: 0x000235BF
		public Uri NextPageLink
		{
			get
			{
				return this.feed.NextPageLink;
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x000253CC File Offset: 0x000235CC
		public static MaterializerFeed CreateFeed(ODataFeed feed, IEnumerable<ODataEntry> entries)
		{
			if (entries == null)
			{
				entries = Enumerable.Empty<ODataEntry>();
			}
			else
			{
				feed.SetAnnotation<IEnumerable<ODataEntry>>(entries);
			}
			return new MaterializerFeed(feed, entries);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x000253E8 File Offset: 0x000235E8
		public static MaterializerFeed GetFeed(ODataFeed feed)
		{
			IEnumerable<ODataEntry> annotation = feed.GetAnnotation<IEnumerable<ODataEntry>>();
			return new MaterializerFeed(feed, annotation);
		}

		// Token: 0x0400055A RID: 1370
		private readonly ODataFeed feed;

		// Token: 0x0400055B RID: 1371
		private readonly IEnumerable<ODataEntry> entries;
	}
}
