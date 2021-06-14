using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000015 RID: 21
	internal sealed class FunctionParameterAliasToken : QueryToken
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00003675 File Offset: 0x00001875
		public FunctionParameterAliasToken(string alias)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(alias, "alias");
			this.Alias = alias;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000368F File Offset: 0x0000188F
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003697 File Offset: 0x00001897
		public string Alias { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000036A0 File Offset: 0x000018A0
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.FunctionParameterAlias;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000036A4 File Offset: 0x000018A4
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			throw new NotImplementedException();
		}
	}
}
