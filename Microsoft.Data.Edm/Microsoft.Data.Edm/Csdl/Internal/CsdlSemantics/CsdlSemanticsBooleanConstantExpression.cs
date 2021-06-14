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
	// Token: 0x0200006B RID: 107
	internal class CsdlSemanticsBooleanConstantExpression : CsdlSemanticsExpression, IEdmBooleanConstantExpression, IEdmExpression, IEdmBooleanValue, IEdmPrimitiveValue, IEdmValue, IEdmElement, IEdmCheckable
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00005269 File Offset: 0x00003469
		public CsdlSemanticsBooleanConstantExpression(CsdlConstantExpression expression, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00005290 File Offset: 0x00003490
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00005298 File Offset: 0x00003498
		public bool Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsBooleanConstantExpression.ComputeValueFunc, null);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000052AC File Offset: 0x000034AC
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.BooleanConstant;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x000052AF File Offset: 0x000034AF
		public EdmValueKind ValueKind
		{
			get
			{
				return this.expression.ValueKind;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060001BA RID: 442 RVA: 0x000052BC File Offset: 0x000034BC
		public IEdmTypeReference Type
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000052BF File Offset: 0x000034BF
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsBooleanConstantExpression.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000052D4 File Offset: 0x000034D4
		private bool ComputeValue()
		{
			bool? flag;
			return EdmValueParser.TryParseBool(this.expression.Value, out flag) && flag.Value;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00005300 File Offset: 0x00003500
		private IEnumerable<EdmError> ComputeErrors()
		{
			bool? flag;
			if (!EdmValueParser.TryParseBool(this.expression.Value, out flag))
			{
				return new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.InvalidBoolean, Strings.ValueParser_InvalidBoolean(this.expression.Value))
				};
			}
			return Enumerable.Empty<EdmError>();
		}

		// Token: 0x040000BC RID: 188
		private readonly CsdlConstantExpression expression;

		// Token: 0x040000BD RID: 189
		private readonly Cache<CsdlSemanticsBooleanConstantExpression, bool> valueCache = new Cache<CsdlSemanticsBooleanConstantExpression, bool>();

		// Token: 0x040000BE RID: 190
		private static readonly Func<CsdlSemanticsBooleanConstantExpression, bool> ComputeValueFunc = (CsdlSemanticsBooleanConstantExpression me) => me.ComputeValue();

		// Token: 0x040000BF RID: 191
		private readonly Cache<CsdlSemanticsBooleanConstantExpression, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsBooleanConstantExpression, IEnumerable<EdmError>>();

		// Token: 0x040000C0 RID: 192
		private static readonly Func<CsdlSemanticsBooleanConstantExpression, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsBooleanConstantExpression me) => me.ComputeErrors();
	}
}
