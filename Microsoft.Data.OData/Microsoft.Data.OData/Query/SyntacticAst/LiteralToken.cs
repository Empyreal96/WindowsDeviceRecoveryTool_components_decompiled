using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x020000CD RID: 205
	internal sealed class LiteralToken : QueryToken
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x00011590 File Offset: 0x0000F790
		public LiteralToken(object value)
		{
			this.value = value;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001159F File Offset: 0x0000F79F
		internal LiteralToken(object value, string originalText) : this(value)
		{
			this.originalText = originalText;
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000115AF File Offset: 0x0000F7AF
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.Literal;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000115B2 File Offset: 0x0000F7B2
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x000115BA File Offset: 0x0000F7BA
		internal string OriginalText
		{
			get
			{
				return this.originalText;
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x000115C2 File Offset: 0x0000F7C2
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x040001DA RID: 474
		private readonly string originalText;

		// Token: 0x040001DB RID: 475
		private readonly object value;
	}
}
