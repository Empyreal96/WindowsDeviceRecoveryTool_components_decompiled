using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Enums
{
	// Token: 0x02000029 RID: 41
	public enum SwVersionComparisonResult
	{
		// Token: 0x040000F9 RID: 249
		UnableToCompare,
		// Token: 0x040000FA RID: 250
		FirstIsGreater,
		// Token: 0x040000FB RID: 251
		SecondIsGreater,
		// Token: 0x040000FC RID: 252
		NumbersAreEqual,
		// Token: 0x040000FD RID: 253
		PackageNotFound
	}
}
