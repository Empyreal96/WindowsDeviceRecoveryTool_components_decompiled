using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200001C RID: 28
	internal static class SelectExpandSemanticBinder
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00003E98 File Offset: 0x00002098
		public static SelectExpandClause Parse(IEdmEntityType elementType, IEdmEntitySet entitySet, ExpandToken expandToken, SelectToken selectToken, ODataUriParserConfiguration configuration)
		{
			expandToken = ExpandTreeNormalizer.NormalizeExpandTree(expandToken);
			selectToken = SelectTreeNormalizer.NormalizeSelectTree(selectToken);
			ExpandBinder expandBinder = ExpandBinderFactory.Create(elementType, entitySet, configuration);
			SelectExpandClause selectExpandClause = expandBinder.Bind(expandToken);
			SelectBinder selectBinder = new SelectBinder(configuration.Model, elementType, configuration.Settings.SelectExpandLimit, selectExpandClause);
			selectExpandClause = selectBinder.Bind(selectToken);
			SelectExpandClause selectExpandClause2 = SelectExpandTreeFinisher.PruneSelectExpandTree(selectExpandClause);
			selectExpandClause2.ComputeFinalSelectedItems();
			new ExpandDepthAndCountValidator(configuration.Settings.MaximumExpansionDepth, configuration.Settings.MaximumExpansionCount).Validate(selectExpandClause2);
			return selectExpandClause2;
		}
	}
}
