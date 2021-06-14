using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000037 RID: 55
	public interface IEdmEntityType : IEdmStructuredType, IEdmSchemaType, IEdmType, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000B0 RID: 176
		IEnumerable<IEdmStructuralProperty> DeclaredKey { get; }
	}
}
