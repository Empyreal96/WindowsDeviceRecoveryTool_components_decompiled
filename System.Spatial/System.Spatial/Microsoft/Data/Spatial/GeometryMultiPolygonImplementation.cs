using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200005E RID: 94
	internal class GeometryMultiPolygonImplementation : GeometryMultiPolygon
	{
		// Token: 0x06000264 RID: 612 RVA: 0x00006A7E File Offset: 0x00004C7E
		internal GeometryMultiPolygonImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, params GeometryPolygon[] polygons) : base(coordinateSystem, creator)
		{
			this.polygons = polygons;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00006A8F File Offset: 0x00004C8F
		internal GeometryMultiPolygonImplementation(SpatialImplementation creator, params GeometryPolygon[] polygons) : this(CoordinateSystem.DefaultGeometry, creator, polygons)
		{
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00006A9E File Offset: 0x00004C9E
		public override bool IsEmpty
		{
			get
			{
				return this.polygons.Length == 0;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00006AAB File Offset: 0x00004CAB
		public override ReadOnlyCollection<Geometry> Geometries
		{
			get
			{
				return new ReadOnlyCollection<Geometry>(this.polygons);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00006AB8 File Offset: 0x00004CB8
		public override ReadOnlyCollection<GeometryPolygon> Polygons
		{
			get
			{
				return new ReadOnlyCollection<GeometryPolygon>(this.polygons);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00006AC8 File Offset: 0x00004CC8
		public override void SendTo(GeometryPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeometry(SpatialType.MultiPolygon);
			for (int i = 0; i < this.polygons.Length; i++)
			{
				this.polygons[i].SendTo(pipeline);
			}
			pipeline.EndGeometry();
		}

		// Token: 0x04000072 RID: 114
		private GeometryPolygon[] polygons;
	}
}
