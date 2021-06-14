using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x020000FF RID: 255
	internal interface IODataEntryMetadataContext
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060006D5 RID: 1749
		ODataEntry Entry { get; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060006D6 RID: 1750
		IODataFeedAndEntryTypeContext TypeContext { get; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060006D7 RID: 1751
		string ActualEntityTypeName { get; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060006D8 RID: 1752
		ICollection<KeyValuePair<string, object>> KeyProperties { get; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060006D9 RID: 1753
		IEnumerable<KeyValuePair<string, object>> ETagProperties { get; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060006DA RID: 1754
		IEnumerable<IEdmNavigationProperty> SelectedNavigationProperties { get; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060006DB RID: 1755
		IDictionary<string, IEdmStructuralProperty> SelectedStreamProperties { get; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060006DC RID: 1756
		IEnumerable<IEdmFunctionImport> SelectedAlwaysBindableOperations { get; }
	}
}
