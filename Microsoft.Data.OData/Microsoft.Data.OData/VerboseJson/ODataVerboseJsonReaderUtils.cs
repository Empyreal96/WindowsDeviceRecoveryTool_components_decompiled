using System;
using System.Globalization;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000205 RID: 517
	internal static class ODataVerboseJsonReaderUtils
	{
		// Token: 0x06000FD3 RID: 4051 RVA: 0x000396B7 File Offset: 0x000378B7
		internal static ODataVerboseJsonReaderUtils.FeedPropertyKind DetermineFeedPropertyKind(string propertyName)
		{
			if (string.CompareOrdinal("__count", propertyName) == 0)
			{
				return ODataVerboseJsonReaderUtils.FeedPropertyKind.Count;
			}
			if (string.CompareOrdinal("__next", propertyName) == 0)
			{
				return ODataVerboseJsonReaderUtils.FeedPropertyKind.NextPageLink;
			}
			if (string.CompareOrdinal("results", propertyName) == 0)
			{
				return ODataVerboseJsonReaderUtils.FeedPropertyKind.Results;
			}
			return ODataVerboseJsonReaderUtils.FeedPropertyKind.Unsupported;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x000396E8 File Offset: 0x000378E8
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
				ODataReaderBehavior readerBehavior = messageReaderSettings.ReaderBehavior;
				string text = value as string;
				if (text != null)
				{
					return ODataVerboseJsonReaderUtils.ConvertStringValue(text, primitiveClrType, version);
				}
				if (value is int)
				{
					return ODataVerboseJsonReaderUtils.ConvertInt32Value((int)value, primitiveClrType, primitiveTypeReference, readerBehavior != null && readerBehavior.UseV1ProviderBehavior);
				}
				if (value is double)
				{
					double value2 = (double)value;
					if (primitiveClrType == typeof(float))
					{
						return Convert.ToSingle(value2);
					}
					if (!ODataVerboseJsonReaderUtils.IsV1PrimitiveType(primitiveClrType) || (primitiveClrType != typeof(double) && (readerBehavior == null || !readerBehavior.UseV1ProviderBehavior)))
					{
						throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertDouble(primitiveTypeReference.ODataFullName()));
					}
				}
				else if (value is bool)
				{
					if (primitiveClrType != typeof(bool) && (readerBehavior == null || readerBehavior.FormatBehaviorKind != ODataBehaviorKind.WcfDataServicesServer))
					{
						throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertBoolean(primitiveTypeReference.ODataFullName()));
					}
				}
				else
				{
					if (value is DateTime)
					{
						return ODataVerboseJsonReaderUtils.ConvertDateTimeValue((DateTime)value, primitiveClrType, primitiveTypeReference, readerBehavior);
					}
					if (value is DateTimeOffset && primitiveClrType != typeof(DateTimeOffset))
					{
						throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertDateTimeOffset(primitiveTypeReference.ODataFullName()));
					}
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

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00039884 File Offset: 0x00037A84
		internal static void EnsureInstance<T>(ref T instance) where T : class, new()
		{
			if (instance == null)
			{
				instance = Activator.CreateInstance<T>();
			}
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0003989E File Offset: 0x00037A9E
		internal static bool ErrorPropertyNotFound(ref ODataVerboseJsonReaderUtils.ErrorPropertyBitMask propertiesFoundBitField, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask propertyFoundBitMask)
		{
			if ((propertiesFoundBitField & propertyFoundBitMask) == propertyFoundBitMask)
			{
				return false;
			}
			propertiesFoundBitField |= propertyFoundBitMask;
			return true;
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x000398B0 File Offset: 0x00037AB0
		internal static void ValidateMetadataStringProperty(string propertyValue, string propertyName)
		{
			if (propertyValue == null)
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_MetadataPropertyWithNullValue(propertyName));
			}
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x000398C1 File Offset: 0x00037AC1
		internal static void VerifyMetadataPropertyNotFound(ref ODataVerboseJsonReaderUtils.MetadataPropertyBitMask propertiesFoundBitField, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask propertyFoundBitMask, string propertyName)
		{
			if ((propertiesFoundBitField & propertyFoundBitMask) != ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.None)
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_MultipleMetadataPropertiesWithSameName(propertyName));
			}
			propertiesFoundBitField |= propertyFoundBitMask;
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x000398DB File Offset: 0x00037ADB
		internal static void ValidateEntityReferenceLinksStringProperty(string propertyValue, string propertyName)
		{
			if (propertyValue == null)
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_EntityReferenceLinksPropertyWithNullValue(propertyName));
			}
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x000398EC File Offset: 0x00037AEC
		internal static void ValidateCountPropertyInEntityReferenceLinks(long? propertyValue)
		{
			if (propertyValue == null)
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_EntityReferenceLinksInlineCountWithNullValue("__count"));
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00039907 File Offset: 0x00037B07
		internal static void VerifyEntityReferenceLinksWrapperPropertyNotFound(ref ODataVerboseJsonReaderUtils.EntityReferenceLinksWrapperPropertyBitMask propertiesFoundBitField, ODataVerboseJsonReaderUtils.EntityReferenceLinksWrapperPropertyBitMask propertyFoundBitMask, string propertyName)
		{
			if ((propertiesFoundBitField & propertyFoundBitMask) == propertyFoundBitMask)
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_MultipleEntityReferenceLinksWrapperPropertiesWithSameName(propertyName));
			}
			propertiesFoundBitField |= propertyFoundBitMask;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00039922 File Offset: 0x00037B22
		internal static void VerifyErrorPropertyNotFound(ref ODataVerboseJsonReaderUtils.ErrorPropertyBitMask propertiesFoundBitField, ODataVerboseJsonReaderUtils.ErrorPropertyBitMask propertyFoundBitMask, string propertyName)
		{
			if (!ODataVerboseJsonReaderUtils.ErrorPropertyNotFound(ref propertiesFoundBitField, propertyFoundBitMask))
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_MultipleErrorPropertiesWithSameName(propertyName));
			}
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00039939 File Offset: 0x00037B39
		internal static void ValidateMediaResourceStringProperty(string propertyValue, string propertyName)
		{
			if (propertyValue == null)
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_MediaResourcePropertyWithNullValue(propertyName));
			}
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0003994A File Offset: 0x00037B4A
		internal static void ValidateFeedProperty(object propertyValue, string propertyName)
		{
			if (propertyValue == null)
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_FeedPropertyWithNullValue(propertyName));
			}
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0003995C File Offset: 0x00037B5C
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
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataVerboseJsonReader_ReadEntryStart));
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00039A0C File Offset: 0x00037C0C
		private static object ConvertStringValue(string stringValue, Type targetType, ODataVersion version)
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
			if (targetType == typeof(DateTimeOffset))
			{
				return PlatformHelper.ConvertStringToDateTimeOffset(stringValue);
			}
			if (targetType == typeof(DateTime) && version >= ODataVersion.V3)
			{
				try
				{
					return PlatformHelper.ConvertStringToDateTime(stringValue);
				}
				catch (FormatException)
				{
				}
			}
			return Convert.ChangeType(stringValue, targetType, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00039AD4 File Offset: 0x00037CD4
		private static object ConvertInt32Value(int intValue, Type targetType, IEdmPrimitiveTypeReference primitiveTypeReference, bool usesV1ProviderBehavior)
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
			if (!ODataVerboseJsonReaderUtils.IsV1PrimitiveType(targetType) || (targetType != typeof(int) && !usesV1ProviderBehavior))
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertInt32(primitiveTypeReference.ODataFullName()));
			}
			return intValue;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00039BDC File Offset: 0x00037DDC
		private static object ConvertDateTimeValue(DateTime datetimeValue, Type targetType, IEdmPrimitiveTypeReference primitiveTypeReference, ODataReaderBehavior readerBehavior)
		{
			if (targetType == typeof(DateTimeOffset) && (datetimeValue.Kind == DateTimeKind.Local || datetimeValue.Kind == DateTimeKind.Utc))
			{
				return new DateTimeOffset(datetimeValue);
			}
			if (targetType != typeof(DateTime) && (readerBehavior == null || readerBehavior.FormatBehaviorKind != ODataBehaviorKind.WcfDataServicesServer))
			{
				throw new ODataException(Strings.ODataJsonReaderUtils_CannotConvertDateTime(primitiveTypeReference.ODataFullName()));
			}
			return datetimeValue;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00039C50 File Offset: 0x00037E50
		private static bool IsV1PrimitiveType(Type targetType)
		{
			return !(targetType == typeof(DateTimeOffset)) && !(targetType == typeof(TimeSpan));
		}

		// Token: 0x02000206 RID: 518
		internal enum FeedPropertyKind
		{
			// Token: 0x040005C0 RID: 1472
			Unsupported,
			// Token: 0x040005C1 RID: 1473
			Count,
			// Token: 0x040005C2 RID: 1474
			Results,
			// Token: 0x040005C3 RID: 1475
			NextPageLink
		}

		// Token: 0x02000207 RID: 519
		[Flags]
		internal enum EntityReferenceLinksWrapperPropertyBitMask
		{
			// Token: 0x040005C5 RID: 1477
			None = 0,
			// Token: 0x040005C6 RID: 1478
			Count = 1,
			// Token: 0x040005C7 RID: 1479
			Results = 2,
			// Token: 0x040005C8 RID: 1480
			NextPageLink = 4
		}

		// Token: 0x02000208 RID: 520
		[Flags]
		internal enum ErrorPropertyBitMask
		{
			// Token: 0x040005CA RID: 1482
			None = 0,
			// Token: 0x040005CB RID: 1483
			Error = 1,
			// Token: 0x040005CC RID: 1484
			Code = 2,
			// Token: 0x040005CD RID: 1485
			Message = 4,
			// Token: 0x040005CE RID: 1486
			MessageLanguage = 8,
			// Token: 0x040005CF RID: 1487
			MessageValue = 16,
			// Token: 0x040005D0 RID: 1488
			InnerError = 32,
			// Token: 0x040005D1 RID: 1489
			TypeName = 64,
			// Token: 0x040005D2 RID: 1490
			StackTrace = 128
		}

		// Token: 0x02000209 RID: 521
		[Flags]
		internal enum MetadataPropertyBitMask
		{
			// Token: 0x040005D4 RID: 1492
			None = 0,
			// Token: 0x040005D5 RID: 1493
			Uri = 1,
			// Token: 0x040005D6 RID: 1494
			Type = 2,
			// Token: 0x040005D7 RID: 1495
			ETag = 4,
			// Token: 0x040005D8 RID: 1496
			MediaUri = 8,
			// Token: 0x040005D9 RID: 1497
			EditMedia = 16,
			// Token: 0x040005DA RID: 1498
			ContentType = 32,
			// Token: 0x040005DB RID: 1499
			MediaETag = 64,
			// Token: 0x040005DC RID: 1500
			Properties = 128,
			// Token: 0x040005DD RID: 1501
			Id = 256,
			// Token: 0x040005DE RID: 1502
			Actions = 512,
			// Token: 0x040005DF RID: 1503
			Functions = 1024
		}
	}
}
