using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000025 RID: 37
	public class StringEnumConverter : JsonConverter
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00007C67 File Offset: 0x00005E67
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00007C6F File Offset: 0x00005E6F
		public bool CamelCaseText { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00007C78 File Offset: 0x00005E78
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00007C80 File Offset: 0x00005E80
		public bool AllowIntegerValues { get; set; }

		// Token: 0x0600018F RID: 399 RVA: 0x00007C89 File Offset: 0x00005E89
		public StringEnumConverter()
		{
			this.AllowIntegerValues = true;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007C98 File Offset: 0x00005E98
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Enum @enum = (Enum)value;
			string text = @enum.ToString("G");
			if (char.IsNumber(text[0]) || text[0] == '-')
			{
				writer.WriteValue(value);
				return;
			}
			Type type = @enum.GetType();
			string value2 = EnumUtils.ToEnumName(type, text, this.CamelCaseText);
			writer.WriteValue(value2);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007D00 File Offset: 0x00005F00
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			Type type = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
			if (reader.TokenType != JsonToken.Null)
			{
				try
				{
					if (reader.TokenType == JsonToken.String)
					{
						string enumText = reader.Value.ToString();
						return EnumUtils.ParseEnumName(enumText, flag, type);
					}
					if (reader.TokenType == JsonToken.Integer)
					{
						if (!this.AllowIntegerValues)
						{
							throw JsonSerializationException.Create(reader, "Integer value {0} is not allowed.".FormatWith(CultureInfo.InvariantCulture, reader.Value));
						}
						return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
					}
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.FormatValueForPrint(reader.Value), objectType), ex);
				}
				throw JsonSerializationException.Create(reader, "Unexpected token {0} when parsing enum.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			if (!ReflectionUtils.IsNullableType(objectType))
			{
				throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
			}
			return null;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007E0C File Offset: 0x0000600C
		public override bool CanConvert(Type objectType)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			return type.IsEnum();
		}
	}
}
