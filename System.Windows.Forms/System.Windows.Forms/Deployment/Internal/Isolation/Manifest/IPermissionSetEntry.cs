using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B7 RID: 183
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("EBE5A1ED-FEBC-42c4-A9E1-E087C6E36635")]
	[ComImport]
	internal interface IPermissionSetEntry
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002A6 RID: 678
		PermissionSetEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002A7 RID: 679
		string Id { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002A8 RID: 680
		string XmlSegment { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
