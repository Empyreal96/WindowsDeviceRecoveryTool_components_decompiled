using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x020000DB RID: 219
	internal sealed class UnaryOperatorToken : QueryToken
	{
		// Token: 0x06000555 RID: 1365 RVA: 0x000127D8 File Offset: 0x000109D8
		public UnaryOperatorToken(UnaryOperatorKind operatorKind, QueryToken operand)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryToken>(operand, "operand");
			this.operatorKind = operatorKind;
			this.operand = operand;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x000127F9 File Offset: 0x000109F9
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.UnaryOperator;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x000127FC File Offset: 0x000109FC
		public UnaryOperatorKind OperatorKind
		{
			get
			{
				return this.operatorKind;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00012804 File Offset: 0x00010A04
		public QueryToken Operand
		{
			get
			{
				return this.operand;
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001280C File Offset: 0x00010A0C
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x0400023E RID: 574
		private readonly UnaryOperatorKind operatorKind;

		// Token: 0x0400023F RID: 575
		private readonly QueryToken operand;
	}
}
