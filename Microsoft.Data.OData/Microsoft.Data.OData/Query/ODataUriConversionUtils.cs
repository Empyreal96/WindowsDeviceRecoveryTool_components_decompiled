using System;
using System.Globalization;
using System.IO;
using System.Spatial;
using System.Text;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Evaluation;
using Microsoft.Data.OData.JsonLight;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.VerboseJson;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020001E7 RID: 487
	internal static class ODataUriConversionUtils
	{
		// Token: 0x06000EFF RID: 3839 RVA: 0x0003530C File Offset: 0x0003350C
		internal static string ConvertToUriPrimitiveLiteral(object value, ODataVersion version)
		{
			ExceptionUtils.CheckArgumentNotNull<object>(value, "value");
			if (value is ISpatial)
			{
				ODataVersionChecker.CheckSpatialValue(version);
			}
			return LiteralFormatter.ForConstantsWithoutEncoding.Format(value);
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00035334 File Offset: 0x00033534
		internal static object ConvertFromComplexOrCollectionValue(string value, ODataVersion version, IEdmModel model, IEdmTypeReference typeReference)
		{
			ODataMessageReaderSettings messageReaderSettings = new ODataMessageReaderSettings();
			if (model.IsUserModel())
			{
				try
				{
					using (StringReader stringReader = new StringReader(value))
					{
						using (ODataJsonLightInputContext odataJsonLightInputContext = new ODataJsonLightInputContext(ODataFormat.Json, stringReader, new MediaType("application", "json"), messageReaderSettings, version, false, true, model, null, null))
						{
							ODataJsonLightPropertyAndValueDeserializer odataJsonLightPropertyAndValueDeserializer = new ODataJsonLightPropertyAndValueDeserializer(odataJsonLightInputContext);
							odataJsonLightPropertyAndValueDeserializer.JsonReader.Read();
							object result = odataJsonLightPropertyAndValueDeserializer.ReadNonEntityValue(null, typeReference, null, null, true, false, false, null);
							odataJsonLightPropertyAndValueDeserializer.ReadPayloadEnd(false);
							return result;
						}
					}
				}
				catch (ODataException)
				{
				}
			}
			object result2;
			using (StringReader stringReader2 = new StringReader(value))
			{
				using (ODataVerboseJsonInputContext odataVerboseJsonInputContext = new ODataVerboseJsonInputContext(ODataFormat.VerboseJson, stringReader2, messageReaderSettings, version, false, true, model, null))
				{
					ODataVerboseJsonPropertyAndValueDeserializer odataVerboseJsonPropertyAndValueDeserializer = new ODataVerboseJsonPropertyAndValueDeserializer(odataVerboseJsonInputContext);
					odataVerboseJsonPropertyAndValueDeserializer.ReadPayloadStart(false);
					object obj = odataVerboseJsonPropertyAndValueDeserializer.ReadNonEntityValue(typeReference, null, null, true, null);
					odataVerboseJsonPropertyAndValueDeserializer.ReadPayloadEnd(false);
					result2 = obj;
				}
			}
			return result2;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00035464 File Offset: 0x00033664
		internal static object VerifyAndCoerceUriPrimitiveLiteral(object primitiveValue, IEdmModel model, IEdmTypeReference expectedTypeReference, ODataVersion version)
		{
			ExceptionUtils.CheckArgumentNotNull<object>(primitiveValue, "primitiveValue");
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ExceptionUtils.CheckArgumentNotNull<IEdmTypeReference>(expectedTypeReference, "expectedTypeReference");
			ODataUriNullValue odataUriNullValue = primitiveValue as ODataUriNullValue;
			if (odataUriNullValue != null)
			{
				if (!expectedTypeReference.IsNullable)
				{
					throw new ODataException(Strings.ODataUriUtils_ConvertFromUriLiteralNullOnNonNullableType(expectedTypeReference.ODataFullName()));
				}
				if (odataUriNullValue.TypeName == null)
				{
					odataUriNullValue.TypeName = expectedTypeReference.ODataFullName();
					return odataUriNullValue;
				}
				IEdmType type = TypeNameOracle.ResolveAndValidateTypeName(model, odataUriNullValue.TypeName, expectedTypeReference.Definition.TypeKind);
				if (type.IsSpatial())
				{
					ODataVersionChecker.CheckSpatialValue(version);
				}
				if (TypePromotionUtils.CanConvertTo(type.ToTypeReference(), expectedTypeReference))
				{
					odataUriNullValue.TypeName = expectedTypeReference.ODataFullName();
					return odataUriNullValue;
				}
				throw new ODataException(Strings.ODataUriUtils_ConvertFromUriLiteralNullTypeVerificationFailure(expectedTypeReference.ODataFullName(), odataUriNullValue.TypeName));
			}
			else
			{
				IEdmPrimitiveTypeReference edmPrimitiveTypeReference = expectedTypeReference.AsPrimitiveOrNull();
				if (edmPrimitiveTypeReference == null)
				{
					throw new ODataException(Strings.ODataUriUtils_ConvertFromUriLiteralTypeVerificationFailure(expectedTypeReference.ODataFullName(), primitiveValue));
				}
				object obj = ODataUriConversionUtils.CoerceNumericType(primitiveValue, edmPrimitiveTypeReference.PrimitiveDefinition());
				if (obj != null)
				{
					return obj;
				}
				Type type2 = primitiveValue.GetType();
				Type nonNullableType = TypeUtils.GetNonNullableType(EdmLibraryExtensions.GetPrimitiveClrType(edmPrimitiveTypeReference));
				if (nonNullableType.IsAssignableFrom(type2))
				{
					return primitiveValue;
				}
				throw new ODataException(Strings.ODataUriUtils_ConvertFromUriLiteralTypeVerificationFailure(edmPrimitiveTypeReference.ODataFullName(), primitiveValue));
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x000355C0 File Offset: 0x000337C0
		internal static string ConvertToUriComplexLiteral(ODataComplexValue complexValue, IEdmModel model, ODataVersion version, ODataFormat format)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataComplexValue>(complexValue, "complexValue");
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			StringBuilder stringBuilder = new StringBuilder();
			using (TextWriter textWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
			{
				ODataMessageWriterSettings messageWriterSettings = new ODataMessageWriterSettings
				{
					Version = new ODataVersion?(version),
					Indent = false
				};
				if (format == ODataFormat.VerboseJson)
				{
					ODataUriConversionUtils.WriteJsonVerboseLiteral(model, messageWriterSettings, textWriter, delegate(ODataVerboseJsonPropertyAndValueSerializer serializer)
					{
						serializer.WriteComplexValue(complexValue, null, true, serializer.CreateDuplicatePropertyNamesChecker(), null);
					});
				}
				else
				{
					if (format != ODataFormat.Json)
					{
						throw new ArgumentException(Strings.ODataUriUtils_ConvertToUriLiteralUnsupportedFormat(format.ToString()));
					}
					ODataUriConversionUtils.WriteJsonLightLiteral(model, messageWriterSettings, textWriter, delegate(ODataJsonLightValueSerializer serializer)
					{
						serializer.WriteComplexValue(complexValue, null, false, true, serializer.CreateDuplicatePropertyNamesChecker());
					});
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x000356AC File Offset: 0x000338AC
		internal static string ConvertToUriNullValue(ODataUriNullValue nullValue)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataUriNullValue>(nullValue, "nullValue");
			if (nullValue.TypeName != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("null");
				stringBuilder.Append("'");
				stringBuilder.Append(nullValue.TypeName);
				stringBuilder.Append("'");
				return stringBuilder.ToString();
			}
			return "null";
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0003573C File Offset: 0x0003393C
		internal static string ConvertToUriCollectionLiteral(ODataCollectionValue collectionValue, IEdmModel model, ODataVersion version, ODataFormat format)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataCollectionValue>(collectionValue, "collectionValue");
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			ODataVersionChecker.CheckCollectionValue(version);
			StringBuilder stringBuilder = new StringBuilder();
			using (TextWriter textWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
			{
				ODataMessageWriterSettings messageWriterSettings = new ODataMessageWriterSettings
				{
					Version = new ODataVersion?(version),
					Indent = false
				};
				if (format == ODataFormat.VerboseJson)
				{
					ODataUriConversionUtils.WriteJsonVerboseLiteral(model, messageWriterSettings, textWriter, delegate(ODataVerboseJsonPropertyAndValueSerializer serializer)
					{
						serializer.WriteCollectionValue(collectionValue, null, false);
					});
				}
				else
				{
					if (format != ODataFormat.Json)
					{
						throw new ArgumentException(Strings.ODataUriUtils_ConvertToUriLiteralUnsupportedFormat(format.ToString()));
					}
					ODataUriConversionUtils.WriteJsonLightLiteral(model, messageWriterSettings, textWriter, delegate(ODataJsonLightValueSerializer serializer)
					{
						serializer.WriteCollectionValue(collectionValue, null, false, true, false);
					});
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00035830 File Offset: 0x00033A30
		private static void WriteJsonVerboseLiteral(IEdmModel model, ODataMessageWriterSettings messageWriterSettings, TextWriter textWriter, Action<ODataVerboseJsonPropertyAndValueSerializer> writeValue)
		{
			using (ODataVerboseJsonOutputContext odataVerboseJsonOutputContext = new ODataVerboseJsonOutputContext(ODataFormat.VerboseJson, textWriter, messageWriterSettings, model))
			{
				ODataVerboseJsonPropertyAndValueSerializer obj = new ODataVerboseJsonPropertyAndValueSerializer(odataVerboseJsonOutputContext);
				writeValue(obj);
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00035878 File Offset: 0x00033A78
		private static void WriteJsonLightLiteral(IEdmModel model, ODataMessageWriterSettings messageWriterSettings, TextWriter textWriter, Action<ODataJsonLightValueSerializer> writeValue)
		{
			using (ODataJsonLightOutputContext odataJsonLightOutputContext = new ODataJsonLightOutputContext(ODataFormat.Json, textWriter, messageWriterSettings, model))
			{
				ODataJsonLightValueSerializer obj = new ODataJsonLightValueSerializer(odataJsonLightOutputContext);
				writeValue(obj);
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000358C0 File Offset: 0x00033AC0
		private static object CoerceNumericType(object primitiveValue, IEdmPrimitiveType targetEdmType)
		{
			ExceptionUtils.CheckArgumentNotNull<object>(primitiveValue, "primitiveValue");
			ExceptionUtils.CheckArgumentNotNull<IEdmPrimitiveType>(targetEdmType, "targetEdmType");
			Type type = primitiveValue.GetType();
			TypeCode typeCode = PlatformHelper.GetTypeCode(type);
			EdmPrimitiveTypeKind primitiveKind = targetEdmType.PrimitiveKind;
			switch (typeCode)
			{
			case TypeCode.SByte:
				switch (primitiveKind)
				{
				case EdmPrimitiveTypeKind.Decimal:
					return Convert.ToDecimal((sbyte)primitiveValue);
				case EdmPrimitiveTypeKind.Double:
					return Convert.ToDouble((sbyte)primitiveValue);
				case EdmPrimitiveTypeKind.Int16:
					return Convert.ToInt16((sbyte)primitiveValue);
				case EdmPrimitiveTypeKind.Int32:
					return Convert.ToInt32((sbyte)primitiveValue);
				case EdmPrimitiveTypeKind.Int64:
					return Convert.ToInt64((sbyte)primitiveValue);
				case EdmPrimitiveTypeKind.SByte:
					return primitiveValue;
				case EdmPrimitiveTypeKind.Single:
					return Convert.ToSingle((sbyte)primitiveValue);
				}
				break;
			case TypeCode.Byte:
				switch (primitiveKind)
				{
				case EdmPrimitiveTypeKind.Byte:
					return primitiveValue;
				case EdmPrimitiveTypeKind.Decimal:
					return Convert.ToDecimal((byte)primitiveValue);
				case EdmPrimitiveTypeKind.Double:
					return Convert.ToDouble((byte)primitiveValue);
				case EdmPrimitiveTypeKind.Int16:
					return Convert.ToInt16((byte)primitiveValue);
				case EdmPrimitiveTypeKind.Int32:
					return Convert.ToInt32((byte)primitiveValue);
				case EdmPrimitiveTypeKind.Int64:
					return Convert.ToInt64((byte)primitiveValue);
				case EdmPrimitiveTypeKind.Single:
					return Convert.ToSingle((byte)primitiveValue);
				}
				break;
			case TypeCode.Int16:
				switch (primitiveKind)
				{
				case EdmPrimitiveTypeKind.Decimal:
					return Convert.ToDecimal((short)primitiveValue);
				case EdmPrimitiveTypeKind.Double:
					return Convert.ToDouble((short)primitiveValue);
				case EdmPrimitiveTypeKind.Int16:
					return primitiveValue;
				case EdmPrimitiveTypeKind.Int32:
					return Convert.ToInt32((short)primitiveValue);
				case EdmPrimitiveTypeKind.Int64:
					return Convert.ToInt64((short)primitiveValue);
				case EdmPrimitiveTypeKind.Single:
					return Convert.ToSingle((short)primitiveValue);
				}
				break;
			case TypeCode.Int32:
				switch (primitiveKind)
				{
				case EdmPrimitiveTypeKind.Decimal:
					return Convert.ToDecimal((int)primitiveValue);
				case EdmPrimitiveTypeKind.Double:
					return Convert.ToDouble((int)primitiveValue);
				case EdmPrimitiveTypeKind.Int32:
					return primitiveValue;
				case EdmPrimitiveTypeKind.Int64:
					return Convert.ToInt64((int)primitiveValue);
				case EdmPrimitiveTypeKind.Single:
					return Convert.ToSingle((int)primitiveValue);
				}
				break;
			case TypeCode.Int64:
			{
				EdmPrimitiveTypeKind edmPrimitiveTypeKind = primitiveKind;
				switch (edmPrimitiveTypeKind)
				{
				case EdmPrimitiveTypeKind.Decimal:
					return Convert.ToDecimal((long)primitiveValue);
				case EdmPrimitiveTypeKind.Double:
					return Convert.ToDouble((long)primitiveValue);
				default:
					switch (edmPrimitiveTypeKind)
					{
					case EdmPrimitiveTypeKind.Int64:
						return primitiveValue;
					case EdmPrimitiveTypeKind.Single:
						return Convert.ToSingle((long)primitiveValue);
					}
					break;
				}
				break;
			}
			case TypeCode.Single:
			{
				EdmPrimitiveTypeKind edmPrimitiveTypeKind2 = primitiveKind;
				if (edmPrimitiveTypeKind2 == EdmPrimitiveTypeKind.Double)
				{
					return Convert.ToDouble((float)primitiveValue);
				}
				if (edmPrimitiveTypeKind2 == EdmPrimitiveTypeKind.Single)
				{
					return primitiveValue;
				}
				break;
			}
			case TypeCode.Double:
			{
				EdmPrimitiveTypeKind edmPrimitiveTypeKind3 = primitiveKind;
				if (edmPrimitiveTypeKind3 == EdmPrimitiveTypeKind.Double)
				{
					return primitiveValue;
				}
				break;
			}
			case TypeCode.Decimal:
			{
				EdmPrimitiveTypeKind edmPrimitiveTypeKind4 = primitiveKind;
				if (edmPrimitiveTypeKind4 == EdmPrimitiveTypeKind.Decimal)
				{
					return primitiveValue;
				}
				break;
			}
			}
			return null;
		}
	}
}
