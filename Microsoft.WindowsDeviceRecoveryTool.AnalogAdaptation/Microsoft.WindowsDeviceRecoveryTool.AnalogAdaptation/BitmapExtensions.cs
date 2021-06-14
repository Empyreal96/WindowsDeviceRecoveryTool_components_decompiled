using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Microsoft.WindowsDeviceRecoveryTool.AnalogAdaptation
{
	// Token: 0x02000003 RID: 3
	public static class BitmapExtensions
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000024F0 File Offset: 0x000006F0
		public static byte[] ToBytes(this Bitmap bitmap)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bitmap.Save(memoryStream, ImageFormat.Png);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				result = memoryStream.ToArray();
			}
			return result;
		}
	}
}
