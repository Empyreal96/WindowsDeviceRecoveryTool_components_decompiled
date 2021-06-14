using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.FawkesAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.FawkesAdaptation
{
	// Token: 0x02000002 RID: 2
	[Export(typeof(IDeviceSupport))]
	internal class FawkesSupport : IDeviceSupport
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[ImportingConstructor]
		public FawkesSupport()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public Guid Id
		{
			get
			{
				return FawkesSupport.SupportGuid;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(FawkesSupport.FawkesVidPid, false)
			};
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002144 File Offset: 0x00000344
		public async Task UpdateDeviceDetectionDataAsync(DeviceDetectionData detectionData, CancellationToken cancellationToken)
		{
			if (detectionData.IsDeviceSupported)
			{
				throw new InvalidOperationException("Device is already supported.");
			}
			VidPidPair vidPidPair = detectionData.VidPidPair;
			if (!(vidPidPair != FawkesSupport.FawkesVidPid))
			{
				detectionData.IsDeviceSupported = true;
				detectionData.DeviceBitmapBytes = Resources.FawkesTile.ToBytes();
				detectionData.DeviceSalesName = Resources.DeviceSalesName;
			}
		}

		// Token: 0x04000001 RID: 1
		private static readonly VidPidPair FawkesVidPid = new VidPidPair("045E", "0654");

		// Token: 0x04000002 RID: 2
		private static readonly Guid SupportGuid = new Guid("58AE8994-C31B-49D9-B23C-F7FAB49ADB6E");
	}
}
