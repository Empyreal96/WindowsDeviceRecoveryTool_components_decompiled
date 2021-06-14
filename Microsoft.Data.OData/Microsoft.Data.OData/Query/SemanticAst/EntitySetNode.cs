using System;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Query.Metadata;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x020000D8 RID: 216
	internal sealed class EntitySetNode : EntityCollectionNode
	{
		// Token: 0x0600053C RID: 1340 RVA: 0x00011E40 File Offset: 0x00010040
		public EntitySetNode(IEdmEntitySet entitySet)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmEntitySet>(entitySet, "entitySet");
			this.entitySet = entitySet;
			this.entityType = new EdmEntityTypeReference(UriEdmHelpers.GetEntitySetElementType(this.EntitySet), false);
			this.collectionTypeReference = EdmCoreModel.GetCollection(this.entityType);
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00011E8D File Offset: 0x0001008D
		public override IEdmTypeReference ItemType
		{
			get
			{
				return this.entityType;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00011E95 File Offset: 0x00010095
		public override IEdmCollectionTypeReference CollectionType
		{
			get
			{
				return this.collectionTypeReference;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00011E9D File Offset: 0x0001009D
		public override IEdmEntityTypeReference EntityItemType
		{
			get
			{
				return this.entityType;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00011EA5 File Offset: 0x000100A5
		public override IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00011EAD File Offset: 0x000100AD
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.EntitySet;
			}
		}

		// Token: 0x04000225 RID: 549
		private readonly IEdmEntitySet entitySet;

		// Token: 0x04000226 RID: 550
		private readonly IEdmEntityTypeReference entityType;

		// Token: 0x04000227 RID: 551
		private readonly IEdmCollectionTypeReference collectionTypeReference;
	}
}
