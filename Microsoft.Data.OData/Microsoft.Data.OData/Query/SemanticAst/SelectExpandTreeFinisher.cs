using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200005F RID: 95
	internal static class SelectExpandTreeFinisher
	{
		// Token: 0x06000267 RID: 615 RVA: 0x00009BCC File Offset: 0x00007DCC
		internal static SelectExpandClause PruneSelectExpandTree(SelectExpandClause clauseToPrune)
		{
			if (clauseToPrune == null)
			{
				return null;
			}
			if (clauseToPrune.Selection is UnknownSelection)
			{
				return null;
			}
			if (clauseToPrune.Selection is AllSelection)
			{
				return clauseToPrune;
			}
			if (clauseToPrune.Expansion != null)
			{
				List<ExpandedNavigationSelectItem> list = new List<ExpandedNavigationSelectItem>();
				foreach (ExpandedNavigationSelectItem expandedNavigationSelectItem in clauseToPrune.Expansion.ExpandItems)
				{
					SelectExpandClause selectExpandClause = SelectExpandTreeFinisher.PruneSelectExpandTree(expandedNavigationSelectItem.SelectAndExpand);
					if (selectExpandClause == expandedNavigationSelectItem.SelectAndExpand)
					{
						list.Add(expandedNavigationSelectItem);
					}
					else if (selectExpandClause != null)
					{
						list.Add(new ExpandedNavigationSelectItem(expandedNavigationSelectItem.PathToNavigationProperty, expandedNavigationSelectItem.EntitySet, expandedNavigationSelectItem.FilterOption, expandedNavigationSelectItem.OrderByOption, expandedNavigationSelectItem.TopOption, expandedNavigationSelectItem.SkipOption, expandedNavigationSelectItem.InlineCountOption, selectExpandClause));
					}
				}
				if (list.Count == 0 && clauseToPrune.Selection is ExpansionsOnly)
				{
					return null;
				}
				return new SelectExpandClause(clauseToPrune.Selection, new Expansion(list));
			}
			else
			{
				if (clauseToPrune.Selection is ExpansionsOnly)
				{
					return null;
				}
				return clauseToPrune;
			}
		}
	}
}
