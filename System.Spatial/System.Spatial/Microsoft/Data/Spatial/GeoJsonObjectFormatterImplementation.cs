using System;
using System.Collections.Generic;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000004 RID: 4
	internal class GeoJsonObjectFormatterImplementation : GeoJsonObjectFormatter
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000023C0 File Offset: 0x000005C0
		public GeoJsonObjectFormatterImplementation(SpatialImplementation creator)
		{
			this.creator = creator;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000023D0 File Offset: 0x000005D0
		public override T Read<T>(IDictionary<string, object> source)
		{
			this.EnsureParsePipeline();
			if (typeof(Geometry).IsAssignableFrom(typeof(T)))
			{
				new GeoJsonObjectReader(this.builder).ReadGeometry(source);
				return this.builder.ConstructedGeometry as T;
			}
			new GeoJsonObjectReader(this.builder).ReadGeography(source);
			return this.builder.ConstructedGeography as T;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000244C File Offset: 0x0000064C
		public override IDictionary<string, object> Write(ISpatial value)
		{
			GeoJsonObjectWriter geoJsonObjectWriter = new GeoJsonObjectWriter();
			value.SendTo(new ForwardingSegment(geoJsonObjectWriter));
			return geoJsonObjectWriter.JsonObject;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002478 File Offset: 0x00000678
		private void EnsureParsePipeline()
		{
			if (this.parsePipeline == null)
			{
				this.builder = this.creator.CreateBuilder();
				this.parsePipeline = this.creator.CreateValidator().ChainTo(this.builder);
				return;
			}
			this.parsePipeline.GeographyPipeline.Reset();
			this.parsePipeline.GeometryPipeline.Reset();
		}

		// Token: 0x04000004 RID: 4
		private readonly SpatialImplementation creator;

		// Token: 0x04000005 RID: 5
		private SpatialBuilder builder;

		// Token: 0x04000006 RID: 6
		private SpatialPipeline parsePipeline;
	}
}
