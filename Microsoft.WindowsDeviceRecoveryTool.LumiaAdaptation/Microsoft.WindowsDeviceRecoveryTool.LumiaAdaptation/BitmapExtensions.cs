using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation
{
	// Token: 0x02000002 RID: 2
	public static class BitmapExtensions
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
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
