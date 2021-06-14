using System;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200000E RID: 14
	internal class TypeWashedToGeographyLongLatPipeline : TypeWashedPipeline
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00002C37 File Offset: 0x00000E37
		public TypeWashedToGeographyLongLatPipeline(SpatialPipeline output)
		{
			this.output = output;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00002C4B File Offset: 0x00000E4B
		public override bool IsGeography
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002C50 File Offset: 0x00000E50
		internal override void SetCoordinateSystem(int? epsgId)
		{
			CoordinateSystem coordinateSystem = CoordinateSystem.Geography(epsgId);
			this.output.SetCoordinateSystem(coordinateSystem);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00002C70 File Offset: 0x00000E70
		internal override void Reset()
		{
			this.output.Reset();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00002C7D File Offset: 0x00000E7D
		internal override void BeginGeo(SpatialType type)
		{
			this.output.BeginGeography(type);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00002C8B File Offset: 0x00000E8B
		internal override void BeginFigure(double coordinate1, double coordinate2, double? coordinate3, double? coordinate4)
		{
			this.output.BeginFigure(new GeographyPosition(coordinate2, coordinate1, coordinate3, coordinate4));
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00002CA2 File Offset: 0x00000EA2
		internal override void LineTo(double coordinate1, double coordinate2, double? coordinate3, double? coordinate4)
		{
			this.output.LineTo(new GeographyPosition(coordinate2, coordinate1, coordinate3, coordinate4));
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00002CB9 File Offset: 0x00000EB9
		internal override void EndFigure()
		{
			this.output.EndFigure();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00002CC6 File Offset: 0x00000EC6
		internal override void EndGeo()
		{
			this.output.EndGeography();
		}

		// Token: 0x04000010 RID: 16
		private readonly GeographyPipeline output;
	}
}
