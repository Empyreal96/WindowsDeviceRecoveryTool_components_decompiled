using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200005E RID: 94
	internal struct IStore_BindingResult_BoundVersion
	{
		// Token: 0x0400018D RID: 397
		[MarshalAs(UnmanagedType.U2)]
		public ushort Revision;

		// Token: 0x0400018E RID: 398
		[MarshalAs(UnmanagedType.U2)]
		public ushort Build;

		// Token: 0x0400018F RID: 399
		[MarshalAs(UnmanagedType.U2)]
		public ushort Minor;

		// Token: 0x04000190 RID: 400
		[MarshalAs(UnmanagedType.U2)]
		public ushort Major;
	}
}
