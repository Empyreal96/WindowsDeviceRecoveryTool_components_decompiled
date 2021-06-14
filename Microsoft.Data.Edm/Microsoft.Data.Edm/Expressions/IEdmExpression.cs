using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x0200003B RID: 59
	public interface IEdmExpression : IEdmElement
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000C9 RID: 201
		EdmExpressionKind ExpressionKind { get; }
	}
}
