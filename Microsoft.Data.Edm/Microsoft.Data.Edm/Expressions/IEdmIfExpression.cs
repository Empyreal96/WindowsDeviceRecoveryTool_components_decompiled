using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x02000057 RID: 87
	public interface IEdmIfExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000150 RID: 336
		IEdmExpression TestExpression { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000151 RID: 337
		IEdmExpression TrueExpression { get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000152 RID: 338
		IEdmExpression FalseExpression { get; }
	}
}
