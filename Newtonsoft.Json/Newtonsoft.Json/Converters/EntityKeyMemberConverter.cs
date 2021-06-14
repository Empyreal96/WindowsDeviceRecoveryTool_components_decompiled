using System;
using System.Globalization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200001F RID: 31
	public class EntityKeyMemberConverter : JsonConverter
	{
		// Token: 0x06000160 RID: 352 RVA: 0x00006EBC File Offset: 0x000050BC
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			EntityKeyMemberConverter.EnsureReflectionObject(value.GetType());
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			string value2 = (string)EntityKeyMemberConverter._reflectionObject.GetValue(value, "Key");
			object value3 = EntityKeyMemberConverter._reflectionObject.GetValue(value, "Value");
			Type type = (value3 != null) ? value3.GetType() : null;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Key") : "Key");
			writer.WriteValue(value2);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Type") : "Type");
			writer.WriteValue((type != null) ? type.FullName : null);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Value") : "Value");
			if (type != null)
			{
				string value4;
				if (JsonSerializerInternalWriter.TryConvertToString(value3, type, out value4))
				{
					writer.WriteValue(value4);
				}
				else
				{
					writer.WriteValue(value3);
				}
			}
			else
			{
				writer.WriteNull();
			}
			writer.WriteEndObject();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006FBD File Offset: 0x000051BD
		private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
		{
			EntityKeyMemberConverter.ReadAndAssert(reader);
			if (reader.TokenType != JsonToken.PropertyName || !string.Equals(reader.Value.ToString(), propertyName, StringComparison.OrdinalIgnoreCase))
			{
				throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith(CultureInfo.InvariantCulture, propertyName));
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006FF8 File Offset: 0x000051F8
		private static void ReadAndAssert(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw new JsonSerializationException("Unexpected end.");
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007010 File Offset: 0x00005210
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			EntityKeyMemberConverter.EnsureReflectionObject(objectType);
			object obj = EntityKeyMemberConverter._reflectionObject.Creator(new object[0]);
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Key");
			EntityKeyMemberConverter.ReadAndAssert(reader);
			EntityKeyMemberConverter._reflectionObject.SetValue(obj, "Key", reader.Value.ToString());
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Type");
			EntityKeyMemberConverter.ReadAndAssert(reader);
			string typeName = reader.Value.ToString();
			Type type = Type.GetType(typeName);
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Value");
			EntityKeyMemberConverter.ReadAndAssert(reader);
			EntityKeyMemberConverter._reflectionObject.SetValue(obj, "Value", serializer.Deserialize(reader, type));
			EntityKeyMemberConverter.ReadAndAssert(reader);
			return obj;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000070BC File Offset: 0x000052BC
		private static void EnsureReflectionObject(Type objectType)
		{
			if (EntityKeyMemberConverter._reflectionObject == null)
			{
				EntityKeyMemberConverter._reflectionObject = ReflectionObject.Create(objectType, new string[]
				{
					"Key",
					"Value"
				});
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000070F3 File Offset: 0x000052F3
		public override bool CanConvert(Type objectType)
		{
			return objectType.AssignableToTypeName("System.Data.EntityKeyMember");
		}

		// Token: 0x0400008E RID: 142
		private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";

		// Token: 0x0400008F RID: 143
		private const string KeyPropertyName = "Key";

		// Token: 0x04000090 RID: 144
		private const string TypePropertyName = "Type";

		// Token: 0x04000091 RID: 145
		private const string ValuePropertyName = "Value";

		// Token: 0x04000092 RID: 146
		private static ReflectionObject _reflectionObject;
	}
}
