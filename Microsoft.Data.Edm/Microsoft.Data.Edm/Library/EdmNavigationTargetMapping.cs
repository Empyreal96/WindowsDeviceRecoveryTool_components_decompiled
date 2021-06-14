using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020000D6 RID: 214
	public class EdmNavigationTargetMapping : IEdmNavigationTargetMapping
	{
		// Token: 0x06000450 RID: 1104 RVA: 0x0000BDFC File Offset: 0x00009FFC
		public EdmNavigationTargetMapping(IEdmNavigationProperty navigationProperty, IEdmEntitySet targetEntitySet)
		{
			this.navigationProperty = navigationProperty;
			this.targetEntitySet = targetEntitySet;
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0000BE12 File Offset: 0x0000A012
		public IEdmNavigationProperty NavigationProperty
		{
			get
			{
				return this.navigationProperty;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0000BE1A File Offset: 0x0000A01A
		public IEdmEntitySet TargetEntitySet
		{
			get
			{
				return this.targetEntitySet;
			}
		}

		// Token: 0x040001A7 RID: 423
		private IEdmNavigationProperty navigationProperty;

		// Token: 0x040001A8 RID: 424
		private IEdmEntitySet targetEntitySet;
	}
}
