using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000E7 RID: 231
	public interface IEdmBinaryTypeReference : IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060004A2 RID: 1186
		bool? IsFixedLength { get; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060004A3 RID: 1187
		bool IsUnbounded { get; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060004A4 RID: 1188
		int? MaxLength { get; }
	}
}
