using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Core
{
	// Token: 0x0200000A RID: 10
	public sealed class DeviceDetectionInformation
	{
		// Token: 0x06000018 RID: 24 RVA: 0x0000246A File Offset: 0x0000066A
		public DeviceDetectionInformation(VidPidPair vidPidPair, bool detectionDeferred)
		{
			this.VidPidPair = vidPidPair;
			this.DetectionDeferred = detectionDeferred;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002480 File Offset: 0x00000680
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002488 File Offset: 0x00000688
		public VidPidPair VidPidPair { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002491 File Offset: 0x00000691
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002499 File Offset: 0x00000699
		public bool DetectionDeferred { get; private set; }
	}
}
