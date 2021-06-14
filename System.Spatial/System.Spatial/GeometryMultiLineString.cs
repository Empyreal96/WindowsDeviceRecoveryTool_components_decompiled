using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x02000027 RID: 39
	public abstract class GeometryMultiLineString : GeometryMultiCurve
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00003EC6 File Offset: 0x000020C6
		protected GeometryMultiLineString(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000126 RID: 294
		public abstract ReadOnlyCollection<GeometryLineString> LineStrings { get; }

		// Token: 0x06000127 RID: 295 RVA: 0x00003ED0 File Offset: 0x000020D0
		public bool Equals(GeometryMultiLineString other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.LineStrings.SequenceEqual(other.LineStrings);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00003F07 File Offset: 0x00002107
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeometryMultiLineString);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00003F15 File Offset: 0x00002115
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeometryLineString>(base.CoordinateSystem, this.LineStrings);
		}
	}
}
