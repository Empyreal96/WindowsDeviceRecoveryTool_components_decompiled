using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000BB RID: 187
	public enum ZLibResultCode
	{
		// Token: 0x04000515 RID: 1301
		Z_OK,
		// Token: 0x04000516 RID: 1302
		Z_STREAM_END,
		// Token: 0x04000517 RID: 1303
		Z_NEED_DICT,
		// Token: 0x04000518 RID: 1304
		Z_ERRNO = -1,
		// Token: 0x04000519 RID: 1305
		Z_STREAM_ERROR = -2,
		// Token: 0x0400051A RID: 1306
		Z_DATA_ERROR = -3,
		// Token: 0x0400051B RID: 1307
		Z_MEM_ERROR = -4,
		// Token: 0x0400051C RID: 1308
		Z_BUF_ERROR = -5,
		// Token: 0x0400051D RID: 1309
		Z_VERSION_ERROR = -6
	}
}
