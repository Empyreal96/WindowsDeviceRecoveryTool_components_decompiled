using System;
using System.Runtime.Serialization;

namespace SoftwareRepository.Discovery
{
	// Token: 0x02000020 RID: 32
	[DataContract]
	public class DiscoveryQueryParameters
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000401B File Offset: 0x0000221B
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00004023 File Offset: 0x00002223
		[DataMember(Name = "customerName", EmitDefaultValue = false)]
		public string CustomerName { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000402C File Offset: 0x0000222C
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004034 File Offset: 0x00002234
		[DataMember(Name = "extendedAttributes", EmitDefaultValue = false)]
		public ExtendedAttributes ExtendedAttributes { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000EC RID: 236 RVA: 0x0000403D File Offset: 0x0000223D
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00004045 File Offset: 0x00002245
		[DataMember(Name = "manufacturerHardwareModel", EmitDefaultValue = false)]
		public string ManufacturerHardwareModel { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000EE RID: 238 RVA: 0x0000404E File Offset: 0x0000224E
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00004056 File Offset: 0x00002256
		[DataMember(Name = "manufacturerHardwareVariant", EmitDefaultValue = false)]
		public string ManufacturerHardwareVariant { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000405F File Offset: 0x0000225F
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004067 File Offset: 0x00002267
		[DataMember(Name = "manufacturerModelName", EmitDefaultValue = false)]
		public string ManufacturerModelName { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004070 File Offset: 0x00002270
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004078 File Offset: 0x00002278
		[DataMember(Name = "manufacturerName")]
		public string ManufacturerName { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004081 File Offset: 0x00002281
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004089 File Offset: 0x00002289
		[DataMember(Name = "manufacturerPackageId", EmitDefaultValue = false)]
		public string ManufacturerPackageId { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004092 File Offset: 0x00002292
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x0000409A File Offset: 0x0000229A
		[DataMember(Name = "manufacturerPlatformId", EmitDefaultValue = false)]
		public string ManufacturerPlatformId { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000040A3 File Offset: 0x000022A3
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000040AB File Offset: 0x000022AB
		[DataMember(Name = "manufacturerProductLine")]
		public string ManufacturerProductLine { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000040B4 File Offset: 0x000022B4
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000040BC File Offset: 0x000022BC
		[DataMember(Name = "manufacturerVariantName", EmitDefaultValue = false)]
		public string ManufacturerVariantName { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000040C5 File Offset: 0x000022C5
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000040CD File Offset: 0x000022CD
		[DataMember(Name = "operatorName", EmitDefaultValue = false)]
		public string OperatorName { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000040D6 File Offset: 0x000022D6
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000040DE File Offset: 0x000022DE
		[DataMember(Name = "packageClass")]
		public string PackageClass { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000040E7 File Offset: 0x000022E7
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000040EF File Offset: 0x000022EF
		[DataMember(Name = "packageRevision", EmitDefaultValue = false)]
		public string PackageRevision { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000040F8 File Offset: 0x000022F8
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00004100 File Offset: 0x00002300
		[DataMember(Name = "packageState", EmitDefaultValue = false)]
		public string PackageState { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004109 File Offset: 0x00002309
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00004111 File Offset: 0x00002311
		[DataMember(Name = "packageSubRevision", EmitDefaultValue = false)]
		public string PackageSubRevision { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000411A File Offset: 0x0000231A
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00004122 File Offset: 0x00002322
		[DataMember(Name = "packageSubtitle", EmitDefaultValue = false)]
		public string PackageSubtitle { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000108 RID: 264 RVA: 0x0000412B File Offset: 0x0000232B
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00004133 File Offset: 0x00002333
		[DataMember(Name = "packageTitle", EmitDefaultValue = false)]
		public string PackageTitle { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600010A RID: 266 RVA: 0x0000413C File Offset: 0x0000233C
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00004144 File Offset: 0x00002344
		[DataMember(Name = "packageType")]
		public string PackageType { get; set; }
	}
}
