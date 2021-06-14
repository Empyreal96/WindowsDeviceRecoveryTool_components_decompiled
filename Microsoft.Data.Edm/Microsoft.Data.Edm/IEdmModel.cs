using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Annotations;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000173 RID: 371
	public interface IEdmModel : IEdmElement
	{
		// Token: 0x17000348 RID: 840
		// (get) Token: 0x0600080B RID: 2059
		IEnumerable<IEdmSchemaElement> SchemaElements { get; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x0600080C RID: 2060
		IEnumerable<IEdmVocabularyAnnotation> VocabularyAnnotations { get; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x0600080D RID: 2061
		IEnumerable<IEdmModel> ReferencedModels { get; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x0600080E RID: 2062
		IEdmDirectValueAnnotationsManager DirectValueAnnotationsManager { get; }

		// Token: 0x0600080F RID: 2063
		IEdmEntityContainer FindDeclaredEntityContainer(string name);

		// Token: 0x06000810 RID: 2064
		IEdmSchemaType FindDeclaredType(string qualifiedName);

		// Token: 0x06000811 RID: 2065
		IEnumerable<IEdmFunction> FindDeclaredFunctions(string qualifiedName);

		// Token: 0x06000812 RID: 2066
		IEdmValueTerm FindDeclaredValueTerm(string qualifiedName);

		// Token: 0x06000813 RID: 2067
		IEnumerable<IEdmVocabularyAnnotation> FindDeclaredVocabularyAnnotations(IEdmVocabularyAnnotatable element);

		// Token: 0x06000814 RID: 2068
		IEnumerable<IEdmStructuredType> FindDirectlyDerivedTypes(IEdmStructuredType baseType);
	}
}
