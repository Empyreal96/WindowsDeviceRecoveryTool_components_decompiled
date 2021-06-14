using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x0200003D RID: 61
	public interface IEdmApplyExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060000CE RID: 206
		IEdmExpression AppliedFunction { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060000CF RID: 207
		IEnumerable<IEdmExpression> Arguments { get; }
	}
}
