using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000B5 RID: 181
	public interface IEdmEntityContainerElement : IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000340 RID: 832
		EdmContainerElementKind ContainerElementKind { get; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000341 RID: 833
		IEdmEntityContainer Container { get; }
	}
}
