using System;
using System.Threading;
using System.Threading.Tasks;
using Nokia.Lucid.DeviceInformation;

namespace Microsoft.WindowsDeviceRecoveryTool.Lucid
{
	// Token: 0x02000002 RID: 2
	public interface ILucidService
	{
		// Token: 0x06000001 RID: 1
		Task<string> TakeFirstDevicePathForInterfaceGuidAsync(string parentDevicePath, Guid interfaceGuid, CancellationToken cancellationToken);

		// Token: 0x06000002 RID: 2
		Task<string> TakeFirstDevicePathForInterfaceGuidAsync(string parentDevicePath, Guid interfaceGuid, int interfaceNumber, CancellationToken cancellationToken);

		// Token: 0x06000003 RID: 3
		Task<string> TakeFirstDevicePathForDeviceAndInterfaceGuidsAsync(string parentDevicePath, Guid deviceInterfaceGuid, Guid deviceSetupClassGuid, CancellationToken cancellationToken);

		// Token: 0x06000004 RID: 4
		DeviceInfo GetDeviceInfoForInterfaceGuid(string interfaceDevicePath, Guid interfaceGuid);
	}
}
