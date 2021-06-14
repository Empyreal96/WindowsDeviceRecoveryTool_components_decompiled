using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000B6 RID: 182
	public interface IEdmFunctionImport : IEdmFunctionBase, IEdmEntityContainerElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000342 RID: 834
		bool IsSideEffecting { get; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000343 RID: 835
		bool IsComposable { get; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000344 RID: 836
		bool IsBindable { get; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000345 RID: 837
		IEdmExpression EntitySet { get; }
	}
}
