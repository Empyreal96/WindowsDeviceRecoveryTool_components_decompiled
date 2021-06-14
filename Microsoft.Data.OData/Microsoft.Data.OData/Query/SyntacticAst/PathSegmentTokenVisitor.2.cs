using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000057 RID: 87
	internal abstract class PathSegmentTokenVisitor : IPathSegmentTokenVisitor
	{
		// Token: 0x06000238 RID: 568 RVA: 0x00008634 File Offset: 0x00006834
		public virtual void Visit(SystemToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000863B File Offset: 0x0000683B
		public virtual void Visit(NonSystemToken tokenIn)
		{
			throw new NotImplementedException();
		}
	}
}
