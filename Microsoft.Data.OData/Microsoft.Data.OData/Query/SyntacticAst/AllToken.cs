using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x0200009D RID: 157
	internal sealed class AllToken : LambdaToken
	{
		// Token: 0x060003B3 RID: 947 RVA: 0x0000BD79 File Offset: 0x00009F79
		public AllToken(QueryToken expression, string parameter, QueryToken parent) : base(expression, parameter, parent)
		{
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000BD84 File Offset: 0x00009F84
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.All;
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000BD88 File Offset: 0x00009F88
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}
	}
}
