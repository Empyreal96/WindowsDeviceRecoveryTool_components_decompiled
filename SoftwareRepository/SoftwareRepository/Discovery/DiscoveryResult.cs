using System;
using System.Net;

namespace SoftwareRepository.Discovery
{
	// Token: 0x02000021 RID: 33
	public class DiscoveryResult
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000414D File Offset: 0x0000234D
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00004155 File Offset: 0x00002355
		public SoftwarePackages Result { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000415E File Offset: 0x0000235E
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00004166 File Offset: 0x00002366
		public HttpStatusCode StatusCode { get; set; }
	}
}
