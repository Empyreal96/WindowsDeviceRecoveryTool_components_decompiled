using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200009D RID: 157
	[StructLayout(LayoutKind.Sequential)]
	internal class COMServerEntry
	{
		// Token: 0x04000292 RID: 658
		public Guid Clsid;

		// Token: 0x04000293 RID: 659
		public uint Flags;

		// Token: 0x04000294 RID: 660
		public Guid ConfiguredGuid;

		// Token: 0x04000295 RID: 661
		public Guid ImplementedClsid;

		// Token: 0x04000296 RID: 662
		public Guid TypeLibrary;

		// Token: 0x04000297 RID: 663
		public uint ThreadingModel;

		// Token: 0x04000298 RID: 664
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x04000299 RID: 665
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostFile;
	}
}
