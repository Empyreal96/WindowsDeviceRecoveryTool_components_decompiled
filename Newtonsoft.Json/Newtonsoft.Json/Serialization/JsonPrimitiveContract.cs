using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000BD RID: 189
	public class JsonPrimitiveContract : JsonContract
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00021FA5 File Offset: 0x000201A5
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x00021FAD File Offset: 0x000201AD
		internal PrimitiveTypeCode TypeCode { get; set; }

		// Token: 0x0600090F RID: 2319 RVA: 0x00021FB6 File Offset: 0x000201B6
		public JsonPrimitiveContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Primitive;
			this.TypeCode = ConvertUtils.GetTypeCode(underlyingType);
			this.IsReadOnlyOrFixedSize = true;
		}
	}
}
