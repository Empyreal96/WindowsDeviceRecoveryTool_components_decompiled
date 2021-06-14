using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000055 RID: 85
	internal class GeographyMultiPolygonImplementation : GeographyMultiPolygon
	{
		// Token: 0x06000236 RID: 566 RVA: 0x0000651E File Offset: 0x0000471E
		internal GeographyMultiPolygonImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, params GeographyPolygon[] polygons) : base(coordinateSystem, creator)
		{
			this.polygons = polygons;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000652F File Offset: 0x0000472F
		internal GeographyMultiPolygonImplementation(SpatialImplementation creator, params GeographyPolygon[] polygons) : this(CoordinateSystem.DefaultGeography, creator, polygons)
		{
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000653E File Offset: 0x0000473E
		public override bool IsEmpty
		{
			get
			{
				return this.polygons.Length == 0;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000654B File Offset: 0x0000474B
		public override ReadOnlyCollection<Geography> Geographies
		{
			get
			{
				return new ReadOnlyCollection<Geography>(this.polygons);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00006558 File Offset: 0x00004758
		public override ReadOnlyCollection<GeographyPolygon> Polygons
		{
			get
			{
				return new ReadOnlyCollection<GeographyPolygon>(this.polygons);
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00006568 File Offset: 0x00004768
		public override void SendTo(GeographyPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeography(SpatialType.MultiPolygon);
			for (int i = 0; i < this.polygons.Length; i++)
			{
				this.polygons[i].SendTo(pipeline);
			}
			pipeline.EndGeography();
		}

		// Token: 0x04000068 RID: 104
		private GeographyPolygon[] polygons;
	}
}
