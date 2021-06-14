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
	// Token: 0x02000056 RID: 86
	internal class CsdlSemanticsBinaryConstantExpression : CsdlSemanticsExpression, IEdmBinaryConstantExpression, IEdmExpression, IEdmBinaryValue, IEdmPrimitiveValue, IEdmValue, IEdmElement, IEdmCheckable
	{
		// Token: 0x06000144 RID: 324 RVA: 0x00004855 File Offset: 0x00002A55
		public CsdlSemanticsBinaryConstantExpression(CsdlConstantExpression expression, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000487C File Offset: 0x00002A7C
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00004884 File Offset: 0x00002A84
		public byte[] Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsBinaryConstantExpression.ComputeValueFunc, null);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00004898 File Offset: 0x00002A98
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.BinaryConstant;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000489B File Offset: 0x00002A9B
		public EdmValueKind ValueKind
		{
			get
			{
				return this.expression.ValueKind;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000048A8 File Offset: 0x00002AA8
		public IEdmTypeReference Type
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000048AB File Offset: 0x00002AAB
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsBinaryConstantExpression.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000048C0 File Offset: 0x00002AC0
		private byte[] ComputeValue()
		{
			byte[] result;
			if (!EdmValueParser.TryParseBinary(this.expression.Value, out result))
			{
				return new byte[0];
			}
			return result;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000048EC File Offset: 0x00002AEC
		private IEnumerable<EdmError> ComputeErrors()
		{
			byte[] array;
			if (!EdmValueParser.TryParseBinary(this.expression.Value, out array))
			{
				return new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.InvalidBinary, Strings.ValueParser_InvalidBinary(this.expression.Value))
				};
			}
			return Enumerable.Empty<EdmError>();
		}

		// Token: 0x0400007F RID: 127
		private readonly CsdlConstantExpression expression;

		// Token: 0x04000080 RID: 128
		private readonly Cache<CsdlSemanticsBinaryConstantExpression, byte[]> valueCache = new Cache<CsdlSemanticsBinaryConstantExpression, byte[]>();

		// Token: 0x04000081 RID: 129
		private static readonly Func<CsdlSemanticsBinaryConstantExpression, byte[]> ComputeValueFunc = (CsdlSemanticsBinaryConstantExpression me) => me.ComputeValue();

		// Token: 0x04000082 RID: 130
		private readonly Cache<CsdlSemanticsBinaryConstantExpression, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsBinaryConstantExpression, IEnumerable<EdmError>>();

		// Token: 0x04000083 RID: 131
		private static readonly Func<CsdlSemanticsBinaryConstantExpression, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsBinaryConstantExpression me) => me.ComputeErrors();
	}
}
