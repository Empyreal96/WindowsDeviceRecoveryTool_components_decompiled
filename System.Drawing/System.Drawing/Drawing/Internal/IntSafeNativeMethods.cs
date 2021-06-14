using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Internal
{
	// Token: 0x020000E3 RID: 227
	[SuppressUnmanagedCodeSecurity]
	internal static class IntSafeNativeMethods
	{
		// Token: 0x06000BBC RID: 3004
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateSolidBrush", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateSolidBrush(int crColor);

		// Token: 0x06000BBD RID: 3005 RVA: 0x0002ACA8 File Offset: 0x00028EA8
		public static IntPtr CreateSolidBrush(int crColor)
		{
			return System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreateSolidBrush(crColor), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000BBE RID: 3006
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreatePen(int fnStyle, int nWidth, int crColor);

		// Token: 0x06000BBF RID: 3007 RVA: 0x0002ACC8 File Offset: 0x00028EC8
		public static IntPtr CreatePen(int fnStyle, int nWidth, int crColor)
		{
			return System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreatePen(fnStyle, nWidth, crColor), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000BC0 RID: 3008
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "ExtCreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntExtCreatePen(int fnStyle, int dwWidth, IntNativeMethods.LOGBRUSH lplb, int dwStyleCount, [MarshalAs(UnmanagedType.LPArray)] int[] lpStyle);

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002ACEC File Offset: 0x00028EEC
		public static IntPtr ExtCreatePen(int fnStyle, int dwWidth, IntNativeMethods.LOGBRUSH lplb, int dwStyleCount, int[] lpStyle)
		{
			return System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntExtCreatePen(fnStyle, dwWidth, lplb, dwStyleCount, lpStyle), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000BC2 RID: 3010
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002AD10 File Offset: 0x00028F10
		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return System.Internal.HandleCollector.Add(IntSafeNativeMethods.IntCreateRectRgn(x1, y1, x2, y2), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000BC4 RID: 3012
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetUserDefaultLCID();

		// Token: 0x06000BC5 RID: 3013
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GdiFlush();

		// Token: 0x0200012F RID: 303
		public sealed class CommonHandles
		{
			// Token: 0x04000CC0 RID: 3264
			public static readonly int EMF = System.Internal.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);

			// Token: 0x04000CC1 RID: 3265
			public static readonly int GDI = System.Internal.HandleCollector.RegisterType("GDI", 90, 50);

			// Token: 0x04000CC2 RID: 3266
			public static readonly int HDC = System.Internal.HandleCollector.RegisterType("HDC", 100, 2);
		}
	}
}
