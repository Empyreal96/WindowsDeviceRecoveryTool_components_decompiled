using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000DF RID: 223
	internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000AC6 RID: 2758
		object UnderlyingDictionary { get; }
	}
}
