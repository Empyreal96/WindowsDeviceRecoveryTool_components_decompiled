using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000051 RID: 81
	public class UsbDevice
	{
		// Token: 0x06000281 RID: 641 RVA: 0x00007AAC File Offset: 0x00005CAC
		public UsbDevice(string portId, string vid, string pid, string locationPath, string typeDesignator, string salesName, string path, string instanceId)
		{
			this.PortId = portId;
			this.LocationPath = locationPath;
			this.Vid = vid;
			this.Pid = pid;
			this.TypeDesignator = typeDesignator;
			this.SalesName = salesName;
			this.interfaces = new List<UsbDeviceEndpoint>();
			this.Path = path;
			this.InstanceId = instanceId;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00007B12 File Offset: 0x00005D12
		public UsbDevice()
		{
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00007B20 File Offset: 0x00005D20
		// (set) Token: 0x06000284 RID: 644 RVA: 0x00007B37 File Offset: 0x00005D37
		public string PortId { get; private set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00007B40 File Offset: 0x00005D40
		// (set) Token: 0x06000286 RID: 646 RVA: 0x00007B57 File Offset: 0x00005D57
		public string LocationPath { get; private set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00007B60 File Offset: 0x00005D60
		// (set) Token: 0x06000288 RID: 648 RVA: 0x00007B77 File Offset: 0x00005D77
		public string Vid { get; private set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00007B80 File Offset: 0x00005D80
		// (set) Token: 0x0600028A RID: 650 RVA: 0x00007B97 File Offset: 0x00005D97
		public string Pid { get; private set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00007BA0 File Offset: 0x00005DA0
		// (set) Token: 0x0600028C RID: 652 RVA: 0x00007BB7 File Offset: 0x00005DB7
		public string TypeDesignator { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00007BC0 File Offset: 0x00005DC0
		// (set) Token: 0x0600028E RID: 654 RVA: 0x00007BD7 File Offset: 0x00005DD7
		public string SalesName { get; private set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00007BE0 File Offset: 0x00005DE0
		// (set) Token: 0x06000290 RID: 656 RVA: 0x00007BF7 File Offset: 0x00005DF7
		public string ProductCode { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00007C00 File Offset: 0x00005E00
		// (set) Token: 0x06000292 RID: 658 RVA: 0x00007C17 File Offset: 0x00005E17
		public string SoftwareVersion { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00007C20 File Offset: 0x00005E20
		// (set) Token: 0x06000294 RID: 660 RVA: 0x00007C37 File Offset: 0x00005E37
		public string Path { get; private set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00007C40 File Offset: 0x00005E40
		// (set) Token: 0x06000296 RID: 662 RVA: 0x00007C57 File Offset: 0x00005E57
		public string InstanceId { get; private set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00007C60 File Offset: 0x00005E60
		public ReadOnlyCollection<UsbDeviceEndpoint> Interfaces
		{
			get
			{
				return this.interfaces.AsReadOnly();
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00007C80 File Offset: 0x00005E80
		public bool SamePortAs(UsbDevice usbDevice)
		{
			return this.PortId.Equals(usbDevice.PortId, StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00007CA4 File Offset: 0x00005EA4
		public void AddInterface(string devicePath)
		{
			this.interfaces.Add(new UsbDeviceEndpoint
			{
				DevicePath = devicePath
			});
		}

		// Token: 0x0400022E RID: 558
		private readonly List<UsbDeviceEndpoint> interfaces;
	}
}
