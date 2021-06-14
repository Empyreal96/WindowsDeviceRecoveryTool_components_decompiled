using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000DB RID: 219
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9f27c750-7dfb-46a1-a673-52e53e2337a9")]
	[ComImport]
	internal interface IDirectoryEntry
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600030B RID: 779
		DirectoryEntry AllData { [SecurityCritical] get; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600030C RID: 780
		uint Flags { [SecurityCritical] get; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600030D RID: 781
		uint Protection { [SecurityCritical] get; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600030E RID: 782
		string BuildFilter { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600030F RID: 783
		object SecurityDescriptor { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }
	}
}
