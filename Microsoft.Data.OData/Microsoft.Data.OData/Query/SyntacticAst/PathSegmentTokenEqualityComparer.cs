using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000056 RID: 86
	internal sealed class PathSegmentTokenEqualityComparer : EqualityComparer<PathSegmentToken>
	{
		// Token: 0x06000234 RID: 564 RVA: 0x000085C7 File Offset: 0x000067C7
		public override bool Equals(PathSegmentToken first, PathSegmentToken second)
		{
			return (first == null && second == null) || (first != null && second != null && this.ToHashableString(first) == this.ToHashableString(second));
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000085EC File Offset: 0x000067EC
		public override int GetHashCode(PathSegmentToken path)
		{
			if (path == null)
			{
				return 0;
			}
			return this.ToHashableString(path).GetHashCode();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000085FF File Offset: 0x000067FF
		private string ToHashableString(PathSegmentToken token)
		{
			if (token.NextToken == null)
			{
				return token.Identifier;
			}
			return token.Identifier + "/" + this.ToHashableString(token.NextToken);
		}
	}
}
