using System;

namespace System.Spatial
{
	// Token: 0x02000032 RID: 50
	public class SpatialPipeline
	{
		// Token: 0x06000151 RID: 337 RVA: 0x000042AD File Offset: 0x000024AD
		public SpatialPipeline()
		{
			this.startingLink = this;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000042BC File Offset: 0x000024BC
		public SpatialPipeline(GeographyPipeline geographyPipeline, GeometryPipeline geometryPipeline)
		{
			this.geographyPipeline = geographyPipeline;
			this.geometryPipeline = geometryPipeline;
			this.startingLink = this;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000042D9 File Offset: 0x000024D9
		public virtual GeographyPipeline GeographyPipeline
		{
			get
			{
				return this.geographyPipeline;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000042E1 File Offset: 0x000024E1
		public virtual GeometryPipeline GeometryPipeline
		{
			get
			{
				return this.geometryPipeline;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000042E9 File Offset: 0x000024E9
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000042F1 File Offset: 0x000024F1
		public SpatialPipeline StartingLink
		{
			get
			{
				return this.startingLink;
			}
			set
			{
				this.startingLink = value;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000042FA File Offset: 0x000024FA
		public static implicit operator GeographyPipeline(SpatialPipeline spatialPipeline)
		{
			if (spatialPipeline != null)
			{
				return spatialPipeline.GeographyPipeline;
			}
			return null;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00004307 File Offset: 0x00002507
		public static implicit operator GeometryPipeline(SpatialPipeline spatialPipeline)
		{
			if (spatialPipeline != null)
			{
				return spatialPipeline.GeometryPipeline;
			}
			return null;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00004314 File Offset: 0x00002514
		public virtual SpatialPipeline ChainTo(SpatialPipeline destination)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000019 RID: 25
		private GeographyPipeline geographyPipeline;

		// Token: 0x0400001A RID: 26
		private GeometryPipeline geometryPipeline;

		// Token: 0x0400001B RID: 27
		private SpatialPipeline startingLink;
	}
}
