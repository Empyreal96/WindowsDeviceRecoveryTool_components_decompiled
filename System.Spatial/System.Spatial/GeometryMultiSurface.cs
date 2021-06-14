using System;

namespace System.Spatial
{
	// Token: 0x02000029 RID: 41
	public abstract class GeometryMultiSurface : GeometryCollection
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00003F8C File Offset: 0x0000218C
		internal GeometryMultiSurface(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}
	}
}
