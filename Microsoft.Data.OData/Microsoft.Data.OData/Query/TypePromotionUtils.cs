using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000BC RID: 188
	internal static class TypePromotionUtils
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x0000EBE8 File Offset: 0x0000CDE8
		internal static bool PromoteOperandTypes(BinaryOperatorKind operatorKind, ref IEdmTypeReference left, ref IEdmTypeReference right)
		{
			if (left == null && right == null)
			{
				return true;
			}
			if ((operatorKind == BinaryOperatorKind.NotEqual || operatorKind == BinaryOperatorKind.Equal) && TypePromotionUtils.TryHandleEqualityOperatorForEntityOrComplexTypes(ref left, ref right))
			{
				return true;
			}
			if ((left != null && !left.IsODataPrimitiveTypeKind()) || (right != null && !right.IsODataPrimitiveTypeKind()))
			{
				return false;
			}
			FunctionSignature[] functionSignatures = TypePromotionUtils.GetFunctionSignatures(operatorKind);
			IEdmTypeReference[] array = new IEdmTypeReference[]
			{
				left,
				right
			};
			bool flag = TypePromotionUtils.FindBestSignature(functionSignatures, array) == 1;
			if (flag)
			{
				left = array[0];
				right = array[1];
				if (left == null)
				{
					left = right;
				}
				else if (right == null)
				{
					right = left;
				}
			}
			return flag;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000EC74 File Offset: 0x0000CE74
		internal static bool PromoteOperandType(UnaryOperatorKind operatorKind, ref IEdmTypeReference typeReference)
		{
			if (typeReference == null)
			{
				return true;
			}
			FunctionSignature[] functionSignatures = TypePromotionUtils.GetFunctionSignatures(operatorKind);
			IEdmTypeReference[] array = new IEdmTypeReference[]
			{
				typeReference
			};
			bool flag = TypePromotionUtils.FindBestSignature(functionSignatures, array) == 1;
			if (flag)
			{
				typeReference = array[0];
			}
			return flag;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000ECB0 File Offset: 0x0000CEB0
		internal static FunctionSignatureWithReturnType FindBestFunctionSignature(FunctionSignatureWithReturnType[] functions, IEdmTypeReference[] argumentTypes)
		{
			List<FunctionSignatureWithReturnType> list = new List<FunctionSignatureWithReturnType>(functions.Length);
			foreach (FunctionSignatureWithReturnType functionSignatureWithReturnType in functions)
			{
				if (functionSignatureWithReturnType.ArgumentTypes.Length == argumentTypes.Length)
				{
					bool flag = true;
					for (int j = 0; j < functionSignatureWithReturnType.ArgumentTypes.Length; j++)
					{
						if (!TypePromotionUtils.CanPromoteTo(argumentTypes[j], functionSignatureWithReturnType.ArgumentTypes[j]))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						list.Add(functionSignatureWithReturnType);
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			int num = -1;
			for (int k = 0; k < list.Count; k++)
			{
				bool flag2 = true;
				for (int l = 0; l < list.Count; l++)
				{
					if (k != l && TypePromotionUtils.MatchesArgumentTypesBetterThan(argumentTypes, list[l].ArgumentTypes, list[k].ArgumentTypes))
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					if (num != -1)
					{
						return null;
					}
					num = k;
				}
			}
			if (num == -1)
			{
				return null;
			}
			return list[num];
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000EDBC File Offset: 0x0000CFBC
		internal static FunctionSignature FindExactFunctionSignature(FunctionSignature[] functions, IEdmTypeReference[] argumentTypes)
		{
			foreach (FunctionSignature functionSignature in functions)
			{
				bool flag = true;
				if (functionSignature.ArgumentTypes.Length == argumentTypes.Length)
				{
					for (int j = 0; j < argumentTypes.Length; j++)
					{
						IEdmTypeReference otherType = functionSignature.ArgumentTypes[j];
						IEdmTypeReference edmTypeReference = argumentTypes[j];
						if (!edmTypeReference.IsODataPrimitiveTypeKind())
						{
							flag = false;
							break;
						}
						if (!edmTypeReference.IsEquivalentTo(otherType))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return functionSignature;
					}
				}
			}
			return null;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000EE2C File Offset: 0x0000D02C
		internal static bool CanConvertTo(IEdmTypeReference sourceReference, IEdmTypeReference targetReference)
		{
			if (sourceReference.IsEquivalentTo(targetReference))
			{
				return true;
			}
			if (targetReference.IsODataComplexTypeKind() || targetReference.IsODataEntityTypeKind())
			{
				return ((IEdmStructuredType)targetReference.Definition).IsAssignableFrom((IEdmStructuredType)sourceReference.Definition);
			}
			IEdmPrimitiveTypeReference edmPrimitiveTypeReference = sourceReference.AsPrimitiveOrNull();
			IEdmPrimitiveTypeReference edmPrimitiveTypeReference2 = targetReference.AsPrimitiveOrNull();
			return edmPrimitiveTypeReference != null && edmPrimitiveTypeReference2 != null && MetadataUtilsCommon.CanConvertPrimitiveTypeTo(edmPrimitiveTypeReference.PrimitiveDefinition(), edmPrimitiveTypeReference2.PrimitiveDefinition());
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000EE98 File Offset: 0x0000D098
		private static FunctionSignature[] GetFunctionSignatures(BinaryOperatorKind operatorKind)
		{
			switch (operatorKind)
			{
			case BinaryOperatorKind.Or:
			case BinaryOperatorKind.And:
				return TypePromotionUtils.logicalSignatures;
			case BinaryOperatorKind.Equal:
			case BinaryOperatorKind.NotEqual:
			case BinaryOperatorKind.GreaterThan:
			case BinaryOperatorKind.GreaterThanOrEqual:
			case BinaryOperatorKind.LessThan:
			case BinaryOperatorKind.LessThanOrEqual:
				return TypePromotionUtils.relationalSignatures;
			case BinaryOperatorKind.Add:
			case BinaryOperatorKind.Subtract:
			case BinaryOperatorKind.Multiply:
			case BinaryOperatorKind.Divide:
			case BinaryOperatorKind.Modulo:
				return TypePromotionUtils.arithmeticSignatures;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.TypePromotionUtils_GetFunctionSignatures_Binary_UnreachableCodepath));
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000EF08 File Offset: 0x0000D108
		private static FunctionSignature[] GetFunctionSignatures(UnaryOperatorKind operatorKind)
		{
			switch (operatorKind)
			{
			case UnaryOperatorKind.Negate:
				return TypePromotionUtils.negationSignatures;
			case UnaryOperatorKind.Not:
				return TypePromotionUtils.notSignatures;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.TypePromotionUtils_GetFunctionSignatures_Unary_UnreachableCodepath));
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000EF5C File Offset: 0x0000D15C
		private static int FindBestSignature(FunctionSignature[] signatures, IEdmTypeReference[] argumentTypes)
		{
			List<FunctionSignature> list = (from signature in signatures
			where TypePromotionUtils.IsApplicable(signature, argumentTypes)
			select signature).ToList<FunctionSignature>();
			if (list.Count > 1)
			{
				list = TypePromotionUtils.FindBestApplicableSignatures(list, argumentTypes);
			}
			int num = list.Count;
			if (num == 1)
			{
				FunctionSignature functionSignature = list[0];
				for (int i = 0; i < argumentTypes.Length; i++)
				{
					argumentTypes[i] = functionSignature.ArgumentTypes[i];
				}
				num = 1;
			}
			else if (num > 1 && argumentTypes.Length == 2 && num == 2 && list[0].ArgumentTypes[0].Definition.IsEquivalentTo(list[1].ArgumentTypes[0].Definition))
			{
				FunctionSignature functionSignature2 = list[0].ArgumentTypes[0].IsNullable ? list[0] : list[1];
				argumentTypes[0] = functionSignature2.ArgumentTypes[0];
				argumentTypes[1] = functionSignature2.ArgumentTypes[1];
				return TypePromotionUtils.FindBestSignature(signatures, argumentTypes);
			}
			return num;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000F090 File Offset: 0x0000D290
		private static bool IsApplicable(FunctionSignature signature, IEdmTypeReference[] argumentTypes)
		{
			if (signature.ArgumentTypes.Length != argumentTypes.Length)
			{
				return false;
			}
			for (int i = 0; i < argumentTypes.Length; i++)
			{
				if (!TypePromotionUtils.CanPromoteTo(argumentTypes[i], signature.ArgumentTypes[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000F0D0 File Offset: 0x0000D2D0
		private static bool CanPromoteTo(IEdmTypeReference sourceType, IEdmTypeReference targetType)
		{
			if (sourceType == null)
			{
				return targetType.IsNullable;
			}
			if (sourceType.IsEquivalentTo(targetType))
			{
				return true;
			}
			if (TypePromotionUtils.CanConvertTo(sourceType, targetType))
			{
				return true;
			}
			if (sourceType.IsNullable && targetType.IsODataValueType())
			{
				IEdmTypeReference sourceReference = sourceType.Definition.ToTypeReference(false);
				if (TypePromotionUtils.CanConvertTo(sourceReference, targetType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000F128 File Offset: 0x0000D328
		private static List<FunctionSignature> FindBestApplicableSignatures(List<FunctionSignature> signatures, IEdmTypeReference[] argumentTypes)
		{
			List<FunctionSignature> list = new List<FunctionSignature>();
			foreach (FunctionSignature functionSignature in signatures)
			{
				bool flag = true;
				foreach (FunctionSignature functionSignature2 in signatures)
				{
					if (functionSignature2 != functionSignature && TypePromotionUtils.MatchesArgumentTypesBetterThan(argumentTypes, functionSignature2.ArgumentTypes, functionSignature.ArgumentTypes))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(functionSignature);
				}
			}
			return list;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000F1D8 File Offset: 0x0000D3D8
		private static bool MatchesArgumentTypesBetterThan(IEdmTypeReference[] argumentTypes, IEdmTypeReference[] firstCandidate, IEdmTypeReference[] secondCandidate)
		{
			bool result = false;
			for (int i = 0; i < argumentTypes.Length; i++)
			{
				if (argumentTypes[i] != null)
				{
					int num = TypePromotionUtils.CompareConversions(argumentTypes[i], firstCandidate[i], secondCandidate[i]);
					if (num < 0)
					{
						return false;
					}
					if (num > 0)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000F218 File Offset: 0x0000D418
		private static int CompareConversions(IEdmTypeReference source, IEdmTypeReference targetA, IEdmTypeReference targetB)
		{
			if (targetA.IsEquivalentTo(targetB))
			{
				return 0;
			}
			if (source.IsEquivalentTo(targetA))
			{
				return 1;
			}
			if (source.IsEquivalentTo(targetB))
			{
				return -1;
			}
			bool flag = TypePromotionUtils.CanConvertTo(targetA, targetB);
			bool flag2 = TypePromotionUtils.CanConvertTo(targetB, targetA);
			if (flag && !flag2)
			{
				return 1;
			}
			if (flag2 && !flag)
			{
				return -1;
			}
			bool isNullable = source.IsNullable;
			bool isNullable2 = targetA.IsNullable;
			bool isNullable3 = targetB.IsNullable;
			if (isNullable == isNullable2 && isNullable != isNullable3)
			{
				return 1;
			}
			if (isNullable != isNullable2 && isNullable == isNullable3)
			{
				return -1;
			}
			if (TypePromotionUtils.IsSignedIntegralType(targetA) && TypePromotionUtils.IsUnsignedIntegralType(targetB))
			{
				return 1;
			}
			if (TypePromotionUtils.IsSignedIntegralType(targetB) && TypePromotionUtils.IsUnsignedIntegralType(targetA))
			{
				return -1;
			}
			if (TypePromotionUtils.IsDecimalType(targetA) && TypePromotionUtils.IsDoubleOrSingle(targetB))
			{
				return 1;
			}
			if (TypePromotionUtils.IsDecimalType(targetB) && TypePromotionUtils.IsDoubleOrSingle(targetA))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000F2DC File Offset: 0x0000D4DC
		private static bool TryHandleEqualityOperatorForEntityOrComplexTypes(ref IEdmTypeReference left, ref IEdmTypeReference right)
		{
			if (left != null && left.IsStructured())
			{
				if (right == null)
				{
					right = left;
					return true;
				}
				if (!right.IsStructured())
				{
					return false;
				}
				if (left.Definition.IsEquivalentTo(right.Definition))
				{
					if (left.IsNullable && !right.IsNullable)
					{
						right = left;
					}
					else
					{
						left = right;
					}
					return true;
				}
				if (TypePromotionUtils.CanConvertTo(left, right))
				{
					left = right;
					return true;
				}
				if (TypePromotionUtils.CanConvertTo(right, left))
				{
					right = left;
					return true;
				}
				return false;
			}
			else
			{
				if (right == null || !right.IsStructured())
				{
					return false;
				}
				if (left == null)
				{
					left = right;
					return true;
				}
				return false;
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000F37C File Offset: 0x0000D57C
		private static bool IsSignedIntegralType(IEdmTypeReference typeReference)
		{
			return TypePromotionUtils.GetNumericTypeKind(typeReference) == TypePromotionUtils.NumericTypeKind.SignedIntegral;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000F387 File Offset: 0x0000D587
		private static bool IsUnsignedIntegralType(IEdmTypeReference typeReference)
		{
			return TypePromotionUtils.GetNumericTypeKind(typeReference) == TypePromotionUtils.NumericTypeKind.UnsignedIntegral;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000F394 File Offset: 0x0000D594
		private static bool IsDecimalType(IEdmTypeReference typeReference)
		{
			IEdmPrimitiveTypeReference edmPrimitiveTypeReference = typeReference.AsPrimitiveOrNull();
			return edmPrimitiveTypeReference != null && edmPrimitiveTypeReference.PrimitiveKind() == EdmPrimitiveTypeKind.Decimal;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000F3B8 File Offset: 0x0000D5B8
		private static bool IsDoubleOrSingle(IEdmTypeReference typeReference)
		{
			IEdmPrimitiveTypeReference edmPrimitiveTypeReference = typeReference.AsPrimitiveOrNull();
			if (edmPrimitiveTypeReference != null)
			{
				EdmPrimitiveTypeKind edmPrimitiveTypeKind = edmPrimitiveTypeReference.PrimitiveKind();
				return edmPrimitiveTypeKind == EdmPrimitiveTypeKind.Double || edmPrimitiveTypeKind == EdmPrimitiveTypeKind.Single;
			}
			return false;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000F3E4 File Offset: 0x0000D5E4
		private static TypePromotionUtils.NumericTypeKind GetNumericTypeKind(IEdmTypeReference typeReference)
		{
			IEdmPrimitiveTypeReference edmPrimitiveTypeReference = typeReference.AsPrimitiveOrNull();
			if (edmPrimitiveTypeReference == null)
			{
				return TypePromotionUtils.NumericTypeKind.NotNumeric;
			}
			switch (edmPrimitiveTypeReference.PrimitiveDefinition().PrimitiveKind)
			{
			case EdmPrimitiveTypeKind.Byte:
				return TypePromotionUtils.NumericTypeKind.UnsignedIntegral;
			case EdmPrimitiveTypeKind.Decimal:
			case EdmPrimitiveTypeKind.Double:
			case EdmPrimitiveTypeKind.Single:
				return TypePromotionUtils.NumericTypeKind.NotIntegral;
			case EdmPrimitiveTypeKind.Int16:
			case EdmPrimitiveTypeKind.Int32:
			case EdmPrimitiveTypeKind.Int64:
			case EdmPrimitiveTypeKind.SByte:
				return TypePromotionUtils.NumericTypeKind.SignedIntegral;
			}
			return TypePromotionUtils.NumericTypeKind.NotNumeric;
		}

		// Token: 0x04000189 RID: 393
		private static readonly FunctionSignature[] logicalSignatures = new FunctionSignature[]
		{
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetBoolean(false),
				EdmCoreModel.Instance.GetBoolean(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetBoolean(true),
				EdmCoreModel.Instance.GetBoolean(true)
			})
		};

		// Token: 0x0400018A RID: 394
		private static readonly FunctionSignature[] notSignatures = new FunctionSignature[]
		{
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetBoolean(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetBoolean(true)
			})
		};

		// Token: 0x0400018B RID: 395
		private static readonly FunctionSignature[] arithmeticSignatures = new FunctionSignature[]
		{
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt32(false),
				EdmCoreModel.Instance.GetInt32(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt32(true),
				EdmCoreModel.Instance.GetInt32(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt64(false),
				EdmCoreModel.Instance.GetInt64(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt64(true),
				EdmCoreModel.Instance.GetInt64(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetSingle(false),
				EdmCoreModel.Instance.GetSingle(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetSingle(true),
				EdmCoreModel.Instance.GetSingle(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDouble(false),
				EdmCoreModel.Instance.GetDouble(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDouble(true),
				EdmCoreModel.Instance.GetDouble(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDecimal(false),
				EdmCoreModel.Instance.GetDecimal(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDecimal(true),
				EdmCoreModel.Instance.GetDecimal(true)
			})
		};

		// Token: 0x0400018C RID: 396
		private static readonly FunctionSignature[] relationalSignatures = new FunctionSignature[]
		{
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt32(false),
				EdmCoreModel.Instance.GetInt32(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt32(true),
				EdmCoreModel.Instance.GetInt32(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt64(false),
				EdmCoreModel.Instance.GetInt64(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt64(true),
				EdmCoreModel.Instance.GetInt64(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetSingle(false),
				EdmCoreModel.Instance.GetSingle(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetSingle(true),
				EdmCoreModel.Instance.GetSingle(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDouble(false),
				EdmCoreModel.Instance.GetDouble(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDouble(true),
				EdmCoreModel.Instance.GetDouble(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDecimal(false),
				EdmCoreModel.Instance.GetDecimal(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDecimal(true),
				EdmCoreModel.Instance.GetDecimal(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetString(true),
				EdmCoreModel.Instance.GetString(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetBinary(true),
				EdmCoreModel.Instance.GetBinary(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetBoolean(false),
				EdmCoreModel.Instance.GetBoolean(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetBoolean(true),
				EdmCoreModel.Instance.GetBoolean(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetGuid(false),
				EdmCoreModel.Instance.GetGuid(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetGuid(true),
				EdmCoreModel.Instance.GetGuid(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTime, false),
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTime, false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTime, true),
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTime, true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTimeOffset, false),
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTimeOffset, false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTimeOffset, true),
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTimeOffset, true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.Time, false),
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.Time, false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.Time, true),
				EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.Time, true)
			})
		};

		// Token: 0x0400018D RID: 397
		private static readonly FunctionSignature[] negationSignatures = new FunctionSignature[]
		{
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt32(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt32(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt64(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetInt64(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetSingle(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetSingle(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDouble(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDouble(true)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDecimal(false)
			}),
			new FunctionSignature(new IEdmTypeReference[]
			{
				EdmCoreModel.Instance.GetDecimal(true)
			})
		};

		// Token: 0x020000BD RID: 189
		private enum NumericTypeKind
		{
			// Token: 0x0400018F RID: 399
			NotNumeric,
			// Token: 0x04000190 RID: 400
			NotIntegral,
			// Token: 0x04000191 RID: 401
			SignedIntegral,
			// Token: 0x04000192 RID: 402
			UnsignedIntegral
		}
	}
}
