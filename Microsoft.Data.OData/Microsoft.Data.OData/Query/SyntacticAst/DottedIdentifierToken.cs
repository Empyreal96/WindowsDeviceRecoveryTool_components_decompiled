using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x020000C2 RID: 194
	internal sealed class DottedIdentifierToken : PathToken
	{
		// Token: 0x060004B1 RID: 1201 RVA: 0x0000FEB7 File Offset: 0x0000E0B7
		public DottedIdentifierToken(string identifier, QueryToken nextToken)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(identifier, "Identifier");
			this.identifier = identifier;
			this.nextToken = nextToken;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0000FED8 File Offset: 0x0000E0D8
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.DottedIdentifier;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0000FEDC File Offset: 0x0000E0DC
		public override string Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0000FEE4 File Offset: 0x0000E0E4
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x0000FEEC File Offset: 0x0000E0EC
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

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000FEF5 File Offset: 0x0000E0F5
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x04000199 RID: 409
		private readonly string identifier;

		// Token: 0x0400019A RID: 410
		private QueryToken nextToken;
	}
}
