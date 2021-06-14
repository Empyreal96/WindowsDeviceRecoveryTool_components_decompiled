using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000023 RID: 35
	internal sealed class ExpandOptionExpandBinder : ExpandBinder
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x000044AD File Offset: 0x000026AD
		public ExpandOptionExpandBinder(ODataUriParserConfiguration configuration, IEdmEntityType entityType, IEdmEntitySet entitySet) : base(configuration, entityType, entitySet)
		{
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000044B8 File Offset: 0x000026B8
		protected override SelectExpandClause GenerateSubExpand(IEdmNavigationProperty currentNavProp, ExpandTermToken tokenIn)
		{
			ExpandBinder expandBinder = new ExpandOptionExpandBinder(base.Configuration, currentNavProp.ToEntityType(), (base.EntitySet != null) ? base.EntitySet.FindNavigationTarget(currentNavProp) : null);
			return expandBinder.Bind(tokenIn.ExpandOption);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000044FC File Offset: 0x000026FC
		protected override SelectExpandClause DecorateExpandWithSelect(SelectExpandClause subExpand, IEdmNavigationProperty currentNavProp, SelectToken select)
		{
			SelectBinder selectBinder = new SelectBinder(base.Model, currentNavProp.ToEntityType(), base.Settings.SelectExpandLimit, subExpand);
			return selectBinder.Bind(select);
		}
	}
}
