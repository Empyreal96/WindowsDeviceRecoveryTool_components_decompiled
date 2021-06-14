using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000A8 RID: 168
	[StructLayout(LayoutKind.Sequential)]
	internal class MetafileHeaderEmf
	{
		// Token: 0x040008F6 RID: 2294
		public MetafileType type;

		// Token: 0x040008F7 RID: 2295
		public int size;

		// Token: 0x040008F8 RID: 2296
		public int version;

		// Token: 0x040008F9 RID: 2297
		public EmfPlusFlags emfPlusFlags;

		// Token: 0x040008FA RID: 2298
		public float dpiX;

		// Token: 0x040008FB RID: 2299
		public float dpiY;

		// Token: 0x040008FC RID: 2300
		public int X;

		// Token: 0x040008FD RID: 2301
		public int Y;

		// Token: 0x040008FE RID: 2302
		public int Width;

		// Token: 0x040008FF RID: 2303
		public int Height;

		// Token: 0x04000900 RID: 2304
		public SafeNativeMethods.ENHMETAHEADER EmfHeader;

		// Token: 0x04000901 RID: 2305
		public int EmfPlusHeaderSize;

		// Token: 0x04000902 RID: 2306
		public int LogicalDpiX;

		// Token: 0x04000903 RID: 2307
		public int LogicalDpiY;
	}
}
