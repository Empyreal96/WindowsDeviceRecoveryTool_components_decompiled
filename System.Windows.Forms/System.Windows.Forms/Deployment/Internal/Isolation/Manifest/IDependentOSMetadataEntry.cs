using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000C3 RID: 195
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CF168CF4-4E8F-4d92-9D2A-60E5CA21CF85")]
	[ComImport]
	internal interface IDependentOSMetadataEntry
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002BD RID: 701
		DependentOSMetadataEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002BE RID: 702
		string SupportUrl { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002BF RID: 703
		string Description { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002C0 RID: 704
		ushort MajorVersion { [SecurityCritical] get; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002C1 RID: 705
		ushort MinorVersion { [SecurityCritical] get; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002C2 RID: 706
		ushort BuildNumber { [SecurityCritical] get; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002C3 RID: 707
		byte ServicePackMajor { [SecurityCritical] get; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002C4 RID: 708
		byte ServicePackMinor { [SecurityCritical] get; }
	}
}
