using System;

namespace System.Spatial
{
	// Token: 0x02000022 RID: 34
	public abstract class Geometry : ISpatial
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00003D14 File Offset: 0x00001F14
		protected Geometry(CoordinateSystem coordinateSystem, SpatialImplementation creator)
		{
			Util.CheckArgumentNull(coordinateSystem, "coordinateSystem");
			Util.CheckArgumentNull(creator, "creator");
			this.coordinateSystem = coordinateSystem;
			this.creator = creator;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00003D40 File Offset: 0x00001F40
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00003D48 File Offset: 0x00001F48
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000114 RID: 276
		public abstract bool IsEmpty { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00003D51 File Offset: 0x00001F51
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00003D59 File Offset: 0x00001F59
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

		// Token: 0x06000117 RID: 279 RVA: 0x00003D62 File Offset: 0x00001F62
		public virtual void SendTo(GeometryPipeline chain)
		{
			Util.CheckArgumentNull(chain, "chain");
			chain.SetCoordinateSystem(this.coordinateSystem);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00003D7C File Offset: 0x00001F7C
		internal bool? BaseEquals(Geometry other)
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

		// Token: 0x04000017 RID: 23
		private SpatialImplementation creator;

		// Token: 0x04000018 RID: 24
		private CoordinateSystem coordinateSystem;
	}
}
