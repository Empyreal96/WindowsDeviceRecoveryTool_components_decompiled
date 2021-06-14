using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Standard
{
	// Token: 0x0200008C RID: 140
	internal static class Utility
	{
		// Token: 0x060001AE RID: 430 RVA: 0x00003E4B File Offset: 0x0000204B
		public static Color ColorFromArgbDword(uint color)
		{
			return Color.FromArgb((byte)((color & 4278190080U) >> 24), (byte)((color & 16711680U) >> 16), (byte)((color & 65280U) >> 8), (byte)(color & 255U));
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00003E7A File Offset: 0x0000207A
		public static int GET_X_LPARAM(IntPtr lParam)
		{
			return Utility.LOWORD(lParam.ToInt32());
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00003E88 File Offset: 0x00002088
		public static int GET_Y_LPARAM(IntPtr lParam)
		{
			return Utility.HIWORD(lParam.ToInt32());
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00003E96 File Offset: 0x00002096
		public static int HIWORD(int i)
		{
			return (int)((short)(i >> 16));
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00003E9D File Offset: 0x0000209D
		public static int LOWORD(int i)
		{
			return (int)((short)(i & 65535));
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00003EA7 File Offset: 0x000020A7
		public static bool IsFlagSet(int value, int mask)
		{
			return (value & mask) != 0;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00003EA7 File Offset: 0x000020A7
		public static bool IsFlagSet(uint value, uint mask)
		{
			return (value & mask) > 0U;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00003EAF File Offset: 0x000020AF
		public static bool IsFlagSet(long value, long mask)
		{
			return (value & mask) != 0L;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00003EAF File Offset: 0x000020AF
		public static bool IsFlagSet(ulong value, ulong mask)
		{
			return (value & mask) > 0UL;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00003EB8 File Offset: 0x000020B8
		public static bool IsOSVistaOrNewer
		{
			get
			{
				return Utility._osVersion >= new Version(6, 0);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00003ECB File Offset: 0x000020CB
		public static bool IsOSWindows7OrNewer
		{
			get
			{
				return Utility._osVersion >= new Version(6, 1);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00003EDE File Offset: 0x000020DE
		public static bool IsPresentationFrameworkVersionLessThan4
		{
			get
			{
				return Utility._presentationFrameworkVersion < new Version(4, 0);
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00003EF1 File Offset: 0x000020F1
		public static BitmapFrame GetBestMatch(IList<BitmapFrame> frames, int width, int height)
		{
			return Utility._GetBestMatch(frames, Utility._GetBitDepth(), width, height);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00003F00 File Offset: 0x00002100
		private static int _MatchImage(BitmapFrame frame, int bitDepth, int width, int height, int bpp)
		{
			return 2 * Utility._WeightedAbs(bpp, bitDepth, false) + Utility._WeightedAbs(frame.PixelWidth, width, true) + Utility._WeightedAbs(frame.PixelHeight, height, true);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00003F38 File Offset: 0x00002138
		private static int _WeightedAbs(int valueHave, int valueWant, bool fPunish)
		{
			int num = valueHave - valueWant;
			if (num < 0)
			{
				num = (fPunish ? -2 : -1) * num;
			}
			return num;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00003F5C File Offset: 0x0000215C
		private static BitmapFrame _GetBestMatch(IList<BitmapFrame> frames, int bitDepth, int width, int height)
		{
			int num = int.MaxValue;
			int num2 = 0;
			int index = 0;
			bool flag = frames[0].Decoder is IconBitmapDecoder;
			int num3 = 0;
			while (num3 < frames.Count && num != 0)
			{
				int num4 = flag ? frames[num3].Thumbnail.Format.BitsPerPixel : frames[num3].Format.BitsPerPixel;
				if (num4 == 0)
				{
					num4 = 8;
				}
				int num5 = Utility._MatchImage(frames[num3], bitDepth, width, height, num4);
				if (num5 < num)
				{
					index = num3;
					num2 = num4;
					num = num5;
				}
				else if (num5 == num && num2 < num4)
				{
					index = num3;
					num2 = num4;
				}
				num3++;
			}
			return frames[index];
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00004020 File Offset: 0x00002220
		[SecuritySafeCritical]
		private static int _GetBitDepth()
		{
			if (Utility.s_bitDepth == 0)
			{
				using (SafeDC desktop = SafeDC.GetDesktop())
				{
					Utility.s_bitDepth = NativeMethods.GetDeviceCaps(desktop, DeviceCap.BITSPIXEL) * NativeMethods.GetDeviceCaps(desktop, DeviceCap.PLANES);
				}
			}
			return Utility.s_bitDepth;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00004074 File Offset: 0x00002274
		[SecurityCritical]
		public static void SafeDeleteObject(ref IntPtr gdiObject)
		{
			IntPtr intPtr = gdiObject;
			gdiObject = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				NativeMethods.DeleteObject(intPtr);
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000040A0 File Offset: 0x000022A0
		[SecurityCritical]
		public static void SafeDestroyWindow(ref IntPtr hwnd)
		{
			IntPtr hwnd2 = hwnd;
			hwnd = IntPtr.Zero;
			if (NativeMethods.IsWindow(hwnd2))
			{
				NativeMethods.DestroyWindow(hwnd2);
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000040C8 File Offset: 0x000022C8
		[SecurityCritical]
		public static void SafeRelease<T>(ref T comObject) where T : class
		{
			T t = comObject;
			comObject = default(T);
			if (t != null)
			{
				Marshal.ReleaseComObject(t);
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000040F8 File Offset: 0x000022F8
		public static void AddDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
		{
			if (component == null)
			{
				return;
			}
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(property, component.GetType());
			dependencyPropertyDescriptor.AddValueChanged(component, listener);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00004120 File Offset: 0x00002320
		public static void RemoveDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
		{
			if (component == null)
			{
				return;
			}
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(property, component.GetType());
			dependencyPropertyDescriptor.RemoveValueChanged(component, listener);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00004148 File Offset: 0x00002348
		public static bool IsThicknessNonNegative(Thickness thickness)
		{
			return Utility.IsDoubleFiniteAndNonNegative(thickness.Top) && Utility.IsDoubleFiniteAndNonNegative(thickness.Left) && Utility.IsDoubleFiniteAndNonNegative(thickness.Bottom) && Utility.IsDoubleFiniteAndNonNegative(thickness.Right);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00004198 File Offset: 0x00002398
		public static bool IsCornerRadiusValid(CornerRadius cornerRadius)
		{
			return Utility.IsDoubleFiniteAndNonNegative(cornerRadius.TopLeft) && Utility.IsDoubleFiniteAndNonNegative(cornerRadius.TopRight) && Utility.IsDoubleFiniteAndNonNegative(cornerRadius.BottomLeft) && Utility.IsDoubleFiniteAndNonNegative(cornerRadius.BottomRight);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000041E6 File Offset: 0x000023E6
		public static bool IsDoubleFiniteAndNonNegative(double d)
		{
			return !double.IsNaN(d) && !double.IsInfinity(d) && d >= 0.0;
		}

		// Token: 0x04000574 RID: 1396
		private static readonly Version _osVersion = Environment.OSVersion.Version;

		// Token: 0x04000575 RID: 1397
		private static readonly Version _presentationFrameworkVersion = Assembly.GetAssembly(typeof(Window)).GetName().Version;

		// Token: 0x04000576 RID: 1398
		private static int s_bitDepth;
	}
}
