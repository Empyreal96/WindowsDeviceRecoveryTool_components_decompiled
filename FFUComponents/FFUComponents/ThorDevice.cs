using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Windows.Flashing.Platform;

namespace FFUComponents
{
	// Token: 0x02000051 RID: 81
	public class ThorDevice : IFFUDeviceInternal, IFFUDevice, IDisposable
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00007854 File Offset: 0x00005A54
		public string DeviceFriendlyName
		{
			get
			{
				return this.flashingDevice.GetDeviceFriendlyName();
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00007861 File Offset: 0x00005A61
		public Guid DeviceUniqueID
		{
			get
			{
				return this.flashingDevice.GetDeviceUniqueID();
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000786E File Offset: 0x00005A6E
		public Guid SerialNumber
		{
			get
			{
				return this.flashingDevice.GetDeviceSerialNumber();
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000787B File Offset: 0x00005A7B
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00007883 File Offset: 0x00005A83
		public string UsbDevicePath { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000788C File Offset: 0x00005A8C
		public string DeviceType
		{
			get
			{
				return "UFPDevice";
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600015F RID: 351 RVA: 0x00007894 File Offset: 0x00005A94
		// (remove) Token: 0x06000160 RID: 352 RVA: 0x000078CC File Offset: 0x00005ACC
		public event EventHandler<ProgressEventArgs> ProgressEvent;

		// Token: 0x06000161 RID: 353 RVA: 0x00007904 File Offset: 0x00005B04
		public ThorDevice(FlashingDevice device, string devicePath)
		{
			this.flashingDevice = device;
			this.UsbDevicePath = devicePath;
			try
			{
				USBSpeedChecker usbspeedChecker = new USBSpeedChecker(devicePath);
				this.connectionType = usbspeedChecker.GetConnectionSpeed();
			}
			catch
			{
				this.connectionType = ConnectionType.Unknown;
			}
			this.telemetryLogger = FlashingTelemetryLogger.Instance;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007960 File Offset: 0x00005B60
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000796F File Offset: 0x00005B6F
		private void Dispose(bool fDisposing)
		{
			if (fDisposing)
			{
				FFUManager.DisconnectDevice(this);
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000797A File Offset: 0x00005B7A
		public void FlashFFUFile(string ffuFilePath)
		{
			this.FlashFFUFile(ffuFilePath, false);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007984 File Offset: 0x00005B84
		public void FlashFFUFile(string ffuFilePath, bool optimizeHint)
		{
			Guid sessionId = Guid.NewGuid();
			try
			{
				this.telemetryLogger.LogFlashingInitialized(sessionId, this, optimizeHint, ffuFilePath);
				this.telemetryLogger.LogThorDeviceUSBConnectionType(sessionId, this.connectionType);
				FileInfo fileInfo = new FileInfo(ffuFilePath);
				long length = fileInfo.Length;
				ThorDevice.Progress progress = new ThorDevice.Progress(this, length);
				HandleRef cancelEvent = default(HandleRef);
				this.telemetryLogger.LogFlashingStarted(sessionId);
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				this.flashingDevice.FlashFFUFile(ffuFilePath, FlashFlags.Normal, progress, cancelEvent);
				stopwatch.Stop();
				this.telemetryLogger.LogFlashingEnded(sessionId, stopwatch, ffuFilePath, this);
			}
			catch (Exception e)
			{
				this.telemetryLogger.LogFlashingException(sessionId, e);
				throw;
			}
			this.Reboot();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007A44 File Offset: 0x00005C44
		public bool WriteWim(string wimPath)
		{
			FileInfo fileInfo = new FileInfo(wimPath);
			long length = fileInfo.Length;
			ThorDevice.Progress progress = new ThorDevice.Progress(this, length);
			this.flashingDevice.WriteWim(wimPath, progress);
			return true;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007A75 File Offset: 0x00005C75
		public bool GetDiskInfo(out uint blockSize, out ulong lastBlock)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007A7C File Offset: 0x00005C7C
		public void ReadDisk(ulong diskOffset, byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007A83 File Offset: 0x00005C83
		public void WriteDisk(ulong diskOffset, byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007A8A File Offset: 0x00005C8A
		public bool SkipTransfer()
		{
			this.flashingDevice.SkipTransfer();
			return true;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007A98 File Offset: 0x00005C98
		public bool EndTransfer()
		{
			return false;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007A9B File Offset: 0x00005C9B
		public bool Reboot()
		{
			this.flashingDevice.Reboot();
			return true;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007AA9 File Offset: 0x00005CA9
		public bool EnterMassStorage()
		{
			this.flashingDevice.EnterMassStorageMode();
			return true;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007AB7 File Offset: 0x00005CB7
		public bool ClearIdOverride()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007ABE File Offset: 0x00005CBE
		public uint SetBootMode(uint bootMode, string profileName)
		{
			this.flashingDevice.SetBootMode(bootMode, profileName);
			this.Reboot();
			return 0U;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007AD5 File Offset: 0x00005CD5
		public string GetServicingLogs(string logFolderPath)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007ADC File Offset: 0x00005CDC
		public bool NeedsTimer()
		{
			return false;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007ADF File Offset: 0x00005CDF
		public void QueryDeviceUnlockId(out byte[] unlockId, out byte[] oemId, out byte[] platformId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007AE6 File Offset: 0x00005CE6
		public void RelockDeviceUnlockId()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007AED File Offset: 0x00005CED
		public void WriteUnlockTokenFile(uint unlockTokenId, byte[] fileData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007AF4 File Offset: 0x00005CF4
		public uint[] QueryUnlockTokenFiles()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007AFB File Offset: 0x00005CFB
		public bool QueryBitlockerState()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000161 RID: 353
		private FlashingDevice flashingDevice;

		// Token: 0x04000162 RID: 354
		private FlashingTelemetryLogger telemetryLogger;

		// Token: 0x04000163 RID: 355
		private ConnectionType connectionType;

		// Token: 0x02000052 RID: 82
		private class Progress : GenericProgress
		{
			// Token: 0x06000177 RID: 375 RVA: 0x00007B02 File Offset: 0x00005D02
			public Progress(ThorDevice device, long ffuFileSize)
			{
				this.Device = device;
				this.FfuFileSize = ffuFileSize;
			}

			// Token: 0x06000178 RID: 376 RVA: 0x00007B48 File Offset: 0x00005D48
			public override void RegisterProgress(uint progress)
			{
				ProgressEventArgs args = new ProgressEventArgs(this.Device, (long)((ulong)progress * (ulong)this.FfuFileSize / 100UL), this.FfuFileSize);
				Task.Factory.StartNew(delegate()
				{
					this.Device.ProgressEvent(this.Device, args);
				});
			}

			// Token: 0x04000166 RID: 358
			private ThorDevice Device;

			// Token: 0x04000167 RID: 359
			private long FfuFileSize;
		}
	}
}
