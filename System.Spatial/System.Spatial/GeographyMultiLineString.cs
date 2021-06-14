using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x0200001C RID: 28
	public abstract class GeographyMultiLineString : GeographyMultiCurve
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x0000395E File Offset: 0x00001B5E
		protected GeographyMultiLineString(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000F1 RID: 241
		public abstract ReadOnlyCollection<GeographyLineString> LineStrings { get; }

		// Token: 0x060000F2 RID: 242 RVA: 0x00003968 File Offset: 0x00001B68
		public bool Equals(GeographyMultiLineString other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.LineStrings.SequenceEqual(other.LineStrings);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000399F File Offset: 0x00001B9F
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeographyMultiLineString);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000039AD File Offset: 0x00001BAD
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<GeographyLineString>(base.CoordinateSystem, this.LineStrings);
		}
	}
}
