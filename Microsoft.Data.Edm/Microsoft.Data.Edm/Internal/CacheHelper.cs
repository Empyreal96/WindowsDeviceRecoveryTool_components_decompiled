using System;

namespace Microsoft.Data.Edm.Internal
{
	// Token: 0x020001BA RID: 442
	internal static class CacheHelper
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x0001F6F7 File Offset: 0x0001D8F7
		internal static object BoxedBool(bool value)
		{
			if (!value)
			{
				return CacheHelper.BoxedFalse;
			}
			return CacheHelper.BoxedTrue;
		}

		// Token: 0x040004E6 RID: 1254
		internal static readonly object Unknown = new object();

		// Token: 0x040004E7 RID: 1255
		internal static readonly object CycleSentinel = new object();

		// Token: 0x040004E8 RID: 1256
		internal static readonly object SecondPassCycleSentinel = new object();

		// Token: 0x040004E9 RID: 1257
		private static readonly object BoxedTrue = true;

		// Token: 0x040004EA RID: 1258
		private static readonly object BoxedFalse = false;
	}
}
