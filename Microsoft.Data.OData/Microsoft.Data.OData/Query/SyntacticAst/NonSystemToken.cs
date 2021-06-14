using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000094 RID: 148
	internal sealed class NonSystemToken : PathSegmentToken
	{
		// Token: 0x0600037E RID: 894 RVA: 0x0000BAB0 File Offset: 0x00009CB0
		public NonSystemToken(string identifier, IEnumerable<NamedValue> namedValues, PathSegmentToken nextToken) : base(nextToken)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(identifier, "identifier");
			this.identifier = identifier;
			this.namedValues = namedValues;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000BAD2 File Offset: 0x00009CD2
		public IEnumerable<NamedValue> NamedValues
		{
			get
			{
				return this.namedValues;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000BADA File Offset: 0x00009CDA
		public override string Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000BAE2 File Offset: 0x00009CE2
		public override bool IsNamespaceOrContainerQualified()
		{
			return this.identifier.Contains(".");
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000BAF4 File Offset: 0x00009CF4
		public override T Accept<T>(IPathSegmentTokenVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<IPathSegmentTokenVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000BB08 File Offset: 0x00009D08
		public override void Accept(IPathSegmentTokenVisitor visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<IPathSegmentTokenVisitor>(visitor, "visitor");
			visitor.Visit(this);
		}

		// Token: 0x04000108 RID: 264
		private readonly IEnumerable<NamedValue> namedValues;

		// Token: 0x04000109 RID: 265
		private readonly string identifier;
	}
}
