using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C4 RID: 196
	internal class JsonSerializerInternalWriter : JsonSerializerInternalBase
	{
		// Token: 0x06000990 RID: 2448 RVA: 0x00025AAA File Offset: 0x00023CAA
		public JsonSerializerInternalWriter(JsonSerializer serializer) : base(serializer)
		{
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00025AC0 File Offset: 0x00023CC0
		public void Serialize(JsonWriter jsonWriter, object value, Type objectType)
		{
			if (jsonWriter == null)
			{
				throw new ArgumentNullException("jsonWriter");
			}
			this._rootContract = ((objectType != null) ? this.Serializer._contractResolver.ResolveContract(objectType) : null);
			this._rootLevel = this._serializeStack.Count + 1;
			JsonContract contractSafe = this.GetContractSafe(value);
			try
			{
				this.SerializeValue(jsonWriter, value, contractSafe, null, null, null);
			}
			catch (Exception ex)
			{
				if (!base.IsErrorHandled(null, contractSafe, null, null, jsonWriter.Path, ex))
				{
					base.ClearErrorContext();
					throw;
				}
				this.HandleError(jsonWriter, 0);
			}
			finally
			{
				this._rootContract = null;
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00025B78 File Offset: 0x00023D78
		private JsonSerializerProxy GetInternalSerializer()
		{
			if (this._internalSerializer == null)
			{
				this._internalSerializer = new JsonSerializerProxy(this);
			}
			return this._internalSerializer;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00025B94 File Offset: 0x00023D94
		private JsonContract GetContractSafe(object value)
		{
			if (value == null)
			{
				return null;
			}
			return this.Serializer._contractResolver.ResolveContract(value.GetType());
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00025BB4 File Offset: 0x00023DB4
		private void SerializePrimitive(JsonWriter writer, object value, JsonPrimitiveContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			if (contract.TypeCode == PrimitiveTypeCode.Bytes)
			{
				bool flag = this.ShouldWriteType(TypeNameHandling.Objects, contract, member, containerContract, containerProperty);
				if (flag)
				{
					writer.WriteStartObject();
					this.WriteTypeProperty(writer, contract.CreatedType);
					writer.WritePropertyName("$value", false);
					JsonWriter.WriteValue(writer, contract.TypeCode, value);
					writer.WriteEndObject();
					return;
				}
			}
			JsonWriter.WriteValue(writer, contract.TypeCode, value);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00025C20 File Offset: 0x00023E20
		private void SerializeValue(JsonWriter writer, object value, JsonContract valueContract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			JsonConverter jsonConverter;
			if ((jsonConverter = ((member != null) ? member.Converter : null)) == null && (jsonConverter = ((containerProperty != null) ? containerProperty.ItemConverter : null)) == null && (jsonConverter = ((containerContract != null) ? containerContract.ItemConverter : null)) == null && (jsonConverter = valueContract.Converter) == null)
			{
				jsonConverter = (this.Serializer.GetMatchingConverter(valueContract.UnderlyingType) ?? valueContract.InternalConverter);
			}
			JsonConverter jsonConverter2 = jsonConverter;
			if (jsonConverter2 != null && jsonConverter2.CanWrite)
			{
				this.SerializeConvertable(writer, jsonConverter2, value, valueContract, containerContract, containerProperty);
				return;
			}
			switch (valueContract.ContractType)
			{
			case JsonContractType.Object:
				this.SerializeObject(writer, value, (JsonObjectContract)valueContract, member, containerContract, containerProperty);
				return;
			case JsonContractType.Array:
			{
				JsonArrayContract jsonArrayContract = (JsonArrayContract)valueContract;
				if (!jsonArrayContract.IsMultidimensionalArray)
				{
					this.SerializeList(writer, (IEnumerable)value, jsonArrayContract, member, containerContract, containerProperty);
					return;
				}
				this.SerializeMultidimensionalArray(writer, (Array)value, jsonArrayContract, member, containerContract, containerProperty);
				return;
			}
			case JsonContractType.Primitive:
				this.SerializePrimitive(writer, value, (JsonPrimitiveContract)valueContract, member, containerContract, containerProperty);
				return;
			case JsonContractType.String:
				this.SerializeString(writer, value, (JsonStringContract)valueContract);
				return;
			case JsonContractType.Dictionary:
			{
				JsonDictionaryContract jsonDictionaryContract = (JsonDictionaryContract)valueContract;
				this.SerializeDictionary(writer, (value is IDictionary) ? ((IDictionary)value) : jsonDictionaryContract.CreateWrapper(value), jsonDictionaryContract, member, containerContract, containerProperty);
				return;
			}
			case JsonContractType.Dynamic:
				this.SerializeDynamic(writer, (IDynamicMetaObjectProvider)value, (JsonDynamicContract)valueContract, member, containerContract, containerProperty);
				return;
			case JsonContractType.Serializable:
				this.SerializeISerializable(writer, (ISerializable)value, (JsonISerializableContract)valueContract, member, containerContract, containerProperty);
				return;
			case JsonContractType.Linq:
				((JToken)value).WriteTo(writer, this.Serializer.Converters.ToArray<JsonConverter>());
				return;
			default:
				return;
			}
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00025DCC File Offset: 0x00023FCC
		private bool? ResolveIsReference(JsonContract contract, JsonProperty property, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			bool? result = null;
			if (property != null)
			{
				result = property.IsReference;
			}
			if (result == null && containerProperty != null)
			{
				result = containerProperty.ItemIsReference;
			}
			if (result == null && collectionContract != null)
			{
				result = collectionContract.ItemIsReference;
			}
			if (result == null)
			{
				result = contract.IsReference;
			}
			return result;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00025E24 File Offset: 0x00024024
		private bool ShouldWriteReference(object value, JsonProperty property, JsonContract valueContract, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			if (value == null)
			{
				return false;
			}
			if (valueContract.ContractType == JsonContractType.Primitive || valueContract.ContractType == JsonContractType.String)
			{
				return false;
			}
			bool? flag = this.ResolveIsReference(valueContract, property, collectionContract, containerProperty);
			if (flag == null)
			{
				if (valueContract.ContractType == JsonContractType.Array)
				{
					flag = new bool?(this.HasFlag(this.Serializer._preserveReferencesHandling, PreserveReferencesHandling.Arrays));
				}
				else
				{
					flag = new bool?(this.HasFlag(this.Serializer._preserveReferencesHandling, PreserveReferencesHandling.Objects));
				}
			}
			return flag.Value && this.Serializer.GetReferenceResolver().IsReferenced(this, value);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00025EBC File Offset: 0x000240BC
		private bool ShouldWriteProperty(object memberValue, JsonProperty property)
		{
			return (property.NullValueHandling.GetValueOrDefault(this.Serializer._nullValueHandling) != NullValueHandling.Ignore || memberValue != null) && (!this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer._defaultValueHandling), DefaultValueHandling.Ignore) || !MiscellaneousUtils.ValueEquals(memberValue, property.GetResolvedDefaultValue()));
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00025F20 File Offset: 0x00024120
		private bool CheckForCircularReference(JsonWriter writer, object value, JsonProperty property, JsonContract contract, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			if (value == null || contract.ContractType == JsonContractType.Primitive || contract.ContractType == JsonContractType.String)
			{
				return true;
			}
			ReferenceLoopHandling? referenceLoopHandling = null;
			if (property != null)
			{
				referenceLoopHandling = property.ReferenceLoopHandling;
			}
			if (referenceLoopHandling == null && containerProperty != null)
			{
				referenceLoopHandling = containerProperty.ItemReferenceLoopHandling;
			}
			if (referenceLoopHandling == null && containerContract != null)
			{
				referenceLoopHandling = containerContract.ItemReferenceLoopHandling;
			}
			if (this._serializeStack.IndexOf(value) != -1)
			{
				string text = "Self referencing loop detected";
				if (property != null)
				{
					text += " for property '{0}'".FormatWith(CultureInfo.InvariantCulture, property.PropertyName);
				}
				text += " with type '{0}'.".FormatWith(CultureInfo.InvariantCulture, value.GetType());
				switch (referenceLoopHandling.GetValueOrDefault(this.Serializer._referenceLoopHandling))
				{
				case ReferenceLoopHandling.Error:
					throw JsonSerializationException.Create(null, writer.ContainerPath, text, null);
				case ReferenceLoopHandling.Ignore:
					if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
					{
						this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, text + ". Skipping serializing self referenced value."), null);
					}
					return false;
				case ReferenceLoopHandling.Serialize:
					if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
					{
						this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, text + ". Serializing self referenced value."), null);
					}
					return true;
				}
			}
			return true;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00026088 File Offset: 0x00024288
		private void WriteReference(JsonWriter writer, object value)
		{
			string reference = this.GetReference(writer, value);
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Writing object reference to Id '{0}' for {1}.".FormatWith(CultureInfo.InvariantCulture, reference, value.GetType())), null);
			}
			writer.WriteStartObject();
			writer.WritePropertyName("$ref", false);
			writer.WriteValue(reference);
			writer.WriteEndObject();
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00026104 File Offset: 0x00024304
		private string GetReference(JsonWriter writer, object value)
		{
			string result;
			try
			{
				string reference = this.Serializer.GetReferenceResolver().GetReference(this, value);
				result = reference;
			}
			catch (Exception ex)
			{
				throw JsonSerializationException.Create(null, writer.ContainerPath, "Error writing object reference for '{0}'.".FormatWith(CultureInfo.InvariantCulture, value.GetType()), ex);
			}
			return result;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00026160 File Offset: 0x00024360
		internal static bool TryConvertToString(object value, Type type, out string s)
		{
			TypeConverter converter = ConvertUtils.GetConverter(type);
			if (converter != null && !(converter is ComponentConverter) && converter.GetType() != typeof(TypeConverter) && converter.CanConvertTo(typeof(string)))
			{
				s = converter.ConvertToInvariantString(value);
				return true;
			}
			if (value is Type)
			{
				s = ((Type)value).AssemblyQualifiedName;
				return true;
			}
			s = null;
			return false;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x000261D0 File Offset: 0x000243D0
		private void SerializeString(JsonWriter writer, object value, JsonStringContract contract)
		{
			this.OnSerializing(writer, contract, value);
			string value2;
			JsonSerializerInternalWriter.TryConvertToString(value, contract.UnderlyingType, out value2);
			writer.WriteValue(value2);
			this.OnSerialized(writer, contract, value);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00026208 File Offset: 0x00024408
		private void OnSerializing(JsonWriter writer, JsonContract contract, object value)
		{
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Started serializing {0}".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			contract.InvokeOnSerializing(value, this.Serializer._context);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0002626C File Offset: 0x0002446C
		private void OnSerialized(JsonWriter writer, JsonContract contract, object value)
		{
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Finished serializing {0}".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			contract.InvokeOnSerialized(value, this.Serializer._context);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x000262D0 File Offset: 0x000244D0
		private void SerializeObject(JsonWriter writer, object value, JsonObjectContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			this.OnSerializing(writer, contract, value);
			this._serializeStack.Add(value);
			this.WriteObjectStart(writer, value, contract, member, collectionContract, containerProperty);
			int top = writer.Top;
			for (int i = 0; i < contract.Properties.Count; i++)
			{
				JsonProperty jsonProperty = contract.Properties[i];
				try
				{
					JsonContract valueContract;
					object value2;
					if (this.CalculatePropertyValues(writer, value, contract, member, jsonProperty, out valueContract, out value2))
					{
						jsonProperty.WritePropertyName(writer);
						this.SerializeValue(writer, value2, valueContract, jsonProperty, contract, member);
					}
				}
				catch (Exception ex)
				{
					if (!base.IsErrorHandled(value, contract, jsonProperty.PropertyName, null, writer.ContainerPath, ex))
					{
						throw;
					}
					this.HandleError(writer, top);
				}
			}
			if (contract.ExtensionDataGetter != null)
			{
				IEnumerable<KeyValuePair<object, object>> enumerable = contract.ExtensionDataGetter(value);
				if (enumerable != null)
				{
					foreach (KeyValuePair<object, object> keyValuePair in enumerable)
					{
						JsonContract contractSafe = this.GetContractSafe(keyValuePair.Key);
						JsonContract contractSafe2 = this.GetContractSafe(keyValuePair.Value);
						bool flag;
						string propertyName = this.GetPropertyName(writer, keyValuePair.Key, contractSafe, out flag);
						if (this.ShouldWriteReference(keyValuePair.Value, null, contractSafe2, contract, member))
						{
							writer.WritePropertyName(propertyName);
							this.WriteReference(writer, keyValuePair.Value);
						}
						else if (this.CheckForCircularReference(writer, keyValuePair.Value, null, contractSafe2, contract, member))
						{
							writer.WritePropertyName(propertyName);
							this.SerializeValue(writer, keyValuePair.Value, contractSafe2, null, contract, member);
						}
					}
				}
			}
			writer.WriteEndObject();
			this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
			this.OnSerialized(writer, contract, value);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x000264A4 File Offset: 0x000246A4
		private bool CalculatePropertyValues(JsonWriter writer, object value, JsonContainerContract contract, JsonProperty member, JsonProperty property, out JsonContract memberContract, out object memberValue)
		{
			if (!property.Ignored && property.Readable && this.ShouldSerialize(writer, property, value) && this.IsSpecified(writer, property, value))
			{
				if (property.PropertyContract == null)
				{
					property.PropertyContract = this.Serializer._contractResolver.ResolveContract(property.PropertyType);
				}
				memberValue = property.ValueProvider.GetValue(value);
				memberContract = (property.PropertyContract.IsSealed ? property.PropertyContract : this.GetContractSafe(memberValue));
				if (this.ShouldWriteProperty(memberValue, property))
				{
					if (this.ShouldWriteReference(memberValue, property, memberContract, contract, member))
					{
						property.WritePropertyName(writer);
						this.WriteReference(writer, memberValue);
						return false;
					}
					if (!this.CheckForCircularReference(writer, memberValue, property, memberContract, contract, member))
					{
						return false;
					}
					if (memberValue == null)
					{
						JsonObjectContract jsonObjectContract = contract as JsonObjectContract;
						Required required = property._required ?? (((jsonObjectContract != null) ? jsonObjectContract.ItemRequired : null) ?? Required.Default);
						if (required == Required.Always)
						{
							throw JsonSerializationException.Create(null, writer.ContainerPath, "Cannot write a null value for property '{0}'. Property requires a value.".FormatWith(CultureInfo.InvariantCulture, property.PropertyName), null);
						}
					}
					return true;
				}
			}
			memberContract = null;
			memberValue = null;
			return false;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00026614 File Offset: 0x00024814
		private void WriteObjectStart(JsonWriter writer, object value, JsonContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			writer.WriteStartObject();
			bool flag = this.ResolveIsReference(contract, member, collectionContract, containerProperty) ?? this.HasFlag(this.Serializer._preserveReferencesHandling, PreserveReferencesHandling.Objects);
			if (flag)
			{
				this.WriteReferenceIdProperty(writer, contract.UnderlyingType, value);
			}
			if (this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionContract, containerProperty))
			{
				this.WriteTypeProperty(writer, contract.UnderlyingType);
			}
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00026688 File Offset: 0x00024888
		private void WriteReferenceIdProperty(JsonWriter writer, Type type, object value)
		{
			string reference = this.GetReference(writer, value);
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "Writing object reference Id '{0}' for {1}.".FormatWith(CultureInfo.InvariantCulture, reference, type)), null);
			}
			writer.WritePropertyName("$id", false);
			writer.WriteValue(reference);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000266F4 File Offset: 0x000248F4
		private void WriteTypeProperty(JsonWriter writer, Type type)
		{
			string typeName = ReflectionUtils.GetTypeName(type, this.Serializer._typeNameAssemblyFormat, this.Serializer._binder);
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "Writing type name '{0}' for {1}.".FormatWith(CultureInfo.InvariantCulture, typeName, type)), null);
			}
			writer.WritePropertyName("$type", false);
			writer.WriteValue(typeName);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00026771 File Offset: 0x00024971
		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00026779 File Offset: 0x00024979
		private bool HasFlag(PreserveReferencesHandling value, PreserveReferencesHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00026781 File Offset: 0x00024981
		private bool HasFlag(TypeNameHandling value, TypeNameHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0002678C File Offset: 0x0002498C
		private void SerializeConvertable(JsonWriter writer, JsonConverter converter, object value, JsonContract contract, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			if (this.ShouldWriteReference(value, null, contract, collectionContract, containerProperty))
			{
				this.WriteReference(writer, value);
				return;
			}
			if (!this.CheckForCircularReference(writer, value, null, contract, collectionContract, containerProperty))
			{
				return;
			}
			this._serializeStack.Add(value);
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Started serializing {0} with converter {1}.".FormatWith(CultureInfo.InvariantCulture, value.GetType(), converter.GetType())), null);
			}
			converter.WriteJson(writer, value, this.GetInternalSerializer());
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Finished serializing {0} with converter {1}.".FormatWith(CultureInfo.InvariantCulture, value.GetType(), converter.GetType())), null);
			}
			this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0002688C File Offset: 0x00024A8C
		private void SerializeList(JsonWriter writer, IEnumerable values, JsonArrayContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			IWrappedCollection wrappedCollection = values as IWrappedCollection;
			object obj = (wrappedCollection != null) ? wrappedCollection.UnderlyingCollection : values;
			this.OnSerializing(writer, contract, obj);
			this._serializeStack.Add(obj);
			bool flag = this.WriteStartArray(writer, obj, contract, member, collectionContract, containerProperty);
			writer.WriteStartArray();
			int top = writer.Top;
			int num = 0;
			foreach (object value in values)
			{
				try
				{
					JsonContract jsonContract = contract.FinalItemContract ?? this.GetContractSafe(value);
					if (this.ShouldWriteReference(value, null, jsonContract, contract, member))
					{
						this.WriteReference(writer, value);
					}
					else if (this.CheckForCircularReference(writer, value, null, jsonContract, contract, member))
					{
						this.SerializeValue(writer, value, jsonContract, null, contract, member);
					}
				}
				catch (Exception ex)
				{
					if (!base.IsErrorHandled(obj, contract, num, null, writer.ContainerPath, ex))
					{
						throw;
					}
					this.HandleError(writer, top);
				}
				finally
				{
					num++;
				}
			}
			writer.WriteEndArray();
			if (flag)
			{
				writer.WriteEndObject();
			}
			this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
			this.OnSerialized(writer, contract, obj);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x000269F8 File Offset: 0x00024BF8
		private void SerializeMultidimensionalArray(JsonWriter writer, Array values, JsonArrayContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			this.OnSerializing(writer, contract, values);
			this._serializeStack.Add(values);
			bool flag = this.WriteStartArray(writer, values, contract, member, collectionContract, containerProperty);
			this.SerializeMultidimensionalArray(writer, values, contract, member, writer.Top, new int[0]);
			if (flag)
			{
				writer.WriteEndObject();
			}
			this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
			this.OnSerialized(writer, contract, values);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00026A6C File Offset: 0x00024C6C
		private void SerializeMultidimensionalArray(JsonWriter writer, Array values, JsonArrayContract contract, JsonProperty member, int initialDepth, int[] indices)
		{
			int num = indices.Length;
			int[] array = new int[num + 1];
			for (int i = 0; i < num; i++)
			{
				array[i] = indices[i];
			}
			writer.WriteStartArray();
			int j = 0;
			while (j < values.GetLength(num))
			{
				array[num] = j;
				bool flag = array.Length == values.Rank;
				if (flag)
				{
					object value = values.GetValue(array);
					try
					{
						JsonContract jsonContract = contract.FinalItemContract ?? this.GetContractSafe(value);
						if (this.ShouldWriteReference(value, null, jsonContract, contract, member))
						{
							this.WriteReference(writer, value);
						}
						else if (this.CheckForCircularReference(writer, value, null, jsonContract, contract, member))
						{
							this.SerializeValue(writer, value, jsonContract, null, contract, member);
						}
						goto IL_DC;
					}
					catch (Exception ex)
					{
						if (base.IsErrorHandled(values, contract, j, null, writer.ContainerPath, ex))
						{
							this.HandleError(writer, initialDepth + 1);
							goto IL_DC;
						}
						throw;
					}
					goto IL_CC;
				}
				goto IL_CC;
				IL_DC:
				j++;
				continue;
				IL_CC:
				this.SerializeMultidimensionalArray(writer, values, contract, member, initialDepth + 1, array);
				goto IL_DC;
			}
			writer.WriteEndArray();
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00026B7C File Offset: 0x00024D7C
		private bool WriteStartArray(JsonWriter writer, object values, JsonArrayContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			bool flag = this.ResolveIsReference(contract, member, containerContract, containerProperty) ?? this.HasFlag(this.Serializer._preserveReferencesHandling, PreserveReferencesHandling.Arrays);
			bool flag2 = this.ShouldWriteType(TypeNameHandling.Arrays, contract, member, containerContract, containerProperty);
			bool flag3 = flag || flag2;
			if (flag3)
			{
				writer.WriteStartObject();
				if (flag)
				{
					this.WriteReferenceIdProperty(writer, contract.UnderlyingType, values);
				}
				if (flag2)
				{
					this.WriteTypeProperty(writer, values.GetType());
				}
				writer.WritePropertyName("$values", false);
			}
			if (contract.ItemContract == null)
			{
				contract.ItemContract = this.Serializer._contractResolver.ResolveContract(contract.CollectionItemType ?? typeof(object));
			}
			return flag3;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00026C3C File Offset: 0x00024E3C
		[SecuritySafeCritical]
		private void SerializeISerializable(JsonWriter writer, ISerializable value, JsonISerializableContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				string text = "Type '{0}' implements ISerializable but cannot be serialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data." + Environment.NewLine + "To fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true." + Environment.NewLine;
				text = text.FormatWith(CultureInfo.InvariantCulture, value.GetType());
				throw JsonSerializationException.Create(null, writer.ContainerPath, text, null);
			}
			this.OnSerializing(writer, contract, value);
			this._serializeStack.Add(value);
			this.WriteObjectStart(writer, value, contract, member, collectionContract, containerProperty);
			SerializationInfo serializationInfo = new SerializationInfo(contract.UnderlyingType, new FormatterConverter());
			value.GetObjectData(serializationInfo, this.Serializer._context);
			foreach (SerializationEntry serializationEntry in serializationInfo)
			{
				JsonContract contractSafe = this.GetContractSafe(serializationEntry.Value);
				if (this.ShouldWriteReference(serializationEntry.Value, null, contractSafe, contract, member))
				{
					writer.WritePropertyName(serializationEntry.Name);
					this.WriteReference(writer, serializationEntry.Value);
				}
				else if (this.CheckForCircularReference(writer, serializationEntry.Value, null, contractSafe, contract, member))
				{
					writer.WritePropertyName(serializationEntry.Name);
					this.SerializeValue(writer, serializationEntry.Value, contractSafe, null, contract, member);
				}
			}
			writer.WriteEndObject();
			this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
			this.OnSerialized(writer, contract, value);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00026D8C File Offset: 0x00024F8C
		private void SerializeDynamic(JsonWriter writer, IDynamicMetaObjectProvider value, JsonDynamicContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			this.OnSerializing(writer, contract, value);
			this._serializeStack.Add(value);
			this.WriteObjectStart(writer, value, contract, member, collectionContract, containerProperty);
			int top = writer.Top;
			for (int i = 0; i < contract.Properties.Count; i++)
			{
				JsonProperty jsonProperty = contract.Properties[i];
				if (jsonProperty.HasMemberAttribute)
				{
					try
					{
						JsonContract valueContract;
						object value2;
						if (this.CalculatePropertyValues(writer, value, contract, member, jsonProperty, out valueContract, out value2))
						{
							jsonProperty.WritePropertyName(writer);
							this.SerializeValue(writer, value2, valueContract, jsonProperty, contract, member);
						}
					}
					catch (Exception ex)
					{
						if (!base.IsErrorHandled(value, contract, jsonProperty.PropertyName, null, writer.ContainerPath, ex))
						{
							throw;
						}
						this.HandleError(writer, top);
					}
				}
			}
			foreach (string text in value.GetDynamicMemberNames())
			{
				object obj;
				if (contract.TryGetMember(value, text, out obj))
				{
					try
					{
						JsonContract contractSafe = this.GetContractSafe(obj);
						if (this.ShouldWriteDynamicProperty(obj))
						{
							if (this.CheckForCircularReference(writer, obj, null, contractSafe, contract, member))
							{
								string name = (contract.PropertyNameResolver != null) ? contract.PropertyNameResolver(text) : text;
								writer.WritePropertyName(name);
								this.SerializeValue(writer, obj, contractSafe, null, contract, member);
							}
						}
					}
					catch (Exception ex2)
					{
						if (!base.IsErrorHandled(value, contract, text, null, writer.ContainerPath, ex2))
						{
							throw;
						}
						this.HandleError(writer, top);
					}
				}
			}
			writer.WriteEndObject();
			this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
			this.OnSerialized(writer, contract, value);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00026F58 File Offset: 0x00025158
		private bool ShouldWriteDynamicProperty(object memberValue)
		{
			return (this.Serializer._nullValueHandling != NullValueHandling.Ignore || memberValue != null) && (!this.HasFlag(this.Serializer._defaultValueHandling, DefaultValueHandling.Ignore) || (memberValue != null && !MiscellaneousUtils.ValueEquals(memberValue, ReflectionUtils.GetDefaultValue(memberValue.GetType()))));
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00026FA8 File Offset: 0x000251A8
		private bool ShouldWriteType(TypeNameHandling typeNameHandlingFlag, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			TypeNameHandling value = ((member != null) ? member.TypeNameHandling : null) ?? (((containerProperty != null) ? containerProperty.ItemTypeNameHandling : null) ?? (((containerContract != null) ? containerContract.ItemTypeNameHandling : null) ?? this.Serializer._typeNameHandling));
			if (this.HasFlag(value, typeNameHandlingFlag))
			{
				return true;
			}
			if (this.HasFlag(value, TypeNameHandling.Auto))
			{
				if (member != null)
				{
					if (contract.UnderlyingType != member.PropertyContract.CreatedType)
					{
						return true;
					}
				}
				else if (containerContract != null)
				{
					if (containerContract.ItemContract == null || contract.UnderlyingType != containerContract.ItemContract.CreatedType)
					{
						return true;
					}
				}
				else if (this._rootContract != null && this._serializeStack.Count == this._rootLevel && contract.UnderlyingType != this._rootContract.CreatedType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x000270D0 File Offset: 0x000252D0
		private void SerializeDictionary(JsonWriter writer, IDictionary values, JsonDictionaryContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			IWrappedDictionary wrappedDictionary = values as IWrappedDictionary;
			object obj = (wrappedDictionary != null) ? wrappedDictionary.UnderlyingDictionary : values;
			this.OnSerializing(writer, contract, obj);
			this._serializeStack.Add(obj);
			this.WriteObjectStart(writer, obj, contract, member, collectionContract, containerProperty);
			if (contract.ItemContract == null)
			{
				contract.ItemContract = this.Serializer._contractResolver.ResolveContract(contract.DictionaryValueType ?? typeof(object));
			}
			if (contract.KeyContract == null)
			{
				contract.KeyContract = this.Serializer._contractResolver.ResolveContract(contract.DictionaryKeyType ?? typeof(object));
			}
			int top = writer.Top;
			foreach (object obj2 in values)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				bool escape;
				string text = this.GetPropertyName(writer, dictionaryEntry.Key, contract.KeyContract, out escape);
				text = ((contract.PropertyNameResolver != null) ? contract.PropertyNameResolver(text) : text);
				try
				{
					object value = dictionaryEntry.Value;
					JsonContract jsonContract = contract.FinalItemContract ?? this.GetContractSafe(value);
					if (this.ShouldWriteReference(value, null, jsonContract, contract, member))
					{
						writer.WritePropertyName(text, escape);
						this.WriteReference(writer, value);
					}
					else if (this.CheckForCircularReference(writer, value, null, jsonContract, contract, member))
					{
						writer.WritePropertyName(text, escape);
						this.SerializeValue(writer, value, jsonContract, null, contract, member);
					}
				}
				catch (Exception ex)
				{
					if (!base.IsErrorHandled(obj, contract, text, null, writer.ContainerPath, ex))
					{
						throw;
					}
					this.HandleError(writer, top);
				}
			}
			writer.WriteEndObject();
			this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
			this.OnSerialized(writer, contract, obj);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000272C8 File Offset: 0x000254C8
		private string GetPropertyName(JsonWriter writer, object name, JsonContract contract, out bool escape)
		{
			if (contract.ContractType == JsonContractType.Primitive)
			{
				JsonPrimitiveContract jsonPrimitiveContract = (JsonPrimitiveContract)contract;
				if (jsonPrimitiveContract.TypeCode == PrimitiveTypeCode.DateTime || jsonPrimitiveContract.TypeCode == PrimitiveTypeCode.DateTimeNullable)
				{
					escape = false;
					StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
					DateTimeUtils.WriteDateTimeString(stringWriter, (DateTime)name, writer.DateFormatHandling, writer.DateFormatString, writer.Culture);
					return stringWriter.ToString();
				}
				if (jsonPrimitiveContract.TypeCode == PrimitiveTypeCode.DateTimeOffset || jsonPrimitiveContract.TypeCode == PrimitiveTypeCode.DateTimeOffsetNullable)
				{
					escape = false;
					StringWriter stringWriter2 = new StringWriter(CultureInfo.InvariantCulture);
					DateTimeUtils.WriteDateTimeOffsetString(stringWriter2, (DateTimeOffset)name, writer.DateFormatHandling, writer.DateFormatString, writer.Culture);
					return stringWriter2.ToString();
				}
				escape = true;
				return Convert.ToString(name, CultureInfo.InvariantCulture);
			}
			else
			{
				string result;
				if (JsonSerializerInternalWriter.TryConvertToString(name, name.GetType(), out result))
				{
					escape = true;
					return result;
				}
				escape = true;
				return name.ToString();
			}
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x000273A8 File Offset: 0x000255A8
		private void HandleError(JsonWriter writer, int initialDepth)
		{
			base.ClearErrorContext();
			if (writer.WriteState == WriteState.Property)
			{
				writer.WriteNull();
			}
			while (writer.Top > initialDepth)
			{
				writer.WriteEnd();
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x000273D0 File Offset: 0x000255D0
		private bool ShouldSerialize(JsonWriter writer, JsonProperty property, object target)
		{
			if (property.ShouldSerialize == null)
			{
				return true;
			}
			bool flag = property.ShouldSerialize(target);
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "ShouldSerialize result for property '{0}' on {1}: {2}".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, property.DeclaringType, flag)), null);
			}
			return flag;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00027448 File Offset: 0x00025648
		private bool IsSpecified(JsonWriter writer, JsonProperty property, object target)
		{
			if (property.GetIsSpecified == null)
			{
				return true;
			}
			bool flag = property.GetIsSpecified(target);
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "IsSpecified result for property '{0}' on {1}: {2}".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, property.DeclaringType, flag)), null);
			}
			return flag;
		}

		// Token: 0x04000359 RID: 857
		private JsonContract _rootContract;

		// Token: 0x0400035A RID: 858
		private int _rootLevel;

		// Token: 0x0400035B RID: 859
		private readonly List<object> _serializeStack = new List<object>();

		// Token: 0x0400035C RID: 860
		private JsonSerializerProxy _internalSerializer;
	}
}
