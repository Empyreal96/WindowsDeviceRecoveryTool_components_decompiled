using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000D7 RID: 215
	internal class TypeInformation
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00029383 File Offset: 0x00027583
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x0002938B File Offset: 0x0002758B
		public Type Type { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00029394 File Offset: 0x00027594
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x0002939C File Offset: 0x0002759C
		public PrimitiveTypeCode TypeCode { get; set; }
	}
}
