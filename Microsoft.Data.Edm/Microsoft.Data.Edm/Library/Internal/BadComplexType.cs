using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000EE RID: 238
	internal class BadComplexType : BadNamedStructuredType, IEdmComplexType, IEdmStructuredType, IEdmSchemaType, IEdmType, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x0000C40B File Offset: 0x0000A60B
		public BadComplexType(string qualifiedName, IEnumerable<EdmError> errors) : base(qualifiedName, errors)
		{
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0000C415 File Offset: 0x0000A615
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Complex;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0000C418 File Offset: 0x0000A618
		public EdmTermKind TermKind
		{
			get
			{
				return EdmTermKind.Type;
			}
		}
	}
}
