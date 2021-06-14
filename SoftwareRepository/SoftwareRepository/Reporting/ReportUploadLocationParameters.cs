using System;
using System.Runtime.Serialization;

namespace SoftwareRepository.Reporting
{
	// Token: 0x0200001A RID: 26
	[DataContract]
	internal class ReportUploadLocationParameters
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000038A4 File Offset: 0x00001AA4
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000038AC File Offset: 0x00001AAC
		[DataMember(Name = "manufacturerName")]
		internal string ManufacturerName { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000038B5 File Offset: 0x00001AB5
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000038BD File Offset: 0x00001ABD
		[DataMember(Name = "manufacturerProductLine")]
		internal string ManufacturerProductLine { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000038C6 File Offset: 0x00001AC6
		// (set) Token: 0x060000AD RID: 173 RVA: 0x000038CE File Offset: 0x00001ACE
		[DataMember(Name = "reportClassification")]
		internal string ReportClassification { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000AE RID: 174 RVA: 0x000038D7 File Offset: 0x00001AD7
		// (set) Token: 0x060000AF RID: 175 RVA: 0x000038DF File Offset: 0x00001ADF
		[DataMember(Name = "fileName")]
		internal string FileName { get; set; }
	}
}
