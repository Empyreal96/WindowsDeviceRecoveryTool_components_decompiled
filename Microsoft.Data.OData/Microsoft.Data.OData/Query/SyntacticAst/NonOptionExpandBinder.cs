using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000040 RID: 64
	internal sealed class NonOptionExpandBinder : ExpandBinder
	{
		// Token: 0x06000199 RID: 409 RVA: 0x000071F9 File Offset: 0x000053F9
		public NonOptionExpandBinder(ODataUriParserConfiguration configuration, IEdmEntityType entityType, IEdmEntitySet entitySet) : base(configuration, entityType, entitySet)
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007204 File Offset: 0x00005404
		protected override SelectExpandClause GenerateSubExpand(IEdmNavigationProperty currentNavProp, ExpandTermToken tokenIn)
		{
			ExpandBinder expandBinder = new NonOptionExpandBinder(base.Configuration, currentNavProp.ToEntityType(), (base.EntitySet != null) ? base.EntitySet.FindNavigationTarget(currentNavProp) : null);
			return expandBinder.Bind(tokenIn.ExpandOption);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007246 File Offset: 0x00005446
		protected override SelectExpandClause DecorateExpandWithSelect(SelectExpandClause subExpand, IEdmNavigationProperty currentNavProp, SelectToken select)
		{
			return subExpand;
		}
	}
}
