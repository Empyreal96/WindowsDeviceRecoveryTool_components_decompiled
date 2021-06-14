using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x02000023 RID: 35
	public abstract class GeometryCollection : Geometry
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00003DED File Offset: 0x00001FED
		protected GeometryCollection(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600011A RID: 282
		public abstract ReadOnlyCollection<Geometry> Geometries { get; }

		// Token: 0x0600011B RID: 283 RVA: 0x00003DF8 File Offset: 0x00001FF8
		public bool Equals(GeometryCollection other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Geometries.SequenceEqual(other.Geometries);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00003E2F File Offset: 0x0000202F
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeometryCollection);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00003E3D File Offset: 0x0000203D
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<Geometry>(base.CoordinateSystem, this.Geometries);
		}
	}
}
