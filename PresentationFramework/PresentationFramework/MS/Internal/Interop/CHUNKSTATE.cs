using System;

namespace MS.Internal.Interop
{
	// Token: 0x02000678 RID: 1656
	[Flags]
	internal enum CHUNKSTATE
	{
		// Token: 0x040035BE RID: 13758
		CHUNK_TEXT = 1,
		// Token: 0x040035BF RID: 13759
		CHUNK_VALUE = 2,
		// Token: 0x040035C0 RID: 13760
		CHUNK_FILTER_OWNED_VALUE = 4
	}
}
