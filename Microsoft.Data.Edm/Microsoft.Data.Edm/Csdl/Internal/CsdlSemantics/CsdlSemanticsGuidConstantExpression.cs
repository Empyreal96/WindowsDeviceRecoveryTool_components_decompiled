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
	// Token: 0x02000053 RID: 83
	internal class CsdlSemanticsGuidConstantExpression : CsdlSemanticsExpression, IEdmGuidConstantExpression, IEdmExpression, IEdmGuidValue, IEdmPrimitiveValue, IEdmValue, IEdmElement, IEdmCheckable
	{
		// Token: 0x06000137 RID: 311 RVA: 0x00004706 File Offset: 0x00002906
		public CsdlSemanticsGuidConstantExpression(CsdlConstantExpression expression, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000472D File Offset: 0x0000292D
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00004735 File Offset: 0x00002935
		public Guid Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsGuidConstantExpression.ComputeValueFunc, null);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00004749 File Offset: 0x00002949
		public IEdmTypeReference Type
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000474C File Offset: 0x0000294C
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.GuidConstant;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000474F File Offset: 0x0000294F
		public EdmValueKind ValueKind
		{
			get
			{
				return this.expression.ValueKind;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000475C File Offset: 0x0000295C
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsGuidConstantExpression.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00004770 File Offset: 0x00002970
		private Guid ComputeValue()
		{
			Guid? guid;
			if (!EdmValueParser.TryParseGuid(this.expression.Value, out guid))
			{
				return Guid.Empty;
			}
			return guid.Value;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000047A0 File Offset: 0x000029A0
		private IEnumerable<EdmError> ComputeErrors()
		{
			Guid? guid;
			if (!EdmValueParser.TryParseGuid(this.expression.Value, out guid))
			{
				return new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.InvalidGuid, Strings.ValueParser_InvalidGuid(this.expression.Value))
				};
			}
			return Enumerable.Empty<EdmError>();
		}

		// Token: 0x04000078 RID: 120
		private readonly CsdlConstantExpression expression;

		// Token: 0x04000079 RID: 121
		private readonly Cache<CsdlSemanticsGuidConstantExpression, Guid> valueCache = new Cache<CsdlSemanticsGuidConstantExpression, Guid>();

		// Token: 0x0400007A RID: 122
		private static readonly Func<CsdlSemanticsGuidConstantExpression, Guid> ComputeValueFunc = (CsdlSemanticsGuidConstantExpression me) => me.ComputeValue();

		// Token: 0x0400007B RID: 123
		private readonly Cache<CsdlSemanticsGuidConstantExpression, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsGuidConstantExpression, IEnumerable<EdmError>>();

		// Token: 0x0400007C RID: 124
		private static readonly Func<CsdlSemanticsGuidConstantExpression, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsGuidConstantExpression me) => me.ComputeErrors();
	}
}
