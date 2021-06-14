using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A2 RID: 162
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("54F198EC-A63A-45ea-A984-452F68D9B35B")]
	[ComImport]
	internal interface IProgIdRedirectionEntry
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000279 RID: 633
		ProgIdRedirectionEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600027A RID: 634
		string ProgId { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600027B RID: 635
		Guid RedirectedGuid { [SecurityCritical] get; }
	}
}
