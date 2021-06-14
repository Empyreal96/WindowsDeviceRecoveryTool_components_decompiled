using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.McjAdaptation
{
	// Token: 0x02000003 RID: 3
	[Export]
	internal static class McjMsrQuery
	{
		// Token: 0x04000003 RID: 3
		public static readonly QueryParameters MadosmaQ501Query = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "MCJ",
			ManufacturerHardwareModel = "MADOSMA Q501",
			PackageClass = "Public"
		};

		// Token: 0x04000004 RID: 4
		public static readonly QueryParameters MadosmaQ601Query = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "MCJ",
			ManufacturerModelName = "Q601",
			ManufacturerHardwareModel = "Q601",
			ManufacturerHardwareVariant = "Q601"
		};
	}
}
