using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020000FC RID: 252
	internal sealed class ODataAtomReaderNavigationLinkDescriptor
	{
		// Token: 0x060006BB RID: 1723 RVA: 0x00017E87 File Offset: 0x00016087
		internal ODataAtomReaderNavigationLinkDescriptor(ODataNavigationLink navigationLink, IEdmNavigationProperty navigationProperty)
		{
			this.navigationLink = navigationLink;
			this.navigationProperty = navigationProperty;
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00017E9D File Offset: 0x0001609D
		internal ODataNavigationLink NavigationLink
		{
			get
			{
				return this.navigationLink;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x00017EA5 File Offset: 0x000160A5
		internal IEdmNavigationProperty NavigationProperty
		{
			get
			{
				return this.navigationProperty;
			}
		}

		// Token: 0x04000297 RID: 663
		private ODataNavigationLink navigationLink;

		// Token: 0x04000298 RID: 664
		private IEdmNavigationProperty navigationProperty;
	}
}
