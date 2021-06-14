using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000034 RID: 52
	public interface IEdmTerm : IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000A5 RID: 165
		EdmTermKind TermKind { get; }
	}
}
