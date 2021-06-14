using System;

namespace Microsoft.Diagnostics.Telemetry
{
	// Token: 0x02000061 RID: 97
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
	internal sealed class EventProviderAttribute : Attribute
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000B744 File Offset: 0x00009944
		// (set) Token: 0x06000245 RID: 581 RVA: 0x0000B74C File Offset: 0x0000994C
		public string Provider { get; private set; }

		// Token: 0x06000246 RID: 582 RVA: 0x0000B755 File Offset: 0x00009955
		public EventProviderAttribute(string providerName)
		{
			this.Provider = providerName;
		}

		// Token: 0x040001E2 RID: 482
		public const string TelemetryGroupId = "{4f50731a-89cf-4782-b3e0-dce8c90476ba}";
	}
}
