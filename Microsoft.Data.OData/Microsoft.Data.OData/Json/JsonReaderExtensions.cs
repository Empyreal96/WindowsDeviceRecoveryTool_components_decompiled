using System;
using System.Diagnostics;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x02000247 RID: 583
	internal static class JsonReaderExtensions
	{
		// Token: 0x060012AD RID: 4781 RVA: 0x0004618A File Offset: 0x0004438A
		internal static void ReadStartObject(this JsonReader jsonReader)
		{
			jsonReader.ReadNext(JsonNodeType.StartObject);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00046193 File Offset: 0x00044393
		internal static void ReadEndObject(this JsonReader jsonReader)
		{
			jsonReader.ReadNext(JsonNodeType.EndObject);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0004619C File Offset: 0x0004439C
		internal static void ReadStartArray(this JsonReader jsonReader)
		{
			jsonReader.ReadNext(JsonNodeType.StartArray);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000461A5 File Offset: 0x000443A5
		internal static void ReadEndArray(this JsonReader jsonReader)
		{
			jsonReader.ReadNext(JsonNodeType.EndArray);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x000461AE File Offset: 0x000443AE
		internal static string GetPropertyName(this JsonReader jsonReader)
		{
			return (string)jsonReader.Value;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000461BC File Offset: 0x000443BC
		internal static string ReadPropertyName(this JsonReader jsonReader)
		{
			jsonReader.ValidateNodeType(JsonNodeType.Property);
			string propertyName = jsonReader.GetPropertyName();
			jsonReader.ReadNext();
			return propertyName;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x000461E0 File Offset: 0x000443E0
		internal static object ReadPrimitiveValue(this JsonReader jsonReader)
		{
			object value = jsonReader.Value;
			jsonReader.ReadNext(JsonNodeType.PrimitiveValue);
			return value;
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x000461FC File Offset: 0x000443FC
		internal static string ReadStringValue(this JsonReader jsonReader)
		{
			object obj = jsonReader.ReadPrimitiveValue();
			string text = obj as string;
			if (obj == null || text != null)
			{
				return text;
			}
			throw JsonReaderExtensions.CreateException(Strings.JsonReaderExtensions_CannotReadValueAsString(obj));
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0004622C File Offset: 0x0004442C
		internal static string ReadStringValue(this JsonReader jsonReader, string propertyName)
		{
			object obj = jsonReader.ReadPrimitiveValue();
			string text = obj as string;
			if (obj == null || text != null)
			{
				return text;
			}
			throw JsonReaderExtensions.CreateException(Strings.JsonReaderExtensions_CannotReadPropertyValueAsString(obj, propertyName));
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0004625C File Offset: 0x0004445C
		internal static double? ReadDoubleValue(this JsonReader jsonReader)
		{
			object obj = jsonReader.ReadPrimitiveValue();
			double? result = obj as double?;
			if (obj == null || result != null)
			{
				return result;
			}
			int? num = obj as int?;
			if (num != null)
			{
				return new double?((double)num.Value);
			}
			throw JsonReaderExtensions.CreateException(Strings.JsonReaderExtensions_CannotReadValueAsDouble(obj));
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x000462B8 File Offset: 0x000444B8
		internal static void SkipValue(this JsonReader jsonReader)
		{
			int num = 0;
			do
			{
				switch (jsonReader.NodeType)
				{
				case JsonNodeType.StartObject:
				case JsonNodeType.StartArray:
					num++;
					break;
				case JsonNodeType.EndObject:
				case JsonNodeType.EndArray:
					num--;
					break;
				}
				jsonReader.ReadNext();
			}
			while (num > 0);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00046305 File Offset: 0x00044505
		internal static JsonNodeType ReadNext(this JsonReader jsonReader)
		{
			jsonReader.Read();
			return jsonReader.NodeType;
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00046314 File Offset: 0x00044514
		internal static bool IsOnValueNode(this JsonReader jsonReader)
		{
			JsonNodeType nodeType = jsonReader.NodeType;
			return nodeType == JsonNodeType.PrimitiveValue || nodeType == JsonNodeType.StartObject || nodeType == JsonNodeType.StartArray;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00046336 File Offset: 0x00044536
		[Conditional("DEBUG")]
		internal static void AssertNotBuffering(this BufferingJsonReader bufferedJsonReader)
		{
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00046338 File Offset: 0x00044538
		[Conditional("DEBUG")]
		internal static void AssertBuffering(this BufferingJsonReader bufferedJsonReader)
		{
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0004633A File Offset: 0x0004453A
		internal static ODataException CreateException(string exceptionMessage)
		{
			return new ODataException(exceptionMessage);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00046342 File Offset: 0x00044542
		private static void ReadNext(this JsonReader jsonReader, JsonNodeType expectedNodeType)
		{
			jsonReader.ValidateNodeType(expectedNodeType);
			jsonReader.Read();
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00046352 File Offset: 0x00044552
		private static void ValidateNodeType(this JsonReader jsonReader, JsonNodeType expectedNodeType)
		{
			if (jsonReader.NodeType != expectedNodeType)
			{
				throw JsonReaderExtensions.CreateException(Strings.JsonReaderExtensions_UnexpectedNodeDetected(expectedNodeType, jsonReader.NodeType));
			}
		}
	}
}
