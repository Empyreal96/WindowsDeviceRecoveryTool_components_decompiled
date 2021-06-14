using System;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000D5 RID: 213
	public enum QueryNodeKind
	{
		// Token: 0x040001F8 RID: 504
		None,
		// Token: 0x040001F9 RID: 505
		Constant,
		// Token: 0x040001FA RID: 506
		Convert,
		// Token: 0x040001FB RID: 507
		NonentityRangeVariableReference,
		// Token: 0x040001FC RID: 508
		BinaryOperator,
		// Token: 0x040001FD RID: 509
		UnaryOperator,
		// Token: 0x040001FE RID: 510
		SingleValuePropertyAccess,
		// Token: 0x040001FF RID: 511
		CollectionPropertyAccess,
		// Token: 0x04000200 RID: 512
		SingleValueFunctionCall,
		// Token: 0x04000201 RID: 513
		Any,
		// Token: 0x04000202 RID: 514
		CollectionNavigationNode,
		// Token: 0x04000203 RID: 515
		SingleNavigationNode,
		// Token: 0x04000204 RID: 516
		SingleValueOpenPropertyAccess,
		// Token: 0x04000205 RID: 517
		SingleEntityCast,
		// Token: 0x04000206 RID: 518
		All,
		// Token: 0x04000207 RID: 519
		EntityCollectionCast,
		// Token: 0x04000208 RID: 520
		EntityRangeVariableReference,
		// Token: 0x04000209 RID: 521
		SingleEntityFunctionCall,
		// Token: 0x0400020A RID: 522
		CollectionFunctionCall,
		// Token: 0x0400020B RID: 523
		EntityCollectionFunctionCall,
		// Token: 0x0400020C RID: 524
		NamedFunctionParameter
	}
}
