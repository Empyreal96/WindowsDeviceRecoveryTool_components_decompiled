using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x02000088 RID: 136
	public interface IEdmPropertyReferenceExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000230 RID: 560
		IEdmExpression Base { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000231 RID: 561
		IEdmProperty ReferencedProperty { get; }
	}
}
