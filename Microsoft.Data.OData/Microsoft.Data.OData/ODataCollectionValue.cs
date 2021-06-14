using System;
using System.Collections;

namespace Microsoft.Data.OData
{
	// Token: 0x02000294 RID: 660
	public sealed class ODataCollectionValue : ODataValue
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600163B RID: 5691 RVA: 0x00050EF5 File Offset: 0x0004F0F5
		// (set) Token: 0x0600163C RID: 5692 RVA: 0x00050EFD File Offset: 0x0004F0FD
		public string TypeName { get; set; }

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600163D RID: 5693 RVA: 0x00050F06 File Offset: 0x0004F106
		// (set) Token: 0x0600163E RID: 5694 RVA: 0x00050F0E File Offset: 0x0004F10E
		public IEnumerable Items { get; set; }
	}
}
