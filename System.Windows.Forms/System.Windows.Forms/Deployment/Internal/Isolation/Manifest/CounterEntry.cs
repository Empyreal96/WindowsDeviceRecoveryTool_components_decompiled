using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000E2 RID: 226
	[StructLayout(LayoutKind.Sequential)]
	internal class CounterEntry
	{
		// Token: 0x04000390 RID: 912
		public Guid CounterSetGuid;

		// Token: 0x04000391 RID: 913
		public uint CounterId;

		// Token: 0x04000392 RID: 914
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000393 RID: 915
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000394 RID: 916
		public uint CounterType;

		// Token: 0x04000395 RID: 917
		public ulong Attributes;

		// Token: 0x04000396 RID: 918
		public uint BaseId;

		// Token: 0x04000397 RID: 919
		public uint DefaultScale;
	}
}
