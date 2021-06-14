using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x0200020B RID: 523
	internal interface IODataVerboseJsonReaderEntryState
	{
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000FF9 RID: 4089
		ODataEntry Entry { get; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000FFA RID: 4090
		IEdmEntityType EntityType { get; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000FFB RID: 4091
		// (set) Token: 0x06000FFC RID: 4092
		bool MetadataPropertyFound { get; set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000FFD RID: 4093
		// (set) Token: 0x06000FFE RID: 4094
		ODataNavigationLink FirstNavigationLink { get; set; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000FFF RID: 4095
		// (set) Token: 0x06001000 RID: 4096
		IEdmNavigationProperty FirstNavigationProperty { get; set; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001001 RID: 4097
		DuplicatePropertyNamesChecker DuplicatePropertyNamesChecker { get; }
	}
}
