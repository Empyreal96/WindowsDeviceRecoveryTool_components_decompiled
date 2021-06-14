using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000018 RID: 24
	public class BinaryConverter : JsonConverter
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00006124 File Offset: 0x00004324
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			byte[] byteArray = this.GetByteArray(value);
			writer.WriteValue(byteArray);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000614C File Offset: 0x0000434C
		private byte[] GetByteArray(object value)
		{
			if (value.GetType().AssignableToTypeName("System.Data.Linq.Binary"))
			{
				this.EnsureReflectionObject(value.GetType());
				return (byte[])this._reflectionObject.GetValue(value, "ToArray");
			}
			if (value is SqlBinary)
			{
				return ((SqlBinary)value).Value;
			}
			throw new JsonSerializationException("Unexpected value type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000061C0 File Offset: 0x000043C0
		private void EnsureReflectionObject(Type t)
		{
			if (this._reflectionObject == null)
			{
				this._reflectionObject = ReflectionObject.Create(t, t.GetConstructor(new Type[]
				{
					typeof(byte[])
				}), new string[]
				{
					"ToArray"
				});
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000620C File Offset: 0x0000440C
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullable(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			else
			{
				byte[] array;
				if (reader.TokenType == JsonToken.StartArray)
				{
					array = this.ReadByteArray(reader);
				}
				else
				{
					if (reader.TokenType != JsonToken.String)
					{
						throw JsonSerializationException.Create(reader, "Unexpected token parsing binary. Expected String or StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
					}
					string s = reader.Value.ToString();
					array = Convert.FromBase64String(s);
				}
				if (type.AssignableToTypeName("System.Data.Linq.Binary"))
				{
					this.EnsureReflectionObject(type);
					return this._reflectionObject.Creator(new object[]
					{
						array
					});
				}
				if (type == typeof(SqlBinary))
				{
					return new SqlBinary(array);
				}
				throw JsonSerializationException.Create(reader, "Unexpected object type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, objectType));
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006310 File Offset: 0x00004510
		private byte[] ReadByteArray(JsonReader reader)
		{
			List<byte> list = new List<byte>();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.Comment:
					continue;
				case JsonToken.Raw:
					break;
				case JsonToken.Integer:
					list.Add(Convert.ToByte(reader.Value, CultureInfo.InvariantCulture));
					continue;
				default:
					if (tokenType == JsonToken.EndArray)
					{
						return list.ToArray();
					}
					break;
				}
				throw JsonSerializationException.Create(reader, "Unexpected token when reading bytes: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading bytes.");
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000639A File Offset: 0x0000459A
		public override bool CanConvert(Type objectType)
		{
			return objectType.AssignableToTypeName("System.Data.Linq.Binary") || (objectType == typeof(SqlBinary) || objectType == typeof(SqlBinary?));
		}

		// Token: 0x04000089 RID: 137
		private const string BinaryTypeName = "System.Data.Linq.Binary";

		// Token: 0x0400008A RID: 138
		private const string BinaryToArrayName = "ToArray";

		// Token: 0x0400008B RID: 139
		private ReflectionObject _reflectionObject;
	}
}
