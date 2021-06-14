using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000023 RID: 35
	public class KeyValuePairConverter : JsonConverter
	{
		// Token: 0x0600017B RID: 379 RVA: 0x000076D4 File Offset: 0x000058D4
		private static ReflectionObject InitializeReflectionObject(Type t)
		{
			IList<Type> genericArguments = t.GetGenericArguments();
			Type type = genericArguments[0];
			Type type2 = genericArguments[1];
			return ReflectionObject.Create(t, t.GetConstructor(new Type[]
			{
				type,
				type2
			}), new string[]
			{
				"Key",
				"Value"
			});
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007730 File Offset: 0x00005930
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ReflectionObject reflectionObject = KeyValuePairConverter.ReflectionObjectPerType.Get(value.GetType());
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Key") : "Key");
			serializer.Serialize(writer, reflectionObject.GetValue(value, "Key"), reflectionObject.GetType("Key"));
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Value") : "Value");
			serializer.Serialize(writer, reflectionObject.GetValue(value, "Value"), reflectionObject.GetType("Value"));
			writer.WriteEndObject();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000077D8 File Offset: 0x000059D8
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			Type key = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
			ReflectionObject reflectionObject = KeyValuePairConverter.ReflectionObjectPerType.Get(key);
			if (reader.TokenType != JsonToken.Null)
			{
				object obj = null;
				object obj2 = null;
				KeyValuePairConverter.ReadAndAssert(reader);
				while (reader.TokenType == JsonToken.PropertyName)
				{
					string a = reader.Value.ToString();
					if (string.Equals(a, "Key", StringComparison.OrdinalIgnoreCase))
					{
						KeyValuePairConverter.ReadAndAssert(reader);
						obj = serializer.Deserialize(reader, reflectionObject.GetType("Key"));
					}
					else if (string.Equals(a, "Value", StringComparison.OrdinalIgnoreCase))
					{
						KeyValuePairConverter.ReadAndAssert(reader);
						obj2 = serializer.Deserialize(reader, reflectionObject.GetType("Value"));
					}
					else
					{
						reader.Skip();
					}
					KeyValuePairConverter.ReadAndAssert(reader);
				}
				return reflectionObject.Creator(new object[]
				{
					obj,
					obj2
				});
			}
			if (!flag)
			{
				throw JsonSerializationException.Create(reader, "Cannot convert null value to KeyValuePair.");
			}
			return null;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000078C8 File Offset: 0x00005AC8
		public override bool CanConvert(Type objectType)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			return type.IsValueType() && type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(KeyValuePair<, >);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000790E File Offset: 0x00005B0E
		private static void ReadAndAssert(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading KeyValuePair.");
			}
		}

		// Token: 0x04000097 RID: 151
		private const string KeyName = "Key";

		// Token: 0x04000098 RID: 152
		private const string ValueName = "Value";

		// Token: 0x04000099 RID: 153
		private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(new Func<Type, ReflectionObject>(KeyValuePairConverter.InitializeReflectionObject));
	}
}
