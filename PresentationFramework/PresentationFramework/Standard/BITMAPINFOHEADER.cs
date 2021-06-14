using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000045 RID: 69
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct BITMAPINFOHEADER
	{
		// Token: 0x04000408 RID: 1032
		public int biSize;

		// Token: 0x04000409 RID: 1033
		public int biWidth;

		// Token: 0x0400040A RID: 1034
		public int biHeight;

		// Token: 0x0400040B RID: 1035
		public short biPlanes;

		// Token: 0x0400040C RID: 1036
		public short biBitCount;

		// Token: 0x0400040D RID: 1037
		public BI biCompression;

		// Token: 0x0400040E RID: 1038
		public int biSizeImage;

		// Token: 0x0400040F RID: 1039
		public int biXPelsPerMeter;

		// Token: 0x04000410 RID: 1040
		public int biYPelsPerMeter;

		// Token: 0x04000411 RID: 1041
		public int biClrUsed;

		// Token: 0x04000412 RID: 1042
		public int biClrImportant;
	}
}
