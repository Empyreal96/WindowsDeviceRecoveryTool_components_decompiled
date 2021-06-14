using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x02000082 RID: 130
	public interface IEdmParameterReferenceExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000215 RID: 533
		IEdmFunctionParameter ReferencedParameter { get; }
	}
}
