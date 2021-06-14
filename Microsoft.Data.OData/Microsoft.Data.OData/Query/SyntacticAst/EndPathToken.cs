using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x020000D2 RID: 210
	internal sealed class EndPathToken : PathToken
	{
		// Token: 0x0600051F RID: 1311 RVA: 0x00011A2B File Offset: 0x0000FC2B
		public EndPathToken(string identifier, QueryToken nextToken)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(identifier, "Identifier");
			this.identifier = identifier;
			this.nextToken = nextToken;
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00011A4C File Offset: 0x0000FC4C
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.EndPath;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00011A4F File Offset: 0x0000FC4F
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x00011A57 File Offset: 0x0000FC57
		public override QueryToken NextToken
		{
			get
			{
				return this.nextToken;
			}
			set
			{
				this.nextToken = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00011A60 File Offset: 0x0000FC60
		public override string Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00011A68 File Offset: 0x0000FC68
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x040001E2 RID: 482
		private readonly string identifier;

		// Token: 0x040001E3 RID: 483
		private QueryToken nextToken;
	}
}
