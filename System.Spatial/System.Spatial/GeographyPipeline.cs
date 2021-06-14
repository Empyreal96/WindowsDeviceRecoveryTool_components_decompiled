using System;

namespace System.Spatial
{
	// Token: 0x02000006 RID: 6
	public abstract class GeographyPipeline
	{
		// Token: 0x0600003E RID: 62
		public abstract void BeginGeography(SpatialType type);

		// Token: 0x0600003F RID: 63
		public abstract void BeginFigure(GeographyPosition position);

		// Token: 0x06000040 RID: 64
		public abstract void LineTo(GeographyPosition position);

		// Token: 0x06000041 RID: 65
		public abstract void EndFigure();

		// Token: 0x06000042 RID: 66
		public abstract void EndGeography();

		// Token: 0x06000043 RID: 67
		public abstract void SetCoordinateSystem(CoordinateSystem coordinateSystem);

		// Token: 0x06000044 RID: 68
		public abstract void Reset();
	}
}
