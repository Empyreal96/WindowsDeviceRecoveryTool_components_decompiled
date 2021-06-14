using System;

namespace System.Security.Policy
{
	// Token: 0x02000506 RID: 1286
	internal class HostContextInternal
	{
		// Token: 0x06005460 RID: 21600 RVA: 0x00160624 File Offset: 0x0015E824
		public HostContextInternal(TrustManagerContext trustManagerContext)
		{
			if (trustManagerContext == null)
			{
				this.persist = true;
				return;
			}
			this.ignorePersistedDecision = trustManagerContext.IgnorePersistedDecision;
			this.noPrompt = trustManagerContext.NoPrompt;
			this.persist = trustManagerContext.Persist;
			this.previousAppId = trustManagerContext.PreviousApplicationIdentity;
		}

		// Token: 0x17001452 RID: 5202
		// (get) Token: 0x06005461 RID: 21601 RVA: 0x00160672 File Offset: 0x0015E872
		public bool IgnorePersistedDecision
		{
			get
			{
				return this.ignorePersistedDecision;
			}
		}

		// Token: 0x17001453 RID: 5203
		// (get) Token: 0x06005462 RID: 21602 RVA: 0x0016067A File Offset: 0x0015E87A
		public bool NoPrompt
		{
			get
			{
				return this.noPrompt;
			}
		}

		// Token: 0x17001454 RID: 5204
		// (get) Token: 0x06005463 RID: 21603 RVA: 0x00160682 File Offset: 0x0015E882
		public bool Persist
		{
			get
			{
				return this.persist;
			}
		}

		// Token: 0x17001455 RID: 5205
		// (get) Token: 0x06005464 RID: 21604 RVA: 0x0016068A File Offset: 0x0015E88A
		public ApplicationIdentity PreviousAppId
		{
			get
			{
				return this.previousAppId;
			}
		}

		// Token: 0x0400364C RID: 13900
		private bool ignorePersistedDecision;

		// Token: 0x0400364D RID: 13901
		private bool noPrompt;

		// Token: 0x0400364E RID: 13902
		private bool persist;

		// Token: 0x0400364F RID: 13903
		private ApplicationIdentity previousAppId;
	}
}
