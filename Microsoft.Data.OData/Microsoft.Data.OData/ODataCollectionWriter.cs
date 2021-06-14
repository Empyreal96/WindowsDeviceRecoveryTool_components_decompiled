using System;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000183 RID: 387
	public abstract class ODataCollectionWriter
	{
		// Token: 0x06000ADE RID: 2782
		public abstract void WriteStart(ODataCollectionStart collectionStart);

		// Token: 0x06000ADF RID: 2783
		public abstract Task WriteStartAsync(ODataCollectionStart collectionStart);

		// Token: 0x06000AE0 RID: 2784
		public abstract void WriteItem(object item);

		// Token: 0x06000AE1 RID: 2785
		public abstract Task WriteItemAsync(object item);

		// Token: 0x06000AE2 RID: 2786
		public abstract void WriteEnd();

		// Token: 0x06000AE3 RID: 2787
		public abstract Task WriteEndAsync();

		// Token: 0x06000AE4 RID: 2788
		public abstract void Flush();

		// Token: 0x06000AE5 RID: 2789
		public abstract Task FlushAsync();
	}
}
