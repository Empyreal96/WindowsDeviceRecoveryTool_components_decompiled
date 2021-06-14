using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000005 RID: 5
	public class DetectionParameters
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002908 File Offset: 0x00000B08
		public DetectionParameters(PhoneTypes phoneTypes, PhoneModes phoneModes)
		{
			this.PhoneTypes = phoneTypes;
			this.PhoneModes = phoneModes;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002924 File Offset: 0x00000B24
		// (set) Token: 0x06000031 RID: 49 RVA: 0x0000293B File Offset: 0x00000B3B
		public PhoneTypes PhoneTypes { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002944 File Offset: 0x00000B44
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000295B File Offset: 0x00000B5B
		public PhoneModes PhoneModes { get; private set; }
	}
}
