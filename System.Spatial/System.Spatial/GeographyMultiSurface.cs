using System;

namespace System.Spatial
{
	// Token: 0x0200001E RID: 30
	public abstract class GeographyMultiSurface : GeographyCollection
	{
		// Token: 0x060000FA RID: 250 RVA: 0x00003A24 File Offset: 0x00001C24
		protected GeographyMultiSurface(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}
	}
}
