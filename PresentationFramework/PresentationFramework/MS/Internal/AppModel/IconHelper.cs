using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MS.Internal.Interop;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x0200077B RID: 1915
	internal static class IconHelper
	{
		// Token: 0x0600790E RID: 30990 RVA: 0x00226A14 File Offset: 0x00224C14
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void EnsureSystemMetrics()
		{
			if (IconHelper.s_systemBitDepth == 0)
			{
				HandleRef hDC = new HandleRef(null, UnsafeNativeMethods.GetDC(default(HandleRef)));
				try
				{
					int num = UnsafeNativeMethods.GetDeviceCaps(hDC, 12);
					num *= UnsafeNativeMethods.GetDeviceCaps(hDC, 14);
					if (num == 8)
					{
						num = 4;
					}
					int systemMetrics = UnsafeNativeMethods.GetSystemMetrics(SM.CXSMICON);
					int systemMetrics2 = UnsafeNativeMethods.GetSystemMetrics(SM.CYSMICON);
					int systemMetrics3 = UnsafeNativeMethods.GetSystemMetrics(SM.CXICON);
					int systemMetrics4 = UnsafeNativeMethods.GetSystemMetrics(SM.CYICON);
					IconHelper.s_smallIconSize = new Size((double)systemMetrics, (double)systemMetrics2);
					IconHelper.s_iconSize = new Size((double)systemMetrics3, (double)systemMetrics4);
					IconHelper.s_systemBitDepth = num;
				}
				finally
				{
					UnsafeNativeMethods.ReleaseDC(default(HandleRef), hDC);
				}
			}
		}

		// Token: 0x0600790F RID: 30991 RVA: 0x00226AC8 File Offset: 0x00224CC8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public static void GetDefaultIconHandles(out NativeMethods.IconHandle largeIconHandle, out NativeMethods.IconHandle smallIconHandle)
		{
			largeIconHandle = null;
			smallIconHandle = null;
			SecurityHelper.DemandUIWindowPermission();
			string moduleFileName = UnsafeNativeMethods.GetModuleFileName(default(HandleRef));
			int num = UnsafeNativeMethods.ExtractIconEx(moduleFileName, 0, out largeIconHandle, out smallIconHandle, 1);
		}

		// Token: 0x06007910 RID: 30992 RVA: 0x00226AFA File Offset: 0x00224CFA
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public static void GetIconHandlesFromImageSource(ImageSource image, out NativeMethods.IconHandle largeIconHandle, out NativeMethods.IconHandle smallIconHandle)
		{
			IconHelper.EnsureSystemMetrics();
			largeIconHandle = IconHelper.CreateIconHandleFromImageSource(image, IconHelper.s_iconSize);
			smallIconHandle = IconHelper.CreateIconHandleFromImageSource(image, IconHelper.s_smallIconSize);
		}

		// Token: 0x06007911 RID: 30993 RVA: 0x00226B1C File Offset: 0x00224D1C
		[SecurityCritical]
		public static NativeMethods.IconHandle CreateIconHandleFromImageSource(ImageSource image, Size size)
		{
			IconHelper.EnsureSystemMetrics();
			bool flag = false;
			BitmapFrame bitmapFrame = image as BitmapFrame;
			bool flag2;
			if (bitmapFrame == null)
			{
				flag2 = (null != null);
			}
			else
			{
				BitmapDecoder decoder = bitmapFrame.Decoder;
				flag2 = (((decoder != null) ? decoder.Frames : null) != null);
			}
			if (flag2)
			{
				bitmapFrame = IconHelper.GetBestMatch(bitmapFrame.Decoder.Frames, size);
				flag = (bitmapFrame.Decoder is IconBitmapDecoder || ((double)bitmapFrame.PixelWidth == size.Width && (double)bitmapFrame.PixelHeight == size.Height));
				image = bitmapFrame;
			}
			if (!flag)
			{
				bitmapFrame = BitmapFrame.Create(IconHelper.GenerateBitmapSource(image, size));
			}
			return IconHelper.CreateIconHandleFromBitmapFrame(bitmapFrame);
		}

		// Token: 0x06007912 RID: 30994 RVA: 0x00226BB0 File Offset: 0x00224DB0
		private static BitmapSource GenerateBitmapSource(ImageSource img, Size renderSize)
		{
			Rect rectangle = new Rect(0.0, 0.0, renderSize.Width, renderSize.Height);
			double num = renderSize.Width / renderSize.Height;
			double num2 = img.Width / img.Height;
			if (img.Width <= renderSize.Width && img.Height <= renderSize.Height)
			{
				rectangle = new Rect((renderSize.Width - img.Width) / 2.0, (renderSize.Height - img.Height) / 2.0, img.Width, img.Height);
			}
			else if (num > num2)
			{
				double num3 = img.Width / img.Height * renderSize.Width;
				rectangle = new Rect((renderSize.Width - num3) / 2.0, 0.0, num3, renderSize.Height);
			}
			else if (num < num2)
			{
				double num4 = img.Height / img.Width * renderSize.Height;
				rectangle = new Rect(0.0, (renderSize.Height - num4) / 2.0, renderSize.Width, num4);
			}
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawImage(img, rectangle);
			drawingContext.Close();
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)renderSize.Width, (int)renderSize.Height, 96.0, 96.0, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(drawingVisual);
			return renderTargetBitmap;
		}

		// Token: 0x06007913 RID: 30995 RVA: 0x00226D50 File Offset: 0x00224F50
		[SecurityCritical]
		private static NativeMethods.IconHandle CreateIconHandleFromBitmapFrame(BitmapFrame sourceBitmapFrame)
		{
			Invariant.Assert(sourceBitmapFrame != null, "sourceBitmapFrame cannot be null here");
			BitmapSource bitmapSource = sourceBitmapFrame;
			if (bitmapSource.Format != PixelFormats.Bgra32 && bitmapSource.Format != PixelFormats.Pbgra32)
			{
				bitmapSource = new FormatConvertedBitmap(bitmapSource, PixelFormats.Bgra32, null, 0.0);
			}
			int pixelWidth = bitmapSource.PixelWidth;
			int pixelHeight = bitmapSource.PixelHeight;
			int bitsPerPixel = bitmapSource.Format.BitsPerPixel;
			int num = (bitsPerPixel * pixelWidth + 31) / 32 * 4;
			int num2 = num * pixelHeight;
			byte[] array = new byte[num2];
			bitmapSource.CopyPixels(array, num, 0);
			return IconHelper.CreateIconCursor(array, pixelWidth, pixelHeight, 0, 0, true);
		}

		// Token: 0x06007914 RID: 30996 RVA: 0x00226DF8 File Offset: 0x00224FF8
		[SecurityCritical]
		internal static NativeMethods.IconHandle CreateIconCursor(byte[] colorArray, int width, int height, int xHotspot, int yHotspot, bool isIcon)
		{
			NativeMethods.BitmapHandle bitmapHandle = null;
			NativeMethods.BitmapHandle bitmapHandle2 = null;
			NativeMethods.IconHandle result;
			try
			{
				NativeMethods.BITMAPINFO bitmapinfo = new NativeMethods.BITMAPINFO(width, -height, 32);
				bitmapinfo.bmiHeader_biCompression = 0;
				IntPtr zero = IntPtr.Zero;
				bitmapHandle = UnsafeNativeMethods.CreateDIBSection(new HandleRef(null, IntPtr.Zero), ref bitmapinfo, 0, ref zero, null, 0);
				if (bitmapHandle.IsInvalid || zero == IntPtr.Zero)
				{
					result = NativeMethods.IconHandle.GetInvalidIcon();
				}
				else
				{
					Marshal.Copy(colorArray, 0, zero, colorArray.Length);
					byte[] array = IconHelper.GenerateMaskArray(width, height, colorArray);
					Invariant.Assert(array != null);
					bitmapHandle2 = UnsafeNativeMethods.CreateBitmap(width, height, 1, 1, array);
					if (bitmapHandle2.IsInvalid)
					{
						result = NativeMethods.IconHandle.GetInvalidIcon();
					}
					else
					{
						result = UnsafeNativeMethods.CreateIconIndirect(new NativeMethods.ICONINFO
						{
							fIcon = isIcon,
							xHotspot = xHotspot,
							yHotspot = yHotspot,
							hbmMask = bitmapHandle2,
							hbmColor = bitmapHandle
						});
					}
				}
			}
			finally
			{
				if (bitmapHandle != null)
				{
					bitmapHandle.Dispose();
					bitmapHandle = null;
				}
				if (bitmapHandle2 != null)
				{
					bitmapHandle2.Dispose();
					bitmapHandle2 = null;
				}
			}
			return result;
		}

		// Token: 0x06007915 RID: 30997 RVA: 0x00226F00 File Offset: 0x00225100
		private static byte[] GenerateMaskArray(int width, int height, byte[] colorArray)
		{
			int num = width * height;
			int num2 = IconHelper.AlignToBytes((double)width, 2) / 8;
			byte[] array = new byte[num2 * height];
			for (int i = 0; i < num; i++)
			{
				int num3 = i % width;
				int num4 = i / width;
				int num5 = num3 / 8;
				byte b = (byte)(128 >> num3 % 8);
				if (colorArray[i * 4 + 3] == 0)
				{
					byte[] array2 = array;
					int num6 = num5 + num2 * num4;
					array2[num6] |= b;
				}
				else
				{
					byte[] array3 = array;
					int num7 = num5 + num2 * num4;
					array3[num7] &= ~b;
				}
				if (num3 == width - 1 && width == 8)
				{
					array[1 + num2 * num4] = byte.MaxValue;
				}
			}
			return array;
		}

		// Token: 0x06007916 RID: 30998 RVA: 0x00226FA0 File Offset: 0x002251A0
		internal static int AlignToBytes(double original, int nBytesCount)
		{
			int num = 8 << nBytesCount - 1;
			return ((int)Math.Ceiling(original) + (num - 1)) / num * num;
		}

		// Token: 0x06007917 RID: 30999 RVA: 0x00226FC8 File Offset: 0x002251C8
		private static int MatchImage(BitmapFrame frame, Size size, int bpp)
		{
			return 2 * IconHelper.MyAbs(bpp, IconHelper.s_systemBitDepth, false) + IconHelper.MyAbs(frame.PixelWidth, (int)size.Width, true) + IconHelper.MyAbs(frame.PixelHeight, (int)size.Height, true);
		}

		// Token: 0x06007918 RID: 31000 RVA: 0x00227010 File Offset: 0x00225210
		private static int MyAbs(int valueHave, int valueWant, bool fPunish)
		{
			int num = valueHave - valueWant;
			if (num < 0)
			{
				num = (fPunish ? -2 : -1) * num;
			}
			return num;
		}

		// Token: 0x06007919 RID: 31001 RVA: 0x00227034 File Offset: 0x00225234
		private static BitmapFrame GetBestMatch(ReadOnlyCollection<BitmapFrame> frames, Size size)
		{
			Invariant.Assert(size.Width != 0.0, "input param width should not be zero");
			Invariant.Assert(size.Height != 0.0, "input param height should not be zero");
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
				int num5 = IconHelper.MatchImage(frames[num3], size, num4);
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

		// Token: 0x0400396B RID: 14699
		private static Size s_smallIconSize;

		// Token: 0x0400396C RID: 14700
		private static Size s_iconSize;

		// Token: 0x0400396D RID: 14701
		private static int s_systemBitDepth;
	}
}
