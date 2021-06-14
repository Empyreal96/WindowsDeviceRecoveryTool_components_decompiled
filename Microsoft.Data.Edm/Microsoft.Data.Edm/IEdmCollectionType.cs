using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x0200006C RID: 108
	public interface IEdmCollectionType : IEdmType, IEdmElement
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060001C1 RID: 449
		IEdmTypeReference ElementType { get; }
	}
}
