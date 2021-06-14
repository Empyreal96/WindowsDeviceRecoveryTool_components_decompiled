using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x0200005B RID: 91
	public interface IEdmLabeledExpression : IEdmNamedElement, IEdmExpression, IEdmElement
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600016D RID: 365
		IEdmExpression Expression { get; }
	}
}
