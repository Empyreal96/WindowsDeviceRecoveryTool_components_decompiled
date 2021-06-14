using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C2 RID: 194
	internal class JsonSerializerInternalReader : JsonSerializerInternalBase
	{
		// Token: 0x0600095C RID: 2396 RVA: 0x000225F9 File Offset: 0x000207F9
		public JsonSerializerInternalReader(JsonSerializer serializer) : base(serializer)
		{
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00022604 File Offset: 0x00020804
		public void Populate(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(target, "target");
			Type type = target.GetType();
			JsonContract jsonContract = this.Serializer._contractResolver.ResolveContract(type);
			if (reader.TokenType == JsonToken.None)
			{
				reader.Read();
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				if (jsonContract.ContractType == JsonContractType.Array)
				{
					JsonArrayContract jsonArrayContract = (JsonArrayContract)jsonContract;
					this.PopulateList(jsonArrayContract.ShouldCreateWrapper ? jsonArrayContract.CreateWrapper(target) : ((IList)target), reader, jsonArrayContract, null, null);
					return;
				}
				throw JsonSerializationException.Create(reader, "Cannot populate JSON array onto type '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
			}
			else
			{
				if (reader.TokenType != JsonToken.StartObject)
				{
					throw JsonSerializationException.Create(reader, "Unexpected initial token '{0}' when populating object. Expected JSON object or array.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				}
				this.CheckedRead(reader);
				string id = null;
				if (this.Serializer.MetadataPropertyHandling != MetadataPropertyHandling.Ignore && reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$id", StringComparison.Ordinal))
				{
					this.CheckedRead(reader);
					id = ((reader.Value != null) ? reader.Value.ToString() : null);
					this.CheckedRead(reader);
				}
				if (jsonContract.ContractType == JsonContractType.Dictionary)
				{
					JsonDictionaryContract jsonDictionaryContract = (JsonDictionaryContract)jsonContract;
					this.PopulateDictionary(jsonDictionaryContract.ShouldCreateWrapper ? jsonDictionaryContract.CreateWrapper(target) : ((IDictionary)target), reader, jsonDictionaryContract, null, id);
					return;
				}
				if (jsonContract.ContractType == JsonContractType.Object)
				{
					this.PopulateObject(target, reader, (JsonObjectContract)jsonContract, null, id);
					return;
				}
				throw JsonSerializationException.Create(reader, "Cannot populate JSON object onto type '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00022789 File Offset: 0x00020989
		private JsonContract GetContractSafe(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return this.Serializer._contractResolver.ResolveContract(type);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x000227A8 File Offset: 0x000209A8
		public object Deserialize(JsonReader reader, Type objectType, bool checkAdditionalContent)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			JsonContract contractSafe = this.GetContractSafe(objectType);
			object result;
			try
			{
				JsonConverter converter = this.GetConverter(contractSafe, null, null, null);
				if (reader.TokenType == JsonToken.None && !this.ReadForType(reader, contractSafe, converter != null))
				{
					if (contractSafe != null && !contractSafe.IsNullable)
					{
						throw JsonSerializationException.Create(reader, "No JSON content found and type '{0}' is not nullable.".FormatWith(CultureInfo.InvariantCulture, contractSafe.UnderlyingType));
					}
					result = null;
				}
				else
				{
					object obj;
					if (converter != null && converter.CanRead)
					{
						obj = this.DeserializeConvertable(converter, reader, objectType, null);
					}
					else
					{
						obj = this.CreateValueInternal(reader, objectType, contractSafe, null, null, null, null);
					}
					if (checkAdditionalContent && reader.Read() && reader.TokenType != JsonToken.Comment)
					{
						throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
					}
					result = obj;
				}
			}
			catch (Exception ex)
			{
				if (!base.IsErrorHandled(null, contractSafe, null, reader as IJsonLineInfo, reader.Path, ex))
				{
					base.ClearErrorContext();
					throw;
				}
				this.HandleError(reader, false, 0);
				result = null;
			}
			return result;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x000228A8 File Offset: 0x00020AA8
		private JsonSerializerProxy GetInternalSerializer()
		{
			if (this._internalSerializer == null)
			{
				this._internalSerializer = new JsonSerializerProxy(this);
			}
			return this._internalSerializer;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000228C4 File Offset: 0x00020AC4
		private JToken CreateJToken(JsonReader reader, JsonContract contract)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (contract != null)
			{
				if (contract.UnderlyingType == typeof(JRaw))
				{
					return JRaw.Create(reader);
				}
				if (reader.TokenType == JsonToken.Null && !(contract.UnderlyingType == typeof(JValue)) && !(contract.UnderlyingType == typeof(JToken)))
				{
					return null;
				}
			}
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jtokenWriter.WriteToken(reader);
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00022968 File Offset: 0x00020B68
		private JToken CreateJObject(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jtokenWriter.WriteStartObject();
				for (;;)
				{
					if (reader.TokenType == JsonToken.PropertyName)
					{
						string text = (string)reader.Value;
						while (reader.Read() && reader.TokenType == JsonToken.Comment)
						{
						}
						if (!this.CheckPropertyName(reader, text))
						{
							jtokenWriter.WritePropertyName(text);
							jtokenWriter.WriteToken(reader, true, true);
						}
					}
					else if (reader.TokenType != JsonToken.Comment)
					{
						break;
					}
					if (!reader.Read())
					{
						goto Block_7;
					}
				}
				jtokenWriter.WriteEndObject();
				return jtokenWriter.Token;
				Block_7:
				throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
			}
			JToken result;
			return result;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00022A18 File Offset: 0x00020C18
		private object CreateValueInternal(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue)
		{
			if (contract != null && contract.ContractType == JsonContractType.Linq)
			{
				return this.CreateJToken(reader, contract);
			}
			for (;;)
			{
				switch (reader.TokenType)
				{
				case JsonToken.StartObject:
					goto IL_6D;
				case JsonToken.StartArray:
					goto IL_7F;
				case JsonToken.StartConstructor:
					goto IL_111;
				case JsonToken.Comment:
					if (!reader.Read())
					{
						goto Block_11;
					}
					continue;
				case JsonToken.Raw:
					goto IL_15A;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
					goto IL_8E;
				case JsonToken.String:
					goto IL_A3;
				case JsonToken.Null:
				case JsonToken.Undefined:
					goto IL_12D;
				}
				break;
			}
			goto IL_16B;
			IL_6D:
			return this.CreateObject(reader, objectType, contract, member, containerContract, containerMember, existingValue);
			IL_7F:
			return this.CreateList(reader, objectType, contract, member, existingValue, null);
			IL_8E:
			return this.EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, contract, objectType);
			IL_A3:
			string text = (string)reader.Value;
			if (string.IsNullOrEmpty(text) && objectType != typeof(string) && objectType != typeof(object) && contract != null && contract.IsNullable)
			{
				return null;
			}
			if (objectType == typeof(byte[]))
			{
				return Convert.FromBase64String(text);
			}
			return this.EnsureType(reader, text, CultureInfo.InvariantCulture, contract, objectType);
			IL_111:
			string value = reader.Value.ToString();
			return this.EnsureType(reader, value, CultureInfo.InvariantCulture, contract, objectType);
			IL_12D:
			if (objectType == typeof(DBNull))
			{
				return DBNull.Value;
			}
			return this.EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, contract, objectType);
			IL_15A:
			return new JRaw((string)reader.Value);
			IL_16B:
			throw JsonSerializationException.Create(reader, "Unexpected token while deserializing object: " + reader.TokenType);
			Block_11:
			throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00022BC4 File Offset: 0x00020DC4
		internal string GetExpectedDescription(JsonContract contract)
		{
			switch (contract.ContractType)
			{
			case JsonContractType.Object:
			case JsonContractType.Dictionary:
			case JsonContractType.Dynamic:
			case JsonContractType.Serializable:
				return "JSON object (e.g. {\"name\":\"value\"})";
			case JsonContractType.Array:
				return "JSON array (e.g. [1,2,3])";
			case JsonContractType.Primitive:
				return "JSON primitive value (e.g. string, number, boolean, null)";
			case JsonContractType.String:
				return "JSON string value";
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00022C1C File Offset: 0x00020E1C
		private JsonConverter GetConverter(JsonContract contract, JsonConverter memberConverter, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			JsonConverter result = null;
			if (memberConverter != null)
			{
				result = memberConverter;
			}
			else if (containerProperty != null && containerProperty.ItemConverter != null)
			{
				result = containerProperty.ItemConverter;
			}
			else if (containerContract != null && containerContract.ItemConverter != null)
			{
				result = containerContract.ItemConverter;
			}
			else if (contract != null)
			{
				JsonConverter matchingConverter;
				if (contract.Converter != null)
				{
					result = contract.Converter;
				}
				else if ((matchingConverter = this.Serializer.GetMatchingConverter(contract.UnderlyingType)) != null)
				{
					result = matchingConverter;
				}
				else if (contract.InternalConverter != null)
				{
					result = contract.InternalConverter;
				}
			}
			return result;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00022C9C File Offset: 0x00020E9C
		private object CreateObject(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue)
		{
			Type type = objectType;
			string text;
			if (this.Serializer.MetadataPropertyHandling == MetadataPropertyHandling.Ignore)
			{
				this.CheckedRead(reader);
				text = null;
			}
			else if (this.Serializer.MetadataPropertyHandling == MetadataPropertyHandling.ReadAhead)
			{
				JTokenReader jtokenReader = reader as JTokenReader;
				if (jtokenReader == null)
				{
					JToken jtoken = JToken.ReadFrom(reader);
					jtokenReader = (JTokenReader)jtoken.CreateReader();
					jtokenReader.Culture = reader.Culture;
					jtokenReader.DateFormatString = reader.DateFormatString;
					jtokenReader.DateParseHandling = reader.DateParseHandling;
					jtokenReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
					jtokenReader.FloatParseHandling = reader.FloatParseHandling;
					jtokenReader.SupportMultipleContent = reader.SupportMultipleContent;
					this.CheckedRead(jtokenReader);
					reader = jtokenReader;
				}
				object result;
				if (this.ReadMetadataPropertiesToken(jtokenReader, ref type, ref contract, member, containerContract, containerMember, existingValue, out result, out text))
				{
					return result;
				}
			}
			else
			{
				this.CheckedRead(reader);
				object result2;
				if (this.ReadMetadataProperties(reader, ref type, ref contract, member, containerContract, containerMember, existingValue, out result2, out text))
				{
					return result2;
				}
			}
			if (this.HasNoDefinedType(contract))
			{
				return this.CreateJObject(reader);
			}
			switch (contract.ContractType)
			{
			case JsonContractType.Object:
			{
				bool flag = false;
				JsonObjectContract jsonObjectContract = (JsonObjectContract)contract;
				object obj;
				if (existingValue != null && (type == objectType || type.IsAssignableFrom(existingValue.GetType())))
				{
					obj = existingValue;
				}
				else
				{
					obj = this.CreateNewObject(reader, jsonObjectContract, member, containerMember, text, out flag);
				}
				if (flag)
				{
					return obj;
				}
				return this.PopulateObject(obj, reader, jsonObjectContract, member, text);
			}
			case JsonContractType.Primitive:
			{
				JsonPrimitiveContract contract2 = (JsonPrimitiveContract)contract;
				if (this.Serializer.MetadataPropertyHandling != MetadataPropertyHandling.Ignore && reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$value", StringComparison.Ordinal))
				{
					this.CheckedRead(reader);
					if (reader.TokenType == JsonToken.StartObject)
					{
						throw JsonSerializationException.Create(reader, "Unexpected token when deserializing primitive value: " + reader.TokenType);
					}
					object result3 = this.CreateValueInternal(reader, type, contract2, member, null, null, existingValue);
					this.CheckedRead(reader);
					return result3;
				}
				break;
			}
			case JsonContractType.Dictionary:
			{
				JsonDictionaryContract jsonDictionaryContract = (JsonDictionaryContract)contract;
				object result4;
				if (existingValue == null)
				{
					bool flag2;
					IDictionary dictionary = this.CreateNewDictionary(reader, jsonDictionaryContract, out flag2);
					if (flag2)
					{
						if (text != null)
						{
							throw JsonSerializationException.Create(reader, "Cannot preserve reference to readonly dictionary, or dictionary created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
						}
						if (contract.OnSerializingCallbacks.Count > 0)
						{
							throw JsonSerializationException.Create(reader, "Cannot call OnSerializing on readonly dictionary, or dictionary created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
						}
						if (contract.OnErrorCallbacks.Count > 0)
						{
							throw JsonSerializationException.Create(reader, "Cannot call OnError on readonly list, or dictionary created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
						}
						if (!jsonDictionaryContract.HasParametrizedCreator)
						{
							throw JsonSerializationException.Create(reader, "Cannot deserialize readonly or fixed size dictionary: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
						}
					}
					this.PopulateDictionary(dictionary, reader, jsonDictionaryContract, member, text);
					if (flag2)
					{
						return jsonDictionaryContract.ParametrizedCreator(new object[]
						{
							dictionary
						});
					}
					if (dictionary is IWrappedDictionary)
					{
						return ((IWrappedDictionary)dictionary).UnderlyingDictionary;
					}
					result4 = dictionary;
				}
				else
				{
					result4 = this.PopulateDictionary(jsonDictionaryContract.ShouldCreateWrapper ? jsonDictionaryContract.CreateWrapper(existingValue) : ((IDictionary)existingValue), reader, jsonDictionaryContract, member, text);
				}
				return result4;
			}
			case JsonContractType.Dynamic:
			{
				JsonDynamicContract contract3 = (JsonDynamicContract)contract;
				return this.CreateDynamic(reader, contract3, member, text);
			}
			case JsonContractType.Serializable:
			{
				JsonISerializableContract contract4 = (JsonISerializableContract)contract;
				return this.CreateISerializable(reader, contract4, member, text);
			}
			}
			string text2 = "Cannot deserialize the current JSON object (e.g. {{\"name\":\"value\"}}) into type '{0}' because the type requires a {1} to deserialize correctly." + Environment.NewLine + "To fix this error either change the JSON to a {1} or change the deserialized type so that it is a normal .NET type (e.g. not a primitive type like integer, not a collection type like an array or List<T>) that can be deserialized from a JSON object. JsonObjectAttribute can also be added to the type to force it to deserialize from a JSON object." + Environment.NewLine;
			text2 = text2.FormatWith(CultureInfo.InvariantCulture, type, this.GetExpectedDescription(contract));
			throw JsonSerializationException.Create(reader, text2);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0002303C File Offset: 0x0002123C
		private bool ReadMetadataPropertiesToken(JTokenReader reader, ref Type objectType, ref JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue, out object newValue, out string id)
		{
			id = null;
			newValue = null;
			if (reader.TokenType == JsonToken.StartObject)
			{
				JObject jobject = (JObject)reader.CurrentToken;
				JToken jtoken = jobject["$ref"];
				if (jtoken != null)
				{
					if (jtoken.Type != JTokenType.String && jtoken.Type != JTokenType.Null)
					{
						throw JsonSerializationException.Create(jtoken, jtoken.Path, "JSON reference {0} property must have a string or null value.".FormatWith(CultureInfo.InvariantCulture, "$ref"), null);
					}
					JToken parent = jtoken.Parent;
					JToken jtoken2 = null;
					if (parent.Next != null)
					{
						jtoken2 = parent.Next;
					}
					else if (parent.Previous != null)
					{
						jtoken2 = parent.Previous;
					}
					string text = (string)jtoken;
					if (text != null)
					{
						if (jtoken2 != null)
						{
							throw JsonSerializationException.Create(jtoken2, jtoken2.Path, "Additional content found in JSON reference object. A JSON reference object should only have a {0} property.".FormatWith(CultureInfo.InvariantCulture, "$ref"), null);
						}
						newValue = this.Serializer.GetReferenceResolver().ResolveReference(this, text);
						if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
						{
							this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader, reader.Path, "Resolved object reference '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, text, newValue.GetType())), null);
						}
						reader.Skip();
						return true;
					}
				}
				JToken jtoken3 = jobject["$type"];
				if (jtoken3 != null)
				{
					string qualifiedTypeName = (string)jtoken3;
					JsonReader reader2 = jtoken3.CreateReader();
					this.CheckedRead(reader2);
					this.ResolveTypeName(reader2, ref objectType, ref contract, member, containerContract, containerMember, qualifiedTypeName);
					JToken jtoken4 = jobject["$value"];
					if (jtoken4 != null)
					{
						for (;;)
						{
							this.CheckedRead(reader);
							if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$value")
							{
								break;
							}
							this.CheckedRead(reader);
							reader.Skip();
						}
						return false;
					}
				}
				JToken jtoken5 = jobject["$id"];
				if (jtoken5 != null)
				{
					id = (string)jtoken5;
				}
				JToken jtoken6 = jobject["$values"];
				if (jtoken6 != null)
				{
					JsonReader reader3 = jtoken6.CreateReader();
					this.CheckedRead(reader3);
					newValue = this.CreateList(reader3, objectType, contract, member, existingValue, id);
					reader.Skip();
					return true;
				}
			}
			this.CheckedRead(reader);
			return false;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0002325C File Offset: 0x0002145C
		private bool ReadMetadataProperties(JsonReader reader, ref Type objectType, ref JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue, out object newValue, out string id)
		{
			id = null;
			newValue = null;
			if (reader.TokenType == JsonToken.PropertyName)
			{
				string text = reader.Value.ToString();
				if (text.Length > 0 && text[0] == '$')
				{
					string text2;
					for (;;)
					{
						text = reader.Value.ToString();
						bool flag;
						if (string.Equals(text, "$ref", StringComparison.Ordinal))
						{
							this.CheckedRead(reader);
							if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null)
							{
								break;
							}
							text2 = ((reader.Value != null) ? reader.Value.ToString() : null);
							this.CheckedRead(reader);
							if (text2 != null)
							{
								goto Block_7;
							}
							flag = true;
						}
						else if (string.Equals(text, "$type", StringComparison.Ordinal))
						{
							this.CheckedRead(reader);
							string qualifiedTypeName = reader.Value.ToString();
							this.ResolveTypeName(reader, ref objectType, ref contract, member, containerContract, containerMember, qualifiedTypeName);
							this.CheckedRead(reader);
							flag = true;
						}
						else if (string.Equals(text, "$id", StringComparison.Ordinal))
						{
							this.CheckedRead(reader);
							id = ((reader.Value != null) ? reader.Value.ToString() : null);
							this.CheckedRead(reader);
							flag = true;
						}
						else
						{
							if (string.Equals(text, "$values", StringComparison.Ordinal))
							{
								goto Block_14;
							}
							flag = false;
						}
						if (!flag || reader.TokenType != JsonToken.PropertyName)
						{
							return false;
						}
					}
					throw JsonSerializationException.Create(reader, "JSON reference {0} property must have a string or null value.".FormatWith(CultureInfo.InvariantCulture, "$ref"));
					Block_7:
					if (reader.TokenType == JsonToken.PropertyName)
					{
						throw JsonSerializationException.Create(reader, "Additional content found in JSON reference object. A JSON reference object should only have a {0} property.".FormatWith(CultureInfo.InvariantCulture, "$ref"));
					}
					newValue = this.Serializer.GetReferenceResolver().ResolveReference(this, text2);
					if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
					{
						this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Resolved object reference '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, text2, newValue.GetType())), null);
					}
					return true;
					Block_14:
					this.CheckedRead(reader);
					object obj = this.CreateList(reader, objectType, contract, member, existingValue, id);
					this.CheckedRead(reader);
					newValue = obj;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00023468 File Offset: 0x00021668
		private void ResolveTypeName(JsonReader reader, ref Type objectType, ref JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, string qualifiedTypeName)
		{
			TypeNameHandling typeNameHandling = ((member != null) ? member.TypeNameHandling : null) ?? (((containerContract != null) ? containerContract.ItemTypeNameHandling : null) ?? (((containerMember != null) ? containerMember.ItemTypeNameHandling : null) ?? this.Serializer._typeNameHandling));
			if (typeNameHandling != TypeNameHandling.None)
			{
				string typeName;
				string assemblyName;
				ReflectionUtils.SplitFullyQualifiedTypeName(qualifiedTypeName, out typeName, out assemblyName);
				Type type;
				try
				{
					type = this.Serializer._binder.BindToType(assemblyName, typeName);
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error resolving type specified in JSON '{0}'.".FormatWith(CultureInfo.InvariantCulture, qualifiedTypeName), ex);
				}
				if (type == null)
				{
					throw JsonSerializationException.Create(reader, "Type specified in JSON '{0}' was not resolved.".FormatWith(CultureInfo.InvariantCulture, qualifiedTypeName));
				}
				if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
				{
					this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Resolved type '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, qualifiedTypeName, type)), null);
				}
				if (objectType != null && objectType != typeof(IDynamicMetaObjectProvider) && !objectType.IsAssignableFrom(type))
				{
					throw JsonSerializationException.Create(reader, "Type specified in JSON '{0}' is not compatible with '{1}'.".FormatWith(CultureInfo.InvariantCulture, type.AssemblyQualifiedName, objectType.AssemblyQualifiedName));
				}
				objectType = type;
				contract = this.GetContractSafe(type);
			}
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00023614 File Offset: 0x00021814
		private JsonArrayContract EnsureArrayContract(JsonReader reader, Type objectType, JsonContract contract)
		{
			if (contract == null)
			{
				throw JsonSerializationException.Create(reader, "Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.InvariantCulture, objectType));
			}
			JsonArrayContract jsonArrayContract = contract as JsonArrayContract;
			if (jsonArrayContract == null)
			{
				string text = "Cannot deserialize the current JSON array (e.g. [1,2,3]) into type '{0}' because the type requires a {1} to deserialize correctly." + Environment.NewLine + "To fix this error either change the JSON to a {1} or change the deserialized type to an array or a type that implements a collection interface (e.g. ICollection, IList) like List<T> that can be deserialized from a JSON array. JsonArrayAttribute can also be added to the type to force it to deserialize from a JSON array." + Environment.NewLine;
				text = text.FormatWith(CultureInfo.InvariantCulture, objectType, this.GetExpectedDescription(contract));
				throw JsonSerializationException.Create(reader, text);
			}
			return jsonArrayContract;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0002367C File Offset: 0x0002187C
		private void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x00023694 File Offset: 0x00021894
		private object CreateList(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue, string id)
		{
			if (this.HasNoDefinedType(contract))
			{
				return this.CreateJToken(reader, contract);
			}
			JsonArrayContract jsonArrayContract = this.EnsureArrayContract(reader, objectType, contract);
			object result;
			if (existingValue == null)
			{
				bool flag;
				IList list = this.CreateNewList(reader, jsonArrayContract, out flag);
				if (flag)
				{
					if (id != null)
					{
						throw JsonSerializationException.Create(reader, "Cannot preserve reference to array or readonly list, or list created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
					}
					if (contract.OnSerializingCallbacks.Count > 0)
					{
						throw JsonSerializationException.Create(reader, "Cannot call OnSerializing on an array or readonly list, or list created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
					}
					if (contract.OnErrorCallbacks.Count > 0)
					{
						throw JsonSerializationException.Create(reader, "Cannot call OnError on an array or readonly list, or list created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
					}
					if (!jsonArrayContract.HasParametrizedCreator && !jsonArrayContract.IsArray)
					{
						throw JsonSerializationException.Create(reader, "Cannot deserialize readonly or fixed size list: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
					}
				}
				if (!jsonArrayContract.IsMultidimensionalArray)
				{
					this.PopulateList(list, reader, jsonArrayContract, member, id);
				}
				else
				{
					this.PopulateMultidimensionalArray(list, reader, jsonArrayContract, member, id);
				}
				if (flag)
				{
					if (jsonArrayContract.IsMultidimensionalArray)
					{
						list = CollectionUtils.ToMultidimensionalArray(list, jsonArrayContract.CollectionItemType, contract.CreatedType.GetArrayRank());
					}
					else
					{
						if (!jsonArrayContract.IsArray)
						{
							return jsonArrayContract.ParametrizedCreator(new object[]
							{
								list
							});
						}
						Array array = Array.CreateInstance(jsonArrayContract.CollectionItemType, list.Count);
						list.CopyTo(array, 0);
						list = array;
					}
				}
				else if (list is IWrappedCollection)
				{
					return ((IWrappedCollection)list).UnderlyingCollection;
				}
				result = list;
			}
			else
			{
				if (!jsonArrayContract.CanDeserialize)
				{
					throw JsonSerializationException.Create(reader, "Cannot populate list type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.CreatedType));
				}
				result = this.PopulateList(jsonArrayContract.ShouldCreateWrapper ? jsonArrayContract.CreateWrapper(existingValue) : ((IList)existingValue), reader, jsonArrayContract, member, id);
			}
			return result;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00023867 File Offset: 0x00021A67
		private bool HasNoDefinedType(JsonContract contract)
		{
			return contract == null || contract.UnderlyingType == typeof(object) || contract.ContractType == JsonContractType.Linq || contract.UnderlyingType == typeof(IDynamicMetaObjectProvider);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000238A4 File Offset: 0x00021AA4
		private object EnsureType(JsonReader reader, object value, CultureInfo culture, JsonContract contract, Type targetType)
		{
			if (targetType == null)
			{
				return value;
			}
			Type objectType = ReflectionUtils.GetObjectType(value);
			if (objectType != targetType)
			{
				if (value == null && contract.IsNullable)
				{
					return null;
				}
				try
				{
					if (!contract.IsConvertable)
					{
						return ConvertUtils.ConvertOrCast(value, culture, contract.NonNullableUnderlyingType);
					}
					JsonPrimitiveContract jsonPrimitiveContract = (JsonPrimitiveContract)contract;
					if (contract.IsEnum)
					{
						if (value is string)
						{
							return Enum.Parse(contract.NonNullableUnderlyingType, value.ToString(), true);
						}
						if (ConvertUtils.IsInteger(jsonPrimitiveContract.TypeCode))
						{
							return Enum.ToObject(contract.NonNullableUnderlyingType, value);
						}
					}
					if (value is BigInteger)
					{
						return ConvertUtils.FromBigInteger((BigInteger)value, targetType);
					}
					return Convert.ChangeType(value, contract.NonNullableUnderlyingType, culture);
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.FormatValueForPrint(value), targetType), ex);
				}
				return value;
			}
			return value;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000239A8 File Offset: 0x00021BA8
		private bool SetPropertyValue(JsonProperty property, JsonConverter propertyConverter, JsonContainerContract containerContract, JsonProperty containerProperty, JsonReader reader, object target)
		{
			bool flag;
			object value;
			JsonContract contract;
			bool flag2;
			if (this.CalculatePropertyDetails(property, ref propertyConverter, containerContract, containerProperty, reader, target, out flag, out value, out contract, out flag2))
			{
				return false;
			}
			object obj;
			if (propertyConverter != null && propertyConverter.CanRead)
			{
				if (!flag2 && target != null && property.Readable)
				{
					value = property.ValueProvider.GetValue(target);
				}
				obj = this.DeserializeConvertable(propertyConverter, reader, property.PropertyType, value);
			}
			else
			{
				obj = this.CreateValueInternal(reader, property.PropertyType, contract, property, containerContract, containerProperty, flag ? value : null);
			}
			if ((!flag || obj != value) && this.ShouldSetPropertyValue(property, obj))
			{
				property.ValueProvider.SetValue(target, obj);
				if (property.SetIsSpecified != null)
				{
					if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
					{
						this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "IsSpecified for property '{0}' on {1} set to true.".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, property.DeclaringType)), null);
					}
					property.SetIsSpecified(target, true);
				}
				return true;
			}
			return flag;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00023AC0 File Offset: 0x00021CC0
		private bool CalculatePropertyDetails(JsonProperty property, ref JsonConverter propertyConverter, JsonContainerContract containerContract, JsonProperty containerProperty, JsonReader reader, object target, out bool useExistingValue, out object currentValue, out JsonContract propertyContract, out bool gottenCurrentValue)
		{
			currentValue = null;
			useExistingValue = false;
			propertyContract = null;
			gottenCurrentValue = false;
			if (property.Ignored)
			{
				return true;
			}
			JsonToken tokenType = reader.TokenType;
			if (property.PropertyContract == null)
			{
				property.PropertyContract = this.GetContractSafe(property.PropertyType);
			}
			ObjectCreationHandling valueOrDefault = property.ObjectCreationHandling.GetValueOrDefault(this.Serializer._objectCreationHandling);
			if (valueOrDefault != ObjectCreationHandling.Replace && (tokenType == JsonToken.StartArray || tokenType == JsonToken.StartObject) && property.Readable)
			{
				currentValue = property.ValueProvider.GetValue(target);
				gottenCurrentValue = true;
				if (currentValue != null)
				{
					propertyContract = this.GetContractSafe(currentValue.GetType());
					useExistingValue = (!propertyContract.IsReadOnlyOrFixedSize && !propertyContract.UnderlyingType.IsValueType());
				}
			}
			if (!property.Writable && !useExistingValue)
			{
				return true;
			}
			if (property.NullValueHandling.GetValueOrDefault(this.Serializer._nullValueHandling) == NullValueHandling.Ignore && tokenType == JsonToken.Null)
			{
				return true;
			}
			if (this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer._defaultValueHandling), DefaultValueHandling.Ignore) && !this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer._defaultValueHandling), DefaultValueHandling.Populate) && JsonTokenUtils.IsPrimitiveToken(tokenType) && MiscellaneousUtils.ValueEquals(reader.Value, property.GetResolvedDefaultValue()))
			{
				return true;
			}
			if (currentValue == null)
			{
				propertyContract = property.PropertyContract;
			}
			else
			{
				propertyContract = this.GetContractSafe(currentValue.GetType());
				if (propertyContract != property.PropertyContract)
				{
					propertyConverter = this.GetConverter(propertyContract, property.MemberConverter, containerContract, containerProperty);
				}
			}
			return false;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00023C58 File Offset: 0x00021E58
		private void AddReference(JsonReader reader, string id, object value)
		{
			try
			{
				if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
				{
					this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Read object reference Id '{0}' for {1}.".FormatWith(CultureInfo.InvariantCulture, id, value.GetType())), null);
				}
				this.Serializer.GetReferenceResolver().AddReference(this, id, value);
			}
			catch (Exception ex)
			{
				throw JsonSerializationException.Create(reader, "Error reading object reference '{0}'.".FormatWith(CultureInfo.InvariantCulture, id), ex);
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00023CF0 File Offset: 0x00021EF0
		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00023CF8 File Offset: 0x00021EF8
		private bool ShouldSetPropertyValue(JsonProperty property, object value)
		{
			return (property.NullValueHandling.GetValueOrDefault(this.Serializer._nullValueHandling) != NullValueHandling.Ignore || value != null) && (!this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer._defaultValueHandling), DefaultValueHandling.Ignore) || this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer._defaultValueHandling), DefaultValueHandling.Populate) || !MiscellaneousUtils.ValueEquals(value, property.GetResolvedDefaultValue())) && property.Writable;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00023D88 File Offset: 0x00021F88
		private IList CreateNewList(JsonReader reader, JsonArrayContract contract, out bool createdFromNonDefaultCreator)
		{
			if (!contract.CanDeserialize)
			{
				throw JsonSerializationException.Create(reader, "Cannot create and populate list type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.CreatedType));
			}
			if (contract.IsReadOnlyOrFixedSize)
			{
				createdFromNonDefaultCreator = true;
				IList list = contract.CreateTemporaryCollection();
				if (contract.ShouldCreateWrapper)
				{
					list = contract.CreateWrapper(list);
				}
				return list;
			}
			if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || this.Serializer._constructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				object obj = contract.DefaultCreator();
				if (contract.ShouldCreateWrapper)
				{
					obj = contract.CreateWrapper(obj);
				}
				createdFromNonDefaultCreator = false;
				return (IList)obj;
			}
			if (contract.HasParametrizedCreator)
			{
				createdFromNonDefaultCreator = true;
				return contract.CreateTemporaryCollection();
			}
			if (!contract.IsInstantiable)
			{
				throw JsonSerializationException.Create(reader, "Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
			}
			throw JsonSerializationException.Create(reader, "Unable to find a constructor to use for type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00023E74 File Offset: 0x00022074
		private IDictionary CreateNewDictionary(JsonReader reader, JsonDictionaryContract contract, out bool createdFromNonDefaultCreator)
		{
			if (contract.IsReadOnlyOrFixedSize)
			{
				createdFromNonDefaultCreator = true;
				return contract.CreateTemporaryDictionary();
			}
			if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || this.Serializer._constructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				object obj = contract.DefaultCreator();
				if (contract.ShouldCreateWrapper)
				{
					obj = contract.CreateWrapper(obj);
				}
				createdFromNonDefaultCreator = false;
				return (IDictionary)obj;
			}
			if (contract.HasParametrizedCreator)
			{
				createdFromNonDefaultCreator = true;
				return contract.CreateTemporaryDictionary();
			}
			if (!contract.IsInstantiable)
			{
				throw JsonSerializationException.Create(reader, "Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
			}
			throw JsonSerializationException.Create(reader, "Unable to find a default constructor to use for type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00023F28 File Offset: 0x00022128
		private void OnDeserializing(JsonReader reader, JsonContract contract, object value)
		{
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Started deserializing {0}".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			contract.InvokeOnDeserializing(value, this.Serializer._context);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00023F90 File Offset: 0x00022190
		private void OnDeserialized(JsonReader reader, JsonContract contract, object value)
		{
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Finished deserializing {0}".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			contract.InvokeOnDeserialized(value, this.Serializer._context);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00023FF8 File Offset: 0x000221F8
		private object PopulateDictionary(IDictionary dictionary, JsonReader reader, JsonDictionaryContract contract, JsonProperty containerProperty, string id)
		{
			IWrappedDictionary wrappedDictionary = dictionary as IWrappedDictionary;
			object obj = (wrappedDictionary != null) ? wrappedDictionary.UnderlyingDictionary : dictionary;
			if (id != null)
			{
				this.AddReference(reader, id, obj);
			}
			this.OnDeserializing(reader, contract, obj);
			int depth = reader.Depth;
			if (contract.KeyContract == null)
			{
				contract.KeyContract = this.GetContractSafe(contract.DictionaryKeyType);
			}
			if (contract.ItemContract == null)
			{
				contract.ItemContract = this.GetContractSafe(contract.DictionaryValueType);
			}
			JsonConverter jsonConverter = contract.ItemConverter ?? this.GetConverter(contract.ItemContract, null, contract, containerProperty);
			PrimitiveTypeCode primitiveTypeCode = (contract.KeyContract is JsonPrimitiveContract) ? ((JsonPrimitiveContract)contract.KeyContract).TypeCode : PrimitiveTypeCode.Empty;
			bool flag = false;
			for (;;)
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.PropertyName:
				{
					object obj2 = reader.Value;
					if (!this.CheckPropertyName(reader, obj2.ToString()))
					{
						try
						{
							try
							{
								DateParseHandling dateParseHandling;
								switch (primitiveTypeCode)
								{
								case PrimitiveTypeCode.DateTime:
								case PrimitiveTypeCode.DateTimeNullable:
									dateParseHandling = DateParseHandling.DateTime;
									break;
								case PrimitiveTypeCode.DateTimeOffset:
								case PrimitiveTypeCode.DateTimeOffsetNullable:
									dateParseHandling = DateParseHandling.DateTimeOffset;
									break;
								default:
									dateParseHandling = DateParseHandling.None;
									break;
								}
								object obj3;
								if (dateParseHandling != DateParseHandling.None && DateTimeUtils.TryParseDateTime(obj2.ToString(), dateParseHandling, reader.DateTimeZoneHandling, reader.DateFormatString, reader.Culture, out obj3))
								{
									obj2 = obj3;
								}
								else
								{
									obj2 = this.EnsureType(reader, obj2, CultureInfo.InvariantCulture, contract.KeyContract, contract.DictionaryKeyType);
								}
							}
							catch (Exception ex)
							{
								throw JsonSerializationException.Create(reader, "Could not convert string '{0}' to dictionary key type '{1}'. Create a TypeConverter to convert from the string to the key type object.".FormatWith(CultureInfo.InvariantCulture, reader.Value, contract.DictionaryKeyType), ex);
							}
							if (!this.ReadForType(reader, contract.ItemContract, jsonConverter != null))
							{
								throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
							}
							object value;
							if (jsonConverter != null && jsonConverter.CanRead)
							{
								value = this.DeserializeConvertable(jsonConverter, reader, contract.DictionaryValueType, null);
							}
							else
							{
								value = this.CreateValueInternal(reader, contract.DictionaryValueType, contract.ItemContract, null, contract, containerProperty, null);
							}
							dictionary[obj2] = value;
							break;
						}
						catch (Exception ex2)
						{
							if (base.IsErrorHandled(obj, contract, obj2, reader as IJsonLineInfo, reader.Path, ex2))
							{
								this.HandleError(reader, true, depth);
								break;
							}
							throw;
						}
						goto IL_218;
					}
					break;
				}
				case JsonToken.Comment:
					break;
				default:
					if (tokenType != JsonToken.EndObject)
					{
						goto Block_8;
					}
					goto IL_218;
				}
				IL_239:
				if (flag || !reader.Read())
				{
					goto IL_248;
				}
				continue;
				IL_218:
				flag = true;
				goto IL_239;
			}
			Block_8:
			throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
			IL_248:
			if (!flag)
			{
				this.ThrowUnexpectedEndException(reader, contract, obj, "Unexpected end when deserializing object.");
			}
			this.OnDeserialized(reader, contract, obj);
			return obj;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x000242A0 File Offset: 0x000224A0
		private object PopulateMultidimensionalArray(IList list, JsonReader reader, JsonArrayContract contract, JsonProperty containerProperty, string id)
		{
			int arrayRank = contract.UnderlyingType.GetArrayRank();
			if (id != null)
			{
				this.AddReference(reader, id, list);
			}
			this.OnDeserializing(reader, contract, list);
			JsonContract contractSafe = this.GetContractSafe(contract.CollectionItemType);
			JsonConverter converter = this.GetConverter(contractSafe, null, contract, containerProperty);
			int? num = null;
			Stack<IList> stack = new Stack<IList>();
			stack.Push(list);
			IList list2 = list;
			bool flag = false;
			for (;;)
			{
				int depth = reader.Depth;
				if (stack.Count == arrayRank)
				{
					try
					{
						if (this.ReadForType(reader, contractSafe, converter != null))
						{
							JsonToken tokenType = reader.TokenType;
							if (tokenType != JsonToken.Comment)
							{
								if (tokenType == JsonToken.EndArray)
								{
									stack.Pop();
									list2 = stack.Peek();
									num = null;
								}
								else
								{
									object value;
									if (converter != null && converter.CanRead)
									{
										value = this.DeserializeConvertable(converter, reader, contract.CollectionItemType, null);
									}
									else
									{
										value = this.CreateValueInternal(reader, contract.CollectionItemType, contractSafe, null, contract, containerProperty, null);
									}
									list2.Add(value);
								}
							}
							goto IL_201;
						}
						goto IL_208;
					}
					catch (Exception ex)
					{
						JsonPosition position = reader.GetPosition(depth);
						if (!base.IsErrorHandled(list, contract, position.Position, reader as IJsonLineInfo, reader.Path, ex))
						{
							throw;
						}
						this.HandleError(reader, true, depth);
						if (num != null && num == position.Position)
						{
							throw JsonSerializationException.Create(reader, "Infinite loop detected from error handling.", ex);
						}
						num = new int?(position.Position);
						goto IL_201;
					}
					goto IL_181;
				}
				goto IL_181;
				IL_201:
				if (flag)
				{
					goto IL_208;
				}
				continue;
				IL_181:
				if (!reader.Read())
				{
					goto IL_208;
				}
				JsonToken tokenType2 = reader.TokenType;
				if (tokenType2 == JsonToken.StartArray)
				{
					IList list3 = new List<object>();
					list2.Add(list3);
					stack.Push(list3);
					list2 = list3;
					goto IL_201;
				}
				if (tokenType2 == JsonToken.Comment)
				{
					goto IL_201;
				}
				if (tokenType2 != JsonToken.EndArray)
				{
					break;
				}
				stack.Pop();
				if (stack.Count > 0)
				{
					list2 = stack.Peek();
					goto IL_201;
				}
				flag = true;
				goto IL_201;
			}
			throw JsonSerializationException.Create(reader, "Unexpected token when deserializing multidimensional array: " + reader.TokenType);
			IL_208:
			if (!flag)
			{
				this.ThrowUnexpectedEndException(reader, contract, list, "Unexpected end when deserializing array.");
			}
			this.OnDeserialized(reader, contract, list);
			return list;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x000244E4 File Offset: 0x000226E4
		private void ThrowUnexpectedEndException(JsonReader reader, JsonContract contract, object currentObject, string message)
		{
			try
			{
				throw JsonSerializationException.Create(reader, message);
			}
			catch (Exception ex)
			{
				if (!base.IsErrorHandled(currentObject, contract, null, reader as IJsonLineInfo, reader.Path, ex))
				{
					throw;
				}
				this.HandleError(reader, false, 0);
			}
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00024534 File Offset: 0x00022734
		private object PopulateList(IList list, JsonReader reader, JsonArrayContract contract, JsonProperty containerProperty, string id)
		{
			IWrappedCollection wrappedCollection = list as IWrappedCollection;
			object obj = (wrappedCollection != null) ? wrappedCollection.UnderlyingCollection : list;
			if (id != null)
			{
				this.AddReference(reader, id, obj);
			}
			if (list.IsFixedSize)
			{
				reader.Skip();
				return obj;
			}
			this.OnDeserializing(reader, contract, obj);
			int depth = reader.Depth;
			if (contract.ItemContract == null)
			{
				contract.ItemContract = this.GetContractSafe(contract.CollectionItemType);
			}
			JsonConverter converter = this.GetConverter(contract.ItemContract, null, contract, containerProperty);
			int? num = null;
			bool flag = false;
			do
			{
				try
				{
					if (!this.ReadForType(reader, contract.ItemContract, converter != null))
					{
						break;
					}
					JsonToken tokenType = reader.TokenType;
					if (tokenType != JsonToken.Comment)
					{
						if (tokenType == JsonToken.EndArray)
						{
							flag = true;
						}
						else
						{
							object value;
							if (converter != null && converter.CanRead)
							{
								value = this.DeserializeConvertable(converter, reader, contract.CollectionItemType, null);
							}
							else
							{
								value = this.CreateValueInternal(reader, contract.CollectionItemType, contract.ItemContract, null, contract, containerProperty, null);
							}
							list.Add(value);
						}
					}
				}
				catch (Exception ex)
				{
					JsonPosition position = reader.GetPosition(depth);
					if (!base.IsErrorHandled(obj, contract, position.Position, reader as IJsonLineInfo, reader.Path, ex))
					{
						throw;
					}
					this.HandleError(reader, true, depth);
					if (num != null && num == position.Position)
					{
						throw JsonSerializationException.Create(reader, "Infinite loop detected from error handling.", ex);
					}
					num = new int?(position.Position);
				}
			}
			while (!flag);
			if (!flag)
			{
				this.ThrowUnexpectedEndException(reader, contract, obj, "Unexpected end when deserializing array.");
			}
			this.OnDeserialized(reader, contract, obj);
			return obj;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x000246F0 File Offset: 0x000228F0
		private object CreateISerializable(JsonReader reader, JsonISerializableContract contract, JsonProperty member, string id)
		{
			Type underlyingType = contract.UnderlyingType;
			if (!JsonTypeReflector.FullyTrusted)
			{
				string text = "Type '{0}' implements ISerializable but cannot be deserialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data." + Environment.NewLine + "To fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true." + Environment.NewLine;
				text = text.FormatWith(CultureInfo.InvariantCulture, underlyingType);
				throw JsonSerializationException.Create(reader, text);
			}
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Deserializing {0} using ISerializable constructor.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			SerializationInfo serializationInfo = new SerializationInfo(contract.UnderlyingType, new JsonFormatterConverter(this, contract, member));
			bool flag = false;
			string text2;
			for (;;)
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.PropertyName:
					text2 = reader.Value.ToString();
					if (!reader.Read())
					{
						goto Block_6;
					}
					serializationInfo.AddValue(text2, JToken.ReadFrom(reader));
					break;
				case JsonToken.Comment:
					break;
				default:
					if (tokenType != JsonToken.EndObject)
					{
						goto Block_5;
					}
					flag = true;
					break;
				}
				if (flag || !reader.Read())
				{
					goto IL_128;
				}
			}
			Block_5:
			throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
			Block_6:
			throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text2));
			IL_128:
			if (!flag)
			{
				this.ThrowUnexpectedEndException(reader, contract, serializationInfo, "Unexpected end when deserializing object.");
			}
			if (contract.ISerializableCreator == null)
			{
				throw JsonSerializationException.Create(reader, "ISerializable type '{0}' does not have a valid constructor. To correctly implement ISerializable a constructor that takes SerializationInfo and StreamingContext parameters should be present.".FormatWith(CultureInfo.InvariantCulture, underlyingType));
			}
			object obj = contract.ISerializableCreator(new object[]
			{
				serializationInfo,
				this.Serializer._context
			});
			if (id != null)
			{
				this.AddReference(reader, id, obj);
			}
			this.OnDeserializing(reader, contract, obj);
			this.OnDeserialized(reader, contract, obj);
			return obj;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x000248AC File Offset: 0x00022AAC
		internal object CreateISerializableItem(JToken token, Type type, JsonISerializableContract contract, JsonProperty member)
		{
			JsonContract contractSafe = this.GetContractSafe(type);
			JsonConverter converter = this.GetConverter(contractSafe, null, contract, member);
			JsonReader reader = token.CreateReader();
			this.CheckedRead(reader);
			object result;
			if (converter != null && converter.CanRead)
			{
				result = this.DeserializeConvertable(converter, reader, type, null);
			}
			else
			{
				result = this.CreateValueInternal(reader, type, contractSafe, null, contract, member, null);
			}
			return result;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00024904 File Offset: 0x00022B04
		private object CreateDynamic(JsonReader reader, JsonDynamicContract contract, JsonProperty member, string id)
		{
			if (!contract.IsInstantiable)
			{
				throw JsonSerializationException.Create(reader, "Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
			}
			if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || this.Serializer._constructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				IDynamicMetaObjectProvider dynamicMetaObjectProvider = (IDynamicMetaObjectProvider)contract.DefaultCreator();
				if (id != null)
				{
					this.AddReference(reader, id, dynamicMetaObjectProvider);
				}
				this.OnDeserializing(reader, contract, dynamicMetaObjectProvider);
				int depth = reader.Depth;
				bool flag = false;
				for (;;)
				{
					JsonToken tokenType = reader.TokenType;
					if (tokenType == JsonToken.PropertyName)
					{
						string text = reader.Value.ToString();
						try
						{
							if (!reader.Read())
							{
								throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text));
							}
							JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(text);
							if (closestMatchProperty != null && closestMatchProperty.Writable && !closestMatchProperty.Ignored)
							{
								if (closestMatchProperty.PropertyContract == null)
								{
									closestMatchProperty.PropertyContract = this.GetContractSafe(closestMatchProperty.PropertyType);
								}
								JsonConverter converter = this.GetConverter(closestMatchProperty.PropertyContract, closestMatchProperty.MemberConverter, null, null);
								if (!this.SetPropertyValue(closestMatchProperty, converter, null, member, reader, dynamicMetaObjectProvider))
								{
									reader.Skip();
								}
							}
							else
							{
								Type type = JsonTokenUtils.IsPrimitiveToken(reader.TokenType) ? reader.ValueType : typeof(IDynamicMetaObjectProvider);
								JsonContract contractSafe = this.GetContractSafe(type);
								JsonConverter converter2 = this.GetConverter(contractSafe, null, null, member);
								object value;
								if (converter2 != null && converter2.CanRead)
								{
									value = this.DeserializeConvertable(converter2, reader, type, null);
								}
								else
								{
									value = this.CreateValueInternal(reader, type, contractSafe, null, null, member, null);
								}
								contract.TrySetMember(dynamicMetaObjectProvider, text, value);
							}
							goto IL_205;
						}
						catch (Exception ex)
						{
							if (base.IsErrorHandled(dynamicMetaObjectProvider, contract, text, reader as IJsonLineInfo, reader.Path, ex))
							{
								this.HandleError(reader, true, depth);
								goto IL_205;
							}
							throw;
						}
						goto IL_1E5;
					}
					if (tokenType != JsonToken.EndObject)
					{
						break;
					}
					goto IL_1E5;
					IL_205:
					if (flag || !reader.Read())
					{
						goto IL_213;
					}
					continue;
					IL_1E5:
					flag = true;
					goto IL_205;
				}
				throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
				IL_213:
				if (!flag)
				{
					this.ThrowUnexpectedEndException(reader, contract, dynamicMetaObjectProvider, "Unexpected end when deserializing object.");
				}
				this.OnDeserialized(reader, contract, dynamicMetaObjectProvider);
				return dynamicMetaObjectProvider;
			}
			throw JsonSerializationException.Create(reader, "Unable to find a default constructor to use for type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00024B94 File Offset: 0x00022D94
		private object CreateObjectUsingCreatorWithParameters(JsonReader reader, JsonObjectContract contract, JsonProperty containerProperty, ObjectConstructor<object> creator, string id)
		{
			ValidationUtils.ArgumentNotNull(creator, "creator");
			Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> dictionary;
			if (!contract.HasRequiredOrDefaultValueProperties && !this.HasFlag(this.Serializer._defaultValueHandling, DefaultValueHandling.Populate))
			{
				dictionary = null;
			}
			else
			{
				dictionary = contract.Properties.ToDictionary((JsonProperty m) => m, (JsonProperty m) => JsonSerializerInternalReader.PropertyPresence.None);
			}
			Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> dictionary2 = dictionary;
			Type underlyingType = contract.UnderlyingType;
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				string arg = string.Join(", ", (from p in contract.CreatorParameters
				select p.PropertyName).ToArray<string>());
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Deserializing {0} using creator with parameters: {1}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType, arg)), null);
			}
			IDictionary<string, object> dictionary4;
			IDictionary<JsonProperty, object> dictionary3 = this.ResolvePropertyAndCreatorValues(contract, containerProperty, reader, underlyingType, out dictionary4);
			object[] array = new object[contract.CreatorParameters.Count];
			IDictionary<JsonProperty, object> dictionary5 = new Dictionary<JsonProperty, object>();
			foreach (KeyValuePair<JsonProperty, object> item in dictionary3)
			{
				JsonProperty property = item.Key;
				JsonProperty jsonProperty;
				if (contract.CreatorParameters.Contains(property))
				{
					jsonProperty = property;
				}
				else
				{
					jsonProperty = contract.CreatorParameters.ForgivingCaseSensitiveFind((JsonProperty p) => p.PropertyName, property.UnderlyingName);
				}
				if (jsonProperty != null)
				{
					int num = contract.CreatorParameters.IndexOf(jsonProperty);
					array[num] = item.Value;
				}
				else
				{
					dictionary5.Add(item);
				}
				if (dictionary2 != null)
				{
					JsonProperty jsonProperty2 = dictionary2.Keys.FirstOrDefault((JsonProperty p) => p.PropertyName == property.PropertyName);
					if (jsonProperty2 != null)
					{
						dictionary2[jsonProperty2] = ((item.Value == null) ? JsonSerializerInternalReader.PropertyPresence.Null : JsonSerializerInternalReader.PropertyPresence.Value);
					}
				}
			}
			object obj = creator(array);
			if (id != null)
			{
				this.AddReference(reader, id, obj);
			}
			this.OnDeserializing(reader, contract, obj);
			foreach (KeyValuePair<JsonProperty, object> keyValuePair in dictionary5)
			{
				JsonProperty key = keyValuePair.Key;
				object value = keyValuePair.Value;
				if (this.ShouldSetPropertyValue(key, value))
				{
					key.ValueProvider.SetValue(obj, value);
				}
				else if (!key.Writable && value != null)
				{
					JsonContract jsonContract = this.Serializer._contractResolver.ResolveContract(key.PropertyType);
					if (jsonContract.ContractType == JsonContractType.Array)
					{
						JsonArrayContract jsonArrayContract = (JsonArrayContract)jsonContract;
						object value2 = key.ValueProvider.GetValue(obj);
						if (value2 == null)
						{
							continue;
						}
						IWrappedCollection wrappedCollection = jsonArrayContract.CreateWrapper(value2);
						IWrappedCollection wrappedCollection2 = jsonArrayContract.CreateWrapper(value);
						using (IEnumerator enumerator3 = wrappedCollection2.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								object value3 = enumerator3.Current;
								wrappedCollection.Add(value3);
							}
							continue;
						}
					}
					if (jsonContract.ContractType == JsonContractType.Dictionary)
					{
						JsonDictionaryContract jsonDictionaryContract = (JsonDictionaryContract)jsonContract;
						object value4 = key.ValueProvider.GetValue(obj);
						if (value4 != null)
						{
							IDictionary dictionary6 = jsonDictionaryContract.ShouldCreateWrapper ? jsonDictionaryContract.CreateWrapper(value4) : ((IDictionary)value4);
							IDictionary dictionary7 = jsonDictionaryContract.ShouldCreateWrapper ? jsonDictionaryContract.CreateWrapper(value) : ((IDictionary)value);
							foreach (object obj2 in dictionary7)
							{
								DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
								dictionary6.Add(dictionaryEntry.Key, dictionaryEntry.Value);
							}
						}
					}
				}
			}
			if (dictionary4 != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair2 in dictionary4)
				{
					contract.ExtensionDataSetter(obj, keyValuePair2.Key, keyValuePair2.Value);
				}
			}
			this.EndObject(obj, reader, contract, reader.Depth, dictionary2);
			this.OnDeserialized(reader, contract, obj);
			return obj;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00025094 File Offset: 0x00023294
		private object DeserializeConvertable(JsonConverter converter, JsonReader reader, Type objectType, object existingValue)
		{
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Started deserializing {0} with converter {1}.".FormatWith(CultureInfo.InvariantCulture, objectType, converter.GetType())), null);
			}
			object result = converter.ReadJson(reader, objectType, existingValue, this.GetInternalSerializer());
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Finished deserializing {0} with converter {1}.".FormatWith(CultureInfo.InvariantCulture, objectType, converter.GetType())), null);
			}
			return result;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00025148 File Offset: 0x00023348
		private IDictionary<JsonProperty, object> ResolvePropertyAndCreatorValues(JsonObjectContract contract, JsonProperty containerProperty, JsonReader reader, Type objectType, out IDictionary<string, object> extensionData)
		{
			extensionData = ((contract.ExtensionDataSetter != null) ? new Dictionary<string, object>() : null);
			IDictionary<JsonProperty, object> dictionary = new Dictionary<JsonProperty, object>();
			bool flag = false;
			string text;
			for (;;)
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.PropertyName:
				{
					text = reader.Value.ToString();
					JsonProperty jsonProperty = contract.CreatorParameters.GetClosestMatchProperty(text) ?? contract.Properties.GetClosestMatchProperty(text);
					if (jsonProperty != null)
					{
						if (jsonProperty.PropertyContract == null)
						{
							jsonProperty.PropertyContract = this.GetContractSafe(jsonProperty.PropertyType);
						}
						JsonConverter converter = this.GetConverter(jsonProperty.PropertyContract, jsonProperty.MemberConverter, contract, containerProperty);
						if (!this.ReadForType(reader, jsonProperty.PropertyContract, converter != null))
						{
							goto Block_7;
						}
						if (!jsonProperty.Ignored)
						{
							if (jsonProperty.PropertyContract == null)
							{
								jsonProperty.PropertyContract = this.GetContractSafe(jsonProperty.PropertyType);
							}
							object value;
							if (converter != null && converter.CanRead)
							{
								value = this.DeserializeConvertable(converter, reader, jsonProperty.PropertyType, null);
							}
							else
							{
								value = this.CreateValueInternal(reader, jsonProperty.PropertyType, jsonProperty.PropertyContract, jsonProperty, contract, containerProperty, null);
							}
							dictionary[jsonProperty] = value;
							break;
						}
					}
					else
					{
						if (!reader.Read())
						{
							goto Block_12;
						}
						if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
						{
							this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Could not find member '{0}' on {1}.".FormatWith(CultureInfo.InvariantCulture, text, contract.UnderlyingType)), null);
						}
						if (this.Serializer._missingMemberHandling == MissingMemberHandling.Error)
						{
							goto Block_15;
						}
					}
					if (extensionData != null)
					{
						object value2 = this.CreateValueInternal(reader, null, null, null, contract, containerProperty, null);
						extensionData[text] = value2;
					}
					else
					{
						reader.Skip();
					}
					break;
				}
				case JsonToken.Comment:
					break;
				default:
					if (tokenType != JsonToken.EndObject)
					{
						goto Block_3;
					}
					flag = true;
					break;
				}
				if (flag || !reader.Read())
				{
					return dictionary;
				}
			}
			Block_3:
			throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
			Block_7:
			throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text));
			Block_12:
			throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text));
			Block_15:
			throw JsonSerializationException.Create(reader, "Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, text, objectType.Name));
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00025380 File Offset: 0x00023580
		private bool ReadForType(JsonReader reader, JsonContract contract, bool hasConverter)
		{
			if (hasConverter)
			{
				return reader.Read();
			}
			switch ((contract != null) ? contract.InternalReadType : ReadType.Read)
			{
			case ReadType.Read:
				while (reader.Read())
				{
					if (reader.TokenType != JsonToken.Comment)
					{
						return true;
					}
				}
				return false;
			case ReadType.ReadAsInt32:
				reader.ReadAsInt32();
				break;
			case ReadType.ReadAsBytes:
				reader.ReadAsBytes();
				break;
			case ReadType.ReadAsString:
				reader.ReadAsString();
				break;
			case ReadType.ReadAsDecimal:
				reader.ReadAsDecimal();
				break;
			case ReadType.ReadAsDateTime:
				reader.ReadAsDateTime();
				break;
			case ReadType.ReadAsDateTimeOffset:
				reader.ReadAsDateTimeOffset();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return reader.TokenType != JsonToken.None;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00025428 File Offset: 0x00023628
		public object CreateNewObject(JsonReader reader, JsonObjectContract objectContract, JsonProperty containerMember, JsonProperty containerProperty, string id, out bool createdFromNonDefaultCreator)
		{
			object obj = null;
			if (objectContract.OverrideCreator != null)
			{
				if (objectContract.CreatorParameters.Count > 0)
				{
					createdFromNonDefaultCreator = true;
					return this.CreateObjectUsingCreatorWithParameters(reader, objectContract, containerMember, objectContract.OverrideCreator, id);
				}
				obj = objectContract.OverrideCreator(new object[0]);
			}
			else if (objectContract.DefaultCreator != null && (!objectContract.DefaultCreatorNonPublic || this.Serializer._constructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor || objectContract.ParametrizedCreator == null))
			{
				obj = objectContract.DefaultCreator();
			}
			else if (objectContract.ParametrizedCreator != null)
			{
				createdFromNonDefaultCreator = true;
				return this.CreateObjectUsingCreatorWithParameters(reader, objectContract, containerMember, objectContract.ParametrizedCreator, id);
			}
			if (obj != null)
			{
				createdFromNonDefaultCreator = false;
				return obj;
			}
			if (!objectContract.IsInstantiable)
			{
				throw JsonSerializationException.Create(reader, "Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, objectContract.UnderlyingType));
			}
			throw JsonSerializationException.Create(reader, "Unable to find a constructor to use for type {0}. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute.".FormatWith(CultureInfo.InvariantCulture, objectContract.UnderlyingType));
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00025518 File Offset: 0x00023718
		private object PopulateObject(object newObject, JsonReader reader, JsonObjectContract contract, JsonProperty member, string id)
		{
			this.OnDeserializing(reader, contract, newObject);
			Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> dictionary;
			if (!contract.HasRequiredOrDefaultValueProperties && !this.HasFlag(this.Serializer._defaultValueHandling, DefaultValueHandling.Populate))
			{
				dictionary = null;
			}
			else
			{
				dictionary = contract.Properties.ToDictionary((JsonProperty m) => m, (JsonProperty m) => JsonSerializerInternalReader.PropertyPresence.None);
			}
			Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> dictionary2 = dictionary;
			if (id != null)
			{
				this.AddReference(reader, id, newObject);
			}
			int depth = reader.Depth;
			bool flag = false;
			for (;;)
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.PropertyName:
				{
					string text = reader.Value.ToString();
					if (!this.CheckPropertyName(reader, text))
					{
						try
						{
							JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(text);
							if (closestMatchProperty == null)
							{
								if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
								{
									this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Could not find member '{0}' on {1}".FormatWith(CultureInfo.InvariantCulture, text, contract.UnderlyingType)), null);
								}
								if (this.Serializer._missingMemberHandling == MissingMemberHandling.Error)
								{
									throw JsonSerializationException.Create(reader, "Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, text, contract.UnderlyingType.Name));
								}
								if (!reader.Read())
								{
									break;
								}
								this.SetExtensionData(contract, member, reader, text, newObject);
								break;
							}
							else
							{
								if (closestMatchProperty.PropertyContract == null)
								{
									closestMatchProperty.PropertyContract = this.GetContractSafe(closestMatchProperty.PropertyType);
								}
								JsonConverter converter = this.GetConverter(closestMatchProperty.PropertyContract, closestMatchProperty.MemberConverter, contract, member);
								if (!this.ReadForType(reader, closestMatchProperty.PropertyContract, converter != null))
								{
									throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text));
								}
								this.SetPropertyPresence(reader, closestMatchProperty, dictionary2);
								if (!this.SetPropertyValue(closestMatchProperty, converter, contract, member, reader, newObject))
								{
									this.SetExtensionData(contract, member, reader, text, newObject);
								}
								break;
							}
						}
						catch (Exception ex)
						{
							if (base.IsErrorHandled(newObject, contract, text, reader as IJsonLineInfo, reader.Path, ex))
							{
								this.HandleError(reader, true, depth);
								break;
							}
							throw;
						}
						goto IL_22A;
					}
					break;
				}
				case JsonToken.Comment:
					break;
				default:
					if (tokenType != JsonToken.EndObject)
					{
						goto Block_7;
					}
					goto IL_22A;
				}
				IL_24A:
				if (flag || !reader.Read())
				{
					goto IL_258;
				}
				continue;
				IL_22A:
				flag = true;
				goto IL_24A;
			}
			Block_7:
			throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
			IL_258:
			if (!flag)
			{
				this.ThrowUnexpectedEndException(reader, contract, newObject, "Unexpected end when deserializing object.");
			}
			this.EndObject(newObject, reader, contract, depth, dictionary2);
			this.OnDeserialized(reader, contract, newObject);
			return newObject;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x000257C0 File Offset: 0x000239C0
		private bool CheckPropertyName(JsonReader reader, string memberName)
		{
			if (this.Serializer.MetadataPropertyHandling == MetadataPropertyHandling.ReadAhead && memberName != null && (memberName == "$id" || memberName == "$ref" || memberName == "$type" || memberName == "$values"))
			{
				reader.Skip();
				return true;
			}
			return false;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00025820 File Offset: 0x00023A20
		private void SetExtensionData(JsonObjectContract contract, JsonProperty member, JsonReader reader, string memberName, object o)
		{
			if (contract.ExtensionDataSetter != null)
			{
				try
				{
					object value = this.CreateValueInternal(reader, null, null, null, contract, member, null);
					contract.ExtensionDataSetter(o, memberName, value);
					return;
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error setting value in extension data for type '{0}'.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType), ex);
				}
			}
			reader.Skip();
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002588C File Offset: 0x00023A8C
		private void EndObject(object newObject, JsonReader reader, JsonObjectContract contract, int initialDepth, Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> propertiesPresence)
		{
			if (propertiesPresence != null)
			{
				foreach (KeyValuePair<JsonProperty, JsonSerializerInternalReader.PropertyPresence> keyValuePair in propertiesPresence)
				{
					JsonProperty key = keyValuePair.Key;
					JsonSerializerInternalReader.PropertyPresence value = keyValuePair.Value;
					if (value != JsonSerializerInternalReader.PropertyPresence.None)
					{
						if (value != JsonSerializerInternalReader.PropertyPresence.Null)
						{
							continue;
						}
					}
					try
					{
						Required required = key._required ?? (contract.ItemRequired ?? Required.Default);
						switch (value)
						{
						case JsonSerializerInternalReader.PropertyPresence.None:
							if (required == Required.AllowNull || required == Required.Always)
							{
								throw JsonSerializationException.Create(reader, "Required property '{0}' not found in JSON.".FormatWith(CultureInfo.InvariantCulture, key.PropertyName));
							}
							if (key.PropertyContract == null)
							{
								key.PropertyContract = this.GetContractSafe(key.PropertyType);
							}
							if (this.HasFlag(key.DefaultValueHandling.GetValueOrDefault(this.Serializer._defaultValueHandling), DefaultValueHandling.Populate) && key.Writable && !key.Ignored)
							{
								key.ValueProvider.SetValue(newObject, this.EnsureType(reader, key.GetResolvedDefaultValue(), CultureInfo.InvariantCulture, key.PropertyContract, key.PropertyType));
							}
							break;
						case JsonSerializerInternalReader.PropertyPresence.Null:
							if (required == Required.Always)
							{
								throw JsonSerializationException.Create(reader, "Required property '{0}' expects a value but got null.".FormatWith(CultureInfo.InvariantCulture, key.PropertyName));
							}
							break;
						}
					}
					catch (Exception ex)
					{
						if (!base.IsErrorHandled(newObject, contract, key.PropertyName, reader as IJsonLineInfo, reader.Path, ex))
						{
							throw;
						}
						this.HandleError(reader, true, initialDepth);
					}
				}
			}
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00025A5C File Offset: 0x00023C5C
		private void SetPropertyPresence(JsonReader reader, JsonProperty property, Dictionary<JsonProperty, JsonSerializerInternalReader.PropertyPresence> requiredProperties)
		{
			if (property != null && requiredProperties != null)
			{
				requiredProperties[property] = ((reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.Undefined) ? JsonSerializerInternalReader.PropertyPresence.Null : JsonSerializerInternalReader.PropertyPresence.Value);
			}
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00025A83 File Offset: 0x00023C83
		private void HandleError(JsonReader reader, bool readPastError, int initialDepth)
		{
			base.ClearErrorContext();
			if (readPastError)
			{
				reader.Skip();
				while (reader.Depth > initialDepth + 1)
				{
					if (!reader.Read())
					{
						return;
					}
				}
			}
		}

		// Token: 0x0400034E RID: 846
		private JsonSerializerProxy _internalSerializer;

		// Token: 0x020000C3 RID: 195
		internal enum PropertyPresence
		{
			// Token: 0x04000356 RID: 854
			None,
			// Token: 0x04000357 RID: 855
			Null,
			// Token: 0x04000358 RID: 856
			Value
		}
	}
}
