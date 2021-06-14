using System;

namespace FFUComponents
{
	// Token: 0x02000037 RID: 55
	internal struct WinUsbInterfaceDescriptor
	{
		// Token: 0x040000B2 RID: 178
		public byte Length;

		// Token: 0x040000B3 RID: 179
		public byte DescriptorType;

		// Token: 0x040000B4 RID: 180
		public byte InterfaceNumber;

		// Token: 0x040000B5 RID: 181
		public byte AlternateSetting;

		// Token: 0x040000B6 RID: 182
		public byte NumEndpoints;

		// Token: 0x040000B7 RID: 183
		public byte InterfaceClass;

		// Token: 0x040000B8 RID: 184
		public byte InterfaceSubClass;

		// Token: 0x040000B9 RID: 185
		public byte InterfaceProtocol;

		// Token: 0x040000BA RID: 186
		public byte Interface;
	}
}
