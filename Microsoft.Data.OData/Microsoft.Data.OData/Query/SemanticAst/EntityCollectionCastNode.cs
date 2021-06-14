using System;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000078 RID: 120
	public sealed class EntityCollectionCastNode : EntityCollectionNode
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0000AC0C File Offset: 0x00008E0C
		public EntityCollectionCastNode(EntityCollectionNode source, IEdmEntityType entityType)
		{
			ExceptionUtils.CheckArgumentNotNull<EntityCollectionNode>(source, "source");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityType>(entityType, "entityType");
			this.source = source;
			this.edmTypeReference = new EdmEntityTypeReference(entityType, false);
			this.entitySet = source.EntitySet;
			this.collectionTypeReference = EdmCoreModel.GetCollection(this.edmTypeReference);
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000AC66 File Offset: 0x00008E66
		public EntityCollectionNode Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000AC6E File Offset: 0x00008E6E
		public override IEdmTypeReference ItemType
		{
			get
			{
				return this.edmTypeReference;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000AC76 File Offset: 0x00008E76
		public override IEdmCollectionTypeReference CollectionType
		{
			get
			{
				return this.collectionTypeReference;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000AC7E File Offset: 0x00008E7E
		public override IEdmEntityTypeReference EntityItemType
		{
			get
			{
				return this.edmTypeReference;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000AC86 File Offset: 0x00008E86
		public override IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000AC8E File Offset: 0x00008E8E
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.EntityCollectionCast;
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000AC92 File Offset: 0x00008E92
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x040000C7 RID: 199
		private readonly EntityCollectionNode source;

		// Token: 0x040000C8 RID: 200
		private readonly IEdmEntityTypeReference edmTypeReference;

		// Token: 0x040000C9 RID: 201
		private readonly IEdmCollectionTypeReference collectionTypeReference;

		// Token: 0x040000CA RID: 202
		private readonly IEdmEntitySet entitySet;
	}
}
