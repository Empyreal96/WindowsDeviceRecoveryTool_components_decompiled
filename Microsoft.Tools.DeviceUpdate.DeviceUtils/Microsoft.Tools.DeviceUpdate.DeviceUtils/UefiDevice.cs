using System;
using FFUComponents;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000026 RID: 38
	public class UefiDevice : Disposable, IUefiDevice, IDevicePropertyCollection, IDisposable
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000F77C File Offset: 0x0000D97C
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000F784 File Offset: 0x0000D984
		public IFFUDevice FFUDevice { get; set; }

		// Token: 0x060000E7 RID: 231 RVA: 0x0000F78D File Offset: 0x0000D98D
		public UefiDevice(IFFUDevice ffuDevice)
		{
			this.FFUDevice = ffuDevice;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000F79C File Offset: 0x0000D99C
		public Guid DeviceUniqueId
		{
			get
			{
				return this.FFUDevice.DeviceUniqueID;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x0000F7AC File Offset: 0x0000D9AC
		public string UniqueID
		{
			get
			{
				return this.DeviceUniqueId.ToString().Replace("-", "");
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000F7DC File Offset: 0x0000D9DC
		public string UefiName
		{
			get
			{
				return this.FFUDevice.DeviceFriendlyName;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000F7E9 File Offset: 0x0000D9E9
		public void UpdateFFUDevice(IFFUDevice device)
		{
			if (device == null)
			{
				throw new Exception("Cannot update FFUDevice with a NULL value");
			}
			this.FFUDevice = device;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000F800 File Offset: 0x0000DA00
		public void FlashFFU(string path, bool optimize, EventHandler<ProgressEventArgs> progressEventHandler)
		{
			this.FFUDevice.ProgressEvent += progressEventHandler;
			this.FFUDevice.EndTransfer();
			this.FFUDevice.FlashFFUFile(path, optimize);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000F827 File Offset: 0x0000DA27
		public void SkipFFU()
		{
			this.FFUDevice.SkipTransfer();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000F835 File Offset: 0x0000DA35
		public void WriteWim(string wimPath, EventHandler<ProgressEventArgs> progressEventHandler)
		{
			this.FFUDevice.ProgressEvent += progressEventHandler;
			this.FFUDevice.EndTransfer();
			this.FFUDevice.WriteWim(wimPath);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000F85C File Offset: 0x0000DA5C
		public void ReadPartition(string partition, out byte[] data)
		{
			uint blockSize = 0U;
			ulong num = 0UL;
			if (!this.FFUDevice.GetDiskInfo(out blockSize, out num))
			{
				throw new DeviceException("Unable to retrieve disk size details.  Please ensure the device supports this FFU operation.");
			}
			GptDevice gptDevice = null;
			if (!GptDevice.CreateInstance(this.FFUDevice, blockSize, out gptDevice))
			{
				throw new DeviceException("Unable to parse GPT on device.  The disk may have been corrupted.");
			}
			if (!gptDevice.ReadPartition(partition, out data))
			{
				throw new DeviceException(string.Format("Error reading partition {0}.", partition));
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000F8C4 File Offset: 0x0000DAC4
		public void WritePartition(string partition, byte[] data)
		{
			uint blockSize = 0U;
			ulong num = 0UL;
			if (!this.FFUDevice.GetDiskInfo(out blockSize, out num))
			{
				throw new DeviceException("Unable to retrieve disk size details.  Please ensure that the device supports this FFU operation");
			}
			GptDevice gptDevice = null;
			if (!GptDevice.CreateInstance(this.FFUDevice, blockSize, out gptDevice))
			{
				throw new DeviceException("Unable to parse GPT on device.  The disk may have been corrupted.");
			}
			if (!gptDevice.WritePartition(partition, data))
			{
				throw new DeviceException(string.Format(" Error writing partition {0}.", partition));
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000F92C File Offset: 0x0000DB2C
		public void ClearPlatformIDOverride()
		{
			if (!this.FFUDevice.ClearIdOverride())
			{
				throw new DeviceException("Unable to clear platform ID.  Please ensure that the device supports this FFU operation.");
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000F953 File Offset: 0x0000DB53
		public virtual string GetProperty(string name)
		{
			return PropertyDeviceCollection.GetPropertyString(this, name);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000F95C File Offset: 0x0000DB5C
		protected override void DisposeManaged()
		{
			if (this.FFUDevice != null)
			{
				this.FFUDevice.Dispose();
				this.FFUDevice = null;
			}
		}
	}
}
