using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x02000019 RID: 25
	public abstract class GeographyLineString : GeographyCurve
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0000388E File Offset: 0x00001A8E
		protected GeographyLineString(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000E6 RID: 230
		public abstract ReadOnlyCollection<GeographyPoint> Points { get; }

		// Token: 0x060000E7 RID: 231 RVA: 0x00003898 File Offset: 0x00001A98
		public bool Equals(GeographyLineString other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Points.SequenceEqual(other.Points);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000038CF File Offset: 0x00001ACF
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeographyLineString);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000038DD File Offset: 0x00001ADD
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeographyPoint>(base.CoordinateSystem, this.Points);
		}
	}
}
