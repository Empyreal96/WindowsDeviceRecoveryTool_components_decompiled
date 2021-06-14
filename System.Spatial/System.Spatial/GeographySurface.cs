using System;

namespace System.Spatial
{
	// Token: 0x02000016 RID: 22
	public abstract class GeographySurface : Geography
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00003815 File Offset: 0x00001A15
		protected GeographySurface(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}
	}
}
