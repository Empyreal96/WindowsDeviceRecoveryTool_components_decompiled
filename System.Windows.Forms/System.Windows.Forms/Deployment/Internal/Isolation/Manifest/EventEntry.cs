using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000CA RID: 202
	[StructLayout(LayoutKind.Sequential)]
	internal class EventEntry
	{
		// Token: 0x0400033C RID: 828
		public uint EventID;

		// Token: 0x0400033D RID: 829
		public uint Level;

		// Token: 0x0400033E RID: 830
		public uint Version;

		// Token: 0x0400033F RID: 831
		public Guid Guid;

		// Token: 0x04000340 RID: 832
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SubTypeName;

		// Token: 0x04000341 RID: 833
		public uint SubTypeValue;

		// Token: 0x04000342 RID: 834
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DisplayName;

		// Token: 0x04000343 RID: 835
		public uint EventNameMicrodomIndex;
	}
}
