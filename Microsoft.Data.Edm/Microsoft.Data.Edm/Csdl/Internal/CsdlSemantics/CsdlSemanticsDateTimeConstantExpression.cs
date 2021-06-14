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
	// Token: 0x02000074 RID: 116
	internal class CsdlSemanticsDateTimeConstantExpression : CsdlSemanticsExpression, IEdmDateTimeConstantExpression, IEdmExpression, IEdmDateTimeValue, IEdmPrimitiveValue, IEdmValue, IEdmElement, IEdmCheckable
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x00005496 File Offset: 0x00003696
		public CsdlSemanticsDateTimeConstantExpression(CsdlConstantExpression expression, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x000054BD File Offset: 0x000036BD
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x000054C5 File Offset: 0x000036C5
		public DateTime Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsDateTimeConstantExpression.ComputeValueFunc, null);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x000054D9 File Offset: 0x000036D9
		public IEdmTypeReference Type
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x000054DC File Offset: 0x000036DC
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.DateTimeConstant;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x000054DF File Offset: 0x000036DF
		public EdmValueKind ValueKind
		{
			get
			{
				return this.expression.ValueKind;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060001DA RID: 474 RVA: 0x000054EC File Offset: 0x000036EC
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsDateTimeConstantExpression.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00005500 File Offset: 0x00003700
		private DateTime ComputeValue()
		{
			DateTime? dateTime;
			if (!EdmValueParser.TryParseDateTime(this.expression.Value, out dateTime))
			{
				return DateTime.MinValue;
			}
			return dateTime.Value;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00005530 File Offset: 0x00003730
		private IEnumerable<EdmError> ComputeErrors()
		{
			DateTime? dateTime;
			if (!EdmValueParser.TryParseDateTime(this.expression.Value, out dateTime))
			{
				return new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.InvalidDateTime, Strings.ValueParser_InvalidDateTime(this.expression.Value))
				};
			}
			return Enumerable.Empty<EdmError>();
		}

		// Token: 0x040000CA RID: 202
		private readonly CsdlConstantExpression expression;

		// Token: 0x040000CB RID: 203
		private readonly Cache<CsdlSemanticsDateTimeConstantExpression, DateTime> valueCache = new Cache<CsdlSemanticsDateTimeConstantExpression, DateTime>();

		// Token: 0x040000CC RID: 204
		private static readonly Func<CsdlSemanticsDateTimeConstantExpression, DateTime> ComputeValueFunc = (CsdlSemanticsDateTimeConstantExpression me) => me.ComputeValue();

		// Token: 0x040000CD RID: 205
		private readonly Cache<CsdlSemanticsDateTimeConstantExpression, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsDateTimeConstantExpression, IEnumerable<EdmError>>();

		// Token: 0x040000CE RID: 206
		private static readonly Func<CsdlSemanticsDateTimeConstantExpression, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsDateTimeConstantExpression me) => me.ComputeErrors();
	}
}
