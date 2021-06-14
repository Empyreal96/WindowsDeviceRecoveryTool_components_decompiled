using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage
{
	// Token: 0x0200000B RID: 11
	public class QueryParameters
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00002ED8 File Offset: 0x000010D8
		public QueryParameters()
		{
			this.ManufacturerProductLine = "WindowsPhone";
			this.PackageType = "VariantSoftware";
			this.PackageClass = "Public";
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002F08 File Offset: 0x00001108
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002F1F File Offset: 0x0000111F
		public string ManufacturerName { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002F28 File Offset: 0x00001128
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002F3F File Offset: 0x0000113F
		public string ManufacturerModelName { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002F48 File Offset: 0x00001148
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002F5F File Offset: 0x0000115F
		public string ManufacturerProductLine { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002F68 File Offset: 0x00001168
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002F7F File Offset: 0x0000117F
		public string PackageType { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002F88 File Offset: 0x00001188
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00002F9F File Offset: 0x0000119F
		public string PackageClass { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002FA8 File Offset: 0x000011A8
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00002FBF File Offset: 0x000011BF
		public string ManufacturerHardwareModel { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002FC8 File Offset: 0x000011C8
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002FDF File Offset: 0x000011DF
		public string ManufacturerHardwareVariant { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002FE8 File Offset: 0x000011E8
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00002FFF File Offset: 0x000011FF
		public Dictionary<string, string> ExtendedAttributes { get; set; }

		// Token: 0x06000082 RID: 130 RVA: 0x00003008 File Offset: 0x00001208
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ManufacturerName: {0}, ", this.ManufacturerName);
			stringBuilder.AppendFormat("ManufacturerModelName: {0}, ", this.ManufacturerModelName);
			stringBuilder.AppendFormat("ManufacturerProductLine: {0}, ", this.ManufacturerProductLine);
			stringBuilder.AppendFormat("PackageType: {0}, ", this.PackageType);
			stringBuilder.AppendFormat("PackageClass: {0}, ", this.PackageClass);
			stringBuilder.AppendFormat("ManufacturerHardwareModel: {0}, ", this.ManufacturerHardwareModel);
			stringBuilder.AppendFormat("ManufacturerHardwareVariant: {0}", this.ManufacturerHardwareVariant);
			return stringBuilder.ToString();
		}
	}
}
