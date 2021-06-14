using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SoftwareRepository.Reporting
{
	// Token: 0x0200001C RID: 28
	[DataContract]
	internal class DownloadReport
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003A8D File Offset: 0x00001C8D
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00003A95 File Offset: 0x00001C95
		[DataMember(Name = "api-version")]
		internal string ApiVersion { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00003A9E File Offset: 0x00001C9E
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00003AA6 File Offset: 0x00001CA6
		[DataMember(Name = "id")]
		internal string Id { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003AAF File Offset: 0x00001CAF
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00003AB7 File Offset: 0x00001CB7
		[DataMember(Name = "fileName")]
		internal string FileName { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003AC0 File Offset: 0x00001CC0
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00003AC8 File Offset: 0x00001CC8
		[DataMember(Name = "url")]
		internal List<string> Url { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003AD1 File Offset: 0x00001CD1
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00003AD9 File Offset: 0x00001CD9
		[DataMember(Name = "status")]
		internal int Status { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003AE2 File Offset: 0x00001CE2
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00003AEA File Offset: 0x00001CEA
		[DataMember(Name = "time")]
		internal long Time { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00003AF3 File Offset: 0x00001CF3
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00003AFB File Offset: 0x00001CFB
		[DataMember(Name = "size")]
		internal long Size { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00003B04 File Offset: 0x00001D04
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00003B0C File Offset: 0x00001D0C
		[DataMember(Name = "connections")]
		internal int Connections { get; set; }
	}
}
