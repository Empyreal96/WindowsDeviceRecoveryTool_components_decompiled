using System;
using System.Net;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x02000012 RID: 18
	public class WebClientEx : WebClient
	{
		// Token: 0x06000093 RID: 147 RVA: 0x0000357C File Offset: 0x0000177C
		public WebClientEx(int timeoutInMiliseconds = 30000)
		{
			this.timeout = timeoutInMiliseconds;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003590 File Offset: 0x00001790
		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest webRequest = base.GetWebRequest(address);
			webRequest.Timeout = this.timeout;
			return webRequest;
		}

		// Token: 0x0400005F RID: 95
		private readonly int timeout;
	}
}
