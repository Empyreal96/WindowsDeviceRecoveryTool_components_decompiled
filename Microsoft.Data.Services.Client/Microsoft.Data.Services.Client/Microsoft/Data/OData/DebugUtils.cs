using System;
using System.Diagnostics;

namespace Microsoft.Data.OData
{
	// Token: 0x02000002 RID: 2
	internal static class DebugUtils
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		[Conditional("DEBUG")]
		internal static void CheckNoExternalCallers()
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020D2 File Offset: 0x000002D2
		[Conditional("DEBUG")]
		internal static void CheckNoExternalCallers(bool checkPublicMethods)
		{
		}
	}
}
