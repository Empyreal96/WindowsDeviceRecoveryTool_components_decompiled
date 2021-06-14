using System;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000EB RID: 235
	internal class NoThrowExpressionVisitor : ExpressionVisitor
	{
		// Token: 0x06000B35 RID: 2869 RVA: 0x0002D652 File Offset: 0x0002B852
		protected override Expression VisitConditional(ConditionalExpression node)
		{
			if (node.IfFalse.NodeType == ExpressionType.Throw)
			{
				return Expression.Condition(node.Test, node.IfTrue, Expression.Constant(NoThrowExpressionVisitor.ErrorResult));
			}
			return base.VisitConditional(node);
		}

		// Token: 0x04000408 RID: 1032
		internal static readonly object ErrorResult = new object();
	}
}
