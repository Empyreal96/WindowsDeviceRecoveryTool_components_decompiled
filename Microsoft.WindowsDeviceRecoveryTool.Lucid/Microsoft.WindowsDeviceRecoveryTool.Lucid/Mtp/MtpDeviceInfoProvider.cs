using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Nokia.Lucid.DeviceInformation;

namespace Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp
{
	// Token: 0x02000006 RID: 6
	[Export]
	internal sealed class MtpDeviceInfoProvider : IMtpDeviceInfoProvider, IDeviceInformationProvider<MtpInterfaceInfo>
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000020B3 File Offset: 0x000002B3
		[ImportingConstructor]
		public MtpDeviceInfoProvider(ILucidService lucidService)
		{
			this.lucidService = lucidService;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002250 File Offset: 0x00000450
		public async Task<MtpInterfaceInfo> ReadInformationAsync(string devicePath, CancellationToken cancellationToken)
		{
			string mtpInterfaceDevicePath = await this.lucidService.TakeFirstDevicePathForDeviceAndInterfaceGuidsAsync(devicePath, WellKnownGuids.MtpInterfaceGuid, WellKnownGuids.WpdDeviceGuid, cancellationToken);
			DeviceInfo lucidInfo = this.lucidService.GetDeviceInfoForInterfaceGuid(mtpInterfaceDevicePath, WellKnownGuids.MtpInterfaceGuid);
			string manufacturer = lucidInfo.ReadManufacturer();
			string description = lucidInfo.ReadDescription();
			Tracer<MtpDeviceInfoProvider>.WriteInformation("Read MTP info for device path: {0}. Manufacturer: {1}, Description: {2}", new object[]
			{
				mtpInterfaceDevicePath,
				manufacturer,
				description
			});
			return new MtpInterfaceInfo(description, manufacturer);
		}

		// Token: 0x04000005 RID: 5
		private readonly ILucidService lucidService;
	}
}
