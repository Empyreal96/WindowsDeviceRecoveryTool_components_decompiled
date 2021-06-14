using System;
using System.Windows;
using System.Windows.Media;

namespace Standard
{
	// Token: 0x02000007 RID: 7
	internal static class DpiHelper
	{
		// Token: 0x06000027 RID: 39 RVA: 0x000023C3 File Offset: 0x000005C3
		public static Point LogicalPixelsToDevice(Point logicalPoint, double dpiScaleX, double dpiScaleY)
		{
			DpiHelper._transformToDevice = Matrix.Identity;
			DpiHelper._transformToDevice.Scale(dpiScaleX, dpiScaleY);
			return DpiHelper._transformToDevice.Transform(logicalPoint);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000023E6 File Offset: 0x000005E6
		public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
		{
			DpiHelper._transformToDip = Matrix.Identity;
			DpiHelper._transformToDip.Scale(1.0 / dpiScaleX, 1.0 / dpiScaleY);
			return DpiHelper._transformToDip.Transform(devicePoint);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002420 File Offset: 0x00000620
		public static Rect LogicalRectToDevice(Rect logicalRectangle, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top), dpiScaleX, dpiScaleY);
			Point point2 = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom), dpiScaleX, dpiScaleY);
			return new Rect(point, point2);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000246C File Offset: 0x0000066C
		public static Rect DeviceRectToLogical(Rect deviceRectangle, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top), dpiScaleX, dpiScaleY);
			Point point2 = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom), dpiScaleX, dpiScaleY);
			return new Rect(point, point2);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000024B8 File Offset: 0x000006B8
		public static Size LogicalSizeToDevice(Size logicalSize, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height), dpiScaleX, dpiScaleY);
			return new Size
			{
				Width = point.X,
				Height = point.Y
			};
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002508 File Offset: 0x00000708
		public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height), dpiScaleX, dpiScaleY);
			return new Size(point.X, point.Y);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002544 File Offset: 0x00000744
		public static Thickness LogicalThicknessToDevice(Thickness logicalThickness, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalThickness.Left, logicalThickness.Top), dpiScaleX, dpiScaleY);
			Point point2 = DpiHelper.LogicalPixelsToDevice(new Point(logicalThickness.Right, logicalThickness.Bottom), dpiScaleX, dpiScaleY);
			return new Thickness(point.X, point.Y, point2.X, point2.Y);
		}

		// Token: 0x04000021 RID: 33
		[ThreadStatic]
		private static Matrix _transformToDevice;

		// Token: 0x04000022 RID: 34
		[ThreadStatic]
		private static Matrix _transformToDip;
	}
}
