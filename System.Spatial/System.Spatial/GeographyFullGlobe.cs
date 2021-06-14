using System;

namespace System.Spatial
{
	// Token: 0x02000017 RID: 23
	public abstract class GeographyFullGlobe : GeographySurface
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x0000381F File Offset: 0x00001A1F
		protected GeographyFullGlobe(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000382C File Offset: 0x00001A2C
		public bool Equals(GeographyFullGlobe other)
		{
			return base.BaseEquals(other) ?? true;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00003853 File Offset: 0x00001A53
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeographyFullGlobe);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00003864 File Offset: 0x00001A64
		public override int GetHashCode()
		{
			CoordinateSystem coordinateSystem = base.CoordinateSystem;
			int[] fields = new int[1];
			return Geography.ComputeHashCodeFor<int>(coordinateSystem, fields);
		}
	}
}
