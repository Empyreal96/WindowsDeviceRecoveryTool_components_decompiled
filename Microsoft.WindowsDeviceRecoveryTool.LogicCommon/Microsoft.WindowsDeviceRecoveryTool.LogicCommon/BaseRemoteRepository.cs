using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon
{
	// Token: 0x02000004 RID: 4
	public abstract class BaseRemoteRepository
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000025E3 File Offset: 0x000007E3
		protected BaseRemoteRepository()
		{
			this.SpeedCalculator = new SpeedCalculator();
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000032 RID: 50 RVA: 0x000025FC File Offset: 0x000007FC
		// (remove) Token: 0x06000033 RID: 51 RVA: 0x00002638 File Offset: 0x00000838
		public event Action<ProgressChangedEventArgs> ProgressChanged;

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002674 File Offset: 0x00000874
		// (set) Token: 0x06000035 RID: 53 RVA: 0x0000268B File Offset: 0x0000088B
		private protected SpeedCalculator SpeedCalculator { protected get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002694 File Offset: 0x00000894
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000026AB File Offset: 0x000008AB
		protected long TotalFilesSize { get; set; }

		// Token: 0x06000038 RID: 56 RVA: 0x000026B4 File Offset: 0x000008B4
		public void SetProxy(IWebProxy settings)
		{
			this.proxySettings = settings;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000026C0 File Offset: 0x000008C0
		protected void RaiseProgressChangedEvent(int percentage, string message = null)
		{
			if (!string.IsNullOrEmpty(message))
			{
				this.lastProgressMessage = message;
			}
			this.lastProgressPercentage = percentage;
			this.RaiseProgressChangedEvent();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000026F0 File Offset: 0x000008F0
		protected void RaiseProgressChangedEvent()
		{
			this.RaiseProgressChangedEvent(new ProgressChangedEventArgs(this.lastProgressPercentage, this.lastProgressMessage, this.SpeedCalculator.TotalDownloadedSize, this.TotalFilesSize, this.SpeedCalculator.BytesPerSecond, this.SpeedCalculator.RemaingSeconds));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002740 File Offset: 0x00000940
		protected void RaiseProgressChangedEvent(ProgressChangedEventArgs progressChangedEventArgs)
		{
			Action<ProgressChangedEventArgs> progressChanged = this.ProgressChanged;
			if (progressChanged != null)
			{
				progressChanged(progressChangedEventArgs);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002768 File Offset: 0x00000968
		protected IWebProxy Proxy()
		{
			return this.proxySettings ?? WebRequest.GetSystemWebProxy();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002789 File Offset: 0x00000989
		public virtual Task<PackageFileInfo> CheckLatestPackage(QueryParameters queryParameters, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002791 File Offset: 0x00000991
		public virtual List<string> DownloadLatestPackage(QueryParameters queryParameters, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002799 File Offset: 0x00000999
		public virtual List<string> DownloadLatestPackage(DownloadParameters downloadParameters, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400000A RID: 10
		protected const long ErrorHandleDiskFull = 39L;

		// Token: 0x0400000B RID: 11
		protected const long ErrorDiskFull = 112L;

		// Token: 0x0400000C RID: 12
		private int lastProgressPercentage;

		// Token: 0x0400000D RID: 13
		private string lastProgressMessage;

		// Token: 0x0400000E RID: 14
		private IWebProxy proxySettings;
	}
}
