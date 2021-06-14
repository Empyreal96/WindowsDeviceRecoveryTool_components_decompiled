using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000060 RID: 96
	internal class GeometryPolygonImplementation : GeometryPolygon
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00006C5E File Offset: 0x00004E5E
		internal GeometryPolygonImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, params GeometryLineString[] rings) : base(coordinateSystem, creator)
		{
			this.rings = (rings ?? new GeometryLineString[0]);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00006C79 File Offset: 0x00004E79
		internal GeometryPolygonImplementation(SpatialImplementation creator, params GeometryLineString[] rings) : this(CoordinateSystem.DefaultGeometry, creator, rings)
		{
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00006C88 File Offset: 0x00004E88
		public override bool IsEmpty
		{
			get
			{
				return this.rings.Length == 0;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00006C95 File Offset: 0x00004E95
		public override ReadOnlyCollection<GeometryLineString> Rings
		{
			get
			{
				return new ReadOnlyCollection<GeometryLineString>(this.rings);
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00006CA4 File Offset: 0x00004EA4
		public override void SendTo(GeometryPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeometry(SpatialType.Polygon);
			for (int i = 0; i < this.rings.Length; i++)
			{
				this.rings[i].SendFigure(pipeline);
			}
			pipeline.EndGeometry();
		}

		// Token: 0x04000077 RID: 119
		private GeometryLineString[] rings;
	}
}
