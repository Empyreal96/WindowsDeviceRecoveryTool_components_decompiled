using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000030 RID: 48
	internal struct WinUsbInterfaceDescriptor
	{
		// Token: 0x0400013A RID: 314
		public byte Length;

		// Token: 0x0400013B RID: 315
		public byte DescriptorType;

		// Token: 0x0400013C RID: 316
		public byte InterfaceNumber;

		// Token: 0x0400013D RID: 317
		public byte AlternateSetting;

		// Token: 0x0400013E RID: 318
		public byte NumEndpoints;

		// Token: 0x0400013F RID: 319
		public byte InterfaceClass;

		// Token: 0x04000140 RID: 320
		public byte InterfaceSubClass;

		// Token: 0x04000141 RID: 321
		public byte InterfaceProtocol;

		// Token: 0x04000142 RID: 322
		public byte Interface;
	}
}
