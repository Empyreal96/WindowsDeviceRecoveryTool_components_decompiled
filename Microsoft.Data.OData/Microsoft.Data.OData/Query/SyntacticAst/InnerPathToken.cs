using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x020000A4 RID: 164
	internal sealed class InnerPathToken : PathToken
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x0000BE3E File Offset: 0x0000A03E
		public InnerPathToken(string identifier, QueryToken nextToken, IEnumerable<NamedValue> namedValues)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(identifier, "Identifier");
			this.identifier = identifier;
			this.nextToken = nextToken;
			this.namedValues = ((namedValues == null) ? null : new ReadOnlyEnumerableForUriParser<NamedValue>(namedValues));
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0000BE71 File Offset: 0x0000A071
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.InnerPath;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0000BE75 File Offset: 0x0000A075
		public override string Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0000BE7D File Offset: 0x0000A07D
		// (set) Token: 0x060003CD RID: 973 RVA: 0x0000BE85 File Offset: 0x0000A085
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

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000BE8E File Offset: 0x0000A08E
		public IEnumerable<NamedValue> NamedValues
		{
			get
			{
				return this.namedValues;
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000BE96 File Offset: 0x0000A096
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x04000129 RID: 297
		private readonly string identifier;

		// Token: 0x0400012A RID: 298
		private readonly IEnumerable<NamedValue> namedValues;

		// Token: 0x0400012B RID: 299
		private QueryToken nextToken;
	}
}
