using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200010E RID: 270
	internal static class Evaluator
	{
		// Token: 0x060012D5 RID: 4821 RVA: 0x00046608 File Offset: 0x00044808
		internal static Expression PartialEval(Expression expression, Func<Expression, bool> canBeEvaluated)
		{
			Evaluator.Nominator nominator = new Evaluator.Nominator(canBeEvaluated);
			HashSet<Expression> candidates = nominator.Nominate(expression);
			return new Evaluator.SubtreeEvaluator(candidates).Eval(expression);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00046630 File Offset: 0x00044830
		internal static Expression PartialEval(Expression expression)
		{
			return Evaluator.PartialEval(expression, new Func<Expression, bool>(Evaluator.CanBeEvaluatedLocally));
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00046644 File Offset: 0x00044844
		private static bool CanBeEvaluatedLocally(Expression expression)
		{
			return expression.NodeType != ExpressionType.Parameter && expression.NodeType != ExpressionType.Lambda && expression.NodeType != (ExpressionType)10000;
		}

		// Token: 0x0200010F RID: 271
		internal class SubtreeEvaluator : DataServiceALinqExpressionVisitor
		{
			// Token: 0x060012D8 RID: 4824 RVA: 0x0004666C File Offset: 0x0004486C
			internal SubtreeEvaluator(HashSet<Expression> candidates)
			{
				this.candidates = candidates;
			}

			// Token: 0x060012D9 RID: 4825 RVA: 0x0004667B File Offset: 0x0004487B
			internal Expression Eval(Expression exp)
			{
				return this.Visit(exp);
			}

			// Token: 0x060012DA RID: 4826 RVA: 0x00046684 File Offset: 0x00044884
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

			// Token: 0x060012DB RID: 4827 RVA: 0x000466A8 File Offset: 0x000448A8
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

			// Token: 0x04000587 RID: 1415
			private HashSet<Expression> candidates;
		}

		// Token: 0x02000110 RID: 272
		internal class Nominator : DataServiceALinqExpressionVisitor
		{
			// Token: 0x060012DC RID: 4828 RVA: 0x00046715 File Offset: 0x00044915
			internal Nominator(Func<Expression, bool> functionCanBeEvaluated)
			{
				this.functionCanBeEvaluated = functionCanBeEvaluated;
			}

			// Token: 0x060012DD RID: 4829 RVA: 0x00046724 File Offset: 0x00044924
			internal HashSet<Expression> Nominate(Expression expression)
			{
				this.candidates = new HashSet<Expression>(EqualityComparer<Expression>.Default);
				this.Visit(expression);
				return this.candidates;
			}

			// Token: 0x060012DE RID: 4830 RVA: 0x00046744 File Offset: 0x00044944
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

			// Token: 0x04000588 RID: 1416
			private Func<Expression, bool> functionCanBeEvaluated;

			// Token: 0x04000589 RID: 1417
			private HashSet<Expression> candidates;

			// Token: 0x0400058A RID: 1418
			private bool cannotBeEvaluated;
		}
	}
}
