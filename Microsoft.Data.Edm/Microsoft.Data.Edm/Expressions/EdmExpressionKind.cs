using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x0200012C RID: 300
	public enum EdmExpressionKind
	{
		// Token: 0x0400021F RID: 543
		None,
		// Token: 0x04000220 RID: 544
		BinaryConstant,
		// Token: 0x04000221 RID: 545
		BooleanConstant,
		// Token: 0x04000222 RID: 546
		DateTimeConstant,
		// Token: 0x04000223 RID: 547
		DateTimeOffsetConstant,
		// Token: 0x04000224 RID: 548
		DecimalConstant,
		// Token: 0x04000225 RID: 549
		FloatingConstant,
		// Token: 0x04000226 RID: 550
		GuidConstant,
		// Token: 0x04000227 RID: 551
		IntegerConstant,
		// Token: 0x04000228 RID: 552
		StringConstant,
		// Token: 0x04000229 RID: 553
		TimeConstant,
		// Token: 0x0400022A RID: 554
		Null,
		// Token: 0x0400022B RID: 555
		Record,
		// Token: 0x0400022C RID: 556
		Collection,
		// Token: 0x0400022D RID: 557
		Path,
		// Token: 0x0400022E RID: 558
		ParameterReference,
		// Token: 0x0400022F RID: 559
		FunctionReference,
		// Token: 0x04000230 RID: 560
		PropertyReference,
		// Token: 0x04000231 RID: 561
		ValueTermReference,
		// Token: 0x04000232 RID: 562
		EntitySetReference,
		// Token: 0x04000233 RID: 563
		EnumMemberReference,
		// Token: 0x04000234 RID: 564
		If,
		// Token: 0x04000235 RID: 565
		AssertType,
		// Token: 0x04000236 RID: 566
		IsType,
		// Token: 0x04000237 RID: 567
		FunctionApplication,
		// Token: 0x04000238 RID: 568
		LabeledExpressionReference,
		// Token: 0x04000239 RID: 569
		Labeled
	}
}
