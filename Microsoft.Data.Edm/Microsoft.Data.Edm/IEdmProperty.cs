using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x0200008C RID: 140
	public interface IEdmProperty : IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000249 RID: 585
		EdmPropertyKind PropertyKind { get; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600024A RID: 586
		IEdmTypeReference Type { get; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600024B RID: 587
		IEdmStructuredType DeclaringType { get; }
	}
}
