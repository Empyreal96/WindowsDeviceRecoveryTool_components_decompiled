using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x02000025 RID: 37
	public abstract class GeometryLineString : GeometryCurve
	{
		// Token: 0x0600011F RID: 287 RVA: 0x00003E5A File Offset: 0x0000205A
		protected GeometryLineString(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000120 RID: 288
		public abstract ReadOnlyCollection<GeometryPoint> Points { get; }

		// Token: 0x06000121 RID: 289 RVA: 0x00003E64 File Offset: 0x00002064
		public bool Equals(GeometryLineString other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Points.SequenceEqual(other.Points);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00003E9B File Offset: 0x0000209B
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeometryLineString);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00003EA9 File Offset: 0x000020A9
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeometryPoint>(base.CoordinateSystem, this.Points);
		}
	}
}
