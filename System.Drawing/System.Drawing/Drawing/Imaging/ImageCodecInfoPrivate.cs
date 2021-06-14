using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000A1 RID: 161
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal class ImageCodecInfoPrivate
	{
		// Token: 0x040008BF RID: 2239
		[MarshalAs(UnmanagedType.Struct)]
		public Guid Clsid;

		// Token: 0x040008C0 RID: 2240
		[MarshalAs(UnmanagedType.Struct)]
		public Guid FormatID;

		// Token: 0x040008C1 RID: 2241
		public IntPtr CodecName = IntPtr.Zero;

		// Token: 0x040008C2 RID: 2242
		public IntPtr DllName = IntPtr.Zero;

		// Token: 0x040008C3 RID: 2243
		public IntPtr FormatDescription = IntPtr.Zero;

		// Token: 0x040008C4 RID: 2244
		public IntPtr FilenameExtension = IntPtr.Zero;

		// Token: 0x040008C5 RID: 2245
		public IntPtr MimeType = IntPtr.Zero;

		// Token: 0x040008C6 RID: 2246
		public int Flags;

		// Token: 0x040008C7 RID: 2247
		public int Version;

		// Token: 0x040008C8 RID: 2248
		public int SigCount;

		// Token: 0x040008C9 RID: 2249
		public int SigSize;

		// Token: 0x040008CA RID: 2250
		public IntPtr SigPattern = IntPtr.Zero;

		// Token: 0x040008CB RID: 2251
		public IntPtr SigMask = IntPtr.Zero;
	}
}
