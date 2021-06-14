using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000176 RID: 374
	public interface IEdmNavigationProperty : IEdmProperty, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x0600082F RID: 2095
		IEdmNavigationProperty Partner { get; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000830 RID: 2096
		EdmOnDeleteAction OnDelete { get; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000831 RID: 2097
		bool IsPrincipal { get; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000832 RID: 2098
		IEnumerable<IEdmStructuralProperty> DependentProperties { get; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000833 RID: 2099
		bool ContainsTarget { get; }
	}
}
