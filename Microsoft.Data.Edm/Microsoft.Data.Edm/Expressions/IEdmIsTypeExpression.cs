using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x02000059 RID: 89
	public interface IEdmIsTypeExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000161 RID: 353
		IEdmExpression Operand { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000162 RID: 354
		IEdmTypeReference Type { get; }
	}
}
