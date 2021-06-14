using System;

namespace MS.Internal.Interop
{
	// Token: 0x02000679 RID: 1657
	internal struct STAT_CHUNK
	{
		// Token: 0x040035C1 RID: 13761
		internal uint idChunk;

		// Token: 0x040035C2 RID: 13762
		internal CHUNK_BREAKTYPE breakType;

		// Token: 0x040035C3 RID: 13763
		internal CHUNKSTATE flags;

		// Token: 0x040035C4 RID: 13764
		internal uint locale;

		// Token: 0x040035C5 RID: 13765
		internal FULLPROPSPEC attribute;

		// Token: 0x040035C6 RID: 13766
		internal uint idChunkSource;

		// Token: 0x040035C7 RID: 13767
		internal uint cwcStartSource;

		// Token: 0x040035C8 RID: 13768
		internal uint cwcLenSource;
	}
}
