using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200017F RID: 383
	internal interface IODataJsonLightReaderEntryState
	{
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000ABC RID: 2748
		ODataEntry Entry { get; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000ABD RID: 2749
		IEdmEntityType EntityType { get; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000ABE RID: 2750
		// (set) Token: 0x06000ABF RID: 2751
		ODataEntityMetadataBuilder MetadataBuilder { get; set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000AC0 RID: 2752
		// (set) Token: 0x06000AC1 RID: 2753
		bool AnyPropertyFound { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000AC2 RID: 2754
		// (set) Token: 0x06000AC3 RID: 2755
		ODataJsonLightReaderNavigationLinkInfo FirstNavigationLinkInfo { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000AC4 RID: 2756
		DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker { get; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000AC5 RID: 2757
		SelectedPropertiesNode SelectedProperties { get; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000AC6 RID: 2758
		List<string> NavigationPropertiesRead { get; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000AC7 RID: 2759
		// (set) Token: 0x06000AC8 RID: 2760
		bool ProcessingMissingProjectedNavigationLinks { get; set; }
	}
}
