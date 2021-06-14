using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x02000041 RID: 65
	public interface IEdmCollectionExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060000E9 RID: 233
		IEdmTypeReference DeclaredType { get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060000EA RID: 234
		IEnumerable<IEdmExpression> Elements { get; }
	}
}
