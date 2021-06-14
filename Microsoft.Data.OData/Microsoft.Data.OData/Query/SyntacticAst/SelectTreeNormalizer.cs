using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000061 RID: 97
	internal static class SelectTreeNormalizer
	{
		// Token: 0x0600026D RID: 621 RVA: 0x00009F5C File Offset: 0x0000815C
		public static SelectToken NormalizeSelectTree(SelectToken treeToNormalize)
		{
			PathReverser pathReverser = new PathReverser();
			List<PathSegmentToken> properties = (from property in treeToNormalize.Properties
			select property.Accept<PathSegmentToken>(pathReverser)).ToList<PathSegmentToken>();
			return new SelectToken(properties);
		}
	}
}
