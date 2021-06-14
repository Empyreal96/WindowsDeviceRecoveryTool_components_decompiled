using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.DiginnosAdaptation
{
	// Token: 0x02000003 RID: 3
	internal static class DiginnosMsrQuery
	{
		// Token: 0x04000002 RID: 2
		public static readonly QueryParameters DG_W10M = new QueryParameters
		{
			ManufacturerName = "Diginnos",
			ManufacturerHardwareModel = "THIRDWAVE",
			ManufacturerModelName = "DG-W10M",
			ManufacturerHardwareVariant = "VAR-GSM"
		};
	}
}
