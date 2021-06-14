using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000F6 RID: 246
	public interface IEdmEntityReferenceType : IEdmType, IEdmElement
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060004D1 RID: 1233
		IEdmEntityType EntityType { get; }
	}
}
