using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200027D RID: 637
	public sealed class AtomCategoryMetadata : ODataAnnotatable
	{
		// Token: 0x0600152C RID: 5420 RVA: 0x0004D9D9 File Offset: 0x0004BBD9
		public AtomCategoryMetadata()
		{
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0004D9E1 File Offset: 0x0004BBE1
		internal AtomCategoryMetadata(AtomCategoryMetadata other)
		{
			if (other == null)
			{
				return;
			}
			this.Term = other.Term;
			this.Scheme = other.Scheme;
			this.Label = other.Label;
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x0004DA11 File Offset: 0x0004BC11
		// (set) Token: 0x0600152F RID: 5423 RVA: 0x0004DA19 File Offset: 0x0004BC19
		public string Term { get; set; }

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x0004DA22 File Offset: 0x0004BC22
		// (set) Token: 0x06001531 RID: 5425 RVA: 0x0004DA2A File Offset: 0x0004BC2A
		public string Scheme { get; set; }

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x0004DA33 File Offset: 0x0004BC33
		// (set) Token: 0x06001533 RID: 5427 RVA: 0x0004DA3B File Offset: 0x0004BC3B
		public string Label { get; set; }
	}
}
