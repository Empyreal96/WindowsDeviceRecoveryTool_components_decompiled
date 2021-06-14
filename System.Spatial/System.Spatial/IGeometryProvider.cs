using System;

namespace System.Spatial
{
	// Token: 0x0200002F RID: 47
	public interface IGeometryProvider
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600014A RID: 330
		// (remove) Token: 0x0600014B RID: 331
		event Action<Geometry> ProduceGeometry;

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600014C RID: 332
		Geometry ConstructedGeometry { get; }
	}
}
