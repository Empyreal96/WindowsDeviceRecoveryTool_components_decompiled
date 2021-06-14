using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Nokia.Mira.IO;

namespace Nokia.Mira.Extensions
{
	// Token: 0x02000034 RID: 52
	public static class WebResponseExtensionMethods
	{
		// Token: 0x060000DB RID: 219 RVA: 0x000045E8 File Offset: 0x000027E8
		internal static void DownloadResponseStream(this WebResponse webResponse, long filePosition, CancellationToken token, IFileStreamFactory fileStreamFactory, Action<long> reportBytesWritten)
		{
			long num = 0L;
			long num2 = 0L;
			long contentLength = webResponse.ContentLength;
			using (Stream responseStream = webResponse.GetResponseStream())
			{
				using (IFileStream fileStream = fileStreamFactory.Create(filePosition))
				{
					byte[] buffer = new byte[2048];
					for (;;)
					{
						token.ThrowIfCancellationRequested();
						int num3 = responseStream.Read(buffer, 0, 2048);
						if (num3 == 0)
						{
							break;
						}
						fileStream.Write(buffer, 0, num3);
						num += (long)num3;
						num2 += (long)num3;
						if (num >= 524288L)
						{
							fileStream.Flush();
							reportBytesWritten(num);
							num = 0L;
						}
					}
					fileStream.Flush();
					reportBytesWritten(num);
				}
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000046B0 File Offset: 0x000028B0
		internal static void DownloadResponseStream(this WebResponse webResponse, CancellationToken token, IFileStreamFactory fileStreamFactory, Action<long> reportBytesWritten)
		{
			webResponse.DownloadResponseStream(0L, token, fileStreamFactory, reportBytesWritten);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000046CA File Offset: 0x000028CA
		public static bool IsContentRangeSupported(this WebResponse webResponse)
		{
			return webResponse.Headers.Keys.Cast<string>().Any((string k) => k == "Content-Range");
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004700 File Offset: 0x00002900
		public static bool TryGetLengthFromRange(this WebResponse response, out long contentLength)
		{
			contentLength = 0L;
			if (!response.IsContentRangeSupported())
			{
				return false;
			}
			string[] values = response.Headers.GetValues("Content-Range");
			if (values == null)
			{
				return false;
			}
			string text = values.FirstOrDefault<string>();
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			string s = text.Split(new char[]
			{
				'/'
			}).LastOrDefault<string>();
			bool result;
			try
			{
				long.TryParse(s, out contentLength);
				if (contentLength < 0L)
				{
					throw new InvalidOperationException("contentLength < 0");
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004794 File Offset: 0x00002994
		public static bool TryGetContentLength(this WebResponse response, out long contentLength)
		{
			contentLength = response.ContentLength;
			return response.ContentLength > 0L;
		}

		// Token: 0x04000073 RID: 115
		private const int BufferSize = 2048;

		// Token: 0x04000074 RID: 116
		private const long FlushInterval = 524288L;
	}
}
