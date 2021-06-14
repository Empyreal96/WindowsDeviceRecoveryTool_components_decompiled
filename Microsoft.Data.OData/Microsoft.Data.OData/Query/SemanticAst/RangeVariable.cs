using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200007C RID: 124
	public abstract class RangeVariable : ODataAnnotatable
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002F1 RID: 753
		public abstract string Name { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002F2 RID: 754
		public abstract IEdmTypeReference TypeReference { get; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002F3 RID: 755
		public abstract int Kind { get; }
	}
}
