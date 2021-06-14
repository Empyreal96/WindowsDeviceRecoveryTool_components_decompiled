using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using FFUComponents;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000027 RID: 39
	public class UefiDeviceFactory : Disposable
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x0000F978 File Offset: 0x0000DB78
		protected UefiDeviceFactory()
		{
			FFUManager.Start();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000F9BE File Offset: 0x0000DBBE
		protected override void DisposeManaged()
		{
			this.Reset();
			FFUManager.Stop();
			base.DisposeManaged();
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
		public int DeviceCount
		{
			get
			{
				int count;
				lock (this.mutex)
				{
					this.Refresh();
					count = this.ffuDevices.Count;
				}
				return count;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x0000FA24 File Offset: 0x0000DC24
		public IUefiDevice[] Devices
		{
			get
			{
				IUefiDevice[] result;
				lock (this.mutex)
				{
					this.Refresh();
					result = this.ffuDevices.Values.ToArray<UefiDevice>();
				}
				return result;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000FA78 File Offset: 0x0000DC78
		public void Reset()
		{
			lock (this.mutex)
			{
				foreach (UefiDevice uefiDevice in this.ffuDevices.Values)
				{
					uefiDevice.Dispose();
				}
				this.ffuDevices.Clear();
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000FB04 File Offset: 0x0000DD04
		public void Refresh()
		{
			lock (this.mutex)
			{
				DateTime now = DateTime.Now;
				if (now.Subtract(UefiDeviceFactory.refreshIntervalSeconds).CompareTo(this.lastRefresh) >= 0)
				{
					this.lastRefresh = now;
					ICollection<IFFUDevice> collection = new List<IFFUDevice>();
					FFUManager.GetFlashableDevices(ref collection);
					HashSet<Guid> hashSet = new HashSet<Guid>();
					foreach (IFFUDevice iffudevice in collection)
					{
						hashSet.Add(iffudevice.DeviceUniqueID);
						if (!this.ffuDevices.ContainsKey(iffudevice.DeviceUniqueID))
						{
							this.ffuDevices[iffudevice.DeviceUniqueID] = new UefiDevice(iffudevice);
						}
					}
					Guid[] array = this.ffuDevices.Keys.ToArray<Guid>();
					foreach (Guid guid in array)
					{
						if (!hashSet.Contains(guid))
						{
							this.ffuDevices[guid].Dispose();
							this.ffuDevices.Remove(guid);
						}
					}
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000FC6C File Offset: 0x0000DE6C
		public uint GetDeviceError()
		{
			uint result;
			lock (this.mutex)
			{
				Guid winUSBClassGuid = FFUManager.WinUSBClassGuid;
				IntPtr intPtr = NativeMethods.SetupDiGetClassDevs(ref winUSBClassGuid, null, IntPtr.Zero, 2);
				if (IntPtr.Zero == intPtr)
				{
					result = 0U;
				}
				else
				{
					NativeMethods.DeviceInformationData deviceInformationData = new NativeMethods.DeviceInformationData
					{
						Size = Marshal.SizeOf(typeof(NativeMethods.DeviceInformationData)),
						ClassGuid = winUSBClassGuid,
						DevInst = 0U,
						Reserved = IntPtr.Zero
					};
					try
					{
						uint num = 0U;
						while (NativeMethods.SetupDiEnumDeviceInfo(intPtr, num++, ref deviceInformationData))
						{
							num += 1U;
							uint num2 = 0U;
							uint result2 = 0U;
							if (NativeMethods.CM_Get_DevNode_Status(ref num2, ref result2, deviceInformationData.DevInst, 0U) == 0U)
							{
								return result2;
							}
						}
						result = 0U;
					}
					finally
					{
						NativeMethods.SetupDiDestroyDeviceInfoList(intPtr);
					}
				}
			}
			return result;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000FD64 File Offset: 0x0000DF64
		public bool UpdateDevice(IFFUDevice device)
		{
			bool result;
			lock (this.mutex)
			{
				if (device == null)
				{
					result = false;
				}
				else if (!this.ffuDevices.ContainsKey(device.DeviceUniqueID))
				{
					result = false;
				}
				else
				{
					UefiDevice uefiDevice = this.ffuDevices[device.DeviceUniqueID];
					if (object.ReferenceEquals(device, uefiDevice.FFUDevice))
					{
						result = false;
					}
					else
					{
						uefiDevice.FFUDevice.Dispose();
						uefiDevice.FFUDevice = device;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0400031A RID: 794
		private object mutex = new object();

		// Token: 0x0400031B RID: 795
		private Dictionary<Guid, UefiDevice> ffuDevices = new Dictionary<Guid, UefiDevice>();

		// Token: 0x0400031C RID: 796
		private static readonly TimeSpan refreshIntervalSeconds = TimeSpan.FromSeconds(2.0);

		// Token: 0x0400031D RID: 797
		private DateTime lastRefresh = DateTime.Now.Subtract(UefiDeviceFactory.refreshIntervalSeconds);

		// Token: 0x0400031E RID: 798
		public static UefiDeviceFactory Instance = new UefiDeviceFactory();
	}
}
