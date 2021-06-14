using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000098 RID: 152
	internal class CsdlSemanticsStringConstantExpression : CsdlSemanticsExpression, IEdmStringConstantExpression, IEdmExpression, IEdmStringValue, IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x06000270 RID: 624 RVA: 0x000061A9 File Offset: 0x000043A9
		public CsdlSemanticsStringConstantExpression(CsdlConstantExpression expression, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000271 RID: 625 RVA: 0x000061C5 File Offset: 0x000043C5
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000272 RID: 626 RVA: 0x000061CD File Offset: 0x000043CD
		public string Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsStringConstantExpression.ComputeValueFunc, null);
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000273 RID: 627 RVA: 0x000061E1 File Offset: 0x000043E1
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.StringConstant;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000274 RID: 628 RVA: 0x000061E5 File Offset: 0x000043E5
		public IEdmTypeReference Type
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000275 RID: 629 RVA: 0x000061E8 File Offset: 0x000043E8
		public EdmValueKind ValueKind
		{
			get
			{
				return this.expression.ValueKind;
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000061F5 File Offset: 0x000043F5
		private string ComputeValue()
		{
			return this.expression.Value;
		}

		// Token: 0x04000119 RID: 281
		private readonly CsdlConstantExpression expression;

		// Token: 0x0400011A RID: 282
		private readonly Cache<CsdlSemanticsStringConstantExpression, string> valueCache = new Cache<CsdlSemanticsStringConstantExpression, string>();

		// Token: 0x0400011B RID: 283
		private static readonly Func<CsdlSemanticsStringConstantExpression, string> ComputeValueFunc = (CsdlSemanticsStringConstantExpression me) => me.ComputeValue();
	}
}
