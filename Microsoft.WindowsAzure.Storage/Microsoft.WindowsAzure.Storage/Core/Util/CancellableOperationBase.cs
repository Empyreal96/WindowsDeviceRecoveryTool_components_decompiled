using System;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000062 RID: 98
	internal class CancellableOperationBase
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x000329B9 File Offset: 0x00030BB9
		// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x000329C1 File Offset: 0x00030BC1
		internal object CancellationLockerObject
		{
			get
			{
				return this.cancellationLockerObject;
			}
			set
			{
				this.cancellationLockerObject = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x000329CA File Offset: 0x00030BCA
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x000329D4 File Offset: 0x00030BD4
		internal bool CancelRequested
		{
			get
			{
				return this.cancelRequested;
			}
			set
			{
				this.cancelRequested = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x000329DF File Offset: 0x00030BDF
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x000329E7 File Offset: 0x00030BE7
		internal Action CancelDelegate { get; set; }

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000329F0 File Offset: 0x00030BF0
		public void Cancel()
		{
			Action action = null;
			lock (this.cancellationLockerObject)
			{
				this.cancelRequested = true;
				if (this.CancelDelegate != null)
				{
					action = this.CancelDelegate;
					this.CancelDelegate = null;
				}
			}
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x040001D3 RID: 467
		private object cancellationLockerObject = new object();

		// Token: 0x040001D4 RID: 468
		private volatile bool cancelRequested;
	}
}
