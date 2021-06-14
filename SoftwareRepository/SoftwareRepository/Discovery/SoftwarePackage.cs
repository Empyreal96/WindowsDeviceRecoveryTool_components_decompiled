using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SoftwareRepository.Discovery
{
	// Token: 0x02000026 RID: 38
	[DataContract]
	public class SoftwarePackage
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00004366 File Offset: 0x00002566
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000436E File Offset: 0x0000256E
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[DataMember(Name = "customerName")]
		public List<string> CustomerName { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00004377 File Offset: 0x00002577
		// (set) Token: 0x06000130 RID: 304 RVA: 0x0000437F File Offset: 0x0000257F
		[DataMember(Name = "extendedAttributes")]
		public ExtendedAttributes ExtendedAttributes { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00004388 File Offset: 0x00002588
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00004390 File Offset: 0x00002590
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[DataMember(Name = "files")]
		public List<SoftwareFile> Files { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00004399 File Offset: 0x00002599
		// (set) Token: 0x06000134 RID: 308 RVA: 0x000043A1 File Offset: 0x000025A1
		[DataMember(Name = "id")]
		public string Id { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000043AA File Offset: 0x000025AA
		// (set) Token: 0x06000136 RID: 310 RVA: 0x000043B2 File Offset: 0x000025B2
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[DataMember(Name = "manufacturerHardwareModel")]
		public List<string> ManufacturerHardwareModel { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000043BB File Offset: 0x000025BB
		// (set) Token: 0x06000138 RID: 312 RVA: 0x000043C3 File Offset: 0x000025C3
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[DataMember(Name = "manufacturerHardwareVariant")]
		public List<string> ManufacturerHardwareVariant { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000043CC File Offset: 0x000025CC
		// (set) Token: 0x0600013A RID: 314 RVA: 0x000043D4 File Offset: 0x000025D4
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[DataMember(Name = "manufacturerModelName")]
		public List<string> ManufacturerModelName { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600013B RID: 315 RVA: 0x000043DD File Offset: 0x000025DD
		// (set) Token: 0x0600013C RID: 316 RVA: 0x000043E5 File Offset: 0x000025E5
		[DataMember(Name = "manufacturerName")]
		public string ManufacturerName { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000043EE File Offset: 0x000025EE
		// (set) Token: 0x0600013E RID: 318 RVA: 0x000043F6 File Offset: 0x000025F6
		[DataMember(Name = "manufacturerPackageId")]
		public string ManufacturerPackageId { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000043FF File Offset: 0x000025FF
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00004407 File Offset: 0x00002607
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[DataMember(Name = "manufacturerPlatformId")]
		public List<string> ManufacturerPlatformId { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00004410 File Offset: 0x00002610
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00004418 File Offset: 0x00002618
		[DataMember(Name = "manufacturerProductLine")]
		public string ManufacturerProductLine { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00004421 File Offset: 0x00002621
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00004429 File Offset: 0x00002629
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[DataMember(Name = "manufacturerVariantName")]
		public List<string> ManufacturerVariantName { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00004432 File Offset: 0x00002632
		// (set) Token: 0x06000146 RID: 326 RVA: 0x0000443A File Offset: 0x0000263A
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[DataMember(Name = "operatorName")]
		public List<string> OperatorName { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00004443 File Offset: 0x00002643
		// (set) Token: 0x06000148 RID: 328 RVA: 0x0000444B File Offset: 0x0000264B
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[DataMember(Name = "packageClass")]
		public List<string> PackageClass { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00004454 File Offset: 0x00002654
		// (set) Token: 0x0600014A RID: 330 RVA: 0x0000445C File Offset: 0x0000265C
		[DataMember(Name = "packageDescription")]
		public string PackageDescription { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00004465 File Offset: 0x00002665
		// (set) Token: 0x0600014C RID: 332 RVA: 0x0000446D File Offset: 0x0000266D
		[DataMember(Name = "packageRevision")]
		public string PackageRevision { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00004476 File Offset: 0x00002676
		// (set) Token: 0x0600014E RID: 334 RVA: 0x0000447E File Offset: 0x0000267E
		[DataMember(Name = "packageState")]
		public string PackageState { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00004487 File Offset: 0x00002687
		// (set) Token: 0x06000150 RID: 336 RVA: 0x0000448F File Offset: 0x0000268F
		[DataMember(Name = "packageSubRevision")]
		public string PackageSubRevision { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00004498 File Offset: 0x00002698
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000044A0 File Offset: 0x000026A0
		[DataMember(Name = "packageSubtitle")]
		public string PackageSubtitle { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000044A9 File Offset: 0x000026A9
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000044B1 File Offset: 0x000026B1
		[DataMember(Name = "packageTitle")]
		public string PackageTitle { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000044BA File Offset: 0x000026BA
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000044C2 File Offset: 0x000026C2
		[DataMember(Name = "packageType")]
		public string PackageType { get; set; }
	}
}
