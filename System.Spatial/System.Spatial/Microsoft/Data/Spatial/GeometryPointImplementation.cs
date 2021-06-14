using System;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200005F RID: 95
	internal class GeometryPointImplementation : GeometryPoint
	{
		// Token: 0x0600026A RID: 618 RVA: 0x00006B0A File Offset: 0x00004D0A
		internal GeometryPointImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
			this.x = double.NaN;
			this.y = double.NaN;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00006B34 File Offset: 0x00004D34
		internal GeometryPointImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator, double x, double y, double? z, double? m) : base(coordinateSystem, creator)
		{
			if (double.IsNaN(x) || double.IsInfinity(x))
			{
				throw new ArgumentException(Strings.InvalidPointCoordinate(x, "x"));
			}
			if (double.IsNaN(y) || double.IsInfinity(y))
			{
				throw new ArgumentException(Strings.InvalidPointCoordinate(y, "y"));
			}
			this.x = x;
			this.y = y;
			this.z = z;
			this.m = m;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00006BB7 File Offset: 0x00004DB7
		public override double X
		{
			get
			{
				if (this.IsEmpty)
				{
					throw new NotSupportedException(Strings.Point_AccessCoordinateWhenEmpty);
				}
				return this.x;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00006BD2 File Offset: 0x00004DD2
		public override double Y
		{
			get
			{
				if (this.IsEmpty)
				{
					throw new NotSupportedException(Strings.Point_AccessCoordinateWhenEmpty);
				}
				return this.y;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00006BED File Offset: 0x00004DED
		public override bool IsEmpty
		{
			get
			{
				return double.IsNaN(this.x);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00006BFA File Offset: 0x00004DFA
		public override double? Z
		{
			get
			{
				return this.z;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000270 RID: 624 RVA: 0x00006C02 File Offset: 0x00004E02
		public override double? M
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00006C0C File Offset: 0x00004E0C
		public override void SendTo(GeometryPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeometry(SpatialType.Point);
			if (!this.IsEmpty)
			{
				pipeline.BeginFigure(new GeometryPosition(this.x, this.y, this.z, this.m));
				pipeline.EndFigure();
			}
			pipeline.EndGeometry();
		}

		// Token: 0x04000073 RID: 115
		private double x;

		// Token: 0x04000074 RID: 116
		private double y;

		// Token: 0x04000075 RID: 117
		private double? z;

		// Token: 0x04000076 RID: 118
		private double? m;
	}
}
