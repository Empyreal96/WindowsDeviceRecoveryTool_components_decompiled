using System;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000BD RID: 189
	public sealed class ListBlockItem
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0003F0A0 File Offset: 0x0003D2A0
		// (set) Token: 0x060010E1 RID: 4321 RVA: 0x0003F0A8 File Offset: 0x0003D2A8
		public string Name { get; internal set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x0003F0B1 File Offset: 0x0003D2B1
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x0003F0B9 File Offset: 0x0003D2B9
		public long Length { get; internal set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x0003F0C2 File Offset: 0x0003D2C2
		// (set) Token: 0x060010E5 RID: 4325 RVA: 0x0003F0CA File Offset: 0x0003D2CA
		public bool Committed { get; internal set; }
	}
}
