using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000108 RID: 264
	public interface IEdmStringTypeReference : IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x0600050F RID: 1295
		bool? IsFixedLength { get; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000510 RID: 1296
		bool IsUnbounded { get; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000511 RID: 1297
		int? MaxLength { get; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000512 RID: 1298
		bool? IsUnicode { get; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000513 RID: 1299
		string Collation { get; }
	}
}
