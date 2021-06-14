using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000F9 RID: 249
	internal class BadEntityType : BadNamedStructuredType, IEdmEntityType, IEdmStructuredType, IEdmSchemaType, IEdmType, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x060004DC RID: 1244 RVA: 0x0000C62B File Offset: 0x0000A82B
		public BadEntityType(string qualifiedName, IEnumerable<EdmError> errors) : base(qualifiedName, errors)
		{
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000C635 File Offset: 0x0000A835
		public IEnumerable<IEdmStructuralProperty> DeclaredKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0000C638 File Offset: 0x0000A838
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Entity;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0000C63B File Offset: 0x0000A83B
		public EdmTermKind TermKind
		{
			get
			{
				return EdmTermKind.Type;
			}
		}
	}
}
