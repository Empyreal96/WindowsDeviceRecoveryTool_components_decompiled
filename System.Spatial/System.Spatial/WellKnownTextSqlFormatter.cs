using System;
using System.IO;

namespace System.Spatial
{
	// Token: 0x0200003B RID: 59
	public abstract class WellKnownTextSqlFormatter : SpatialFormatter<TextReader, TextWriter>
	{
		// Token: 0x06000181 RID: 385 RVA: 0x000047ED File Offset: 0x000029ED
		protected WellKnownTextSqlFormatter(SpatialImplementation creator) : base(creator)
		{
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000047F6 File Offset: 0x000029F6
		public static WellKnownTextSqlFormatter Create()
		{
			return SpatialImplementation.CurrentImplementation.CreateWellKnownTextSqlFormatter();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00004802 File Offset: 0x00002A02
		public static WellKnownTextSqlFormatter Create(bool allowOnlyTwoDimensions)
		{
			return SpatialImplementation.CurrentImplementation.CreateWellKnownTextSqlFormatter(allowOnlyTwoDimensions);
		}
	}
}
