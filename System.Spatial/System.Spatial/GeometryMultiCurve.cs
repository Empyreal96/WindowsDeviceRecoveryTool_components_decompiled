using System;

namespace System.Spatial
{
	// Token: 0x02000026 RID: 38
	public abstract class GeometryMultiCurve : GeometryCollection
	{
		// Token: 0x06000124 RID: 292 RVA: 0x00003EBC File Offset: 0x000020BC
		protected GeometryMultiCurve(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}
	}
}
