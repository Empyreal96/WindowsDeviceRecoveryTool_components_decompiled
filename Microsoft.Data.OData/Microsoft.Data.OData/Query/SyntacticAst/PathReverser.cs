using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000055 RID: 85
	internal sealed class PathReverser : PathSegmentTokenVisitor<PathSegmentToken>
	{
		// Token: 0x0600022F RID: 559 RVA: 0x000084E0 File Offset: 0x000066E0
		public PathReverser()
		{
			this.childToken = null;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000084EF File Offset: 0x000066EF
		private PathReverser(PathSegmentToken childToken)
		{
			this.childToken = childToken;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008500 File Offset: 0x00006700
		public override PathSegmentToken Visit(NonSystemToken tokenIn)
		{
			ExceptionUtils.CheckArgumentNotNull<NonSystemToken>(tokenIn, "tokenIn");
			if (tokenIn.NextToken != null)
			{
				NonSystemToken nextChildToken = new NonSystemToken(tokenIn.Identifier, tokenIn.NamedValues, this.childToken);
				return PathReverser.BuildNextStep(tokenIn.NextToken, nextChildToken);
			}
			return new NonSystemToken(tokenIn.Identifier, tokenIn.NamedValues, this.childToken);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000855C File Offset: 0x0000675C
		public override PathSegmentToken Visit(SystemToken tokenIn)
		{
			ExceptionUtils.CheckArgumentNotNull<SystemToken>(tokenIn, "tokenIn");
			if (tokenIn.NextToken != null)
			{
				SystemToken nextChildToken = new SystemToken(tokenIn.Identifier, this.childToken);
				return PathReverser.BuildNextStep(tokenIn.NextToken, nextChildToken);
			}
			return new SystemToken(tokenIn.Identifier, this.childToken);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000085AC File Offset: 0x000067AC
		private static PathSegmentToken BuildNextStep(PathSegmentToken nextLevelToken, PathSegmentToken nextChildToken)
		{
			PathReverser visitor = new PathReverser(nextChildToken);
			return nextLevelToken.Accept<PathSegmentToken>(visitor);
		}

		// Token: 0x04000088 RID: 136
		private readonly PathSegmentToken childToken;
	}
}
