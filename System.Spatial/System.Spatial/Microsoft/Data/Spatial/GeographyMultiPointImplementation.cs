using System;
using System.Collections.ObjectModel;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000054 RID: 84
	internal class GeographyMultiPointImplementation : GeographyMultiPoint
	{
		// Token: 0x06000230 RID: 560 RVA: 0x0000648A File Offset: 0x0000468A
		internal GeographyMultiPointImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, params GeographyPoint[] points) : base(coordinateSystem, creator)
		{
			this.points = (points ?? new GeographyPoint[0]);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000064A5 File Offset: 0x000046A5
		internal GeographyMultiPointImplementation(SpatialImplementation creator, params GeographyPoint[] points) : this(CoordinateSystem.DefaultGeography, creator, points)
		{
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000232 RID: 562 RVA: 0x000064B4 File Offset: 0x000046B4
		public override bool IsEmpty
		{
			get
			{
				return this.points.Length == 0;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000233 RID: 563 RVA: 0x000064C1 File Offset: 0x000046C1
		public override ReadOnlyCollection<Geography> Geographies
		{
			get
			{
				return new ReadOnlyCollection<Geography>(this.points);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000234 RID: 564 RVA: 0x000064CE File Offset: 0x000046CE
		public override ReadOnlyCollection<GeographyPoint> Points
		{
			get
			{
				return new ReadOnlyCollection<GeographyPoint>(this.points);
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000064DC File Offset: 0x000046DC
		public override void SendTo(GeographyPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeography(SpatialType.MultiPoint);
			for (int i = 0; i < this.points.Length; i++)
			{
				this.points[i].SendTo(pipeline);
			}
			pipeline.EndGeography();
		}

		// Token: 0x04000067 RID: 103
		private GeographyPoint[] points;
	}
}
