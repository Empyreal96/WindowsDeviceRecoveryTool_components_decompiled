using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000275 RID: 629
	public sealed class ODataResourceCollectionInfo : ODataAnnotatable
	{
		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060014D1 RID: 5329 RVA: 0x0004D5CD File Offset: 0x0004B7CD
		// (set) Token: 0x060014D2 RID: 5330 RVA: 0x0004D5D5 File Offset: 0x0004B7D5
		public Uri Url { get; set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060014D3 RID: 5331 RVA: 0x0004D5DE File Offset: 0x0004B7DE
		// (set) Token: 0x060014D4 RID: 5332 RVA: 0x0004D5E6 File Offset: 0x0004B7E6
		public string Name { get; set; }
	}
}
