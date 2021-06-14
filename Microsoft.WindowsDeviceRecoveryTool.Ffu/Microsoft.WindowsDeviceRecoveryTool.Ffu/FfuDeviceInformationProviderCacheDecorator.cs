using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.Core.Cache;

namespace Microsoft.WindowsDeviceRecoveryTool.Ffu
{
	// Token: 0x02000003 RID: 3
	[Export(typeof(IFfuDeviceInformationProvider))]
	internal sealed class FfuDeviceInformationProviderCacheDecorator : IFfuDeviceInformationProvider, IDeviceInformationProvider<FfuDeviceInformation>
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[ImportingConstructor]
		public FfuDeviceInformationProviderCacheDecorator(FfuDeviceInformationProvider informationProvider, IDeviceInformationCacheManager cacheManager)
		{
			this.informationProvider = informationProvider;
			this.cacheManager = cacheManager;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002066 File Offset: 0x00000266
		public Task<FfuDeviceInformation> ReadInformationAsync(string usbDeviceInterfaceDevicePath, CancellationToken cancellationToken)
		{
			return this.informationProvider.ReadInformationAsync(usbDeviceInterfaceDevicePath, this.cacheManager, cancellationToken);
		}

		// Token: 0x04000001 RID: 1
		private readonly FfuDeviceInformationProvider informationProvider;

		// Token: 0x04000002 RID: 2
		private readonly IDeviceInformationCacheManager cacheManager;
	}
}
