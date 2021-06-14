using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200004A RID: 74
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonDictionaryAttribute : JsonContainerAttribute
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x0000ACCF File Offset: 0x00008ECF
		public JsonDictionaryAttribute()
		{
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000ACD7 File Offset: 0x00008ED7
		public JsonDictionaryAttribute(string id) : base(id)
		{
		}
	}
}
