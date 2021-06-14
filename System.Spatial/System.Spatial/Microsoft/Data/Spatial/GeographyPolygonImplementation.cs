using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000057 RID: 87
	internal class GeographyPolygonImplementation : GeographyPolygon
	{
		// Token: 0x06000244 RID: 580 RVA: 0x000066FE File Offset: 0x000048FE
		internal GeographyPolygonImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, params GeographyLineString[] rings) : base(coordinateSystem, creator)
		{
			this.rings = (rings ?? new GeographyLineString[0]);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00006719 File Offset: 0x00004919
		internal GeographyPolygonImplementation(SpatialImplementation creator, params GeographyLineString[] rings) : this(CoordinateSystem.DefaultGeography, creator, rings)
		{
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00006728 File Offset: 0x00004928
		public override bool IsEmpty
		{
			get
			{
				return this.rings.Length == 0;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00006735 File Offset: 0x00004935
		public override ReadOnlyCollection<GeographyLineString> Rings
		{
			get
			{
				return new ReadOnlyCollection<GeographyLineString>(this.rings);
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00006744 File Offset: 0x00004944
		public override void SendTo(GeographyPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeography(SpatialType.Polygon);
			for (int i = 0; i < this.rings.Length; i++)
			{
				this.rings[i].SendFigure(pipeline);
			}
			pipeline.EndGeography();
		}

		// Token: 0x0400006D RID: 109
		private GeographyLineString[] rings;
	}
}
