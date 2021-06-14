using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000224 RID: 548
	public sealed class AtomStreamReferenceMetadata : ODataAnnotatable
	{
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x00040263 File Offset: 0x0003E463
		// (set) Token: 0x0600112C RID: 4396 RVA: 0x0004026B File Offset: 0x0003E46B
		public AtomLinkMetadata SelfLink { get; set; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x00040274 File Offset: 0x0003E474
		// (set) Token: 0x0600112E RID: 4398 RVA: 0x0004027C File Offset: 0x0003E47C
		public AtomLinkMetadata EditLink { get; set; }
	}
}
