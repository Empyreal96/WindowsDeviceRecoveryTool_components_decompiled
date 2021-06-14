using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x0200001A RID: 26
	public abstract class GeographyCollection : Geography
	{
		// Token: 0x060000EA RID: 234 RVA: 0x000038F0 File Offset: 0x00001AF0
		protected GeographyCollection(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000EB RID: 235
		public abstract ReadOnlyCollection<Geography> Geographies { get; }

		// Token: 0x060000EC RID: 236 RVA: 0x000038FC File Offset: 0x00001AFC
		public bool Equals(GeographyCollection other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				return this.Geographies.SequenceEqual(other.Geographies);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00003933 File Offset: 0x00001B33
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeographyCollection);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00003941 File Offset: 0x00001B41
		public override int GetHashCode()
		{
			return Geography.ComputeHashCodeFor<Geography>(base.CoordinateSystem, this.Geographies);
		}
	}
}
