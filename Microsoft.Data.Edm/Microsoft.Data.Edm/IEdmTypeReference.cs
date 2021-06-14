using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x0200006E RID: 110
	public interface IEdmTypeReference : IEdmElement
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060001CA RID: 458
		bool IsNullable { get; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060001CB RID: 459
		IEdmType Definition { get; }
	}
}
