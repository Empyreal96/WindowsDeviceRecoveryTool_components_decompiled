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
	// Token: 0x02000047 RID: 71
	internal class CsdlSemanticsDateTimeOffsetConstantExpression : CsdlSemanticsExpression, IEdmDateTimeOffsetConstantExpression, IEdmExpression, IEdmDateTimeOffsetValue, IEdmPrimitiveValue, IEdmValue, IEdmElement, IEdmCheckable
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00003749 File Offset: 0x00001949
		public CsdlSemanticsDateTimeOffsetConstantExpression(CsdlConstantExpression expression, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00003770 File Offset: 0x00001970
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00003778 File Offset: 0x00001978
		public DateTimeOffset Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsDateTimeOffsetConstantExpression.ComputeValueFunc, null);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000378C File Offset: 0x0000198C
		public IEdmTypeReference Type
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000378F File Offset: 0x0000198F
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.DateTimeOffsetConstant;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00003792 File Offset: 0x00001992
		public EdmValueKind ValueKind
		{
			get
			{
				return this.expression.ValueKind;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000379F File Offset: 0x0000199F
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsDateTimeOffsetConstantExpression.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000037B4 File Offset: 0x000019B4
		private DateTimeOffset ComputeValue()
		{
			DateTimeOffset? dateTimeOffset;
			if (!EdmValueParser.TryParseDateTimeOffset(this.expression.Value, out dateTimeOffset))
			{
				return DateTimeOffset.MinValue;
			}
			return dateTimeOffset.Value;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000037E4 File Offset: 0x000019E4
		private IEnumerable<EdmError> ComputeErrors()
		{
			DateTimeOffset? dateTimeOffset;
			if (!EdmValueParser.TryParseDateTimeOffset(this.expression.Value, out dateTimeOffset))
			{
				return new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.InvalidDateTimeOffset, Strings.ValueParser_InvalidDateTimeOffset(this.expression.Value))
				};
			}
			return Enumerable.Empty<EdmError>();
		}

		// Token: 0x0400005E RID: 94
		private readonly CsdlConstantExpression expression;

		// Token: 0x0400005F RID: 95
		private readonly Cache<CsdlSemanticsDateTimeOffsetConstantExpression, DateTimeOffset> valueCache = new Cache<CsdlSemanticsDateTimeOffsetConstantExpression, DateTimeOffset>();

		// Token: 0x04000060 RID: 96
		private static readonly Func<CsdlSemanticsDateTimeOffsetConstantExpression, DateTimeOffset> ComputeValueFunc = (CsdlSemanticsDateTimeOffsetConstantExpression me) => me.ComputeValue();

		// Token: 0x04000061 RID: 97
		private readonly Cache<CsdlSemanticsDateTimeOffsetConstantExpression, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsDateTimeOffsetConstantExpression, IEnumerable<EdmError>>();

		// Token: 0x04000062 RID: 98
		private static readonly Func<CsdlSemanticsDateTimeOffsetConstantExpression, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsDateTimeOffsetConstantExpression me) => me.ComputeErrors();
	}
}
