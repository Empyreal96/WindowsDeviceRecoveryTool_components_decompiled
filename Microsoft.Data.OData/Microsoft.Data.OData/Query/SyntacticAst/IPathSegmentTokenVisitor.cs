using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x0200002F RID: 47
	internal interface IPathSegmentTokenVisitor<T>
	{
		// Token: 0x06000137 RID: 311
		T Visit(SystemToken tokenIn);

		// Token: 0x06000138 RID: 312
		T Visit(NonSystemToken tokenIn);
	}
}
