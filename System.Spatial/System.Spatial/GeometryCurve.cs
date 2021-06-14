using System;

namespace System.Spatial
{
	// Token: 0x02000024 RID: 36
	public abstract class GeometryCurve : Geometry
	{
		// Token: 0x0600011E RID: 286 RVA: 0x00003E50 File Offset: 0x00002050
		protected GeometryCurve(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}
	}
}
