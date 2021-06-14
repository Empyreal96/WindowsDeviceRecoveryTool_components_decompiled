using System;
using System.Spatial;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000003 RID: 3
	internal static class EdmValueUtils
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020D4 File Offset: 0x000002D4
		internal static IEdmDelayedValue ConvertPrimitiveValue(object primitiveValue, IEdmPrimitiveTypeReference type)
		{
			switch (PlatformHelper.GetTypeCode(primitiveValue.GetType()))
			{
			case TypeCode.Boolean:
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Boolean);
				return new EdmBooleanConstant(type, (bool)primitiveValue);
			case TypeCode.SByte:
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.SByte);
				return new EdmIntegerConstant(type, (long)((sbyte)primitiveValue));
			case TypeCode.Byte:
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Byte);
				return new EdmIntegerConstant(type, (long)((ulong)((byte)primitiveValue)));
			case TypeCode.Int16:
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Int16);
				return new EdmIntegerConstant(type, (long)((short)primitiveValue));
			case TypeCode.Int32:
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Int32);
				return new EdmIntegerConstant(type, (long)((int)primitiveValue));
			case TypeCode.Int64:
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Int64);
				return new EdmIntegerConstant(type, (long)primitiveValue);
			case TypeCode.Single:
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Single);
				return new EdmFloatingConstant(type, (double)((float)primitiveValue));
			case TypeCode.Double:
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Double);
				return new EdmFloatingConstant(type, (double)primitiveValue);
			case TypeCode.Decimal:
			{
				IEdmDecimalTypeReference type2 = (IEdmDecimalTypeReference)EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Decimal);
				return new EdmDecimalConstant(type2, (decimal)primitiveValue);
			}
			case TypeCode.DateTime:
			{
				IEdmTemporalTypeReference type3 = (IEdmTemporalTypeReference)EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.DateTime);
				return new EdmDateTimeConstant(type3, (DateTime)primitiveValue);
			}
			case TypeCode.String:
			{
				IEdmStringTypeReference type4 = (IEdmStringTypeReference)EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.String);
				return new EdmStringConstant(type4, (string)primitiveValue);
			}
			}
			return EdmValueUtils.ConvertPrimitiveValueWithoutTypeCode(primitiveValue, type);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002250 File Offset: 0x00000450
		internal static object ToClrValue(this IEdmPrimitiveValue edmValue)
		{
			EdmPrimitiveTypeKind primitiveKind = edmValue.Type.PrimitiveKind();
			switch (edmValue.ValueKind)
			{
			case EdmValueKind.Binary:
				return ((IEdmBinaryValue)edmValue).Value;
			case EdmValueKind.Boolean:
				return ((IEdmBooleanValue)edmValue).Value;
			case EdmValueKind.DateTimeOffset:
				return ((IEdmDateTimeOffsetValue)edmValue).Value;
			case EdmValueKind.DateTime:
				return ((IEdmDateTimeValue)edmValue).Value;
			case EdmValueKind.Decimal:
				return ((IEdmDecimalValue)edmValue).Value;
			case EdmValueKind.Floating:
				return EdmValueUtils.ConvertFloatingValue((IEdmFloatingValue)edmValue, primitiveKind);
			case EdmValueKind.Guid:
				return ((IEdmGuidValue)edmValue).Value;
			case EdmValueKind.Integer:
				return EdmValueUtils.ConvertIntegerValue((IEdmIntegerValue)edmValue, primitiveKind);
			case EdmValueKind.String:
				return ((IEdmStringValue)edmValue).Value;
			case EdmValueKind.Time:
				return ((IEdmTimeValue)edmValue).Value;
			}
			throw new ODataException(Strings.EdmValueUtils_CannotConvertTypeToClrValue(edmValue.ValueKind));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002360 File Offset: 0x00000560
		private static object ConvertFloatingValue(IEdmFloatingValue floatingValue, EdmPrimitiveTypeKind primitiveKind)
		{
			double value = floatingValue.Value;
			if (primitiveKind == EdmPrimitiveTypeKind.Single)
			{
				return Convert.ToSingle(value);
			}
			return value;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000238C File Offset: 0x0000058C
		private static object ConvertIntegerValue(IEdmIntegerValue integerValue, EdmPrimitiveTypeKind primitiveKind)
		{
			long value = integerValue.Value;
			if (primitiveKind != EdmPrimitiveTypeKind.Byte)
			{
				switch (primitiveKind)
				{
				case EdmPrimitiveTypeKind.Int16:
					return Convert.ToInt16(value);
				case EdmPrimitiveTypeKind.Int32:
					return Convert.ToInt32(value);
				case EdmPrimitiveTypeKind.SByte:
					return Convert.ToSByte(value);
				}
				return value;
			}
			return Convert.ToByte(value);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023F8 File Offset: 0x000005F8
		private static IEdmDelayedValue ConvertPrimitiveValueWithoutTypeCode(object primitiveValue, IEdmPrimitiveTypeReference type)
		{
			byte[] array = primitiveValue as byte[];
			if (array != null)
			{
				IEdmBinaryTypeReference type2 = (IEdmBinaryTypeReference)EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Binary);
				return new EdmBinaryConstant(type2, array);
			}
			if (primitiveValue is DateTimeOffset)
			{
				IEdmTemporalTypeReference type3 = (IEdmTemporalTypeReference)EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.DateTimeOffset);
				return new EdmDateTimeOffsetConstant(type3, (DateTimeOffset)primitiveValue);
			}
			if (primitiveValue is Guid)
			{
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Guid);
				return new EdmGuidConstant(type, (Guid)primitiveValue);
			}
			if (primitiveValue is TimeSpan)
			{
				IEdmTemporalTypeReference type4 = (IEdmTemporalTypeReference)EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Time);
				return new EdmTimeConstant(type4, (TimeSpan)primitiveValue);
			}
			if (primitiveValue is ISpatial)
			{
				throw new NotImplementedException();
			}
			IEdmDelayedValue result;
			if (EdmValueUtils.TryConvertClientSpecificPrimitiveValue(primitiveValue, type, out result))
			{
				return result;
			}
			throw new ODataException(Strings.EdmValueUtils_UnsupportedPrimitiveType(primitiveValue.GetType().FullName));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000024B8 File Offset: 0x000006B8
		private static bool TryConvertClientSpecificPrimitiveValue(object primitiveValue, IEdmPrimitiveTypeReference type, out IEdmDelayedValue convertedValue)
		{
			byte[] value;
			if (ClientConvert.TryConvertBinaryToByteArray(primitiveValue, out value))
			{
				type = EdmValueUtils.EnsurePrimitiveType(type, EdmPrimitiveTypeKind.Binary);
				convertedValue = new EdmBinaryConstant((IEdmBinaryTypeReference)type, value);
				return true;
			}
			PrimitiveType primitiveType;
			if (PrimitiveType.TryGetPrimitiveType(primitiveValue.GetType(), out primitiveType))
			{
				type = EdmValueUtils.EnsurePrimitiveType(type, primitiveType.PrimitiveKind);
				if (primitiveType.PrimitiveKind == EdmPrimitiveTypeKind.String)
				{
					convertedValue = new EdmStringConstant((IEdmStringTypeReference)type, primitiveType.TypeConverter.ToString(primitiveValue));
					return true;
				}
			}
			convertedValue = null;
			return false;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002530 File Offset: 0x00000730
		private static IEdmPrimitiveTypeReference EnsurePrimitiveType(IEdmPrimitiveTypeReference type, EdmPrimitiveTypeKind primitiveKindFromValue)
		{
			if (type == null)
			{
				type = EdmCoreModel.Instance.GetPrimitive(primitiveKindFromValue, true);
			}
			else
			{
				EdmPrimitiveTypeKind primitiveKind = type.PrimitiveDefinition().PrimitiveKind;
				if (primitiveKind != primitiveKindFromValue)
				{
					string text = type.FullName();
					if (text == null)
					{
						throw new ODataException(Strings.EdmValueUtils_IncorrectPrimitiveTypeKindNoTypeName(primitiveKind.ToString(), primitiveKindFromValue.ToString()));
					}
					throw new ODataException(Strings.EdmValueUtils_IncorrectPrimitiveTypeKind(text, primitiveKindFromValue.ToString(), primitiveKind.ToString()));
				}
			}
			return type;
		}
	}
}
