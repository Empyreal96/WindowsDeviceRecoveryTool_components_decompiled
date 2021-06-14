using System;
using System.Diagnostics;

namespace Microsoft.Data.OData
{
	// Token: 0x02000273 RID: 627
	internal static class DebugUtils
	{
		// Token: 0x060014CF RID: 5327 RVA: 0x0004D5C9 File Offset: 0x0004B7C9
		[Conditional("DEBUG")]
		internal static void CheckNoExternalCallers()
		{
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004D5CB File Offset: 0x0004B7CB
		[Conditional("DEBUG")]
		internal static void CheckNoExternalCallers(bool checkPublicMethods)
		{
		}
	}
}
