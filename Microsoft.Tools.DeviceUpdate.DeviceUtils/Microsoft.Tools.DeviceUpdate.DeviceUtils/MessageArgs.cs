using System;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000022 RID: 34
	public class MessageArgs : EventArgs
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x0000F496 File Offset: 0x0000D696
		public MessageArgs(string message)
		{
			this.Message = message;
		}

		// Token: 0x04000318 RID: 792
		public readonly string Message;
	}
}
