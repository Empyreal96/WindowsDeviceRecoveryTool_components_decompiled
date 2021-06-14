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
	// Token: 0x02000077 RID: 119
	internal class CsdlSemanticsDecimalConstantExpression : CsdlSemanticsExpression, IEdmDecimalConstantExpression, IEdmExpression, IEdmDecimalValue, IEdmPrimitiveValue, IEdmValue, IEdmElement, IEdmCheckable
	{
		// Token: 0x060001E1 RID: 481 RVA: 0x000055E5 File Offset: 0x000037E5
		public CsdlSemanticsDecimalConstantExpression(CsdlConstantExpression expression, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000560C File Offset: 0x0000380C
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00005614 File Offset: 0x00003814
		public decimal Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsDecimalConstantExpression.ComputeValueFunc, null);
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00005628 File Offset: 0x00003828
		public IEdmTypeReference Type
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000562B File Offset: 0x0000382B
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.DecimalConstant;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000562E File Offset: 0x0000382E
		public EdmValueKind ValueKind
		{
			get
			{
				return this.expression.ValueKind;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000563B File Offset: 0x0000383B
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsDecimalConstantExpression.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00005650 File Offset: 0x00003850
		private decimal ComputeValue()
		{
			decimal? num;
			if (!EdmValueParser.TryParseDecimal(this.expression.Value, out num))
			{
				return 0m;
			}
			return num.Value;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00005680 File Offset: 0x00003880
		private IEnumerable<EdmError> ComputeErrors()
		{
			decimal? num;
			if (!EdmValueParser.TryParseDecimal(this.expression.Value, out num))
			{
				return new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.InvalidDecimal, Strings.ValueParser_InvalidDecimal(this.expression.Value))
				};
			}
			return Enumerable.Empty<EdmError>();
		}

		// Token: 0x040000D1 RID: 209
		private readonly CsdlConstantExpression expression;

		// Token: 0x040000D2 RID: 210
		private readonly Cache<CsdlSemanticsDecimalConstantExpression, decimal> valueCache = new Cache<CsdlSemanticsDecimalConstantExpression, decimal>();

		// Token: 0x040000D3 RID: 211
		private static readonly Func<CsdlSemanticsDecimalConstantExpression, decimal> ComputeValueFunc = (CsdlSemanticsDecimalConstantExpression me) => me.ComputeValue();

		// Token: 0x040000D4 RID: 212
		private readonly Cache<CsdlSemanticsDecimalConstantExpression, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsDecimalConstantExpression, IEnumerable<EdmError>>();

		// Token: 0x040000D5 RID: 213
		private static readonly Func<CsdlSemanticsDecimalConstantExpression, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsDecimalConstantExpression me) => me.ComputeErrors();
	}
}
