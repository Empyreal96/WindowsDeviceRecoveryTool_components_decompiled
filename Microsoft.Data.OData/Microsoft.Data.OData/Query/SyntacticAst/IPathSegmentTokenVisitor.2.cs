using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000030 RID: 48
	internal interface IPathSegmentTokenVisitor
	{
		// Token: 0x06000139 RID: 313
		void Visit(SystemToken tokenIn);

		// Token: 0x0600013A RID: 314
		void Visit(NonSystemToken tokenIn);
	}
}
