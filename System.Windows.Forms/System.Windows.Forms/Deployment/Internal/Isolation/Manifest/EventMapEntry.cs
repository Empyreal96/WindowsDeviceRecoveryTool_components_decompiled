using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000CD RID: 205
	[StructLayout(LayoutKind.Sequential)]
	internal class EventMapEntry
	{
		// Token: 0x0400034C RID: 844
		[MarshalAs(UnmanagedType.LPWStr)]
		public string MapName;

		// Token: 0x0400034D RID: 845
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x0400034E RID: 846
		public uint Value;

		// Token: 0x0400034F RID: 847
		public bool IsValueMap;
	}
}
