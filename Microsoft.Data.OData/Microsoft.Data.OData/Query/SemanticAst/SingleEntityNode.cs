using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200007F RID: 127
	public abstract class SingleEntityNode : SingleValueNode
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000300 RID: 768
		public abstract IEdmEntitySet EntitySet { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000301 RID: 769
		public abstract IEdmEntityTypeReference EntityTypeReference { get; }
	}
}
