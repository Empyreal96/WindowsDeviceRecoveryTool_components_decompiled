using System;

namespace System.Spatial
{
	// Token: 0x02000008 RID: 8
	public abstract class GeometryPipeline
	{
		// Token: 0x0600004E RID: 78
		public abstract void BeginGeometry(SpatialType type);

		// Token: 0x0600004F RID: 79
		public abstract void BeginFigure(GeometryPosition position);

		// Token: 0x06000050 RID: 80
		public abstract void LineTo(GeometryPosition position);

		// Token: 0x06000051 RID: 81
		public abstract void EndFigure();

		// Token: 0x06000052 RID: 82
		public abstract void EndGeometry();

		// Token: 0x06000053 RID: 83
		public abstract void SetCoordinateSystem(CoordinateSystem coordinateSystem);

		// Token: 0x06000054 RID: 84
		public abstract void Reset();
	}
}
