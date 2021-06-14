using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020001E4 RID: 484
	public interface IODataUrlResolver
	{
		// Token: 0x06000EF4 RID: 3828
		Uri ResolveUrl(Uri baseUri, Uri payloadUri);
	}
}
