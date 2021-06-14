using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x020000CB RID: 203
	public interface IEdmCollectionValue : IEdmValue, IEdmElement
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000412 RID: 1042
		IEnumerable<IEdmDelayedValue> Elements { get; }
	}
}
