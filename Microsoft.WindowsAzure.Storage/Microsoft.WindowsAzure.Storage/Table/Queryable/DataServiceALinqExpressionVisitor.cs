using System;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200010B RID: 267
	internal abstract class DataServiceALinqExpressionVisitor : ALinqExpressionVisitor
	{
		// Token: 0x060012CC RID: 4812 RVA: 0x00046490 File Offset: 0x00044690
		internal override Expression Visit(Expression exp)
		{
			if (exp == null)
			{
				return null;
			}
			ResourceExpressionType nodeType = (ResourceExpressionType)exp.NodeType;
			switch (nodeType)
			{
			case ResourceExpressionType.RootResourceSet:
			case ResourceExpressionType.ResourceNavigationProperty:
				return this.VisitResourceSetExpression((ResourceSetExpression)exp);
			case ResourceExpressionType.ResourceNavigationPropertySingleton:
				return this.VisitNavigationPropertySingletonExpression((NavigationPropertySingletonExpression)exp);
			default:
				if (nodeType != ResourceExpressionType.InputReference)
				{
					return base.Visit(exp);
				}
				return this.VisitInputReferenceExpression((InputReferenceExpression)exp);
			}
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x000464FC File Offset: 0x000446FC
		internal virtual Expression VisitResourceSetExpression(ResourceSetExpression rse)
		{
			Expression expression = this.Visit(rse.Source);
			if (expression != rse.Source)
			{
				rse = new ResourceSetExpression(rse.Type, expression, rse.MemberExpression, rse.ResourceType, rse.ExpandPaths, rse.CountOption, rse.CustomQueryOptions, rse.Projection);
			}
			return rse;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00046554 File Offset: 0x00044754
		internal virtual Expression VisitNavigationPropertySingletonExpression(NavigationPropertySingletonExpression npse)
		{
			Expression expression = this.Visit(npse.Source);
			if (expression != npse.Source)
			{
				npse = new NavigationPropertySingletonExpression(npse.Type, expression, npse.MemberExpression, npse.MemberExpression.Type, npse.ExpandPaths, npse.CountOption, npse.CustomQueryOptions, npse.Projection);
			}
			return npse;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x000465B0 File Offset: 0x000447B0
		internal virtual Expression VisitInputReferenceExpression(InputReferenceExpression ire)
		{
			ResourceExpression resourceExpression = (ResourceExpression)this.Visit(ire.Target);
			return resourceExpression.CreateReference();
		}
	}
}
