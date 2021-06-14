using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000094 RID: 148
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipDataEntry
	{
		// Token: 0x04000284 RID: 644
		public uint index;

		// Token: 0x04000285 RID: 645
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;

		// Token: 0x04000286 RID: 646
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;
	}
}
