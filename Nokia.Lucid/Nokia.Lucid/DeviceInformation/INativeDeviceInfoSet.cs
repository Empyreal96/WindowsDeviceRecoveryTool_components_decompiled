using System;
using Nokia.Lucid.Interop.SafeHandles;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x02000013 RID: 19
	public interface INativeDeviceInfoSet
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000091 RID: 145
		SafeDeviceInfoSetHandle SafeDeviceInfoSetHandle { get; }
	}
}
