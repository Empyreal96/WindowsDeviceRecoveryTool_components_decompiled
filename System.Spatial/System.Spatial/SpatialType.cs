using System;

namespace System.Spatial
{
	// Token: 0x02000035 RID: 53
	public enum SpatialType : byte
	{
		// Token: 0x04000020 RID: 32
		Unknown,
		// Token: 0x04000021 RID: 33
		Point,
		// Token: 0x04000022 RID: 34
		LineString,
		// Token: 0x04000023 RID: 35
		Polygon,
		// Token: 0x04000024 RID: 36
		MultiPoint,
		// Token: 0x04000025 RID: 37
		MultiLineString,
		// Token: 0x04000026 RID: 38
		MultiPolygon,
		// Token: 0x04000027 RID: 39
		Collection,
		// Token: 0x04000028 RID: 40
		FullGlobe = 11
	}
}
