using System;

namespace Microsoft.Data.Edm.Internal
{
	// Token: 0x020001C8 RID: 456
	internal static class TupleInternal
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x0001FC69 File Offset: 0x0001DE69
		public static TupleInternal<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new TupleInternal<T1, T2>(item1, item2);
		}
	}
}
