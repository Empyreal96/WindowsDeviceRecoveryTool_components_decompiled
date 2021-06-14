using System;

namespace ClickerUtilityLibrary.DataModel
{
	// Token: 0x02000010 RID: 16
	public class CMD_CODE
	{
		// Token: 0x04000073 RID: 115
		public const byte CMD_GET_BL_INFO = 0;

		// Token: 0x04000074 RID: 116
		public const byte CMD_ERASE_FLASH = 1;

		// Token: 0x04000075 RID: 117
		public const byte CMD_WRITE_FW_CONFIG = 2;

		// Token: 0x04000076 RID: 118
		public const byte CMD_READ_FW_CONFIG = 3;

		// Token: 0x04000077 RID: 119
		public const byte CMD_DOWNLOAD_FW = 4;

		// Token: 0x04000078 RID: 120
		public const byte CMD_READ_MEMORY = 5;

		// Token: 0x04000079 RID: 121
		public const byte CMD_RUN_APP = 6;

		// Token: 0x0400007A RID: 122
		public const byte CMD_DUMP_FLASH = 7;

		// Token: 0x0400007B RID: 123
		public const byte CMD_CALC_IMAGE_CHECKSUM = 8;

		// Token: 0x0400007C RID: 124
		public const byte CMD_RESET = 9;

		// Token: 0x0400007D RID: 125
		public const byte CMD_GET_HWID = 10;
	}
}
