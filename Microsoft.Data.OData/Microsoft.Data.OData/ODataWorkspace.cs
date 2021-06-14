using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x02000276 RID: 630
	public sealed class ODataWorkspace : ODataAnnotatable
	{
		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x0004D5F7 File Offset: 0x0004B7F7
		// (set) Token: 0x060014D7 RID: 5335 RVA: 0x0004D5FF File Offset: 0x0004B7FF
		public IEnumerable<ODataResourceCollectionInfo> Collections { get; set; }
	}
}
