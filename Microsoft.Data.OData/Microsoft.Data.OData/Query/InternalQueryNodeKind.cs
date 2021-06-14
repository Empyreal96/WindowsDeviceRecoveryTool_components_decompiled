using System;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000D6 RID: 214
	internal enum InternalQueryNodeKind
	{
		// Token: 0x0400020E RID: 526
		None,
		// Token: 0x0400020F RID: 527
		Constant,
		// Token: 0x04000210 RID: 528
		Convert,
		// Token: 0x04000211 RID: 529
		NonentityRangeVariableReference,
		// Token: 0x04000212 RID: 530
		BinaryOperator,
		// Token: 0x04000213 RID: 531
		UnaryOperator,
		// Token: 0x04000214 RID: 532
		SingleValuePropertyAccess,
		// Token: 0x04000215 RID: 533
		CollectionPropertyAccess,
		// Token: 0x04000216 RID: 534
		SingleValueFunctionCall,
		// Token: 0x04000217 RID: 535
		Any,
		// Token: 0x04000218 RID: 536
		CollectionNavigationNode,
		// Token: 0x04000219 RID: 537
		SingleNavigationNode,
		// Token: 0x0400021A RID: 538
		SingleValueOpenPropertyAccess,
		// Token: 0x0400021B RID: 539
		SingleEntityCast,
		// Token: 0x0400021C RID: 540
		All,
		// Token: 0x0400021D RID: 541
		EntityCollectionCast,
		// Token: 0x0400021E RID: 542
		EntityRangeVariableReference,
		// Token: 0x0400021F RID: 543
		SingleEntityFunctionCall,
		// Token: 0x04000220 RID: 544
		CollectionFunctionCall,
		// Token: 0x04000221 RID: 545
		EntityCollectionFunctionCall,
		// Token: 0x04000222 RID: 546
		NamedFunctionParameter,
		// Token: 0x04000223 RID: 547
		EntitySet,
		// Token: 0x04000224 RID: 548
		KeyLookup
	}
}
