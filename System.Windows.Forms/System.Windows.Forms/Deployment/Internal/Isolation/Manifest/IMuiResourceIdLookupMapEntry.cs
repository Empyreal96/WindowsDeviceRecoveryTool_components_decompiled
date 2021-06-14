using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000081 RID: 129
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("24abe1f7-a396-4a03-9adf-1d5b86a5569f")]
	[ComImport]
	internal interface IMuiResourceIdLookupMapEntry
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000225 RID: 549
		MuiResourceIdLookupMapEntry AllData { [SecurityCritical] get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000226 RID: 550
		uint Count { [SecurityCritical] get; }
	}
}
