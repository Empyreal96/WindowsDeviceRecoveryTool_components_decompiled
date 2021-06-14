using System;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000149 RID: 329
	public abstract class ODataCollectionReader
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060008DF RID: 2271
		public abstract ODataCollectionReaderState State { get; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060008E0 RID: 2272
		public abstract object Item { get; }

		// Token: 0x060008E1 RID: 2273
		public abstract bool Read();

		// Token: 0x060008E2 RID: 2274
		public abstract Task<bool> ReadAsync();
	}
}
