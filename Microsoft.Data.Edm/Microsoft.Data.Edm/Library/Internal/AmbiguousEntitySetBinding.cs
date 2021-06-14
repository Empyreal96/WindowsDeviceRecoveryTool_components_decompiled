using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000DD RID: 221
	internal class AmbiguousEntitySetBinding : AmbiguousBinding<IEdmEntitySet>, IEdmEntitySet, IEdmEntityContainerElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x0600046C RID: 1132 RVA: 0x0000BF46 File Offset: 0x0000A146
		public AmbiguousEntitySetBinding(IEdmEntitySet first, IEdmEntitySet second) : base(first, second)
		{
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0000BF50 File Offset: 0x0000A150
		public IEdmEntityType ElementType
		{
			get
			{
				return new BadEntityType(string.Empty, base.Errors);
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0000BF62 File Offset: 0x0000A162
		public EdmContainerElementKind ContainerElementKind
		{
			get
			{
				return EdmContainerElementKind.EntitySet;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x0000BF68 File Offset: 0x0000A168
		public IEdmEntityContainer Container
		{
			get
			{
				IEdmEntitySet edmEntitySet = base.Bindings.FirstOrDefault<IEdmEntitySet>();
				if (edmEntitySet == null)
				{
					return null;
				}
				return edmEntitySet.Container;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0000BF8C File Offset: 0x0000A18C
		public IEnumerable<IEdmNavigationTargetMapping> NavigationTargets
		{
			get
			{
				return Enumerable.Empty<IEdmNavigationTargetMapping>();
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000BF93 File Offset: 0x0000A193
		public IEdmEntitySet FindNavigationTarget(IEdmNavigationProperty property)
		{
			return null;
		}
	}
}
