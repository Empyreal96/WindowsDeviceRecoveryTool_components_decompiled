using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B4 RID: 180
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1583EFE9-832F-4d08-B041-CAC5ACEDB948")]
	[ComImport]
	internal interface IEntryPointEntry
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600029F RID: 671
		EntryPointEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002A0 RID: 672
		string Name { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002A1 RID: 673
		string CommandLine_File { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002A2 RID: 674
		string CommandLine_Parameters { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002A3 RID: 675
		IReferenceIdentity Identity { [SecurityCritical] get; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002A4 RID: 676
		uint Flags { [SecurityCritical] get; }
	}
}
