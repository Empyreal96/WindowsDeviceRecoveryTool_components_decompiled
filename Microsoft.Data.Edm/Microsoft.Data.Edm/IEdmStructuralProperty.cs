using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x0200008D RID: 141
	public interface IEdmStructuralProperty : IEdmProperty, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600024C RID: 588
		string DefaultValueString { get; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600024D RID: 589
		EdmConcurrencyMode ConcurrencyMode { get; }
	}
}
