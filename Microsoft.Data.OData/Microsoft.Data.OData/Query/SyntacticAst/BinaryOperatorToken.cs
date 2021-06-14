using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x020000A7 RID: 167
	internal sealed class BinaryOperatorToken : QueryToken
	{
		// Token: 0x060003E6 RID: 998 RVA: 0x0000C502 File Offset: 0x0000A702
		public BinaryOperatorToken(BinaryOperatorKind operatorKind, QueryToken left, QueryToken right)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryToken>(left, "left");
			ExceptionUtils.CheckArgumentNotNull<QueryToken>(right, "right");
			this.operatorKind = operatorKind;
			this.left = left;
			this.right = right;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000C535 File Offset: 0x0000A735
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.BinaryOperator;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000C538 File Offset: 0x0000A738
		public BinaryOperatorKind OperatorKind
		{
			get
			{
				return this.operatorKind;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000C540 File Offset: 0x0000A740
		public QueryToken Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000C548 File Offset: 0x0000A748
		public QueryToken Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000C550 File Offset: 0x0000A750
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x0400013C RID: 316
		private readonly BinaryOperatorKind operatorKind;

		// Token: 0x0400013D RID: 317
		private readonly QueryToken left;

		// Token: 0x0400013E RID: 318
		private readonly QueryToken right;
	}
}
