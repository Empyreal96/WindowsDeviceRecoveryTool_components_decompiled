using System;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x020001A2 RID: 418
	public abstract class ODataWriter
	{
		// Token: 0x06000CB1 RID: 3249
		public abstract void WriteStart(ODataFeed feed);

		// Token: 0x06000CB2 RID: 3250
		public abstract Task WriteStartAsync(ODataFeed feed);

		// Token: 0x06000CB3 RID: 3251
		public abstract void WriteStart(ODataEntry entry);

		// Token: 0x06000CB4 RID: 3252
		public abstract Task WriteStartAsync(ODataEntry entry);

		// Token: 0x06000CB5 RID: 3253
		public abstract void WriteStart(ODataNavigationLink navigationLink);

		// Token: 0x06000CB6 RID: 3254
		public abstract Task WriteStartAsync(ODataNavigationLink navigationLink);

		// Token: 0x06000CB7 RID: 3255
		public abstract void WriteEnd();

		// Token: 0x06000CB8 RID: 3256
		public abstract Task WriteEndAsync();

		// Token: 0x06000CB9 RID: 3257
		public abstract void WriteEntityReferenceLink(ODataEntityReferenceLink entityReferenceLink);

		// Token: 0x06000CBA RID: 3258
		public abstract Task WriteEntityReferenceLinkAsync(ODataEntityReferenceLink entityReferenceLink);

		// Token: 0x06000CBB RID: 3259
		public abstract void Flush();

		// Token: 0x06000CBC RID: 3260
		public abstract Task FlushAsync();
	}
}
