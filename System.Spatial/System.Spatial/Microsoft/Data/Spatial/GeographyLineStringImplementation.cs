using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000052 RID: 82
	internal class GeographyLineStringImplementation : GeographyLineString
	{
		// Token: 0x06000226 RID: 550 RVA: 0x000063A2 File Offset: 0x000045A2
		internal GeographyLineStringImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, params GeographyPoint[] points) : base(coordinateSystem, creator)
		{
			this.points = (points ?? new GeographyPoint[0]);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000063BD File Offset: 0x000045BD
		public override bool IsEmpty
		{
			get
			{
				return this.points.Length == 0;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000063CA File Offset: 0x000045CA
		public override ReadOnlyCollection<GeographyPoint> Points
		{
			get
			{
				return new ReadOnlyCollection<GeographyPoint>(this.points);
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000063D7 File Offset: 0x000045D7
		public override void SendTo(GeographyPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeography(SpatialType.LineString);
			this.SendFigure(pipeline);
			pipeline.EndGeography();
		}

		// Token: 0x04000065 RID: 101
		private GeographyPoint[] points;
	}
}
