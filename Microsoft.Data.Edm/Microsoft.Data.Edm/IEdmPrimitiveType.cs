using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000FF RID: 255
	public interface IEdmPrimitiveType : IEdmSchemaType, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmType, IEdmElement
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060004EF RID: 1263
		EdmPrimitiveTypeKind PrimitiveKind { get; }
	}
}
