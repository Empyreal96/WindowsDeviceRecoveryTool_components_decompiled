using System;

namespace ComponentAce.Encryption
{
	// Token: 0x0200000C RID: 12
	internal enum CipherBlockMode
	{
		// Token: 0x04000026 RID: 38
		CTS,
		// Token: 0x04000027 RID: 39
		CTR,
		// Token: 0x04000028 RID: 40
		CBC,
		// Token: 0x04000029 RID: 41
		CFB,
		// Token: 0x0400002A RID: 42
		OFB,
		// Token: 0x0400002B RID: 43
		ECB,
		// Token: 0x0400002C RID: 44
		CTSMAC,
		// Token: 0x0400002D RID: 45
		CBCMAC,
		// Token: 0x0400002E RID: 46
		CFBMAC
	}
}
