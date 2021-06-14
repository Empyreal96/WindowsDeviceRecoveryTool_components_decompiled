using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Microsoft.WindowsDeviceRecoveryTool.AlcatelAdaptation
{
	// Token: 0x02000004 RID: 4
	public static class BitmapExtensions
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000238C File Offset: 0x0000058C
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
