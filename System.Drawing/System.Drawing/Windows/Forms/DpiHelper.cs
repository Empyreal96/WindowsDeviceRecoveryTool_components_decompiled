using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200000B RID: 11
	internal static class DpiHelper
	{
		// Token: 0x06000015 RID: 21 RVA: 0x0000258C File Offset: 0x0000078C
		private static void Initialize()
		{
			if (DpiHelper.isInitialized)
			{
				return;
			}
			if (DpiHelper.IsDpiAwarenessValueSet())
			{
				DpiHelper.enableHighDpi = true;
			}
			else
			{
				try
				{
					string text = ConfigurationManager.AppSettings.Get("EnableWindowsFormsHighDpiAutoResizing");
					if (!string.IsNullOrEmpty(text) && string.Equals(text, "true", StringComparison.InvariantCultureIgnoreCase))
					{
						DpiHelper.enableHighDpi = true;
					}
				}
				catch
				{
				}
			}
			if (DpiHelper.enableHighDpi)
			{
				IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
				if (dc != IntPtr.Zero)
				{
					DpiHelper.deviceDpi = (double)UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
					UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
				}
			}
			DpiHelper.isInitialized = true;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002640 File Offset: 0x00000840
		internal static bool IsDpiAwarenessValueSet()
		{
			bool result = false;
			try
			{
				if (string.IsNullOrEmpty(DpiHelper.dpiAwarenessValue))
				{
					DpiHelper.dpiAwarenessValue = ConfigurationOptions.GetConfigSettingValue("DpiAwareness");
				}
			}
			catch
			{
			}
			if (!string.IsNullOrEmpty(DpiHelper.dpiAwarenessValue))
			{
				string a = DpiHelper.dpiAwarenessValue.ToLowerInvariant();
				if (!(a == "true") && !(a == "system") && !(a == "true/pm") && !(a == "permonitor") && !(a == "permonitorv2"))
				{
					if (!(a == "false"))
					{
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000026EC File Offset: 0x000008EC
		internal static int DeviceDpi
		{
			get
			{
				DpiHelper.Initialize();
				return (int)DpiHelper.deviceDpi;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000026F9 File Offset: 0x000008F9
		private static double LogicalToDeviceUnitsScalingFactor
		{
			get
			{
				if (DpiHelper.logicalToDeviceUnitsScalingFactor == 0.0)
				{
					DpiHelper.Initialize();
					DpiHelper.logicalToDeviceUnitsScalingFactor = DpiHelper.deviceDpi / 96.0;
				}
				return DpiHelper.logicalToDeviceUnitsScalingFactor;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000272C File Offset: 0x0000092C
		private static InterpolationMode InterpolationMode
		{
			get
			{
				if (DpiHelper.interpolationMode == InterpolationMode.Invalid)
				{
					int num = (int)Math.Round(DpiHelper.LogicalToDeviceUnitsScalingFactor * 100.0);
					if (num % 100 == 0)
					{
						DpiHelper.interpolationMode = InterpolationMode.NearestNeighbor;
					}
					else if (num < 100)
					{
						DpiHelper.interpolationMode = InterpolationMode.HighQualityBilinear;
					}
					else
					{
						DpiHelper.interpolationMode = InterpolationMode.HighQualityBicubic;
					}
				}
				return DpiHelper.interpolationMode;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002780 File Offset: 0x00000980
		private static Bitmap ScaleBitmapToSize(Bitmap logicalImage, Size deviceImageSize)
		{
			Bitmap bitmap = new Bitmap(deviceImageSize.Width, deviceImageSize.Height, logicalImage.PixelFormat);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = DpiHelper.InterpolationMode;
				RectangleF srcRect = new RectangleF(0f, 0f, (float)logicalImage.Size.Width, (float)logicalImage.Size.Height);
				RectangleF destRect = new RectangleF(0f, 0f, (float)deviceImageSize.Width, (float)deviceImageSize.Height);
				srcRect.Offset(-0.5f, -0.5f);
				graphics.DrawImage(logicalImage, destRect, srcRect, GraphicsUnit.Pixel);
			}
			return bitmap;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002844 File Offset: 0x00000A44
		private static Bitmap CreateScaledBitmap(Bitmap logicalImage, int deviceDpi = 0)
		{
			Size deviceImageSize = DpiHelper.LogicalToDeviceUnits(logicalImage.Size, deviceDpi);
			return DpiHelper.ScaleBitmapToSize(logicalImage, deviceImageSize);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002865 File Offset: 0x00000A65
		public static bool IsScalingRequired
		{
			get
			{
				DpiHelper.Initialize();
				return DpiHelper.deviceDpi != 96.0;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002880 File Offset: 0x00000A80
		public static int LogicalToDeviceUnits(int value, int devicePixels = 0)
		{
			if (devicePixels == 0)
			{
				return (int)Math.Round(DpiHelper.LogicalToDeviceUnitsScalingFactor * (double)value);
			}
			double num = (double)devicePixels / 96.0;
			return (int)Math.Round(num * (double)value);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000028B8 File Offset: 0x00000AB8
		public static double LogicalToDeviceUnits(double value, int devicePixels = 0)
		{
			if (devicePixels == 0)
			{
				return DpiHelper.LogicalToDeviceUnitsScalingFactor * value;
			}
			double num = (double)devicePixels / 96.0;
			return num * value;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000028E0 File Offset: 0x00000AE0
		public static int LogicalToDeviceUnitsX(int value)
		{
			return DpiHelper.LogicalToDeviceUnits(value, 0);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028E0 File Offset: 0x00000AE0
		public static int LogicalToDeviceUnitsY(int value)
		{
			return DpiHelper.LogicalToDeviceUnits(value, 0);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028E9 File Offset: 0x00000AE9
		public static Size LogicalToDeviceUnits(Size logicalSize, int deviceDpi = 0)
		{
			return new Size(DpiHelper.LogicalToDeviceUnits(logicalSize.Width, deviceDpi), DpiHelper.LogicalToDeviceUnits(logicalSize.Height, deviceDpi));
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000290A File Offset: 0x00000B0A
		public static Bitmap CreateResizedBitmap(Bitmap logicalImage, Size targetImageSize)
		{
			if (logicalImage == null)
			{
				return null;
			}
			return DpiHelper.ScaleBitmapToSize(logicalImage, targetImageSize);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002918 File Offset: 0x00000B18
		public static void ScaleBitmapLogicalToDevice(ref Bitmap logicalBitmap, int deviceDpi = 0)
		{
			if (logicalBitmap == null)
			{
				return;
			}
			Bitmap bitmap = DpiHelper.CreateScaledBitmap(logicalBitmap, deviceDpi);
			if (bitmap != null)
			{
				logicalBitmap.Dispose();
				logicalBitmap = bitmap;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002940 File Offset: 0x00000B40
		public static int ConvertToGivenDpiPixel(int value, double pixelFactor)
		{
			int num = (int)Math.Round((double)value * pixelFactor);
			if (num != 0)
			{
				return num;
			}
			return 1;
		}

		// Token: 0x0400008F RID: 143
		internal const double LogicalDpi = 96.0;

		// Token: 0x04000090 RID: 144
		private static bool isInitialized = false;

		// Token: 0x04000091 RID: 145
		private static double deviceDpi = 96.0;

		// Token: 0x04000092 RID: 146
		private static double logicalToDeviceUnitsScalingFactor = 0.0;

		// Token: 0x04000093 RID: 147
		private static bool enableHighDpi = false;

		// Token: 0x04000094 RID: 148
		private static string dpiAwarenessValue = null;

		// Token: 0x04000095 RID: 149
		private static InterpolationMode interpolationMode = InterpolationMode.Invalid;
	}
}
