using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nokia.Mira
{
	// Token: 0x02000033 RID: 51
	public interface IWebFile
	{
		// Token: 0x060000DA RID: 218
		Task DownloadAsync(string destinationFileName, CancellationToken cancellationToken, IProgress<DownloadProgressInfo> progress, DownloadSettings downloadSettings, IDownloadPool downloadPool);
	}
}
