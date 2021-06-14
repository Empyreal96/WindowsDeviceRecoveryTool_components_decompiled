using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Tools.Connectivity;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000010 RID: 16
	public class IpDeviceFactory : Disposable
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00003DF6 File Offset: 0x00001FF6
		protected IpDeviceFactory()
		{
			this.AccessDenied = false;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003E1B File Offset: 0x0000201B
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003E23 File Offset: 0x00002023
		public bool AccessDenied { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003E2C File Offset: 0x0000202C
		public int DeviceCount
		{
			get
			{
				int count;
				lock (this.mutex)
				{
					this.Refresh();
					count = this.ipDevices.Count;
				}
				return count;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003E7C File Offset: 0x0000207C
		public IIpDevice[] Devices
		{
			get
			{
				IIpDevice[] result;
				lock (this.mutex)
				{
					this.Refresh();
					result = this.ipDevices.Values.ToArray<IpDevice>();
				}
				return result;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003ED0 File Offset: 0x000020D0
		public void Reset()
		{
			lock (this.mutex)
			{
				foreach (IIpDevice ipDevice in this.ipDevices.Values)
				{
					ipDevice.Dispose();
				}
				this.ipDevices.Clear();
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003F5C File Offset: 0x0000215C
		private void Refresh()
		{
			lock (this.mutex)
			{
				DeviceDiscoveryService deviceDiscoveryService = null;
				try
				{
					this.AccessDenied = false;
					deviceDiscoveryService = new DeviceDiscoveryService();
					deviceDiscoveryService.Start(default(Guid));
					List<DiscoveredDeviceInfo> list = deviceDiscoveryService.DevicesDiscovered();
					HashSet<Guid> hashSet = new HashSet<Guid>();
					foreach (DiscoveredDeviceInfo discoveredDeviceInfo in list)
					{
						hashSet.Add(discoveredDeviceInfo.UniqueId);
					}
					IpDevice[] array = this.ipDevices.Values.ToArray<IpDevice>();
					foreach (IpDevice ipDevice in array)
					{
						if (!hashSet.Contains(ipDevice.DeviceUniqueId))
						{
							ipDevice.Dispose();
							this.ipDevices.Remove(ipDevice.DeviceUniqueId);
						}
					}
					foreach (DiscoveredDeviceInfo discoveredDeviceInfo2 in list)
					{
						if (discoveredDeviceInfo2.Connection == DiscoveredDeviceInfo.ConnectionType.IpOverUsb)
						{
							IpDevice ipDevice2 = null;
							try
							{
								ipDevice2 = this.ipDevices[discoveredDeviceInfo2.UniqueId];
								if (!ipDevice2.DeviceCommunicator.IsIpDevice())
								{
									throw new DeviceException("IpDevice connection lost");
								}
							}
							catch
							{
								this.ipDevices.Remove(discoveredDeviceInfo2.UniqueId);
								if (ipDevice2 != null)
								{
									ipDevice2.Dispose();
									ipDevice2 = null;
								}
								ipDevice2 = null;
							}
							if (ipDevice2 == null)
							{
								IpDeviceCommunicator ipDeviceCommunicator = null;
								try
								{
									ipDeviceCommunicator = new IpDeviceCommunicator(discoveredDeviceInfo2.UniqueId);
									ipDeviceCommunicator.Connect();
									if (ipDeviceCommunicator.IsIpDevice())
									{
										ipDevice2 = new IpDevice(discoveredDeviceInfo2, ipDeviceCommunicator);
										this.ipDevices[discoveredDeviceInfo2.UniqueId] = ipDevice2;
										ipDeviceCommunicator = null;
									}
								}
								catch (AccessDeniedException)
								{
									this.AccessDenied = true;
								}
								catch
								{
								}
								finally
								{
									if (ipDeviceCommunicator != null)
									{
										ipDeviceCommunicator.Dispose();
										ipDeviceCommunicator = null;
									}
								}
							}
						}
					}
				}
				catch
				{
					this.Reset();
				}
				if (deviceDiscoveryService != null)
				{
					try
					{
						deviceDiscoveryService.Stop();
					}
					catch
					{
					}
					deviceDiscoveryService = null;
				}
			}
		}

		// Token: 0x04000048 RID: 72
		private object mutex = new object();

		// Token: 0x04000049 RID: 73
		private Dictionary<Guid, IpDevice> ipDevices = new Dictionary<Guid, IpDevice>();

		// Token: 0x0400004A RID: 74
		public static IpDeviceFactory Instance = new IpDeviceFactory();
	}
}
