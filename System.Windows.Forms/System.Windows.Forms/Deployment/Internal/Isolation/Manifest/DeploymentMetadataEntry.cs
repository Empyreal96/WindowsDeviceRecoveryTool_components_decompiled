using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000BE RID: 190
	[StructLayout(LayoutKind.Sequential)]
	internal class DeploymentMetadataEntry
	{
		// Token: 0x040002F2 RID: 754
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DeploymentProviderCodebase;

		// Token: 0x040002F3 RID: 755
		[MarshalAs(UnmanagedType.LPWStr)]
		public string MinimumRequiredVersion;

		// Token: 0x040002F4 RID: 756
		public ushort MaximumAge;

		// Token: 0x040002F5 RID: 757
		public byte MaximumAge_Unit;

		// Token: 0x040002F6 RID: 758
		public uint DeploymentFlags;
	}
}
