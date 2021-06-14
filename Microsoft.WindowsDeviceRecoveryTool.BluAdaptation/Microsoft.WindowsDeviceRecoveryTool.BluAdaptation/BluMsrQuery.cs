using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.BluAdaptation
{
	// Token: 0x02000003 RID: 3
	[Export]
	internal static class BluMsrQuery
	{
		// Token: 0x04000005 RID: 5
		public static readonly QueryParameters WinHdLteX150QQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "Win HD LTE",
			ManufacturerHardwareModel = "Win HD LTE",
			ManufacturerHardwareVariant = "X150Q"
		};

		// Token: 0x04000006 RID: 6
		public static readonly QueryParameters WinHdLteX150EQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "X150E",
			ManufacturerHardwareModel = "WIN HD LTE",
			ManufacturerHardwareVariant = "QL850"
		};

		// Token: 0x04000007 RID: 7
		public static readonly QueryParameters WinJrLteX130QQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "X130Q",
			ManufacturerHardwareModel = "WIN JR LTE",
			ManufacturerHardwareVariant = "QL650"
		};

		// Token: 0x04000008 RID: 8
		public static readonly QueryParameters WinJrLteX130EQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "X130E",
			ManufacturerHardwareModel = "WIN JR LTE",
			ManufacturerHardwareVariant = "QL650"
		};

		// Token: 0x04000009 RID: 9
		public static readonly QueryParameters WinJRW410AQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "WIN JR W410a",
			ManufacturerHardwareModel = "W410a",
			ManufacturerHardwareVariant = "W410a"
		};

		// Token: 0x0400000A RID: 10
		public static readonly QueryParameters WinJRW410iQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "WIN JR W410i",
			ManufacturerHardwareModel = "W410i",
			ManufacturerHardwareVariant = "TBW5703_P3_001"
		};

		// Token: 0x0400000B RID: 11
		public static readonly QueryParameters WinJRW410uQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "WIN JR W410u",
			ManufacturerHardwareModel = "W410u",
			ManufacturerHardwareVariant = "TBW5720_P1_001"
		};

		// Token: 0x0400000C RID: 12
		public static readonly QueryParameters WinJRW410lQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "WIN JR W410l",
			ManufacturerHardwareModel = "W410l",
			ManufacturerHardwareVariant = "TBW5720_P1_002"
		};

		// Token: 0x0400000D RID: 13
		public static readonly QueryParameters WinHdW510UQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "WIN HD W510u",
			ManufacturerHardwareModel = "W510U",
			ManufacturerHardwareVariant = "W510U"
		};

		// Token: 0x0400000E RID: 14
		public static readonly QueryParameters WinHdW510lQuery = new QueryParameters
		{
			ManufacturerProductLine = "WindowsPhone",
			PackageType = "VariantSoftware",
			ManufacturerName = "BLU",
			ManufacturerModelName = "WIN HD W510l",
			ManufacturerHardwareModel = "W510l",
			ManufacturerHardwareVariant = "TBW5705_P3_001"
		};
	}
}
