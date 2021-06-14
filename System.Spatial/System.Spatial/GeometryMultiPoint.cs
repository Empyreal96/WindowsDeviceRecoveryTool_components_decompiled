using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x02000028 RID: 40
	public abstract class GeometryMultiPoint : GeometryCollection
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00003F28 File Offset: 0x00002128
		protected GeometryMultiPoint(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600012B RID: 299
		public abstract ReadOnlyCollection<GeometryPoint> Points { get; }

		// Token: 0x0600012C RID: 300 RVA: 0x00003F34 File Offset: 0x00002134
		public bool Equals(GeometryMultiPoint other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Points.SequenceEqual(other.Points);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00003F6B File Offset: 0x0000216B
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeometryMultiPoint);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00003F79 File Offset: 0x00002179
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeometryPoint>(base.CoordinateSystem, this.Points);
		}
	}
}
