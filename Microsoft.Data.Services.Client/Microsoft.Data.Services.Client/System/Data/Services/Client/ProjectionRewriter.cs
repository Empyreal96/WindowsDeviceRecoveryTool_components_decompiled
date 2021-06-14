using System;
using System.Data.Services.Client.Metadata;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Services.Client
{
	// Token: 0x020000B3 RID: 179
	internal class ProjectionRewriter : ALinqExpressionVisitor
	{
		// Token: 0x060005B1 RID: 1457 RVA: 0x00015A14 File Offset: 0x00013C14
		private ProjectionRewriter(Type proposedParameterType)
		{
			this.newLambdaParameter = Expression.Parameter(proposedParameterType, "it");
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00015A48 File Offset: 0x00013C48
		internal static LambdaExpression TryToRewrite(LambdaExpression le, ResourceExpression source)
		{
			Type proposedParameterType = source.ResourceType;
			LambdaExpression result;
			if (!ResourceBinder.PatternRules.MatchSingleArgumentLambda(le, out le) || ClientTypeUtil.TypeOrElementTypeIsEntity(le.Parameters[0].Type) || !le.Parameters[0].Type.GetProperties().Any((PropertyInfo p) => p.PropertyType == proposedParameterType))
			{
				result = le;
			}
			else
			{
				ProjectionRewriter projectionRewriter = new ProjectionRewriter(proposedParameterType);
				result = projectionRewriter.Rebind(le, source);
			}
			return result;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00015ACC File Offset: 0x00013CCC
		internal LambdaExpression Rebind(LambdaExpression lambda, ResourceExpression source)
		{
			this.successfulRebind = true;
			this.oldLambdaParameter = lambda.Parameters[0];
			this.projectionSource = source;
			Expression body = this.Visit(lambda.Body);
			if (this.successfulRebind)
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
			throw new NotSupportedException(Strings.ALinq_CanOnlyProjectTheLeaf);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00015B68 File Offset: 0x00013D68
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			if (m.Expression == this.oldLambdaParameter)
			{
				ResourceSetExpression resourceSetExpression = this.projectionSource as ResourceSetExpression;
				if (resourceSetExpression != null && resourceSetExpression.HasTransparentScope && resourceSetExpression.TransparentScope.Accessor == m.Member.Name)
				{
					return this.newLambdaParameter;
				}
				this.successfulRebind = false;
			}
			return base.VisitMemberAccess(m);
		}

		// Token: 0x04000316 RID: 790
		private readonly ParameterExpression newLambdaParameter;

		// Token: 0x04000317 RID: 791
		private ParameterExpression oldLambdaParameter;

		// Token: 0x04000318 RID: 792
		private ResourceExpression projectionSource;

		// Token: 0x04000319 RID: 793
		private bool successfulRebind;
	}
}
