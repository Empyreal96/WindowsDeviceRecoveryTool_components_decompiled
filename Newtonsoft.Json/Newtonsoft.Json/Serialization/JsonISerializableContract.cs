using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000BA RID: 186
	public class JsonISerializableContract : JsonContainerContract
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00021D38 File Offset: 0x0001FF38
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x00021D40 File Offset: 0x0001FF40
		public ObjectConstructor<object> ISerializableCreator { get; set; }

		// Token: 0x060008F4 RID: 2292 RVA: 0x00021D49 File Offset: 0x0001FF49
		public JsonISerializableContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Serializable;
		}
	}
}
