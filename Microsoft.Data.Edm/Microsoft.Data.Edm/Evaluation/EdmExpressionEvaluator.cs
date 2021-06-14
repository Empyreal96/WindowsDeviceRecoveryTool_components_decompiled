using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Evaluation
{
	// Token: 0x020000C4 RID: 196
	public class EdmExpressionEvaluator
	{
		// Token: 0x060003F8 RID: 1016 RVA: 0x0000A79B File Offset: 0x0000899B
		public EdmExpressionEvaluator(IDictionary<IEdmFunction, Func<IEdmValue[], IEdmValue>> builtInFunctions)
		{
			this.builtInFunctions = builtInFunctions;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000A7B5 File Offset: 0x000089B5
		public EdmExpressionEvaluator(IDictionary<IEdmFunction, Func<IEdmValue[], IEdmValue>> builtInFunctions, Func<string, IEdmValue[], IEdmValue> lastChanceFunctionApplier) : this(builtInFunctions)
		{
			this.lastChanceFunctionApplier = lastChanceFunctionApplier;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000A7C5 File Offset: 0x000089C5
		public IEdmValue Evaluate(IEdmExpression expression)
		{
			EdmUtil.CheckArgumentNull<IEdmExpression>(expression, "expression");
			return this.Eval(expression, null);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000A7DB File Offset: 0x000089DB
		public IEdmValue Evaluate(IEdmExpression expression, IEdmStructuredValue context)
		{
			EdmUtil.CheckArgumentNull<IEdmExpression>(expression, "expression");
			return this.Eval(expression, context);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000A7F1 File Offset: 0x000089F1
		public IEdmValue Evaluate(IEdmExpression expression, IEdmStructuredValue context, IEdmTypeReference targetType)
		{
			EdmUtil.CheckArgumentNull<IEdmExpression>(expression, "expression");
			EdmUtil.CheckArgumentNull<IEdmTypeReference>(targetType, "targetType");
			return EdmExpressionEvaluator.AssertType(targetType, this.Eval(expression, context));
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000A819 File Offset: 0x00008A19
		private static bool InRange(long value, long min, long max)
		{
			return value >= min && value <= max;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000A828 File Offset: 0x00008A28
		private static bool FitsInSingle(double value)
		{
			return value >= -3.4028234663852886E+38 && value <= 3.4028234663852886E+38;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000A847 File Offset: 0x00008A47
		private static bool MatchesType(IEdmTypeReference targetType, IEdmValue operand)
		{
			return EdmExpressionEvaluator.MatchesType(targetType, operand, true);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000A854 File Offset: 0x00008A54
		private static bool MatchesType(IEdmTypeReference targetType, IEdmValue operand, bool testPropertyTypes)
		{
			IEdmTypeReference type = operand.Type;
			EdmValueKind valueKind = operand.ValueKind;
			if (type != null && valueKind != EdmValueKind.Null && type.Definition.IsOrInheritsFrom(targetType.Definition))
			{
				return true;
			}
			switch (valueKind)
			{
			case EdmValueKind.Binary:
				if (targetType.IsBinary())
				{
					IEdmBinaryTypeReference edmBinaryTypeReference = targetType.AsBinary();
					return edmBinaryTypeReference.IsUnbounded || edmBinaryTypeReference.MaxLength == null || edmBinaryTypeReference.MaxLength.Value >= ((IEdmBinaryValue)operand).Value.Length;
				}
				break;
			case EdmValueKind.Boolean:
				return targetType.IsBoolean();
			case EdmValueKind.Collection:
				if (targetType.IsCollection())
				{
					IEdmTypeReference targetType2 = targetType.AsCollection().ElementType();
					foreach (IEdmDelayedValue edmDelayedValue in ((IEdmCollectionValue)operand).Elements)
					{
						if (!EdmExpressionEvaluator.MatchesType(targetType2, edmDelayedValue.Value))
						{
							return false;
						}
					}
					return true;
				}
				break;
			case EdmValueKind.DateTimeOffset:
				return targetType.IsDateTimeOffset();
			case EdmValueKind.DateTime:
				return targetType.IsDateTime();
			case EdmValueKind.Decimal:
				return targetType.IsDecimal();
			case EdmValueKind.Enum:
				return ((IEdmEnumValue)operand).Type.Definition.IsEquivalentTo(targetType.Definition);
			case EdmValueKind.Floating:
				return targetType.IsDouble() || (targetType.IsSingle() && EdmExpressionEvaluator.FitsInSingle(((IEdmFloatingValue)operand).Value));
			case EdmValueKind.Guid:
				return targetType.IsGuid();
			case EdmValueKind.Integer:
				if (targetType.TypeKind() == EdmTypeKind.Primitive)
				{
					switch (targetType.AsPrimitive().PrimitiveKind())
					{
					case EdmPrimitiveTypeKind.Byte:
						return EdmExpressionEvaluator.InRange(((IEdmIntegerValue)operand).Value, 0L, 255L);
					case EdmPrimitiveTypeKind.Double:
					case EdmPrimitiveTypeKind.Int64:
					case EdmPrimitiveTypeKind.Single:
						return true;
					case EdmPrimitiveTypeKind.Int16:
						return EdmExpressionEvaluator.InRange(((IEdmIntegerValue)operand).Value, -32768L, 32767L);
					case EdmPrimitiveTypeKind.Int32:
						return EdmExpressionEvaluator.InRange(((IEdmIntegerValue)operand).Value, -2147483648L, 2147483647L);
					case EdmPrimitiveTypeKind.SByte:
						return EdmExpressionEvaluator.InRange(((IEdmIntegerValue)operand).Value, -128L, 127L);
					}
				}
				break;
			case EdmValueKind.Null:
				return targetType.IsNullable;
			case EdmValueKind.String:
				if (targetType.IsString())
				{
					IEdmStringTypeReference edmStringTypeReference = targetType.AsString();
					return edmStringTypeReference.IsUnbounded || edmStringTypeReference.MaxLength == null || edmStringTypeReference.MaxLength.Value >= ((IEdmStringValue)operand).Value.Length;
				}
				break;
			case EdmValueKind.Structured:
				if (targetType.IsStructured())
				{
					return EdmExpressionEvaluator.AssertOrMatchStructuredType(targetType.AsStructured(), (IEdmStructuredValue)operand, testPropertyTypes, null);
				}
				break;
			case EdmValueKind.Time:
				return targetType.IsTime();
			}
			return false;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000AB38 File Offset: 0x00008D38
		private static IEdmValue AssertType(IEdmTypeReference targetType, IEdmValue operand)
		{
			IEdmTypeReference type = operand.Type;
			EdmValueKind valueKind = operand.ValueKind;
			if ((type != null && valueKind != EdmValueKind.Null && type.Definition.IsOrInheritsFrom(targetType.Definition)) || targetType.TypeKind() == EdmTypeKind.None)
			{
				return operand;
			}
			EdmValueKind edmValueKind = valueKind;
			bool flag;
			if (edmValueKind != EdmValueKind.Collection)
			{
				if (edmValueKind != EdmValueKind.Structured)
				{
					flag = EdmExpressionEvaluator.MatchesType(targetType, operand);
				}
				else if (targetType.IsStructured())
				{
					IEdmStructuredTypeReference edmStructuredTypeReference = targetType.AsStructured();
					List<IEdmPropertyValue> list = new List<IEdmPropertyValue>();
					flag = EdmExpressionEvaluator.AssertOrMatchStructuredType(edmStructuredTypeReference, (IEdmStructuredValue)operand, true, list);
					if (flag)
					{
						return new EdmStructuredValue(edmStructuredTypeReference, list);
					}
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				if (targetType.IsCollection())
				{
					return new EdmExpressionEvaluator.AssertTypeCollectionValue(targetType.AsCollection(), (IEdmCollectionValue)operand);
				}
				flag = false;
			}
			if (!flag)
			{
				throw new InvalidOperationException(Strings.Edm_Evaluator_FailedTypeAssertion(targetType.ToTraceString()));
			}
			return operand;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000ABFC File Offset: 0x00008DFC
		private static bool AssertOrMatchStructuredType(IEdmStructuredTypeReference structuredTargetType, IEdmStructuredValue structuredValue, bool testPropertyTypes, List<IEdmPropertyValue> newProperties)
		{
			IEdmTypeReference type = structuredValue.Type;
			if (type != null && type.TypeKind() != EdmTypeKind.Row && !structuredTargetType.StructuredDefinition().InheritsFrom(type.AsStructured().StructuredDefinition()))
			{
				return false;
			}
			HashSetInternal<IEdmPropertyValue> hashSetInternal = new HashSetInternal<IEdmPropertyValue>();
			foreach (IEdmProperty edmProperty in structuredTargetType.StructuralProperties())
			{
				IEdmPropertyValue edmPropertyValue = structuredValue.FindPropertyValue(edmProperty.Name);
				if (edmPropertyValue == null)
				{
					return false;
				}
				hashSetInternal.Add(edmPropertyValue);
				if (testPropertyTypes)
				{
					if (newProperties != null)
					{
						newProperties.Add(new EdmPropertyValue(edmPropertyValue.Name, EdmExpressionEvaluator.AssertType(edmProperty.Type, edmPropertyValue.Value)));
					}
					else if (!EdmExpressionEvaluator.MatchesType(edmProperty.Type, edmPropertyValue.Value))
					{
						return false;
					}
				}
			}
			if (structuredTargetType.IsEntity())
			{
				foreach (IEdmNavigationProperty edmNavigationProperty in structuredTargetType.AsEntity().NavigationProperties())
				{
					IEdmPropertyValue edmPropertyValue2 = structuredValue.FindPropertyValue(edmNavigationProperty.Name);
					if (edmPropertyValue2 == null)
					{
						return false;
					}
					if (testPropertyTypes && !EdmExpressionEvaluator.MatchesType(edmNavigationProperty.Type, edmPropertyValue2.Value, false))
					{
						return false;
					}
					hashSetInternal.Add(edmPropertyValue2);
					if (newProperties != null)
					{
						newProperties.Add(edmPropertyValue2);
					}
				}
			}
			if (newProperties != null)
			{
				foreach (IEdmPropertyValue item in structuredValue.PropertyValues)
				{
					if (!hashSetInternal.Contains(item))
					{
						newProperties.Add(item);
					}
				}
			}
			return true;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000ADCC File Offset: 0x00008FCC
		private IEdmValue Eval(IEdmExpression expression, IEdmStructuredValue context)
		{
			switch (expression.ExpressionKind)
			{
			case EdmExpressionKind.BinaryConstant:
				return (IEdmBinaryConstantExpression)expression;
			case EdmExpressionKind.BooleanConstant:
				return (IEdmBooleanConstantExpression)expression;
			case EdmExpressionKind.DateTimeConstant:
				return (IEdmDateTimeConstantExpression)expression;
			case EdmExpressionKind.DateTimeOffsetConstant:
				return (IEdmDateTimeOffsetConstantExpression)expression;
			case EdmExpressionKind.DecimalConstant:
				return (IEdmDecimalConstantExpression)expression;
			case EdmExpressionKind.FloatingConstant:
				return (IEdmFloatingConstantExpression)expression;
			case EdmExpressionKind.GuidConstant:
				return (IEdmGuidConstantExpression)expression;
			case EdmExpressionKind.IntegerConstant:
				return (IEdmIntegerConstantExpression)expression;
			case EdmExpressionKind.StringConstant:
				return (IEdmStringConstantExpression)expression;
			case EdmExpressionKind.TimeConstant:
				return (IEdmTimeConstantExpression)expression;
			case EdmExpressionKind.Null:
				return (IEdmNullExpression)expression;
			case EdmExpressionKind.Record:
			{
				IEdmRecordExpression edmRecordExpression = (IEdmRecordExpression)expression;
				EdmExpressionEvaluator.DelayedExpressionContext context2 = new EdmExpressionEvaluator.DelayedExpressionContext(this, context);
				List<IEdmPropertyValue> list = new List<IEdmPropertyValue>();
				foreach (IEdmPropertyConstructor constructor in edmRecordExpression.Properties)
				{
					list.Add(new EdmExpressionEvaluator.DelayedRecordProperty(context2, constructor));
				}
				return new EdmStructuredValue((edmRecordExpression.DeclaredType != null) ? edmRecordExpression.DeclaredType.AsStructured() : null, list);
			}
			case EdmExpressionKind.Collection:
			{
				IEdmCollectionExpression edmCollectionExpression = (IEdmCollectionExpression)expression;
				EdmExpressionEvaluator.DelayedExpressionContext delayedContext = new EdmExpressionEvaluator.DelayedExpressionContext(this, context);
				List<IEdmDelayedValue> list2 = new List<IEdmDelayedValue>();
				foreach (IEdmExpression expression2 in edmCollectionExpression.Elements)
				{
					list2.Add(this.MapLabeledExpressionToDelayedValue(expression2, delayedContext, context));
				}
				return new EdmCollectionValue((edmCollectionExpression.DeclaredType != null) ? edmCollectionExpression.DeclaredType.AsCollection() : null, list2);
			}
			case EdmExpressionKind.Path:
			{
				EdmUtil.CheckArgumentNull<IEdmStructuredValue>(context, "context");
				IEdmPathExpression edmPathExpression = (IEdmPathExpression)expression;
				IEdmValue edmValue = context;
				foreach (string text in edmPathExpression.Path)
				{
					edmValue = this.FindProperty(text, edmValue);
					if (edmValue == null)
					{
						throw new InvalidOperationException(Strings.Edm_Evaluator_UnboundPath(text));
					}
				}
				return edmValue;
			}
			case EdmExpressionKind.ParameterReference:
			case EdmExpressionKind.FunctionReference:
			case EdmExpressionKind.PropertyReference:
			case EdmExpressionKind.ValueTermReference:
			case EdmExpressionKind.EntitySetReference:
			case EdmExpressionKind.EnumMemberReference:
				throw new InvalidOperationException(Strings.Edm_Evaluator_UnrecognizedExpressionKind(((int)expression.ExpressionKind).ToString(CultureInfo.InvariantCulture)));
			case EdmExpressionKind.If:
			{
				IEdmIfExpression edmIfExpression = (IEdmIfExpression)expression;
				if (((IEdmBooleanValue)this.Eval(edmIfExpression.TestExpression, context)).Value)
				{
					return this.Eval(edmIfExpression.TrueExpression, context);
				}
				return this.Eval(edmIfExpression.FalseExpression, context);
			}
			case EdmExpressionKind.AssertType:
			{
				IEdmAssertTypeExpression edmAssertTypeExpression = (IEdmAssertTypeExpression)expression;
				IEdmValue operand = this.Eval(edmAssertTypeExpression.Operand, context);
				IEdmTypeReference type = edmAssertTypeExpression.Type;
				return EdmExpressionEvaluator.AssertType(type, operand);
			}
			case EdmExpressionKind.IsType:
			{
				IEdmIsTypeExpression edmIsTypeExpression = (IEdmIsTypeExpression)expression;
				IEdmValue operand2 = this.Eval(edmIsTypeExpression.Operand, context);
				IEdmTypeReference type2 = edmIsTypeExpression.Type;
				return new EdmBooleanConstant(EdmExpressionEvaluator.MatchesType(type2, operand2));
			}
			case EdmExpressionKind.FunctionApplication:
			{
				IEdmApplyExpression edmApplyExpression = (IEdmApplyExpression)expression;
				IEdmExpression appliedFunction = edmApplyExpression.AppliedFunction;
				IEdmFunctionReferenceExpression edmFunctionReferenceExpression = appliedFunction as IEdmFunctionReferenceExpression;
				if (edmFunctionReferenceExpression != null)
				{
					IList<IEdmExpression> list3 = edmApplyExpression.Arguments.ToList<IEdmExpression>();
					IEdmValue[] array = new IEdmValue[list3.Count<IEdmExpression>()];
					int num = 0;
					foreach (IEdmExpression expression3 in list3)
					{
						array[num++] = this.Eval(expression3, context);
					}
					IEdmFunction referencedFunction = edmFunctionReferenceExpression.ReferencedFunction;
					Func<IEdmValue[], IEdmValue> func;
					if (this.builtInFunctions.TryGetValue(referencedFunction, out func))
					{
						return func(array);
					}
					if (this.lastChanceFunctionApplier != null)
					{
						return this.lastChanceFunctionApplier(referencedFunction.FullName(), array);
					}
				}
				throw new InvalidOperationException(Strings.Edm_Evaluator_UnboundFunction((edmFunctionReferenceExpression != null) ? edmFunctionReferenceExpression.ReferencedFunction.ToTraceString() : string.Empty));
			}
			case EdmExpressionKind.LabeledExpressionReference:
				return this.MapLabeledExpressionToDelayedValue(((IEdmLabeledExpressionReferenceExpression)expression).ReferencedLabeledExpression, null, context).Value;
			case EdmExpressionKind.Labeled:
				return this.MapLabeledExpressionToDelayedValue(expression, new EdmExpressionEvaluator.DelayedExpressionContext(this, context), context).Value;
			default:
				throw new InvalidOperationException(Strings.Edm_Evaluator_UnrecognizedExpressionKind(((int)expression.ExpressionKind).ToString(CultureInfo.InvariantCulture)));
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000B214 File Offset: 0x00009414
		private IEdmDelayedValue MapLabeledExpressionToDelayedValue(IEdmExpression expression, EdmExpressionEvaluator.DelayedExpressionContext delayedContext, IEdmStructuredValue context)
		{
			IEdmLabeledExpression edmLabeledExpression = expression as IEdmLabeledExpression;
			if (edmLabeledExpression == null)
			{
				return new EdmExpressionEvaluator.DelayedCollectionElement(delayedContext, expression);
			}
			EdmExpressionEvaluator.DelayedValue delayedValue;
			if (this.labeledValues.TryGetValue(edmLabeledExpression, out delayedValue))
			{
				return delayedValue;
			}
			delayedValue = new EdmExpressionEvaluator.DelayedCollectionElement(delayedContext ?? new EdmExpressionEvaluator.DelayedExpressionContext(this, context), edmLabeledExpression.Expression);
			this.labeledValues[edmLabeledExpression] = delayedValue;
			return delayedValue;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000B26C File Offset: 0x0000946C
		private IEdmValue FindProperty(string name, IEdmValue context)
		{
			IEdmValue result = null;
			IEdmStructuredValue edmStructuredValue = context as IEdmStructuredValue;
			if (edmStructuredValue != null)
			{
				IEdmPropertyValue edmPropertyValue = edmStructuredValue.FindPropertyValue(name);
				if (edmPropertyValue != null)
				{
					result = edmPropertyValue.Value;
				}
			}
			return result;
		}

		// Token: 0x0400018B RID: 395
		private readonly IDictionary<IEdmFunction, Func<IEdmValue[], IEdmValue>> builtInFunctions;

		// Token: 0x0400018C RID: 396
		private readonly Dictionary<IEdmLabeledExpression, EdmExpressionEvaluator.DelayedValue> labeledValues = new Dictionary<IEdmLabeledExpression, EdmExpressionEvaluator.DelayedValue>();

		// Token: 0x0400018D RID: 397
		private readonly Func<string, IEdmValue[], IEdmValue> lastChanceFunctionApplier;

		// Token: 0x020000C5 RID: 197
		private class DelayedExpressionContext
		{
			// Token: 0x06000406 RID: 1030 RVA: 0x0000B298 File Offset: 0x00009498
			public DelayedExpressionContext(EdmExpressionEvaluator expressionEvaluator, IEdmStructuredValue context)
			{
				this.expressionEvaluator = expressionEvaluator;
				this.context = context;
			}

			// Token: 0x06000407 RID: 1031 RVA: 0x0000B2AE File Offset: 0x000094AE
			public IEdmValue Eval(IEdmExpression expression)
			{
				return this.expressionEvaluator.Eval(expression, this.context);
			}

			// Token: 0x0400018E RID: 398
			private readonly EdmExpressionEvaluator expressionEvaluator;

			// Token: 0x0400018F RID: 399
			private readonly IEdmStructuredValue context;
		}

		// Token: 0x020000C7 RID: 199
		private abstract class DelayedValue : IEdmDelayedValue
		{
			// Token: 0x06000409 RID: 1033 RVA: 0x0000B2C2 File Offset: 0x000094C2
			public DelayedValue(EdmExpressionEvaluator.DelayedExpressionContext context)
			{
				this.context = context;
			}

			// Token: 0x170001A7 RID: 423
			// (get) Token: 0x0600040A RID: 1034
			public abstract IEdmExpression Expression { get; }

			// Token: 0x170001A8 RID: 424
			// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000B2D1 File Offset: 0x000094D1
			public IEdmValue Value
			{
				get
				{
					if (this.value == null)
					{
						this.value = this.context.Eval(this.Expression);
					}
					return this.value;
				}
			}

			// Token: 0x04000190 RID: 400
			private readonly EdmExpressionEvaluator.DelayedExpressionContext context;

			// Token: 0x04000191 RID: 401
			private IEdmValue value;
		}

		// Token: 0x020000C9 RID: 201
		private class DelayedRecordProperty : EdmExpressionEvaluator.DelayedValue, IEdmPropertyValue, IEdmDelayedValue
		{
			// Token: 0x0600040D RID: 1037 RVA: 0x0000B2F8 File Offset: 0x000094F8
			public DelayedRecordProperty(EdmExpressionEvaluator.DelayedExpressionContext context, IEdmPropertyConstructor constructor) : base(context)
			{
				this.constructor = constructor;
			}

			// Token: 0x170001AA RID: 426
			// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000B308 File Offset: 0x00009508
			public string Name
			{
				get
				{
					return this.constructor.Name;
				}
			}

			// Token: 0x170001AB RID: 427
			// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000B315 File Offset: 0x00009515
			public override IEdmExpression Expression
			{
				get
				{
					return this.constructor.Value;
				}
			}

			// Token: 0x04000192 RID: 402
			private readonly IEdmPropertyConstructor constructor;
		}

		// Token: 0x020000CA RID: 202
		private class DelayedCollectionElement : EdmExpressionEvaluator.DelayedValue
		{
			// Token: 0x06000410 RID: 1040 RVA: 0x0000B322 File Offset: 0x00009522
			public DelayedCollectionElement(EdmExpressionEvaluator.DelayedExpressionContext context, IEdmExpression expression) : base(context)
			{
				this.expression = expression;
			}

			// Token: 0x170001AC RID: 428
			// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000B332 File Offset: 0x00009532
			public override IEdmExpression Expression
			{
				get
				{
					return this.expression;
				}
			}

			// Token: 0x04000193 RID: 403
			private readonly IEdmExpression expression;
		}

		// Token: 0x020000CC RID: 204
		private class AssertTypeCollectionValue : EdmElement, IEdmCollectionValue, IEdmValue, IEdmElement, IEnumerable<IEdmDelayedValue>, IEnumerable
		{
			// Token: 0x06000413 RID: 1043 RVA: 0x0000B33A File Offset: 0x0000953A
			public AssertTypeCollectionValue(IEdmCollectionTypeReference targetCollectionType, IEdmCollectionValue collectionValue)
			{
				this.targetCollectionType = targetCollectionType;
				this.collectionValue = collectionValue;
			}

			// Token: 0x170001AE RID: 430
			// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000B350 File Offset: 0x00009550
			IEnumerable<IEdmDelayedValue> IEdmCollectionValue.Elements
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170001AF RID: 431
			// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000B353 File Offset: 0x00009553
			IEdmTypeReference IEdmValue.Type
			{
				get
				{
					return this.targetCollectionType;
				}
			}

			// Token: 0x170001B0 RID: 432
			// (get) Token: 0x06000416 RID: 1046 RVA: 0x0000B35B File Offset: 0x0000955B
			EdmValueKind IEdmValue.ValueKind
			{
				get
				{
					return EdmValueKind.Collection;
				}
			}

			// Token: 0x06000417 RID: 1047 RVA: 0x0000B35E File Offset: 0x0000955E
			IEnumerator<IEdmDelayedValue> IEnumerable<IEdmDelayedValue>.GetEnumerator()
			{
				return new EdmExpressionEvaluator.AssertTypeCollectionValue.AssertTypeCollectionValueEnumerator(this);
			}

			// Token: 0x06000418 RID: 1048 RVA: 0x0000B366 File Offset: 0x00009566
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new EdmExpressionEvaluator.AssertTypeCollectionValue.AssertTypeCollectionValueEnumerator(this);
			}

			// Token: 0x04000194 RID: 404
			private readonly IEdmCollectionTypeReference targetCollectionType;

			// Token: 0x04000195 RID: 405
			private readonly IEdmCollectionValue collectionValue;

			// Token: 0x020000CD RID: 205
			private class AssertTypeCollectionValueEnumerator : IEnumerator<IEdmDelayedValue>, IDisposable, IEnumerator
			{
				// Token: 0x06000419 RID: 1049 RVA: 0x0000B36E File Offset: 0x0000956E
				public AssertTypeCollectionValueEnumerator(EdmExpressionEvaluator.AssertTypeCollectionValue value)
				{
					this.value = value;
					this.enumerator = value.collectionValue.Elements.GetEnumerator();
				}

				// Token: 0x170001B1 RID: 433
				// (get) Token: 0x0600041A RID: 1050 RVA: 0x0000B393 File Offset: 0x00009593
				public IEdmDelayedValue Current
				{
					get
					{
						return new EdmExpressionEvaluator.AssertTypeCollectionValue.AssertTypeCollectionValueEnumerator.DelayedAssertType(this.value.targetCollectionType.ElementType(), this.enumerator.Current);
					}
				}

				// Token: 0x170001B2 RID: 434
				// (get) Token: 0x0600041B RID: 1051 RVA: 0x0000B3B5 File Offset: 0x000095B5
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x0600041C RID: 1052 RVA: 0x0000B3BD File Offset: 0x000095BD
				bool IEnumerator.MoveNext()
				{
					return this.enumerator.MoveNext();
				}

				// Token: 0x0600041D RID: 1053 RVA: 0x0000B3CA File Offset: 0x000095CA
				void IEnumerator.Reset()
				{
					this.enumerator.Reset();
				}

				// Token: 0x0600041E RID: 1054 RVA: 0x0000B3D7 File Offset: 0x000095D7
				void IDisposable.Dispose()
				{
					this.enumerator.Dispose();
				}

				// Token: 0x04000196 RID: 406
				private readonly EdmExpressionEvaluator.AssertTypeCollectionValue value;

				// Token: 0x04000197 RID: 407
				private readonly IEnumerator<IEdmDelayedValue> enumerator;

				// Token: 0x020000CE RID: 206
				private class DelayedAssertType : IEdmDelayedValue
				{
					// Token: 0x0600041F RID: 1055 RVA: 0x0000B3E4 File Offset: 0x000095E4
					public DelayedAssertType(IEdmTypeReference targetType, IEdmDelayedValue value)
					{
						this.delayedValue = value;
						this.targetType = targetType;
					}

					// Token: 0x170001B3 RID: 435
					// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000B3FA File Offset: 0x000095FA
					public IEdmValue Value
					{
						get
						{
							if (this.value == null)
							{
								this.value = EdmExpressionEvaluator.AssertType(this.targetType, this.delayedValue.Value);
							}
							return this.value;
						}
					}

					// Token: 0x04000198 RID: 408
					private readonly IEdmDelayedValue delayedValue;

					// Token: 0x04000199 RID: 409
					private readonly IEdmTypeReference targetType;

					// Token: 0x0400019A RID: 410
					private IEdmValue value;
				}
			}
		}
	}
}
