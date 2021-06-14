using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nokia.Mira
{
	// Token: 0x02000036 RID: 54
	public static class WebFileExtensionMethods
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00004872 File Offset: 0x00002A72
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName)
		{
			return webFile.DownloadAsync(destinationFileName, CancellationToken.None, new WebFileExtensionMethods.NullProgress(), DownloadSettings.Default, DownloadPool.DefaultInstance);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000488F File Offset: 0x00002A8F
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, DownloadSettings downloadSettings)
		{
			return webFile.DownloadAsync(destinationFileName, CancellationToken.None, new WebFileExtensionMethods.NullProgress(), downloadSettings, DownloadPool.DefaultInstance);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000048A8 File Offset: 0x00002AA8
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, CancellationToken cancellationToken)
		{
			return webFile.DownloadAsync(destinationFileName, cancellationToken, new WebFileExtensionMethods.NullProgress(), DownloadSettings.Default, DownloadPool.DefaultInstance);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000048C1 File Offset: 0x00002AC1
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, CancellationToken cancellationToken, DownloadSettings downloadSettings)
		{
			return webFile.DownloadAsync(destinationFileName, cancellationToken, new WebFileExtensionMethods.NullProgress(), downloadSettings, DownloadPool.DefaultInstance);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000048D6 File Offset: 0x00002AD6
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, IProgress<DownloadProgressInfo> progress)
		{
			return webFile.DownloadAsync(destinationFileName, CancellationToken.None, progress, DownloadSettings.Default, DownloadPool.DefaultInstance);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000048EF File Offset: 0x00002AEF
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, IProgress<DownloadProgressInfo> progress, DownloadSettings downloadSettings)
		{
			return webFile.DownloadAsync(destinationFileName, CancellationToken.None, progress, downloadSettings, DownloadPool.DefaultInstance);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004904 File Offset: 0x00002B04
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, CancellationToken cancellationToken, IProgress<DownloadProgressInfo> progress)
		{
			return webFile.DownloadAsync(destinationFileName, cancellationToken, progress, DownloadSettings.Default, DownloadPool.DefaultInstance);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004919 File Offset: 0x00002B19
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, CancellationToken cancellationToken, IProgress<DownloadProgressInfo> progress, DownloadSettings downloadSettings)
		{
			return webFile.DownloadAsync(destinationFileName, cancellationToken, progress, downloadSettings, DownloadPool.DefaultInstance);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000492B File Offset: 0x00002B2B
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, IDownloadPool downloadPool)
		{
			return webFile.DownloadAsync(destinationFileName, CancellationToken.None, new WebFileExtensionMethods.NullProgress(), DownloadSettings.Default, downloadPool);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004944 File Offset: 0x00002B44
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, DownloadSettings downloadSettings, IDownloadPool downloadPool)
		{
			return webFile.DownloadAsync(destinationFileName, CancellationToken.None, new WebFileExtensionMethods.NullProgress(), downloadSettings, downloadPool);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004959 File Offset: 0x00002B59
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, CancellationToken cancellationToken, IDownloadPool downloadPool)
		{
			return webFile.DownloadAsync(destinationFileName, cancellationToken, new WebFileExtensionMethods.NullProgress(), DownloadSettings.Default, downloadPool);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000496E File Offset: 0x00002B6E
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, CancellationToken cancellationToken, DownloadSettings downloadSettings, IDownloadPool downloadPool)
		{
			return webFile.DownloadAsync(destinationFileName, cancellationToken, new WebFileExtensionMethods.NullProgress(), downloadSettings, downloadPool);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004980 File Offset: 0x00002B80
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, IProgress<DownloadProgressInfo> progress, IDownloadPool downloadPool)
		{
			return webFile.DownloadAsync(destinationFileName, CancellationToken.None, progress, DownloadSettings.Default, downloadPool);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004995 File Offset: 0x00002B95
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, IProgress<DownloadProgressInfo> progress, DownloadSettings downloadSettings, IDownloadPool downloadPool)
		{
			return webFile.DownloadAsync(destinationFileName, CancellationToken.None, progress, downloadSettings, downloadPool);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000049A7 File Offset: 0x00002BA7
		public static Task DownloadAsync(this IWebFile webFile, string destinationFileName, CancellationToken cancellationToken, IProgress<DownloadProgressInfo> progress, IDownloadPool downloadPool)
		{
			return webFile.DownloadAsync(destinationFileName, cancellationToken, progress, DownloadSettings.Default, downloadPool);
		}

		// Token: 0x02000037 RID: 55
		private class NullProgress : IProgress<DownloadProgressInfo>
		{
			// Token: 0x060000F4 RID: 244 RVA: 0x000049B9 File Offset: 0x00002BB9
			public void Report(DownloadProgressInfo value)
			{
			}
		}
	}
}
