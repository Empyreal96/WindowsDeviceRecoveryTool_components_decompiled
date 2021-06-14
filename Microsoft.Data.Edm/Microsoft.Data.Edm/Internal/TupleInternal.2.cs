using System;

namespace Microsoft.Data.Edm.Internal
{
	// Token: 0x020001C9 RID: 457
	internal class TupleInternal<T1, T2>
	{
		// Token: 0x06000AC3 RID: 2755 RVA: 0x0001FC72 File Offset: 0x0001DE72
		public TupleInternal(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0001FC88 File Offset: 0x0001DE88
		// (set) Token: 0x06000AC5 RID: 2757 RVA: 0x0001FC90 File Offset: 0x0001DE90
		public T1 Item1 { get; private set; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0001FC99 File Offset: 0x0001DE99
		// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x0001FCA1 File Offset: 0x0001DEA1
		public T2 Item2 { get; private set; }
	}
}
