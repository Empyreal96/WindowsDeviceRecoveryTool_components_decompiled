using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x0200004F RID: 79
	public interface IEdmFunctionReferenceExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600012D RID: 301
		IEdmFunction ReferencedFunction { get; }
	}
}
