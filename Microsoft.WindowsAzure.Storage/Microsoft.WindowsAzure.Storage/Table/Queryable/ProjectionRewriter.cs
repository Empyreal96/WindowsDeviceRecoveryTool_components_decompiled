using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000123 RID: 291
	internal class ProjectionRewriter : ALinqExpressionVisitor
	{
		// Token: 0x060013A1 RID: 5025 RVA: 0x00049A80 File Offset: 0x00047C80
		private ProjectionRewriter(Type proposedParameterType)
		{
			this.newLambdaParameter = Expression.Parameter(proposedParameterType, "it");
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00049AB4 File Offset: 0x00047CB4
		internal static LambdaExpression TryToRewrite(LambdaExpression le, Type proposedParameterType)
		{
			LambdaExpression result;
			if (!ResourceBinder.PatternRules.MatchSingleArgumentLambda(le, out le) || !le.Parameters[0].Type.GetProperties().Any((PropertyInfo p) => p.PropertyType == proposedParameterType))
			{
				result = le;
			}
			else
			{
				ProjectionRewriter projectionRewriter = new ProjectionRewriter(proposedParameterType);
				result = projectionRewriter.Rebind(le);
			}
			return result;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00049B1C File Offset: 0x00047D1C
		internal LambdaExpression Rebind(LambdaExpression lambda)
		{
			this.sucessfulRebind = true;
			this.oldLambdaParameter = lambda.Parameters[0];
			Expression body = this.Visit(lambda.Body);
			if (this.sucessfulRebind)
			{
				Type delegateType = typeof(Func<, >).MakeGenericType(new Type[]
				{
					this.newLambdaParameter.Type,
					lambda.Body.Type
				});
				return Expression.Lambda(delegateType, body, new ParameterExpression[]
				{
					this.newLambdaParameter
				});
			}
			throw new NotSupportedException("Can only project the last entity type in the query being translated.");
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00049BAE File Offset: 0x00047DAE
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			if (m.Expression == this.oldLambdaParameter)
			{
				if (m.Type == this.newLambdaParameter.Type)
				{
					return this.newLambdaParameter;
				}
				this.sucessfulRebind = false;
			}
			return base.VisitMemberAccess(m);
		}

		// Token: 0x040005C6 RID: 1478
		private readonly ParameterExpression newLambdaParameter;

		// Token: 0x040005C7 RID: 1479
		private ParameterExpression oldLambdaParameter;

		// Token: 0x040005C8 RID: 1480
		private bool sucessfulRebind;
	}
}
