using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000DB RID: 219
	internal class AmbiguousEntityContainerBinding : AmbiguousBinding<IEdmEntityContainer>, IEdmEntityContainer, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x0000BF0F File Offset: 0x0000A10F
		public AmbiguousEntityContainerBinding(IEdmEntityContainer first, IEdmEntityContainer second) : base(first, second)
		{
			this.namespaceName = (first.Namespace ?? string.Empty);
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0000BF2E File Offset: 0x0000A12E
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.EntityContainer;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x0000BF31 File Offset: 0x0000A131
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000BF39 File Offset: 0x0000A139
		public IEnumerable<IEdmEntityContainerElement> Elements
		{
			get
			{
				return Enumerable.Empty<IEdmEntityContainerElement>();
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000BF40 File Offset: 0x0000A140
		public IEdmEntitySet FindEntitySet(string name)
		{
			return null;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000BF43 File Offset: 0x0000A143
		public IEnumerable<IEdmFunctionImport> FindFunctionImports(string name)
		{
			return null;
		}

		// Token: 0x040001AC RID: 428
		private readonly string namespaceName;
	}
}
