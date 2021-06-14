using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000C9 RID: 201
	internal class FilterQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x0600065B RID: 1627 RVA: 0x000197D0 File Offset: 0x000179D0
		internal FilterQueryOptionExpression(Type type) : base(type)
		{
			this.individualExpressions = new List<Expression>();
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x000197E4 File Offset: 0x000179E4
		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)10006;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x000197EB File Offset: 0x000179EB
		internal ReadOnlyCollection<Expression> PredicateConjuncts
		{
			get
			{
				return new ReadOnlyCollection<Expression>(this.individualExpressions);
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x000197F8 File Offset: 0x000179F8
		public void AddPredicateConjuncts(IEnumerable<Expression> predicates)
		{
			this.individualExpressions.AddRange(predicates);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00019808 File Offset: 0x00017A08
		public Expression GetPredicate()
		{
			Expression expression = null;
			bool flag = true;
			foreach (Expression expression2 in this.individualExpressions)
			{
				if (flag)
				{
					expression = expression2;
					flag = false;
				}
				else
				{
					expression = Expression.And(expression, expression2);
				}
			}
			return expression;
		}

		// Token: 0x04000408 RID: 1032
		private readonly List<Expression> individualExpressions;
	}
}
