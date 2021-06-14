using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp
{
	// Token: 0x02000003 RID: 3
	public sealed class MtpInterfaceInfo
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002050 File Offset: 0x00000250
		internal MtpInterfaceInfo(string description, string manufacturer)
		{
			this.Description = description;
			this.Manufacturer = manufacturer;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002066 File Offset: 0x00000266
		// (set) Token: 0x06000007 RID: 7 RVA: 0x0000206E File Offset: 0x0000026E
		public string Description { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002077 File Offset: 0x00000277
		// (set) Token: 0x06000009 RID: 9 RVA: 0x0000207F File Offset: 0x0000027F
		public string Manufacturer { get; private set; }
	}
}
