using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000B3 RID: 179
	public interface IEdmFunctionBase : IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600032E RID: 814
		IEdmTypeReference ReturnType { get; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600032F RID: 815
		IEnumerable<IEdmFunctionParameter> Parameters { get; }

		// Token: 0x06000330 RID: 816
		IEdmFunctionParameter FindParameter(string name);
	}
}
