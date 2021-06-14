using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200001E RID: 30
	public class DiscriminatedUnionConverter : JsonConverter
	{
		// Token: 0x0600015B RID: 347 RVA: 0x00006AF4 File Offset: 0x00004CF4
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			Type type = value.GetType();
			MethodCall<object, object> getUnionFields = FSharpUtils.GetUnionFields;
			object target = null;
			object[] array = new object[3];
			array[0] = value;
			array[1] = type;
			object arg = getUnionFields(target, array);
			object arg2 = FSharpUtils.GetUnionCaseInfo(arg);
			object obj = FSharpUtils.GetUnionCaseFields(arg);
			object obj2 = FSharpUtils.GetUnionCaseInfoName(arg2);
			object[] array2 = obj as object[];
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Case") : "Case");
			writer.WriteValue((string)obj2);
			if (array2 != null && array2.Length > 0)
			{
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Fields") : "Fields");
				serializer.Serialize(writer, obj);
			}
			writer.WriteEndObject();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006BC8 File Offset: 0x00004DC8
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			object obj = null;
			string text = null;
			JArray jarray = null;
			DiscriminatedUnionConverter.ReadAndAssert(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text2 = reader.Value.ToString();
				if (string.Equals(text2, "Case", StringComparison.OrdinalIgnoreCase))
				{
					DiscriminatedUnionConverter.ReadAndAssert(reader);
					MethodCall<object, object> getUnionCases = FSharpUtils.GetUnionCases;
					object target = null;
					object[] array = new object[2];
					array[0] = objectType;
					IEnumerable enumerable = (IEnumerable)getUnionCases(target, array);
					text = reader.Value.ToString();
					foreach (object obj2 in enumerable)
					{
						if ((string)FSharpUtils.GetUnionCaseInfoName(obj2) == text)
						{
							obj = obj2;
							break;
						}
					}
					if (obj == null)
					{
						throw JsonSerializationException.Create(reader, "No union type found with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
					}
				}
				else
				{
					if (!string.Equals(text2, "Fields", StringComparison.OrdinalIgnoreCase))
					{
						throw JsonSerializationException.Create(reader, "Unexpected property '{0}' found when reading union.".FormatWith(CultureInfo.InvariantCulture, text2));
					}
					DiscriminatedUnionConverter.ReadAndAssert(reader);
					if (reader.TokenType != JsonToken.StartArray)
					{
						throw JsonSerializationException.Create(reader, "Union fields must been an array.");
					}
					jarray = (JArray)JToken.ReadFrom(reader);
				}
				DiscriminatedUnionConverter.ReadAndAssert(reader);
			}
			if (obj == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union name found.".FormatWith(CultureInfo.InvariantCulture, "Case"));
			}
			PropertyInfo[] array2 = (PropertyInfo[])FSharpUtils.GetUnionCaseInfoFields(obj, new object[0]);
			object[] array3 = new object[array2.Length];
			if (array2.Length > 0 && jarray == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union fields found.".FormatWith(CultureInfo.InvariantCulture, "Fields"));
			}
			if (jarray != null)
			{
				if (array2.Length != jarray.Count)
				{
					throw JsonSerializationException.Create(reader, "The number of field values does not match the number of properties definied by union '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
				}
				for (int i = 0; i < jarray.Count; i++)
				{
					JToken jtoken = jarray[i];
					PropertyInfo propertyInfo = array2[i];
					array3[i] = jtoken.ToObject(propertyInfo.PropertyType, serializer);
				}
			}
			MethodCall<object, object> makeUnion = FSharpUtils.MakeUnion;
			object target2 = null;
			object[] array4 = new object[3];
			array4[0] = obj;
			array4[1] = array3;
			return makeUnion(target2, array4);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006E08 File Offset: 0x00005008
		public override bool CanConvert(Type objectType)
		{
			if (typeof(IEnumerable).IsAssignableFrom(objectType))
			{
				return false;
			}
			object[] customAttributes = objectType.GetCustomAttributes(true);
			bool flag = false;
			foreach (object obj in customAttributes)
			{
				Type type = obj.GetType();
				if (type.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute")
				{
					FSharpUtils.EnsureInitialized(type.Assembly());
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			MethodCall<object, object> isUnion = FSharpUtils.IsUnion;
			object target = null;
			object[] array2 = new object[2];
			array2[0] = objectType;
			return (bool)isUnion(target, array2);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006E9B File Offset: 0x0000509B
		private static void ReadAndAssert(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading union.");
			}
		}

		// Token: 0x0400008C RID: 140
		private const string CasePropertyName = "Case";

		// Token: 0x0400008D RID: 141
		private const string FieldsPropertyName = "Fields";
	}
}
