using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000C0 RID: 192
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CFA3F59F-334D-46bf-A5A5-5D11BB2D7EBC")]
	[ComImport]
	internal interface IDeploymentMetadataEntry
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002B6 RID: 694
		DeploymentMetadataEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002B7 RID: 695
		string DeploymentProviderCodebase { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002B8 RID: 696
		string MinimumRequiredVersion { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002B9 RID: 697
		ushort MaximumAge { [SecurityCritical] get; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002BA RID: 698
		byte MaximumAge_Unit { [SecurityCritical] get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002BB RID: 699
		uint DeploymentFlags { [SecurityCritical] get; }
	}
}
