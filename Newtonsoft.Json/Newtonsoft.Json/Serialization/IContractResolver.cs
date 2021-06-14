using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009D RID: 157
	public interface IContractResolver
	{
		// Token: 0x060007F8 RID: 2040
		JsonContract ResolveContract(Type type);
	}
}
