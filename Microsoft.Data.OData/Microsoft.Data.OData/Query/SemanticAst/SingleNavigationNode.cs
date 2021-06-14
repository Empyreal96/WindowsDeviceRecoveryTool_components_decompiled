using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x020000E2 RID: 226
	public sealed class SingleNavigationNode : SingleEntityNode
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x000136C0 File Offset: 0x000118C0
		public SingleNavigationNode(IEdmNavigationProperty navigationProperty, SingleEntityNode source)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmNavigationProperty>(navigationProperty, "navigationProperty");
			ExceptionUtils.CheckArgumentNotNull<SingleEntityNode>(source, "source");
			EdmMultiplicity edmMultiplicity = navigationProperty.TargetMultiplicityTemporary();
			if (edmMultiplicity != EdmMultiplicity.One && edmMultiplicity != EdmMultiplicity.ZeroOrOne)
			{
				throw new ArgumentException(Strings.Nodes_CollectionNavigationNode_MustHaveSingleMultiplicity);
			}
			this.source = source;
			this.navigationProperty = navigationProperty;
			this.entityTypeReference = (IEdmEntityTypeReference)this.NavigationProperty.Type;
			this.entitySet = ((source.EntitySet != null) ? source.EntitySet.FindNavigationTarget(navigationProperty) : null);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00013744 File Offset: 0x00011944
		public SingleNavigationNode(IEdmNavigationProperty navigationProperty, IEdmEntitySet sourceSet)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmNavigationProperty>(navigationProperty, "navigationProperty");
			EdmMultiplicity edmMultiplicity = navigationProperty.TargetMultiplicityTemporary();
			if (edmMultiplicity != EdmMultiplicity.One && edmMultiplicity != EdmMultiplicity.ZeroOrOne)
			{
				throw new ArgumentException(Strings.Nodes_CollectionNavigationNode_MustHaveSingleMultiplicity);
			}
			this.navigationProperty = navigationProperty;
			this.entityTypeReference = (IEdmEntityTypeReference)this.NavigationProperty.Type;
			this.entitySet = ((sourceSet != null) ? sourceSet.FindNavigationTarget(navigationProperty) : null);
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x000137AC File Offset: 0x000119AC
		public SingleEntityNode Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x000137B4 File Offset: 0x000119B4
		public IEdmNavigationProperty NavigationProperty
		{
			get
			{
				return this.navigationProperty;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x000137BC File Offset: 0x000119BC
		public EdmMultiplicity TargetMultiplicity
		{
			get
			{
				return this.NavigationProperty.TargetMultiplicityTemporary();
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x000137C9 File Offset: 0x000119C9
		public override IEdmTypeReference TypeReference
		{
			get
			{
				return this.entityTypeReference;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x000137D1 File Offset: 0x000119D1
		public override IEdmEntityTypeReference EntityTypeReference
		{
			get
			{
				return this.entityTypeReference;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x000137D9 File Offset: 0x000119D9
		public override IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x000137E1 File Offset: 0x000119E1
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.SingleNavigationNode;
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000137E5 File Offset: 0x000119E5
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x04000257 RID: 599
		private readonly IEdmEntitySet entitySet;

		// Token: 0x04000258 RID: 600
		private readonly SingleEntityNode source;

		// Token: 0x04000259 RID: 601
		private readonly IEdmNavigationProperty navigationProperty;

		// Token: 0x0400025A RID: 602
		private readonly IEdmEntityTypeReference entityTypeReference;
	}
}
