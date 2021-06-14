using System;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200000C RID: 12
	internal abstract class TypeWashedPipeline
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600008E RID: 142
		public abstract bool IsGeography { get; }

		// Token: 0x0600008F RID: 143
		internal abstract void SetCoordinateSystem(int? epsgId);

		// Token: 0x06000090 RID: 144
		internal abstract void Reset();

		// Token: 0x06000091 RID: 145
		internal abstract void BeginGeo(SpatialType type);

		// Token: 0x06000092 RID: 146
		internal abstract void BeginFigure(double coordinate1, double coordinate2, double? coordinate3, double? coordinate4);

		// Token: 0x06000093 RID: 147
		internal abstract void LineTo(double coordinate1, double coordinate2, double? coordinate3, double? coordinate4);

		// Token: 0x06000094 RID: 148
		internal abstract void EndFigure();

		// Token: 0x06000095 RID: 149
		internal abstract void EndGeo();
	}
}
