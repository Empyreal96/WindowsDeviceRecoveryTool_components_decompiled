using System;
using System.ComponentModel;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x02000013 RID: 19
	public class WorkerHelper : IDisposable
	{
		// Token: 0x06000095 RID: 149 RVA: 0x000035B8 File Offset: 0x000017B8
		public WorkerHelper(DoWorkEventHandler workEventHandler)
		{
			this.backgroundWorker = new BackgroundWorker();
			this.backgroundWorker.DoWork += workEventHandler;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000035DC File Offset: 0x000017DC
		public bool IsBusy
		{
			get
			{
				return this.backgroundWorker.IsBusy;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000035F9 File Offset: 0x000017F9
		public void RunWorkerAsync()
		{
			this.backgroundWorker.RunWorkerAsync();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003608 File Offset: 0x00001808
		public void Dispose()
		{
			if (this.backgroundWorker != null)
			{
				this.backgroundWorker.Dispose();
			}
		}

		// Token: 0x04000060 RID: 96
		private readonly BackgroundWorker backgroundWorker;
	}
}
