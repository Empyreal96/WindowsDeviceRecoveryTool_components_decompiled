using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000025 RID: 37
	public class DeviceIdentifier
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00004488 File Offset: 0x00002688
		public DeviceIdentifier(string vid, string pid, params int[] mi)
		{
			this.Vid = vid;
			this.Pid = pid;
			this.Mi = mi;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000044AB File Offset: 0x000026AB
		public DeviceIdentifier(string vid, string pid)
		{
			this.Vid = vid;
			this.Pid = pid;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000044C6 File Offset: 0x000026C6
		public DeviceIdentifier(string vid)
		{
			this.Vid = vid;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000044DC File Offset: 0x000026DC
		// (set) Token: 0x06000133 RID: 307 RVA: 0x000044F3 File Offset: 0x000026F3
		public string Vid { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000044FC File Offset: 0x000026FC
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00004513 File Offset: 0x00002713
		public string Pid { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000451C File Offset: 0x0000271C
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00004533 File Offset: 0x00002733
		public int[] Mi { get; private set; }
	}
}
