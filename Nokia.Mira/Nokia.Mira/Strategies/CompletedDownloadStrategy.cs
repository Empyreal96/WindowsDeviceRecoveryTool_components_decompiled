using System;
using System.Threading.Tasks;
using Nokia.Mira.Primitives;

namespace Nokia.Mira.Strategies
{
	// Token: 0x0200002D RID: 45
	internal class CompletedDownloadStrategy : IDownloadStrategy
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00003A70 File Offset: 0x00001C70
		public ChunkStatus GetNextChunk(out Lazy<Task> chunk)
		{
			TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
			completionSource.SetResult(null);
			chunk = new Lazy<Task>(() => completionSource.Task);
			return ChunkStatus.RanToCompletion;
		}
	}
}
