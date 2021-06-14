using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000083 RID: 131
	internal class CsdlSemanticsParameterReferenceExpression : CsdlSemanticsExpression, IEdmParameterReferenceExpression, IEdmExpression, IEdmElement, IEdmCheckable
	{
		// Token: 0x06000216 RID: 534 RVA: 0x00005AEA File Offset: 0x00003CEA
		public CsdlSemanticsParameterReferenceExpression(CsdlParameterReferenceExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00005B0D File Offset: 0x00003D0D
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00005B15 File Offset: 0x00003D15
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.ParameterReference;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00005B19 File Offset: 0x00003D19
		public IEdmFunctionParameter ReferencedParameter
		{
			get
			{
				return this.referencedCache.GetValue(this, CsdlSemanticsParameterReferenceExpression.ComputeReferencedFunc, null);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00005B2D File Offset: 0x00003D2D
		public IEnumerable<EdmError> Errors
		{
			get
			{
				if (this.ReferencedParameter is IUnresolvedElement)
				{
					return this.ReferencedParameter.Errors();
				}
				return Enumerable.Empty<EdmError>();
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00005B4D File Offset: 0x00003D4D
		private IEdmFunctionParameter ComputeReferenced()
		{
			return new UnresolvedParameter(new UnresolvedFunction(string.Empty, Strings.Bad_UnresolvedFunction(string.Empty), base.Location), this.expression.Parameter, base.Location);
		}

		// Token: 0x040000EC RID: 236
		private readonly CsdlParameterReferenceExpression expression;

		// Token: 0x040000ED RID: 237
		private readonly IEdmEntityType bindingContext;

		// Token: 0x040000EE RID: 238
		private readonly Cache<CsdlSemanticsParameterReferenceExpression, IEdmFunctionParameter> referencedCache = new Cache<CsdlSemanticsParameterReferenceExpression, IEdmFunctionParameter>();

		// Token: 0x040000EF RID: 239
		private static readonly Func<CsdlSemanticsParameterReferenceExpression, IEdmFunctionParameter> ComputeReferencedFunc = (CsdlSemanticsParameterReferenceExpression me) => me.ComputeReferenced();
	}
}
