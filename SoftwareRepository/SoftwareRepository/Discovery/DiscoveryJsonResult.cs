using System;
using System.Net;

namespace SoftwareRepository.Discovery
{
	// Token: 0x0200001F RID: 31
	public class DiscoveryJsonResult
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003FF9 File Offset: 0x000021F9
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00004001 File Offset: 0x00002201
		public string Result { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000400A File Offset: 0x0000220A
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00004012 File Offset: 0x00002212
		public HttpStatusCode StatusCode { get; set; }
	}
}
