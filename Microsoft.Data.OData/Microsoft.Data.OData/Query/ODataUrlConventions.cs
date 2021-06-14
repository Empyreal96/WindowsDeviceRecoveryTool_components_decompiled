using System;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000051 RID: 81
	public sealed class ODataUrlConventions
	{
		// Token: 0x0600021F RID: 543 RVA: 0x00008204 File Offset: 0x00006404
		private ODataUrlConventions(UrlConvention urlConvention)
		{
			this.urlConvention = urlConvention;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00008213 File Offset: 0x00006413
		public static ODataUrlConventions Default
		{
			get
			{
				return ODataUrlConventions.DefaultInstance;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000821A File Offset: 0x0000641A
		public static ODataUrlConventions KeyAsSegment
		{
			get
			{
				return ODataUrlConventions.KeyAsSegmentInstance;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00008221 File Offset: 0x00006421
		internal UrlConvention UrlConvention
		{
			get
			{
				return this.urlConvention;
			}
		}

		// Token: 0x04000084 RID: 132
		private static readonly ODataUrlConventions DefaultInstance = new ODataUrlConventions(UrlConvention.CreateWithExplicitValue(false));

		// Token: 0x04000085 RID: 133
		private static readonly ODataUrlConventions KeyAsSegmentInstance = new ODataUrlConventions(UrlConvention.CreateWithExplicitValue(true));

		// Token: 0x04000086 RID: 134
		private readonly UrlConvention urlConvention;
	}
}
