using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Annotations
{
	// Token: 0x020000A0 RID: 160
	public interface IEdmValueAnnotation : IEdmVocabularyAnnotation, IEdmElement
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060002A2 RID: 674
		IEdmExpression Value { get; }
	}
}
