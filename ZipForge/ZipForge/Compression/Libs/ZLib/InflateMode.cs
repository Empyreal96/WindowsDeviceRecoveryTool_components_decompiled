using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000B1 RID: 177
	internal enum InflateMode
	{
		// Token: 0x040004CC RID: 1228
		METHOD,
		// Token: 0x040004CD RID: 1229
		FLAG,
		// Token: 0x040004CE RID: 1230
		DICT4,
		// Token: 0x040004CF RID: 1231
		DICT3,
		// Token: 0x040004D0 RID: 1232
		DICT2,
		// Token: 0x040004D1 RID: 1233
		DICT1,
		// Token: 0x040004D2 RID: 1234
		DICT0,
		// Token: 0x040004D3 RID: 1235
		BLOCKS,
		// Token: 0x040004D4 RID: 1236
		CHECK4,
		// Token: 0x040004D5 RID: 1237
		CHECK3,
		// Token: 0x040004D6 RID: 1238
		CHECK2,
		// Token: 0x040004D7 RID: 1239
		CHECK1,
		// Token: 0x040004D8 RID: 1240
		DONE,
		// Token: 0x040004D9 RID: 1241
		BAD
	}
}
