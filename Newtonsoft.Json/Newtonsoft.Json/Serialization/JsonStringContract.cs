using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C6 RID: 198
	public class JsonStringContract : JsonPrimitiveContract
	{
		// Token: 0x060009F3 RID: 2547 RVA: 0x00027878 File Offset: 0x00025A78
		public JsonStringContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.String;
		}
	}
}
