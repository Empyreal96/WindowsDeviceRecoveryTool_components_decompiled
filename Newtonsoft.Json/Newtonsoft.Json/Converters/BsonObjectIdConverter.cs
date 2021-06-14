using System;
using System.Globalization;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000019 RID: 25
	public class BsonObjectIdConverter : JsonConverter
	{
		// Token: 0x06000143 RID: 323 RVA: 0x000063DC File Offset: 0x000045DC
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			BsonObjectId bsonObjectId = (BsonObjectId)value;
			BsonWriter bsonWriter = writer as BsonWriter;
			if (bsonWriter != null)
			{
				bsonWriter.WriteObjectId(bsonObjectId.Value);
				return;
			}
			writer.WriteValue(bsonObjectId.Value);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00006414 File Offset: 0x00004614
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.Bytes)
			{
				throw new JsonSerializationException("Expected Bytes but got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			byte[] value = (byte[])reader.Value;
			return new BsonObjectId(value);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000645D File Offset: 0x0000465D
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(BsonObjectId);
		}
	}
}
