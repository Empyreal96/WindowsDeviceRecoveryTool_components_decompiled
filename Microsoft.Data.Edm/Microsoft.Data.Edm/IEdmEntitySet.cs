using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000DC RID: 220
	public interface IEdmEntitySet : IEdmEntityContainerElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000469 RID: 1129
		IEdmEntityType ElementType { get; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600046A RID: 1130
		IEnumerable<IEdmNavigationTargetMapping> NavigationTargets { get; }

		// Token: 0x0600046B RID: 1131
		IEdmEntitySet FindNavigationTarget(IEdmNavigationProperty navigationProperty);
	}
}
