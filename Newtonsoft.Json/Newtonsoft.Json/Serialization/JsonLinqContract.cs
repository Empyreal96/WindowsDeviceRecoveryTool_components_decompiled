using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000BB RID: 187
	public class JsonLinqContract : JsonContract
	{
		// Token: 0x060008F5 RID: 2293 RVA: 0x00021D59 File Offset: 0x0001FF59
		public JsonLinqContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Linq;
		}
	}
}
