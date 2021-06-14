using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000E6 RID: 230
	public sealed class AtomCategoriesMetadata
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00013C98 File Offset: 0x00011E98
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x00013CA0 File Offset: 0x00011EA0
		public bool? Fixed { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00013CA9 File Offset: 0x00011EA9
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x00013CB1 File Offset: 0x00011EB1
		public string Scheme { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x00013CBA File Offset: 0x00011EBA
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x00013CC2 File Offset: 0x00011EC2
		public Uri Href { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00013CCB File Offset: 0x00011ECB
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x00013CD3 File Offset: 0x00011ED3
		public IEnumerable<AtomCategoryMetadata> Categories { get; set; }
	}
}
