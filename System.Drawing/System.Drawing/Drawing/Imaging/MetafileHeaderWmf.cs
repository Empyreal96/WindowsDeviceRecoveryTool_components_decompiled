using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000A9 RID: 169
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal class MetafileHeaderWmf
	{
		// Token: 0x04000904 RID: 2308
		public MetafileType type;

		// Token: 0x04000905 RID: 2309
		public int size = Marshal.SizeOf(typeof(MetafileHeaderWmf));

		// Token: 0x04000906 RID: 2310
		public int version;

		// Token: 0x04000907 RID: 2311
		public EmfPlusFlags emfPlusFlags;

		// Token: 0x04000908 RID: 2312
		public float dpiX;

		// Token: 0x04000909 RID: 2313
		public float dpiY;

		// Token: 0x0400090A RID: 2314
		public int X;

		// Token: 0x0400090B RID: 2315
		public int Y;

		// Token: 0x0400090C RID: 2316
		public int Width;

		// Token: 0x0400090D RID: 2317
		public int Height;

		// Token: 0x0400090E RID: 2318
		[MarshalAs(UnmanagedType.Struct)]
		public MetaHeader WmfHeader = new MetaHeader();

		// Token: 0x0400090F RID: 2319
		public int dummy1;

		// Token: 0x04000910 RID: 2320
		public int dummy2;

		// Token: 0x04000911 RID: 2321
		public int dummy3;

		// Token: 0x04000912 RID: 2322
		public int dummy4;

		// Token: 0x04000913 RID: 2323
		public int dummy5;

		// Token: 0x04000914 RID: 2324
		public int dummy6;

		// Token: 0x04000915 RID: 2325
		public int dummy7;

		// Token: 0x04000916 RID: 2326
		public int dummy8;

		// Token: 0x04000917 RID: 2327
		public int dummy9;

		// Token: 0x04000918 RID: 2328
		public int dummy10;

		// Token: 0x04000919 RID: 2329
		public int dummy11;

		// Token: 0x0400091A RID: 2330
		public int dummy12;

		// Token: 0x0400091B RID: 2331
		public int dummy13;

		// Token: 0x0400091C RID: 2332
		public int dummy14;

		// Token: 0x0400091D RID: 2333
		public int dummy15;

		// Token: 0x0400091E RID: 2334
		public int dummy16;

		// Token: 0x0400091F RID: 2335
		public int EmfPlusHeaderSize;

		// Token: 0x04000920 RID: 2336
		public int LogicalDpiX;

		// Token: 0x04000921 RID: 2337
		public int LogicalDpiY;
	}
}
