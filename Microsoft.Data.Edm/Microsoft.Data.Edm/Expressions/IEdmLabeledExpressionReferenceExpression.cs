using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x0200005D RID: 93
	public interface IEdmLabeledExpressionReferenceExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000178 RID: 376
		IEdmLabeledExpression ReferencedLabeledExpression { get; }
	}
}
