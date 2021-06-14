using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.HPAdaptation
{
	// Token: 0x02000003 RID: 3
	internal static class HPMsrQuery
	{
		// Token: 0x04000004 RID: 4
		public static readonly QueryParameters Elitex3 = new QueryParameters
		{
			ManufacturerName = "HP",
			ManufacturerHardwareModel = "Elite x3",
			ManufacturerModelName = "Elite x3",
			ManufacturerHardwareVariant = "FalconBaseUnit"
		};

		// Token: 0x04000005 RID: 5
		public static readonly QueryParameters Elitex3_Telstra = new QueryParameters
		{
			ManufacturerName = "HP",
			ManufacturerHardwareModel = "Elite x3 Telstra",
			ManufacturerModelName = "Elite x3 Telstra",
			ManufacturerHardwareVariant = "FalconBaseUnit"
		};

		// Token: 0x04000006 RID: 6
		public static readonly QueryParameters Elitex3_Verizon = new QueryParameters
		{
			ManufacturerName = "HP",
			ManufacturerHardwareModel = "Elite x3 Verizon",
			ManufacturerModelName = "Elite x3 Verizon",
			ManufacturerHardwareVariant = "FalconBaseUnit"
		};
	}
}
