using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000058 RID: 88
	internal class CsdlSemanticsIfExpression : CsdlSemanticsExpression, IEdmIfExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x06000153 RID: 339 RVA: 0x000049A1 File Offset: 0x00002BA1
		public CsdlSemanticsIfExpression(CsdlIfExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000049DA File Offset: 0x00002BDA
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000049E2 File Offset: 0x00002BE2
		public IEdmEntityType BindingContext
		{
			get
			{
				return this.bindingContext;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000049EA File Offset: 0x00002BEA
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.If;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000049EE File Offset: 0x00002BEE
		public IEdmExpression TestExpression
		{
			get
			{
				return this.testCache.GetValue(this, CsdlSemanticsIfExpression.ComputeTestFunc, null);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00004A02 File Offset: 0x00002C02
		public IEdmExpression TrueExpression
		{
			get
			{
				return this.ifTrueCache.GetValue(this, CsdlSemanticsIfExpression.ComputeIfTrueFunc, null);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00004A16 File Offset: 0x00002C16
		public IEdmExpression FalseExpression
		{
			get
			{
				return this.ifFalseCache.GetValue(this, CsdlSemanticsIfExpression.ComputeIfFalseFunc, null);
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004A2A File Offset: 0x00002C2A
		private IEdmExpression ComputeTest()
		{
			return CsdlSemanticsModel.WrapExpression(this.expression.Test, this.BindingContext, base.Schema);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00004A48 File Offset: 0x00002C48
		private IEdmExpression ComputeIfTrue()
		{
			return CsdlSemanticsModel.WrapExpression(this.expression.IfTrue, this.BindingContext, base.Schema);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004A66 File Offset: 0x00002C66
		private IEdmExpression ComputeIfFalse()
		{
			return CsdlSemanticsModel.WrapExpression(this.expression.IfFalse, this.BindingContext, base.Schema);
		}

		// Token: 0x04000086 RID: 134
		private readonly CsdlIfExpression expression;

		// Token: 0x04000087 RID: 135
		private readonly IEdmEntityType bindingContext;

		// Token: 0x04000088 RID: 136
		private readonly Cache<CsdlSemanticsIfExpression, IEdmExpression> testCache = new Cache<CsdlSemanticsIfExpression, IEdmExpression>();

		// Token: 0x04000089 RID: 137
		private static readonly Func<CsdlSemanticsIfExpression, IEdmExpression> ComputeTestFunc = (CsdlSemanticsIfExpression me) => me.ComputeTest();

		// Token: 0x0400008A RID: 138
		private readonly Cache<CsdlSemanticsIfExpression, IEdmExpression> ifTrueCache = new Cache<CsdlSemanticsIfExpression, IEdmExpression>();

		// Token: 0x0400008B RID: 139
		private static readonly Func<CsdlSemanticsIfExpression, IEdmExpression> ComputeIfTrueFunc = (CsdlSemanticsIfExpression me) => me.ComputeIfTrue();

		// Token: 0x0400008C RID: 140
		private readonly Cache<CsdlSemanticsIfExpression, IEdmExpression> ifFalseCache = new Cache<CsdlSemanticsIfExpression, IEdmExpression>();

		// Token: 0x0400008D RID: 141
		private static readonly Func<CsdlSemanticsIfExpression, IEdmExpression> ComputeIfFalseFunc = (CsdlSemanticsIfExpression me) => me.ComputeIfFalse();
	}
}
