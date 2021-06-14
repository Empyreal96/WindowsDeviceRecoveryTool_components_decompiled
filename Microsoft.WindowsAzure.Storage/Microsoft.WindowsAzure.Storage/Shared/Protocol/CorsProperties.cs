using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x0200015C RID: 348
	public sealed class CorsProperties
	{
		// Token: 0x06001509 RID: 5385 RVA: 0x00050150 File Offset: 0x0004E350
		public CorsProperties()
		{
			this.CorsRules = new List<CorsRule>();
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x00050163 File Offset: 0x0004E363
		// (set) Token: 0x0600150B RID: 5387 RVA: 0x0005016B File Offset: 0x0004E36B
		public IList<CorsRule> CorsRules { get; internal set; }
	}
}
