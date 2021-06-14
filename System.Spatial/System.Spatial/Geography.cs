using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Spatial
{
	// Token: 0x02000015 RID: 21
	public abstract class Geography : ISpatial
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x000036F8 File Offset: 0x000018F8
		protected Geography(CoordinateSystem coordinateSystem, SpatialImplementation creator)
		{
			Util.CheckArgumentNull(coordinateSystem, "coordinateSystem");
			Util.CheckArgumentNull(creator, "creator");
			this.coordinateSystem = coordinateSystem;
			this.creator = creator;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00003724 File Offset: 0x00001924
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x0000372C File Offset: 0x0000192C
		public CoordinateSystem CoordinateSystem
		{
			get
			{
				return this.coordinateSystem;
			}
			internal set
			{
				this.coordinateSystem = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000D8 RID: 216
		public abstract bool IsEmpty { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00003735 File Offset: 0x00001935
		// (set) Token: 0x060000DA RID: 218 RVA: 0x0000373D File Offset: 0x0000193D
		internal SpatialImplementation Creator
		{
			get
			{
				return this.creator;
			}
			set
			{
				this.creator = value;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003746 File Offset: 0x00001946
		public virtual void SendTo(GeographyPipeline chain)
		{
			Util.CheckArgumentNull(chain, "chain");
			chain.SetCoordinateSystem(this.coordinateSystem);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003778 File Offset: 0x00001978
		internal static int ComputeHashCodeFor<T>(CoordinateSystem coords, IEnumerable<T> fields)
		{
			Func<int, T, int> func = null;
			int hashCode = coords.GetHashCode();
			if (func == null)
			{
				func = ((int current, T field) => current * 397 ^ field.GetHashCode());
			}
			return fields.Aggregate(hashCode, func);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000037A4 File Offset: 0x000019A4
		internal bool? BaseEquals(Geography other)
		{
			if (other == null)
			{
				return new bool?(false);
			}
			if (object.ReferenceEquals(this, other))
			{
				return new bool?(true);
			}
			if (!this.coordinateSystem.Equals(other.coordinateSystem))
			{
				return new bool?(false);
			}
			if (this.IsEmpty || other.IsEmpty)
			{
				return new bool?(this.IsEmpty && other.IsEmpty);
			}
			return null;
		}

		// Token: 0x04000015 RID: 21
		private SpatialImplementation creator;

		// Token: 0x04000016 RID: 22
		private CoordinateSystem coordinateSystem;
	}
}
