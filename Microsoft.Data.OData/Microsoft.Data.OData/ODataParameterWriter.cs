using System;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000195 RID: 405
	public abstract class ODataParameterWriter
	{
		// Token: 0x06000C10 RID: 3088
		public abstract void WriteStart();

		// Token: 0x06000C11 RID: 3089
		public abstract Task WriteStartAsync();

		// Token: 0x06000C12 RID: 3090
		public abstract void WriteValue(string parameterName, object parameterValue);

		// Token: 0x06000C13 RID: 3091
		public abstract Task WriteValueAsync(string parameterName, object parameterValue);

		// Token: 0x06000C14 RID: 3092
		public abstract ODataCollectionWriter CreateCollectionWriter(string parameterName);

		// Token: 0x06000C15 RID: 3093
		public abstract Task<ODataCollectionWriter> CreateCollectionWriterAsync(string parameterName);

		// Token: 0x06000C16 RID: 3094
		public abstract void WriteEnd();

		// Token: 0x06000C17 RID: 3095
		public abstract Task WriteEndAsync();

		// Token: 0x06000C18 RID: 3096
		public abstract void Flush();

		// Token: 0x06000C19 RID: 3097
		public abstract Task FlushAsync();
	}
}
