using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000015 RID: 21
	internal sealed class UrlConvention
	{
		// Token: 0x06000072 RID: 114 RVA: 0x0000395D File Offset: 0x00001B5D
		private UrlConvention(bool generateKeyAsSegment)
		{
			this.generateKeyAsSegment = generateKeyAsSegment;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000396C File Offset: 0x00001B6C
		internal bool GenerateKeyAsSegment
		{
			get
			{
				return this.generateKeyAsSegment;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003974 File Offset: 0x00001B74
		internal static UrlConvention CreateWithExplicitValue(bool generateKeyAsSegment)
		{
			return new UrlConvention(generateKeyAsSegment);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000397C File Offset: 0x00001B7C
		internal void AddRequiredHeaders(HeaderCollection requestHeaders)
		{
			if (this.GenerateKeyAsSegment)
			{
				requestHeaders.SetHeader("DataServiceUrlConventions", "KeyAsSegment");
			}
		}

		// Token: 0x0400001A RID: 26
		private const string ConventionTermNamespace = "Com.Microsoft.Data.Services.Conventions.V1";

		// Token: 0x0400001B RID: 27
		private const string ConventionTermName = "UrlConventions";

		// Token: 0x0400001C RID: 28
		private const string KeyAsSegmentConventionName = "KeyAsSegment";

		// Token: 0x0400001D RID: 29
		private const string UrlConventionHeaderName = "DataServiceUrlConventions";

		// Token: 0x0400001E RID: 30
		private readonly bool generateKeyAsSegment;
	}
}
