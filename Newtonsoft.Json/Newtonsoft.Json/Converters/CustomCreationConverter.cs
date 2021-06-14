using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200001A RID: 26
	public abstract class CustomCreationConverter<T> : JsonConverter
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00006477 File Offset: 0x00004677
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006484 File Offset: 0x00004684
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			T t = this.Create(objectType);
			if (t == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, t);
			return t;
		}

		// Token: 0x06000149 RID: 329
		public abstract T Create(Type objectType);

		// Token: 0x0600014A RID: 330 RVA: 0x000064CC File Offset: 0x000046CC
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000064DE File Offset: 0x000046DE
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
