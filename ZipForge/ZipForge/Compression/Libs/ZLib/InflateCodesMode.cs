using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000AF RID: 175
	internal enum InflateCodesMode
	{
		// Token: 0x040004B3 RID: 1203
		START,
		// Token: 0x040004B4 RID: 1204
		LEN,
		// Token: 0x040004B5 RID: 1205
		LENEXT,
		// Token: 0x040004B6 RID: 1206
		DIST,
		// Token: 0x040004B7 RID: 1207
		DISTEXT,
		// Token: 0x040004B8 RID: 1208
		COPY,
		// Token: 0x040004B9 RID: 1209
		LIT,
		// Token: 0x040004BA RID: 1210
		WASH,
		// Token: 0x040004BB RID: 1211
		END,
		// Token: 0x040004BC RID: 1212
		BADCODE
	}
}
