using System;
using System.Collections.Generic;
using System.Spatial;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x02000176 RID: 374
	internal static class ODataJsonReaderCoreUtils
	{
		// Token: 0x06000A9D RID: 2717 RVA: 0x00023374 File Offset: 0x00021574
		internal static ISpatial ReadSpatialValue(BufferingJsonReader jsonReader, bool insideJsonObjectValue, ODataInputContext inputContext, IEdmPrimitiveTypeReference expectedValueTypeReference, bool validateNullValue, int recursionDepth, string propertyName)
		{
			ODataVersionChecker.CheckSpatialValue(inputContext.Version);
			if (!insideJsonObjectValue && ODataJsonReaderCoreUtils.TryReadNullValue(jsonReader, inputContext, expectedValueTypeReference, validateNullValue, propertyName))
			{
				return null;
			}
			ISpatial spatial = null;
			if (insideJsonObjectValue || jsonReader.NodeType == JsonNodeType.StartObject)
			{
				IDictionary<string, object> source = ODataJsonReaderCoreUtils.ReadObjectValue(jsonReader, insideJsonObjectValue, inputContext, recursionDepth);
				GeoJsonObjectFormatter geoJsonObjectFormatter = SpatialImplementation.CurrentImplementation.CreateGeoJsonObjectFormatter();
				if (expectedValueTypeReference.IsGeographyType())
				{
					spatial = geoJsonObjectFormatter.Read<Geography>(source);
				}
				else
				{
					spatial = geoJsonObjectFormatter.Read<Geometry>(source);
				}
			}
			if (spatial == null)
			{
				throw new ODataException(Strings.ODataJsonReaderCoreUtils_CannotReadSpatialPropertyValue);
			}
			return spatial;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000233EC File Offset: 0x000215EC
		internal static bool TryReadNullValue(BufferingJsonReader jsonReader, ODataInputContext inputContext, IEdmTypeReference expectedTypeReference, bool validateNullValue, string propertyName)
		{
			if (jsonReader.NodeType == JsonNodeType.PrimitiveValue && jsonReader.Value == null)
			{
				jsonReader.ReadNext();
				ReaderValidationUtils.ValidateNullValue(inputContext.Model, expectedTypeReference, inputContext.MessageReaderSettings, validateNullValue, inputContext.Version, propertyName);
				return true;
			}
			return false;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00023424 File Offset: 0x00021624
		private static IDictionary<string, object> ReadObjectValue(JsonReader jsonReader, bool insideJsonObjectValue, ODataInputContext inputContext, int recursionDepth)
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref recursionDepth, inputContext.MessageReaderSettings.MessageQuotas.MaxNestingDepth);
			IDictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
			if (!insideJsonObjectValue)
			{
				jsonReader.ReadNext();
			}
			while (jsonReader.NodeType != JsonNodeType.EndObject)
			{
				string key = jsonReader.ReadPropertyName();
				JsonNodeType nodeType = jsonReader.NodeType;
				object value;
				switch (nodeType)
				{
				case JsonNodeType.StartObject:
					value = ODataJsonReaderCoreUtils.ReadObjectValue(jsonReader, false, inputContext, recursionDepth);
					break;
				case JsonNodeType.EndObject:
					goto IL_76;
				case JsonNodeType.StartArray:
					value = ODataJsonReaderCoreUtils.ReadArrayValue(jsonReader, inputContext, recursionDepth);
					break;
				default:
					if (nodeType != JsonNodeType.PrimitiveValue)
					{
						goto IL_76;
					}
					value = jsonReader.ReadPrimitiveValue();
					break;
				}
				dictionary.Add(key, value);
				continue;
				IL_76:
				return null;
			}
			jsonReader.ReadEndObject();
			return dictionary;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x000234C4 File Offset: 0x000216C4
		private static IEnumerable<object> ReadArrayValue(JsonReader jsonReader, ODataInputContext inputContext, int recursionDepth)
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref recursionDepth, inputContext.MessageReaderSettings.MessageQuotas.MaxNestingDepth);
			List<object> list = new List<object>();
			jsonReader.ReadNext();
			while (jsonReader.NodeType != JsonNodeType.EndArray)
			{
				JsonNodeType nodeType = jsonReader.NodeType;
				switch (nodeType)
				{
				case JsonNodeType.StartObject:
					list.Add(ODataJsonReaderCoreUtils.ReadObjectValue(jsonReader, false, inputContext, recursionDepth));
					continue;
				case JsonNodeType.EndObject:
					break;
				case JsonNodeType.StartArray:
					list.Add(ODataJsonReaderCoreUtils.ReadArrayValue(jsonReader, inputContext, recursionDepth));
					continue;
				default:
					if (nodeType == JsonNodeType.PrimitiveValue)
					{
						list.Add(jsonReader.ReadPrimitiveValue());
						continue;
					}
					break;
				}
				return null;
			}
			jsonReader.ReadEndArray();
			return list;
		}
	}
}
