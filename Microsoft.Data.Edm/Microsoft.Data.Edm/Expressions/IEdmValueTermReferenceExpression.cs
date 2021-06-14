using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x02000128 RID: 296
	public interface IEdmValueTermReferenceExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060005C3 RID: 1475
		IEdmExpression Base { get; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060005C4 RID: 1476
		IEdmValueTerm Term { get; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060005C5 RID: 1477
		string Qualifier { get; }
	}
}
