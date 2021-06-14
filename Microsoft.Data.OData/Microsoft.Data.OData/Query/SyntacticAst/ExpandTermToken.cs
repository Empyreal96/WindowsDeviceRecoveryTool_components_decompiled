using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000091 RID: 145
	internal sealed class ExpandTermToken : QueryToken
	{
		// Token: 0x06000366 RID: 870 RVA: 0x0000B8EC File Offset: 0x00009AEC
		public ExpandTermToken(PathSegmentToken pathToNavProp)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentToken>(pathToNavProp, "pathToNavigationProperty");
			this.pathToNavProp = pathToNavProp;
			this.filterOption = null;
			this.orderByOption = null;
			this.topOption = null;
			this.skipOption = null;
			this.inlineCountOption = null;
			this.selectOption = null;
			this.expandOption = null;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000B954 File Offset: 0x00009B54
		public ExpandTermToken(PathSegmentToken pathToNavProp, SelectToken selectOption, ExpandToken expandOption)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentToken>(pathToNavProp, "pathToNavigationProperty");
			this.pathToNavProp = pathToNavProp;
			this.filterOption = null;
			this.orderByOption = null;
			this.topOption = null;
			this.skipOption = null;
			this.selectOption = selectOption;
			this.expandOption = expandOption;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000B9B0 File Offset: 0x00009BB0
		public ExpandTermToken(PathSegmentToken pathToNavProp, QueryToken filterOption, OrderByToken orderByOption, long? topOption, long? skipOption, InlineCountKind? inlineCountOption, SelectToken selectOption, ExpandToken expandOption)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentToken>(pathToNavProp, "property");
			this.pathToNavProp = pathToNavProp;
			this.filterOption = filterOption;
			this.orderByOption = orderByOption;
			this.topOption = topOption;
			this.skipOption = skipOption;
			this.inlineCountOption = inlineCountOption;
			this.selectOption = selectOption;
			this.expandOption = expandOption;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0000BA0B File Offset: 0x00009C0B
		public PathSegmentToken PathToNavProp
		{
			get
			{
				return this.pathToNavProp;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0000BA13 File Offset: 0x00009C13
		public QueryToken FilterOption
		{
			get
			{
				return this.filterOption;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000BA1B File Offset: 0x00009C1B
		public OrderByToken OrderByOption
		{
			get
			{
				return this.orderByOption;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0000BA23 File Offset: 0x00009C23
		public long? TopOption
		{
			get
			{
				return this.topOption;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000BA2B File Offset: 0x00009C2B
		public long? SkipOption
		{
			get
			{
				return this.skipOption;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000BA33 File Offset: 0x00009C33
		public InlineCountKind? InlineCountOption
		{
			get
			{
				return this.inlineCountOption;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000BA3B File Offset: 0x00009C3B
		public SelectToken SelectOption
		{
			get
			{
				return this.selectOption;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000BA43 File Offset: 0x00009C43
		public ExpandToken ExpandOption
		{
			get
			{
				return this.expandOption;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000BA4B File Offset: 0x00009C4B
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.ExpandTerm;
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000BA4F File Offset: 0x00009C4F
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x040000FE RID: 254
		private readonly PathSegmentToken pathToNavProp;

		// Token: 0x040000FF RID: 255
		private readonly QueryToken filterOption;

		// Token: 0x04000100 RID: 256
		private readonly OrderByToken orderByOption;

		// Token: 0x04000101 RID: 257
		private readonly long? topOption;

		// Token: 0x04000102 RID: 258
		private readonly long? skipOption;

		// Token: 0x04000103 RID: 259
		private readonly InlineCountKind? inlineCountOption;

		// Token: 0x04000104 RID: 260
		private readonly SelectToken selectOption;

		// Token: 0x04000105 RID: 261
		private readonly ExpandToken expandOption;
	}
}
