using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000BA RID: 186
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("2474ECB4-8EFD-4410-9F31-B3E7C4A07731")]
	[ComImport]
	internal interface IAssemblyRequestEntry
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002AA RID: 682
		AssemblyRequestEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002AB RID: 683
		string Name { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002AC RID: 684
		string permissionSetID { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
