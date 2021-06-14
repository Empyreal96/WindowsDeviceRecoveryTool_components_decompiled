using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000D0 RID: 208
	public static class EdmElementComparer
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x0000B4D8 File Offset: 0x000096D8
		public static bool IsEquivalentTo(this IEdmType thisType, IEdmType otherType)
		{
			if (thisType == otherType)
			{
				return true;
			}
			if (thisType == null || otherType == null)
			{
				return false;
			}
			if (thisType.TypeKind != otherType.TypeKind)
			{
				return false;
			}
			switch (thisType.TypeKind)
			{
			case EdmTypeKind.None:
				return otherType.TypeKind == EdmTypeKind.None;
			case EdmTypeKind.Primitive:
				return ((IEdmPrimitiveType)thisType).IsEquivalentTo((IEdmPrimitiveType)otherType);
			case EdmTypeKind.Entity:
			case EdmTypeKind.Complex:
			case EdmTypeKind.Enum:
				return ((IEdmSchemaType)thisType).IsEquivalentTo((IEdmSchemaType)otherType);
			case EdmTypeKind.Row:
				return ((IEdmRowType)thisType).IsEquivalentTo((IEdmRowType)otherType);
			case EdmTypeKind.Collection:
				return ((IEdmCollectionType)thisType).IsEquivalentTo((IEdmCollectionType)otherType);
			case EdmTypeKind.EntityReference:
				return ((IEdmEntityReferenceType)thisType).IsEquivalentTo((IEdmEntityReferenceType)otherType);
			default:
				throw new InvalidOperationException(Strings.UnknownEnumVal_TypeKind(thisType.TypeKind));
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000B5AC File Offset: 0x000097AC
		public static bool IsEquivalentTo(this IEdmTypeReference thisType, IEdmTypeReference otherType)
		{
			if (thisType == otherType)
			{
				return true;
			}
			if (thisType == null || otherType == null)
			{
				return false;
			}
			EdmTypeKind edmTypeKind = thisType.TypeKind();
			if (edmTypeKind != otherType.TypeKind())
			{
				return false;
			}
			if (edmTypeKind == EdmTypeKind.Primitive)
			{
				return ((IEdmPrimitiveTypeReference)thisType).IsEquivalentTo((IEdmPrimitiveTypeReference)otherType);
			}
			return thisType.IsNullable == otherType.IsNullable && thisType.Definition.IsEquivalentTo(otherType.Definition);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000B610 File Offset: 0x00009810
		internal static bool IsFunctionSignatureEquivalentTo(this IEdmFunctionBase thisFunction, IEdmFunctionBase otherFunction)
		{
			if (thisFunction == otherFunction)
			{
				return true;
			}
			if (thisFunction.Name != otherFunction.Name)
			{
				return false;
			}
			if (!thisFunction.ReturnType.IsEquivalentTo(otherFunction.ReturnType))
			{
				return false;
			}
			IEnumerator<IEdmFunctionParameter> enumerator = otherFunction.Parameters.GetEnumerator();
			foreach (IEdmFunctionParameter thisParameter in thisFunction.Parameters)
			{
				enumerator.MoveNext();
				if (!thisParameter.IsEquivalentTo(enumerator.Current))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000B6B0 File Offset: 0x000098B0
		private static bool IsEquivalentTo(this IEdmFunctionParameter thisParameter, IEdmFunctionParameter otherParameter)
		{
			return thisParameter == otherParameter || (thisParameter != null && otherParameter != null && (thisParameter.Name == otherParameter.Name && thisParameter.Mode == otherParameter.Mode) && thisParameter.Type.IsEquivalentTo(otherParameter.Type));
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000B6FF File Offset: 0x000098FF
		private static bool IsEquivalentTo(this IEdmPrimitiveType thisType, IEdmPrimitiveType otherType)
		{
			return thisType.PrimitiveKind == otherType.PrimitiveKind && thisType.FullName() == otherType.FullName();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000B722 File Offset: 0x00009922
		private static bool IsEquivalentTo(this IEdmSchemaType thisType, IEdmSchemaType otherType)
		{
			return object.ReferenceEquals(thisType, otherType);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000B72B File Offset: 0x0000992B
		private static bool IsEquivalentTo(this IEdmCollectionType thisType, IEdmCollectionType otherType)
		{
			return thisType.ElementType.IsEquivalentTo(otherType.ElementType);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000B73E File Offset: 0x0000993E
		private static bool IsEquivalentTo(this IEdmEntityReferenceType thisType, IEdmEntityReferenceType otherType)
		{
			return thisType.EntityType.IsEquivalentTo(otherType.EntityType);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000B754 File Offset: 0x00009954
		private static bool IsEquivalentTo(this IEdmRowType thisType, IEdmRowType otherType)
		{
			if (thisType.DeclaredProperties.Count<IEdmProperty>() != otherType.DeclaredProperties.Count<IEdmProperty>())
			{
				return false;
			}
			IEnumerator<IEdmProperty> enumerator = thisType.DeclaredProperties.GetEnumerator();
			foreach (IEdmProperty edmProperty in otherType.DeclaredProperties)
			{
				enumerator.MoveNext();
				if (!((IEdmStructuralProperty)enumerator.Current).IsEquivalentTo((IEdmStructuralProperty)edmProperty))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000B7E8 File Offset: 0x000099E8
		private static bool IsEquivalentTo(this IEdmStructuralProperty thisProp, IEdmStructuralProperty otherProp)
		{
			return thisProp == otherProp || (thisProp != null && otherProp != null && thisProp.Name == otherProp.Name && thisProp.Type.IsEquivalentTo(otherProp.Type));
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000B820 File Offset: 0x00009A20
		private static bool IsEquivalentTo(this IEdmPrimitiveTypeReference thisType, IEdmPrimitiveTypeReference otherType)
		{
			EdmPrimitiveTypeKind edmPrimitiveTypeKind = thisType.PrimitiveKind();
			if (edmPrimitiveTypeKind != otherType.PrimitiveKind())
			{
				return false;
			}
			EdmPrimitiveTypeKind edmPrimitiveTypeKind2 = edmPrimitiveTypeKind;
			switch (edmPrimitiveTypeKind2)
			{
			case EdmPrimitiveTypeKind.Binary:
				return ((IEdmBinaryTypeReference)thisType).IsEquivalentTo((IEdmBinaryTypeReference)otherType);
			case EdmPrimitiveTypeKind.Boolean:
			case EdmPrimitiveTypeKind.Byte:
				goto IL_93;
			case EdmPrimitiveTypeKind.DateTime:
			case EdmPrimitiveTypeKind.DateTimeOffset:
				break;
			case EdmPrimitiveTypeKind.Decimal:
				return ((IEdmDecimalTypeReference)thisType).IsEquivalentTo((IEdmDecimalTypeReference)otherType);
			default:
				switch (edmPrimitiveTypeKind2)
				{
				case EdmPrimitiveTypeKind.String:
					return ((IEdmStringTypeReference)thisType).IsEquivalentTo((IEdmStringTypeReference)otherType);
				case EdmPrimitiveTypeKind.Stream:
					goto IL_93;
				case EdmPrimitiveTypeKind.Time:
					break;
				default:
					goto IL_93;
				}
				break;
			}
			return ((IEdmTemporalTypeReference)thisType).IsEquivalentTo((IEdmTemporalTypeReference)otherType);
			IL_93:
			if (edmPrimitiveTypeKind.IsSpatial())
			{
				return ((IEdmSpatialTypeReference)thisType).IsEquivalentTo((IEdmSpatialTypeReference)otherType);
			}
			return thisType.IsNullable == otherType.IsNullable && thisType.Definition.IsEquivalentTo(otherType.Definition);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000B8FC File Offset: 0x00009AFC
		private static bool IsEquivalentTo(this IEdmBinaryTypeReference thisType, IEdmBinaryTypeReference otherType)
		{
			return thisType.IsNullable == otherType.IsNullable && thisType.IsFixedLength == otherType.IsFixedLength && thisType.IsUnbounded == otherType.IsUnbounded && thisType.MaxLength == otherType.MaxLength;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000B98C File Offset: 0x00009B8C
		private static bool IsEquivalentTo(this IEdmDecimalTypeReference thisType, IEdmDecimalTypeReference otherType)
		{
			return thisType.IsNullable == otherType.IsNullable && thisType.Precision == otherType.Precision && thisType.Scale == otherType.Scale;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000BA0C File Offset: 0x00009C0C
		private static bool IsEquivalentTo(this IEdmTemporalTypeReference thisType, IEdmTemporalTypeReference otherType)
		{
			return thisType.TypeKind() == otherType.TypeKind() && thisType.IsNullable == otherType.IsNullable && thisType.Precision == otherType.Precision;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000BA68 File Offset: 0x00009C68
		private static bool IsEquivalentTo(this IEdmStringTypeReference thisType, IEdmStringTypeReference otherType)
		{
			return thisType.IsNullable == otherType.IsNullable && thisType.IsFixedLength == otherType.IsFixedLength && thisType.IsUnbounded == otherType.IsUnbounded && thisType.MaxLength == otherType.MaxLength && thisType.IsUnicode == otherType.IsUnicode && thisType.Collation == otherType.Collation;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000BB48 File Offset: 0x00009D48
		private static bool IsEquivalentTo(this IEdmSpatialTypeReference thisType, IEdmSpatialTypeReference otherType)
		{
			return thisType.IsNullable == otherType.IsNullable && thisType.SpatialReferenceIdentifier == otherType.SpatialReferenceIdentifier;
		}
	}
}
