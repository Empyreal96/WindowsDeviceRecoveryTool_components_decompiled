using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x0200010B RID: 267
	public interface IEdmTemporalTypeReference : IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600051E RID: 1310
		int? Precision { get; }
	}
}
