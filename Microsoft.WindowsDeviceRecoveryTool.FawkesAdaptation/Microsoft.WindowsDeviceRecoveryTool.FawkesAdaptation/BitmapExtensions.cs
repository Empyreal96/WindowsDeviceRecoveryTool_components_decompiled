using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Microsoft.WindowsDeviceRecoveryTool.FawkesAdaptation
{
	// Token: 0x02000003 RID: 3
	public static class BitmapExtensions
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000021B8 File Offset: 0x000003B8
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
