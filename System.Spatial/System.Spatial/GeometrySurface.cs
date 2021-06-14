using System;

namespace System.Spatial
{
	// Token: 0x0200002C RID: 44
	public abstract class GeometrySurface : Geometry
	{
		// Token: 0x06000141 RID: 321 RVA: 0x0000421A File Offset: 0x0000241A
		internal GeometrySurface(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}
	}
}
