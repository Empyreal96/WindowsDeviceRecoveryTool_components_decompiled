using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000040 RID: 64
	internal class CsdlSemanticsAssertTypeExpression : CsdlSemanticsExpression, IEdmAssertTypeExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x060000DF RID: 223 RVA: 0x000034F5 File Offset: 0x000016F5
		public CsdlSemanticsAssertTypeExpression(CsdlAssertTypeExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003523 File Offset: 0x00001723
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000352B File Offset: 0x0000172B
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.AssertType;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000352F File Offset: 0x0000172F
		public IEdmExpression Operand
		{
			get
			{
				return this.operandCache.GetValue(this, CsdlSemanticsAssertTypeExpression.ComputeOperandFunc, null);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003543 File Offset: 0x00001743
		public IEdmTypeReference Type
		{
			get
			{
				return this.typeCache.GetValue(this, CsdlSemanticsAssertTypeExpression.ComputeTypeFunc, null);
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00003557 File Offset: 0x00001757
		private IEdmExpression ComputeOperand()
		{
			return CsdlSemanticsModel.WrapExpression(this.expression.Operand, this.bindingContext, base.Schema);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00003575 File Offset: 0x00001775
		private IEdmTypeReference ComputeType()
		{
			return CsdlSemanticsModel.WrapTypeReference(base.Schema, this.expression.Type);
		}

		// Token: 0x0400004E RID: 78
		private readonly CsdlAssertTypeExpression expression;

		// Token: 0x0400004F RID: 79
		private readonly IEdmEntityType bindingContext;

		// Token: 0x04000050 RID: 80
		private readonly Cache<CsdlSemanticsAssertTypeExpression, IEdmExpression> operandCache = new Cache<CsdlSemanticsAssertTypeExpression, IEdmExpression>();

		// Token: 0x04000051 RID: 81
		private static readonly Func<CsdlSemanticsAssertTypeExpression, IEdmExpression> ComputeOperandFunc = (CsdlSemanticsAssertTypeExpression me) => me.ComputeOperand();

		// Token: 0x04000052 RID: 82
		private readonly Cache<CsdlSemanticsAssertTypeExpression, IEdmTypeReference> typeCache = new Cache<CsdlSemanticsAssertTypeExpression, IEdmTypeReference>();

		// Token: 0x04000053 RID: 83
		private static readonly Func<CsdlSemanticsAssertTypeExpression, IEdmTypeReference> ComputeTypeFunc = (CsdlSemanticsAssertTypeExpression me) => me.ComputeType();
	}
}
