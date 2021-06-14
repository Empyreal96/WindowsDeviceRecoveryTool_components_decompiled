using System;

namespace FFUComponents
{
	// Token: 0x02000038 RID: 56
	internal struct WinUsbPipeInformation
	{
		// Token: 0x040000BB RID: 187
		public WinUsbPipeType PipeType;

		// Token: 0x040000BC RID: 188
		public byte PipeId;

		// Token: 0x040000BD RID: 189
		public ushort MaximumPacketSize;

		// Token: 0x040000BE RID: 190
		public byte Interval;
	}
}
