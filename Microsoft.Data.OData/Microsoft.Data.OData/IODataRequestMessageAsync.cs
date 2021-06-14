using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000262 RID: 610
	public interface IODataRequestMessageAsync : IODataRequestMessage
	{
		// Token: 0x06001425 RID: 5157
		Task<Stream> GetStreamAsync();
	}
}
