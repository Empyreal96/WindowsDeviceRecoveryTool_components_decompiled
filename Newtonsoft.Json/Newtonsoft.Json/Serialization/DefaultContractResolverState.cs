using System;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A2 RID: 162
	internal class DefaultContractResolverState
	{
		// Token: 0x040002C3 RID: 707
		public Dictionary<ResolverContractKey, JsonContract> ContractCache;

		// Token: 0x040002C4 RID: 708
		public PropertyNameTable NameTable = new PropertyNameTable();
	}
}
