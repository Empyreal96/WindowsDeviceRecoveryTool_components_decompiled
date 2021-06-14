using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000F8 RID: 248
	internal class BadEntitySet : BadElement, IEdmEntitySet, IEdmEntityContainerElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x0000C5DC File Offset: 0x0000A7DC
		public BadEntitySet(string name, IEdmEntityContainer container, IEnumerable<EdmError> errors) : base(errors)
		{
			this.name = (name ?? string.Empty);
			this.container = container;
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x0000C5FC File Offset: 0x0000A7FC
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000C604 File Offset: 0x0000A804
		public IEdmEntityType ElementType
		{
			get
			{
				return new BadEntityType(string.Empty, base.Errors);
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0000C616 File Offset: 0x0000A816
		public EdmContainerElementKind ContainerElementKind
		{
			get
			{
				return EdmContainerElementKind.EntitySet;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000C619 File Offset: 0x0000A819
		public IEdmEntityContainer Container
		{
			get
			{
				return this.container;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x0000C621 File Offset: 0x0000A821
		public IEnumerable<IEdmNavigationTargetMapping> NavigationTargets
		{
			get
			{
				return Enumerable.Empty<IEdmNavigationTargetMapping>();
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000C628 File Offset: 0x0000A828
		public IEdmEntitySet FindNavigationTarget(IEdmNavigationProperty property)
		{
			return null;
		}

		// Token: 0x040001CA RID: 458
		private readonly string name;

		// Token: 0x040001CB RID: 459
		private readonly IEdmEntityContainer container;
	}
}
