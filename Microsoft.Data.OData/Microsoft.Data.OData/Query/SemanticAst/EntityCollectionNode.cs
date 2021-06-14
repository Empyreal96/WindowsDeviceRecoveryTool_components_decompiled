using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000045 RID: 69
	public abstract class EntityCollectionNode : CollectionNode
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001B8 RID: 440
		public abstract IEdmEntityTypeReference EntityItemType { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001B9 RID: 441
		public abstract IEdmEntitySet EntitySet { get; }
	}
}
