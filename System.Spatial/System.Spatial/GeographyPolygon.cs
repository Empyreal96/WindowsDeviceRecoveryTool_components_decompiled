using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x02000021 RID: 33
	public abstract class GeographyPolygon : GeographySurface
	{
		// Token: 0x0600010C RID: 268 RVA: 0x00003CB2 File Offset: 0x00001EB2
		protected GeographyPolygon(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600010D RID: 269
		public abstract ReadOnlyCollection<GeographyLineString> Rings { get; }

		// Token: 0x0600010E RID: 270 RVA: 0x00003CBC File Offset: 0x00001EBC
		public bool Equals(GeographyPolygon other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Rings.SequenceEqual(other.Rings);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00003CF3 File Offset: 0x00001EF3
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeographyPolygon);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003D01 File Offset: 0x00001F01
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeographyLineString>(base.CoordinateSystem, this.Rings);
		}
	}
}
