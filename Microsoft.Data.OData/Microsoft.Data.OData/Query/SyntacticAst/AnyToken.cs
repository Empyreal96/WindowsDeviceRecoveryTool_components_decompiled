using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x0200009F RID: 159
	internal sealed class AnyToken : LambdaToken
	{
		// Token: 0x060003BB RID: 955 RVA: 0x0000BDCA File Offset: 0x00009FCA
		public AnyToken(QueryToken expression, string parameter, QueryToken parent) : base(expression, parameter, parent)
		{
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0000BDD5 File Offset: 0x00009FD5
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.Any;
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000BDD9 File Offset: 0x00009FD9
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}
	}
}
