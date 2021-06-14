using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000060 RID: 96
	internal sealed class SelectPropertyVisitor : PathSegmentTokenVisitor
	{
		// Token: 0x06000268 RID: 616 RVA: 0x00009CDC File Offset: 0x00007EDC
		public SelectPropertyVisitor(IEdmModel model, IEdmEntityType entityType, int maxDepth, SelectExpandClause expandClauseToDecorate)
		{
			this.model = model;
			this.entityType = entityType;
			this.maxDepth = maxDepth;
			this.expandClauseToDecorate = expandClauseToDecorate;
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00009D01 File Offset: 0x00007F01
		public SelectExpandClause DecoratedExpandClause
		{
			get
			{
				return this.expandClauseToDecorate;
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00009D09 File Offset: 0x00007F09
		public override void Visit(SystemToken tokenIn)
		{
			ExceptionUtils.CheckArgumentNotNull<SystemToken>(tokenIn, "tokenIn");
			throw new ODataException(Strings.SelectPropertyVisitor_SystemTokenInSelect(tokenIn.Identifier));
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00009D28 File Offset: 0x00007F28
		public override void Visit(NonSystemToken tokenIn)
		{
			ExceptionUtils.CheckArgumentNotNull<NonSystemToken>(tokenIn, "tokenIn");
			SelectItem itemToAdd;
			if (tokenIn.NextToken == null && SelectPathSegmentTokenBinder.TryBindAsWildcard(tokenIn, this.model, out itemToAdd))
			{
				this.expandClauseToDecorate.AddSelectItem(itemToAdd);
				return;
			}
			this.ProcessTokenAsPath(tokenIn);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00009D6C File Offset: 0x00007F6C
		private void ProcessTokenAsPath(NonSystemToken tokenIn)
		{
			List<ODataPathSegment> list = new List<ODataPathSegment>();
			IEdmEntityType edmEntityType = this.entityType;
			if (tokenIn.IsNamespaceOrContainerQualified())
			{
				PathSegmentToken pathSegmentToken;
				list.AddRange(SelectExpandPathBinder.FollowTypeSegments(tokenIn, this.model, this.maxDepth, ref edmEntityType, out pathSegmentToken));
				tokenIn = (pathSegmentToken as NonSystemToken);
				if (tokenIn == null)
				{
					throw new ODataException(Strings.SelectPropertyVisitor_SystemTokenInSelect(pathSegmentToken.Identifier));
				}
			}
			ODataPathSegment odataPathSegment = SelectPathSegmentTokenBinder.ConvertNonTypeTokenToSegment(tokenIn, this.model, edmEntityType);
			list.Add(odataPathSegment);
			ODataSelectPath selectedPath = new ODataSelectPath(list);
			PathSelectItem pathSelectItem = new PathSelectItem(selectedPath);
			if (odataPathSegment is NavigationPropertySegment)
			{
				bool flag = false;
				bool flag2 = false;
				foreach (ExpandedNavigationSelectItem expandedNavigationSelectItem in this.expandClauseToDecorate.Expansion.ExpandItems)
				{
					IEdmNavigationProperty navigationProperty = expandedNavigationSelectItem.PathToNavigationProperty.GetNavigationProperty();
					if (expandedNavigationSelectItem.PathToNavigationProperty.Equals(pathSelectItem.SelectedPath))
					{
						flag = true;
						if (tokenIn.NextToken == null)
						{
							expandedNavigationSelectItem.SelectAndExpand.SetAllSelectionRecursively();
						}
						else
						{
							SelectPropertyVisitor visitor = new SelectPropertyVisitor(this.model, navigationProperty.ToEntityType(), this.maxDepth, expandedNavigationSelectItem.SelectAndExpand);
							tokenIn.NextToken.Accept(visitor);
						}
					}
					else if (expandedNavigationSelectItem.PathToNavigationProperty.LastSegment.Equals(pathSelectItem.SelectedPath.LastSegment))
					{
						flag2 = true;
					}
				}
				if (flag2 && !flag)
				{
					throw new ODataException(Strings.SelectPropertyVisitor_DisparateTypeSegmentsInSelectExpand);
				}
				if (flag)
				{
					this.expandClauseToDecorate.InitializeEmptySelection();
					return;
				}
				if (tokenIn.NextToken != null)
				{
					throw new ODataException(Strings.SelectionItemBinder_NoExpandForSelectedProperty(tokenIn.Identifier));
				}
				this.expandClauseToDecorate.AddSelectItem(pathSelectItem);
				return;
			}
			else
			{
				if (tokenIn.NextToken != null)
				{
					throw new ODataException(Strings.SelectionItemBinder_NonNavigationPathToken);
				}
				this.expandClauseToDecorate.AddSelectItem(pathSelectItem);
				return;
			}
		}

		// Token: 0x0400009F RID: 159
		private readonly IEdmModel model;

		// Token: 0x040000A0 RID: 160
		private readonly int maxDepth;

		// Token: 0x040000A1 RID: 161
		private readonly SelectExpandClause expandClauseToDecorate;

		// Token: 0x040000A2 RID: 162
		private readonly IEdmEntityType entityType;
	}
}
