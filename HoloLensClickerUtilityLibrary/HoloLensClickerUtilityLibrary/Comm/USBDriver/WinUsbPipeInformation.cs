using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000031 RID: 49
	internal struct WinUsbPipeInformation
	{
		// Token: 0x04000143 RID: 323
		public WinUsbPipeType PipeType;

		// Token: 0x04000144 RID: 324
		public byte PipeId;

		// Token: 0x04000145 RID: 325
		public ushort MaximumPacketSize;

		// Token: 0x04000146 RID: 326
		public byte Interval;
	}
}
