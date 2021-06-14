using System;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public class MtpException : DeviceException
	{
		// Token: 0x060000CD RID: 205 RVA: 0x0000424C File Offset: 0x0000244C
		public MtpException() : base("MTP communication error\n- This may be recoverable by simply reconnecting the device.\n- If it occurred during a multi-step operation, your device may be in an unknown state and need to be reflashed.")
		{
		}
	}
}
