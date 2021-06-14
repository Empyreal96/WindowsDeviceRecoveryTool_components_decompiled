using System;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000068 RID: 104
	internal class TypeWashedToGeometryPipeline : TypeWashedPipeline
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x00007984 File Offset: 0x00005B84
		public TypeWashedToGeometryPipeline(SpatialPipeline output)
		{
			this.output = output;
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00007998 File Offset: 0x00005B98
		public override bool IsGeography
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000799C File Offset: 0x00005B9C
		internal override void SetCoordinateSystem(int? epsgId)
		{
			CoordinateSystem coordinateSystem = CoordinateSystem.Geometry(epsgId);
			this.output.SetCoordinateSystem(coordinateSystem);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000079BC File Offset: 0x00005BBC
		internal override void Reset()
		{
			this.output.Reset();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000079C9 File Offset: 0x00005BC9
		internal override void BeginGeo(SpatialType type)
		{
			this.output.BeginGeometry(type);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000079D7 File Offset: 0x00005BD7
		internal override void BeginFigure(double coordinate1, double coordinate2, double? coordinate3, double? coordinate4)
		{
			this.output.BeginFigure(new GeometryPosition(coordinate1, coordinate2, coordinate3, coordinate4));
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000079EE File Offset: 0x00005BEE
		internal override void LineTo(double coordinate1, double coordinate2, double? coordinate3, double? coordinate4)
		{
			this.output.LineTo(new GeometryPosition(coordinate1, coordinate2, coordinate3, coordinate4));
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00007A05 File Offset: 0x00005C05
		internal override void EndFigure()
		{
			this.output.EndFigure();
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00007A12 File Offset: 0x00005C12
		internal override void EndGeo()
		{
			this.output.EndGeometry();
		}

		// Token: 0x040000B6 RID: 182
		private readonly GeometryPipeline output;
	}
}
