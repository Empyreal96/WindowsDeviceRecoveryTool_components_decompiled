using System;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x02000003 RID: 3
	[CLSCompliant(true)]
	public class DiscoveredEventArgs : EventArgs
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002268 File Offset: 0x00000468
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002270 File Offset: 0x00000470
		public DiscoveredDeviceInfo Info { get; internal set; }

		// Token: 0x0600000C RID: 12 RVA: 0x00002279 File Offset: 0x00000479
		internal DiscoveredEventArgs(DiscoveredDeviceInfo info)
		{
			this.Info = info;
		}
	}
}
