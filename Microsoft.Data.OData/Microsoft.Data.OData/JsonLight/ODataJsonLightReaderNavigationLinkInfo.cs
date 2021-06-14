using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000162 RID: 354
	internal sealed class ODataJsonLightReaderNavigationLinkInfo
	{
		// Token: 0x060009BA RID: 2490 RVA: 0x0001EF8A File Offset: 0x0001D18A
		private ODataJsonLightReaderNavigationLinkInfo(ODataNavigationLink navigationLink, IEdmNavigationProperty navigationProperty, bool isExpanded)
		{
			this.navigationLink = navigationLink;
			this.navigationProperty = navigationProperty;
			this.isExpanded = isExpanded;
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0001EFA7 File Offset: 0x0001D1A7
		internal ODataNavigationLink NavigationLink
		{
			get
			{
				return this.navigationLink;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0001EFAF File Offset: 0x0001D1AF
		internal IEdmNavigationProperty NavigationProperty
		{
			get
			{
				return this.navigationProperty;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0001EFB7 File Offset: 0x0001D1B7
		internal bool IsExpanded
		{
			get
			{
				return this.isExpanded;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0001EFBF File Offset: 0x0001D1BF
		internal ODataFeed ExpandedFeed
		{
			get
			{
				return this.expandedFeed;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0001EFC7 File Offset: 0x0001D1C7
		internal bool HasEntityReferenceLink
		{
			get
			{
				return this.entityReferenceLinks != null && this.entityReferenceLinks.First != null;
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0001EFE4 File Offset: 0x0001D1E4
		internal static ODataJsonLightReaderNavigationLinkInfo CreateDeferredLinkInfo(ODataNavigationLink navigationLink, IEdmNavigationProperty navigationProperty)
		{
			return new ODataJsonLightReaderNavigationLinkInfo(navigationLink, navigationProperty, false);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0001EFF0 File Offset: 0x0001D1F0
		internal static ODataJsonLightReaderNavigationLinkInfo CreateExpandedEntryLinkInfo(ODataNavigationLink navigationLink, IEdmNavigationProperty navigationProperty)
		{
			return new ODataJsonLightReaderNavigationLinkInfo(navigationLink, navigationProperty, true);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0001F008 File Offset: 0x0001D208
		internal static ODataJsonLightReaderNavigationLinkInfo CreateExpandedFeedLinkInfo(ODataNavigationLink navigationLink, IEdmNavigationProperty navigationProperty, ODataFeed expandedFeed)
		{
			return new ODataJsonLightReaderNavigationLinkInfo(navigationLink, navigationProperty, true)
			{
				expandedFeed = expandedFeed
			};
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0001F028 File Offset: 0x0001D228
		internal static ODataJsonLightReaderNavigationLinkInfo CreateSingletonEntityReferenceLinkInfo(ODataNavigationLink navigationLink, IEdmNavigationProperty navigationProperty, ODataEntityReferenceLink entityReferenceLink, bool isExpanded)
		{
			ODataJsonLightReaderNavigationLinkInfo odataJsonLightReaderNavigationLinkInfo = new ODataJsonLightReaderNavigationLinkInfo(navigationLink, navigationProperty, isExpanded);
			if (entityReferenceLink != null)
			{
				odataJsonLightReaderNavigationLinkInfo.entityReferenceLinks = new LinkedList<ODataEntityReferenceLink>();
				odataJsonLightReaderNavigationLinkInfo.entityReferenceLinks.AddFirst(entityReferenceLink);
			}
			return odataJsonLightReaderNavigationLinkInfo;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0001F05C File Offset: 0x0001D25C
		internal static ODataJsonLightReaderNavigationLinkInfo CreateCollectionEntityReferenceLinksInfo(ODataNavigationLink navigationLink, IEdmNavigationProperty navigationProperty, LinkedList<ODataEntityReferenceLink> entityReferenceLinks, bool isExpanded)
		{
			return new ODataJsonLightReaderNavigationLinkInfo(navigationLink, navigationProperty, isExpanded)
			{
				entityReferenceLinks = entityReferenceLinks
			};
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0001F07C File Offset: 0x0001D27C
		internal static ODataJsonLightReaderNavigationLinkInfo CreateProjectedNavigationLinkInfo(IEdmNavigationProperty navigationProperty)
		{
			ODataNavigationLink odataNavigationLink = new ODataNavigationLink
			{
				Name = navigationProperty.Name,
				IsCollection = new bool?(navigationProperty.Type.IsCollection())
			};
			return new ODataJsonLightReaderNavigationLinkInfo(odataNavigationLink, navigationProperty, false);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0001F0C0 File Offset: 0x0001D2C0
		internal ODataEntityReferenceLink ReportEntityReferenceLink()
		{
			if (this.entityReferenceLinks != null && this.entityReferenceLinks.First != null)
			{
				ODataEntityReferenceLink value = this.entityReferenceLinks.First.Value;
				this.entityReferenceLinks.RemoveFirst();
				return value;
			}
			return null;
		}

		// Token: 0x04000397 RID: 919
		private readonly ODataNavigationLink navigationLink;

		// Token: 0x04000398 RID: 920
		private readonly IEdmNavigationProperty navigationProperty;

		// Token: 0x04000399 RID: 921
		private readonly bool isExpanded;

		// Token: 0x0400039A RID: 922
		private ODataFeed expandedFeed;

		// Token: 0x0400039B RID: 923
		private LinkedList<ODataEntityReferenceLink> entityReferenceLinks;
	}
}
