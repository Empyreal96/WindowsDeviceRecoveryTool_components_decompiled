using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000AD RID: 173
	internal enum InflateBlockMode
	{
		// Token: 0x04000495 RID: 1173
		TYPE,
		// Token: 0x04000496 RID: 1174
		LENS,
		// Token: 0x04000497 RID: 1175
		STORED,
		// Token: 0x04000498 RID: 1176
		TABLE,
		// Token: 0x04000499 RID: 1177
		BTREE,
		// Token: 0x0400049A RID: 1178
		DTREE,
		// Token: 0x0400049B RID: 1179
		CODES,
		// Token: 0x0400049C RID: 1180
		DRY,
		// Token: 0x0400049D RID: 1181
		DONE,
		// Token: 0x0400049E RID: 1182
		BAD
	}
}
