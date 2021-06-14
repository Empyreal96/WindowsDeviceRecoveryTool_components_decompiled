using System;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000C8 RID: 200
	internal enum ExpressionTokenKind
	{
		// Token: 0x040001AE RID: 430
		Unknown,
		// Token: 0x040001AF RID: 431
		End,
		// Token: 0x040001B0 RID: 432
		Equal,
		// Token: 0x040001B1 RID: 433
		Identifier,
		// Token: 0x040001B2 RID: 434
		NullLiteral,
		// Token: 0x040001B3 RID: 435
		BooleanLiteral,
		// Token: 0x040001B4 RID: 436
		StringLiteral,
		// Token: 0x040001B5 RID: 437
		IntegerLiteral,
		// Token: 0x040001B6 RID: 438
		Int64Literal,
		// Token: 0x040001B7 RID: 439
		SingleLiteral,
		// Token: 0x040001B8 RID: 440
		DateTimeLiteral,
		// Token: 0x040001B9 RID: 441
		DateTimeOffsetLiteral,
		// Token: 0x040001BA RID: 442
		TimeLiteral,
		// Token: 0x040001BB RID: 443
		DecimalLiteral,
		// Token: 0x040001BC RID: 444
		DoubleLiteral,
		// Token: 0x040001BD RID: 445
		GuidLiteral,
		// Token: 0x040001BE RID: 446
		BinaryLiteral,
		// Token: 0x040001BF RID: 447
		GeographyLiteral,
		// Token: 0x040001C0 RID: 448
		GeometryLiteral,
		// Token: 0x040001C1 RID: 449
		Exclamation,
		// Token: 0x040001C2 RID: 450
		OpenParen,
		// Token: 0x040001C3 RID: 451
		CloseParen,
		// Token: 0x040001C4 RID: 452
		Comma,
		// Token: 0x040001C5 RID: 453
		Colon,
		// Token: 0x040001C6 RID: 454
		Minus,
		// Token: 0x040001C7 RID: 455
		Slash,
		// Token: 0x040001C8 RID: 456
		Question,
		// Token: 0x040001C9 RID: 457
		Dot,
		// Token: 0x040001CA RID: 458
		Star,
		// Token: 0x040001CB RID: 459
		SemiColon,
		// Token: 0x040001CC RID: 460
		ParameterAlias,
		// Token: 0x040001CD RID: 461
		BracketedExpression
	}
}
