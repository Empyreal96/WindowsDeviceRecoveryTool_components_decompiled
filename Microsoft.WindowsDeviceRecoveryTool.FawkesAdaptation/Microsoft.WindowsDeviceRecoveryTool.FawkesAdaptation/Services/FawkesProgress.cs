using System;
using ClickerUtilityLibrary;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.FawkesAdaptation.Services
{
	// Token: 0x0200000A RID: 10
	internal class FawkesProgress : Progress<FawkesProgressData>
	{
		// Token: 0x0600005A RID: 90 RVA: 0x0000361B File Offset: 0x0000181B
		public FawkesProgress(Action<FawkesProgressData> progressHandler) : base(progressHandler)
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003624 File Offset: 0x00001824
		internal void SetupUpdaterEvents(ClickerFwUpdater updaterInstance)
		{
			if (this.updater != null)
			{
				throw new InvalidOperationException("This instance was setup already");
			}
			this.updater = updaterInstance;
			this.updater.UpdaterEvent += this.UpdaterOnUpdaterEvent;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003657 File Offset: 0x00001857
		internal void CleanUpdaterEvents()
		{
			if (this.updater != null)
			{
				this.updater.UpdaterEvent -= this.UpdaterOnUpdaterEvent;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003678 File Offset: 0x00001878
		private void UpdaterOnUpdaterEvent(object sender, FwUpdaterEventArgs fwUpdaterEventArgs)
		{
			Tracer<FawkesProgress>.WriteInformation("Fawkes firmware update event received: {0}, params: {1}", new object[]
			{
				fwUpdaterEventArgs.Type,
				fwUpdaterEventArgs.Parameters
			});
			switch (fwUpdaterEventArgs.Type)
			{
			case FwUpdaterEventArgs.EventType.UpdateCompleted:
				this.ProcessUpdateFinishedEvent(fwUpdaterEventArgs.Parameters);
				return;
			case FwUpdaterEventArgs.EventType.UpdateProgress:
				this.ProcessUpdateProgressEvent(fwUpdaterEventArgs.Parameters);
				return;
			case FwUpdaterEventArgs.EventType.DeviceDisconnected:
				this.ProcessDeviceDisconnectedEvent(fwUpdaterEventArgs.Parameters);
				return;
			case FwUpdaterEventArgs.EventType.ConnectedToApplication:
				this.ProcessDeviceConnectedToApplicationEvent(fwUpdaterEventArgs.Parameters);
				return;
			case FwUpdaterEventArgs.EventType.ConnectedToBootLoader:
				this.ProcessDeviceConnectedToBootLoaderEvent(fwUpdaterEventArgs.Parameters);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003710 File Offset: 0x00001910
		private void ProcessDeviceDisconnectedEvent(object parameters)
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003712 File Offset: 0x00001912
		private void ProcessDeviceConnectedToApplicationEvent(object parameters)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003714 File Offset: 0x00001914
		private void ProcessDeviceConnectedToBootLoaderEvent(object parameters)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003716 File Offset: 0x00001916
		private void ProcessUpdateFinishedEvent(object parameters)
		{
			this.OnReport(new FawkesProgressData(new double?(100.0), "WaitUntilPhoneTurnsOn"));
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003738 File Offset: 0x00001938
		private void ProcessUpdateProgressEvent(object parameters)
		{
			double value = -1.0;
			if (parameters is double)
			{
				value = (double)parameters * 100.0;
			}
			this.OnReport(new FawkesProgressData(new double?(value), "FlashingMessageInstallingSoftware"));
		}

		// Token: 0x0400001F RID: 31
		private ClickerFwUpdater updater;
	}
}
