using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200000B RID: 11
	internal class CsdlAssertTypeExpression : CsdlExpressionBase
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002981 File Offset: 0x00000B81
		public CsdlAssertTypeExpression(CsdlTypeReference type, CsdlExpressionBase operand, CsdlLocation location) : base(location)
		{
			this.type = type;
			this.operand = operand;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002998 File Offset: 0x00000B98
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.AssertType;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000299C File Offset: 0x00000B9C
		public CsdlTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000029A4 File Offset: 0x00000BA4
		public CsdlExpressionBase Operand
		{
			get
			{
				return this.operand;
			}
		}

		// Token: 0x0400000F RID: 15
		private readonly CsdlTypeReference type;

		// Token: 0x04000010 RID: 16
		private readonly CsdlExpressionBase operand;
	}
}
