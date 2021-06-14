using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200005E RID: 94
	public interface IJEnumerable<out T> : IEnumerable<T>, IEnumerable where T : JToken
	{
		// Token: 0x17000103 RID: 259
		IJEnumerable<JToken> this[object key]
		{
			get;
		}
	}
}
