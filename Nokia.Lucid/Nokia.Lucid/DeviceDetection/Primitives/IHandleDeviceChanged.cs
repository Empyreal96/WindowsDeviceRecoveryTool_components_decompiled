using System;
using Nokia.Lucid.Interop.Win32Types;

namespace Nokia.Lucid.DeviceDetection.Primitives
{
	// Token: 0x02000003 RID: 3
	internal interface IHandleDeviceChanged
	{
		// Token: 0x06000006 RID: 6
		void HandleDeviceChanged(int changeType, ref DEV_BROADCAST_DEVICEINTERFACE data);
	}
}
