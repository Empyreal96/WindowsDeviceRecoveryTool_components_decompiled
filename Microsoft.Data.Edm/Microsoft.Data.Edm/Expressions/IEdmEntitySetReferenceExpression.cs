using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x0200004B RID: 75
	public interface IEdmEntitySetReferenceExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600011B RID: 283
		IEdmEntitySet ReferencedEntitySet { get; }
	}
}
