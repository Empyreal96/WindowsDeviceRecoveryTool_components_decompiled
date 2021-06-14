using System;
using Microsoft.Diagnostics.Tracing;

namespace Microsoft.Diagnostics.Telemetry
{
	// Token: 0x02000060 RID: 96
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	internal sealed class EventDescriptionAttribute : Attribute
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000B6D6 File Offset: 0x000098D6
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000B6DE File Offset: 0x000098DE
		public string Description { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000B6E7 File Offset: 0x000098E7
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000B6EF File Offset: 0x000098EF
		public EventKeywords Keywords { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000B6F8 File Offset: 0x000098F8
		// (set) Token: 0x0600023C RID: 572 RVA: 0x0000B700 File Offset: 0x00009900
		public EventLevel Level { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000B709 File Offset: 0x00009909
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0000B711 File Offset: 0x00009911
		public EventOpcode Opcode { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000B71A File Offset: 0x0000991A
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000B722 File Offset: 0x00009922
		public EventTags Tags { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000B72B File Offset: 0x0000992B
		// (set) Token: 0x06000242 RID: 578 RVA: 0x0000B733 File Offset: 0x00009933
		public EventActivityOptions ActivityOptions { get; set; }
	}
}
