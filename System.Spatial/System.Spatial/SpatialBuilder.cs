using System;

namespace System.Spatial
{
	// Token: 0x02000033 RID: 51
	public class SpatialBuilder : SpatialPipeline, IShapeProvider, IGeographyProvider, IGeometryProvider
	{
		// Token: 0x0600015A RID: 346 RVA: 0x0000431B File Offset: 0x0000251B
		public SpatialBuilder(GeographyPipeline geographyInput, GeometryPipeline geometryInput, IGeographyProvider geographyOutput, IGeometryProvider geometryOutput) : base(geographyInput, geometryInput)
		{
			this.geographyOutput = geographyOutput;
			this.geometryOutput = geometryOutput;
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600015B RID: 347 RVA: 0x00004334 File Offset: 0x00002534
		// (remove) Token: 0x0600015C RID: 348 RVA: 0x00004342 File Offset: 0x00002542
		public event Action<Geography> ProduceGeography
		{
			add
			{
				this.geographyOutput.ProduceGeography += value;
			}
			remove
			{
				this.geographyOutput.ProduceGeography -= value;
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600015D RID: 349 RVA: 0x00004350 File Offset: 0x00002550
		// (remove) Token: 0x0600015E RID: 350 RVA: 0x0000435E File Offset: 0x0000255E
		public event Action<Geometry> ProduceGeometry
		{
			add
			{
				this.geometryOutput.ProduceGeometry += value;
			}
			remove
			{
				this.geometryOutput.ProduceGeometry -= value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000436C File Offset: 0x0000256C
		public Geography ConstructedGeography
		{
			get
			{
				return this.geographyOutput.ConstructedGeography;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00004379 File Offset: 0x00002579
		public Geometry ConstructedGeometry
		{
			get
			{
				return this.geometryOutput.ConstructedGeometry;
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00004386 File Offset: 0x00002586
		public static SpatialBuilder Create()
		{
			return SpatialImplementation.CurrentImplementation.CreateBuilder();
		}

		// Token: 0x0400001C RID: 28
		private readonly IGeographyProvider geographyOutput;

		// Token: 0x0400001D RID: 29
		private readonly IGeometryProvider geometryOutput;
	}
}
