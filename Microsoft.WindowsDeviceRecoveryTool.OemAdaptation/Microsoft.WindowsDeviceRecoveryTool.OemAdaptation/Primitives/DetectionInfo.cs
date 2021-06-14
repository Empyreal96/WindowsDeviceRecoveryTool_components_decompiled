using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Core;

namespace Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives
{
	// Token: 0x02000003 RID: 3
	public sealed class DetectionInfo
	{
		// Token: 0x06000002 RID: 2 RVA: 0x000020A0 File Offset: 0x000002A0
		public DetectionInfo(IEnumerable<DeviceDetectionInformation> deviceDetectionInformations)
		{
			if (deviceDetectionInformations == null)
			{
				throw new ArgumentNullException("deviceDetectionInformations");
			}
			DeviceDetectionInformation[] array = deviceDetectionInformations.ToArray<DeviceDetectionInformation>();
			if (array.Length == 0)
			{
				throw new ArgumentException("deviceReturnedValues should have at least one element");
			}
			this.DeviceDetectionInformations = array;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020DF File Offset: 0x000002DF
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020E7 File Offset: 0x000002E7
		public DeviceDetectionInformation[] DeviceDetectionInformations { get; private set; }
	}
}
