using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Interop.SirepClient;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x02000002 RID: 2
	[CLSCompliant(true)]
	public class DeviceDiscoveryService
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		// (set) Token: 0x06000002 RID: 2 RVA: 0x000020D8 File Offset: 0x000002D8
		public TimeSpan Timeout { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000003 RID: 3 RVA: 0x000020E4 File Offset: 0x000002E4
		// (remove) Token: 0x06000004 RID: 4 RVA: 0x0000211C File Offset: 0x0000031C
		public event EventHandler<DiscoveredEventArgs> Discovered;

		// Token: 0x06000005 RID: 5 RVA: 0x00002151 File Offset: 0x00000351
		public DeviceDiscoveryService()
		{
			this.sirepDiscovery = new SirepDiscovery();
			this.sirepDiscovery.Discovered += this.SirepDeviceDiscovered;
			this.discoveredDeviceInfos = new Dictionary<Guid, DiscoveredDeviceInfo>();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002186 File Offset: 0x00000386
		public void Start(Guid soughtGuid = default(Guid))
		{
			this.sirepDiscovery.Start(soughtGuid);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002194 File Offset: 0x00000394
		public void Stop()
		{
			this.sirepDiscovery.Stop();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021A1 File Offset: 0x000003A1
		public List<DiscoveredDeviceInfo> DevicesDiscovered()
		{
			return new List<DiscoveredDeviceInfo>(this.discoveredDeviceInfos.Values);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021B4 File Offset: 0x000003B4
		private void SirepDeviceDiscovered(object sender, SirepDiscovery.DiscoveredEventArgs e)
		{
			DiscoveredDeviceInfo discoveredDeviceInfo = new DiscoveredDeviceInfo(e);
			if (discoveredDeviceInfo.UniqueId == Guid.Empty)
			{
				try
				{
					PhysicalAddress physicalAddress = PhysicalAddress.Parse(discoveredDeviceInfo.Name);
					byte[] addressBytes = physicalAddress.GetAddressBytes();
					byte[] array = new byte[8];
					Array.Copy(addressBytes, 0, array, 2, 6);
					discoveredDeviceInfo.UniqueId = new Guid(0, 0, 0, array);
				}
				catch (FormatException)
				{
				}
			}
			if (!this.discoveredDeviceInfos.ContainsKey(discoveredDeviceInfo.UniqueId))
			{
				this.discoveredDeviceInfos.Add(discoveredDeviceInfo.UniqueId, discoveredDeviceInfo);
				EventHandler<DiscoveredEventArgs> discovered = this.Discovered;
				if (discovered != null)
				{
					this.Discovered(this, new DiscoveredEventArgs(discoveredDeviceInfo));
				}
			}
		}

		// Token: 0x04000001 RID: 1
		private SirepDiscovery sirepDiscovery;

		// Token: 0x04000002 RID: 2
		private Dictionary<Guid, DiscoveredDeviceInfo> discoveredDeviceInfos;
	}
}
