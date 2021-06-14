using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200007D RID: 125
	internal class CsdlSemanticsIntConstantExpression : CsdlSemanticsExpression, IEdmIntegerConstantExpression, IEdmExpression, IEdmIntegerValue, IEdmPrimitiveValue, IEdmValue, IEdmElement, IEdmCheckable
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00005889 File Offset: 0x00003A89
		public CsdlSemanticsIntConstantExpression(CsdlConstantExpression expression, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060001FC RID: 508 RVA: 0x000058B0 File Offset: 0x00003AB0
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000058B8 File Offset: 0x00003AB8
		public long Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsIntConstantExpression.ComputeValueFunc, null);
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060001FE RID: 510 RVA: 0x000058CC File Offset: 0x00003ACC
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.IntegerConstant;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060001FF RID: 511 RVA: 0x000058CF File Offset: 0x00003ACF
		public EdmValueKind ValueKind
		{
			get
			{
				return this.expression.ValueKind;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000058DC File Offset: 0x00003ADC
		public IEdmTypeReference Type
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000058DF File Offset: 0x00003ADF
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsIntConstantExpression.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000058F4 File Offset: 0x00003AF4
		private long ComputeValue()
		{
			long? num;
			if (!EdmValueParser.TryParseLong(this.expression.Value, out num))
			{
				return 0L;
			}
			return num.Value;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00005920 File Offset: 0x00003B20
		private IEnumerable<EdmError> ComputeErrors()
		{
			long? num;
			if (!EdmValueParser.TryParseLong(this.expression.Value, out num))
			{
				return new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.InvalidInteger, Strings.ValueParser_InvalidInteger(this.expression.Value))
				};
			}
			return Enumerable.Empty<EdmError>();
		}

		// Token: 0x040000DF RID: 223
		private readonly CsdlConstantExpression expression;

		// Token: 0x040000E0 RID: 224
		private readonly Cache<CsdlSemanticsIntConstantExpression, long> valueCache = new Cache<CsdlSemanticsIntConstantExpression, long>();

		// Token: 0x040000E1 RID: 225
		private static readonly Func<CsdlSemanticsIntConstantExpression, long> ComputeValueFunc = (CsdlSemanticsIntConstantExpression me) => me.ComputeValue();

		// Token: 0x040000E2 RID: 226
		private readonly Cache<CsdlSemanticsIntConstantExpression, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsIntConstantExpression, IEnumerable<EdmError>>();

		// Token: 0x040000E3 RID: 227
		private static readonly Func<CsdlSemanticsIntConstantExpression, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsIntConstantExpression me) => me.ComputeErrors();
	}
}
