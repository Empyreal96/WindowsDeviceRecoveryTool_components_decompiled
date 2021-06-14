using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200006C RID: 108
	internal sealed class Expansion
	{
		// Token: 0x06000298 RID: 664 RVA: 0x0000A3BB File Offset: 0x000085BB
		public Expansion(IEnumerable<ExpandedNavigationSelectItem> expandItems)
		{
			this.expandItems = ((expandItems as ExpandedNavigationSelectItem[]) ?? expandItems.ToArray<ExpandedNavigationSelectItem>());
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000A3D9 File Offset: 0x000085D9
		public IEnumerable<ExpandedNavigationSelectItem> ExpandItems
		{
			get
			{
				return this.expandItems;
			}
		}

		// Token: 0x040000B4 RID: 180
		private readonly IEnumerable<ExpandedNavigationSelectItem> expandItems;
	}
}
