using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x0200002A RID: 42
	public abstract class GeometryMultiPolygon : GeometryMultiSurface
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00003F96 File Offset: 0x00002196
		protected GeometryMultiPolygon(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000131 RID: 305
		public abstract ReadOnlyCollection<GeometryPolygon> Polygons { get; }

		// Token: 0x06000132 RID: 306 RVA: 0x00003FA0 File Offset: 0x000021A0
		public bool Equals(GeometryMultiPolygon other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Polygons.SequenceEqual(other.Polygons);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00003FD7 File Offset: 0x000021D7
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeometryMultiPolygon);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00003FE5 File Offset: 0x000021E5
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeometryPolygon>(base.CoordinateSystem, this.Polygons);
		}
	}
}
