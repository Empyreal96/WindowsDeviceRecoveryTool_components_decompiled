using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000F2 RID: 242
	public interface IEdmDecimalTypeReference : IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060004C1 RID: 1217
		int? Precision { get; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060004C2 RID: 1218
		int? Scale { get; }
	}
}
