using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x020001C0 RID: 448
	internal static class JsonWriterExtensions
	{
		// Token: 0x06000DD6 RID: 3542 RVA: 0x000303D4 File Offset: 0x0002E5D4
		internal static void WriteJsonObjectValue(this IJsonWriter jsonWriter, IDictionary<string, object> jsonObjectValue, Action<IJsonWriter> injectPropertyAction, ODataVersion odataVersion)
		{
			jsonWriter.StartObjectScope();
			if (injectPropertyAction != null)
			{
				injectPropertyAction(jsonWriter);
			}
			foreach (KeyValuePair<string, object> keyValuePair in jsonObjectValue)
			{
				jsonWriter.WriteName(keyValuePair.Key);
				jsonWriter.WriteJsonValue(keyValuePair.Value, odataVersion);
			}
			jsonWriter.EndObjectScope();
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00030448 File Offset: 0x0002E648
		internal static void WritePrimitiveValue(this IJsonWriter jsonWriter, object value, ODataVersion odataVersion)
		{
			switch (PlatformHelper.GetTypeCode(value.GetType()))
			{
			case TypeCode.Boolean:
				jsonWriter.WriteValue((bool)value);
				return;
			case TypeCode.SByte:
				jsonWriter.WriteValue((sbyte)value);
				return;
			case TypeCode.Byte:
				jsonWriter.WriteValue((byte)value);
				return;
			case TypeCode.Int16:
				jsonWriter.WriteValue((short)value);
				return;
			case TypeCode.Int32:
				jsonWriter.WriteValue((int)value);
				return;
			case TypeCode.Int64:
				jsonWriter.WriteValue((long)value);
				return;
			case TypeCode.Single:
				jsonWriter.WriteValue((float)value);
				return;
			case TypeCode.Double:
				jsonWriter.WriteValue((double)value);
				return;
			case TypeCode.Decimal:
				jsonWriter.WriteValue((decimal)value);
				return;
			case TypeCode.DateTime:
				jsonWriter.WriteValue((DateTime)value, odataVersion);
				return;
			case TypeCode.String:
				jsonWriter.WriteValue((string)value);
				return;
			}
			byte[] array = value as byte[];
			if (array != null)
			{
				jsonWriter.WriteValue(Convert.ToBase64String(array));
				return;
			}
			if (value is DateTimeOffset)
			{
				jsonWriter.WriteValue((DateTimeOffset)value, odataVersion);
				return;
			}
			if (value is Guid)
			{
				jsonWriter.WriteValue((Guid)value);
				return;
			}
			if (value is TimeSpan)
			{
				jsonWriter.WriteValue((TimeSpan)value);
				return;
			}
			throw new ODataException(Strings.ODataJsonWriter_UnsupportedValueType(value.GetType().FullName));
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000305AC File Offset: 0x0002E7AC
		private static void WriteJsonArrayValue(this IJsonWriter jsonWriter, IEnumerable arrayValue, ODataVersion odataVersion)
		{
			jsonWriter.StartArrayScope();
			foreach (object propertyValue in arrayValue)
			{
				jsonWriter.WriteJsonValue(propertyValue, odataVersion);
			}
			jsonWriter.EndArrayScope();
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00030608 File Offset: 0x0002E808
		private static void WriteJsonValue(this IJsonWriter jsonWriter, object propertyValue, ODataVersion odataVersion)
		{
			if (propertyValue == null)
			{
				jsonWriter.WriteValue(null);
				return;
			}
			if (EdmLibraryExtensions.IsPrimitiveType(propertyValue.GetType()))
			{
				jsonWriter.WritePrimitiveValue(propertyValue, odataVersion);
				return;
			}
			IDictionary<string, object> dictionary = propertyValue as IDictionary<string, object>;
			if (dictionary != null)
			{
				jsonWriter.WriteJsonObjectValue(dictionary, null, odataVersion);
				return;
			}
			IEnumerable arrayValue = propertyValue as IEnumerable;
			jsonWriter.WriteJsonArrayValue(arrayValue, odataVersion);
		}
	}
}
