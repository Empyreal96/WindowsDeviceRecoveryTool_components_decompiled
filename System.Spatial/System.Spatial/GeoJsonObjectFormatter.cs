using System;
using System.Collections.Generic;

namespace System.Spatial
{
	// Token: 0x02000003 RID: 3
	public abstract class GeoJsonObjectFormatter
	{
		// Token: 0x06000027 RID: 39 RVA: 0x000023AC File Offset: 0x000005AC
		public static GeoJsonObjectFormatter Create()
		{
			return SpatialImplementation.CurrentImplementation.CreateGeoJsonObjectFormatter();
		}

		// Token: 0x06000028 RID: 40
		public abstract T Read<T>(IDictionary<string, object> source) where T : class, ISpatial;

		// Token: 0x06000029 RID: 41
		public abstract IDictionary<string, object> Write(ISpatial value);
	}
}
