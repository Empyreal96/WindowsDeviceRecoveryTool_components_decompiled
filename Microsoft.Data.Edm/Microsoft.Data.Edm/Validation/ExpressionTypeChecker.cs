using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Validation
{
	// Token: 0x02000124 RID: 292
	public static class ExpressionTypeChecker
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x0000DBCE File Offset: 0x0000BDCE
		public static bool TryAssertType(this IEdmExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			return expression.TryAssertType(type, null, false, out discoveredErrors);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0000DBDC File Offset: 0x0000BDDC
		public static bool TryAssertType(this IEdmExpression expression, IEdmTypeReference type, IEdmType context, bool matchExactly, out IEnumerable<EdmError> discoveredErrors)
		{
			EdmUtil.CheckArgumentNull<IEdmExpression>(expression, "expression");
			if (type == null || type.TypeKind() == EdmTypeKind.None)
			{
				discoveredErrors = Enumerable.Empty<EdmError>();
				return true;
			}
			switch (expression.ExpressionKind)
			{
			case EdmExpressionKind.BinaryConstant:
			case EdmExpressionKind.BooleanConstant:
			case EdmExpressionKind.DateTimeConstant:
			case EdmExpressionKind.DateTimeOffsetConstant:
			case EdmExpressionKind.DecimalConstant:
			case EdmExpressionKind.FloatingConstant:
			case EdmExpressionKind.GuidConstant:
			case EdmExpressionKind.IntegerConstant:
			case EdmExpressionKind.StringConstant:
			case EdmExpressionKind.TimeConstant:
			{
				IEdmPrimitiveValue edmPrimitiveValue = (IEdmPrimitiveValue)expression;
				if (edmPrimitiveValue.Type != null)
				{
					return edmPrimitiveValue.Type.TestTypeReferenceMatch(type, expression.Location(), matchExactly, out discoveredErrors);
				}
				return edmPrimitiveValue.TryAssertPrimitiveAsType(type, out discoveredErrors);
			}
			case EdmExpressionKind.Null:
				return ((IEdmNullExpression)expression).TryAssertNullAsType(type, out discoveredErrors);
			case EdmExpressionKind.Record:
			{
				IEdmRecordExpression edmRecordExpression = (IEdmRecordExpression)expression;
				if (edmRecordExpression.DeclaredType != null)
				{
					return edmRecordExpression.DeclaredType.TestTypeReferenceMatch(type, expression.Location(), matchExactly, out discoveredErrors);
				}
				return edmRecordExpression.TryAssertRecordAsType(type, context, matchExactly, out discoveredErrors);
			}
			case EdmExpressionKind.Collection:
			{
				IEdmCollectionExpression edmCollectionExpression = (IEdmCollectionExpression)expression;
				if (edmCollectionExpression.DeclaredType != null)
				{
					return edmCollectionExpression.DeclaredType.TestTypeReferenceMatch(type, expression.Location(), matchExactly, out discoveredErrors);
				}
				return edmCollectionExpression.TryAssertCollectionAsType(type, context, matchExactly, out discoveredErrors);
			}
			case EdmExpressionKind.Path:
				return ((IEdmPathExpression)expression).TryAssertPathAsType(type, context, matchExactly, out discoveredErrors);
			case EdmExpressionKind.If:
				return ((IEdmIfExpression)expression).TryAssertIfAsType(type, context, matchExactly, out discoveredErrors);
			case EdmExpressionKind.AssertType:
				return ((IEdmAssertTypeExpression)expression).Type.TestTypeReferenceMatch(type, expression.Location(), matchExactly, out discoveredErrors);
			case EdmExpressionKind.IsType:
				return EdmCoreModel.Instance.GetBoolean(false).TestTypeReferenceMatch(type, expression.Location(), matchExactly, out discoveredErrors);
			case EdmExpressionKind.FunctionApplication:
			{
				IEdmApplyExpression edmApplyExpression = (IEdmApplyExpression)expression;
				if (edmApplyExpression.AppliedFunction != null)
				{
					IEdmFunctionBase edmFunctionBase = edmApplyExpression.AppliedFunction as IEdmFunctionBase;
					if (edmFunctionBase != null)
					{
						return edmFunctionBase.ReturnType.TestTypeReferenceMatch(type, expression.Location(), matchExactly, out discoveredErrors);
					}
				}
				discoveredErrors = Enumerable.Empty<EdmError>();
				return true;
			}
			case EdmExpressionKind.LabeledExpressionReference:
				return ((IEdmLabeledExpressionReferenceExpression)expression).ReferencedLabeledExpression.TryAssertType(type, out discoveredErrors);
			case EdmExpressionKind.Labeled:
				return ((IEdmLabeledExpression)expression).Expression.TryAssertType(type, context, matchExactly, out discoveredErrors);
			}
			discoveredErrors = new EdmError[]
			{
				new EdmError(expression.Location(), EdmErrorCode.ExpressionNotValidForTheAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionNotValidForTheAssertedType)
			};
			return false;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0000DE14 File Offset: 0x0000C014
		internal static bool TryAssertPrimitiveAsType(this IEdmPrimitiveValue expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsPrimitive())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.PrimitiveConstantExpressionNotValidForNonPrimitiveType, Strings.EdmModel_Validator_Semantic_PrimitiveConstantExpressionNotValidForNonPrimitiveType)
				};
				return false;
			}
			switch (expression.ValueKind)
			{
			case EdmValueKind.Binary:
				return ExpressionTypeChecker.TryAssertBinaryConstantAsType((IEdmBinaryConstantExpression)expression, type, out discoveredErrors);
			case EdmValueKind.Boolean:
				return ExpressionTypeChecker.TryAssertBooleanConstantAsType((IEdmBooleanConstantExpression)expression, type, out discoveredErrors);
			case EdmValueKind.DateTimeOffset:
				return ExpressionTypeChecker.TryAssertDateTimeOffsetConstantAsType((IEdmDateTimeOffsetConstantExpression)expression, type, out discoveredErrors);
			case EdmValueKind.DateTime:
				return ExpressionTypeChecker.TryAssertDateTimeConstantAsType((IEdmDateTimeConstantExpression)expression, type, out discoveredErrors);
			case EdmValueKind.Decimal:
				return ExpressionTypeChecker.TryAssertDecimalConstantAsType((IEdmDecimalConstantExpression)expression, type, out discoveredErrors);
			case EdmValueKind.Floating:
				return ExpressionTypeChecker.TryAssertFloatingConstantAsType((IEdmFloatingConstantExpression)expression, type, out discoveredErrors);
			case EdmValueKind.Guid:
				return ExpressionTypeChecker.TryAssertGuidConstantAsType((IEdmGuidConstantExpression)expression, type, out discoveredErrors);
			case EdmValueKind.Integer:
				return ExpressionTypeChecker.TryAssertIntegerConstantAsType((IEdmIntegerConstantExpression)expression, type, out discoveredErrors);
			case EdmValueKind.String:
				return ExpressionTypeChecker.TryAssertStringConstantAsType((IEdmStringConstantExpression)expression, type, out discoveredErrors);
			case EdmValueKind.Time:
				return ExpressionTypeChecker.TryAssertTimeConstantAsType((IEdmTimeConstantExpression)expression, type, out discoveredErrors);
			}
			discoveredErrors = new EdmError[]
			{
				new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
			};
			return false;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0000DF48 File Offset: 0x0000C148
		internal static bool TryAssertNullAsType(this IEdmNullExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsNullable)
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.NullCannotBeAssertedToBeANonNullableType, Strings.EdmModel_Validator_Semantic_NullCannotBeAssertedToBeANonNullableType)
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0000DF8C File Offset: 0x0000C18C
		internal static bool TryAssertPathAsType(this IEdmPathExpression expression, IEdmTypeReference type, IEdmType context, bool matchExactly, out IEnumerable<EdmError> discoveredErrors)
		{
			IEdmStructuredType edmStructuredType = context as IEdmStructuredType;
			if (edmStructuredType != null)
			{
				IEdmType edmType = context;
				foreach (string text in expression.Path)
				{
					IEdmStructuredType edmStructuredType2 = edmType as IEdmStructuredType;
					if (edmStructuredType2 == null)
					{
						discoveredErrors = new EdmError[]
						{
							new EdmError(expression.Location(), EdmErrorCode.PathIsNotValidForTheGivenContext, Strings.EdmModel_Validator_Semantic_PathIsNotValidForTheGivenContext(text))
						};
						return false;
					}
					IEdmProperty edmProperty = edmStructuredType2.FindProperty(text);
					edmType = ((edmProperty != null) ? edmProperty.Type.Definition : null);
					if (edmType == null)
					{
						discoveredErrors = Enumerable.Empty<EdmError>();
						return true;
					}
				}
				return edmType.TestTypeMatch(type.Definition, expression.Location(), matchExactly, out discoveredErrors);
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0000E06C File Offset: 0x0000C26C
		internal static bool TryAssertIfAsType(this IEdmIfExpression expression, IEdmTypeReference type, IEdmType context, bool matchExactly, out IEnumerable<EdmError> discoveredErrors)
		{
			IEnumerable<EdmError> collection;
			bool flag = expression.TrueExpression.TryAssertType(type, context, matchExactly, out collection);
			IEnumerable<EdmError> collection2;
			flag = (expression.FalseExpression.TryAssertType(type, context, matchExactly, out collection2) && flag);
			if (!flag)
			{
				List<EdmError> list = new List<EdmError>(collection);
				list.AddRange(collection2);
				discoveredErrors = list;
			}
			else
			{
				discoveredErrors = Enumerable.Empty<EdmError>();
			}
			return flag;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0000E0E4 File Offset: 0x0000C2E4
		internal static bool TryAssertRecordAsType(this IEdmRecordExpression expression, IEdmTypeReference type, IEdmType context, bool matchExactly, out IEnumerable<EdmError> discoveredErrors)
		{
			EdmUtil.CheckArgumentNull<IEdmRecordExpression>(expression, "expression");
			EdmUtil.CheckArgumentNull<IEdmTypeReference>(type, "type");
			if (!type.IsStructured())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.RecordExpressionNotValidForNonStructuredType, Strings.EdmModel_Validator_Semantic_RecordExpressionNotValidForNonStructuredType)
				};
				return false;
			}
			HashSetInternal<string> hashSetInternal = new HashSetInternal<string>();
			List<EdmError> list = new List<EdmError>();
			IEdmStructuredTypeReference type2 = type.AsStructured();
			using (IEnumerator<IEdmProperty> enumerator = type2.StructuredDefinition().Properties().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IEdmProperty typeProperty = enumerator.Current;
					IEdmPropertyConstructor edmPropertyConstructor = expression.Properties.FirstOrDefault((IEdmPropertyConstructor p) => p.Name == typeProperty.Name);
					if (edmPropertyConstructor == null)
					{
						list.Add(new EdmError(expression.Location(), EdmErrorCode.RecordExpressionMissingRequiredProperty, Strings.EdmModel_Validator_Semantic_RecordExpressionMissingProperty(typeProperty.Name)));
					}
					else
					{
						IEnumerable<EdmError> enumerable;
						if (!edmPropertyConstructor.Value.TryAssertType(typeProperty.Type, context, matchExactly, out enumerable))
						{
							foreach (EdmError item in enumerable)
							{
								list.Add(item);
							}
						}
						hashSetInternal.Add(typeProperty.Name);
					}
				}
			}
			if (!type2.IsOpen())
			{
				foreach (IEdmPropertyConstructor edmPropertyConstructor2 in expression.Properties)
				{
					if (!hashSetInternal.Contains(edmPropertyConstructor2.Name))
					{
						list.Add(new EdmError(expression.Location(), EdmErrorCode.RecordExpressionHasExtraProperties, Strings.EdmModel_Validator_Semantic_RecordExpressionHasExtraProperties(edmPropertyConstructor2.Name)));
					}
				}
			}
			if (list.FirstOrDefault<EdmError>() != null)
			{
				discoveredErrors = list;
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0000E2EC File Offset: 0x0000C4EC
		internal static bool TryAssertCollectionAsType(this IEdmCollectionExpression expression, IEdmTypeReference type, IEdmType context, bool matchExactly, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsCollection())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.CollectionExpressionNotValidForNonCollectionType, Strings.EdmModel_Validator_Semantic_CollectionExpressionNotValidForNonCollectionType)
				};
				return false;
			}
			IEdmTypeReference type2 = type.AsCollection().ElementType();
			bool flag = true;
			List<EdmError> list = new List<EdmError>();
			foreach (IEdmExpression expression2 in expression.Elements)
			{
				IEnumerable<EdmError> collection;
				flag = (expression2.TryAssertType(type2, context, matchExactly, out collection) && flag);
				list.AddRange(collection);
			}
			discoveredErrors = list;
			return flag;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0000E39C File Offset: 0x0000C59C
		private static bool TryAssertGuidConstantAsType(IEdmGuidConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsGuid())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0000E3E0 File Offset: 0x0000C5E0
		private static bool TryAssertFloatingConstantAsType(IEdmFloatingConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsFloating())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0000E424 File Offset: 0x0000C624
		private static bool TryAssertDecimalConstantAsType(IEdmDecimalConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsDecimal())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0000E468 File Offset: 0x0000C668
		private static bool TryAssertDateTimeOffsetConstantAsType(IEdmDateTimeOffsetConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsDateTimeOffset())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0000E4AC File Offset: 0x0000C6AC
		private static bool TryAssertDateTimeConstantAsType(IEdmDateTimeConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsDateTime())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0000E4F0 File Offset: 0x0000C6F0
		private static bool TryAssertTimeConstantAsType(IEdmTimeConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsTime())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0000E534 File Offset: 0x0000C734
		private static bool TryAssertBooleanConstantAsType(IEdmBooleanConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsBoolean())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0000E578 File Offset: 0x0000C778
		private static bool TryAssertStringConstantAsType(IEdmStringConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsString())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			IEdmStringTypeReference edmStringTypeReference = type.AsString();
			if (edmStringTypeReference.MaxLength != null && expression.Value.Length > edmStringTypeReference.MaxLength.Value)
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.StringConstantLengthOutOfRange, Strings.EdmModel_Validator_Semantic_StringConstantLengthOutOfRange(expression.Value.Length, edmStringTypeReference.MaxLength.Value))
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0000E638 File Offset: 0x0000C838
		private static bool TryAssertIntegerConstantAsType(IEdmIntegerConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsIntegral())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			EdmPrimitiveTypeKind edmPrimitiveTypeKind = type.PrimitiveKind();
			if (edmPrimitiveTypeKind == EdmPrimitiveTypeKind.Byte)
			{
				return ExpressionTypeChecker.TryAssertIntegerConstantInRange(expression, 0L, 255L, out discoveredErrors);
			}
			switch (edmPrimitiveTypeKind)
			{
			case EdmPrimitiveTypeKind.Int16:
				return ExpressionTypeChecker.TryAssertIntegerConstantInRange(expression, -32768L, 32767L, out discoveredErrors);
			case EdmPrimitiveTypeKind.Int32:
				return ExpressionTypeChecker.TryAssertIntegerConstantInRange(expression, -2147483648L, 2147483647L, out discoveredErrors);
			case EdmPrimitiveTypeKind.Int64:
				return ExpressionTypeChecker.TryAssertIntegerConstantInRange(expression, long.MinValue, long.MaxValue, out discoveredErrors);
			case EdmPrimitiveTypeKind.SByte:
				return ExpressionTypeChecker.TryAssertIntegerConstantInRange(expression, -128L, 127L, out discoveredErrors);
			default:
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0000E71C File Offset: 0x0000C91C
		private static bool TryAssertIntegerConstantInRange(IEdmIntegerConstantExpression expression, long min, long max, out IEnumerable<EdmError> discoveredErrors)
		{
			if (expression.Value < min || expression.Value > max)
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.IntegerConstantValueOutOfRange, Strings.EdmModel_Validator_Semantic_IntegerConstantValueOutOfRange)
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0000E768 File Offset: 0x0000C968
		private static bool TryAssertBinaryConstantAsType(IEdmBinaryConstantExpression expression, IEdmTypeReference type, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!type.IsBinary())
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType)
				};
				return false;
			}
			IEdmBinaryTypeReference edmBinaryTypeReference = type.AsBinary();
			if (edmBinaryTypeReference.MaxLength != null && expression.Value.Length > edmBinaryTypeReference.MaxLength.Value)
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(expression.Location(), EdmErrorCode.BinaryConstantLengthOutOfRange, Strings.EdmModel_Validator_Semantic_BinaryConstantLengthOutOfRange(expression.Value.Length, edmBinaryTypeReference.MaxLength.Value))
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0000E820 File Offset: 0x0000CA20
		private static bool TestTypeReferenceMatch(this IEdmTypeReference expressionType, IEdmTypeReference assertedType, EdmLocation location, bool matchExactly, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!expressionType.TestNullabilityMatch(assertedType, location, out discoveredErrors))
			{
				return false;
			}
			if (expressionType.IsBad())
			{
				discoveredErrors = Enumerable.Empty<EdmError>();
				return true;
			}
			return expressionType.Definition.TestTypeMatch(assertedType.Definition, location, matchExactly, out discoveredErrors);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0000E858 File Offset: 0x0000CA58
		private static bool TestTypeMatch(this IEdmType expressionType, IEdmType assertedType, EdmLocation location, bool matchExactly, out IEnumerable<EdmError> discoveredErrors)
		{
			if (matchExactly)
			{
				if (!expressionType.IsEquivalentTo(assertedType))
				{
					discoveredErrors = new EdmError[]
					{
						new EdmError(location, EdmErrorCode.ExpressionNotValidForTheAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionNotValidForTheAssertedType)
					};
					return false;
				}
			}
			else
			{
				if (expressionType.TypeKind == EdmTypeKind.None || expressionType.IsBad())
				{
					discoveredErrors = Enumerable.Empty<EdmError>();
					return true;
				}
				if (expressionType.TypeKind == EdmTypeKind.Primitive && assertedType.TypeKind == EdmTypeKind.Primitive)
				{
					IEdmPrimitiveType edmPrimitiveType = expressionType as IEdmPrimitiveType;
					IEdmPrimitiveType edmPrimitiveType2 = assertedType as IEdmPrimitiveType;
					if (!edmPrimitiveType.PrimitiveKind.PromotesTo(edmPrimitiveType2.PrimitiveKind))
					{
						discoveredErrors = new EdmError[]
						{
							new EdmError(location, EdmErrorCode.ExpressionPrimitiveKindNotValidForAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionPrimitiveKindCannotPromoteToAssertedType(expressionType.ToTraceString(), assertedType.ToTraceString()))
						};
						return false;
					}
				}
				else if (!expressionType.IsOrInheritsFrom(assertedType))
				{
					discoveredErrors = new EdmError[]
					{
						new EdmError(location, EdmErrorCode.ExpressionNotValidForTheAssertedType, Strings.EdmModel_Validator_Semantic_ExpressionNotValidForTheAssertedType)
					};
					return false;
				}
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0000E944 File Offset: 0x0000CB44
		private static bool TestNullabilityMatch(this IEdmTypeReference expressionType, IEdmTypeReference assertedType, EdmLocation location, out IEnumerable<EdmError> discoveredErrors)
		{
			if (!assertedType.IsNullable && expressionType.IsNullable)
			{
				discoveredErrors = new EdmError[]
				{
					new EdmError(location, EdmErrorCode.CannotAssertNullableTypeAsNonNullableType, Strings.EdmModel_Validator_Semantic_CannotAssertNullableTypeAsNonNullableType(expressionType.FullName()))
				};
				return false;
			}
			discoveredErrors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0000E98E File Offset: 0x0000CB8E
		private static bool PromotesTo(this EdmPrimitiveTypeKind startingKind, EdmPrimitiveTypeKind target)
		{
			return startingKind == target || ExpressionTypeChecker.promotionMap[(int)startingKind, (int)target];
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0000E9AC File Offset: 0x0000CBAC
		private static bool[,] InitializePromotionMap()
		{
			int num = (from f in typeof(EdmPrimitiveTypeKind).GetFields()
			where f.IsLiteral
			select f).Count<FieldInfo>();
			bool[,] array = new bool[num, num];
			array[3, 9] = true;
			array[3, 10] = true;
			array[3, 11] = true;
			array[12, 9] = true;
			array[12, 10] = true;
			array[12, 11] = true;
			array[9, 10] = true;
			array[9, 11] = true;
			array[10, 11] = true;
			array[13, 7] = true;
			array[21, 17] = true;
			array[19, 17] = true;
			array[23, 17] = true;
			array[24, 17] = true;
			array[22, 17] = true;
			array[18, 17] = true;
			array[20, 17] = true;
			array[29, 25] = true;
			array[27, 25] = true;
			array[31, 25] = true;
			array[32, 25] = true;
			array[30, 25] = true;
			array[26, 25] = true;
			array[28, 25] = true;
			return array;
		}

		// Token: 0x04000208 RID: 520
		private static readonly bool[,] promotionMap = ExpressionTypeChecker.InitializePromotionMap();
	}
}
