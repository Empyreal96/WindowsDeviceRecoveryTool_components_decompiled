using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x02000100 RID: 256
	internal class BadPrimitiveType : BadType, IEdmPrimitiveType, IEdmSchemaType, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmType, IEdmElement
	{
		// Token: 0x060004F0 RID: 1264 RVA: 0x0000C777 File Offset: 0x0000A977
		public BadPrimitiveType(string qualifiedName, EdmPrimitiveTypeKind primitiveKind, IEnumerable<EdmError> errors) : base(errors)
		{
			this.primitiveKind = primitiveKind;
			qualifiedName = (qualifiedName ?? string.Empty);
			EdmUtil.TryGetNamespaceNameFromQualifiedName(qualifiedName, out this.namespaceName, out this.name);
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0000C7A6 File Offset: 0x0000A9A6
		public EdmPrimitiveTypeKind PrimitiveKind
		{
			get
			{
				return this.primitiveKind;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0000C7AE File Offset: 0x0000A9AE
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0000C7B6 File Offset: 0x0000A9B6
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0000C7BE File Offset: 0x0000A9BE
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Primitive;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0000C7C1 File Offset: 0x0000A9C1
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.TypeDefinition;
			}
		}

		// Token: 0x040001D3 RID: 467
		private readonly EdmPrimitiveTypeKind primitiveKind;

		// Token: 0x040001D4 RID: 468
		private readonly string name;

		// Token: 0x040001D5 RID: 469
		private readonly string namespaceName;
	}
}
