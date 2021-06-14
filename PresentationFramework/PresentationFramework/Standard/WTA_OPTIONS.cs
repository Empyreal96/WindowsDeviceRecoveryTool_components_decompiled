using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000053 RID: 83
	[StructLayout(LayoutKind.Explicit)]
	internal struct WTA_OPTIONS
	{
		// Token: 0x04000479 RID: 1145
		public const uint Size = 8U;

		// Token: 0x0400047A RID: 1146
		[FieldOffset(0)]
		public WTNCA dwFlags;

		// Token: 0x0400047B RID: 1147
		[FieldOffset(4)]
		public WTNCA dwMask;
	}
}
