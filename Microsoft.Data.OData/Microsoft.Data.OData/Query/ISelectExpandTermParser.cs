using System;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000024 RID: 36
	internal interface ISelectExpandTermParser
	{
		// Token: 0x060000E8 RID: 232
		SelectToken ParseSelect();

		// Token: 0x060000E9 RID: 233
		ExpandToken ParseExpand();

		// Token: 0x060000EA RID: 234
		PathSegmentToken ParseSingleSelectTerm(bool isInnerTerm);

		// Token: 0x060000EB RID: 235
		ExpandTermToken ParseSingleExpandTerm(bool isInnerTerm);
	}
}
