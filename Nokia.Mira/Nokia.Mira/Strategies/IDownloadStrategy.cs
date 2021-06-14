using System;
using System.Threading.Tasks;
using Nokia.Mira.Primitives;

namespace Nokia.Mira.Strategies
{
	// Token: 0x02000028 RID: 40
	internal interface IDownloadStrategy
	{
		// Token: 0x060000A9 RID: 169
		ChunkStatus GetNextChunk(out Lazy<Task> chunk);
	}
}
