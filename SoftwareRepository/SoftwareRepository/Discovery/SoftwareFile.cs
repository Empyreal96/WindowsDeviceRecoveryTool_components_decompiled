using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SoftwareRepository.Discovery
{
	// Token: 0x02000024 RID: 36
	[DataContract]
	public class SoftwareFile
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00004300 File Offset: 0x00002500
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00004308 File Offset: 0x00002508
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[DataMember(Name = "checksum")]
		public List<SoftwareFileChecksum> Checksum { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00004311 File Offset: 0x00002511
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00004319 File Offset: 0x00002519
		[DataMember(Name = "fileName")]
		public string FileName { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004322 File Offset: 0x00002522
		// (set) Token: 0x06000124 RID: 292 RVA: 0x0000432A File Offset: 0x0000252A
		[DataMember(Name = "fileSize")]
		public long FileSize { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004333 File Offset: 0x00002533
		// (set) Token: 0x06000126 RID: 294 RVA: 0x0000433B File Offset: 0x0000253B
		[DataMember(Name = "fileType")]
		public string FileType { get; set; }
	}
}
