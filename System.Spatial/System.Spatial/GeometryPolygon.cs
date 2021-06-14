using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x0200002D RID: 45
	public abstract class GeometryPolygon : GeometrySurface
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00004224 File Offset: 0x00002424
		protected GeometryPolygon(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000143 RID: 323
		public abstract ReadOnlyCollection<GeometryLineString> Rings { get; }

		// Token: 0x06000144 RID: 324 RVA: 0x00004230 File Offset: 0x00002430
		public bool Equals(GeometryPolygon other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Rings.SequenceEqual(other.Rings);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00004267 File Offset: 0x00002467
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeometryPolygon);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00004275 File Offset: 0x00002475
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeometryLineString>(base.CoordinateSystem, this.Rings);
		}
	}
}
