using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nokia.Mira.Primitives;
using Nokia.Mira.Strategies;

namespace Nokia.Mira
{
	// Token: 0x02000035 RID: 53
	public sealed class WebFile : IWebFile
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x000047A8 File Offset: 0x000029A8
		public WebFile(Uri uri) : this(new HttpWebRequestFactory(uri))
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000047B6 File Offset: 0x000029B6
		public WebFile(IHttpWebRequestFactory httpWebRequestFactory)
		{
			if (httpWebRequestFactory == null)
			{
				throw new ArgumentNullException("httpWebRequestFactory");
			}
			this.httpWebRequestFactory = httpWebRequestFactory;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000047D4 File Offset: 0x000029D4
		public Task DownloadAsync(string destinationFileName, CancellationToken cancellationToken, IProgress<DownloadProgressInfo> progress, DownloadSettings downloadSettings, IDownloadPool downloadPool)
		{
			WebFile.ValidateFileName(destinationFileName);
			DownloadTask downloadTask = new DownloadTask(this.httpWebRequestFactory, destinationFileName, cancellationToken, progress, downloadSettings, new DownloadStrategyFactory(), downloadPool);
			return downloadTask.StartAsync();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004808 File Offset: 0x00002A08
		private static void ValidateFileName(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			string fileName2 = Path.GetFileName(fileName);
			if (fileName == string.Empty || Path.GetInvalidFileNameChars().Any(new Func<char, bool>(fileName2.Contains<char>)) || Path.GetInvalidPathChars().Any(new Func<char, bool>(fileName.Contains<char>)))
			{
				throw new ArgumentException("fileName");
			}
		}

		// Token: 0x04000076 RID: 118
		private readonly IHttpWebRequestFactory httpWebRequestFactory;
	}
}
