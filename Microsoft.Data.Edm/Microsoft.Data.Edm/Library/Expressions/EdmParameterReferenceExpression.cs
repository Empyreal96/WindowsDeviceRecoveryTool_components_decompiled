using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Library.Expressions
{
	// Token: 0x02000187 RID: 391
	public class EdmParameterReferenceExpression : EdmElement, IEdmParameterReferenceExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x0600089F RID: 2207 RVA: 0x0001806D File Offset: 0x0001626D
		public EdmParameterReferenceExpression(IEdmFunctionParameter referencedParameter)
		{
			EdmUtil.CheckArgumentNull<IEdmFunctionParameter>(referencedParameter, "referencedParameter");
			this.referencedParameter = referencedParameter;
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00018088 File Offset: 0x00016288
		public IEdmFunctionParameter ReferencedParameter
		{
			get
			{
				return this.referencedParameter;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00018090 File Offset: 0x00016290
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.ParameterReference;
			}
		}

		// Token: 0x04000445 RID: 1093
		private readonly IEdmFunctionParameter referencedParameter;
	}
}
