using System;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200000D RID: 13
	internal class TypeWashedToGeographyLatLongPipeline : TypeWashedPipeline
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00002B9A File Offset: 0x00000D9A
		public TypeWashedToGeographyLatLongPipeline(SpatialPipeline output)
		{
			this.output = output;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00002BAE File Offset: 0x00000DAE
		public override bool IsGeography
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002BB4 File Offset: 0x00000DB4
		internal override void SetCoordinateSystem(int? epsgId)
		{
			CoordinateSystem coordinateSystem = CoordinateSystem.Geography(epsgId);
			this.output.SetCoordinateSystem(coordinateSystem);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002BD4 File Offset: 0x00000DD4
		internal override void Reset()
		{
			this.output.Reset();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002BE1 File Offset: 0x00000DE1
		internal override void BeginGeo(SpatialType type)
		{
			this.output.BeginGeography(type);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002BEF File Offset: 0x00000DEF
		internal override void BeginFigure(double coordinate1, double coordinate2, double? coordinate3, double? coordinate4)
		{
			this.output.BeginFigure(new GeographyPosition(coordinate1, coordinate2, coordinate3, coordinate4));
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002C06 File Offset: 0x00000E06
		internal override void LineTo(double coordinate1, double coordinate2, double? coordinate3, double? coordinate4)
		{
			this.output.LineTo(new GeographyPosition(coordinate1, coordinate2, coordinate3, coordinate4));
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002C1D File Offset: 0x00000E1D
		internal override void EndFigure()
		{
			this.output.EndFigure();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002C2A File Offset: 0x00000E2A
		internal override void EndGeo()
		{
			this.output.EndGeography();
		}

		// Token: 0x0400000F RID: 15
		private readonly GeographyPipeline output;
	}
}
