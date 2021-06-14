using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000013 RID: 19
	internal abstract class QueryToken : ODataAnnotatable
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000072 RID: 114
		public abstract QueryTokenKind Kind { get; }

		// Token: 0x06000073 RID: 115
		public abstract T Accept<T>(ISyntacticTreeVisitor<T> visitor);

		// Token: 0x04000030 RID: 48
		public static readonly QueryToken[] EmptyTokens = new QueryToken[0];
	}
}
