using System;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000E3 RID: 227
	internal class AmbiguousTypeBinding : AmbiguousBinding<IEdmSchemaType>, IEdmSchemaType, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmType, IEdmElement
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0000C192 File Offset: 0x0000A392
		public AmbiguousTypeBinding(IEdmSchemaType first, IEdmSchemaType second) : base(first, second)
		{
			this.namespaceName = (first.Namespace ?? string.Empty);
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x0000C1B1 File Offset: 0x0000A3B1
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.TypeDefinition;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0000C1B4 File Offset: 0x0000A3B4
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x0000C1BC File Offset: 0x0000A3BC
		public EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.None;
			}
		}

		// Token: 0x040001B5 RID: 437
		private readonly string namespaceName;
	}
}
