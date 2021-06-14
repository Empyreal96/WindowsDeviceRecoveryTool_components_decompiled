using System;
using Interop.SirepClient;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x02000004 RID: 4
	[CLSCompliant(true)]
	public class DiscoveredDeviceInfo
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002288 File Offset: 0x00000488
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002290 File Offset: 0x00000490
		public DiscoveredDeviceInfo.ConnectionType Connection { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002299 File Offset: 0x00000499
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000022A1 File Offset: 0x000004A1
		public Guid UniqueId { get; internal set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022AA File Offset: 0x000004AA
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000022B2 File Offset: 0x000004B2
		public string Name { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000022BB File Offset: 0x000004BB
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000022C3 File Offset: 0x000004C3
		public string Location { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000022CC File Offset: 0x000004CC
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000022D4 File Offset: 0x000004D4
		public string Address { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000022DD File Offset: 0x000004DD
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000022E5 File Offset: 0x000004E5
		public string Architecture { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000022EE File Offset: 0x000004EE
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000022F6 File Offset: 0x000004F6
		public string OSVersion { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000022FF File Offset: 0x000004FF
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002307 File Offset: 0x00000507
		internal int SirepPort { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002310 File Offset: 0x00000510
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002318 File Offset: 0x00000518
		internal int SshPort { get; set; }

		// Token: 0x0600001F RID: 31 RVA: 0x00002324 File Offset: 0x00000524
		internal DiscoveredDeviceInfo(SirepDiscovery.DiscoveredEventArgs e)
		{
			this.Connection = (DiscoveredDeviceInfo.ConnectionType)e.ConnectionType;
			this.UniqueId = e.Guid;
			this.Name = e.Name;
			this.Location = e.Location;
			this.Address = e.Address;
			this.Architecture = e.Architecture;
			this.OSVersion = e.OSVersion;
			this.SirepPort = (int)e.SirepPort;
			this.SshPort = (int)e.SshPort;
		}

		// Token: 0x02000005 RID: 5
		public enum ConnectionType
		{
			// Token: 0x04000010 RID: 16
			Other,
			// Token: 0x04000011 RID: 17
			IpOverUsb,
			// Token: 0x04000012 RID: 18
			SirepBroadcast1,
			// Token: 0x04000013 RID: 19
			SirepBroadcast2,
			// Token: 0x04000014 RID: 20
			MDNS
		}
	}
}
