using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000260 RID: 608
	public interface IODataResponseMessageAsync : IODataResponseMessage
	{
		// Token: 0x06001418 RID: 5144
		Task<Stream> GetStreamAsync();
	}
}
