using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.AlcatelAdaptation
{
	// Token: 0x02000003 RID: 3
	internal static class AlcatelMsrQuery
	{
		// Token: 0x04000004 RID: 4
		public static readonly QueryParameters IDOL4S = new QueryParameters
		{
			ManufacturerName = "AlcatelOne",
			ManufacturerHardwareModel = "6071W",
			ManufacturerModelName = "IDOL 4S with Windows 10",
			ManufacturerHardwareVariant = "VAR-GSM"
		};

		// Token: 0x04000005 RID: 5
		public static readonly QueryParameters IDOL4S_NA = new QueryParameters
		{
			ManufacturerName = "AlcatelOne",
			ManufacturerHardwareModel = "6071W",
			ManufacturerModelName = "IDOL 4S with Windows 10 NA",
			ManufacturerHardwareVariant = "VAR-GSM"
		};

		// Token: 0x04000006 RID: 6
		public static readonly QueryParameters IDO4S_PRO = new QueryParameters
		{
			ManufacturerName = "AlcatelOne",
			ManufacturerHardwareModel = "6077X",
			ManufacturerModelName = "IDOL 4 PRO",
			ManufacturerHardwareVariant = "VAR-GSM"
		};

		// Token: 0x04000007 RID: 7
		public static readonly QueryParameters FierceXLPara = new QueryParameters
		{
			ManufacturerName = "AlcatelOne",
			ManufacturerHardwareModel = "5055W",
			ManufacturerModelName = "FierceXL"
		};
	}
}
