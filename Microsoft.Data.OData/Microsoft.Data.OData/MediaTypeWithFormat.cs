using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020001B2 RID: 434
	internal sealed class MediaTypeWithFormat
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0002EB0E File Offset: 0x0002CD0E
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x0002EB16 File Offset: 0x0002CD16
		public MediaType MediaType { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x0002EB1F File Offset: 0x0002CD1F
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x0002EB27 File Offset: 0x0002CD27
		public ODataFormat Format { get; set; }
	}
}
