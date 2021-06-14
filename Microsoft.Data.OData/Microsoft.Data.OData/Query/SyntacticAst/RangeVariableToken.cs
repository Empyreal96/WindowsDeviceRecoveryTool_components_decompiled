using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x020000B4 RID: 180
	internal sealed class RangeVariableToken : QueryToken
	{
		// Token: 0x06000469 RID: 1129 RVA: 0x0000E8ED File Offset: 0x0000CAED
		public RangeVariableToken(string name)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(name, "visitor");
			this.name = name;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0000E907 File Offset: 0x0000CB07
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.RangeVariable;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0000E90B File Offset: 0x0000CB0B
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000E913 File Offset: 0x0000CB13
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x0400017D RID: 381
		private readonly string name;
	}
}
