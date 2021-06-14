using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000020 RID: 32
	public class ExpandoObjectConverter : JsonConverter
	{
		// Token: 0x06000167 RID: 359 RVA: 0x00007108 File Offset: 0x00005308
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000710A File Offset: 0x0000530A
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return this.ReadValue(reader);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007114 File Offset: 0x00005314
		private object ReadValue(JsonReader reader)
		{
			while (reader.TokenType == JsonToken.Comment)
			{
				if (!reader.Read())
				{
					throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
				}
			}
			switch (reader.TokenType)
			{
			case JsonToken.StartObject:
				return this.ReadObject(reader);
			case JsonToken.StartArray:
				return this.ReadList(reader);
			default:
				if (JsonTokenUtils.IsPrimitiveToken(reader.TokenType))
				{
					return reader.Value;
				}
				throw JsonSerializationException.Create(reader, "Unexpected token when converting ExpandoObject: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000071A0 File Offset: 0x000053A0
		private object ReadList(JsonReader reader)
		{
			IList<object> list = new List<object>();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.EndArray)
					{
						return list;
					}
					object item = this.ReadValue(reader);
					list.Add(item);
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000071EC File Offset: 0x000053EC
		private object ReadObject(JsonReader reader)
		{
			IDictionary<string, object> dictionary = new ExpandoObject();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.PropertyName:
				{
					string key = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
					}
					object value = this.ReadValue(reader);
					dictionary[key] = value;
					break;
				}
				case JsonToken.Comment:
					break;
				default:
					if (tokenType == JsonToken.EndObject)
					{
						return dictionary;
					}
					break;
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007266 File Offset: 0x00005466
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ExpandoObject);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00007278 File Offset: 0x00005478
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
