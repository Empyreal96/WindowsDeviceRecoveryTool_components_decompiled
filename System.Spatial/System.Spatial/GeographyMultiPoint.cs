using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x0200001D RID: 29
	public abstract class GeographyMultiPoint : GeographyCollection
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x000039C0 File Offset: 0x00001BC0
		protected GeographyMultiPoint(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000F6 RID: 246
		public abstract ReadOnlyCollection<GeographyPoint> Points { get; }

		// Token: 0x060000F7 RID: 247 RVA: 0x000039CC File Offset: 0x00001BCC
		public bool Equals(GeographyMultiPoint other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Points.SequenceEqual(other.Points);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00003A03 File Offset: 0x00001C03
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeographyMultiPoint);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00003A11 File Offset: 0x00001C11
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeographyPoint>(base.CoordinateSystem, this.Points);
		}
	}
}
