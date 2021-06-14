using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020001CF RID: 463
	[Flags]
	public enum ODataUndeclaredPropertyBehaviorKinds
	{
		// Token: 0x040004BC RID: 1212
		None = 0,
		// Token: 0x040004BD RID: 1213
		IgnoreUndeclaredValueProperty = 1,
		// Token: 0x040004BE RID: 1214
		ReportUndeclaredLinkProperty = 2
	}
}
