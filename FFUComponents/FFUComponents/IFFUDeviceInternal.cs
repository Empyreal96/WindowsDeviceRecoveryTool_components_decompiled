using System;

namespace FFUComponents
{
	// Token: 0x02000020 RID: 32
	internal interface IFFUDeviceInternal : IFFUDevice, IDisposable
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000BB RID: 187
		string UsbDevicePath { get; }

		// Token: 0x060000BC RID: 188
		bool NeedsTimer();
	}
}
