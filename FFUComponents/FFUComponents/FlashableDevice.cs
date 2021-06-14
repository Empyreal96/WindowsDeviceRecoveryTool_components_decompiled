using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000007 RID: 7
	[Guid("CBA774B0-D968-4363-898D-D7FCDCFBDDB2")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces(typeof(IFlashableDeviceNotify))]
	[ComVisible(true)]
	public class FlashableDevice : IFlashableDevice, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000015 RID: 21 RVA: 0x00002278 File Offset: 0x00000478
		// (remove) Token: 0x06000016 RID: 22 RVA: 0x000022B0 File Offset: 0x000004B0
		public event ProgressHandler Progress;

		// Token: 0x06000017 RID: 23 RVA: 0x000022E5 File Offset: 0x000004E5
		public FlashableDevice(IFFUDevice ffuDev)
		{
			this.theDev = ffuDev;
			this.theDev.ProgressEvent += this.theDev_ProgressEvent;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000230B File Offset: 0x0000050B
		public void Dispose()
		{
			this.theDev.ProgressEvent -= this.theDev_ProgressEvent;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002324 File Offset: 0x00000524
		private void theDev_ProgressEvent(object sender, ProgressEventArgs e)
		{
			if (this.Progress != null)
			{
				this.Progress(e.Position, e.Length);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002345 File Offset: 0x00000545
		public string GetFriendlyName()
		{
			return this.theDev.DeviceFriendlyName;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002354 File Offset: 0x00000554
		public string GetUniqueIDStr()
		{
			return this.theDev.DeviceUniqueID.ToString();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000237C File Offset: 0x0000057C
		public string GetSerialNumberStr()
		{
			return this.theDev.SerialNumber.ToString();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023A4 File Offset: 0x000005A4
		public bool FlashFFU(string filePath)
		{
			bool result = true;
			try
			{
				this.theDev.FlashFFUFile(filePath);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0400000B RID: 11
		private IFFUDevice theDev;
	}
}
