using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000DA RID: 218
	public interface IEdmEntityContainer : IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000460 RID: 1120
		IEnumerable<IEdmEntityContainerElement> Elements { get; }

		// Token: 0x06000461 RID: 1121
		IEdmEntitySet FindEntitySet(string setName);

		// Token: 0x06000462 RID: 1122
		IEnumerable<IEdmFunctionImport> FindFunctionImports(string functionName);
	}
}
