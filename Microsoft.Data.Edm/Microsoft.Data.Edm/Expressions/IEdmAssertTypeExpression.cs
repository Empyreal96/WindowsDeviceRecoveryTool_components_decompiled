using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x0200003F RID: 63
	public interface IEdmAssertTypeExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060000DD RID: 221
		IEdmExpression Operand { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060000DE RID: 222
		IEdmTypeReference Type { get; }
	}
}
