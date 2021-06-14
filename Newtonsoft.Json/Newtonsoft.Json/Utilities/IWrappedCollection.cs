using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000D4 RID: 212
	internal interface IWrappedCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000A6A RID: 2666
		object UnderlyingCollection { get; }
	}
}
