using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004F3 RID: 1267
	[SuppressUnmanagedCodeSecurity]
	internal static class IntSafeNativeMethods
	{
		// Token: 0x06005331 RID: 21297
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateSolidBrush", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateSolidBrush(int crColor);

		// Token: 0x06005332 RID: 21298 RVA: 0x0015CFEC File Offset: 0x0015B1EC
		public static IntPtr CreateSolidBrush(int crColor)
		{
			return System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreateSolidBrush(crColor), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06005333 RID: 21299
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreatePen(int fnStyle, int nWidth, int crColor);

		// Token: 0x06005334 RID: 21300 RVA: 0x0015D00C File Offset: 0x0015B20C
		public static IntPtr CreatePen(int fnStyle, int nWidth, int crColor)
		{
			return System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreatePen(fnStyle, nWidth, crColor), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06005335 RID: 21301
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "ExtCreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntExtCreatePen(int fnStyle, int dwWidth, IntNativeMethods.LOGBRUSH lplb, int dwStyleCount, [MarshalAs(UnmanagedType.LPArray)] int[] lpStyle);

		// Token: 0x06005336 RID: 21302 RVA: 0x0015D030 File Offset: 0x0015B230
		public static IntPtr ExtCreatePen(int fnStyle, int dwWidth, IntNativeMethods.LOGBRUSH lplb, int dwStyleCount, int[] lpStyle)
		{
			return System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntExtCreatePen(fnStyle, dwWidth, lplb, dwStyleCount, lpStyle), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06005337 RID: 21303
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x06005338 RID: 21304 RVA: 0x0015D054 File Offset: 0x0015B254
		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreateRectRgn(x1, y1, x2, y2), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06005339 RID: 21305
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetUserDefaultLCID();

		// Token: 0x0600533A RID: 21306
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GdiFlush();

		// Token: 0x0200086D RID: 2157
		public sealed class CommonHandles
		{
			// Token: 0x040043B3 RID: 17331
			public static readonly int EMF = System.Internal.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);

			// Token: 0x040043B4 RID: 17332
			public static readonly int GDI = System.Internal.HandleCollector.RegisterType("GDI", 90, 50);

			// Token: 0x040043B5 RID: 17333
			public static readonly int HDC = System.Internal.HandleCollector.RegisterType("HDC", 100, 2);
		}
	}
}
