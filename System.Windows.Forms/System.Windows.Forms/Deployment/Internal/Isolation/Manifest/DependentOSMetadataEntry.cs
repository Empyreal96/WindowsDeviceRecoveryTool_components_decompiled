using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000C1 RID: 193
	[StructLayout(LayoutKind.Sequential)]
	internal class DependentOSMetadataEntry
	{
		// Token: 0x040002FD RID: 765
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x040002FE RID: 766
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x040002FF RID: 767
		public ushort MajorVersion;

		// Token: 0x04000300 RID: 768
		public ushort MinorVersion;

		// Token: 0x04000301 RID: 769
		public ushort BuildNumber;

		// Token: 0x04000302 RID: 770
		public byte ServicePackMajor;

		// Token: 0x04000303 RID: 771
		public byte ServicePackMinor;
	}
}
