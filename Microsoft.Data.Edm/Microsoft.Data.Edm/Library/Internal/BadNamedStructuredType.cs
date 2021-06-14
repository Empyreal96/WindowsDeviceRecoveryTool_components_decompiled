using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000EC RID: 236
	internal abstract class BadNamedStructuredType : BadStructuredType, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x060004B6 RID: 1206 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
		protected BadNamedStructuredType(string qualifiedName, IEnumerable<EdmError> errors) : base(errors)
		{
			qualifiedName = (qualifiedName ?? string.Empty);
			EdmUtil.TryGetNamespaceNameFromQualifiedName(qualifiedName, out this.namespaceName, out this.name);
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0000C3F8 File Offset: 0x0000A5F8
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0000C400 File Offset: 0x0000A600
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0000C408 File Offset: 0x0000A608
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.TypeDefinition;
			}
		}

		// Token: 0x040001C1 RID: 449
		private readonly string namespaceName;

		// Token: 0x040001C2 RID: 450
		private readonly string name;
	}
}
