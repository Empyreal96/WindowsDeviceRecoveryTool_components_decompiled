using System;

namespace Nokia.Mira.Chunks
{
	// Token: 0x02000009 RID: 9
	internal interface IChunkInformationProvider
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000017 RID: 23
		ChunkInformation Current { get; }

		// Token: 0x06000018 RID: 24
		void MoveNext();

		// Token: 0x06000019 RID: 25
		void MoveBack();
	}
}
