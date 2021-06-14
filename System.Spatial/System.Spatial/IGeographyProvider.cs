using System;

namespace System.Spatial
{
	// Token: 0x0200002E RID: 46
	public interface IGeographyProvider
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000147 RID: 327
		// (remove) Token: 0x06000148 RID: 328
		event Action<Geography> ProduceGeography;

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000149 RID: 329
		Geography ConstructedGeography { get; }
	}
}
