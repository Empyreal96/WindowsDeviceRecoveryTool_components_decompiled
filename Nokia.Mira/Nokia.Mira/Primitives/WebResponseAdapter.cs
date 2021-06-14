using System;
using System.Net;
using System.Threading;
using Nokia.Mira.Extensions;
using Nokia.Mira.IO;

namespace Nokia.Mira.Primitives
{
	// Token: 0x0200001E RID: 30
	internal class WebResponseAdapter : IWebResponse
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00002E00 File Offset: 0x00001000
		public WebResponseAdapter(HttpWebResponse webResponse)
		{
			this.webResponse = webResponse;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002E0F File Offset: 0x0000100F
		public long ContentLength
		{
			get
			{
				return this.webResponse.ContentLength;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002E1C File Offset: 0x0000101C
		public DateTime LastModified
		{
			get
			{
				return this.webResponse.LastModified;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002E29 File Offset: 0x00001029
		public HttpStatusCode StatusCode
		{
			get
			{
				return this.webResponse.StatusCode;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002E36 File Offset: 0x00001036
		public bool IsContentRangeSupported()
		{
			return this.webResponse.IsContentRangeSupported();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002E43 File Offset: 0x00001043
		public bool TryGetLengthFromRange(out long contentLength)
		{
			return this.webResponse.TryGetLengthFromRange(out contentLength);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002E51 File Offset: 0x00001051
		public bool TryGetContentLength(out long contentLength)
		{
			return this.webResponse.TryGetContentLength(out contentLength);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002E5F File Offset: 0x0000105F
		public void Close()
		{
			this.webResponse.Close();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002E6C File Offset: 0x0000106C
		public void DownloadResponseStream(long filePosition, CancellationToken token, IFileStreamFactory fileStreamFactory, Action<long> reportBytesWritten)
		{
			this.webResponse.DownloadResponseStream(filePosition, token, fileStreamFactory, reportBytesWritten);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002E7E File Offset: 0x0000107E
		public void DownloadResponseStream(CancellationToken token, IFileStreamFactory fileStreamFactory, Action<long> reportBytesWritten)
		{
			this.webResponse.DownloadResponseStream(token, fileStreamFactory, reportBytesWritten);
		}

		// Token: 0x0400003B RID: 59
		private readonly HttpWebResponse webResponse;
	}
}
