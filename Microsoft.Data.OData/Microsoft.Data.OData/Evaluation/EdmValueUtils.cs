using System;
using System.Spatial;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000161 RID: 353
	internal static class EdmValueUtils
	{
		// Token: 0x060009B2 RID: 2482 RVA: 0x0001EA9C File Offset: 0x0001CC9C
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

		// Token: 0x060009B3 RID: 2483 RVA: 0x0001EC18 File Offset: 0x0001CE18
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

		// Token: 0x060009B4 RID: 2484 RVA: 0x0001ED28 File Offset: 0x0001CF28
		internal static bool TryGetStreamProperty(IEdmStructuredValue entityInstance, string streamPropertyName, out IEdmProperty streamProperty)
		{
			streamProperty = null;
			if (streamPropertyName != null)
			{
				streamProperty = entityInstance.Type.AsEntity().FindProperty(streamPropertyName);
				if (streamProperty == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0001ED4C File Offset: 0x0001CF4C
		internal static object GetPrimitivePropertyClrValue(this IEdmStructuredValue structuredValue, string propertyName)
		{
			IEdmStructuredTypeReference type = structuredValue.Type.AsStructured();
			IEdmPropertyValue edmPropertyValue = structuredValue.FindPropertyValue(propertyName);
			if (edmPropertyValue == null)
			{
				throw new ODataException(Strings.EdmValueUtils_PropertyDoesntExist(type.FullName(), propertyName));
			}
			if (edmPropertyValue.Value.ValueKind == EdmValueKind.Null)
			{
				return null;
			}
			IEdmPrimitiveValue edmPrimitiveValue = edmPropertyValue.Value as IEdmPrimitiveValue;
			if (edmPrimitiveValue == null)
			{
				throw new ODataException(Strings.EdmValueUtils_NonPrimitiveValue(edmPropertyValue.Name, type.FullName()));
			}
			return edmPrimitiveValue.ToClrValue();
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0001EDC0 File Offset: 0x0001CFC0
		private static object ConvertFloatingValue(IEdmFloatingValue floatingValue, EdmPrimitiveTypeKind primitiveKind)
		{
			double value = floatingValue.Value;
			if (primitiveKind == EdmPrimitiveTypeKind.Single)
			{
				return Convert.ToSingle(value);
			}
			return value;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001EDEC File Offset: 0x0001CFEC
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

		// Token: 0x060009B8 RID: 2488 RVA: 0x0001EE58 File Offset: 0x0001D058
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
			throw new ODataException(Strings.EdmValueUtils_UnsupportedPrimitiveType(primitiveValue.GetType().FullName));
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0001EF0C File Offset: 0x0001D10C
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
