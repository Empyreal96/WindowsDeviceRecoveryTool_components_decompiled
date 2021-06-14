using System;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000E0 RID: 224
	[Flags]
	public enum SharedAccessFilePermissions
	{
		// Token: 0x040004E5 RID: 1253
		None = 0,
		// Token: 0x040004E6 RID: 1254
		Read = 1,
		// Token: 0x040004E7 RID: 1255
		Write = 2,
		// Token: 0x040004E8 RID: 1256
		Delete = 4,
		// Token: 0x040004E9 RID: 1257
		List = 8
	}
}
