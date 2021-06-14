using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x02000090 RID: 144
	public interface IEdmRecordExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000259 RID: 601
		IEdmStructuredTypeReference DeclaredType { get; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600025A RID: 602
		IEnumerable<IEdmPropertyConstructor> Properties { get; }
	}
}
