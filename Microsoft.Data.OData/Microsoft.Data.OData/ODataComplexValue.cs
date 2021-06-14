using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x020002A8 RID: 680
	public sealed class ODataComplexValue : ODataValue
	{
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060016EB RID: 5867 RVA: 0x00052F2A File Offset: 0x0005112A
		// (set) Token: 0x060016EC RID: 5868 RVA: 0x00052F32 File Offset: 0x00051132
		public IEnumerable<ODataProperty> Properties { get; set; }

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x00052F3B File Offset: 0x0005113B
		// (set) Token: 0x060016EE RID: 5870 RVA: 0x00052F43 File Offset: 0x00051143
		public string TypeName { get; set; }
	}
}
