using System;
using System.Runtime.Serialization;

namespace SoftwareRepository.Discovery
{
	// Token: 0x02000025 RID: 37
	[DataContract]
	public class SoftwareFileChecksum
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00004344 File Offset: 0x00002544
		// (set) Token: 0x06000129 RID: 297 RVA: 0x0000434C File Offset: 0x0000254C
		[DataMember(Name = "type")]
		public string ChecksumType { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00004355 File Offset: 0x00002555
		// (set) Token: 0x0600012B RID: 299 RVA: 0x0000435D File Offset: 0x0000255D
		[DataMember(Name = "value")]
		public string Value { get; set; }
	}
}
