using System;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200002C RID: 44
	internal interface IFunctionCallParser
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000128 RID: 296
		ExpressionLexer Lexer { get; }

		// Token: 0x06000129 RID: 297
		QueryToken ParseIdentifierAsFunction(QueryToken parent);
	}
}
