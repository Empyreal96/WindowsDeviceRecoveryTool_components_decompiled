using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x0200015D RID: 349
	public sealed class CorsRule
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x00050174 File Offset: 0x0004E374
		// (set) Token: 0x0600150D RID: 5389 RVA: 0x00050199 File Offset: 0x0004E399
		public IList<string> AllowedOrigins
		{
			get
			{
				IList<string> result;
				if ((result = this.allowedOrigins) == null)
				{
					result = (this.allowedOrigins = new List<string>());
				}
				return result;
			}
			set
			{
				this.allowedOrigins = value;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x000501A4 File Offset: 0x0004E3A4
		// (set) Token: 0x0600150F RID: 5391 RVA: 0x000501C9 File Offset: 0x0004E3C9
		public IList<string> ExposedHeaders
		{
			get
			{
				IList<string> result;
				if ((result = this.exposedHeaders) == null)
				{
					result = (this.exposedHeaders = new List<string>());
				}
				return result;
			}
			set
			{
				this.exposedHeaders = value;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x000501D4 File Offset: 0x0004E3D4
		// (set) Token: 0x06001511 RID: 5393 RVA: 0x000501F9 File Offset: 0x0004E3F9
		public IList<string> AllowedHeaders
		{
			get
			{
				IList<string> result;
				if ((result = this.allowedHeaders) == null)
				{
					result = (this.allowedHeaders = new List<string>());
				}
				return result;
			}
			set
			{
				this.allowedHeaders = value;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x00050202 File Offset: 0x0004E402
		// (set) Token: 0x06001513 RID: 5395 RVA: 0x0005020A File Offset: 0x0004E40A
		public CorsHttpMethods AllowedMethods { get; set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x00050213 File Offset: 0x0004E413
		// (set) Token: 0x06001515 RID: 5397 RVA: 0x0005021B File Offset: 0x0004E41B
		public int MaxAgeInSeconds { get; set; }

		// Token: 0x0400098F RID: 2447
		private IList<string> allowedOrigins;

		// Token: 0x04000990 RID: 2448
		private IList<string> exposedHeaders;

		// Token: 0x04000991 RID: 2449
		private IList<string> allowedHeaders;
	}
}
