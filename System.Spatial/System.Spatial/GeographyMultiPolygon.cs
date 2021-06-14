using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x0200001F RID: 31
	public abstract class GeographyMultiPolygon : GeographyMultiSurface
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00003A2E File Offset: 0x00001C2E
		protected GeographyMultiPolygon(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000FC RID: 252
		public abstract ReadOnlyCollection<GeographyPolygon> Polygons { get; }

		// Token: 0x060000FD RID: 253 RVA: 0x00003A38 File Offset: 0x00001C38
		public bool Equals(GeographyMultiPolygon other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Polygons.SequenceEqual(other.Polygons);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00003A6F File Offset: 0x00001C6F
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeographyMultiPolygon);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003A7D File Offset: 0x00001C7D
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeographyPolygon>(base.CoordinateSystem, this.Polygons);
		}
	}
}
