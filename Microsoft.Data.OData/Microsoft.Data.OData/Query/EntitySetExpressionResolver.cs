using System;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000069 RID: 105
	internal static class EntitySetExpressionResolver
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000A1E8 File Offset: 0x000083E8
		internal static IEdmEntitySet ResolveEntitySetFromExpression(IEdmExpression expression)
		{
			if (expression == null)
			{
				return null;
			}
			EdmExpressionKind expressionKind = expression.ExpressionKind;
			if (expressionKind == EdmExpressionKind.EntitySetReference)
			{
				return ((IEdmEntitySetReferenceExpression)expression).ReferencedEntitySet;
			}
			throw new NotSupportedException(Strings.Nodes_NonStaticEntitySetExpressionsAreNotSupportedInThisRelease);
		}
	}
}
