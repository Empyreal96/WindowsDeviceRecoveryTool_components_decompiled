using System;
using System.Net;
using System.Threading;
using Nokia.Mira.IO;

namespace Nokia.Mira.Primitives
{
	// Token: 0x0200001C RID: 28
	internal interface IWebResponse
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006B RID: 107
		long ContentLength { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006C RID: 108
		DateTime LastModified { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006D RID: 109
		HttpStatusCode StatusCode { get; }

		// Token: 0x0600006E RID: 110
		bool IsContentRangeSupported();

		// Token: 0x0600006F RID: 111
		bool TryGetLengthFromRange(out long contentLength);

		// Token: 0x06000070 RID: 112
		bool TryGetContentLength(out long contentLength);

		// Token: 0x06000071 RID: 113
		void Close();

		// Token: 0x06000072 RID: 114
		void DownloadResponseStream(long filePosition, CancellationToken token, IFileStreamFactory fileStreamFactory, Action<long> reportBytesWritten);

		// Token: 0x06000073 RID: 115
		void DownloadResponseStream(CancellationToken token, IFileStreamFactory fileStreamFactory, Action<long> reportBytesWritten);
	}
}
