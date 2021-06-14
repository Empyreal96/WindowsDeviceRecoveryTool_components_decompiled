using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200003E RID: 62
	[Flags]
	public enum OPEN_VIRTUAL_DISK_FLAG
	{
		// Token: 0x040000F9 RID: 249
		OPEN_VIRTUAL_DISK_FLAG_NONE = 0,
		// Token: 0x040000FA RID: 250
		OPEN_VIRTUAL_DISK_FLAG_NO_PARENTS = 1,
		// Token: 0x040000FB RID: 251
		OPEN_VIRTUAL_DISK_FLAG_BLANK_FILE = 2,
		// Token: 0x040000FC RID: 252
		OPEN_VIRTUAL_DISK_FLAG_BOOT_DRIVE = 4
	}
}
