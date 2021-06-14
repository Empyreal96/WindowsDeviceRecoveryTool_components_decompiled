using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x0200002E RID: 46
	public interface IEdmSchemaElement : IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000096 RID: 150
		EdmSchemaElementKind SchemaElementKind { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000097 RID: 151
		string Namespace { get; }
	}
}
