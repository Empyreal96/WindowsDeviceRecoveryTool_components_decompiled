using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000A2 RID: 162
	public interface IEdmValueTerm : IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060002A9 RID: 681
		IEdmTypeReference Type { get; }
	}
}
