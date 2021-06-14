using System;
using Microsoft.Data.Spatial;

namespace System.Spatial
{
	// Token: 0x02000042 RID: 66
	public abstract class SpatialImplementation
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001AE RID: 430 RVA: 0x000051D0 File Offset: 0x000033D0
		// (set) Token: 0x060001AF RID: 431 RVA: 0x000051D7 File Offset: 0x000033D7
		public static SpatialImplementation CurrentImplementation
		{
			get
			{
				return SpatialImplementation.spatialImplementation;
			}
			internal set
			{
				SpatialImplementation.spatialImplementation = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001B0 RID: 432
		// (set) Token: 0x060001B1 RID: 433
		public abstract SpatialOperations Operations { get; set; }

		// Token: 0x060001B2 RID: 434
		public abstract SpatialBuilder CreateBuilder();

		// Token: 0x060001B3 RID: 435
		public abstract GeoJsonObjectFormatter CreateGeoJsonObjectFormatter();

		// Token: 0x060001B4 RID: 436
		public abstract GmlFormatter CreateGmlFormatter();

		// Token: 0x060001B5 RID: 437
		public abstract WellKnownTextSqlFormatter CreateWellKnownTextSqlFormatter();

		// Token: 0x060001B6 RID: 438
		public abstract WellKnownTextSqlFormatter CreateWellKnownTextSqlFormatter(bool allowOnlyTwoDimensions);

		// Token: 0x060001B7 RID: 439
		public abstract SpatialPipeline CreateValidator();

		// Token: 0x060001B8 RID: 440 RVA: 0x000051E0 File Offset: 0x000033E0
		internal SpatialOperations VerifyAndGetNonNullOperations()
		{
			SpatialOperations operations = this.Operations;
			if (operations == null)
			{
				throw new NotImplementedException(Strings.SpatialImplementation_NoRegisteredOperations);
			}
			return operations;
		}

		// Token: 0x04000040 RID: 64
		private static SpatialImplementation spatialImplementation = new DataServicesSpatialImplementation();
	}
}
