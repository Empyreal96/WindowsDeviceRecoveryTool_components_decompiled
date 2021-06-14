using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000D1 RID: 209
	public interface IEdmNavigationTargetMapping
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000438 RID: 1080
		IEdmNavigationProperty NavigationProperty { get; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000439 RID: 1081
		IEdmEntitySet TargetEntitySet { get; }
	}
}
