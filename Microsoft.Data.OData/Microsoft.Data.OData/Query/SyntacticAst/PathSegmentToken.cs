using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000093 RID: 147
	internal abstract class PathSegmentToken : ODataAnnotatable
	{
		// Token: 0x06000377 RID: 887 RVA: 0x0000BA90 File Offset: 0x00009C90
		protected PathSegmentToken(PathSegmentToken nextToken)
		{
			this.nextToken = nextToken;
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0000BA9F File Offset: 0x00009C9F
		public PathSegmentToken NextToken
		{
			get
			{
				return this.nextToken;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000379 RID: 889
		public abstract string Identifier { get; }

		// Token: 0x0600037A RID: 890
		public abstract bool IsNamespaceOrContainerQualified();

		// Token: 0x0600037B RID: 891
		public abstract T Accept<T>(IPathSegmentTokenVisitor<T> visitor);

		// Token: 0x0600037C RID: 892
		public abstract void Accept(IPathSegmentTokenVisitor visitor);

		// Token: 0x0600037D RID: 893 RVA: 0x0000BAA7 File Offset: 0x00009CA7
		internal void SetNextToken(PathSegmentToken nextTokenIn)
		{
			this.nextToken = nextTokenIn;
		}

		// Token: 0x04000107 RID: 263
		private PathSegmentToken nextToken;
	}
}
