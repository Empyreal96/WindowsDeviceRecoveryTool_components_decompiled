using System;

namespace ComponentAce.Compression.GZip
{
	// Token: 0x0200003B RID: 59
	internal class GzipConsts
	{
		// Token: 0x06000230 RID: 560 RVA: 0x000168F0 File Offset: 0x000158F0
		private GzipConsts()
		{
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000168F8 File Offset: 0x000158F8
		public static GzipConsts GetInstance()
		{
			if (GzipConsts._instance == null)
			{
				GzipConsts._instance = new GzipConsts();
			}
			return GzipConsts._instance;
		}

		// Token: 0x040001A4 RID: 420
		public const int TextFlagBitNumber = 0;

		// Token: 0x040001A5 RID: 421
		public const int CrcFlagBitNumber = 1;

		// Token: 0x040001A6 RID: 422
		public const int ExtraFieldFlagBitNumber = 2;

		// Token: 0x040001A7 RID: 423
		public const int NameFlagBitNumber = 3;

		// Token: 0x040001A8 RID: 424
		public const int CommentFlagBitNumber = 4;

		// Token: 0x040001A9 RID: 425
		public const int DefaultMaxBlockSize = 1048576;

		// Token: 0x040001AA RID: 426
		public const int BlockSizeForMax = 1572864;

		// Token: 0x040001AB RID: 427
		public const int BlockSizeForFastest = 524288;

		// Token: 0x040001AC RID: 428
		public const byte DeflateCompressionMode = 8;

		// Token: 0x040001AD RID: 429
		public const byte DefaultExtraFlag = 4;

		// Token: 0x040001AE RID: 430
		public const byte DefaultId1 = 31;

		// Token: 0x040001AF RID: 431
		public const byte DefaultId2 = 139;

		// Token: 0x040001B0 RID: 432
		private static GzipConsts _instance;
	}
}
