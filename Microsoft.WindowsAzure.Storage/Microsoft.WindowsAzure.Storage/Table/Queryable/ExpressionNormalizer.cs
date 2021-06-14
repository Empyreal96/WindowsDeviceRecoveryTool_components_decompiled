using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000111 RID: 273
	internal class ExpressionNormalizer : DataServiceALinqExpressionVisitor
	{
		// Token: 0x060012DF RID: 4831 RVA: 0x000467A5 File Offset: 0x000449A5
		private ExpressionNormalizer(Dictionary<Expression, Expression> normalizerRewrites)
		{
			this.normalizerRewrites = normalizerRewrites;
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x000467C4 File Offset: 0x000449C4
		internal Dictionary<Expression, Expression> NormalizerRewrites
		{
			get
			{
				return this.normalizerRewrites;
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x000467CC File Offset: 0x000449CC
		internal static Expression Normalize(Expression expression, Dictionary<Expression, Expression> rewrites)
		{
			ExpressionNormalizer expressionNormalizer = new ExpressionNormalizer(rewrites);
			return expressionNormalizer.Visit(expression);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x000467EC File Offset: 0x000449EC
		internal override Expression VisitBinary(BinaryExpression b)
		{
			BinaryExpression binaryExpression = (BinaryExpression)base.VisitBinary(b);
			if (binaryExpression.NodeType == ExpressionType.Equal)
			{
				Expression expression = ExpressionNormalizer.UnwrapObjectConvert(binaryExpression.Left);
				Expression expression2 = ExpressionNormalizer.UnwrapObjectConvert(binaryExpression.Right);
				if (expression != binaryExpression.Left || expression2 != binaryExpression.Right)
				{
					binaryExpression = ExpressionNormalizer.CreateRelationalOperator(ExpressionType.Equal, expression, expression2);
				}
			}
			ExpressionNormalizer.Pattern pattern;
			if (this.patterns.TryGetValue(binaryExpression.Left, out pattern) && pattern.Kind == ExpressionNormalizer.PatternKind.Compare && ExpressionNormalizer.IsConstantZero(binaryExpression.Right))
			{
				ExpressionNormalizer.ComparePattern comparePattern = (ExpressionNormalizer.ComparePattern)pattern;
				BinaryExpression binaryExpression2;
				if (ExpressionNormalizer.TryCreateRelationalOperator(binaryExpression.NodeType, comparePattern.Left, comparePattern.Right, out binaryExpression2))
				{
					binaryExpression = binaryExpression2;
				}
			}
			this.RecordRewrite(b, binaryExpression);
			return binaryExpression;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x000468A0 File Offset: 0x00044AA0
		internal override Expression VisitUnary(UnaryExpression u)
		{
			UnaryExpression unaryExpression = (UnaryExpression)base.VisitUnary(u);
			Expression expression = unaryExpression;
			this.RecordRewrite(u, expression);
			return expression;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x000468C8 File Offset: 0x00044AC8
		private static Expression UnwrapObjectConvert(Expression input)
		{
			if (input.NodeType == ExpressionType.Constant && input.Type == typeof(object))
			{
				ConstantExpression constantExpression = (ConstantExpression)input;
				if (constantExpression.Value != null && constantExpression.Value.GetType() != typeof(object))
				{
					return Expression.Constant(constantExpression.Value, constantExpression.Value.GetType());
				}
			}
			while (ExpressionType.Convert == input.NodeType && typeof(object) == input.Type)
			{
				input = ((UnaryExpression)input).Operand;
			}
			return input;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00046967 File Offset: 0x00044B67
		private static bool IsConstantZero(Expression expression)
		{
			return expression.NodeType == ExpressionType.Constant && ((ConstantExpression)expression).Value.Equals(0);
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0004698C File Offset: 0x00044B8C
		internal override Expression VisitMethodCall(MethodCallExpression call)
		{
			Expression expression = this.VisitMethodCallNoRewrite(call);
			this.RecordRewrite(call, expression);
			return expression;
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x000469AC File Offset: 0x00044BAC
		internal Expression VisitMethodCallNoRewrite(MethodCallExpression call)
		{
			MethodCallExpression methodCallExpression = (MethodCallExpression)base.VisitMethodCall(call);
			if (methodCallExpression.Method.IsStatic && methodCallExpression.Method.Name.StartsWith("op_", StringComparison.Ordinal))
			{
				string name;
				if (methodCallExpression.Arguments.Count == 2 && (name = methodCallExpression.Method.Name) != null)
				{
					if (<PrivateImplementationDetails>{C5AE8AAC-86EB-4B18-9730-1255943A81F7}.$$method0x600123a-1 == null)
					{
						<PrivateImplementationDetails>{C5AE8AAC-86EB-4B18-9730-1255943A81F7}.$$method0x600123a-1 = new Dictionary<string, int>(14)
						{
							{
								"op_Equality",
								0
							},
							{
								"op_Inequality",
								1
							},
							{
								"op_GreaterThan",
								2
							},
							{
								"op_GreaterThanOrEqual",
								3
							},
							{
								"op_LessThan",
								4
							},
							{
								"op_LessThanOrEqual",
								5
							},
							{
								"op_Multiply",
								6
							},
							{
								"op_Subtraction",
								7
							},
							{
								"op_Addition",
								8
							},
							{
								"op_Division",
								9
							},
							{
								"op_Modulus",
								10
							},
							{
								"op_BitwiseAnd",
								11
							},
							{
								"op_BitwiseOr",
								12
							},
							{
								"op_ExclusiveOr",
								13
							}
						};
					}
					int num;
					if (<PrivateImplementationDetails>{C5AE8AAC-86EB-4B18-9730-1255943A81F7}.$$method0x600123a-1.TryGetValue(name, out num))
					{
						switch (num)
						{
						case 0:
							return Expression.Equal(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], false, methodCallExpression.Method);
						case 1:
							return Expression.NotEqual(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], false, methodCallExpression.Method);
						case 2:
							return Expression.GreaterThan(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], false, methodCallExpression.Method);
						case 3:
							return Expression.GreaterThanOrEqual(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], false, methodCallExpression.Method);
						case 4:
							return Expression.LessThan(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], false, methodCallExpression.Method);
						case 5:
							return Expression.LessThanOrEqual(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], false, methodCallExpression.Method);
						case 6:
							return Expression.Multiply(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], methodCallExpression.Method);
						case 7:
							return Expression.Subtract(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], methodCallExpression.Method);
						case 8:
							return Expression.Add(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], methodCallExpression.Method);
						case 9:
							return Expression.Divide(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], methodCallExpression.Method);
						case 10:
							return Expression.Modulo(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], methodCallExpression.Method);
						case 11:
							return Expression.And(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], methodCallExpression.Method);
						case 12:
							return Expression.Or(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], methodCallExpression.Method);
						case 13:
							return Expression.ExclusiveOr(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], methodCallExpression.Method);
						}
					}
				}
				string name2;
				if (methodCallExpression.Arguments.Count == 1 && (name2 = methodCallExpression.Method.Name) != null)
				{
					if (<PrivateImplementationDetails>{C5AE8AAC-86EB-4B18-9730-1255943A81F7}.$$method0x600123a-2 == null)
					{
						<PrivateImplementationDetails>{C5AE8AAC-86EB-4B18-9730-1255943A81F7}.$$method0x600123a-2 = new Dictionary<string, int>(6)
						{
							{
								"op_UnaryNegation",
								0
							},
							{
								"op_UnaryPlus",
								1
							},
							{
								"op_Explicit",
								2
							},
							{
								"op_Implicit",
								3
							},
							{
								"op_OnesComplement",
								4
							},
							{
								"op_False",
								5
							}
						};
					}
					int num2;
					if (<PrivateImplementationDetails>{C5AE8AAC-86EB-4B18-9730-1255943A81F7}.$$method0x600123a-2.TryGetValue(name2, out num2))
					{
						switch (num2)
						{
						case 0:
							return Expression.Negate(methodCallExpression.Arguments[0], methodCallExpression.Method);
						case 1:
							return Expression.UnaryPlus(methodCallExpression.Arguments[0], methodCallExpression.Method);
						case 2:
						case 3:
							return Expression.Convert(methodCallExpression.Arguments[0], methodCallExpression.Type, methodCallExpression.Method);
						case 4:
						case 5:
							return Expression.Not(methodCallExpression.Arguments[0], methodCallExpression.Method);
						}
					}
				}
			}
			if (methodCallExpression.Method.IsStatic && methodCallExpression.Method.Name == "Equals" && methodCallExpression.Arguments.Count > 1)
			{
				return Expression.Equal(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], false, methodCallExpression.Method);
			}
			if (!methodCallExpression.Method.IsStatic && methodCallExpression.Method.Name == "Equals" && methodCallExpression.Arguments.Count > 0)
			{
				return ExpressionNormalizer.CreateRelationalOperator(ExpressionType.Equal, methodCallExpression.Object, methodCallExpression.Arguments[0]);
			}
			if (methodCallExpression.Method.IsStatic && methodCallExpression.Method.Name == "CompareString" && methodCallExpression.Method.DeclaringType.FullName == "Microsoft.VisualBasic.CompilerServices.Operators")
			{
				return this.CreateCompareExpression(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1]);
			}
			if (!methodCallExpression.Method.IsStatic && methodCallExpression.Method.Name == "CompareTo" && methodCallExpression.Arguments.Count == 1 && methodCallExpression.Method.ReturnType == typeof(int))
			{
				return this.CreateCompareExpression(methodCallExpression.Object, methodCallExpression.Arguments[0]);
			}
			if (methodCallExpression.Method.IsStatic && methodCallExpression.Method.Name == "Compare" && methodCallExpression.Arguments.Count > 1 && methodCallExpression.Method.ReturnType == typeof(int))
			{
				return this.CreateCompareExpression(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1]);
			}
			return ExpressionNormalizer.NormalizePredicateArgument(methodCallExpression);
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0004702C File Offset: 0x0004522C
		private static MethodCallExpression NormalizePredicateArgument(MethodCallExpression callExpression)
		{
			int index;
			Expression value;
			MethodCallExpression result;
			if (ExpressionNormalizer.HasPredicateArgument(callExpression, out index) && ExpressionNormalizer.TryMatchCoalescePattern(callExpression.Arguments[index], out value))
			{
				List<Expression> list = new List<Expression>(callExpression.Arguments);
				list[index] = value;
				result = Expression.Call(callExpression.Object, callExpression.Method, list);
			}
			else
			{
				result = callExpression;
			}
			return result;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00047084 File Offset: 0x00045284
		private static bool HasPredicateArgument(MethodCallExpression callExpression, out int argumentOrdinal)
		{
			argumentOrdinal = 0;
			bool result = false;
			SequenceMethod sequenceMethod;
			if (2 <= callExpression.Arguments.Count && ReflectionUtil.TryIdentifySequenceMethod(callExpression.Method, out sequenceMethod))
			{
				SequenceMethod sequenceMethod2 = sequenceMethod;
				if (sequenceMethod2 <= SequenceMethod.SkipWhileOrdinal)
				{
					switch (sequenceMethod2)
					{
					case SequenceMethod.Where:
					case SequenceMethod.WhereOrdinal:
						break;
					default:
						switch (sequenceMethod2)
						{
						case SequenceMethod.TakeWhile:
						case SequenceMethod.TakeWhileOrdinal:
						case SequenceMethod.SkipWhile:
						case SequenceMethod.SkipWhileOrdinal:
							break;
						case SequenceMethod.Skip:
							return result;
						default:
							return result;
						}
						break;
					}
				}
				else
				{
					switch (sequenceMethod2)
					{
					case SequenceMethod.FirstPredicate:
					case SequenceMethod.FirstOrDefaultPredicate:
					case SequenceMethod.LastPredicate:
					case SequenceMethod.LastOrDefaultPredicate:
					case SequenceMethod.SinglePredicate:
					case SequenceMethod.SingleOrDefaultPredicate:
						break;
					case SequenceMethod.FirstOrDefault:
					case SequenceMethod.Last:
					case SequenceMethod.LastOrDefault:
					case SequenceMethod.Single:
					case SequenceMethod.SingleOrDefault:
						return result;
					default:
						switch (sequenceMethod2)
						{
						case SequenceMethod.AnyPredicate:
						case SequenceMethod.All:
						case SequenceMethod.CountPredicate:
						case SequenceMethod.LongCountPredicate:
							break;
						case SequenceMethod.Count:
						case SequenceMethod.LongCount:
							return result;
						default:
							return result;
						}
						break;
					}
				}
				argumentOrdinal = 1;
				result = true;
			}
			return result;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0004714C File Offset: 0x0004534C
		private static bool TryMatchCoalescePattern(Expression expression, out Expression normalized)
		{
			normalized = null;
			bool result = false;
			if (expression.NodeType == ExpressionType.Quote)
			{
				UnaryExpression unaryExpression = (UnaryExpression)expression;
				if (ExpressionNormalizer.TryMatchCoalescePattern(unaryExpression.Operand, out normalized))
				{
					result = true;
					normalized = Expression.Quote(normalized);
				}
			}
			else if (expression.NodeType == ExpressionType.Lambda)
			{
				LambdaExpression lambdaExpression = (LambdaExpression)expression;
				if (lambdaExpression.Body.NodeType == ExpressionType.Coalesce && lambdaExpression.Body.Type == typeof(bool))
				{
					BinaryExpression binaryExpression = (BinaryExpression)lambdaExpression.Body;
					if (binaryExpression.Right.NodeType == ExpressionType.Constant && false.Equals(((ConstantExpression)binaryExpression.Right).Value))
					{
						normalized = Expression.Lambda(lambdaExpression.Type, Expression.Convert(binaryExpression.Left, typeof(bool)), lambdaExpression.Parameters);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00047230 File Offset: 0x00045430
		private static bool RelationalOperatorPlaceholder<TLeft, TRight>(TLeft left, TRight right)
		{
			return object.ReferenceEquals(left, right);
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00047244 File Offset: 0x00045444
		private static BinaryExpression CreateRelationalOperator(ExpressionType op, Expression left, Expression right)
		{
			BinaryExpression result;
			ExpressionNormalizer.TryCreateRelationalOperator(op, left, right, out result);
			return result;
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00047260 File Offset: 0x00045460
		private static bool TryCreateRelationalOperator(ExpressionType op, Expression left, Expression right, out BinaryExpression result)
		{
			MethodInfo method = ExpressionNormalizer.StaticRelationalOperatorPlaceholderMethod.MakeGenericMethod(new Type[]
			{
				left.Type,
				right.Type
			});
			switch (op)
			{
			case ExpressionType.Equal:
				result = Expression.Equal(left, right, false, method);
				return true;
			case ExpressionType.ExclusiveOr:
			case ExpressionType.Invoke:
			case ExpressionType.Lambda:
			case ExpressionType.LeftShift:
				break;
			case ExpressionType.GreaterThan:
				result = Expression.GreaterThan(left, right, false, method);
				return true;
			case ExpressionType.GreaterThanOrEqual:
				result = Expression.GreaterThanOrEqual(left, right, false, method);
				return true;
			case ExpressionType.LessThan:
				result = Expression.LessThan(left, right, false, method);
				return true;
			case ExpressionType.LessThanOrEqual:
				result = Expression.LessThanOrEqual(left, right, false, method);
				return true;
			default:
				if (op == ExpressionType.NotEqual)
				{
					result = Expression.NotEqual(left, right, false, method);
					return true;
				}
				break;
			}
			result = null;
			return false;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0004731C File Offset: 0x0004551C
		private Expression CreateCompareExpression(Expression left, Expression right)
		{
			Expression expression = Expression.Condition(ExpressionNormalizer.CreateRelationalOperator(ExpressionType.Equal, left, right), Expression.Constant(0), Expression.Condition(ExpressionNormalizer.CreateRelationalOperator(ExpressionType.GreaterThan, left, right), Expression.Constant(1), Expression.Constant(-1)));
			this.patterns[expression] = new ExpressionNormalizer.ComparePattern(left, right);
			return expression;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0004737B File Offset: 0x0004557B
		private void RecordRewrite(Expression source, Expression rewritten)
		{
			if (source != rewritten)
			{
				this.NormalizerRewrites.Add(rewritten, source);
			}
		}

		// Token: 0x0400058B RID: 1419
		private const bool LiftToNull = false;

		// Token: 0x0400058C RID: 1420
		private readonly Dictionary<Expression, ExpressionNormalizer.Pattern> patterns = new Dictionary<Expression, ExpressionNormalizer.Pattern>(ReferenceEqualityComparer<Expression>.Instance);

		// Token: 0x0400058D RID: 1421
		private readonly Dictionary<Expression, Expression> normalizerRewrites;

		// Token: 0x0400058E RID: 1422
		private static readonly MethodInfo StaticRelationalOperatorPlaceholderMethod = typeof(ExpressionNormalizer).GetMethod("RelationalOperatorPlaceholder", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x02000112 RID: 274
		private abstract class Pattern
		{
			// Token: 0x170002F0 RID: 752
			// (get) Token: 0x060012F1 RID: 4849
			internal abstract ExpressionNormalizer.PatternKind Kind { get; }
		}

		// Token: 0x02000113 RID: 275
		private enum PatternKind
		{
			// Token: 0x04000590 RID: 1424
			Compare
		}

		// Token: 0x02000114 RID: 276
		private sealed class ComparePattern : ExpressionNormalizer.Pattern
		{
			// Token: 0x060012F3 RID: 4851 RVA: 0x000473B3 File Offset: 0x000455B3
			internal ComparePattern(Expression left, Expression right)
			{
				this.Left = left;
				this.Right = right;
			}

			// Token: 0x170002F1 RID: 753
			// (get) Token: 0x060012F4 RID: 4852 RVA: 0x000473C9 File Offset: 0x000455C9
			internal override ExpressionNormalizer.PatternKind Kind
			{
				get
				{
					return ExpressionNormalizer.PatternKind.Compare;
				}
			}

			// Token: 0x04000591 RID: 1425
			internal readonly Expression Left;

			// Token: 0x04000592 RID: 1426
			internal readonly Expression Right;
		}
	}
}
