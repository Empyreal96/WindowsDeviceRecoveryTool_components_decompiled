using System;
using Nokia.Mira.Primitives;

namespace Nokia.Mira
{
	// Token: 0x02000014 RID: 20
	public interface IDownloadPool
	{
		// Token: 0x0600003F RID: 63
		void QueueDownloadTask(DownloadTask task);
	}
}
