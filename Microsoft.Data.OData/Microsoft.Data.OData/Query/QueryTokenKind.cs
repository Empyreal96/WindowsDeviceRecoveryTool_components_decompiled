using System;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000DA RID: 218
	internal enum QueryTokenKind
	{
		// Token: 0x0400022A RID: 554
		BinaryOperator = 3,
		// Token: 0x0400022B RID: 555
		UnaryOperator,
		// Token: 0x0400022C RID: 556
		Literal,
		// Token: 0x0400022D RID: 557
		FunctionCall,
		// Token: 0x0400022E RID: 558
		EndPath,
		// Token: 0x0400022F RID: 559
		OrderBy,
		// Token: 0x04000230 RID: 560
		CustomQueryOption,
		// Token: 0x04000231 RID: 561
		Select,
		// Token: 0x04000232 RID: 562
		Star,
		// Token: 0x04000233 RID: 563
		Expand = 13,
		// Token: 0x04000234 RID: 564
		TypeSegment,
		// Token: 0x04000235 RID: 565
		Any,
		// Token: 0x04000236 RID: 566
		InnerPath,
		// Token: 0x04000237 RID: 567
		DottedIdentifier,
		// Token: 0x04000238 RID: 568
		RangeVariable,
		// Token: 0x04000239 RID: 569
		All,
		// Token: 0x0400023A RID: 570
		ExpandTerm,
		// Token: 0x0400023B RID: 571
		FunctionParameter,
		// Token: 0x0400023C RID: 572
		FunctionParameterAlias,
		// Token: 0x0400023D RID: 573
		RawFunctionParameterValue
	}
}
