using System;

namespace System.Security.Policy
{
	// Token: 0x02000503 RID: 1283
	[Serializable]
	internal class ApplicationTrustExtraInfo
	{
		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x0600544C RID: 21580 RVA: 0x0016047D File Offset: 0x0015E67D
		// (set) Token: 0x0600544D RID: 21581 RVA: 0x00160485 File Offset: 0x0015E685
		public bool RequestsShellIntegration
		{
			get
			{
				return this.requestsShellIntegration;
			}
			set
			{
				this.requestsShellIntegration = value;
			}
		}

		// Token: 0x0400363C RID: 13884
		private bool requestsShellIntegration = true;
	}
}
