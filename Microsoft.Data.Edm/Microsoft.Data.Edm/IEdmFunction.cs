using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000DE RID: 222
	public interface IEdmFunction : IEdmFunctionBase, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000472 RID: 1138
		string DefiningExpression { get; }
	}
}
