﻿using System;

namespace System.Windows.Forms
{
	// Token: 0x0200000C RID: 12
	internal enum DpiAwarenessContext
	{
		// Token: 0x04000097 RID: 151
		DPI_AWARENESS_CONTEXT_UNSPECIFIED,
		// Token: 0x04000098 RID: 152
		DPI_AWARENESS_CONTEXT_UNAWARE = -1,
		// Token: 0x04000099 RID: 153
		DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = -2,
		// Token: 0x0400009A RID: 154
		DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = -3,
		// Token: 0x0400009B RID: 155
		DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4
	}
}