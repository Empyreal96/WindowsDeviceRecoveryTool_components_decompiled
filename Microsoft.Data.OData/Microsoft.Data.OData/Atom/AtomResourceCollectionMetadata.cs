using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000269 RID: 617
	public sealed class AtomResourceCollectionMetadata
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0004BC81 File Offset: 0x00049E81
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x0004BC89 File Offset: 0x00049E89
		public AtomTextConstruct Title { get; set; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x0004BC92 File Offset: 0x00049E92
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x0004BC9A File Offset: 0x00049E9A
		public string Accept { get; set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x0004BCA3 File Offset: 0x00049EA3
		// (set) Token: 0x0600145C RID: 5212 RVA: 0x0004BCAB File Offset: 0x00049EAB
		public AtomCategoriesMetadata Categories { get; set; }
	}
}
