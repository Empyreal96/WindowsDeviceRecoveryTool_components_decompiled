using System;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200005A RID: 90
	internal class GeographyFullGlobeImplementation : GeographyFullGlobe
	{
		// Token: 0x0600024F RID: 591 RVA: 0x000068C2 File Offset: 0x00004AC2
		internal GeographyFullGlobeImplementation(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000068CC File Offset: 0x00004ACC
		internal GeographyFullGlobeImplementation(SpatialImplementation creator) : this(CoordinateSystem.DefaultGeography, creator)
		{
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000251 RID: 593 RVA: 0x000068DA File Offset: 0x00004ADA
		public override bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000068DD File Offset: 0x00004ADD
		public override void SendTo(GeographyPipeline pipeline)
		{
			base.SendTo(pipeline);
			pipeline.BeginGeography(SpatialType.FullGlobe);
			pipeline.EndGeography();
		}
	}
}
