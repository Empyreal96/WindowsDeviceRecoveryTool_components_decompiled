using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.Core.Cache;

namespace Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp
{
	// Token: 0x02000005 RID: 5
	[Export(typeof(IMtpDeviceInfoProvider))]
	internal sealed class MtpDeviceInfoProviderCacheDecorator : IMtpDeviceInfoProvider, IDeviceInformationProvider<MtpInterfaceInfo>
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002088 File Offset: 0x00000288
		[ImportingConstructor]
		public MtpDeviceInfoProviderCacheDecorator(MtpDeviceInfoProvider deviceInfoProvider, IDeviceInformationCacheManager cacheManager)
		{
			this.deviceInfoProvider = deviceInfoProvider;
			this.cacheManager = cacheManager;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000209E File Offset: 0x0000029E
		public Task<MtpInterfaceInfo> ReadInformationAsync(string devicePath, CancellationToken token)
		{
			return this.deviceInfoProvider.ReadInformationAsync(devicePath, this.cacheManager, token);
		}

		// Token: 0x04000003 RID: 3
		private readonly MtpDeviceInfoProvider deviceInfoProvider;

		// Token: 0x04000004 RID: 4
		private readonly IDeviceInformationCacheManager cacheManager;
	}
}
