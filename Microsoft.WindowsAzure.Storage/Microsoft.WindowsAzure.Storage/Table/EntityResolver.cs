using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200013E RID: 318
	// (Invoke) Token: 0x06001486 RID: 5254
	public delegate T EntityResolver<T>(string partitionKey, string rowKey, DateTimeOffset timestamp, IDictionary<string, EntityProperty> properties, string etag);
}
