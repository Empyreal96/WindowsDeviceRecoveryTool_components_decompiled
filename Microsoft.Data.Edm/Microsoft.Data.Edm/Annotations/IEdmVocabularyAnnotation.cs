using System;

namespace Microsoft.Data.Edm.Annotations
{
	// Token: 0x0200009C RID: 156
	public interface IEdmVocabularyAnnotation : IEdmElement
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000286 RID: 646
		string Qualifier { get; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000287 RID: 647
		IEdmTerm Term { get; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000288 RID: 648
		IEdmVocabularyAnnotatable Target { get; }
	}
}
