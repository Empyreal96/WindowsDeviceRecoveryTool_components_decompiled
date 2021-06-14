using System;
using System.Globalization;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200019F RID: 415
	internal static class ODataJsonLightReaderUtils
	{
		// Token: 0x06000CA0 RID: 3232 RVA: 0x0002B430 File Offset: 0x00029630
		internal static object ConvertValue(object value, IEdmPrimitiveTypeReference primitiveTypeReference, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool validateNullValue, string propertyName)
		{
			if (value == null)
			{
				ReaderValidationUtils.ValidateNullValue(EdmCoreModel.Instance, primitiveTypeReference, messageReaderSettings, validateNullValue, version, propertyName);
				return null;
			}
			try
			{
				Type primitiveClrType = EdmLibraryExtensions.GetPrimitiveClrType(primitiveTypeReference.PrimitiveDefinition(), false);
				string text = value as string;
				if (text != null)
				{
					return ODataJsonLightReaderUtils.ConvertStringValue(text, primitiveClrType);
				}
				if (value is int)
				{
					return ODataJsonLightReaderUtils.ConvertInt32Value((int)value, primitiveClrType, primitiveTypeReference);
				}
				if (value is double)
				{
					double value2 = (double)value;
					if (primitiveClrType == typeof(float))
					{
						return Convert.ToSingle(value2);
					}
					if (primitiveClrType != typeof(double))
					{
						throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertDouble(primitiveTypeReference.ODataFullName()));
					}
				}
				else if (value is bool)
				{
					if (primitiveClrType != typeof(bool))
					{
						throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertBoolean(primitiveTypeReference.ODataFullName()));
					}
				}
				else if (value is DateTime)
				{
					if (primitiveClrType != typeof(DateTime))
					{
						throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertDateTime(primitiveTypeReference.ODataFullName()));
					}
				}
				else if (value is DateTimeOffset && primitiveClrType != typeof(DateTimeOffset))
				{
					throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertDateTimeOffset(primitiveTypeReference.ODataFullName()));
				}
			}
			catch (Exception ex)
			{
				if (!ExceptionUtils.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ReaderValidationUtils.GetPrimitiveTypeConversionException(primitiveTypeReference, ex);
			}
			return value;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002B5A4 File Offset: 0x000297A4
		internal static void EnsureInstance<T>(ref T instance) where T : class, new()
		{
			if (instance == null)
			{
				instance = Activator.CreateInstance<T>();
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0002B5BE File Offset: 0x000297BE
		internal static bool IsODataAnnotationName(string propertyName)
		{
			return propertyName.StartsWith("odata.", StringComparison.Ordinal);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0002B5CC File Offset: 0x000297CC
		internal static bool IsAnnotationProperty(string propertyName)
		{
			return propertyName.IndexOf('.') >= 0;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0002B5DC File Offset: 0x000297DC
		internal static void ValidateAnnotationStringValue(string propertyValue, string annotationName)
		{
			if (propertyValue == null)
			{
				throw new ODataException(Strings.ODataJsonLightReaderUtils_AnnotationWithNullValue(annotationName));
			}
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0002B5F0 File Offset: 0x000297F0
		internal static string GetPayloadTypeName(object payloadItem)
		{
			if (payloadItem == null)
			{
				return null;
			}
			TypeCode typeCode = PlatformHelper.GetTypeCode(payloadItem.GetType());
			TypeCode typeCode2 = typeCode;
			if (typeCode2 == TypeCode.Boolean)
			{
				return "Edm.Boolean";
			}
			if (typeCode2 == TypeCode.Int32)
			{
				return "Edm.Int32";
			}
			switch (typeCode2)
			{
			case TypeCode.Double:
				return "Edm.Double";
			case TypeCode.DateTime:
				return "Edm.DateTime";
			case TypeCode.String:
				return "Edm.String";
			}
			ODataComplexValue odataComplexValue = payloadItem as ODataComplexValue;
			if (odataComplexValue != null)
			{
				return odataComplexValue.TypeName;
			}
			ODataCollectionValue odataCollectionValue = payloadItem as ODataCollectionValue;
			if (odataCollectionValue != null)
			{
				return odataCollectionValue.TypeName;
			}
			ODataEntry odataEntry = payloadItem as ODataEntry;
			if (odataEntry != null)
			{
				return odataEntry.TypeName;
			}
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataJsonLightReader_ReadEntryStart));
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0002B6A0 File Offset: 0x000298A0
		private static object ConvertStringValue(string stringValue, Type targetType)
		{
			if (targetType == typeof(byte[]))
			{
				return Convert.FromBase64String(stringValue);
			}
			if (targetType == typeof(Guid))
			{
				return new Guid(stringValue);
			}
			if (targetType == typeof(TimeSpan))
			{
				return XmlConvert.ToTimeSpan(stringValue);
			}
			if (targetType == typeof(DateTime))
			{
				return PlatformHelper.ConvertStringToDateTime(stringValue);
			}
			if (targetType == typeof(DateTimeOffset))
			{
				return PlatformHelper.ConvertStringToDateTimeOffset(stringValue);
			}
			return Convert.ChangeType(stringValue, targetType, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0002B74C File Offset: 0x0002994C
		private static object ConvertInt32Value(int intValue, Type targetType, IEdmPrimitiveTypeReference primitiveTypeReference)
		{
			if (targetType == typeof(short))
			{
				return Convert.ToInt16(intValue);
			}
			if (targetType == typeof(byte))
			{
				return Convert.ToByte(intValue);
			}
			if (targetType == typeof(sbyte))
			{
				return Convert.ToSByte(intValue);
			}
			if (targetType == typeof(float))
			{
				return Convert.ToSingle(intValue);
			}
			if (targetType == typeof(double))
			{
				return Convert.ToDouble(intValue);
			}
			if (targetType == typeof(decimal) || targetType == typeof(long))
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertInt64OrDecimal);
			}
			if (targetType != typeof(int))
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertInt32(primitiveTypeReference.ODataFullName()));
			}
			return intValue;
		}
	}
}
