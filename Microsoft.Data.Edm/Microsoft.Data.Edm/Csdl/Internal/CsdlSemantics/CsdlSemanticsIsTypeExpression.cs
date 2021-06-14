using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200005A RID: 90
	internal class CsdlSemanticsIsTypeExpression : CsdlSemanticsExpression, IEdmIsTypeExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00004B0F File Offset: 0x00002D0F
		public CsdlSemanticsIsTypeExpression(CsdlIsTypeExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00004B3D File Offset: 0x00002D3D
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00004B45 File Offset: 0x00002D45
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.IsType;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00004B49 File Offset: 0x00002D49
		public IEdmExpression Operand
		{
			get
			{
				return this.operandCache.GetValue(this, CsdlSemanticsIsTypeExpression.ComputeOperandFunc, null);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00004B5D File Offset: 0x00002D5D
		public IEdmTypeReference Type
		{
			get
			{
				return this.typeCache.GetValue(this, CsdlSemanticsIsTypeExpression.ComputeTypeFunc, null);
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00004B71 File Offset: 0x00002D71
		private IEdmExpression ComputeOperand()
		{
			return CsdlSemanticsModel.WrapExpression(this.expression.Operand, this.bindingContext, base.Schema);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00004B8F File Offset: 0x00002D8F
		private IEdmTypeReference ComputeType()
		{
			return CsdlSemanticsModel.WrapTypeReference(base.Schema, this.expression.Type);
		}

		// Token: 0x04000091 RID: 145
		private readonly CsdlIsTypeExpression expression;

		// Token: 0x04000092 RID: 146
		private readonly IEdmEntityType bindingContext;

		// Token: 0x04000093 RID: 147
		private readonly Cache<CsdlSemanticsIsTypeExpression, IEdmExpression> operandCache = new Cache<CsdlSemanticsIsTypeExpression, IEdmExpression>();

		// Token: 0x04000094 RID: 148
		private static readonly Func<CsdlSemanticsIsTypeExpression, IEdmExpression> ComputeOperandFunc = (CsdlSemanticsIsTypeExpression me) => me.ComputeOperand();

		// Token: 0x04000095 RID: 149
		private readonly Cache<CsdlSemanticsIsTypeExpression, IEdmTypeReference> typeCache = new Cache<CsdlSemanticsIsTypeExpression, IEdmTypeReference>();

		// Token: 0x04000096 RID: 150
		private static readonly Func<CsdlSemanticsIsTypeExpression, IEdmTypeReference> ComputeTypeFunc = (CsdlSemanticsIsTypeExpression me) => me.ComputeType();
	}
}
