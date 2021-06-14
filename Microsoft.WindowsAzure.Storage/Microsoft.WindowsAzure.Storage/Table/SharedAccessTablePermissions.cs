using System;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000142 RID: 322
	[Flags]
	public enum SharedAccessTablePermissions
	{
		// Token: 0x040007FB RID: 2043
		None = 0,
		// Token: 0x040007FC RID: 2044
		Query = 1,
		// Token: 0x040007FD RID: 2045
		Add = 2,
		// Token: 0x040007FE RID: 2046
		Update = 4,
		// Token: 0x040007FF RID: 2047
		Delete = 8
	}
}
