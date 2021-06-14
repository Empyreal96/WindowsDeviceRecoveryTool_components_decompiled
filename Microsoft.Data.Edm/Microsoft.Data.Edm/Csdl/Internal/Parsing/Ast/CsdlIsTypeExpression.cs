using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000013 RID: 19
	internal class CsdlIsTypeExpression : CsdlExpressionBase
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00002ADA File Offset: 0x00000CDA
		public CsdlIsTypeExpression(CsdlTypeReference type, CsdlExpressionBase operand, CsdlLocation location) : base(location)
		{
			this.type = type;
			this.operand = operand;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002AF1 File Offset: 0x00000CF1
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.IsType;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002AF5 File Offset: 0x00000CF5
		public CsdlTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002AFD File Offset: 0x00000CFD
		public CsdlExpressionBase Operand
		{
			get
			{
				return this.operand;
			}
		}

		// Token: 0x0400001D RID: 29
		private readonly CsdlTypeReference type;

		// Token: 0x0400001E RID: 30
		private readonly CsdlExpressionBase operand;
	}
}
