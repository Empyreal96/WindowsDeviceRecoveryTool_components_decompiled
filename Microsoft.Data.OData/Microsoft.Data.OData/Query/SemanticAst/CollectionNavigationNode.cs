using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000079 RID: 121
	public sealed class CollectionNavigationNode : EntityCollectionNode
	{
		// Token: 0x060002DB RID: 731 RVA: 0x0000ACA6 File Offset: 0x00008EA6
		public CollectionNavigationNode(IEdmNavigationProperty navigationProperty, SingleEntityNode source) : this(navigationProperty)
		{
			ExceptionUtils.CheckArgumentNotNull<SingleEntityNode>(source, "source");
			this.source = source;
			this.entitySet = ((source.EntitySet != null) ? source.EntitySet.FindNavigationTarget(navigationProperty) : null);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000ACDE File Offset: 0x00008EDE
		public CollectionNavigationNode(IEdmNavigationProperty navigationProperty, IEdmEntitySet sourceSet) : this(navigationProperty)
		{
			this.entitySet = ((sourceSet != null) ? sourceSet.FindNavigationTarget(navigationProperty) : null);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000ACFC File Offset: 0x00008EFC
		private CollectionNavigationNode(IEdmNavigationProperty navigationProperty)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmNavigationProperty>(navigationProperty, "navigationProperty");
			if (navigationProperty.TargetMultiplicityTemporary() != EdmMultiplicity.Many)
			{
				throw new ArgumentException(Strings.Nodes_CollectionNavigationNode_MustHaveManyMultiplicity);
			}
			this.navigationProperty = navigationProperty;
			this.collectionTypeReference = navigationProperty.Type.AsCollection();
			this.edmEntityTypeReference = this.collectionTypeReference.ElementType().AsEntityOrNull();
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000AD5C File Offset: 0x00008F5C
		public SingleValueNode Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000AD64 File Offset: 0x00008F64
		public EdmMultiplicity TargetMultiplicity
		{
			get
			{
				return this.navigationProperty.TargetMultiplicityTemporary();
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000AD71 File Offset: 0x00008F71
		public IEdmNavigationProperty NavigationProperty
		{
			get
			{
				return this.navigationProperty;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000AD79 File Offset: 0x00008F79
		public override IEdmTypeReference ItemType
		{
			get
			{
				return this.edmEntityTypeReference;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000AD81 File Offset: 0x00008F81
		public override IEdmCollectionTypeReference CollectionType
		{
			get
			{
				return this.collectionTypeReference;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000AD89 File Offset: 0x00008F89
		public override IEdmEntityTypeReference EntityItemType
		{
			get
			{
				return this.edmEntityTypeReference;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000AD91 File Offset: 0x00008F91
		public override IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000AD99 File Offset: 0x00008F99
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.CollectionNavigationNode;
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000AD9D File Offset: 0x00008F9D
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x040000CB RID: 203
		private readonly IEdmNavigationProperty navigationProperty;

		// Token: 0x040000CC RID: 204
		private readonly IEdmEntityTypeReference edmEntityTypeReference;

		// Token: 0x040000CD RID: 205
		private readonly IEdmCollectionTypeReference collectionTypeReference;

		// Token: 0x040000CE RID: 206
		private readonly SingleValueNode source;

		// Token: 0x040000CF RID: 207
		private readonly IEdmEntitySet entitySet;
	}
}
