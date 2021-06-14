using System;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000157 RID: 343
	public abstract class ODataReader
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000945 RID: 2373
		public abstract ODataReaderState State { get; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000946 RID: 2374
		public abstract ODataItem Item { get; }

		// Token: 0x06000947 RID: 2375
		public abstract bool Read();

		// Token: 0x06000948 RID: 2376
		public abstract Task<bool> ReadAsync();
	}
}
