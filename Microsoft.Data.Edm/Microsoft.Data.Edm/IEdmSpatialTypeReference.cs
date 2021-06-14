using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000094 RID: 148
	public interface IEdmSpatialTypeReference : IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600026C RID: 620
		int? SpatialReferenceIdentifier { get; }
	}
}
