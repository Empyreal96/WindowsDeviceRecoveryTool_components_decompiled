using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000AA RID: 170
	internal class Adler32
	{
		// Token: 0x0600079C RID: 1948 RVA: 0x0002D370 File Offset: 0x0002C370
		internal static long GetAdler32Checksum(long adler, byte[] buf, int index, int len)
		{
			if (buf == null)
			{
				return 1L;
			}
			long num = adler & 65535L;
			long num2 = adler >> 16 & 65535L;
			while (len > 0)
			{
				int i = (len < 5552) ? len : 5552;
				len -= i;
				while (i >= 16)
				{
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					i -= 16;
				}
				if (i != 0)
				{
					do
					{
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
					}
					while (--i != 0);
				}
				num %= 65521L;
				num2 %= 65521L;
			}
			return num2 << 16 | num;
		}

		// Token: 0x04000435 RID: 1077
		private const int BASE = 65521;

		// Token: 0x04000436 RID: 1078
		private const int NMAX = 5552;
	}
}
