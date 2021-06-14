using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000C2 RID: 194
	internal static class Evaluator
	{
		// Token: 0x06000637 RID: 1591 RVA: 0x000187CC File Offset: 0x000169CC
		internal static Expression PartialEval(Expression expression, Func<Expression, bool> canBeEvaluated)
		{
			Evaluator.Nominator nominator = new Evaluator.Nominator(canBeEvaluated);
			HashSet<Expression> candidates = nominator.Nominate(expression);
			return new Evaluator.SubtreeEvaluator(candidates).Eval(expression);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000187F4 File Offset: 0x000169F4
		internal static Expression PartialEval(Expression expression)
		{
			return Evaluator.PartialEval(expression, new Func<Expression, bool>(Evaluator.CanBeEvaluatedLocally));
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00018808 File Offset: 0x00016A08
		private static bool CanBeEvaluatedLocally(Expression expression)
		{
			return expression.NodeType != ExpressionType.Parameter && expression.NodeType != ExpressionType.Lambda && expression.NodeType != (ExpressionType)10000;
		}

		// Token: 0x020000C3 RID: 195
		internal class SubtreeEvaluator : DataServiceALinqExpressionVisitor
		{
			// Token: 0x0600063A RID: 1594 RVA: 0x00018830 File Offset: 0x00016A30
			internal SubtreeEvaluator(HashSet<Expression> candidates)
			{
				this.candidates = candidates;
			}

			// Token: 0x0600063B RID: 1595 RVA: 0x0001883F File Offset: 0x00016A3F
			internal Expression Eval(Expression exp)
			{
				return this.Visit(exp);
			}

			// Token: 0x0600063C RID: 1596 RVA: 0x00018848 File Offset: 0x00016A48
			internal override Expression Visit(Expression exp)
			{
				if (exp == null)
				{
					return null;
				}
				if (this.candidates.Contains(exp))
				{
					return Evaluator.SubtreeEvaluator.Evaluate(exp);
				}
				return base.Visit(exp);
			}

			// Token: 0x0600063D RID: 1597 RVA: 0x0001886C File Offset: 0x00016A6C
			private static Expression Evaluate(Expression e)
			{
				if (e.NodeType == ExpressionType.Constant)
				{
					return e;
				}
				LambdaExpression lambdaExpression = Expression.Lambda(e, new ParameterExpression[0]);
				Delegate @delegate = lambdaExpression.Compile();
				object obj = @delegate.DynamicInvoke(null);
				Type type = e.Type;
				if (obj != null && type.IsArray && type.GetElementType() == obj.GetType().GetElementType())
				{
					type = obj.GetType();
				}
				return Expression.Constant(obj, type);
			}

			// Token: 0x040003FC RID: 1020
			private HashSet<Expression> candidates;
		}

		// Token: 0x020000C4 RID: 196
		internal class Nominator : DataServiceALinqExpressionVisitor
		{
			// Token: 0x0600063E RID: 1598 RVA: 0x000188D9 File Offset: 0x00016AD9
			internal Nominator(Func<Expression, bool> functionCanBeEvaluated)
			{
				this.functionCanBeEvaluated = functionCanBeEvaluated;
			}

			// Token: 0x0600063F RID: 1599 RVA: 0x000188E8 File Offset: 0x00016AE8
			internal HashSet<Expression> Nominate(Expression expression)
			{
				this.candidates = new HashSet<Expression>(EqualityComparer<Expression>.Default);
				this.Visit(expression);
				return this.candidates;
			}

			// Token: 0x06000640 RID: 1600 RVA: 0x00018908 File Offset: 0x00016B08
			internal override Expression Visit(Expression expression)
			{
				if (expression != null)
				{
					bool flag = this.cannotBeEvaluated;
					this.cannotBeEvaluated = false;
					base.Visit(expression);
					if (!this.cannotBeEvaluated)
					{
						if (this.functionCanBeEvaluated(expression))
						{
							this.candidates.Add(expression);
						}
						else
						{
							this.cannotBeEvaluated = true;
						}
					}
					this.cannotBeEvaluated = (this.cannotBeEvaluated || flag);
				}
				return expression;
			}

			// Token: 0x040003FD RID: 1021
			private Func<Expression, bool> functionCanBeEvaluated;

			// Token: 0x040003FE RID: 1022
			private HashSet<Expression> candidates;

			// Token: 0x040003FF RID: 1023
			private bool cannotBeEvaluated;
		}
	}
}
