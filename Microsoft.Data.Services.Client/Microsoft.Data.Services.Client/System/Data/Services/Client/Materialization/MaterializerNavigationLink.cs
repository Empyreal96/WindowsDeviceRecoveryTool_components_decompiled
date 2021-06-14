using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000117 RID: 279
	internal class MaterializerNavigationLink
	{
		// Token: 0x06000920 RID: 2336 RVA: 0x00025403 File Offset: 0x00023603
		private MaterializerNavigationLink(ODataNavigationLink link, object materializedFeedOrEntry)
		{
			this.link = link;
			this.feedOrEntry = materializedFeedOrEntry;
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x00025419 File Offset: 0x00023619
		public ODataNavigationLink Link
		{
			get
			{
				return this.link;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x00025421 File Offset: 0x00023621
		public MaterializerEntry Entry
		{
			get
			{
				return this.feedOrEntry as MaterializerEntry;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0002542E File Offset: 0x0002362E
		public ODataFeed Feed
		{
			get
			{
				return this.feedOrEntry as ODataFeed;
			}
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0002543C File Offset: 0x0002363C
		public static MaterializerNavigationLink CreateLink(ODataNavigationLink link, MaterializerEntry entry)
		{
			MaterializerNavigationLink materializerNavigationLink = new MaterializerNavigationLink(link, entry);
			link.SetAnnotation<MaterializerNavigationLink>(materializerNavigationLink);
			return materializerNavigationLink;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0002545C File Offset: 0x0002365C
		public static MaterializerNavigationLink CreateLink(ODataNavigationLink link, ODataFeed feed)
		{
			MaterializerNavigationLink materializerNavigationLink = new MaterializerNavigationLink(link, feed);
			link.SetAnnotation<MaterializerNavigationLink>(materializerNavigationLink);
			return materializerNavigationLink;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00025479 File Offset: 0x00023679
		public static MaterializerNavigationLink GetLink(ODataNavigationLink link)
		{
			return link.GetAnnotation<MaterializerNavigationLink>();
		}

		// Token: 0x0400055C RID: 1372
		private readonly ODataNavigationLink link;

		// Token: 0x0400055D RID: 1373
		private readonly object feedOrEntry;
	}
}
