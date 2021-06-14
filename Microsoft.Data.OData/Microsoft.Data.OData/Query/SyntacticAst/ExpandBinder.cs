using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000021 RID: 33
	internal abstract class ExpandBinder
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00004161 File Offset: 0x00002361
		protected ExpandBinder(ODataUriParserConfiguration configuration, IEdmEntityType entityType, IEdmEntitySet entitySet)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataUriParserConfiguration>(configuration, "configuration");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityType>(entityType, "topLevelEntityType");
			this.configuration = configuration;
			this.entityType = entityType;
			this.entitySet = entitySet;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004194 File Offset: 0x00002394
		public IEdmModel Model
		{
			get
			{
				return this.configuration.Model;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000041A1 File Offset: 0x000023A1
		public IEdmEntityType EntityType
		{
			get
			{
				return this.entityType;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000041A9 File Offset: 0x000023A9
		public IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000041B1 File Offset: 0x000023B1
		protected ODataUriParserSettings Settings
		{
			get
			{
				return this.configuration.Settings;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000041BE File Offset: 0x000023BE
		protected ODataUriParserConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000041D0 File Offset: 0x000023D0
		public SelectExpandClause Bind(ExpandToken tokenIn)
		{
			ExceptionUtils.CheckArgumentNotNull<ExpandToken>(tokenIn, "tokenIn");
			List<ExpandedNavigationSelectItem> expandItems = (from i in tokenIn.ExpandTerms.Select(new Func<ExpandTermToken, ExpandedNavigationSelectItem>(this.GenerateExpandItem))
			where i != null
			select i).ToList<ExpandedNavigationSelectItem>();
			return new SelectExpandClause(UnknownSelection.Instance, new Expansion(expandItems));
		}

		// Token: 0x060000DE RID: 222
		protected abstract SelectExpandClause GenerateSubExpand(IEdmNavigationProperty currentNavProp, ExpandTermToken tokenIn);

		// Token: 0x060000DF RID: 223
		protected abstract SelectExpandClause DecorateExpandWithSelect(SelectExpandClause subExpand, IEdmNavigationProperty currentNavProp, SelectToken select);

		// Token: 0x060000E0 RID: 224 RVA: 0x00004237 File Offset: 0x00002437
		private static SelectExpandClause BuildDefaultSubExpand()
		{
			return new SelectExpandClause(UnknownSelection.Instance, new Expansion(new List<ExpandedNavigationSelectItem>()));
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004250 File Offset: 0x00002450
		private ExpandedNavigationSelectItem GenerateExpandItem(ExpandTermToken tokenIn)
		{
			ExceptionUtils.CheckArgumentNotNull<ExpandTermToken>(tokenIn, "tokenIn");
			if (tokenIn.PathToNavProp.NextToken != null && !tokenIn.PathToNavProp.IsNamespaceOrContainerQualified())
			{
				throw new ODataException(Strings.ExpandItemBinder_TraversingANonNormalizedTree);
			}
			PathSegmentToken pathToNavProp = tokenIn.PathToNavProp;
			IEdmEntityType edmEntityType = this.entityType;
			List<ODataPathSegment> list = new List<ODataPathSegment>();
			PathSegmentToken pathSegmentToken = pathToNavProp;
			if (pathToNavProp.IsNamespaceOrContainerQualified())
			{
				list.AddRange(SelectExpandPathBinder.FollowTypeSegments(pathToNavProp, this.Model, this.Settings.SelectExpandLimit, ref edmEntityType, out pathSegmentToken));
			}
			IEdmProperty edmProperty = edmEntityType.FindProperty(pathSegmentToken.Identifier);
			if (edmProperty == null)
			{
				throw new ODataException(Strings.MetadataBinder_PropertyNotDeclared(edmEntityType.FullName(), pathToNavProp.Identifier));
			}
			IEdmNavigationProperty edmNavigationProperty = edmProperty as IEdmNavigationProperty;
			if (edmNavigationProperty != null)
			{
				list.Add(new NavigationPropertySegment(edmNavigationProperty, null));
				ODataExpandPath pathToNavigationProperty = new ODataExpandPath(list);
				SelectExpandClause selectExpandClause;
				if (tokenIn.ExpandOption != null)
				{
					selectExpandClause = this.GenerateSubExpand(edmNavigationProperty, tokenIn);
				}
				else
				{
					selectExpandClause = ExpandBinder.BuildDefaultSubExpand();
				}
				selectExpandClause = this.DecorateExpandWithSelect(selectExpandClause, edmNavigationProperty, tokenIn.SelectOption);
				IEdmEntitySet targetEntitySet = null;
				if (this.entitySet != null)
				{
					targetEntitySet = this.entitySet.FindNavigationTarget(edmNavigationProperty);
				}
				FilterClause filterOption = null;
				if (tokenIn.FilterOption != null)
				{
					MetadataBinder metadataBinder = this.BuildNewMetadataBinder(targetEntitySet);
					FilterBinder filterBinder = new FilterBinder(new MetadataBinder.QueryTokenVisitor(metadataBinder.Bind), metadataBinder.BindingState);
					filterOption = filterBinder.BindFilter(tokenIn.FilterOption);
				}
				OrderByClause orderByOption = null;
				if (tokenIn.OrderByOption != null)
				{
					MetadataBinder metadataBinder2 = this.BuildNewMetadataBinder(targetEntitySet);
					OrderByBinder orderByBinder = new OrderByBinder(new MetadataBinder.QueryTokenVisitor(metadataBinder2.Bind));
					orderByOption = orderByBinder.BindOrderBy(metadataBinder2.BindingState, new OrderByToken[]
					{
						tokenIn.OrderByOption
					});
				}
				return new ExpandedNavigationSelectItem(pathToNavigationProperty, targetEntitySet, filterOption, orderByOption, tokenIn.TopOption, tokenIn.SkipOption, tokenIn.InlineCountOption, selectExpandClause);
			}
			if (this.Settings.UseWcfDataServicesServerBehavior && !edmProperty.Type.IsStream())
			{
				return null;
			}
			throw new ODataException(Strings.ExpandItemBinder_PropertyIsNotANavigationProperty(pathToNavProp.Identifier, edmEntityType.FullName()));
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004444 File Offset: 0x00002644
		private MetadataBinder BuildNewMetadataBinder(IEdmEntitySet targetEntitySet)
		{
			BindingState bindingState = new BindingState(this.configuration)
			{
				ImplicitRangeVariable = NodeFactory.CreateImplicitRangeVariable(targetEntitySet.ElementType.ToTypeReference(), targetEntitySet)
			};
			bindingState.RangeVariables.Push(bindingState.ImplicitRangeVariable);
			return new MetadataBinder(bindingState);
		}

		// Token: 0x0400004D RID: 77
		private readonly ODataUriParserConfiguration configuration;

		// Token: 0x0400004E RID: 78
		private readonly IEdmEntitySet entitySet;

		// Token: 0x0400004F RID: 79
		private readonly IEdmEntityType entityType;
	}
}
