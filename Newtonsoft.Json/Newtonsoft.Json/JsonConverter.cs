using System;
using Newtonsoft.Json.Schema;

namespace Newtonsoft.Json
{
	// Token: 0x02000017 RID: 23
	public abstract class JsonConverter
	{
		// Token: 0x06000135 RID: 309
		public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

		// Token: 0x06000136 RID: 310
		public abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

		// Token: 0x06000137 RID: 311
		public abstract bool CanConvert(Type objectType);

		// Token: 0x06000138 RID: 312 RVA: 0x00006112 File Offset: 0x00004312
		public virtual JsonSchema GetSchema()
		{
			return null;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00006115 File Offset: 0x00004315
		public virtual bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00006118 File Offset: 0x00004318
		public virtual bool CanWrite
		{
			get
			{
				return true;
			}
		}
	}
}
