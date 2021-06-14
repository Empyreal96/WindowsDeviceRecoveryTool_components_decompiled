using System;

namespace Standard
{
	// Token: 0x02000017 RID: 23
	[Flags]
	internal enum ErrorModes
	{
		// Token: 0x040000D2 RID: 210
		Default = 0,
		// Token: 0x040000D3 RID: 211
		FailCriticalErrors = 1,
		// Token: 0x040000D4 RID: 212
		NoGpFaultErrorBox = 2,
		// Token: 0x040000D5 RID: 213
		NoAlignmentFaultExcept = 4,
		// Token: 0x040000D6 RID: 214
		NoOpenFileErrorBox = 32768
	}
}
